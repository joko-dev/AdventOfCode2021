using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.SharedKernel
{
    public class PuzzleInput
    {
        private List<string> _lines = new List<string>();

        public List<string> Lines 
        {
            get { return _lines; }
        }

        public PuzzleInput(string filePath) : this(filePath, false) { }

        public PuzzleInput(string filePath, bool ignoreEmptyLines)
        {
            string line;
            StreamReader file = File.OpenText(filePath);

            while ((line = file.ReadLine()) != null)
            {
                if(!ignoreEmptyLines | line != "")
                {
                    _lines.Add(line);
                }
            }
        }

        public int[,] getInputAsMatrixInt()
        {
            int height = Lines.Count();
            int width = Lines.OrderBy(s => s.Length).Last().Length;
            int[,] matrix = new int[width,height];

            for(int h = 0; h < height; h++)
            {
                for(int w = 0; w < width; w++)
                {
                    matrix[w, h] = (int)char.GetNumericValue(Lines[h][w]);     
                }
            }

            return matrix;
        }
    }
}
