namespace APIGateway.Auth
{
    public static class IdentityHeaders
    {
        /// <summary>
        /// Identity headers a client could forge. The gateway removes them from every incoming request.
        /// </summary>
        public static readonly string[] Spoofable = ["sub", "email", "emails", "user_id", "userId", "X-User-Id"];
    }
}
