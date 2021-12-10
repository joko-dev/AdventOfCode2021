using AdventOfCode2021.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Day4
{
    // Would be cleaner if board is extracted into own class

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 4: Giant Squid"));
            Console.WriteLine("Bingo file:");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            List<int> bingoNumbers = puzzleInput.Lines[0].Split(',').Select(int.Parse).ToList();
            List<(int value, bool drawn)[,]> boards = getBoards(puzzleInput.Lines);

            int lastCalledNumber = 0;
            (int value, bool drawn)[,] board = determineWinningBoard(boards, bingoNumbers, out lastCalledNumber);
            int sumUnmarkedNumbers = getSumUnmarkedNumbers(board);

            Console.WriteLine("Score Winning Board: unmarked numbers({0}) * last called number ({1}): {2}", sumUnmarkedNumbers, lastCalledNumber, sumUnmarkedNumbers * lastCalledNumber);

            boards = getBoards(puzzleInput.Lines);
            lastCalledNumber = 0;
            board = determineLastWinningBoard(boards, bingoNumbers, out lastCalledNumber);
            sumUnmarkedNumbers = getSumUnmarkedNumbers(board);
            Console.WriteLine("Score Last Winning Board: unmarked numbers({0}) * last called number ({1}): {2}", sumUnmarkedNumbers, lastCalledNumber, sumUnmarkedNumbers * lastCalledNumber);
        }

        private static (int value, bool drawn)[,] determineWinningBoard(List<(int value, bool drawn)[,]> boards, List<int> bingoNumbers, out int lastCalledNumber)
        {
            (int value, bool drawn)[,] winningBoard = null;
            lastCalledNumber = 0;

            foreach (int number in bingoNumbers)
            {
                foreach ((int value, bool drawn)[,] board in boards)
                {
                    callNumber(board, number);
                    if (isWinningBoard(board))
                    {
                        winningBoard = board;
                        break;
                    }
                }

                if (winningBoard is not null)
                {
                    lastCalledNumber = number;
                    break;
                }
            }

            return winningBoard;
        }

        private static (int value, bool drawn)[,] determineLastWinningBoard(List<(int value, bool drawn)[,]> boards, List<int> bingoNumbers, out int lastCalledNumber)
        {
            (int value, bool drawn)[,] lastWinningBoard = null;
            lastCalledNumber = 0;
            bool[] wonBoards = new bool[boards.Count];


            foreach (int number in bingoNumbers)
            {
                for(int i = 0; i < boards.Count; i++)
                {
                    if(!wonBoards[i])
                    {
                        (int value, bool drawn)[,] board = boards[i];

                        callNumber(board, number);
                        if (isWinningBoard(board))
                        {
                            lastWinningBoard = board;
                            wonBoards[i] = true;
                        }
                    }   
                }

                if(Array.TrueForAll(wonBoards, value => { return value;}))
                {
                    lastCalledNumber = number;
                    break;
                }

            }

            return lastWinningBoard;
        }

        private static bool isWinningBoard((int value, bool drawn)[,] board)
        {
            bool winning = isWinningBoardHorizontal(board) | isWinningBoardVertical(board);
            return winning;
        }

        private static bool isWinningBoardHorizontal((int value, bool drawn)[,] board)
        {
            bool winning = false;
            for (int x = 0; x < board.GetLength(0); x++)
            {
                winning = true;

                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (!board[x, y].drawn)
                    {
                        winning = false;
                        break;
                    }
                }

                if (winning)
                {
                    break;
                }
            }
            return winning;
        }
        private static bool isWinningBoardVertical((int value, bool drawn)[,] board)
        {
            bool winning = false;
            for (int y = 0; y < board.GetLength(1); y++)
            {
                winning = true;

                for (int x = 0; x < board.GetLength(0); x++)
                {
                    if (!board[x, y].drawn)
                    {
                        winning = false;
                        break;
                    }
                }

                if (winning)
                {
                    break;
                }
            }
            return winning;
        }

        private static void callNumber((int value, bool drawn)[,] board, int calledNumber)
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x,y].value == calledNumber)
                    {
                        board[x, y].drawn = true;
                    }
                }
            }
        }

        private static int getSumUnmarkedNumbers((int value, bool drawn)[,] board)
        {
            int sum = 0;

            foreach((int value, bool drawn) entry in board)
            {
                if (!entry.drawn)
                {
                    sum += entry.value;
                }
            }

            return sum;
        }

        private static List<(int value, bool drawn)[,]> getBoards(List<string> lines)
        {
            List<(int value, bool drawn)[,]> boards = new List<(int, bool)[,]>();

            for(int i=1; i < lines.Count; i+=5)
            {
                (int value, bool drawn)[,] board = new (int, bool)[5,5];
                
                for(int j = 0; j < 5; j++)
                {
                    string[] values = lines[i + j].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    int k = 0;

                    foreach (string number in values)
                    {
                        board[j, k].value = Int32.Parse(values[k]);
                        board[j, k].drawn = false;
                        k++;
                    }
                    
                }

                boards.Add(board);
            }

            return boards;
        }
    }
}
