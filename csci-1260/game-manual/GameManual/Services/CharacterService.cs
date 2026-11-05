using GameManual.Models;
using System.Text.Json.Serialization;

namespace GameManual.Services
{
    public class CharacterService
    {
        private readonly ApiClient _api;

        public CharacterService(ApiClient api) => _api = api;

        // === Public API ===

        /// <summary>Return total number of Pokémon in PokéAPI.</summary>
        public async Task<int> GetTotalCountAsync()
        {
            var list = await _api.GetAsync<PokemonList>("pokemon?limit=1&offset=0") ?? new();
            return list.count;
        }

        /// <summary>Fetch a page of Pokémon (offset/limit) with details + flavor text.</summary>
        public async Task<List<Character>> GetPageAsync(int offset, int pageSize)
        {
            var list = await _api.GetAsync<PokemonList>($"pokemon?limit={pageSize}&offset={offset}") ?? new();
            var results = new List<Character>();
            if (list.results is null) return results;

            foreach (var entry in list.results)
            {
                var id = ExtractId(entry.url);

                var detail = await _api.GetAsync<PokemonDetail>($"pokemon/{id}");
                if (detail is null) continue;

                // optional: species for flavor text
                string description = await GetFlavorTextAsync(id);

                var img = detail.sprites?.other?.official_artwork?.front_default
                          ?? detail.sprites?.front_default
                          ?? "";

                int? hp = detail.stats?.FirstOrDefault(s => s.stat.name == "hp")?.base_stat;
                int? atk = detail.stats?.FirstOrDefault(s => s.stat.name == "attack")?.base_stat;
                int? def = detail.stats?.FirstOrDefault(s => s.stat.name == "defense")?.base_stat;

                results.Add(new Character
                {
                    Id = id,
                    Name = Capitalize(detail.name),
                    ImageUrl = img,
                    Description = description,
                    Type = "Pokemon",
                    BaseHP = hp,
                    BaseAttack = atk,
                    BaseDefense = def
                });
            }

            return results;
        }

        /// <summary>Convenience: first N Pokémon.</summary>
        public async Task<IReadOnlyList<Character>> GetAllAsync(int limit = 30)
        {
            return await GetPageAsync(0, limit);
        }

        /// <summary>Fetch a single Pokémon by numeric id (with flavor text).</summary>
        public async Task<Character?> GetByIdAsync(int id)
        {
            var detail = await _api.GetAsync<PokemonDetail>($"pokemon/{id}");
            if (detail is null) return null;

            string description = await GetFlavorTextAsync(id);

            var img = detail.sprites?.other?.official_artwork?.front_default
                      ?? detail.sprites?.front_default
                      ?? "";

            int? hp = detail.stats?.FirstOrDefault(s => s.stat.name == "hp")?.base_stat;
            int? atk = detail.stats?.FirstOrDefault(s => s.stat.name == "attack")?.base_stat;
            int? def = detail.stats?.FirstOrDefault(s => s.stat.name == "defense")?.base_stat;

            return new Character
            {
                Id = id,
                Name = Capitalize(detail.name),
                ImageUrl = img,
                Description = description,
                Type = "Pokemon",
                BaseHP = hp,
                BaseAttack = atk,
                BaseDefense = def
            };
        }

        // === Private helpers ===

        private async Task<string> GetFlavorTextAsync(int id)
        {
            try
            {
                var species = await _api.GetAsync<PokemonSpecies>($"pokemon-species/{id}");
                var en = species?.flavor_text_entries?
                    .FirstOrDefault(f => f.language.name == "en");

                if (en is null) return "";
                // Clean weird line breaks from the API
                return en.flavor_text.Replace('\n', ' ').Replace('\f', ' ').Trim();
            }
            catch
            {
                return "";
            }
        }

        private static int ExtractId(string url) =>
            int.Parse(url.TrimEnd('/').Split('/')[^1]);

        private static string Capitalize(string s) =>
            string.IsNullOrWhiteSpace(s) ? s : char.ToUpper(s[0]) + s[1..];

        // === DTOs for PokéAPI responses ===

        private class PokemonList
        {
            public int count { get; set; }
            public List<PokeRef>? results { get; set; }
        }

        private class PokeRef
        {
            public string name { get; set; } = "";
            public string url { get; set; } = "";
        }

        private class PokemonDetail
        {
            public string name { get; set; } = "";
            public Sprites? sprites { get; set; }
            public List<PokeStat>? stats { get; set; }
        }

        private class Sprites
        {
            public string? front_default { get; set; }
            public OtherSprites? other { get; set; }
        }

        private class OtherSprites
        {
            [JsonPropertyName("official-artwork")]
            public Artwork? official_artwork { get; set; }
        }

        private class Artwork
        {
            public string? front_default { get; set; }
        }

        private class PokeStat
        {
            public int base_stat { get; set; }
            public NamedRef stat { get; set; } = new();
        }

        private class NamedRef
        {
            public string name { get; set; } = "";
        }

        private class PokemonSpecies
        {
            public List<FlavorEntry>? flavor_text_entries { get; set; }
        }

        private class FlavorEntry
        {
            public string flavor_text { get; set; } = "";
            public NamedRef language { get; set; } = new();
        }
    }
}
