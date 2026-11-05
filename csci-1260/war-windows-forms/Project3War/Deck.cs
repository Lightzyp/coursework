using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3War
{
    public class Deck
    {
        private Stack<Card> cards = new Stack<Card>();
        private Random rand = new Random();

        public Deck()
        {
            for (int i = 0; i < 52; i++)
            {
                var suit = (Suit)rand.Next(0, 4);
                var rank = (Rank)rand.Next(2, 15);
                cards.Push(new Card(suit, (Rank)rank));
            }
        }

        public Stack<Card> GetCards()
        {
            return cards;
        }
    }
}
