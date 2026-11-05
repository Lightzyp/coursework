using System;

namespace Project_4.Models
{
    /// <summary>
    /// Represents one saved password entry for a specific site.
    /// </summary>
    public class PasswordEntry
    {
        /// <summary>
        /// The name or URL of the site (used as a unique key).
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// The username or email for this site.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password for this site.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// A masked version of the password for display purposes.
        /// </summary>
        public string MaskedPassword => new string('*', Password?.Length ?? 0);

        public PasswordEntry()
        {
            Site = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
        }

        public PasswordEntry(string? site, string? username, string? password)
        {
            Site = site ?? string.Empty;
            Username = username ?? string.Empty;
            Password = password ?? string.Empty;
        }

        public override string ToString()
        {
            return $"{Site} ({Username})";
        }
    }
}
