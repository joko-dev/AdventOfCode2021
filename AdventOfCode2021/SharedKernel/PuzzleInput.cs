﻿using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2021.SharedKernel
{
    public class PuzzleInput
    {
        private List<string> _lines = new List<string>();

        public List<string> Lines 
        {
            get { return _lines; }
        }

        public PuzzleInput(string filePath)
        {
            string line;
            StreamReader file = File.OpenText(filePath);

            while ((line = file.ReadLine()) != null)
            {
                _lines.Add(line);
            }
        }
    }
}
