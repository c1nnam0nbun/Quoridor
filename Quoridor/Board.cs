﻿using System;
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
        
        private readonly Cell[] cells = new Cell[99];
        private readonly Wall[] walls = new Wall[20];
        private readonly Point[] anchorPoints = new Point[81];
        private readonly List<Point> takenPoints = new List<Point>();

        private Size Size;
        private Point Location;
        
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
        }

        public void Update()
        {
            foreach (Cell cell in cells) cell.Update();
            foreach (Wall wall in walls) wall.Update();
            if (Wall.ActiveWall == null)
            {
                foreach (Wall w in walls)
                {
                    int count = 0;
                    foreach (Point p in anchorPoints)
                    {
                        Point temp = new Point(p.X + 1, p.Y + 1);
                        if (w.ContainsPoint(temp) && count != 2 && !takenPoints.Contains(p))
                        {
                            takenPoints.Add(p);
                            count++;
                        }
                    }
                }
                return;
            }
            
            if (Wall.ActiveWall.Size.Width > Wall.ActiveWall.Size.Height)
            {
                Point point = FindClosestHorizontal();
                /*bool isTaken = false;
                foreach (Wall w in walls)
                {
                    Point temp = new Point(point.X + 1, point.Y + 1);
                    if (!w.ContainsPoint(temp)) continue;
                    isTaken = true;
                    break;
                }*/

                if (!takenPoints.Contains(point))
                {
                    Wall.ActiveWall.Position = point;
                    /*int count = 0;
                    foreach (Point p in anchorPoints)
                    {
                        Point temp = new Point(p.X + 1, p.Y + 1);
                        if (Wall.ActiveWall.ContainsPoint(temp) && count != 2)
                        {
                            takenPoints.Add(p);
                            count++;
                        }
                    }*/
                }
            }
            else
            {
                Wall.ActiveWall.Position = FindClosestVertical();
            }

            if (Wall.ActiveWall == null)
            {
                foreach (Wall w in walls)
                {
                    int count = 0;
                    foreach (Point p in anchorPoints)
                    {
                        Point temp = new Point(p.X + 1, p.Y + 1);
                        if (Wall.ActiveWall.ContainsPoint(temp) && count != 2)
                        {
                            takenPoints.Add(p);
                            count++;
                        }
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
                            cells[i + j * 11] = new Cell(i * CELL_SIDE + i * OFFSET + Location.X, j * CELL_SIDE + j * OFFSET + Location.Y, CELL_SIDE * 2, CELL_SIDE, false);
                            break;
                        case 10:
                            cells[i + j * 11] = new Cell(i * CELL_SIDE + CELL_SIDE + i * OFFSET + Location.X, j * CELL_SIDE + j * OFFSET + Location.Y, CELL_SIDE * 2, CELL_SIDE, false);
                            break;
                        default:
                            cells[i + j * 11] = new Cell(i * CELL_SIDE + CELL_SIDE + i * OFFSET + Location.X, j * CELL_SIDE + j * OFFSET + Location.Y, CELL_SIDE, CELL_SIDE, true);
                            break;
                    }
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
            int off = 0;
            int index = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int idx = j + i * 10 + off;
                    Cell c = cells[idx];
                    if (i == 0)
                    {
                        anchorPoints[index] = new Point(c.Position.X + c.Size.Width, c.Position.Y - 10);
                        anchorPoints[index + 9] = new Point(c.Position.X + c.Size.Width, c.Position.Y + c.Size.Height);
                    }
                    else
                        anchorPoints[index + 9] = new Point(c.Position.X + c.Size.Width, c.Position.Y + c.Size.Height);
                    index++;
                }

                off += 1;
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
            int offset = 1;
            for (int i = 0; i < anchorPoints.Length; i++)
            {
                if (i < 9) continue;
                if (i == 8 + offset * 9)
                {
                    offset++;
                    continue;
                }
                Point point = anchorPoints[i];
                if (DistSquared(Input.MousePosition, point) < DistSquared(Input.MousePosition, closest))
                    closest = point;
            }

            return closest;
        }
        
        private Point FindClosestVertical()
        {
            Point closest = new Point(int.MaxValue, int.MaxValue);
            int offset = 0;
            for (int i = 0; i < anchorPoints.Length; i++)
            {
                /*if (i < 9 /*|| i == 7 + offset * 9#1#) continue;*/
                if (i == 0 || i > 81 - 9) continue;
                if (i == 9 + offset * 9)
                {
                    offset++;
                    continue;
                }
                Point point = anchorPoints[i];
                if (DistSquared(Input.MousePosition, point) < DistSquared(Input.MousePosition, closest))
                    closest = point;
            }

            return closest;
        }
    }
}