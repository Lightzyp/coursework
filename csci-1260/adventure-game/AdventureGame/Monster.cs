using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public sealed class Monster : ICharacter
    {

        private static readonly Random Rng = new();

        public string Name { get; }
        public int Health { get; private set; }
        public int AttackMin { get; }
        public int AttackMax { get; }


        // Explicitly implement the interface properties
        string ICharacter.name => Name;
        int ICharacter.health => Health;

        public Monster(string name, int health, int attackMin, int attackMax)
        {
            Name = name;
            Health = health;
            AttackMin = attackMin;
            AttackMax = attackMax;
        }

        public bool IsDead => Health <= 0;
        public int AttackPower => Rng.Next(AttackMin, AttackMax + 1);

        public string Attack(ICharacter target)
        {
            int dmg = AttackPower;
            target.TakeDamage(dmg);
            return $"{Name} strikes {target.name} for {dmg} damage.";
        }

        public void TakeDamage(int amount)
        {
            if (amount < 0) amount = 0;
            Health = Math.Max(0, Health - amount);
        }
    }
}
