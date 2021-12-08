using AdventOfCode2021.SharedKernel;
using System;
using System.Collections.Generic;

namespace AdventOfCode2021.Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lanternfish: ");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            List<Lanternfish> fishes = getLanternfishes(puzzleInput.Lines);
            List<Lanternfish> fishes80 = simulateLanternfishReproductionSingleFish(fishes, 80);
            
            Console.WriteLine("Lanternfish count after 80 days: {0}", fishes80.Count);
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
