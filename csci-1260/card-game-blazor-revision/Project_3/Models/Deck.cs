namespace Project_3.Models
{
    /// <summary>
    /// Deck uses a Stack<Card>. We'll push a randomized 52-card deck into it.
    /// </summary>
    public sealed class Deck
    {
        private readonly Stack<Card> _cards = new();
        private static readonly Random _rng = new();

        public int Count => _cards.Count;

        public Deck()
        {
            var list = new List<Card>(capacity: 52);
            foreach (Suit s in Enum.GetValues(typeof(Suit)))
                foreach (Rank r in Enum.GetValues(typeof(Rank)))
                    list.Add(new Card(s, r));

            // Shuffle (Fisher–Yates)
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = _rng.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }

            // Push to stack
            foreach (var c in list)
                _cards.Push(c);
        }

        public bool TryDraw(out Card? card)
        {
            if (_cards.Count == 0) { card = null; return false; }
            card = _cards.Pop();
            return true;
        }
    }
}
