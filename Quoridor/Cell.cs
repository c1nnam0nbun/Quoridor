using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Quoridor
{
    public class Cell
    {
        public bool IsPlayable { get; set; }
        public Point Position { get; set; }
        public Size Size { get; protected set; }

        private List<Cell> neighbours = new List<Cell>();

        protected Brush PrimaryBrush = Brushes.BurlyWood;
        protected Brush HoverBrush = Brushes.SandyBrown;

        public Cell(int x, int y, int width, int height, bool isPlayable)
        {
            Position = new Point(x, y);
            Size = new Size(width, height);
            IsPlayable = isPlayable;
        }

        public bool ContainsPoint(Point point)
        {
            return point.X > Position.X && point.Y > Position.Y && point.X < Position.X + Size.Width && point.Y < Position.Y + Size.Height;
        }

        public void AddNeighbour(Cell neighbour)
        {
            neighbours.Add(neighbour);
        }
        
        public void RemoveNeighbour(Cell neighbour)
        {
            neighbours.Remove(neighbour);
        }

        public bool HasNeighbour(Cell cell)
        {
            return neighbours.Contains(cell);
        }

        public void Draw(Graphics g)
        {
            if (ContainsPoint(Input.MousePosition) && IsPlayable) 
                g.FillRectangle(HoverBrush, Position.X, Position.Y, Size.Width, Size.Height);
            else 
                g.FillRectangle(PrimaryBrush, Position.X, Position.Y, Size.Width, Size.Height);
        }
    }
}