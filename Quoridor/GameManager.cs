using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Quoridor
{
    public static class GameManager
    {
        public static Form Window { get; private set; }
        
        public static GameState GameState { get; set; } = new GameStartedState();
        public static TurnState TurnState { get; set; }
        

        private static Board Board { get; set; }

        private static Cell[,] _cells = new Cell[11, 9];
        private static Wall[,] _walls = new Wall[2, 10];
        private static Point[,] _anchorPoints = new Point[9, 9];

        private static List<Point> BannedPointsV { get; } = new List<Point>();
        private static List<Point> BannedPointsH { get; } = new List<Point>();

        public static Player PlayerOne { get; private set; }
        public static Player PlayerTwo { get; private set; }

        private static readonly Point[] NeighboursRemovedCurrentTurn = new Point[4];
        
        public static Cell PressedCell { get; private set; }
        public static Wall PlacedWall { get; private set; }

        public static void Init(Form window)
        {
            Window = window;
            Board = new Board(window.ClientSize);
        }
        
        public static void GameUpdate()
        {
            GameState.Update();
            TurnState?.Update();
            
            TurnState?.MakeMove();
            
            GameState.CheckWin();

            if (Input.IsKeyDown(Keys.F5))
            {
                GameReset();
                return;
            }
            
            foreach (Cell cell in _cells) cell?.Update();
            if (PlayerOne == null || PlayerTwo == null) return;

            if (Wall.ActiveWall != null)
            {
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
        }

        public static void PlacePlayerTwo(Cell cell)
        {
            PlayerTwo = new Player(cell, Brushes.Navy);
            for (int k = 0; k < 10; k++) PlayerTwo.Walls[k] = _walls[1, k];
            TurnState.ChangeTurn();
        }
        
        public static void PlacePlayerOne(Cell cell)
        {
            PlayerOne = new Player(cell, Brushes.DarkRed);
            for (int k = 0; k < 10; k++) PlayerOne.Walls[k] = _walls[0, k];
            TurnState.ChangeTurn();
        }

        public static void GameReset()
        {
            PlayerOne = null;
            PlayerTwo = null;
            foreach (Wall wall in _walls) wall.Reset();
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Cell cell = _cells[i, j];
                    cell.IsTaken = false;
                    if (i > 0 && !cell.HasNeighbour(_cells[i - 1, j])) cell.AddNeighbour(_cells[i - 1, j]);
                    if (i < 10 && !cell.HasNeighbour(_cells[i + 1, j])) cell.AddNeighbour(_cells[i + 1, j]);
                    if (j > 0 && !cell.HasNeighbour(_cells[i, j - 1])) cell.AddNeighbour(_cells[i, j - 1]);
                    if (j < 8 && !cell.HasNeighbour(_cells[i, j + 1])) cell.AddNeighbour(_cells[i, j + 1]);
                }
            }
            BannedPointsH.Clear();
            BannedPointsV.Clear();
            
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (i >= 8 || j <= 0) BannedPointsH.Add(_anchorPoints[i, j]);
                    if (i <= 0 || j >= 8) BannedPointsV.Add(_anchorPoints[i, j]);
                }
            }

            PlacedWall = null;
            
            GameState = new GameStartedState();
            TurnState = null;
        }

        private static void OnCellPressed(Cell cell)
        {
            PressedCell = cell;
        }

        public static void GameDraw(Graphics g)
        {
            Board.Draw(g);
            GameState?.Draw(g);
            PlayerOne?.Draw(g);
            PlayerTwo?.Draw(g);
        }

        public static ref Cell[,] CreateCells()
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (i == 0 || i == 10) _cells[i, j] = new Cell(new Point(i, j), false);
                    else _cells[i, j] = new Cell(new Point(i, j));
                    _cells[i, j].PressCallback = OnCellPressed;
                }
            }

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Cell cell = _cells[i, j];
                    if (i > 0) cell.AddNeighbour(_cells[i - 1, j]);
                    if (i < 10) cell.AddNeighbour(_cells[i + 1, j]);
                    if (j > 0) cell.AddNeighbour(_cells[i, j - 1]);
                    if (j < 8) cell.AddNeighbour(_cells[i, j + 1]);
                }
            }

            return ref _cells;
        }

        public static Cell GetCellAt(int i, int j)
        {
            return _cells[i, j];
        }

        public static ref Wall[,] CreateWalls()
        {
            for (int i = 0; i < 10; i++)
            {
                _walls[0, i] = new Wall(OnWallPlaced);
                _walls[1, i] = new Wall(OnWallPlaced);
            }

            return ref _walls;
        }

        public static ref Point[,] CreateAnchorPoints(Cell[,] boardCells)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Cell c = boardCells[i, j];
                    _anchorPoints[i, j] = new Point(c.Position.X + c.Size.Width, c.Position.Y - 10);
                    if (i >= 8 || j <= 0) BannedPointsH.Add(_anchorPoints[i, j]);
                    if (i <= 0 || j >= 8) BannedPointsV.Add(_anchorPoints[i, j]);
                }
            }

            return ref _anchorPoints;
        }

        public static List<Point> GetAvailablePointsHorizontal()
        {
            return _anchorPoints.Cast<Point>().Where(point => !BannedPointsH.Contains(point)).ToList();
        }
        
        public static List<Point> GetAvailablePointsVertical()
        {
            return _anchorPoints.Cast<Point>().Where(point => !BannedPointsV.Contains(point)).ToList();
        }

        public static void OnWallPlaced(Wall wall)
        {
            if (wall.IsHorizontal())
            {
                TryRemoveNeighboursHorizontal(wall);
                if (Pathfinder.CheckIfPathExistsForLeftPlayer(PlayerOne) && Pathfinder.CheckIfPathExistsForRightPlayer(PlayerTwo))
                {
                    BanPointsHorizontal(wall);
                    PlacedWall = wall;
                    //TurnState.ChangeTurn();
                }
                else
                {
                    RestoreNeighbours();
                    wall.Reset();
                }
            }
            else
            {
                TryRemoveNeighboursVertical(wall);
                if (Pathfinder.CheckIfPathExistsForLeftPlayer(PlayerOne) && Pathfinder.CheckIfPathExistsForRightPlayer(PlayerTwo))
                {
                    BanPointsVertical(wall);
                    PlacedWall = wall;
                    //TurnState.ChangeTurn();
                }
                else
                {
                    RestoreNeighbours();
                    wall.Reset();
                }
            }
        }

        private static void RestoreNeighbours()
        {
            _cells[NeighboursRemovedCurrentTurn[0].X, NeighboursRemovedCurrentTurn[0].Y].AddNeighbour(_cells[NeighboursRemovedCurrentTurn[1].X, NeighboursRemovedCurrentTurn[1].Y]);
            _cells[NeighboursRemovedCurrentTurn[1].X, NeighboursRemovedCurrentTurn[1].Y].AddNeighbour(_cells[NeighboursRemovedCurrentTurn[0].X, NeighboursRemovedCurrentTurn[0].Y]);
            _cells[NeighboursRemovedCurrentTurn[2].X, NeighboursRemovedCurrentTurn[2].Y].AddNeighbour(_cells[NeighboursRemovedCurrentTurn[3].X, NeighboursRemovedCurrentTurn[3].Y]);
            _cells[NeighboursRemovedCurrentTurn[3].X, NeighboursRemovedCurrentTurn[3].Y].AddNeighbour(_cells[NeighboursRemovedCurrentTurn[2].X, NeighboursRemovedCurrentTurn[2].Y]);
        }

        private static void TryRemoveNeighboursHorizontal(Wall wall)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Point p = _anchorPoints[i, j];
                    if (p != wall.Position) continue;

                    NeighboursRemovedCurrentTurn[0] = new Point(i + 1, j);
                    NeighboursRemovedCurrentTurn[1] = new Point(i + 1, j - 1);
                    NeighboursRemovedCurrentTurn[2] = new Point(i + 2, j);
                    NeighboursRemovedCurrentTurn[3] = new Point(i + 2, j - 1);

                    _cells[i + 1, j].RemoveNeighbour(_cells[i + 1, j - 1]);
                    _cells[i + 1, j - 1].RemoveNeighbour(_cells[i + 1, j]);
                    _cells[i + 2, j].RemoveNeighbour(_cells[i + 2, j - 1]);
                    _cells[i + 2, j - 1].RemoveNeighbour(_cells[i + 2, j]);
                    return;
                }
            }
        }
        
        private static void TryRemoveNeighboursVertical(Wall wall)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Point p = _anchorPoints[i, j];
                    if (p != wall.Position) continue;
                    
                    NeighboursRemovedCurrentTurn[0] = new Point(i, j);
                    NeighboursRemovedCurrentTurn[1] = new Point(i + 1, j);
                    NeighboursRemovedCurrentTurn[2] = new Point(i, j + 1);
                    NeighboursRemovedCurrentTurn[3] = new Point(i + 1, j + 1);

                    _cells[i, j].RemoveNeighbour(_cells[i + 1, j]);
                    _cells[i + 1, j].RemoveNeighbour(_cells[i, j]);
                    _cells[i, j + 1].RemoveNeighbour(_cells[i + 1, j + 1]);
                    _cells[i + 1, j + 1].RemoveNeighbour(_cells[i, j + 1]);
                    return;
                }
            }
        }
        
        private static void BanPointsHorizontal(Wall wall)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Point p = _anchorPoints[i, j];
                    if (p != wall.Position) continue;
                    if (!BannedPointsH.Contains(_anchorPoints[i, j])) BannedPointsH.Add(_anchorPoints[i, j]);
                    if (!BannedPointsH.Contains(_anchorPoints[i + 1, j]))
                        BannedPointsH.Add(_anchorPoints[i + 1, j]);
                    if (i != 0 && !BannedPointsH.Contains(_anchorPoints[i - 1, j]))
                        BannedPointsH.Add(_anchorPoints[i - 1, j]);
                    if (!BannedPointsV.Contains(_anchorPoints[i + 1, j - 1]))
                        BannedPointsV.Add(_anchorPoints[i + 1, j - 1]);
                            
                    return;
                }
            }
        }

        private static void BanPointsVertical(Wall wall)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Point p = _anchorPoints[i, j];
                    if (p != wall.Position) continue;
                    if (!BannedPointsV.Contains(_anchorPoints[i, j])) BannedPointsV.Add(_anchorPoints[i, j]);
                    if (!BannedPointsV.Contains(_anchorPoints[i, j + 1]))
                        BannedPointsV.Add(_anchorPoints[i, j + 1]);
                    if (j != 0 && !BannedPointsV.Contains(_anchorPoints[i, j - 1]))
                        BannedPointsV.Add(_anchorPoints[i, j - 1]);
                    if (!BannedPointsH.Contains(_anchorPoints[i - 1, j + 1]))
                        BannedPointsH.Add(_anchorPoints[i - 1, j + 1]);

                    return;
                }
            }
        }
        
        private static Point FindClosestHorizontal()
        {
            Point closest = new Point(int.MaxValue, int.MaxValue);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    Point p = _anchorPoints[i, j];
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
                    Point p = _anchorPoints[i, j];
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