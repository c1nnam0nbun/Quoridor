using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Quoridor
{
    public static class GameManager
    {
        private static Form window;
        private static bool isGameStarted;

        private static Board board;

        private static Cell[,] cells = new Cell[11, 9];
        private static Wall[,] walls = new Wall[2, 10];
        private static Point[,] anchorPoints = new Point[9, 9];

        private static readonly List<Point> bannedPointsV = new List<Point>();
        private static readonly List<Point> bannedPointsH = new List<Point>();

        private static Player playerOne, playerTwo, activePlayer;

        private static int turnCount;

        public static void Init(Form window)
        {
            GameManager.window = window;
            board = new Board(window.ClientSize);
        }

        public static void GameStart(Graphics g)
        {
            if (isGameStarted) return;
            Size s = new Size(300, 100);
            Point p = new Point(window.Size.Width / 2 - s.Width / 2, window.Size.Height / 2 - s.Height / 2);
            PopupMessage msg = new PopupMessage(p, s, "Place your players", () => isGameStarted = true);
            msg.Show(g);
        }

        public static void GameUpdate()
        {
            if (!isGameStarted) return;
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
                        if (playerOne == null)
                        {
                            if (i <= 1)
                            {
                                playerOne = new Player(cell, Brushes.DarkRed);
                                for (int k = 0; k < 10; k++) playerOne.Walls[k] = walls[0, k];
                            }
                        }

                        if (playerTwo == null)
                        {
                            if (i >= 9)
                            {
                                playerTwo = new Player(cell, Brushes.Navy);
                                for (int k = 0; k < 10; k++) playerTwo.Walls[k] = walls[1, k];
                            }
                        }
                    }
                }
            }

            if (playerOne == null || playerTwo == null || activePlayer == null) return;
            foreach (Cell cell in cells) cell.Update();
            if (playerOne == null || playerTwo == null || activePlayer == null) return;
            
            foreach (Wall wall in activePlayer.Walls) wall.Update();
            if (Wall.ActiveWall == null) return;
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

        private static void GameReset()
        {
            playerOne = null;
            playerTwo = null;
            foreach (Wall wall in walls) wall.Reset();
            turnCount = 0;
            activePlayer = null;
            bannedPointsH.Clear();
            bannedPointsV.Clear();
            isGameStarted = false;
        }
        
        private static void ChangeTurn()
        {
            activePlayer = turnCount % 2 == 0 ? playerOne : playerTwo;
        }

        private static void OnCellPressed(Cell cell)
        {
            if (activePlayer == null) return;
            if (Wall.ActiveWall == null && activePlayer.Move(cell))
            {
                if (playerOne.currentCell.Index.X == 9)
                {
                    GameReset();
                    return;
                }

                if (playerTwo.currentCell.Index.X == 1)
                {
                    GameReset();
                    return;
                }
                turnCount++;
            }
        }

        public static void GameDraw(Graphics g)
        {
            board.Draw(g);
            playerOne?.Draw(g);
            playerTwo?.Draw(g);
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
            if (activePlayer != null && activePlayer.Walls.Contains(wall)) turnCount++;
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
                            if (!bannedPointsH.Contains(anchorPoints[i, j])) bannedPointsH.Add(anchorPoints[i, j]);
                            if (!bannedPointsH.Contains(anchorPoints[i + 1, j]))
                                bannedPointsH.Add(anchorPoints[i + 1, j]);
                            if (i != 0 && !bannedPointsH.Contains(anchorPoints[i - 1, j]))
                                bannedPointsH.Add(anchorPoints[i - 1, j]);
                            if (!bannedPointsV.Contains(anchorPoints[i + 1, j - 1]))
                                bannedPointsV.Add(anchorPoints[i + 1, j - 1]);

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
                            if (!bannedPointsV.Contains(anchorPoints[i, j])) bannedPointsV.Add(anchorPoints[i, j]);
                            if (!bannedPointsV.Contains(anchorPoints[i, j + 1]))
                                bannedPointsV.Add(anchorPoints[i, j + 1]);
                            if (j != 0 && !bannedPointsV.Contains(anchorPoints[i, j - 1]))
                                bannedPointsV.Add(anchorPoints[i, j - 1]);
                            if (!bannedPointsH.Contains(anchorPoints[i - 1, j + 1]))
                                bannedPointsH.Add(anchorPoints[i - 1, j + 1]);

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