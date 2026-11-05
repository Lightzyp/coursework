namespace Project3.Models
{
    /// <summary>
    /// Hand holds a Queue&lt;Card&gt;. Front card is played each round. Winnings get enqueued.
    /// </summary>
    public sealed class Hand
    {
        private readonly Queue<Card> _cards = new();
        public int Count => _cards.Count;

        public void Enqueue(Card c) => _cards.Enqueue(c);
        public void EnqueueRange(IEnumerable<Card> cards)
        {
            foreach (var c in cards) _cards.Enqueue(c);
        }

        public Card? DrawTop()
        {
            return _cards.Count > 0 ? _cards.Dequeue() : null;
        }
    }
}
