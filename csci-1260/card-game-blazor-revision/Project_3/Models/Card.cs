namespace Project_3.Models
{
    /// <summary>
    /// Represents a single playing card. Ace is high (rank 14), 2 is low (rank 2).
    /// </summary>
    public enum Suit { Clubs, Diamonds, Hearts, Spades }

    public enum Rank
    {
        Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten,
        Jack = 11, Queen, King, Ace = 14
    }

    public sealed class Card : IComparable<Card>
    {
        public Suit Suit { get; }
        public Rank Rank { get; }

        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }

        public int CompareTo(Card? other)
        {
            if (other is null) return 1;
            // Compare rank only; suits do not matter in War.
            return Rank.CompareTo(other.Rank);
        }

        public override string ToString() => $"{Rank} of {Suit}";
    }
}

