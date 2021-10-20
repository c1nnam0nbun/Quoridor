using System;
using System.Drawing;

namespace Quoridor
{
    public static class Rng
    {
        private static readonly Random _rng = new Random();
        public static int GetRandomIntInRange(int min, int max)
        {
            return _rng.Next(min, max);
        }
    }
}