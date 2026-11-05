namespace AdventureGameManual.Models;

public enum CharacterType { Player, Monster, NPC }

public class Character
{
    public int Id { get; set; }
    public string Name { get; set; } = "sdggassaffnjng";
    public CharacterType Type { get; set; }

    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";

    public int HP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
}
