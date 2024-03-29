﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Quoridor
{
    public class Cell
    {
        public bool IsPlayable { get; }
        public Point Position { get; set; }
        public Size Size { get; set; }
        public Point Index { get; }

        public Action<Cell> PressCallback { get; set; }
        public List<Cell> Neighbours { get; } = new List<Cell>();
        public bool IsTaken { get; set; }

        private Brush PrimaryBrush { get; } = Brushes.BurlyWood;
        private Brush HoverBrush { get; } = Brushes.SandyBrown;

        public Cell(Point index, bool isPlayable = true)
        {
            Index = index;
            IsPlayable = isPlayable;
        }

        public void Update()
        {
            if (!IsPlayable) return;
            if (ContainsPoint(Input.MousePosition) && Input.IsMouseButtonDown(MouseButtons.Left)) PressCallback?.Invoke(this);
        }

        private bool ContainsPoint(Point point)
        {
            return point.X > Position.X && point.Y > Position.Y && point.X < Position.X + Size.Width && point.Y < Position.Y + Size.Height;
        }

        public void AddNeighbour(Cell neighbour)
        {
            Neighbours.Add(neighbour);
        }
        
        public void RemoveNeighbour(Cell neighbour)
        {
            Neighbours.Remove(neighbour);
        }

        public bool HasNeighbour(Cell cell)
        {
            return Neighbours.Contains(cell);
        }

        public void Draw(Graphics g)
        {
            if (ContainsPoint(Input.MousePosition) && IsPlayable) 
                g.FillRectangle(HoverBrush, Position.X, Position.Y, Size.Width, Size.Height);
            else 
                g.FillRectangle(PrimaryBrush, Position.X, Position.Y, Size.Width, Size.Height);
        }
    }
}