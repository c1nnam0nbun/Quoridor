using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quoridor
{
    public class PopupMessage
    {
        private String text;
        private Point position;
        private Size size;
        private Action okCallback;
        
        public PopupMessage(Point position, Size size, String text, Action okCallback)
        {
            this.text = text;
            this.position = position;
            this.size = size;
            this.okCallback = okCallback;
        }

        public void Show(Graphics g)
        {
            g.FillRectangle(Brushes.Chocolate, position.X, position.Y, size.Width, size.Height);
            g.DrawString(text, new Font(FontFamily.GenericMonospace, 20), Brushes.Black, position.X, position.Y);
            Point p = new Point(position.X + size.Width / 2 - 40, position.Y + size.Height - 40);
            Size s = new Size(80, 30);
            g.FillRectangle(Brushes.Peru, p.X, p.Y, s.Width, s.Height);

            if (Input.MousePosition.X > p.X && Input.MousePosition.Y > p.Y &&
                Input.MousePosition.X < p.X + s.Width && Input.MousePosition.Y < p.Y + s.Height &&
                Input.IsMouseButtonDown(MouseButtons.Left))
            {
                okCallback();
            }
        }
    }
}