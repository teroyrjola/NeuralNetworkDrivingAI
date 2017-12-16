using System;
using System.Globalization;

namespace Assets.Scripts
{
    public class HelperFunc
    {
        private static readonly Random Random = new Random();

        public static double RandomWeight()
        {
            return Random.NextDouble() * 2 - 1;
        }

        public static double SigmoidFunction(double value)
        {
            if (value > 10) return 1.0;
            else if (value < -10) return -1.0;
            else return 1.0 / (1.0 + Math.Exp(-value));
        }

        public static float ParseTimeFromGUI(string time)
        {
            try
            {
                float bestTime = float.Parse(time.Remove(0, 11), CultureInfo.InvariantCulture);
                return bestTime;
            }
            catch (FormatException e)
            {
                return 100f;
            }
        }
    }
}