using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quoridor
{
    public class Wall
    {
        public static Wall ActiveWall { get; private set; }

        private bool IsSelected { get; set; }
        private Point DefaultPosition { get; set; }
        private Size DefaultSize { get; set; }
        private Size SelectedSize { get; set; }

        private bool IsRotationLocked { get; set; }
        private bool IsPlaced { get; set; }
        public Point Position { get; set; }
        private Size Size { get; set; }

        private Action<Wall> PlacedCallback { get; }

        private Brush PrimaryBrush { get; set; } = Brushes.DarkRed;
        private Brush HoverBrush { get; set; } = Brushes.Crimson;
        
        public Wall(Action<Wall> placedCallback)
        {
            this.PlacedCallback = placedCallback;
        }

        public void SetDefaultPosition(Point position)
        {
            Position = position;
            DefaultPosition = position;
        }

        public void SetDefaultSize(Size size)
        {
            Size = size;
            DefaultSize = size;
            SelectedSize = new Size(Size.Width + 30, Size.Height);
        }

        public void Update()
        {
            if ((ActiveWall != null && ActiveWall != this) || IsPlaced) return;

            if (Input.IsMouseButtonDown(MouseButtons.Left) && ContainsPoint(Input.MousePosition))
            {
                IsSelected = true;
                ActiveWall = this;
            }

            if (!Input.IsMouseButtonDown(MouseButtons.Left))
            {
                IsSelected = false;
                ActiveWall = null;
            }

            if (IsSelected)
            {
                Position = Input.MousePosition;
                Size = SelectedSize;

                if (Input.IsMouseButtonDown(MouseButtons.Right))
                {
                    Reset();
                }

                if (Input.IsKeyDown(Keys.R))
                {
                    if (!IsRotationLocked)
                    {
                        Rotate();
                    }
                }
            }
            else
            {
                if (Position != DefaultPosition)
                {
                    IsPlaced = true;
                    PlacedCallback?.Invoke(this);
                    PrimaryBrush = HoverBrush;
                }
            }

            if (!Input.IsKeyDown(Keys.R)) IsRotationLocked = false;
        }

        public void Reset()
        {
            Position = DefaultPosition;
            Size = DefaultSize;
            SelectedSize = new Size(Size.Width + 30, Size.Height);
            IsSelected = false;
            IsPlaced = false;
            IsRotationLocked = false;
            PrimaryBrush = Brushes.DarkRed;
            HoverBrush = Brushes.Crimson;
            ActiveWall = null;
        }

        public void Draw(Graphics g)
        {
            if (ContainsPoint(Input.MousePosition))
                g.FillRectangle(HoverBrush, Position.X, Position.Y, Size.Width, Size.Height);
            else
                g.FillRectangle(ActiveWall == this ? HoverBrush : PrimaryBrush, Position.X, Position.Y, Size.Width,
                    Size.Height);
        }

        public void Rotate()
        {
            SelectedSize = new Size(SelectedSize.Height, SelectedSize.Width);
            Select();
            IsRotationLocked = true;
        }

        public void Select()
        {
            Size = SelectedSize;
            IsSelected = true;
            PrimaryBrush = HoverBrush;
        }

        public bool IsHorizontal()
        {
            return Size.Width > Size.Height;
        }

        private bool ContainsPoint(Point point)
        {
            return point.X > Position.X && point.Y > Position.Y && point.X < Position.X + Size.Width &&
                   point.Y < Position.Y + Size.Height;
        }
    }
}