using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Quoridor
{
    public class Pathfinder
    {
        /*public static bool CheckIfPathExists(Player playerOne, Player playerTwo)
        {
            List<Cell> visited = new List<Cell>();
            bool isFound = false;
            
            bool playerOneHasPath = false;
            foreach (Cell cell in playerOne.CurrentCell.Neighbours)
            {
                if (isFound) break;
                if (!cell.IsPlayable) continue;
                if (!visited.Contains(cell)) visited.Add(cell);

                foreach (Cell neighbour in cell.Neighbours)
                {
                    if (isFound) break;
                    if (!visited.Contains(neighbour)) visited.Add(neighbour);
                    
                    if (neighbour.Index.X == 9)
                    {
                        playerOneHasPath = true;
                        isFound = true;
                    }
                }
            }
            
            visited.Clear();
            bool playerTwoHasPath = false;
            foreach (Cell cell in playerTwo.CurrentCell.Neighbours)
            {
                if (!cell.IsPlayable) continue;
                if (!visited.Contains(cell)) visited.Add(cell);

                foreach (Cell neighbour in cell.Neighbours)
                {
                    if (!visited.Contains(neighbour)) visited.Add(neighbour);
                    if (neighbour.Index.X == 1) playerTwoHasPath = true;
                }
            }

            return playerOneHasPath && playerTwoHasPath;
        }*/

        /*public static bool CheckIfPathExists(Cell[,] cells, Player playerOne, Player playerTwo)
        {
            List<Cell> unvisited = new List<Cell>();
            Dictionary<Cell, int> dist = new Dictionary<Cell, int>();
            Dictionary<Cell, Cell> prev = new Dictionary<Cell, Cell>();

            foreach (Cell cell in cells)
            {
                dist.Add(cell, int.MaxValue);
                prev.Add(cell, null);
                unvisited.Add(cell);
            }

            dist[playerOne.CurrentCell] = 0;

            while (unvisited.Count > 0)
            {
                Cell closest = playerOne.CurrentCell;
                foreach (KeyValuePair<Cell,int> pair in dist)
                {
                    if (pair.Value < dist[closest]) closest = pair.Key; break;
                }

                unvisited.Remove(closest);
                foreach (Cell cell in closest.Neighbours)
                {
                    if (!unvisited.Contains(cell)) continue;
                    int alt = dist[closest] + Math.Abs(cell.Index.X - closest.Index.X);
                    if (alt < dist[cell])
                    {
                        dist[cell] = alt;
                        prev[cell] = closest;
                    }
                }
            }
            
            return true;
        }*/

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