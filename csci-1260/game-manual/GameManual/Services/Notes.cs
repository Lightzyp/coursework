using GameManual.Models;

namespace GameManual.Services
{
    public class NoteService
    {
        private int _nextId = 1;
        // simple in-memory store
        private readonly List<Note> _notes = new();

        public IEnumerable<Note> GetAllForPokemon(int pokemonId) =>
            _notes.Where(n => n.PokemonId == pokemonId).OrderByDescending(n => n.UpdatedAt ?? n.CreatedAt);

        public Note Create(int pokemonId, string title, string content)
        {
            var n = new Note
            {
                Id = _nextId++,
                PokemonId = pokemonId,
                Title = title,
                Content = content
            };
            _notes.Add(n);
            return n;
        }

        public void Update(Note note)
        {
            var existing = _notes.FirstOrDefault(n => n.Id == note.Id);
            if (existing is null) return;
            existing.Title = note.Title;
            existing.Content = note.Content;
            existing.UpdatedAt = DateTime.UtcNow;
        }

        public void Delete(int id) => _notes.RemoveAll(n => n.Id == id);
    }
}
