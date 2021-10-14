using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Quoridor
{
    public class Board
    {
        private const int CellSide = 70;
        private const int Offset = 10;
        private const int Width = 11;
        private const int Height = 9;

        public Cell[,] Cells { get; }
        private Wall[,] Walls { get; }
        private Point[,] AnchorPoints { get; }

        private Size BoardSize { get; }
        private Point Location { get; }

        public Board(Size size)
        {
            int totalTakenSpaceHeight = Height * CellSide + (Height - 1) * Offset;
            int freeSpaceHeight = size.Height - totalTakenSpaceHeight;
            int marginTop = freeSpaceHeight / 2;

            int totalTakenSpaceWidth = 13 * CellSide + (Width - 1) * Offset;
            int freeSpaceWidth = size.Width - totalTakenSpaceWidth;
            int marginLeft = freeSpaceWidth / 2;

            BoardSize = new Size(totalTakenSpaceWidth, totalTakenSpaceHeight);
            Location = new Point(marginLeft, marginTop);
            
            Cells = GameManager.CreateCells();
            PositionCells();

            Walls = GameManager.CreateWalls();
            PositionWalls();
            
            AnchorPoints = GameManager.CreateAnchorPoints(Cells);
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.SaddleBrown, Location.X, Location.Y, BoardSize.Width, BoardSize.Height);
            g.DrawRectangle(new Pen(Color.FromArgb(102, 65, 35), 10), Location.X - 5, Location.Y - 5, BoardSize.Width + 10, BoardSize.Height + 10);
            foreach (Cell cell in Cells) cell.Draw(g);
            /*foreach (Point point in anchorPoints)
            {
                g.FillEllipse(Brushes.White, point.X, point.Y, 10, 10);
            }*/
            foreach (Wall wall in Walls) wall?.Draw(g);
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
                            Cells[i, j].Position = new Point(i * CellSide + i * Offset + Location.X, j * CellSide + j * Offset + Location.Y);
                            Cells[i, j].Size = new Size(CellSide * 2, CellSide);
                            break;
                        case 10:
                            Cells[i, j].Position = new Point(i * CellSide + CellSide + i * Offset + Location.X, j * CellSide + j * Offset + Location.Y);
                            Cells[i, j].Size = new Size(CellSide * 2, CellSide);
                            break;
                        default:
                            Cells[i, j].Position = new Point(i * CellSide + CellSide + i * Offset + Location.X, j * CellSide + j * Offset + Location.Y);
                            Cells[i, j].Size = new Size(CellSide, CellSide);
                            break;
                    }
                }
            }
        }

        private void PositionWalls()
        {
            for (int i = 0; i < 10; i++)
            {
                Walls[0, i].SetDefaultPosition(new Point(Location.X, i * CellSide + i * Offset + Location.Y - 10));
                Walls[0, i].SetDefaultSize(new Size(CellSide * 2, 10));
                Walls[1, i].SetDefaultPosition(new Point(Location.X + CellSide * 11 + Offset * 10, i * CellSide + i * Offset + Location.Y - 10));
                Walls[1, i].SetDefaultSize(new Size(CellSide * 2, 10));
            }
        }
    }
}