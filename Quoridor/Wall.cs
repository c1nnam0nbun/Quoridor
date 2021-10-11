using System.Drawing;
using System.Windows.Forms;

namespace Quoridor
{
    public class Wall : Cell
    {
        public static Wall ActiveWall;
        
        private bool isSelected;
        private Point defaultPosition;
        private Size defaultSize;
        private Size selectedSize;

        private bool rotationLocked;
        public bool IsPlaced { get; private set; }

        public Wall(int x, int y, int width, int height) : base(x, y, width, height, true)
        {
            PrimaryBrush = Brushes.DarkRed;
            HoverBrush = Brushes.Crimson;
            defaultPosition = Position;
            defaultSize = Size;
            selectedSize = new Size(Size.Width + 30, Size.Height);
        }

        public new void Update()
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
                    Position = defaultPosition;
                    Size = defaultSize;
                    selectedSize = new Size(Size.Width + 30, Size.Height);
                    isSelected = false;
                    ActiveWall = null;
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
                    PrimaryBrush = HoverBrush;
                }
            }
            
            if (!Input.IsKeyDown(Keys.R)) rotationLocked = false;
        }

        public new void Draw(Graphics g)
        {
            if (ActiveWall == this)
                g.FillRectangle(HoverBrush, Position.X, Position.Y, Size.Width, Size.Height);
            else 
                base.Draw(g);
        }

        public bool IsHorizontal()
        {
            return Size.Width > Size.Height;
        }
        
        public bool IsVertical()
        {
            return Size.Width < Size.Height;
        }
    }
}