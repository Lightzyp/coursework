using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3War
{
    public class Hand
    {
        public Queue<Card> Cards { get; private set; }

        public Hand()
        {
            Cards = new Queue<Card>();
        }

        public void AddCard(Card c)
        {
            Cards.Enqueue(c);
        }

        public void AddCards(IEnumerable<Card> wonCards)
        {
            foreach (var card in wonCards)
                Cards.Enqueue(card);
        }

        public Card PlayCard()
        {
            if (Cards.Count > 0)
                return Cards.Dequeue();
            return null;
        }

        public bool HasCards()
        {
            return Cards.Count > 0;
        }
    }
}
