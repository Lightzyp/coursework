using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Project_4.Models;

namespace Project_4.Services
{
    public class PasswordManagerService
    {
        private readonly string _filePath;
        private readonly Dictionary<string, PasswordEntry> _entries = new();

        // Master password management
        private byte[]? _encryptionKey;
        private bool _isUnlocked = false;

        public string? LastError { get; private set; }
        public string? LastBackupPath { get; private set; }
        public string FileLocation => _filePath;
        public bool IsUnlocked => _isUnlocked;

        // Auto-lock timer
        private System.Timers.Timer? _autoLockTimer;
        private const int AUTO_LOCK_MINUTES = 5;

        public PasswordManagerService()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folder = Path.Combine(appData, "PasswordManager");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            _filePath = Path.Combine(folder, "passwords.enc"); // Changed to .enc

            SetupAutoLock();
        }

        #region Master Password & Encryption

        /// <summary>
        /// Creates a new master password (first time setup)
        /// </summary>
        public bool CreateMasterPassword(string masterPassword)
        {
            try
            {
                LastError = null;

                if (string.IsNullOrWhiteSpace(masterPassword))
                {
                    LastError = "Master password cannot be empty.";
                    return false;
                }

                if (masterPassword.Length < 8)
                {
                    LastError = "Master password must be at least 8 characters.";
                    return false;
                }

                // Generate salt and hash
                byte[] salt = GenerateSalt();
                byte[] passwordHash = HashPassword(masterPassword, salt);

                // Save salt and hash
                string hashFile = Path.Combine(Path.GetDirectoryName(_filePath)!, "master.hash");
                var hashData = new
                {
                    Salt = Convert.ToBase64String(salt),
                    Hash = Convert.ToBase64String(passwordHash)
                };

                File.WriteAllText(hashFile, JsonSerializer.Serialize(hashData));

                // Derive encryption key and unlock
                _encryptionKey = DeriveKey(masterPassword, salt);
                _isUnlocked = true;
                _entries.Clear();
                Save();

                if (!string.IsNullOrEmpty(LastError))
                {
                    Lock();
                    return false;
                }

                ResetAutoLockTimer();

                return true;
            }
            catch (Exception ex)
            {
                LastError = $"Failed to create master password: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Verifies master password and unlocks the password manager
        /// </summary>
        public bool UnlockWithMasterPassword(string masterPassword)
        {
            try
            {
                LastError = null;
                string hashFile = Path.Combine(Path.GetDirectoryName(_filePath)!, "master.hash");

                if (!File.Exists(hashFile))
                {
                    LastError = "No master password set. Please create one first.";
                    return false;
                }

                // Load saved hash and salt
                var hashData = JsonSerializer.Deserialize<Dictionary<string, string>>(
                    File.ReadAllText(hashFile)
                )!;

                byte[] salt = Convert.FromBase64String(hashData["Salt"]);
                byte[] savedHash = Convert.FromBase64String(hashData["Hash"]);

                // Verify password
                byte[] enteredHash = HashPassword(masterPassword, salt);

                if (!CryptographicOperations.FixedTimeEquals(savedHash, enteredHash))
                {
                    LastError = "Incorrect master password.";
                    return false;
                }

                // Derive encryption key and unlock
                _encryptionKey = DeriveKey(masterPassword, salt);
                _isUnlocked = true;

                // Load encrypted passwords
                if (!Load())
                {
                    Lock();
                    return false;
                }

                ResetAutoLockTimer();
                return true;
            }
            catch (Exception ex)
            {
                LastError = $"Failed to unlock: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Checks if master password has been set up
        /// </summary>
        public bool IsMasterPasswordSet()
        {
            string hashFile = Path.Combine(Path.GetDirectoryName(_filePath)!, "master.hash");
            return File.Exists(hashFile);
        }

        /// <summary>
        /// Locks the password manager and clears sensitive data from memory
        /// </summary>
        public void Lock()
        {
            _isUnlocked = false;
            LastBackupPath = null;
            _entries.Clear();

            // Clear encryption key from memory
            if (_encryptionKey != null)
            {
                Array.Clear(_encryptionKey, 0, _encryptionKey.Length);
                _encryptionKey = null;
            }

            _autoLockTimer?.Stop();
        }

        /// <summary>
        /// Hashes password using PBKDF2 with 100,000 iterations
        /// </summary>
        private byte[] HashPassword(string password, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                100000,
                HashAlgorithmName.SHA256
            );
            return pbkdf2.GetBytes(32);
        }

        /// <summary>
        /// Derives encryption key from master password
        /// </summary>
        private byte[] DeriveKey(string password, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                100000,
                HashAlgorithmName.SHA256
            );
            return pbkdf2.GetBytes(32); // 256-bit key for AES-256
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        #endregion

        #region Auto-Lock Timer

        private void SetupAutoLock()
        {
            _autoLockTimer = new System.Timers.Timer(AUTO_LOCK_MINUTES * 60 * 1000);
            _autoLockTimer.Elapsed += (s, e) => Lock();
            _autoLockTimer.AutoReset = false;
        }

        private void ResetAutoLockTimer()
        {
            _autoLockTimer?.Stop();
            _autoLockTimer?.Start();
        }

        #endregion

        #region Password Entry Management

        public List<PasswordEntry> GetAll()
        {
            if (!_isUnlocked)
            {
                LastError = "Password manager is locked.";
                return new List<PasswordEntry>();
            }

            LastError = null;
            ResetAutoLockTimer();
            return _entries.Values.ToList();
        }

        public void AddOrUpdate(PasswordEntry entry)
        {
            LastError = null;

            if (!_isUnlocked)
            {
                LastError = "Password manager is locked.";
                return;
            }

            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(entry.Site))
                {
                    LastError = "Site name cannot be empty.";
                    return;
                }

                _entries[entry.Site] = entry;
                Save();
                ResetAutoLockTimer();
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
        }

        public void Update(PasswordEntry updatedEntry)
        {
            LastError = null;

            if (!_isUnlocked)
            {
                LastError = "Password manager is locked.";
                return;
            }

            try
            {
                if (_entries.ContainsKey(updatedEntry.Site))
                {
                    _entries[updatedEntry.Site] = updatedEntry;
                    Save();
                    ResetAutoLockTimer();
                }
                else
                {
                    LastError = "Entry not found to update.";
                }
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
        }

        public void Remove(string site)
        {
            LastError = null;

            if (!_isUnlocked)
            {
                LastError = "Password manager is locked.";
                return;
            }

            if (_entries.Remove(site))
            {
                Save();
                ResetAutoLockTimer();
            }
        }

        public PasswordImportResult ImportEntries(IEnumerable<PasswordEntry> entries)
        {
            LastError = null;

            if (!_isUnlocked)
            {
                LastError = "Password manager is locked.";
                return new PasswordImportResult(0, 0, 0);
            }

            var imported = 0;
            var skipped = 0;
            var renamed = 0;

            foreach (var entry in entries)
            {
                if (string.IsNullOrWhiteSpace(entry.Site) || string.IsNullOrEmpty(entry.Password))
                {
                    skipped++;
                    continue;
                }

                var site = CreateUniqueSiteLabel(entry.Site.Trim(), entry.Username.Trim());
                if (!site.Equals(entry.Site.Trim(), StringComparison.Ordinal))
                    renamed++;

                _entries[site] = new PasswordEntry(site, entry.Username.Trim(), entry.Password, entry.Notes);
                imported++;
            }

            if (imported > 0)
            {
                Save();
                ResetAutoLockTimer();
            }

            return new PasswordImportResult(imported, skipped, renamed);
        }

        private string CreateUniqueSiteLabel(string site, string username)
        {
            if (!_entries.ContainsKey(site))
                return site;

            var suffix = string.IsNullOrWhiteSpace(username) ? "imported" : username;
            var candidate = $"{site} ({suffix})";
            var counter = 2;

            while (_entries.ContainsKey(candidate))
            {
                candidate = $"{site} ({suffix} {counter})";
                counter++;
            }

            return candidate;
        }

        #endregion

        #region Encryption & File Operations

        /// <summary>
        /// Encrypts data using AES-256-GCM
        /// </summary>
        private byte[] Encrypt(string plaintext)
        {
            if (_encryptionKey == null)
                throw new InvalidOperationException("Encryption key not available.");

            const int nonceSize = 12; // 96 bits recommended for GCM
            const int tagSize = 16;   // 128 bits

            using var aes = new AesGcm(_encryptionKey, tagSize);

            byte[] nonce = new byte[nonceSize];
            byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] ciphertext = new byte[plainBytes.Length];
            byte[] tag = new byte[tagSize];

            RandomNumberGenerator.Fill(nonce);
            aes.Encrypt(nonce, plainBytes, ciphertext, tag);

            // Combine: nonce + tag + ciphertext
            byte[] result = new byte[nonce.Length + tag.Length + ciphertext.Length];
            Buffer.BlockCopy(nonce, 0, result, 0, nonce.Length);
            Buffer.BlockCopy(tag, 0, result, nonce.Length, tag.Length);
            Buffer.BlockCopy(ciphertext, 0, result, nonce.Length + tag.Length, ciphertext.Length);

            return result;
        }

        /// <summary>
        /// Decrypts data using AES-256-GCM
        /// </summary>
        private string Decrypt(byte[] encryptedData)
        {
            if (_encryptionKey == null)
                throw new InvalidOperationException("Encryption key not available.");

            const int nonceSize = 12;
            const int tagSize = 16;

            using var aes = new AesGcm(_encryptionKey, tagSize);

            byte[] nonce = new byte[nonceSize];
            byte[] tag = new byte[tagSize];
            byte[] ciphertext = new byte[encryptedData.Length - nonceSize - tagSize];

            Buffer.BlockCopy(encryptedData, 0, nonce, 0, nonceSize);
            Buffer.BlockCopy(encryptedData, nonceSize, tag, 0, tagSize);
            Buffer.BlockCopy(encryptedData, nonceSize + tagSize, ciphertext, 0, ciphertext.Length);

            byte[] plainBytes = new byte[ciphertext.Length];
            aes.Decrypt(nonce, ciphertext, tag, plainBytes);

            return Encoding.UTF8.GetString(plainBytes);
        }

        private bool Load()
        {
            try
            {
                if (!File.Exists(_filePath) || _encryptionKey == null)
                    return true;

                byte[] encryptedData = File.ReadAllBytes(_filePath);

                // Handle empty file
                if (encryptedData.Length == 0)
                    return true;

                string json = Decrypt(encryptedData);

                var list = JsonSerializer.Deserialize<List<PasswordEntry>>(json);

                if (list != null)
                {
                    _entries.Clear();
                    foreach (var item in list)
                        _entries[item.Site] = item;
                }

                return true;
            }
            catch (Exception ex)
            {
                LastError = "Failed to load passwords: " + ex.Message;
                return false;
            }
        }

        private void Save()
        {
            try
            {
                if (_encryptionKey == null)
                {
                    LastError = "Cannot save: encryption key not available.";
                    return;
                }

                var json = JsonSerializer.Serialize(_entries.Values.ToList());
                byte[] encryptedData = Encrypt(json);
                File.WriteAllBytes(_filePath, encryptedData);
            }
            catch (Exception ex)
            {
                LastError = "Failed to save passwords: " + ex.Message;
            }
        }

        #endregion

        #region Utility Methods

        public void OpenPasswordFolder()
        {
            try
            {
                var folder = Path.GetDirectoryName(_filePath);

                if (folder != null && Directory.Exists(folder))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = folder,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
        }

        public void BackupToDesktop()
        {
            LastError = null;
            LastBackupPath = null;

            if (!_isUnlocked)
            {
                LastError = "Password manager is locked.";
                return;
            }

            try
            {
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string dest = Path.Combine(desktop, $"passwords_backup_{timestamp}.enc");

                File.Copy(_filePath, dest, overwrite: true);
                LastBackupPath = dest;
                ResetAutoLockTimer();
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
        }

        /// <summary>
        /// Generates a cryptographically secure random password
        /// </summary>
        public static string GenerateSecurePassword(int length = 16, bool includeSpecial = true)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%^&*()_+-=[]{}|;:,.<>?";

            string chars = letters + digits;
            if (includeSpecial)
                chars += special;

            var result = new char[length];
            using var rng = RandomNumberGenerator.Create();
            var buffer = new byte[length * 4];
            rng.GetBytes(buffer);

            for (int i = 0; i < length; i++)
            {
                var randomValue = BitConverter.ToUInt32(buffer, i * 4);
                result[i] = chars[(int)(randomValue % chars.Length)];
            }

            return new string(result);
        }

        #endregion
    }

    public sealed record PasswordImportResult(int Imported, int Skipped, int Renamed);
}
