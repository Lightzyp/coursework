using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public sealed class Game
    {
        public Maze Maze { get; }
        public Player Player { get; }
        public bool Won { get; private set; }
        public bool Lost => Player.IsDead;

        private readonly Queue<string> _messages = new();

        public Game(int width, int height)
        {
            Maze = new Maze(width, height);
            Player = new Player("Hero", baseAttack: 2, startHealth: 30, startX: 1, startY: 1);
            _messages.Enqueue("Use WASD or Arrow Keys to move. Press Q to quit.");
        }

        public void TryMove(int dx, int dy)
        {
            if (Won || Lost) return;

            int nx = Player.X + dx, ny = Player.Y + dy;
            if (!Maze.InBounds(nx, ny) || Maze.Tiles[nx, ny].IsWall)
            { _messages.Enqueue("You bump into a wall."); return; }

            Player.X = nx; Player.Y = ny;
            var tile = Maze.Tiles[nx, ny];

            if (tile.IsExit) { Won = true; _messages.Enqueue("You found the exit! You win!"); return; }

            if (tile.Monster is not null)
            {
                Battle(tile.Monster);
                if (tile.Monster.IsDead) tile.Monster = null;
            }
            else if (tile.Item is not null)
            {
                _messages.Enqueue(tile.Item.Apply(Player));
                tile.Item = null;
            }
        }

        private void Battle(Monster m)
        {
            _messages.Enqueue($"A {m.Name} appears! (HP: {m.Health})");
            while (!Player.IsDead && !m.IsDead)
            {
                _messages.Enqueue(Player.Attack(m));
                if (m.IsDead) { _messages.Enqueue($"{m.Name} is defeated!"); break; }

                _messages.Enqueue(m.Attack(Player));
                if (Player.IsDead) { _messages.Enqueue("You perish... Game Over."); break; }
            }
        }

        public string Render()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Player: {Player.Name}  HP: {Player.Health}  ATK: {Player.AttackPower}");
            sb.AppendLine();

            for (int y = 0; y < Maze.Height; y++)
            {
                for (int x = 0; x < Maze.Width; x++)
                {
                    if (x == Player.X && y == Player.Y) { sb.Append('P'); continue; }
                    var t = Maze.Tiles[x, y];
                    if (t.IsWall) sb.Append('#');
                    else if (t.IsExit) sb.Append('E');
                    else if (t.Monster is not null) sb.Append('M');
                    else if (t.Item is not null) sb.Append('I');
                    else sb.Append('.');
                }
                sb.AppendLine();
            }

            sb.AppendLine();
            int shown = 0;
            foreach (var msg in _messages)
            {
                sb.AppendLine(msg);
                if (++shown >= 5) break;
            }
            while (_messages.Count > 12) _messages.Dequeue();
            return sb.ToString();
        }
    }
}
