using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public sealed class Maze
    {
        public int Width { get; }
        public int Height { get; }
        public Tile[,] Tiles { get; }
        private readonly Random _rng = new();

        public Maze(int width, int height)
        {
            // odd sizes produce nicer mazes
            Width = width % 2 == 0 ? width + 1 : width;
            Height = height % 2 == 0 ? height + 1 : height;

            Tiles = new Tile[Width, Height];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    Tiles[x, y] = new Tile { IsWall = true };

            GeneratePerfectMaze();
            PlaceExit();
            PlaceContent();
        }

        public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

        private void GeneratePerfectMaze() => Carve(1, 1);

        private void Carve(int sx, int sy)
        {
            Tiles[sx, sy].IsWall = false;
            var dirs = new List<(int dx, int dy)> { (2, 0), (-2, 0), (0, 2), (0, -2) };
            Shuffle(dirs);
            foreach (var (dx, dy) in dirs)
            {
                int nx = sx + dx, ny = sy + dy;
                if (InBounds(nx, ny) && Tiles[nx, ny].IsWall)
                {
                    Tiles[sx + dx / 2, sy + dy / 2].IsWall = false; // knock wall between
                    Tiles[nx, ny].IsWall = false;
                    Carve(nx, ny);
                }
            }
        }

        private void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            { int j = _rng.Next(i + 1); (list[i], list[j]) = (list[j], list[i]); }
        }

        private void PlaceExit()
        {
            // find a far-ish open tile and mark exit
            for (int y = Height - 2; y >= 1; y--)
                for (int x = Width - 2; x >= 1; x--)
                    if (!Tiles[x, y].IsWall) { Tiles[x, y].IsExit = true; return; }
        }

        private void PlaceContent()
        {
            int targetWeapons = Math.Max(2, (Width * Height) / 150);
            int targetPotions = Math.Max(3, (Width * Height) / 120);
            int targetMonsters = Math.Max(4, (Width * Height) / 90);

            PlaceMany(targetWeapons, () => new Weapon(RandomWeaponName(), _rng.Next(1, 4)));
            PlaceMany(targetPotions, () => new Potion("Small Potion", _rng.Next(5, 13)));
            PlaceMany(targetMonsters, () => new Monster(RandomMonsterName(), _rng.Next(10, 21), 1, _rng.Next(3, 6)));
        }

        private void PlaceMany<T>(int count, Func<T> factory)
        {
            int tries = 0, placed = 0, maxTries = count * 50;
            while (placed < count && tries++ < maxTries)
            {
                int x = _rng.Next(1, Width - 1);
                int y = _rng.Next(1, Height - 1);
                if (Tiles[x, y].IsWall || Tiles[x, y].IsExit) continue;

                var obj = factory();
                if (obj is Item item)
                {
                    if (Tiles[x, y].Item is null && Tiles[x, y].Monster is null)
                    { Tiles[x, y].Item = item; placed++; }
                }
                else if (obj is Monster m)
                {
                    if (Tiles[x, y].Item is null && Tiles[x, y].Monster is null)
                    { Tiles[x, y].Monster = m; placed++; }
                }
            }
        }

        private string RandomWeaponName()
        {
            string[] names = { "Dagger", "Short Sword", "Axe", "Mace" };
            return names[_rng.Next(names.Length)];
        }

        private string RandomMonsterName()
        {
            string[] names = { "Goblin", "Rat", "Skeleton", "Slime" };
            return names[_rng.Next(names.Length)];
        }
    }
}
