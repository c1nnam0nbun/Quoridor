using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Quoridor
{
    public class Pathfinder
    {
        public static bool CheckIfPathExistsForLeftPlayer(Player player)
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
        }
    }
}