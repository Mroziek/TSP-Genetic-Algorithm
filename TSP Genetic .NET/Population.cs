using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP_Genetic.NET
{
    class Population
    {
        public Path[] PopulationArray;
        int numberOfPaths;
        public int numberOfCities;
        public static Random r = new Random();

        Path bestPathInPopulation;
        public int lengthofBestPath = int.MaxValue;

        public Population(int numberOfPaths, int numberOfCities)
        {
            PopulationArray = new Path[numberOfPaths];
            for (int i = 0; i < numberOfPaths; ++i)
            {
                PopulationArray[i] = new Path(numberOfCities);
            }

            this.numberOfPaths = numberOfPaths;
            this.numberOfCities = numberOfCities;
        }


        public void PrintPopulation()
        {
            foreach (Path path in PopulationArray)
            {
                path.PrintPath();
                Console.WriteLine();
            }
            Console.WriteLine();
        }


        public void Crossover(int crossoverChance)
        {
            Path[] newPopulationArray = new Path[numberOfPaths];

            for (int k = 0; k < numberOfPaths; k++)
            {
                var winner = PopulationArray[k].Copy();
                newPopulationArray[k] = winner.Copy();

                if (Program.r.Next(0, 100) < crossoverChance)
                {
                    int individual1 = Program.r.Next(0, numberOfPaths);
                    int individual2 = Program.r.Next(0, numberOfPaths);

                    int gen1 = r.Next(0, numberOfCities);
                    int gen2 = r.Next(0, numberOfCities);

                    if (gen1 > gen2)
                    {
                        int foo1 = gen2;
                        gen2 = gen1;
                        gen1 = foo1;
                    }

                    int[] array1 = new int[gen2 - gen1];
                    int[] newPath1 = new int[numberOfCities];

                    for (int p = gen1, x = 0; p < gen2; p++, x++)
                    {
                        array1[x] = PopulationArray[individual1].PathCities[p];
                    }

                    for (int p = 0; p < numberOfCities; p++)
                    {
                        newPath1[p] = -1;
                    }

                    for (int p = gen1; p < gen2; p++)
                    {
                        newPath1[p] = PopulationArray[individual1].PathCities[p];
                    }

                    int foo3 = 0;
                    for (int p = 0; p < numberOfCities;)
                    {
                        if (newPath1[p] == -1)
                        {
                            if (!array1.Contains(PopulationArray[individual2].PathCities[foo3]))
                            {
                                newPath1[p] = PopulationArray[individual2].PathCities[foo3];
                                p++;
                            }
                            foo3++;
                        }
                        else p++;
                    }

                    for (int p = 0; p < numberOfCities; p++)
                    {
                        newPopulationArray[k].PathCities[p] = newPath1[p];
                    }
                }                             
            }

            for (int k = 0; k < numberOfPaths; k++)
            {                
                var winner = newPopulationArray[k].Copy();
                PopulationArray[k] = winner.Copy();
            }
        }


        public void Mutation(int mutationChance)
        {
            for (int k = 0; k < 30; k++)
            {
                if (Program.r.Next(0, 100) < mutationChance)
                {
                    PopulationArray[k].MutatePath();
                }
            }
        }


        public void TournamentSelection()
        {
            Path[] newPopulationArray = new Path[numberOfPaths];

            for (int k = 0; k < numberOfPaths; k++)
            {
                int tournamentWinner;
                var winner = PopulationArray[k].Copy();
                newPopulationArray[k] = winner.Copy();

                int individual1 = r.Next(0, numberOfPaths);
                int individual2 = r.Next(0, numberOfPaths);
                int individual3 = r.Next(0, numberOfPaths);

                int sumDistancePath1 = PopulationArray[individual1].CalculateFitness();
                int sumDistancePath2 = PopulationArray[individual2].CalculateFitness();
                int sumDistancePath3 = PopulationArray[individual3].CalculateFitness();

                if (sumDistancePath1 <= sumDistancePath2 && sumDistancePath1 <= sumDistancePath3) tournamentWinner = individual1;
                else if (sumDistancePath2 <= sumDistancePath1 && sumDistancePath2 <= sumDistancePath3) tournamentWinner = individual2;
                else tournamentWinner = individual3;

                for (int p = 0; p < numberOfCities; p++)
                {
                    newPopulationArray[k].PathCities[p] = PopulationArray[tournamentWinner].PathCities[p];
                }        
            }

            for (int k = 0; k < numberOfPaths; k++)
            {
                for (int p = 0; p < numberOfCities; p++)
                {
                    PopulationArray[k].PathCities[p] = newPopulationArray[k].PathCities[p];
                }
            }
            
        }


        public bool BestPathRefresh()
        {
            bool newBestPathFound = false;
            for (int k = 0; k < numberOfPaths; k++)
            {
                if (PopulationArray[k].CalculateFitness() < lengthofBestPath)
                {
                    bestPathInPopulation=PopulationArray[k];
                    lengthofBestPath = PopulationArray[k].CalculateFitness();
                    newBestPathFound = true;
                }
            }
            return newBestPathFound;
        }

    }
}
