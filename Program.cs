using System;
using System.Linq;
using System.Collections.Generic;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Solver solver = new Solver(int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine()), Console.ReadLine());
            Console.WriteLine(solver.FindMinimalClickCount(Console.ReadLine()));
        }
    }

    public class Solver
    {
        private Keyboard _keyboard;
        private int w, h;

        public Solver(int width, int height, string tableContents)
        {
            this._keyboard = new Keyboard(width, height, tableContents);
            this.w = width;
            this.h = height;
        }

        public int FindMinimalClickCount(string desiredString)
        {
            int[][,] helpers = new int[2][,];
            for (int i = 0; i < helpers.Length; i++)
            {
                helpers[i] = new int[this.h,this.w];
            }

            byte currHelper = 0;

            for (int i = 0; i<this.h;i++)
            {
                for(int j = 0;j <this.w;j++)
                {
                    helpers[currHelper][i, j] = i+j;
                }
            }
            
            for(int i = 0;i < desiredString.Length;i++)
            {
                char currentChar = desiredString[i];
                if (!_keyboard.contains(currentChar)) continue;

                for(int y1 = 0; y1 < this.h;y1++)
                {
                    for (int x1 = 0; x1 < this.w; x1++)
                    {
                        helpers[(currHelper + 1) % 2][y1,x1] = int.MaxValue;

                        for(int y2 = 0; y2 < this.h;y2++)
                        {
                            for(int x2 = 0;x2 < this.w;x2++)
                            {
                                if (_keyboard.charAt(y2, x2) != currentChar) continue;
                                helpers[(currHelper + 1) % 2][y1, x1] = Math.Min(
                                    helpers[(currHelper + 1) % 2][y1, x1],
                                    helpers[currHelper][y2, x2] + Math.Abs(x2 - x1) + Math.Abs(y2 - y1) + 1);
                            }
                        }
                    }
                }
                currHelper = (byte)((currHelper + 1) % 2);
            }

            int final = int.MaxValue;

            for(int i = 0;i <this.h;i++)
            {
                for(int j = 0; j<this.w;j++)
                {
                    final = Math.Min(final, helpers[currHelper][i, j]);
                }
            }

            return final;  
        }
    }

    public class Keyboard
    {
        private char[,] _keyinfo;
        private string kbInitString;

        public Keyboard(int width, int height, string kbInitString)
        {
            this.kbInitString = kbInitString;
            _keyinfo = new char[height,width];

            for (int i = 0; i < kbInitString.Length; i++)
            {
                char c = kbInitString[i];
                int iIndex = i / width;
                int jIndex = i % width;

                _keyinfo[iIndex,jIndex] = c;
            }
        }

        public bool contains(char c)
        {
            return kbInitString.Contains(c);
        }

        public char charAt(int i, int j)
        {
            return _keyinfo[i,j];
        }

        public override string ToString()
        {
            string lf = System.Environment.NewLine;
            string final = "[" + lf;

            for (int i = 0; i < _keyinfo.GetLength(0); i++)
            {
                final += "[";
                for(int j = 0;j < _keyinfo.GetLength(1);j++)
                {
                    final += _keyinfo.GetValue(i, j);
                }
                final += "]" + lf;
            }
            final += "]";
            return final;
        }
    }
}