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
                PopulationArray[i].GenerateRandomPath();
            }

            this.numberOfPaths = numberOfPaths;
            this.numberOfCities = numberOfCities;
            bestPathInPopulation = new Path(numberOfCities);
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


        public void CrossoverPopulation(int crossoverChance)
        {
            Path[] newPopulationArray = new Path[numberOfPaths];
            for (int i = 0; i < numberOfPaths; ++i)
            {
                newPopulationArray[i] = new Path(numberOfCities);
            }

            for (int k = 0; k < PopulationArray.Length; k++)
            {
                int individual1 = k;
                int individual2 = Program.r.Next(0, numberOfPaths);

                if (Program.r.Next(0, 100) < crossoverChance && !PopulationArray[individual1].PathCities.SequenceEqual(PopulationArray[individual2].PathCities))
                {

                    Array.Copy(CrossoverPaths(PopulationArray[individual1].PathCities, PopulationArray[individual2].PathCities), newPopulationArray[k].PathCities, newPopulationArray[k].PathCities.Length);
                }

                else
                {
                    Array.Copy(PopulationArray[k].PathCities, newPopulationArray[k].PathCities, PopulationArray[k].PathCities.Length);
                }
            }

            Array.Copy(newPopulationArray, PopulationArray, newPopulationArray.Length);
        }

        int[] CrossoverPaths(int[] path1, int[] path2)
        {
            int pos1 = r.Next(0, path1.Length);
            int pos2 = r.Next(0, path1.Length);

            if (pos1 > pos2)
            {
                int foo1 = pos2;
                pos2 = pos1;
                pos1 = foo1;
            }

            int[] tab1 = new int[pos2 - pos1];
            int[] newPath = new int[path1.Length];

            for (int p = pos1, x = 0; p < pos2; p++, x++)
            {
                tab1[x] = path1[p];
            }

            for (int p = 0; p < path1.Length; p++)
            {
                newPath[p] = -1;
            }

            for (int p = pos1; p < pos2; p++)
            {
                newPath[p] = path1[p];
            }

            int foo = 0;
            for (int p = 0; p < path1.Length;)
            {
                if (newPath[p] == -1)
                {
                    if (!tab1.Contains(path2[foo]))
                    {
                        newPath[p] = path2[foo];
                        p++;
                    }
                    foo++;
                }
                else p++;
            }
            return newPath;
        }


        public void Mutation(int mutationChance)
        {
            for (int k = 0; k < PopulationArray.Length; k++)
            {
                if (Program.r.Next(0, 100) < mutationChance)
                {
                    PopulationArray[k].MutatePath();
                }
            }
        }


        public void TournamentSelection(int startPrinting)
        {
            Path[] newPopulationArray = new Path[numberOfPaths];
            for (int i = 0; i < numberOfPaths; ++i)
            {
                newPopulationArray[i] = new Path(numberOfCities);
            }

            int[] fitnessArray = new int[numberOfPaths];

            for (int k = 0; k < numberOfPaths; k++)
            {
                fitnessArray[k] = PopulationArray[k].CalculateFitness();
                if (fitnessArray[k] < lengthofBestPath)
                {
                    if (startPrinting > 5000)
                    {
                        PopulationArray[k].PrintPath();
                    }
                    Array.Copy(PopulationArray[k].PathCities, bestPathInPopulation.PathCities, PopulationArray[k].PathCities.Length);
                    lengthofBestPath = fitnessArray[k];
                    Console.WriteLine();
                }
            }

            for (int k = 0; k < numberOfPaths; k++)
            {
                int tournamentWinner;

                int individual1 = r.Next(0, numberOfPaths);
                int individual2 = r.Next(0, numberOfPaths);
                int individual3 = r.Next(0, numberOfPaths);

                int sumDistancePath1 = fitnessArray[individual1];
                int sumDistancePath2 = fitnessArray[individual2];
                int sumDistancePath3 = fitnessArray[individual3];

                if (sumDistancePath1 <= sumDistancePath2 && sumDistancePath1 <= sumDistancePath3) tournamentWinner = individual1;
                else if (sumDistancePath2 <= sumDistancePath1 && sumDistancePath2 <= sumDistancePath3) tournamentWinner = individual2;
                else tournamentWinner = individual3;

                Array.Copy(PopulationArray[tournamentWinner].PathCities, newPopulationArray[k].PathCities, PopulationArray[k].PathCities.Length);
            }

            Array.Copy(newPopulationArray, PopulationArray, newPopulationArray.Length);
        }

        public void RouletteSelection()
        {
            Path[] newPopulationArray = new Path[numberOfPaths];
            for (int i = 0; i < numberOfPaths; ++i)
            {
                newPopulationArray[i] = new Path(numberOfCities);
            }

            double[] fitnessArray = new double[numberOfPaths];
            double fitnessSum = 0;

            for (int k = 0; k < numberOfPaths; k++)
            {
                fitnessArray[k] = PopulationArray[k].CalculateFitness();
                fitnessSum += fitnessArray[k];
                if (fitnessArray[k] < lengthofBestPath)
                {
                    PopulationArray[k].PrintPath();
                    Array.Copy(PopulationArray[k].PathCities, bestPathInPopulation.PathCities, PopulationArray[k].PathCities.Length);
                    lengthofBestPath = int.Parse(fitnessArray[k].ToString());
                    Console.WriteLine();
                }
            }

            for (int k = 0; k < numberOfPaths; k++)
            {
                fitnessArray[k] = (fitnessArray[k] / fitnessSum) * numberOfPaths * 100;
            }

            for (int k = 0; k < numberOfPaths;)
            {
                int chance = Program.r.Next(60, 100);
                int randomed = Program.r.Next(0, numberOfPaths);

                if (fitnessArray[randomed] < chance)
                {
                    Array.Copy(PopulationArray[randomed].PathCities, newPopulationArray[k].PathCities, PopulationArray[k].PathCities.Length);
                    k++;
                }

            }
            Array.Copy(newPopulationArray, PopulationArray, newPopulationArray.Length);
        }

    }
}
