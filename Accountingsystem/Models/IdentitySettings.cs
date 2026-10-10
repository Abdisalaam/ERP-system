using System;

namespace Accountingsystem.Models
{
    public class IdentitySettings
    {
        public IdentitySettings()
        {
            MinimumPasswordLength = 6;
            RequireNonLetterOrDigit = true;
            RequireDigit = true;
            RequireLowercase = true;
            RequireUppercase = true;
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;
            AllowOnlyAlphanumericUserNames = false;
            RequireUniqueEmail = true;
        }

        public bool AllowOnlyAlphanumericUserNames { get; set; }
        public bool RequireUniqueEmail { get; set; }
        public int MinimumPasswordLength { get; set; }
        public bool RequireNonLetterOrDigit { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireUppercase { get; set; }
        public bool UserLockoutEnabledByDefault { get; set; }
        public TimeSpan DefaultAccountLockoutTimeSpan { get; set; }
        public int MaxFailedAccessAttemptsBeforeLockout { get; set; }
    }
}