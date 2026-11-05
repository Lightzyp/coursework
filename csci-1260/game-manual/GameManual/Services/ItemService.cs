using GameManual.Models;

namespace GameManual.Services
{
    public class ItemService
    {
        private readonly ApiClient _api;
        private List<Item>? _cache;

        public ItemService(ApiClient api) => _api = api;

        public async Task<IReadOnlyList<Item>> GetAllAsync(int limit = 30)
        {
            if (_cache is not null && _cache.Count >= limit) return _cache;

            var list = await _api.GetAsync<ItemList>("item?limit=" + limit) ?? new();
            var results = new List<Item>();

            if (list.results is null) return results;

            foreach (var entry in list.results)
            {
                var id = ExtractTrailingId(entry.url);
                var detail = await _api.GetAsync<ItemDetail>($"item/{id}");
                if (detail is null) continue;

                var img = detail.sprites?.@default ?? "";
                string desc = "";
                if (detail.effect_entries is not null)
                {
                    var en = detail.effect_entries.FirstOrDefault(e => e.language.name == "en");
                    if (en is not null) desc = en.short_effect;
                }

                results.Add(new Item
                {
                    Id = id,
                    Name = Capitalize(detail.name),
                    ImageUrl = img,
                    Description = desc,
                    Type = "Item"
                });
            }

            _cache = results;
            return _cache;
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            var all = await GetAllAsync(Math.Max(30, id));
            return all.FirstOrDefault(i => i.Id == id);
        }

        private static int ExtractTrailingId(string url)
        {
            var parts = url.TrimEnd('/').Split('/');
            return int.Parse(parts[^1]);
        }
        private static string Capitalize(string s) =>
            string.IsNullOrWhiteSpace(s) ? s : char.ToUpper(s[0]) + s[1..];

        // ---- DTOs ----
        private class ItemList { public List<PokeRef>? results { get; set; } }
        private class PokeRef { public string name { get; set; } = ""; public string url { get; set; } = ""; }

        private class ItemDetail
        {
            public string name { get; set; } = "";
            public ItemSprites? sprites { get; set; }
            public List<EffectEntry>? effect_entries { get; set; }
        }
        private class ItemSprites { public string? @default { get; set; } }
        private class EffectEntry
        {
            public string short_effect { get; set; } = "";
            public NamedRef language { get; set; } = new();
        }
        private class NamedRef { public string name { get; set; } = ""; }
    }
}
