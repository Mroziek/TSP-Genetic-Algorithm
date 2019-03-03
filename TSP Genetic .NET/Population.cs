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

                //newPopulationArray[k].PathCities=CrossoverPaths(PopulationArray[individual1].PathCities, PopulationArray[individual2].PathCities);
                // Array.Copy(CrossoverPaths(PopulationArray[individual1].PathCities, PopulationArray[individual2].PathCities), newPopulationArray[k].PathCities, newPopulationArray[k].PathCities.Length);

                if (Program.r.Next(0, 100) < crossoverChance && !PopulationArray[individual1].PathCities.SequenceEqual(PopulationArray[individual2].PathCities))
                {
                    //PopulationArray[k].PrintPath();
                    //Console.WriteLine();
                    //newPopulationArray[k].PrintPath();
                    //Console.WriteLine();
                    Array.Copy(CrossoverPaths(PopulationArray[individual1].PathCities, PopulationArray[individual2].PathCities), newPopulationArray[k].PathCities, newPopulationArray[k].PathCities.Length);
                    //newPopulationArray[k].PrintPath();
                    //Console.WriteLine("xd");
                    //Console.ReadKey();

                }

                else
                {
                    Array.Copy(PopulationArray[k].PathCities, newPopulationArray[k].PathCities, PopulationArray[k].PathCities.Length);
                }
            }

            Array.Copy(newPopulationArray, PopulationArray, newPopulationArray.Length);
        }

        int[] CrossoverPaths(int[] sciezka1, int[] sciezka2)
        {
            int poz1 = r.Next(0, sciezka1.Length);
            int poz2 = r.Next(0, sciezka1.Length);

            if (poz1 > poz2)
            {
                int pomocniczy = poz2;
                poz2 = poz1;
                poz1 = pomocniczy;
            }

            int[] tablicaPomoc = new int[poz2 - poz1];
            int[] nowaSciezka = new int[sciezka1.Length];

            for (int p = poz1, x = 0; p < poz2; p++, x++)
            {
                tablicaPomoc[x] = sciezka1[p];
            }

            for (int p = 0; p < sciezka1.Length; p++)
            {
                nowaSciezka[p] = -1;
            }

            for (int p = poz1; p < poz2; p++)
            {
                nowaSciezka[p] = sciezka1[p];
            }

            int pomocnicza = 0;
            for (int p = 0; p < sciezka1.Length;)
            {
                if (nowaSciezka[p] == -1)
                {
                    if (!tablicaPomoc.Contains(sciezka2[pomocnicza]))
                    {
                        nowaSciezka[p] = sciezka2[pomocnicza];
                        p++;
                    }
                    pomocnicza++;
                }
                else p++;
            }
            return nowaSciezka;
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


        public void TournamentSelection()
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
                    PopulationArray[k].PrintPath();
                    Array.Copy(PopulationArray[k].PathCities, bestPathInPopulation.PathCities, PopulationArray[k].PathCities.Length);
                    lengthofBestPath = fitnessArray[k];
                    Console.WriteLine();
                    //Console.WriteLine(lengthofBestPath);
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
                    //Console.WriteLine(lengthofBestPath);
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
