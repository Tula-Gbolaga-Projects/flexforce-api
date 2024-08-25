using System;
using System.Collections.Generic;

namespace agency_portal_api.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public string RoleName { get; set; }
    }

    public class ApiTokenResponse
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
