using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Quoridor
{
    public class Board
    {
        private const int CELL_SIDE = 70;
        private const int OFFSET = 10;
        private const int WIDTH = 11;
        private const int HEIGHT = 9;

        private readonly Cell[,] cells;
        private readonly Wall[,] walls;
        private readonly Point[,] anchorPoints;

        private Size Size;
        private Point Location;
        
        public Board(Size size)
        {
            int totalTakenSpaceHeight = HEIGHT * CELL_SIDE + (HEIGHT - 1) * OFFSET;
            int freeSpaceHeight = size.Height - totalTakenSpaceHeight;
            int marginTop = freeSpaceHeight / 2;

            int totalTakenSpaceWidth = 13 * CELL_SIDE + (WIDTH - 1) * OFFSET;
            int freeSpaceWidth = size.Width - totalTakenSpaceWidth;
            int marginLeft = freeSpaceWidth / 2;

            Size = new Size(totalTakenSpaceWidth, totalTakenSpaceHeight);
            Location = new Point(marginLeft, marginTop);
            
            cells = GameManager.CreateCells();
            PositionCells();

            walls = GameManager.CreateWalls();
            PositionWalls();
            
            anchorPoints = GameManager.CreateAnchorPoints(cells);
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.SaddleBrown, Location.X, Location.Y, Size.Width, Size.Height);
            g.DrawRectangle(new Pen(Color.FromArgb(102, 65, 35), 10), Location.X - 5, Location.Y - 5, Size.Width + 10, Size.Height + 10);
            foreach (Cell cell in cells) cell.Draw(g);
            /*foreach (Point point in anchorPoints)
            {
                g.FillEllipse(Brushes.White, point.X, point.Y, 10, 10);
            }*/
            foreach (Wall wall in walls) wall?.Draw(g);
        }
        
        private void PositionCells()
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    switch (i)
                    {
                        case 0:
                            cells[i, j].Position = new Point(i * CELL_SIDE + i * OFFSET + Location.X, j * CELL_SIDE + j * OFFSET + Location.Y);
                            cells[i, j].Size = new Size(CELL_SIDE * 2, CELL_SIDE);
                            break;
                        case 10:
                            cells[i, j].Position = new Point(i * CELL_SIDE + CELL_SIDE + i * OFFSET + Location.X, j * CELL_SIDE + j * OFFSET + Location.Y);
                            cells[i, j].Size = new Size(CELL_SIDE * 2, CELL_SIDE);
                            break;
                        default:
                            cells[i, j].Position = new Point(i * CELL_SIDE + CELL_SIDE + i * OFFSET + Location.X, j * CELL_SIDE + j * OFFSET + Location.Y);
                            cells[i, j].Size = new Size(CELL_SIDE, CELL_SIDE);
                            break;
                    }
                }
            }
        }

        private void PositionWalls()
        {
            for (int i = 0; i < 10; i++)
            {
                walls[0, i].SetDefaultPosition(new Point(Location.X, i * CELL_SIDE + i * OFFSET + Location.Y - 10));
                walls[0, i].SetDefaultSize(new Size(CELL_SIDE * 2, 10));
                walls[1, i].SetDefaultPosition(new Point(Location.X + CELL_SIDE * 11 + OFFSET * 10, i * CELL_SIDE + i * OFFSET + Location.Y - 10));
                walls[1, i].SetDefaultSize(new Size(CELL_SIDE * 2, 10));
            }
        }
    }
}