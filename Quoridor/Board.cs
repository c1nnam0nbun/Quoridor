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

        private readonly Cell[,] cells = new Cell[11, 9];
        private readonly Wall[] walls = new Wall[20];
        private readonly Point[,] anchorPoints = new Point[9,9];
        private readonly List<Point> bannedPointsV = new List<Point>();
        private readonly List<Point> bannedPointsH = new List<Point>();

        private Size Size;
        private Point Location;

        private Player player;
        
        public Board(Form form)
        {
            int totalTakenSpaceHeight = HEIGHT * CELL_SIDE + (HEIGHT - 1) * OFFSET;
            int freeSpaceHeight = form.ClientSize.Height - totalTakenSpaceHeight;
            int marginTop = freeSpaceHeight / 2;

            int totalTakenSpaceWidth = 13 * CELL_SIDE + (WIDTH - 1) * OFFSET;
            int freeSpaceWidth = form.ClientSize.Width - totalTakenSpaceWidth;
            int marginLeft = freeSpaceWidth / 2;

            Size = new Size(totalTakenSpaceWidth, totalTakenSpaceHeight);
            Location = new Point(marginLeft, marginTop);
            
            CreateCells();
            CreateWalls();
            CreateAnchorPoints();

            player = new Player(cells[1, 0]);
        }

        public void Update()
        {
            foreach (Wall wall in walls) wall.Update();
            if (Wall.ActiveWall == null)
            {
                foreach (Wall w in walls)
                {
                    if (!w.IsPlaced) continue;
                    if (w.IsHorizontal())
                    {
                        void BanPointsH()
                        {
                            for (int i = 0; i < 9; i++)
                            {
                                for (int j = 0; j < 9; j++)
                                {
                                    Point p = anchorPoints[i, j];
                                    if (p != w.Position) continue;
                                    if (!bannedPointsH.Contains(anchorPoints[i, j])) bannedPointsH.Add(anchorPoints[i, j]);
                                    if (!bannedPointsH.Contains(anchorPoints[i + 1, j])) bannedPointsH.Add(anchorPoints[i + 1, j]);
                                    if (i != 0 && !bannedPointsH.Contains(anchorPoints[i - 1, j]))
                                            bannedPointsH.Add(anchorPoints[i - 1, j]);
                                    if (!bannedPointsV.Contains(anchorPoints[i + 1, j - 1])) 
                                        bannedPointsV.Add(anchorPoints[i + 1, j - 1]);
                                    
                                    cells[i+1,j].RemoveNeighbour(cells[i+1,j-1]);
                                    cells[i+1,j-1].RemoveNeighbour(cells[i+1,j]);
                                    cells[i+2,j].RemoveNeighbour(cells[i+2,j-1]);
                                    cells[i+2,j-1].RemoveNeighbour(cells[i+2,j]);
                                    return;
                                }
                            }
                        }
                        BanPointsH();
                    }
                    else
                    {
                        void BanPointsV()
                        {
                            for (int i = 0; i < 9; i++)
                            {
                                for (int j = 0; j < 9; j++)
                                {
                                    Point p = anchorPoints[i, j];
                                    if (p != w.Position) continue;
                                    if (!bannedPointsV.Contains(anchorPoints[i, j])) bannedPointsV.Add(anchorPoints[i, j]);
                                    if (!bannedPointsV.Contains(anchorPoints[i, j + 1]))bannedPointsV.Add(anchorPoints[i, j + 1]);
                                    if (j != 0 && !bannedPointsV.Contains(anchorPoints[i, j - 1])) bannedPointsV.Add(anchorPoints[i, j - 1]);
                                    if (!bannedPointsH.Contains(anchorPoints[i - 1, j + 1])) bannedPointsH.Add(anchorPoints[i - 1, j + 1]);
                                    
                                    cells[i,j].RemoveNeighbour(cells[i+1,j]);
                                    cells[i+1,j].RemoveNeighbour(cells[i,j]);
                                    cells[i,j+1].RemoveNeighbour(cells[i+1,j+1]);
                                    cells[i+1,j+1].RemoveNeighbour(cells[i,j+1]);
                                    return;
                                }
                            }
                        }
                        BanPointsV();
                    }
                }

                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        Cell cell = cells[i, j];
                        if (!cell.IsPlayable) break;
                        if (cell.ContainsPoint(Input.MousePosition) && Input.IsMouseButtonDown(MouseButtons.Left))
                        {
                            player.Move(cell);
                        }
                    }
                }
            }
            else
            {
                if (Wall.ActiveWall.IsHorizontal())
                {
                    Point point = FindClosestHorizontal();
                    if (!bannedPointsH.Contains(point))
                    {
                        Wall.ActiveWall.Position = point;
                    }
                }
                else
                {
                    Point point = FindClosestVertical();
                    if (!bannedPointsV.Contains(point))
                    {
                        Wall.ActiveWall.Position = point;
                    }
                }
            }
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.SaddleBrown, Location.X, Location.Y, Size.Width, Size.Height);
            g.DrawRectangle(new Pen(Color.FromArgb(102, 65, 35), 10), Location.X - 5, Location.Y - 5, Size.Width + 10, Size.Height + 10);
            foreach (Cell cell in cells) cell.Draw(g);
            foreach (Point point in anchorPoints)
            {
                g.FillEllipse(Brushes.White, point.X, point.Y, 10, 10);
            }
            foreach (Wall wall in walls) wall?.Draw(g);
            player.Draw(g);
        }

        private void CreateCells()
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    switch (i)
                    {
                        case 0:
                            cells[i,j] = new Cell(i * CELL_SIDE + i * OFFSET + Location.X, j * CELL_SIDE + j * OFFSET + Location.Y, CELL_SIDE * 2, CELL_SIDE, false);
                            break;
                        case 10:
                            cells[i,j] = new Cell(i * CELL_SIDE + CELL_SIDE + i * OFFSET + Location.X, j * CELL_SIDE + j * OFFSET + Location.Y, CELL_SIDE * 2, CELL_SIDE, false);
                            break;
                        default:
                            cells[i,j] = new Cell(i * CELL_SIDE + CELL_SIDE + i * OFFSET + Location.X, j * CELL_SIDE + j * OFFSET + Location.Y, CELL_SIDE, CELL_SIDE, true);
                            break;
                    }
                }
            }

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Cell cell = cells[i, j];
                    if (i > 0) cell.AddNeighbour(cells[i-1, j]);
                    if (i < 10) cell.AddNeighbour(cells[i+1, j]); 
                    if (j > 0) cell.AddNeighbour(cells[i, j-1]);
                    if (j < 8) cell.AddNeighbour(cells[i, j+1]);
                }
            }
        }

        private void CreateWalls()
        {
            for (int i = 0; i < 10; i++)
            {
                walls[i] = new Wall(Location.X, i * CELL_SIDE + i * OFFSET + Location.Y - 10, CELL_SIDE * 2, 10);
                walls[i + 10] = new Wall(Location.X + CELL_SIDE * 11 + OFFSET * 10, i * CELL_SIDE + i * OFFSET + Location.Y - 10, CELL_SIDE * 2, 10);
            }
        }
        
        private void CreateAnchorPoints()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Cell c = cells[i, j];
                    anchorPoints[i, j] = new Point(c.Position.X + c.Size.Width, c.Position.Y - 10);
                }
            }
        }

        private double DistSquared(Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }

        private Point FindClosestHorizontal()
        {
            Point closest = new Point(int.MaxValue, int.MaxValue);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    Point p = anchorPoints[i, j];
                    if (DistSquared(Input.MousePosition, p) < DistSquared(Input.MousePosition, closest))
                        closest = p;
                }
            }

            return closest;
        }
        
        private Point FindClosestVertical()
        {
            Point closest = new Point(int.MaxValue, int.MaxValue);
            for (int i = 1; i < 9; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Point p = anchorPoints[i, j];
                    if (DistSquared(Input.MousePosition, p) < DistSquared(Input.MousePosition, closest))
                        closest = p;
                }
            }

            return closest;
        }
    }
}