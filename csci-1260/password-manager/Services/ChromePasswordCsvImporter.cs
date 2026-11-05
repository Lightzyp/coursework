using System.Text;
using Project_4.Models;

namespace Project_4.Services;

public static class ChromePasswordCsvImporter
{
    public static ChromePasswordImportPreview Parse(string csv)
    {
        var rows = ParseRows(csv);
        if (rows.Count < 2)
            throw new FormatException("The CSV file does not contain any password rows.");

        var headers = rows[0]
            .Select((header, index) => new { Header = NormalizeHeader(header), Index = index })
            .Where(item => !string.IsNullOrEmpty(item.Header))
            .ToDictionary(item => item.Header, item => item.Index, StringComparer.OrdinalIgnoreCase);

        if (!headers.ContainsKey("password"))
            throw new FormatException("This does not look like a Chrome password export. The password column is missing.");

        var entries = new List<PasswordEntry>();
        var skippedRows = 0;

        foreach (var row in rows.Skip(1))
        {
            var password = GetValue(row, headers, "password");
            if (string.IsNullOrEmpty(password))
            {
                skippedRows++;
                continue;
            }

            var name = GetValue(row, headers, "name");
            var url = GetValue(row, headers, "url", "origin_url", "action_url");
            var site = string.IsNullOrWhiteSpace(name)
                ? GetSiteName(url)
                : name.Trim();

            if (string.IsNullOrWhiteSpace(site))
                site = "Imported Chrome login";

            var username = GetValue(row, headers, "username", "username_value").Trim();
            var note = GetValue(row, headers, "note", "notes").Trim();
            var notes = BuildNotes(url, note);

            entries.Add(new PasswordEntry(site, username, password, notes));
        }

        if (entries.Count == 0)
            throw new FormatException("No importable passwords were found in the CSV file.");

        return new ChromePasswordImportPreview(entries, skippedRows);
    }

    private static string BuildNotes(string url, string note)
    {
        var notes = new List<string>();

        if (!string.IsNullOrWhiteSpace(url))
            notes.Add($"URL: {url.Trim()}");

        if (!string.IsNullOrWhiteSpace(note))
            notes.Add(note);

        return string.Join(Environment.NewLine, notes);
    }

    private static string GetSiteName(string url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out var uri) && !string.IsNullOrWhiteSpace(uri.Host))
            return uri.Host;

        return url.Trim();
    }

    private static string GetValue(IReadOnlyList<string> row, IReadOnlyDictionary<string, int> headers, params string[] names)
    {
        foreach (var name in names)
        {
            if (headers.TryGetValue(name, out var index) && index < row.Count)
                return row[index];
        }

        return string.Empty;
    }

    private static string NormalizeHeader(string header) =>
        header.Trim().TrimStart('\uFEFF').ToLowerInvariant();

    private static List<List<string>> ParseRows(string csv)
    {
        var rows = new List<List<string>>();
        var row = new List<string>();
        var field = new StringBuilder();
        var insideQuotes = false;

        for (var index = 0; index < csv.Length; index++)
        {
            var current = csv[index];

            if (current == '"')
            {
                if (insideQuotes && index + 1 < csv.Length && csv[index + 1] == '"')
                {
                    field.Append('"');
                    index++;
                }
                else
                {
                    insideQuotes = !insideQuotes;
                }

                continue;
            }

            if (current == ',' && !insideQuotes)
            {
                row.Add(field.ToString());
                field.Clear();
                continue;
            }

            if ((current == '\r' || current == '\n') && !insideQuotes)
            {
                if (current == '\r' && index + 1 < csv.Length && csv[index + 1] == '\n')
                    index++;

                row.Add(field.ToString());
                field.Clear();

                if (row.Any(value => !string.IsNullOrEmpty(value)))
                    rows.Add(row);

                row = new List<string>();
                continue;
            }

            field.Append(current);
        }

        if (insideQuotes)
            throw new FormatException("The CSV file contains an unfinished quoted value.");

        row.Add(field.ToString());
        if (row.Any(value => !string.IsNullOrEmpty(value)))
            rows.Add(row);

        return rows;
    }
}

public sealed record ChromePasswordImportPreview(IReadOnlyList<PasswordEntry> Entries, int SkippedRows);
