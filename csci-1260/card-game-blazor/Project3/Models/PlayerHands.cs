namespace Project3.Models
{
    /// <summary>
    /// Dictionary of player name → Hand.
    /// </summary>
    public sealed class PlayerHands
    {
        public Dictionary<string, Hand> Hands { get; } = new();

        public Hand this[string name] => Hands[name];

        public IEnumerable<string> Names => Hands.Keys;

        public void Add(string name, Hand hand) => Hands.Add(name, hand);

        public int ActivePlayersWithCards =>
            Hands.Values.Count(h => h.Count > 0);
    }
}
