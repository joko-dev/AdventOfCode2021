using AdventOfCode2021.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lanternfish: ");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            //simulating single fishes. very low performance with high day count.
            List<Lanternfish> fishes = getLanternfishes(puzzleInput.Lines);
            List<Lanternfish> fishes80 = simulateLanternfishReproductionSingleFish(fishes, 80);
            Console.WriteLine("Lanternfish count after 80 days (simulating single fishes): {0}", fishes80.Count);

            //simulating all fishes per day at once
            List<Int64> fishesPerDay = getLanternfishesPerDay(puzzleInput.Lines);
            List<Int64> fishesPerDay80 = simulateLanternfishPerDay(fishesPerDay, 80);
            List<Int64> fishesPerDay256 = simulateLanternfishPerDay(fishesPerDay, 256);
            Console.WriteLine("Lanternfish count after 80 days (simulating days): {0}", fishesPerDay80.Sum());
            Console.WriteLine("Lanternfish count after 256 days (simulating days): {0}", fishesPerDay256.Sum());
        }

        private static List<Int64> simulateLanternfishPerDay(List<Int64> fishesPerDay, int daysToSimulate)
        {
            List<Int64> fishesPerDaySimulated = new List<Int64>(fishesPerDay);

            for (int i = 1; i <= daysToSimulate; i++)
            {
                List<Int64> fishesPerDayTemp = new List<Int64>(new Int64[fishesPerDaySimulated.Count]);
                // 
                for(int day = fishesPerDaySimulated.Count - 1; day >= 1; day--)
                {
                    fishesPerDayTemp[day - 1] = fishesPerDaySimulated[day];
                }
                //
                fishesPerDayTemp[fishesPerDayTemp.Count - 1] = fishesPerDaySimulated[0];
                //
                fishesPerDayTemp[6] += fishesPerDaySimulated[0];

                fishesPerDaySimulated = fishesPerDayTemp;
            }

            return fishesPerDaySimulated;
        }

        private static List<Int64> getLanternfishesPerDay(List<string> lines)
        {
            List<Int64> fishesPerDay = new List<Int64>(new Int64[9]);
            string[] fishes = lines[0].Split(',');

            foreach (string fish in fishes)
            {
                fishesPerDay[int.Parse(fish)] += 1;
            }

            return fishesPerDay;
        }

        private static List<Lanternfish> simulateLanternfishReproductionSingleFish(List<Lanternfish> fishes, int daysToSimulate)
        {
            List<Lanternfish> simulateFishes = new List<Lanternfish>(fishes.Count);
            fishes.ForEach((item) =>
            {
                simulateFishes.Add((Lanternfish)item.Clone());
            });

            for (int i = 1; i <= daysToSimulate; i++)
            {
                List<Lanternfish> newFishes = new List<Lanternfish>();
                foreach (Lanternfish fish in simulateFishes)
                {
                    Lanternfish newFish = fish.OneDayOlder();
                    if(newFish != null)
                    {
                        newFishes.Add(newFish);
                    }
                }
                simulateFishes.AddRange(newFishes);
            }

            return simulateFishes;
        }

        private static List<Lanternfish> getLanternfishes(List<string> lines)
        {
            List<Lanternfish> fishes = new List<Lanternfish>();
            foreach (string line in lines)
            {
                string[] days = line.Split(',');
                foreach(string day in days)
                {
                    fishes.Add(new Lanternfish(int.Parse(day)));
                }
            }
            return fishes;
        }
    }
}
