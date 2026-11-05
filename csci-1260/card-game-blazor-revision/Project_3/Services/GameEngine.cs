using System;
using System.Collections.Generic;
using System.Linq;
using Project_3.Models;

namespace Project_3.Services
{
    public sealed class GameEngine
    {
        public IReadOnlyList<string> PlayerOrder => _playerOrder;
        public string? WinnerName { get; private set; }
        public bool GameActive { get; private set; }

        public int RoundCount { get; private set; } = 0;
        private const int MaxRounds = 20000; // safety cap

        private readonly Dictionary<string, Hand> _hands = new();
        private readonly Dictionary<string, Card> _playedThisRound = new();
        private readonly List<Card> _sidePot = new(); // tie pile
        private readonly List<string> _playerOrder = new();

        private static readonly Random _rng = new(); // for shuffling winnings

        public void StartNewGame(IEnumerable<string> players)
        {
            WinnerName = null;
            GameActive = true;

            _hands.Clear();
            _playedThisRound.Clear();
            _sidePot.Clear();
            _playerOrder.Clear();
            RoundCount = 0;

            // Create players
            foreach (var p in players.Distinct().Where(p => !string.IsNullOrWhiteSpace(p)))
            {
                _hands[p] = new Hand();
                _playerOrder.Add(p);
            }

            // Deal from Deck (Stack) into player Hands (Queues)
            var deck = new Deck();
            int i = 0;
            while (deck.TryDraw(out var card) && card is not null)
            {
                var name = _playerOrder[i % _playerOrder.Count];
                _hands[name].Enqueue(card);
                i++;
            }
        }

        public int HandCount(string player) => _hands.TryGetValue(player, out var h) ? h.Count : 0;

        public void PlayOneRound()
        {
            if (!GameActive) return;

            // If 1 or 0 players have cards, finish now.
            var activePlayers = _playerOrder.Where(p => _hands[p].Count > 0).ToList();
            if (activePlayers.Count <= 1)
            {
                // Award side pot if exactly one player remains
                if (activePlayers.Count == 1 && _sidePot.Count > 0)
                {
                    _hands[activePlayers[0]].EnqueueRange(_sidePot);
                    _sidePot.Clear();
                }
                FinalizeWinner();
                return;
            }

            // Safety: prevent infinite games
            if (RoundCount >= MaxRounds)
            {
                var best = _playerOrder
                    .Select(p => (Player: p, Count: _hands[p].Count))
                    .OrderByDescending(x => x.Count)
                    .First();
                WinnerName = best.Player;
                GameActive = false;
                return;
            }

            RoundCount++;

            _playedThisRound.Clear();

            // 1) Each active player reveals top card
            foreach (var p in _playerOrder)
            {
                if (_hands[p].TryDequeue(out var card) && card is not null)
                    _playedThisRound[p] = card;
            }

            // If nobody (or only one) played, resolve/finish.
            if (_playedThisRound.Count <= 1)
            {
                if (_playedThisRound.Count == 1 && _sidePot.Count > 0)
                {
                    var lone = _playedThisRound.Keys.First();
                    var winCards = new List<Card> { _playedThisRound[lone] };
                    winCards.AddRange(_sidePot);
                    _sidePot.Clear();
                    _hands[lone].EnqueueRange(winCards);
                }
                FinalizeWinner();
                return;
            }

            // 2) Determine highest rank
            var highest = _playedThisRound.Values.Max(c => c.Rank);
            var roundWinners = _playedThisRound.Where(kv => kv.Value.Rank == highest)
                                               .Select(kv => kv.Key)
                                               .ToList();

            if (roundWinners.Count > 1)
            {
                // Tie: move played cards to side pot
                _sidePot.AddRange(_playedThisRound.Values);

                // If only one player still has cards after the tie, award pot now
                var stillActive = _playerOrder.Where(p => _hands[p].Count > 0).ToList();
                if (stillActive.Count == 1 && _sidePot.Count > 0)
                {
                    _hands[stillActive[0]].EnqueueRange(_sidePot);
                    _sidePot.Clear();
                }
                return; // play another round later
            }

            // 3) Single winner collects (shuffle winnings to break cycles)
            var winner = roundWinners[0];
            CollectToWinner(winner);

            // 4) Check game over
            if (_playerOrder.All(p => p == winner || _hands[p].Count == 0))
            {
                WinnerName = winner;
                GameActive = false;
            }
        }

        private void CollectToWinner(string winner)
        {
            var winnings = new List<Card>(_playedThisRound.Count + _sidePot.Count);

            // gather played cards in player order (then we shuffle)
            foreach (var p in _playerOrder)
                if (_playedThisRound.TryGetValue(p, out var c))
                    winnings.Add(c);

            if (_sidePot.Count > 0)
            {
                winnings.AddRange(_sidePot);
                _sidePot.Clear();
            }

            // 🔑 Break deterministic cycles: shuffle before enqueueing
            for (int i = winnings.Count - 1; i > 0; i--)
            {
                int j = _rng.Next(i + 1);
                (winnings[i], winnings[j]) = (winnings[j], winnings[i]);
            }

            _hands[winner].EnqueueRange(winnings);
        }

        private void FinalizeWinner()
        {
            var remaining = _playerOrder.Where(p => _hands[p].Count > 0).ToList();

            if (remaining.Count == 1)
            {
                if (_sidePot.Count > 0)
                {
                    _hands[remaining[0]].EnqueueRange(_sidePot);
                    _sidePot.Clear();
                }
                WinnerName = remaining[0];
            }
            else
            {
                if (remaining.Count > 1)
                {
                    var best = remaining
                        .Select(p => (Player: p, Count: _hands[p].Count))
                        .OrderByDescending(x => x.Count)
                        .First();
                    WinnerName = best.Player;
                }
                else
                {
                    WinnerName = null; // edge case
                }
            }
            GameActive = false;
        }
    }
}
