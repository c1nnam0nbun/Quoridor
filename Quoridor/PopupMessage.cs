using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quoridor
{
    public class PopupMessage
    {
        public static void ShowChooseModeMessage(Point position, Size size, Action vsPlayerCallback, Action vsComputerCallback, Graphics g)
        {
            g.FillRectangle(Brushes.Chocolate, position.X, position.Y, size.Width, size.Height);
            g.DrawString("Choose mode", new Font(FontFamily.GenericMonospace, 20), Brushes.Black, position.X + 50, position.Y);
            
            Point p = new Point(position.X + 10, position.Y + size.Height - 110);
            Size s = new Size(size.Width - 20, 40);
            g.FillRectangle(Brushes.Peru, p.X, p.Y, s.Width, s.Height);
            g.DrawString("Player vs Player", new Font(FontFamily.GenericMonospace, 18), Brushes.Black, p.X , p.Y + 5);

            if (Input.MousePosition.X > p.X && Input.MousePosition.Y > p.Y &&
                Input.MousePosition.X < p.X + s.Width && Input.MousePosition.Y < p.Y + s.Height &&
                Input.IsMouseButtonDown(MouseButtons.Left))
            {
                vsPlayerCallback();
            }
            
            p = new Point(position.X + 10, position.Y + size.Height - 60);
            s = new Size(size.Width - 20, 40);
            g.FillRectangle(Brushes.Peru, p.X, p.Y, s.Width, s.Height);
            g.DrawString("Player vs Computer", new Font(FontFamily.GenericMonospace, 18), Brushes.Black, p.X , p.Y + 5);

            if (Input.MousePosition.X > p.X && Input.MousePosition.Y > p.Y &&
                Input.MousePosition.X < p.X + s.Width && Input.MousePosition.Y < p.Y + s.Height &&
                Input.IsMouseButtonDown(MouseButtons.Left))
            {
                vsComputerCallback();
            }
        }

        public static void ShowBeginGameMessage(Point position, Size size, Action okCallback, Graphics g)
        {
            g.FillRectangle(Brushes.Chocolate, position.X, position.Y, size.Width, size.Height);
            g.DrawString("Place your player", new Font(FontFamily.GenericMonospace, 20), Brushes.Black, position.X, position.Y);
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