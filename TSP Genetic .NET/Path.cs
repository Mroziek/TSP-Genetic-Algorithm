using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP_Genetic.NET
{
   public class Path
    {
        public int[] PathCities;       

        public Path(int numberOfCities)
        {
            PathCities = new int[numberOfCities];

            bool[] isDrawn = new bool[numberOfCities];
            int randomCity;

            for (int n = 0; n < numberOfCities;)
            {
                randomCity = Program.r.Next(0, numberOfCities);

                if (isDrawn[randomCity] == false)
                {
                    isDrawn[randomCity] = true;
                    PathCities[n] = randomCity;
                    n++;
                }
            }
        }

        public Path(int[] cities)
        {
            PathCities = cities;
        }

        public Path Copy() => new Path(PathCities);     

        public int CalculateFitness()
        {
            int sumDistancePath = 0;

            for (int n = 0; n < PathCities.Length - 1; n++)
            {
                sumDistancePath += Program.distancesArray[PathCities[n], PathCities[n + 1]];
            }
            sumDistancePath += Program.distancesArray[PathCities[PathCities.Length - 1], PathCities[0]];

            return sumDistancePath;
        }

        public void PrintPath()
        {
            foreach (int city in PathCities)
            {
                Console.Write(city+"-");
            }
            Console.Write(CalculateFitness());
        }

        public void MutatePath()
        {
            int gen1 = Program.r.Next(0, Program.numberOfCities);
            int gen2 = Program.r.Next(0, Program.numberOfCities);
          
            if (gen1 > gen2)
            {
                int foo = gen2;
                gen2 = gen1;
                gen1 = foo;
            }

            int[] tab1 = new int[gen2 - gen1];

            for (int p = gen1, x = 0; p < gen2; p++, x++)
            {
                tab1[x] = PathCities[p];
            }

            Array.Reverse(tab1);

            for (int p = gen1, x = 0; p < gen2; p++, x++)
            {
                PathCities[p] = tab1[x];
            }

        }
    }
}
    
