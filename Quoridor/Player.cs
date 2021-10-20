using System.Drawing;
using System.Linq;

namespace Quoridor
{
    public class Player
    {
        public Cell CurrentCell { get; private set; }
        private Brush Brush { get; }
        public Wall[] Walls { get; set; } = new Wall[10];

        public Player(Cell initialCell, Brush brush)
        {
            CurrentCell = initialCell;
            CurrentCell.IsTaken = true;
            Brush = brush;
        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(Brush, CurrentCell.Position.X, CurrentCell.Position.Y, CurrentCell.Size.Width, CurrentCell.Size.Height);
        }

        public bool Move(Cell cell)
        {
            if (cell == null) return false;
            if (!CurrentCell.HasNeighbour(cell)) return false;
            if (cell.IsTaken)
            {
                if (CurrentCell.Index.X == cell.Index.X - 1)
                {
                    Cell curr = cell;
                    Cell next = cell.Neighbours.Find(n => n.Index.X == curr.Index.X + 1);
                    if (next == null || !next.IsPlayable) return false;
                    cell = next;
                }

                else if (CurrentCell.Index.X == cell.Index.X + 1)
                {
                    Cell curr = cell;
                    Cell next = cell.Neighbours.Find(n => n.Index.X == curr.Index.X - 1);
                    if (next == null || !next.IsPlayable) return false;
                    cell = next;
                }

                else if (CurrentCell.Index.Y == cell.Index.Y - 1)
                {
                    Cell curr = cell;
                    Cell next = cell.Neighbours.Find(n => n.Index.Y == curr.Index.Y + 1);
                    if (next == null || !next.IsPlayable) return false;
                    cell = next;
                }

                else if (CurrentCell.Index.Y == cell.Index.Y + 1)
                {
                    Cell curr = cell;
                    Cell next = cell.Neighbours.Find(n => n.Index.Y == curr.Index.Y - 1);
                    if (next == null || !next.IsPlayable) return false;
                    cell = next;
                }
                
                else return false;
            }
            
            CurrentCell.IsTaken = false;
            cell.IsTaken = true;
            CurrentCell = cell;
            return true;
        }
    }
}