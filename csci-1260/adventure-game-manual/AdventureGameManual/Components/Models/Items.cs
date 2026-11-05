namespace AdventureGameManual.Models;

public enum ItemType { Weapon, Potion, Armor, KeyItem }

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public ItemType Type { get; set; }

    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";

    public int Power { get; set; }
    public int Value { get; set; }
}
