using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public sealed class Player : ICharacter
    {
        // Explicit interface implementation for ICharacter
        string ICharacter.name => Name;
        int ICharacter.health => Health;
        public string Name { get; }
        public int Health { get; private set; }
        public int BaseAttack { get; }
        public int X { get; set; }
        public int Y { get; set; }

        private readonly List<Weapon> _inventory = new();

        public Player(string name, int baseAttack, int startHealth, int startX, int startY)
        {
            Name = name;
            BaseAttack = baseAttack;
            Health = startHealth;
            X = startX;
            Y = startY;
        }

        public void AddWeapon(Weapon w) => _inventory.Add(w);
        private int BestWeaponModifier => _inventory.Count == 0 ? 0 : _inventory.Max(w => w.AttackModifier);
        public int AttackPower => BaseAttack + BestWeaponModifier;
        public bool IsDead => Health <= 0;

        public string Attack(ICharacter target)
        {
            target.TakeDamage(AttackPower);
            return $"{Name} hits {target.name} for {AttackPower} damage.";
        }

        public void TakeDamage(int amount)
        {
            if (amount < 0) amount = 0;
            Health = Math.Max(0, Health - amount);
        }

        public void Heal(int amount) => Health += Math.Max(0, amount);
    }
}
