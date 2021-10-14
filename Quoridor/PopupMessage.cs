using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quoridor
{
    public class PopupMessage
    {
        private string Text { get; }
        private Point Position { get; }
        private Size Size { get; }
        private Action OkCallback { get; }

        public PopupMessage(Point position, Size size, String text, Action okCallback)
        {
            this.Text = text;
            this.Position = position;
            this.Size = size;
            this.OkCallback = okCallback;
        }

        public void Show(Graphics g)
        {
            g.FillRectangle(Brushes.Chocolate, Position.X, Position.Y, Size.Width, Size.Height);
            g.DrawString(Text, new Font(FontFamily.GenericMonospace, 20), Brushes.Black, Position.X, Position.Y);
            Point p = new Point(Position.X + Size.Width / 2 - 40, Position.Y + Size.Height - 40);
            Size s = new Size(80, 30);
            g.FillRectangle(Brushes.Peru, p.X, p.Y, s.Width, s.Height);

            if (Input.MousePosition.X > p.X && Input.MousePosition.Y > p.Y &&
                Input.MousePosition.X < p.X + s.Width && Input.MousePosition.Y < p.Y + s.Height &&
                Input.IsMouseButtonDown(MouseButtons.Left))
            {
                OkCallback();
            }
        }
    }
}