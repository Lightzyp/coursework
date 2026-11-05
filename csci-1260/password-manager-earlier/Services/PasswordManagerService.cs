using System.Text.Json;
using Project_4.Models;

namespace Project_4.Services
{
    public class PasswordManagerService
    {
        // File path for storing passwords
        private readonly string? _filePath;

        // Dictionary storing password entries using the site as the key
        private readonly Dictionary<string, PasswordEntry> _entries = new();

        // Stores any error messages
        public string? LastError { get; private set; }

        // Allows UI to display where the file is saved
        public string FileLocation => _filePath!;

        public PasswordManagerService()
        {
            // Get local AppData folder (per-user storage)
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folder = Path.Combine(appData, "PasswordManager");

            // Create PasswordManager folder if it doesn’t exist
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // Full path to JSON file
            _filePath = Path.Combine(folder, "passwords.json");

            // Load any saved passwords on startup
            Load();
        }

        // Opens the password folder in File Explorer
        public void OpenPasswordFolder()
        {
            try
            {
                var folder = Path.GetDirectoryName(_filePath!);

                if (folder != null && Directory.Exists(folder))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = folder,
                        UseShellExecute = true // Required to open Explorer
                    });
                }
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
        }

        // Returns all saved password entries
        public List<PasswordEntry> GetAll()
        {
            return _entries.Values.ToList();
        }

        // Adds a new entry or updates an existing one
        public void AddOrUpdate(PasswordEntry entry)
        {
            try
            {
                _entries[entry.Site] = entry;
                Save();
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
        }

        // Updates an existing entry (used for the Edit Password page)
        public void Update(PasswordEntry updatedEntry)
        {
            try
            {
                if (_entries.ContainsKey(updatedEntry.Site))
                {
                    _entries[updatedEntry.Site] = updatedEntry;
                    Save();
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

        // Deletes a password entry by site name
        public void Remove(string site)
        {
            if (_entries.Remove(site))
                Save();
        }

        // Loads saved passwords from the JSON file
        private void Load()
        {
            try
            {
                if (!File.Exists(_filePath!))
                    return; // No file yet, nothing to load

                string json = File.ReadAllText(_filePath!);
                var list = JsonSerializer.Deserialize<List<PasswordEntry>>(json);

                if (list != null)
                {
                    _entries.Clear();

                    // Convert list to dictionary
                    foreach (var item in list)
                        _entries[item.Site] = item;
                }
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
        }

        // Saves all password entries to the JSON file
        private void Save()
        {
            try
            {
                var json = JsonSerializer.Serialize(
                    _entries.Values.ToList(),
                    new JsonSerializerOptions() { WriteIndented = true }
                );

                File.WriteAllText(_filePath!, json);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
        }

        // Creates a backup of the password file on the desktop
        public void BackupToDesktop()
        {
            try
            {
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string dest = Path.Combine(desktop, "passwords_backup.json");

                File.Copy(_filePath!, dest, overwrite: true);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
        }
    }
}
