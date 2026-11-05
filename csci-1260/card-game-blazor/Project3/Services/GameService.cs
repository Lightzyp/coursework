using Project3.Models;

namespace Project3.Services
{
    /// <summary>
    /// Core game orchestration: deals the Deck (Stack), manages Hands (Queues),
    /// per-round plays into PlayedCards (Dictionary), tracks the pot (List),
    /// determines winners, and logs actions.
    /// </summary>
    public sealed class GameService
    {
        private readonly Random _rng = new();
        public bool IsActive { get; private set; }
        public bool HasWinner => !string.IsNullOrEmpty(WinnerName);
        public string WinnerName { get; private set; } = string.Empty;

        public PlayerHands PlayerHands { get; private set; } = new();
        public List<string> PlayerOrder { get; private set; } = new();

        private Deck? _deck;
        private readonly List<Card> _pot = new(); // pile for ties
        private readonly PlayedCards _played = new();

        // Lightweight readable log for rubric "documentation/readability"
        public LinkedList<string> Log { get; } = new();

        public int PotCount => _pot.Count;
        public IReadOnlyDictionary<string, Card> PlayedCardsSnapshot => _played.Snapshot;

        public void StartNewGame(int playerCount)
        {
            Log.Clear();
            WinnerName = string.Empty;
            IsActive = true;

            _deck = new Deck(_rng);
            PlayerHands = new PlayerHands();
            PlayerOrder = Enumerable.Range(1, playerCount).Select(i => $"Player {i}").ToList();
            foreach (var name in PlayerOrder)
            {
                PlayerHands.Add(name, new Hand());
            }

            DealRoundRobin();

            _pot.Clear();
            _played.Clear();

            AddLog($"Game started with {playerCount} player(s). Deck randomly generated (52 cards, duplicates allowed).");
            DumpCounts();
        }

        public void PlayOneRound()
        {
            if (!IsActive || HasWinner) return;

            _played.Clear();

            // Each active player draws top card (Queue front)
            foreach (var name in PlayerOrder)
            {
                var hand = PlayerHands[name];
                var card = hand.DrawTop();
                if (card is not null)
                {
                    _played.Add(name, card);
                }
            }

            if (_played.Snapshot.Count == 0)
            {
                // Everyone ran out — edge case; decide winner by who holds most? Here we end game.
                DetermineWinnerByCounts();
                return;
            }

            // Determine leaders by highest rank
            var (_, leaders) = _played.GetLeaders();

            // Put all table cards into a temporary pile (go to pot if tie else to winner)
            var thisRoundTable = _played.AllCards().ToList();

            if (leaders.Count == 1)
            {
                var winner = leaders[0];
                // Winner takes current table + accumulated pot -> enqueue to the end (bottom)
                PlayerHands[winner].EnqueueRange(thisRoundTable.Concat(_pot));
                AddLog($"{winner} wins the round and takes {thisRoundTable.Count + _pot.Count} card(s).");
                _pot.Clear();
            }
            else
            {
                // Tie: move current table cards into pot (winner-takes-all later)
                _pot.AddRange(thisRoundTable);
                AddLog($"Tie between {string.Join(", ", leaders)} — {thisRoundTable.Count} card(s) moved to pot (now {_pot.Count}).");
            }

            DumpCounts();
            CheckForGameWinner();
        }

        // ————————— helpers —————————

        private void DealRoundRobin()
        {
            // Even distribution: pop from Stack and enqueue to each Hand in turn
            int i = 0;
            while (_deck!.Any)
            {
                var targetName = PlayerOrder[i % PlayerOrder.Count];
                PlayerHands[targetName].Enqueue(_deck.Draw());
                i++;
            }
        }

        private void CheckForGameWinner()
        {
            var withCards = PlayerOrder.Where(n => PlayerHands[n].Count > 0).ToList();
            if (withCards.Count == 1 && _pot.Count == 0 && _played.Snapshot.Count == 0)
            {
                WinnerName = withCards[0];
                IsActive = false;
                AddLog($"*** {WinnerName} has collected all cards. GAME OVER. ***");
            }
            else if (withCards.Count == 0)
            {
                DetermineWinnerByCounts(); // rare edge - declare by max count (shouldn't happen often)
            }
        }

        private void DetermineWinnerByCounts()
        {
            // If no one can play, choose player with most cards in hand (stable rule to end stalemate)
            var best = PlayerOrder
                .Select(n => (name: n, count: PlayerHands[n].Count))
                .OrderByDescending(x => x.count)
                .FirstOrDefault();

            WinnerName = best.name;
            IsActive = false;
            AddLog($"*** No more playable cards. {WinnerName} has the most cards ({best.count}). GAME OVER. ***");
        }

        private void AddLog(string line)
        {
            Log.AddFirst(line);
            while (Log.Count > 400) Log.RemoveLast();
        }

        private void DumpCounts()
        {
            var parts = PlayerOrder.Select(n => $"{n}:{PlayerHands[n].Count}");
            AddLog("Counts → " + string.Join(" | ", parts) + $" | Pot:{_pot.Count}");
        }

        // Expose counts for UI
        public Dictionary<string, Hand> PlayerHandsDict => PlayerHands.Hands;
        public Dictionary<string, Hand> PlayerHandsSnapshot =>
            PlayerHands.Hands.ToDictionary(kv => kv.Key, kv => kv.Value);
    }
}
