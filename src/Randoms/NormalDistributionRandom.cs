/// Name: SosielVariables.cs
/// Description:
/// Authors: Multiple.
/// Last updated: July 10th, 2020.
/// Copyright: Garry Sotnik

using System;

namespace SOSIEL.Randoms
{
    public class NormalDistributionRandom
    {
        private static NormalDistributionRandom random;

        double _mean;
        double _stdDev;

        public double Next()
        {
            return Next(_mean, _stdDev);
        }


        public double Next(double mean, double stdDev)
        {
            Random r = LinearUniformRandom.GetInstance;

            double u1 = 1 - r.NextDouble();
            double u2 = 1 - r.NextDouble();

            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);


            return mean + stdDev * randStdNormal;
        }

        public static NormalDistributionRandom GetInstance
        {
            get
            {
                if (random == null)
                    random = new NormalDistributionRandom(0.3, 0.3);

                return random;
            }
        }

        private NormalDistributionRandom(double mean, double stdDev)
        {
            _mean = mean;
            _stdDev = stdDev;
        }

    }
}
