using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Project_4.Services;

public class PwnedPasswordService
{
    private readonly HttpClient _httpClient;

    public PwnedPasswordService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> GetExposureCountAsync(string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(password))
            return 0;

        var passwordHash = Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(password)));
        var prefix = passwordHash[..5];
        var suffix = passwordHash[5..];

        using var request = new HttpRequestMessage(HttpMethod.Get, $"range/{prefix}");
        request.Headers.Add("Add-Padding", "true");

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var matches = await response.Content.ReadAsStringAsync(cancellationToken);
        foreach (var line in matches.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = line.Trim().Split(':', 2);
            if (parts.Length == 2 &&
                parts[0].Equals(suffix, StringComparison.OrdinalIgnoreCase) &&
                int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out var count))
            {
                return count;
            }
        }

        return 0;
    }
}
