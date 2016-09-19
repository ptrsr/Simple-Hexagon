using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GXPEngine
{
    public class Waves
    {
        private static int[,] walls = new int[6,21];
        private static int length;

        private static string[,] waves = new string[4, 2] 
        {
            {"gate.txt","9"},
            {"twister.txt","11"},
            {"slaom.txt", "11"},
            {"kill me.txt","18"}
        };

        public static int[,] ReturnWalls()
        {
            return walls;
        }

        public static void LoadWave()
        {
            string fileName = ReturnRandomWave();
            StreamReader streamReader = new StreamReader(fileName);
            string text = streamReader.ReadToEnd();
            streamReader.Close();

            int i = 0, j = 0;
            foreach (var row in text.Split('\n'))
            {
                j = 0;
                foreach (var col in row.Trim().Split(' '))
                {
                    walls[i, j] = int.Parse(col.Trim());
                    j++;
                }
                i++;
            }
        }

        public static int ReturnLength()
        {
            return length;
        }

        private static string ReturnRandomWave() 
        {
            Random rnd = new Random();
            int number = rnd.Next(0, 4);

            length = int.Parse(waves[number, 1]);
            return waves[number, 0];
        }

    }
}
