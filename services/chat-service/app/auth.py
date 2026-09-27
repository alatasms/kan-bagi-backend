"""Keycloak access token validation (see docs/auth.md). The caller is always taken from the token."""
import os
from dataclasses import dataclass

import jwt
from fastapi import HTTPException, Request, WebSocket, status
from starlette.concurrency import run_in_threadpool

ISSUER = os.getenv("AUTH_ISSUER", "")
JWKS_URL = os.getenv("AUTH_JWKS_URL", "")
AUDIENCE = os.getenv("AUTH_AUDIENCE", "bloodapp-api")
ADMIN_ROLE = "admin"

_jwks_client = jwt.PyJWKClient(JWKS_URL, cache_keys=True) if JWKS_URL else None


@dataclass(frozen=True)
class CurrentUser:
    user_id: str
    roles: frozenset

    @property
    def is_admin(self) -> bool:
        return ADMIN_ROLE in self.roles


def decode_token(token: str) -> CurrentUser:
    """Checks signature, issuer, audience and expiry. Raises jwt.PyJWTError when the token is not valid."""
    if _jwks_client is None:
        raise jwt.InvalidTokenError("AUTH_JWKS_URL is not configured")
    signing_key = _jwks_client.get_signing_key_from_jwt(token).key
    claims = jwt.decode(token, signing_key, algorithms=["RS256"], audience=AUDIENCE, issuer=ISSUER,
                        options={"require": ["exp", "sub"]})
    return CurrentUser(user_id=claims["sub"], roles=frozenset(claims.get("roles", [])))


def _bearer_token(authorization) -> str:
    if authorization and authorization.lower().startswith("bearer "):
        return authorization[7:].strip()
    return ""


async def _authenticate(token: str):
    if not token:
        return None
    try:
        # Fetching signing keys is blocking I/O; keys are cached after the first call.
        return await run_in_threadpool(decode_token, token)
    except (jwt.PyJWTError, KeyError):
        return None


async def current_user(request: Request) -> CurrentUser:
    """FastAPI dependency for HTTP endpoints."""
    user = await _authenticate(_bearer_token(request.headers.get("authorization")))
    if user is None:
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Invalid or missing access token.")
    return user


async def websocket_user(websocket: WebSocket):
    """
    The token may come in the Authorization header (mobile) or, because browsers cannot set headers on
    WebSocket requests, in the `access_token` query parameter.
    """
    token = _bearer_token(websocket.headers.get("authorization")) or websocket.query_params.get("access_token", "")
    return await _authenticate(token)
