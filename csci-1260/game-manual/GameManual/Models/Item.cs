namespace GameManual.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string Description { get; set; } = "";
        public string Type { get; set; } = ""; // e.g., Weapon, Potion
    }
}

