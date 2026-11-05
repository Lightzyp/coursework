using AdventureGameManual.Models;

namespace AdventureGameManual.Services;

public class GameDataService
{
    private readonly List<Character> _characters =
    [
        new Character
        {
            Id = 1, Name = "Hero", Type = CharacterType.Player,
            HP = 30, Attack = 8, Defense = 5,
            ImageUrl = "https://images.pexels.com/photos/15668552/pexels-photo-15668552.jpeg",
            Description = "Your brave adventurer."
        },
        new Character
        {
            Id = 2, Name = "Slime", Type = CharacterType.Monster,
            HP = 12, Attack = 3, Defense = 1,
            ImageUrl = "https://media.istockphoto.com/id/1127239954/photo/mucus-character.jpg?s=2048x2048&w=is&k=20&c=oTmW6RAUYOYt_suaSphtqAJ85NjFwQKv2tWSqJI4IBw=",
            Description = "Jiggly menace."
        },
        new Character
        {
            Id = 3, Name = "Guide", Type = CharacterType.NPC,
            HP = 999, Attack = 0, Defense = 0,
            ImageUrl = "https://media.istockphoto.com/id/104240543/photo/wizard-portrait.jpg?s=2048x2048&w=is&k=20&c=AzS39BjRlOE123_LL5vkVvQKby9qFZdXarqyHbcdB-Q=",
            Description = "Knows a lot. Talks a lot."
        } 
    ];

    private readonly List<Item> _items =
    [
        new Item
        {
            Id = 101, Name = "Iron Sword", Type = ItemType.Weapon,
            Power = 10, Value = 25,
            ImageUrl = "https://images.pexels.com/photos/339805/pexels-photo-339805.jpeg",
            Description = "Reliable early-game blade."
        },
        new Item
        {
            Id = 102, Name = "Health Potion", Type = ItemType.Potion,
            Power = 15, Value = 10,
            ImageUrl = "https://media.istockphoto.com/id/478305688/photo/potion.jpg?s=1024x1024&w=is&k=20&c=oCOECYgTovK5GPUo2v1IlPQ4JOSU8MfjShClCNSSYjE=",
            Description = "Restores 15 HP."
        }
    ];

    public IReadOnlyList<Character> Characters => _characters;
    public IReadOnlyList<Item> Items => _items;

    public Character? GetCharacter(int id) => _characters.FirstOrDefault(c => c.Id == id);
    public Item? GetItem(int id) => _items.FirstOrDefault(i => i.Id == id);

    public IEnumerable<IGrouping<CharacterType, Character>> CharactersByType()
        => _characters.GroupBy(c => c.Type);

    public IEnumerable<IGrouping<ItemType, Item>> ItemsByType()
        => _items.GroupBy(i => i.Type);
}
