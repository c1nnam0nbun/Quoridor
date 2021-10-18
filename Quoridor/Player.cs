using System.Drawing;

namespace Quoridor
{
    public class Player
    {
        public Cell CurrentCell { get; private set; }
        private Brush Brush { get; }
        public Wall[] Walls { get; } = new Wall[10];

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
            if (!CurrentCell.HasNeighbour(cell) || cell.IsTaken) return false;
            CurrentCell.IsTaken = false;
            cell.IsTaken = true;
            CurrentCell = cell;
            return true;
        }
    }
}