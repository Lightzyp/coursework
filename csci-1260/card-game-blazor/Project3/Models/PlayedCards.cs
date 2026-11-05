namespace Project3.Models
{
    /// <summary>
    /// Dictionary of player name → Card currently on the table for this round.
    /// </summary>
    public sealed class PlayedCards
    {
        private readonly Dictionary<string, Card> _table = new();

        public IReadOnlyDictionary<string, Card> Snapshot => _table;

        public void Clear() => _table.Clear();

        public void Add(string playerName, Card card) => _table[playerName] = card;

        public IEnumerable<Card> AllCards() => _table.Values;

        public (int maxRank, List<string> leaders) GetLeaders()
        {
            int max = int.MinValue;
            var leaders = new List<string>();
            foreach (var kv in _table)
            {
                int r = (int)kv.Value.Rank;
                if (r > max)
                {
                    max = r;
                    leaders.Clear();
                    leaders.Add(kv.Key);
                }
                else if (r == max)
                {
                    leaders.Add(kv.Key);
                }
            }
            return (max, leaders);
        }
    }
}
