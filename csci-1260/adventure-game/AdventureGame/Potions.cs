using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public sealed class Potion : Item
    {
        public int HealAmount { get; }

        public Potion(string name, int healAmount)
            : base(name, $"You used a {name}! Healed {healAmount} HP.")
        {
            HealAmount = healAmount;
        }

        public override string Apply(Player player)
        {
            player.Heal(HealAmount);
            return $"{PickupMessage} Current HP: {player.Health}.";
        }
    }
}
