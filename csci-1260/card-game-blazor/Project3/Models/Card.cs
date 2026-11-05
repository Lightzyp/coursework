namespace Project3.Models
{
    public enum Suit { Hearts, Diamonds, Clubs, Spades }

    public enum Rank
    {
        Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten,
        Jack = 11, Queen = 12, King = 13, Ace = 14
    }

    /// <summary>
    /// Represents a single card and supports comparison by Rank (Ace high).
    /// </summary>
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
            return Rank.CompareTo(other.Rank); // suit doesn't matter for War
        }

        public override string ToString() => $"{Rank} of {Suit}";
    }
}
