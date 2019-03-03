using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP_Genetic.NET
{
    public class FileReaderDistances
    {
        public static int[,] ReadDistancesFromFile(String filePath)
        {
            String[] lines = File.ReadAllLines(filePath);

            int size = int.Parse(lines[0]);
            int[,] distancesArray = new int[size, size];

            int x = 0;

            foreach (string line in lines)
            {
                string[] row = line.Trim().Split(' ');

                if (line != lines[0])
                {
                    for (int i = 0; i < row.Length; i++)
                    {
                        int value = int.Parse(row[i]);
                        distancesArray[x, i] = value;
                        distancesArray[i, x] = value;
                    }
                    x++;
                }
            }
            return distancesArray;
        }
        public static int NumberOfCities(String filePath)
        {
            String[] lines = File.ReadAllLines(filePath);
            return int.Parse(lines[0]);
        }
    }
}
