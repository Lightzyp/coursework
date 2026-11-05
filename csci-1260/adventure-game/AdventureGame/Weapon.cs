using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public sealed class Weapon : Item
    {

         
        public int AttackModifier { get; }

        public Weapon(string name, int attackModifier)
            : base(name, $"You picked up {name}! Attack +{attackModifier}.")
        {
            AttackModifier = attackModifier;
        }

        public override string Apply(Player player)
        {
            player.AddWeapon(this);
            return PickupMessage;
        }
    }
}
