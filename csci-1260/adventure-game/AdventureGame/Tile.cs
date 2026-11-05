using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public sealed class Tile
    {
        public bool IsWall { get; set; }
        public bool IsExit { get; set; }
        public Item? Item { get; set; }
        public Monster? Monster { get; set; }
        public bool IsEmptyFloor => !IsWall && !IsExit && Item is null && Monster is null;
    }
}
