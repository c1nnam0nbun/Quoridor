using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Quoridor
{
    public static class GameManager
    {
        private static Form Window { get; set; }
        private static bool IsGameStarted { get; set; }

        private static Board Board { get; set; }

        private static Cell[,] cells = new Cell[11, 9];
        private static Wall[,] walls = new Wall[2, 10];
        private static Point[,] anchorPoints = new Point[9, 9];

        private static List<Point> BannedPointsV { get; } = new List<Point>();
        private static List<Point> BannedPointsH { get; } = new List<Point>();

        private static Player PlayerOne { get; set; }
        private static Player PlayerTwo { get; set; }
        private static Player ActivePlayer { get; set; }

        private static int TurnCount { get; set; }

        public static void Init(Form window)
        {
            Window = window;
            Board = new Board(window.ClientSize);
        }

        public static void GameStart(Graphics g)
        {
            if (IsGameStarted) return;
            Size s = new Size(300, 100);
            Point p = new Point(Window.Size.Width / 2 - s.Width / 2, Window.Size.Height / 2 - s.Height / 2);
            PopupMessage msg = new PopupMessage(p, s, "Place your players", () => IsGameStarted = true);
            msg.Show(g);
        }

        public static void GameUpdate()
        {
            if (!IsGameStarted) return;
            if (Input.IsKeyDown(Keys.F5))
            {
                GameReset();
                return;
            }
            
            ChangeTurn();

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Cell cell = cells[i, j];
                    if (!cell.IsPlayable) break;
                    if (cell.ContainsPoint(Input.MousePosition) && Input.IsMouseButtonDown(MouseButtons.Left))
                    {
                        if (PlayerOne == null)
                        {
                            if (i <= 1)
                            {
                                PlayerOne = new Player(cell, Brushes.DarkRed);
                                for (int k = 0; k < 10; k++) PlayerOne.Walls[k] = walls[0, k];
                            }
                        }

                        if (PlayerTwo == null)
                        {
                            if (i >= 9)
                            {
                                PlayerTwo = new Player(cell, Brushes.Navy);
                                for (int k = 0; k < 10; k++) PlayerTwo.Walls[k] = walls[1, k];
                            }
                        }
                    }
                }
            }

            if (PlayerOne == null || PlayerTwo == null || ActivePlayer == null) return;
            foreach (Cell cell in cells) cell.Update();
            if (PlayerOne == null || PlayerTwo == null || ActivePlayer == null) return;
            
            foreach (Wall wall in ActivePlayer.Walls) wall.Update();
            if (Wall.ActiveWall == null) return;
            if (Wall.ActiveWall.IsHorizontal())
            {
                Point point = FindClosestHorizontal();
                if (!BannedPointsH.Contains(point))
                {
                    Wall.ActiveWall.Position = point;
                }
            }
            else
            {
                Point point = FindClosestVertical();
                if (!BannedPointsV.Contains(point))
                {
                    Wall.ActiveWall.Position = point;
                }
            }
        }

        private static void GameReset()
        {
            PlayerOne = null;
            PlayerTwo = null;
            foreach (Wall wall in walls) wall.Reset();
            TurnCount = 0;
            ActivePlayer = null;
            BannedPointsH.Clear();
            BannedPointsV.Clear();
            IsGameStarted = false;
        }
        
        private static void ChangeTurn()
        {
            ActivePlayer = TurnCount % 2 == 0 ? PlayerOne : PlayerTwo;
        }

        private static void OnCellPressed(Cell cell)
        {
            if (ActivePlayer == null) return;
            if (Wall.ActiveWall == null && ActivePlayer.Move(cell))
            {
                if (PlayerOne.CurrentCell.Index.X == 9)
                {
                    GameReset();
                    return;
                }

                if (PlayerTwo.CurrentCell.Index.X == 1)
                {
                    GameReset();
                    return;
                }
                TurnCount++;
            }
        }

        public static void GameDraw(Graphics g)
        {
            Board.Draw(g);
            PlayerOne?.Draw(g);
            PlayerTwo?.Draw(g);
        }

        public static ref Cell[,] CreateCells()
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (i == 0 || i == 10) cells[i, j] = new Cell(new Point(i, j), false);
                    else cells[i, j] = new Cell(new Point(i, j));
                    cells[i, j].PressCallback = OnCellPressed;
                }
            }

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Cell cell = cells[i, j];
                    if (i > 0) cell.AddNeighbour(cells[i - 1, j]);
                    if (i < 10) cell.AddNeighbour(cells[i + 1, j]);
                    if (j > 0) cell.AddNeighbour(cells[i, j - 1]);
                    if (j < 8) cell.AddNeighbour(cells[i, j + 1]);
                }
            }

            return ref cells;
        }

        public static ref Wall[,] CreateWalls()
        {
            for (int i = 0; i < 10; i++)
            {
                walls[0, i] = new Wall(OnWallPlaced);
                walls[1, i] = new Wall(OnWallPlaced);
            }

            return ref walls;
        }

        public static ref Point[,] CreateAnchorPoints(Cell[,] boardCells)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Cell c = boardCells[i, j];
                    anchorPoints[i, j] = new Point(c.Position.X + c.Size.Width, c.Position.Y - 10);
                }
            }

            return ref anchorPoints;
        }

        private static void OnWallPlaced(Wall wall)
        {
            if (ActivePlayer != null && ActivePlayer.Walls.Contains(wall)) TurnCount++;
            if (wall.IsHorizontal())
            {
                void BanPointsH()
                {
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            Point p = anchorPoints[i, j];
                            if (p != wall.Position) continue;
                            if (!BannedPointsH.Contains(anchorPoints[i, j])) BannedPointsH.Add(anchorPoints[i, j]);
                            if (!BannedPointsH.Contains(anchorPoints[i + 1, j]))
                                BannedPointsH.Add(anchorPoints[i + 1, j]);
                            if (i != 0 && !BannedPointsH.Contains(anchorPoints[i - 1, j]))
                                BannedPointsH.Add(anchorPoints[i - 1, j]);
                            if (!BannedPointsV.Contains(anchorPoints[i + 1, j - 1]))
                                BannedPointsV.Add(anchorPoints[i + 1, j - 1]);

                            cells[i + 1, j].RemoveNeighbour(cells[i + 1, j - 1]);
                            cells[i + 1, j - 1].RemoveNeighbour(cells[i + 1, j]);
                            cells[i + 2, j].RemoveNeighbour(cells[i + 2, j - 1]);
                            cells[i + 2, j - 1].RemoveNeighbour(cells[i + 2, j]);
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
                            if (p != wall.Position) continue;
                            if (!BannedPointsV.Contains(anchorPoints[i, j])) BannedPointsV.Add(anchorPoints[i, j]);
                            if (!BannedPointsV.Contains(anchorPoints[i, j + 1]))
                                BannedPointsV.Add(anchorPoints[i, j + 1]);
                            if (j != 0 && !BannedPointsV.Contains(anchorPoints[i, j - 1]))
                                BannedPointsV.Add(anchorPoints[i, j - 1]);
                            if (!BannedPointsH.Contains(anchorPoints[i - 1, j + 1]))
                                BannedPointsH.Add(anchorPoints[i - 1, j + 1]);

                            cells[i, j].RemoveNeighbour(cells[i + 1, j]);
                            cells[i + 1, j].RemoveNeighbour(cells[i, j]);
                            cells[i, j + 1].RemoveNeighbour(cells[i + 1, j + 1]);
                            cells[i + 1, j + 1].RemoveNeighbour(cells[i, j + 1]);
                            return;
                        }
                    }
                }

                BanPointsV();
            }
        }
        
        private static Point FindClosestHorizontal()
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
        
        private static Point FindClosestVertical()
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
        
        private static double DistSquared(Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }
    }
}