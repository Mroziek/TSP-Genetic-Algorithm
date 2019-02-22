using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP_Genetic.NET
{
    class Program
    {
        static string filePath = "C:\\berlin52.txt";
        public static int[,] distancesArray = FileReaderDistances.ReadDistancesFromFile(filePath);
        public static int numberOfCities = FileReaderDistances.NumberOfCities(filePath);
        public static Random r = new Random();


        static void Main(string[] args)
        {
            int individualsInGeneration = 50;
            int mutationChance = 30;
            int crossoverChance = 50;
            int numberOfLoops = 100000;


            Population population = new Population(individualsInGeneration, numberOfCities);
            //population.PrintPopulation();

            for (int i = 0; i < numberOfLoops; i++)
            {
                population.Crossover(crossoverChance);
                population.Mutation(mutationChance);
                population.TournamentSelection();

                if (population.BestPathRefresh())
                {
                    Console.WriteLine(population.lengthofBestPath + " w pętli nr: " + i);
                    //population.PrintPopulation();
                }
            }
            Console.ReadLine();

        }


    }
}
