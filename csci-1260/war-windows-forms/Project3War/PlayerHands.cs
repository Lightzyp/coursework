using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3War
{
    public class PlayerHands
    {
        
        
            private Dictionary<string, Hand> hands;

            public PlayerHands()
            {
                hands = new Dictionary<string, Hand>();
            }

            
            public void AddPlayer(string name)
            {
                hands[name] = new Hand();
            }

            // Get a specific player's Hand
            public Hand GetHand(string name)
            {
                return hands[name];
            }

            // Get list of player names
            public IEnumerable<string> GetPlayers()
            {
                return hands.Keys;
            }

            // Optional: get full dictionary if needed
            public Dictionary<string, Hand> GetAllHands()
            {
                return hands;
            }
        
    }

}
