#!/usr/bin/env bash
# End-to-end check against a running stack (docker compose up). Needs bash and curl.
#   cd deploy && ./smoke-test.sh
# Creates two throwaway Keycloak users, completes their profiles, opens a post as one of them and
# expects the other to receive the notification email in Mailpit. The users are deleted at the end.
set -euo pipefail

GATEWAY=${GATEWAY:-http://localhost:8000}
KEYCLOAK=${KEYCLOAK:-http://localhost:8080}
MAILPIT=${MAILPIT:-http://localhost:8025}
KEYCLOAK_ADMIN_USER=${KEYCLOAK_ADMIN_USER:-admin}
KEYCLOAK_ADMIN_PASSWORD=${KEYCLOAK_ADMIN_PASSWORD:-admin_dev}
# A user with the hospital_staff role; the development realm ships one.
STAFF_USER=${STAFF_USER:-staff@bloodapp.local}
STAFF_PASSWORD=${STAFF_PASSWORD:-staff123}
RUN=$(date +%s)
OWNER="owner-$RUN@smoke.local"
DONOR="donor-$RUN@smoke.local"
PASSWORD="smoke-$RUN"

failures=0
pass() { printf '  \033[32mok\033[0m   %s\n' "$1"; }
fail() { printf '  \033[31mFAIL\033[0m %s\n' "$1"; failures=$((failures + 1)); }
json_field() { grep -oE "\"$1\":\"?[^\",}]*" | head -1 | sed -E "s/^\"$1\":\"?//"; } # first occurrence
token_sub() { # the "sub" claim of a JWT
  local payload
  payload=$(echo "$1" | cut -d. -f2 | tr '_-' '/+')
  while [ $(( ${#payload} % 4 )) -ne 0 ]; do payload="$payload="; done
  echo "$payload" | base64 -d 2>/dev/null | json_field sub
}

wait_for() { # url, name
  for _ in $(seq 1 60); do
    curl -fs -o /dev/null "$1" && return 0
    sleep 3
  done
  echo "$2 did not become ready at $1" >&2
  exit 1
}

admin_token() {
  curl -fs "$KEYCLOAK/realms/master/protocol/openid-connect/token" \
    -d grant_type=password -d client_id=admin-cli \
    -d username="$KEYCLOAK_ADMIN_USER" -d password="$KEYCLOAK_ADMIN_PASSWORD" | json_field access_token
}

user_token() { # email
  curl -fs "$KEYCLOAK/realms/bloodapp/protocol/openid-connect/token" \
    -d grant_type=password -d client_id=bloodapp-mobile -d username="$1" -d password="$PASSWORD" | json_field access_token
}

create_user() { # email
  curl -fs -o /dev/null -H "Authorization: Bearer $ADMIN" -H "Content-Type: application/json" \
    -d "{\"username\":\"$1\",\"email\":\"$1\",\"emailVerified\":true,\"enabled\":true,\"firstName\":\"Smoke\",\"lastName\":\"Test\",\"credentials\":[{\"type\":\"password\",\"value\":\"$PASSWORD\",\"temporary\":false}]}" \
    "$KEYCLOAK/admin/realms/bloodapp/users"
}

delete_user() { # email
  local id
  id=$(curl -fs -H "Authorization: Bearer $(admin_token)" "$KEYCLOAK/admin/realms/bloodapp/users?username=$1&exact=true" | json_field id)
  [ -n "$id" ] && curl -fs -o /dev/null -X DELETE -H "Authorization: Bearer $(admin_token)" "$KEYCLOAK/admin/realms/bloodapp/users/$id" || true
}

status() { # method path token [body] -> HTTP status
  local args=(-s -o /dev/null -w '%{http_code}' -X "$1")
  [ -n "${3:-}" ] && args+=(-H "Authorization: Bearer $3")
  [ -n "${4:-}" ] && args+=(-H "Content-Type: application/json" -d "$4")
  curl "${args[@]}" "$GATEWAY$2"
}

expect() { # description expected actual
  if [ "$2" = "$3" ]; then pass "$1"; else fail "$1 (expected $2, got $3)"; fi
}

cleanup() { delete_user "$OWNER"; delete_user "$DONOR"; }
trap cleanup EXIT

echo "Waiting for the stack..."
wait_for "$KEYCLOAK/realms/bloodapp/.well-known/openid-configuration" Keycloak
wait_for "$GATEWAY/health/live" Gateway
wait_for "$MAILPIT/api/v1/messages" Mailpit

ADMIN=$(admin_token)
create_user "$OWNER"
create_user "$DONOR"
OWNER_TOKEN=$(user_token "$OWNER")
DONOR_TOKEN=$(user_token "$DONOR")

echo "Authentication"
expect "request without token is rejected" 401 "$(status GET /profile/get-session-info)"
expect "forged identity header is rejected" 401 "$(curl -s -o /dev/null -w '%{http_code}' -H 'sub: 00000000-0000-0000-0000-000000000001' "$GATEWAY/profile/get-session-info")"

echo "Profiles"
profile() { echo "{\"name\":\"$1\",\"surname\":\"Test\",\"phoneNumber\":\"05551112233\",\"birthDate\":\"1990-01-01\",\"bloodType\":\"$2\",\"gender\":\"Male\"}"; }
expect "owner completes profile" 200 "$(status POST /profile/complete-profile "$OWNER_TOKEN" "$(profile Owner A_Positive)")"
expect "donor (O-) completes profile" 200 "$(status POST /profile/complete-profile "$DONOR_TOKEN" "$(profile Donor O_Negative)")"
expect "completing a profile twice is a conflict" 409 "$(status POST /profile/complete-profile "$OWNER_TOKEN" "$(profile Owner A_Positive)")"

echo "Posts and notifications"
sleep 3 # profile events reach post-service and notification-service
POST='{"patientFullName":"Smoke Patient","patientAge":40,"title":"Smoke test","description":"Smoke test post, safe to ignore.","phoneNumbers":["05551112233"],"bloodType":"A_Positive","hospitalId":25}'
expect "owner creates a post" 200 "$(status POST /post/create-post "$OWNER_TOKEN" "$POST")"
expect "a second active post is a conflict" 409 "$(status POST /post/create-post "$OWNER_TOKEN" "$POST")"
expect "page size above the limit is rejected" 400 "$(status GET '/post/get-all-posts?Paging=1000' "$DONOR_TOKEN")"

received=""
for _ in $(seq 1 20); do
  received=$(curl -fs "$MAILPIT/api/v1/search?query=to:$DONOR%20subject:Kan" | json_field messages_count)
  [ "${received:-0}" -gt 0 ] && break
  sleep 2
done
expect "compatible donor is emailed about the post" 1 "${received:-0}"
expect "owner is not emailed about their own post" 0 "$(curl -fs "$MAILPIT/api/v1/search?query=to:$OWNER%20subject:Kan%20%C4%B0htiyac%C4%B1%20Talebi" | json_field messages_count)"

echo "Donation matching and QR verification"
OWNER_ID=$(token_sub "$OWNER_TOKEN")
DONOR_ID=$(token_sub "$DONOR_TOKEN")
STAFF_TOKEN=$(curl -fs "$KEYCLOAK/realms/bloodapp/protocol/openid-connect/token" \
  -d grant_type=password -d client_id=bloodapp-mobile -d username="$STAFF_USER" -d password="$STAFF_PASSWORD" | json_field access_token)
POST_ID=$(curl -fs -H "Authorization: Bearer $OWNER_TOKEN" "$GATEWAY/post/get-user-active-post" | json_field id)
MATCHING_ID=$(curl -s -H "Authorization: Bearer $DONOR_TOKEN" -H "Content-Type: application/json" \
  -d "{\"postId\":\"$POST_ID\",\"ownerId\":\"$OWNER_ID\"}" "$GATEWAY/matching/create" | json_field matchingId)
if [ -n "$MATCHING_ID" ]; then pass "donor responds to the post and gets a QR code"; else fail "donor responds to the post and gets a QR code"; fi
expect "post owner can view the matching" 200 "$(status GET "/matching/$MATCHING_ID" "$OWNER_TOKEN")"
expect "donor cannot verify their own donation" 403 "$(status POST /matching/validate "$DONOR_TOKEN" "{\"matchingId\":$MATCHING_ID}")"
expect "hospital staff verify the donation" 200 "$(status POST /matching/validate "$STAFF_TOKEN" "{\"matchingId\":$MATCHING_ID}")"
expect "a donation is verified only once" 409 "$(status POST /matching/validate "$STAFF_TOKEN" "{\"matchingId\":$MATCHING_ID}")"
expect "a user cannot list someone else's matchings" 403 "$(status GET "/matching/user/$DONOR_ID" "$OWNER_TOKEN")"
if curl -fs -H "Authorization: Bearer $DONOR_TOKEN" "$GATEWAY/aggregate/me/posts" | grep -q "\"isVerified\":true"; then
  pass "donor's posts show the verified donation (gateway aggregate)"
else
  fail "donor's posts show the verified donation (gateway aggregate)"
fi

echo "Eligibility form"
expect "donor fills in the form" 200 "$(status POST /evaluation-form/createForm "$DONOR_TOKEN" '{"onamFormuOkundumu":true,"saglikliMi":true}')"
expect "hospital staff read the donor's form" 200 "$(status GET "/evaluation-form/getFormById?donorId=$DONOR_ID" "$STAFF_TOKEN")"
expect "another user cannot read the donor's form" 403 "$(status GET "/evaluation-form/getFormById?donorId=$DONOR_ID" "$OWNER_TOKEN")"

echo "Chat"
ROOM_ID=$(curl -s -H "Authorization: Bearer $DONOR_TOKEN" -H "Content-Type: application/json" \
  -d "{\"user1_id\":\"$DONOR_ID\",\"user2_id\":\"$OWNER_ID\",\"user1_fullname\":\"Donor\",\"user2_fullname\":\"Owner\"}" "$GATEWAY/chat/create-room" | json_field room_id)
if [ -n "$ROOM_ID" ]; then pass "donor opens a chat with the post owner"; else fail "donor opens a chat with the post owner"; fi
expect "participant reads the room's messages" 200 "$(status GET "/chat/get-messages-by-room-id?room_id=$ROOM_ID" "$OWNER_TOKEN")"
expect "non-participant cannot read the room's messages" 403 "$(status GET "/chat/get-messages-by-room-id?room_id=$ROOM_ID" "$STAFF_TOKEN")"
expect "cannot open a room between two other users" 403 "$(status POST /chat/create-room "$STAFF_TOKEN" "{\"user1_id\":\"$DONOR_ID\",\"user2_id\":\"$OWNER_ID\",\"user1_fullname\":\"a\",\"user2_fullname\":\"b\"}")"

echo
if [ "$failures" -eq 0 ]; then echo "All checks passed."; else echo "$failures check(s) failed."; exit 1; fi
