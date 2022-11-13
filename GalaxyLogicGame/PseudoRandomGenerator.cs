using System;
namespace GalaxyLogicGame
{
    public class PseudoRandomGenerator
    {
        /**
         * This is the simplest implementation of RNG
         * 
         * inspiration: https://jamesmccaffrey.wordpress.com/2019/05/20/a-pseudo-pseudo-random-number-generator/
         * 
         * The same implementation will be used on the server and smartcontracts, that will cross check the game
         */

        private double seed;

        public PseudoRandomGenerator(int seed)
        {
            this.seed = seed + 0.5;
        }

        public double Next()
        {
            double x = Math.Sin(this.seed) * 100 + 0.3;
            double result = Math.Round(x - Math.Floor(x), 3);  // (0.0,1.0)
            this.seed = result;  // for next call
            return result;
        }

        public int Next(int max)
        {
            double x = Math.Sin(this.seed) * 100 + 0.3;
            double result = Math.Round(x - Math.Floor(x), 3);  // [0.0,1.0)
            this.seed = result;  // for next call
            return (int)(result * max);
        }

        public int Next(int min, int max)
        {
            double x = Math.Sin(this.seed) * 100 + 0.3;
            double result = Math.Round(x - Math.Floor(x), 3);  // [0.0,1.0)
            this.seed = result;  // for next call
            max = max - min;
            return (int)(result * max) + min;
        }

    }
}

