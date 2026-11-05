namespace Project3.Models
{
    /// <summary>
    /// Deck holds a Stack&lt;Card&gt; and initializes with 52 RANDOM cards (duplicates allowed).
    /// </summary>
    public sealed class Deck
    {
        private readonly Random _rng;
        public Stack<Card> Cards { get; } = new();

        public Deck(Random rng)
        {
            _rng = rng;
            InitializeRandom52();
        }

        private void InitializeRandom52()
        {
            Cards.Clear();
            var suits = Enum.GetValues<Suit>();
            var ranks = Enum.GetValues<Rank>();
            // Generate 52 random cards, duplicates allowed per project spec
            for (int i = 0; i < 52; i++)
            {
                var suit = suits[_rng.Next(suits.Length)];
                var rank = ranks[_rng.Next(ranks.Length)];
                Cards.Push(new Card(suit, rank));
            }
        }

        public bool Any => Cards.Count > 0;

        public Card Draw() => Cards.Pop();
    }
}
