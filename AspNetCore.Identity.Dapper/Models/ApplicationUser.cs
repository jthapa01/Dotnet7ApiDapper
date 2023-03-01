﻿using System.Security.Claims;
using AspNetCore.Identity.Dapper.Stores;
using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Identity.Dapper.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int UserType { get; set; } 
        public bool IsActive { get; set; } = true;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; } = DateTime.UtcNow;

        internal List<Claim>? Claims { get; set; }
        internal List<UserRole>? Roles { get; set; }
        internal List<UserLoginInfo>? Logins { get; set; }
        internal List<UserToken>? Tokens { get; set; }
    }
}
