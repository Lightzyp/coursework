using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3War
{
    public class PlayedCards
    {
        public Dictionary<string, Card> CardsPlayed { get; private set; } = new Dictionary<string, Card>();

        public void AddPlayedCard(string playerName, Card card)
        {
            CardsPlayed[playerName] = card;
        }

        public List<KeyValuePair<string, Card>> GetCards()
        {
            return CardsPlayed.ToList();
        }

        public void Clear()
        {
            CardsPlayed.Clear();
        }
    }

}
