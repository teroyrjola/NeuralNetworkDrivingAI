using System;

namespace Assets.Scripts
{
    public class HelperFunc
    {
        private static readonly Random Random = new Random();
        public static double RandomWeight()
        {
            return Random.NextDouble() * 2 - 1;
        }
    }
}