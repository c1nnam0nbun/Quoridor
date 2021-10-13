using System.Drawing;

namespace Quoridor
{
    public class Player
    {
        private Cell currentCell;

        public Player(Cell initialCell)
        {
            currentCell = initialCell;
        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.DarkRed, currentCell.Position.X, currentCell.Position.Y, currentCell.Size.Width, currentCell.Size.Height);
        }

        public void Move(Cell cell)
        {
            if (currentCell.HasNeighbour(cell))
                currentCell = cell;
        }
    }
}