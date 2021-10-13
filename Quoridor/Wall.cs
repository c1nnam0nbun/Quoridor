using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quoridor
{
    public class Wall
    {
        public static Wall ActiveWall;
        
        private bool isSelected;
        private Point defaultPosition;
        private Size defaultSize;
        private Size selectedSize;

        private bool rotationLocked;
        public bool IsPlaced { get; private set; }
        public Point Position { get; set; }
        public Size Size { get; set; }

        private Action<Wall> placedCallback;

        private Brush PrimaryBrush = Brushes.DarkRed;
        private Brush HoverBrush = Brushes.Crimson;

        /*public Wall(int x, int y, int width, int height)
        {
            Position = new Point(x, y);
            Size = new Size(width, height);
            defaultPosition = Position;
            defaultSize = Size;
            selectedSize = new Size(Size.Width + 30, Size.Height);
        }*/

        public Wall(Action<Wall> placedCallback)
        {
            this.placedCallback = placedCallback;
        }

        public void SetDefaultPosition(Point position)
        {
            Position = position;
            defaultPosition = position;
        }

        public void SetDefaultSize(Size size)
        {
            Size = size;
            defaultSize = size;
            selectedSize = new Size(Size.Width + 30, Size.Height);
        }

        public void Update()
        {
            if ((ActiveWall != null && ActiveWall != this) || IsPlaced) return;
            
            if (Input.IsMouseButtonDown(MouseButtons.Left) && ContainsPoint(Input.MousePosition))
            {
                isSelected = true;
                ActiveWall = this;
            }
            if (!Input.IsMouseButtonDown(MouseButtons.Left))
            {
                isSelected = false;
                ActiveWall = null;
            }

            if (isSelected)
            {
                Position = Input.MousePosition;
                Size = selectedSize;

                if (Input.IsMouseButtonDown(MouseButtons.Right))
                {
                    Reset();
                }
                
                if (Input.IsKeyDown(Keys.R))
                {
                    if (!rotationLocked)
                    {
                        selectedSize = new Size(selectedSize.Height, selectedSize.Width);
                        rotationLocked = true;
                    }
                
                }
            }
            else
            {
                if (Position != defaultPosition)
                {
                    IsPlaced = true;
                    placedCallback?.Invoke(this);
                    PrimaryBrush = HoverBrush;
                }
            }
            
            if (!Input.IsKeyDown(Keys.R)) rotationLocked = false;
        }

        public void Reset()
        {
            Position = defaultPosition;
            Size = defaultSize;
            selectedSize = new Size(Size.Width + 30, Size.Height);
            isSelected = false;
            IsPlaced = false;
            rotationLocked = false;
            PrimaryBrush = Brushes.DarkRed;
            HoverBrush = Brushes.Crimson;
            ActiveWall = null;
        }

        public void Draw(Graphics g)
        {
            if (ContainsPoint(Input.MousePosition)) g.FillRectangle(HoverBrush, Position.X, Position.Y, Size.Width, Size.Height);
            else g.FillRectangle(ActiveWall == this ? HoverBrush : PrimaryBrush, Position.X, Position.Y, Size.Width, Size.Height);
        }

        public bool IsHorizontal()
        {
            return Size.Width > Size.Height;
        }
        
        private bool ContainsPoint(Point point)
        {
            return point.X > Position.X && point.Y > Position.Y && point.X < Position.X + Size.Width && point.Y < Position.Y + Size.Height;
        }
        
    }
}