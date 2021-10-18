using System;
using System.Drawing;

namespace Quoridor
{
    public class ComputerPlayer : Player
    {
        private static Random rng = new Random();
        
        public ComputerPlayer(Cell initialCell, Brush brush) : base(initialCell, brush)
        {
        }

        public static int GetDesiredCellIndex()
        {
            return rng.Next(0, 8);
        }

        public new bool Move(Cell cell)
        {
            return false;
        }
    }
}