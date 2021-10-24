using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Quoridor
{
    public static class Pathfinder
    {
        /*public static bool CheckIfPathExistsForLeftPlayer(Player player)
        {
            List<Cell> visited = new List<Cell>();
            List<Cell> path = new List<Cell>();
            Cell current = player.CurrentCell;

            while (current.Index.X != 9)
            {
                visited.Add(current);
                path.Add(current);
                bool isFound = false;
                
                foreach (Cell cell in current.Neighbours.Where(cell => !visited.Contains(cell) && !cell.IsTaken && cell.IsPlayable).Where(cell => cell.Index.X > current.Index.X))
                {
                    current = cell;
                    path.Add(cell);
                    isFound = true;
                }

                if (isFound) continue;
                
                foreach (Cell cell in current.Neighbours.Where(cell => !visited.Contains(cell) && !cell.IsTaken && cell.IsPlayable).Where(cell => cell.Index.Y > current.Index.Y))
                {
                    current = cell;
                    path.Add(cell);
                    isFound = true;
                }
                
                if (isFound) continue;

                foreach (Cell cell in current.Neighbours.Where(cell => !visited.Contains(cell) && !cell.IsTaken && cell.IsPlayable).Where(cell => cell.Index.X < current.Index.X))
                {
                    current = cell;
                    path.Add(cell); 
                    isFound = true;
                }

                if (isFound) continue;

                foreach (Cell cell in current.Neighbours.Where(cell => !visited.Contains(cell) && !cell.IsTaken && cell.IsPlayable).Where(cell => cell.Index.Y < current.Index.Y))
                {
                    current = cell;
                    path.Add(cell); 
                    isFound = true;
                }

                if (isFound) continue;

                for (int i = path.Count - 1; i >= 0; i--)
                {
                    Cell btc = path[i];
                    path.RemoveAt(i);
                    foreach (Cell cell in btc.Neighbours)
                    {
                        if (!visited.Contains(cell) && cell.IsPlayable)
                        {
                            current = cell;
                            path.Add(cell);
                            isFound = true;
                            break;
                        }

                        if (isFound) break;
                    }
                }
                if (isFound) continue;
                
                return false;
            }
            return true;
        }
        
        public static bool CheckIfPathExistsForRightPlayer(Player player)
        {
            List<Cell> visited = new List<Cell>();
            List<Cell> path = new List<Cell>();
            Cell current = player.CurrentCell;

            while (current.Index.X != 1)
            {
                visited.Add(current);
                path.Add(current);
                bool isFound = false;
                
                foreach (Cell cell in current.Neighbours.Where(cell => !visited.Contains(cell) && !cell.IsTaken && cell.IsPlayable).Where(cell => cell.Index.X < current.Index.X))
                {
                    current = cell;
                    path.Add(cell);
                    isFound = true;
                }

                if (isFound) continue;
                
                foreach (Cell cell in current.Neighbours.Where(cell => !visited.Contains(cell) && !cell.IsTaken && cell.IsPlayable).Where(cell => cell.Index.Y > current.Index.Y))
                {
                    current = cell;
                    path.Add(cell);
                    isFound = true;
                }
                
                if (isFound) continue;

                foreach (Cell cell in current.Neighbours.Where(cell => !visited.Contains(cell) && !cell.IsTaken && cell.IsPlayable).Where(cell => cell.Index.X > current.Index.X))
                {
                    current = cell;
                    path.Add(cell);
                    isFound = true;
                }

                if (isFound) continue;

                foreach (Cell cell in current.Neighbours.Where(cell => !visited.Contains(cell) && !cell.IsTaken && cell.IsPlayable).Where(cell => cell.Index.Y < current.Index.Y))
                {
                    current = cell;
                    path.Add(cell);
                    isFound = true;
                }

                if (isFound) continue;

                for (int i = path.Count - 1; i >= 0; i--)
                {
                    Cell btc = path[i];
                    path.RemoveAt(i);
                    foreach (Cell cell in btc.Neighbours)
                    {
                        if (!visited.Contains(cell) && cell.IsPlayable)
                        {
                            current = cell;
                            path.Add(cell);
                            isFound = true;
                            break;
                        }

                        if (isFound) break;
                    }
                }

                if (isFound) continue;
                
                return false;
            }
            return true;
        }*/

        private static List<Cell> _openSet = new List<Cell>();

        private static Dictionary<Cell, int> _gScore = new Dictionary<Cell, int>();
        private static Dictionary<Cell, int> _fScore = new Dictionary<Cell, int>();

        private static Dictionary<Cell, Cell> _cameFrom = new Dictionary<Cell, Cell>();

        public static bool FindPath(Cell start, int endIndex)
        {
            for (int i = 0; i < 9; i++)
            {
                _openSet.Clear();
                _gScore.Clear();
                _fScore.Clear();
                _cameFrom.Clear();
                if (AStarPathfinder(start, new Point(endIndex, i))) return true;
            }
            return false;
        }

        private static bool AStarPathfinder(Cell start, Point goal)
        {
            _openSet.Add(start);
            _gScore.Add(start, 0);
            _fScore.Add(start, Heuristic(start.Index, goal));

            while (_openSet.Count > 0)
            {
                int lowest = 0;
                for (int i = 0; i < _openSet.Count; i++)
                {
                    if (!_fScore.ContainsKey(_openSet[lowest])) _fScore.Add(_openSet[lowest], int.MaxValue);
                    if (_fScore[_openSet[i]] < _fScore[_openSet[lowest]]) lowest = i;
                }

                Cell current = _openSet[lowest];
                if (current.Index == goal)
                {
                    List<Cell> path = new List<Cell>();
                    path.Add(current);
                    while (_cameFrom.ContainsKey(current))
                    {
                        current = _cameFrom[current];
                        path.Add(current);
                    }
                    return true;
                }

                _openSet.Remove(current);
                foreach (Cell neighbour in current.Neighbours)
                {
                    if (!neighbour.IsPlayable) continue;
                    int tempG = _gScore[current] + 1;
                    if (!_gScore.ContainsKey(neighbour)) _gScore.Add(neighbour, int.MaxValue);
                    if (tempG < _gScore[neighbour])
                    {
                        _cameFrom[neighbour] = current;
                        _gScore[neighbour] = tempG;
                        _fScore[neighbour] = _gScore[neighbour] + Heuristic(neighbour.Index, goal);
                        if (!_openSet.Contains(neighbour)) _openSet.Add(neighbour);
                    }
                }
            }
            
            return false;
        }

        private static int Heuristic(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}