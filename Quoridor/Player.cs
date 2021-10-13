using System.Drawing;

namespace Quoridor
{
    public class Player
    {
        public Cell currentCell;
        private Brush brush;
        public Wall[] Walls { get; } = new Wall[10];

        public Player(Cell initialCell, Brush brush)
        {
            currentCell = initialCell;
            currentCell.IsTaken = true;
            this.brush = brush;
        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(brush, currentCell.Position.X, currentCell.Position.Y, currentCell.Size.Width, currentCell.Size.Height);
        }

        public bool Move(Cell cell)
        {
            if (!currentCell.HasNeighbour(cell) || cell.IsTaken) return false;
            currentCell.IsTaken = false;
            cell.IsTaken = true;
            currentCell = cell;
            return true;
        }
    }
}