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

        public static int GetDesiredIndex(int min, int max)
        {
            return rng.Next(min, max);
        }
    }
}