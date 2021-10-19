using System;
using System.Drawing;

namespace Quoridor
{
    public class RNG
    {
        private static Random rng = new Random();
        public static int GetRandonIntInRange(int min, int max)
        {
            return rng.Next(min, max);
        }
    }
}