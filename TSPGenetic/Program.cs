using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace TSPGenetic
{
    class Program
    {
        public static String FilePath= @"C:\Users\Marcin\Desktop\berlin52.txt";
        public static int[,] distancesArray = Program.ReadDistancesFromFile(FilePath);
        public static Random r = new Random();

        public static int NumberOfCities(String filePath)
        {
            String[] lines = File.ReadAllLines(filePath);
            return int.Parse(lines[0]);
        }

        public static int[,] ReadDistancesFromFile(String sciezkaPliku) //read txt file to tab
        {
            String[] lines = File.ReadAllLines(sciezkaPliku);

            int size = int.Parse(lines[0]);
            int[,] TabDistances = new int[size, size];

            int x = 0;

            foreach (string line in lines)
            {
                string[] row = line.Trim().Split(' ');

                if (line != lines[0])
                {
                    for (int i = 0; i < row.Length; i++)
                    {
                        int value = int.Parse(row[i]);
                        TabDistances[x, i] = value;
                        TabDistances[i, x] = value;
                    }
                    x++;
                }
            }
            return TabDistances;
        }

        public static int[] Mutation(int[] path)
        {
            int gen1 = r.Next(0, path.Length);
            int gen2 = r.Next(0, path.Length);

            if (gen1 > gen2)
            {
                int foo = gen2;
                gen2 = gen1;
                gen1 = foo;
            }

            int[] tab1 = new int[gen2 - gen1];

            for (int p = gen1, x = 0; p < gen2; p++, x++)
            {
                tab1[x] = path[p];
            }

            Array.Reverse(tab1);

            for (int p = gen1, x = 0; p < gen2; p++, x++)
            {
                path[p] = tab1[x];
            }

            return path;
        }

        public static int[] Crossover(int[] path1, int[] sciezka2)
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
                    if (!tab1.Contains(sciezka2[foo]))
                    {
                        newPath[p] = sciezka2[foo];
                        p++;
                    }
                    foo++;
                }
                else p++;
            }
            return newPath;
        }

        public static int[,] TournamentSelection(int[,] population, int numberofCities, int numberOfIndividuals)
        {
            int[,] newPopulation = new int[numberOfIndividuals, numberofCities];
            int[] PathsDistances = new int[numberOfIndividuals];
            int sumDistancePath;
            int tournamentWinner;

            for (int k = 0; k < numberOfIndividuals; k++)
            {
                sumDistancePath = 0;
                for (int n = 0; n < numberofCities - 1; n++)
                {
                    sumDistancePath += distancesArray[population[k, n], population[k, n + 1]];
                }
                PathsDistances[k] = sumDistancePath;
            }

            for (int k = 0; k < numberOfIndividuals; k++)
            {
                int individual1 = r.Next(0, numberOfIndividuals);
                int individual2 = r.Next(0, numberOfIndividuals);
                int individual3 = r.Next(0, numberOfIndividuals);

                int sumDistancePath1 = PathsDistances[individual1];
                int sumDistancePath2 = PathsDistances[individual2];
                int sumDistancePath3 = PathsDistances[individual3];

                if (sumDistancePath1 <= sumDistancePath2 && sumDistancePath1 <= sumDistancePath3) tournamentWinner = individual1;
                else if (sumDistancePath2 <= sumDistancePath1 && sumDistancePath2 <= sumDistancePath3) tournamentWinner = individual2;
                else tournamentWinner = individual3;

                for (int a = 0; a < numberofCities; a++)
                {
                    newPopulation[k, a] = population[tournamentWinner, a];
                }
            }
            return newPopulation;
        }


        static void Main(string[] args)
        {
            int pathLength = NumberOfCities(FilePath);

            int individualsInGeneration = 80;
            int mutationChance = 30;
            int crossoverChance = 40;
            int numberOfLoops = 1000000;

            int[] bestSolutionPath = new int[pathLength];
            int shortestDistance = 100000000;
            int lastBestResult = 0;

            int[,] population = new int[individualsInGeneration, pathLength];
            int[,] newPopulation = new int[individualsInGeneration, pathLength];

            Random r = new Random();


            for (int k = 0; k < individualsInGeneration; k++) //first random generation
            {
                bool[] isDrawn = new bool[pathLength];
                int randomCity;

                for (int n = 0; n < pathLength;)
                {
                    randomCity = r.Next(0, pathLength);

                    if (isDrawn[randomCity] == false)
                    {
                        isDrawn[randomCity] = true;
                        population[k, n] = randomCity;
                        n++;
                    }
                }
            }


            //main loop
            for (int i = 0; i < numberOfLoops; i++)
            {
                for (int k = 0; k < individualsInGeneration / 2; k++) //crossover
                {
                    int random1 = r.Next(0, individualsInGeneration);
                    int random2 = r.Next(0, individualsInGeneration);
                    if (r.Next(0, 100) < crossoverChance) 
                    {
                        int[] individual1 = new int[pathLength];
                        int[] individual2 = new int[pathLength];
                        int[] foo = new int[pathLength];
                        int[] foo2 = new int[pathLength];

                        for (int p = 0; p < pathLength; p++)
                        {
                            individual1[p] = population[random1, p];
                        }

                        for (int p = 0; p < pathLength; p++)
                        {
                            individual2[p] = population[random2, p];
                        }
                        int counter = 0;
                        while ((individual1.SequenceEqual(individual2) && counter < 10))
                        {
                            random1 = r.Next(0, individualsInGeneration);
                            random2 = r.Next(0, individualsInGeneration);

                            for (int p = 0; p < pathLength; p++)
                            {
                                individual1[p] = population[random1, p];
                            }

                            for (int p = 0; p < pathLength; p++)
                            {
                                individual2[p] = population[random2, p];
                            }
                            counter++;
                        }

                        foo = Crossover(individual1, individual2);
                        foo2 = Crossover(individual2, individual1);

                        for (int p = 0; p < pathLength; p++)
                        {
                            newPopulation[k, p] = foo[p];
                        }

                        for (int p = 0; p < pathLength; p++)
                        {
                            newPopulation[individualsInGeneration - k - 1, p] = foo2[p];
                        }

                    }
                    else
                    {
                        for (int p = 0; p < pathLength; p++)
                        {
                            newPopulation[k, p] = population[random1, p];
                            newPopulation[individualsInGeneration - k - 1, p] = population[random2, p];
                        }
                    }

                }
                Array.Copy(newPopulation, population, population.Length); 


                for (int k = 0; k < individualsInGeneration; k++) //mutation
                {          
                    if (r.Next(0, 100) < mutationChance)
                    {
                        int[] individual1 = new int[pathLength];

                        for (int p = 0; p < pathLength; p++)
                        {
                            individual1[p] = population[k, p];
                        }
                        individual1 = Mutation(individual1);

                        for (int p = 0; p < pathLength; p++)
                        {
                            population[k, p] = individual1[p];
                        }
                    }

                    int sumDistance = 0;
                    for (int n = 0; n < pathLength - 1; n++)
                    {
                        sumDistance += distancesArray[population[k, n], population[k, n + 1]];
                    }
                    sumDistance += distancesArray[population[k, pathLength - 1], population[k, 0]];

                    if (sumDistance < shortestDistance && i > 0)
                    {
                        shortestDistance = sumDistance;
                        for (int n = 0; n < pathLength; n++)
                        {
                            if (n < pathLength - 1)
                                Console.Write(population[k, n] + "-");
                            else
                                Console.Write(population[k, n] + " ");
                        }
                        Console.WriteLine(sumDistance + " loop nr: " + i);

                        for (int p = 0; p < pathLength; p++)
                        {
                            bestSolutionPath[p] = population[k, p];
                        }

                    }
                 
                }

                //tournament selection - random 3 paths - select best of them
                population = TournamentSelection(population, pathLength, individualsInGeneration);

                //elitism - replace 3 paths in population with best known solution
                if (i > 1000)
                {
                    for (int p = 0; p < pathLength; p++)
                    {
                        population[0, p] = bestSolutionPath[p];
                        population[1, p] = bestSolutionPath[p];
                        population[2, p] = bestSolutionPath[p];
                    }
                }
            }
            Console.ReadKey();
        }
    }
}


