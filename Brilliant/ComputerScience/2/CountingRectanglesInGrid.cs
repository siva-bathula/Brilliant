using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._2
{
    public class CountingRectanglesInGrid : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("");
        }

        void ISolve.Solve()
        {
            CountEmAll14();
        }

        /// <summary>
        /// All rectangles including squares in 6x6 grid.
        /// </summary>
        void Solve3()
        {
            int totRectangles = 0;

            for (int i = 6; i >= 1; i--)
            {
                for (int j = 6; j >= 1; j--)
                {
                    totRectangles += (i * j);
                }
            }

            Console.WriteLine("Total rectangles :: {0}", totRectangles);
            Console.ReadKey();
        }

        /// <summary>
        /// Rectangular grid of size 20x30 of square size 1x1 blocks.
        /// </summary>
        void Solve1()
        {
            int totRectangles = 0;

            for (int i = 30, a = 1; i >= 1; i--, a++)
            {
                for (int j = 20, b = 1; j >= 1; j--, b++)
                {
                    totRectangles += (i * j);
                }
            }

            Console.WriteLine("Total rectangles :: {0}", totRectangles);
            Console.ReadKey();
        }

        /// <summary>
        /// Rectangular grid of size 6x5 of square size 1x1 blocks.
        /// </summary>
        void Solve2()
        {
            int totRectangles = 0;

            for (int i = 6, a = 1; i >= 1; i--, a++)
            {
                for (int j = 5, b = 1; j >= 1; j--, b++)
                {
                    totRectangles += (i * j);
                }
            }

            Console.WriteLine("Total rectangles :: {0}", totRectangles);
            Console.ReadKey();
        }

        /// <summary>
        /// Number of quadrilaterals in m x n grid with top left 1 x 1 portion folded to form a small triangle.
        /// </summary>
        internal static void CountEmAll14()
        {
            //int m = 20, n = 30;
            int m = 4, n = 5;

            //quadrilaterals including the corner triangle.
            int nQuad1 = m - 1 + n - 1;
            int nQuad2 = m - 1 + n - 1;

            //Approach 1.
            //all rectangles and squares in the 20x29 rectangle
            for (int i = m, a = 1; i >= 1; i--, a++)
            {
                for (int j = n - 1, b = 1; j >= 1; j--, b++)
                {
                    nQuad1 += (i * j);
                }
            }

            //all rectangles including the 30th row
            nQuad1 += n * m * (m - 1) / 2;

            //Approach 2.
            //all rectangles and squares in the 29x20 rectangle
            for (int i = m - 1, a = 1; i >= 1; i--, a++)
            {
                for (int j = n, b = 1; j >= 1; j--, b++)
                {
                    nQuad2 += (i * j);
                }
            }

            //all rectangles including the 1st column
            nQuad2 += m * n * (n - 1) / 2;

            Console.WriteLine("Approach 1 : Possible quadrilaterals of the given grid is :: {0}", nQuad1);
            Console.WriteLine("Approach 2 : Possible quadrilaterals of the given grid is :: {0}", nQuad2);
            Console.ReadKey();
        }
    }
}