namespace GameManual.Models
{
    public class Note
    {
        public int Id { get; set; }
        public int PokemonId { get; set; }            //  tie note to a specific Pokémon
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
