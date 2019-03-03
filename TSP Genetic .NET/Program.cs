using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP_Genetic.NET
{
    class Program
    {
        static string filePath = "C:\\gr120.txt";
        public static int[,] distancesArray = FileReaderDistances.ReadDistancesFromFile(filePath);
        public static int numberOfCities = FileReaderDistances.NumberOfCities(filePath);
        public static Random r = new Random();

        static void Main(string[] args)
        {
            int individualsInGeneration = 80;
            int mutationChance = 55;
            int crossoverChance = 40;
            int numberOfLoops = 100000;

            Population population = new Population(individualsInGeneration, numberOfCities);

            for (int i = 0; i < numberOfLoops; i++)
            {
                population.CrossoverPopulation(crossoverChance);
                population.Mutation(mutationChance);
                population.TournamentSelection(10000);
                //population.RouletteSelection();

                if (i % 10000 == 0) Console.WriteLine("Loop: " + i);
            }
            Console.ReadLine();

        }


    }
}
