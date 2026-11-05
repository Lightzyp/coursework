using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public interface ICharacter
    {
        string name { get; }
        int health { get; }

        int AttackPower { get; }

        bool IsDead { get; }

        string Attack(ICharacter target);

        void TakeDamage (int amount); 
        
    }
}
