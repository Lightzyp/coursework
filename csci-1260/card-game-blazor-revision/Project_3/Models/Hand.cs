namespace Project_3.Models
{
    /// <summary>
    /// Player hand implemented as a queue (front = play, back = collect).
    /// </summary>
    public sealed class Hand
    {
        private readonly Queue<Card> _cards = new();

        public int Count => _cards.Count;

        public bool TryDequeue(out Card? card) => _cards.TryDequeue(out card);

        public void Enqueue(Card card) => _cards.Enqueue(card);

        public void EnqueueRange(IEnumerable<Card> cards)
        {
            foreach (var c in cards) _cards.Enqueue(c);
        }
    }
}