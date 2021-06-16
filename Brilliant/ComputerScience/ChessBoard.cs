using GenericDefs.DotNet;
using GenericDefs.Classes.NumberTypes;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Brilliant.ComputerScience
{
    public class ChessBoard : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/queens-are-powerful-in-chess/");
        }

        void ISolve.Solve()
        {
            Part1();
        }

        /// <summary>
        /// https://brilliant.org/problems/queens-are-powerful-in-chess/
        /// </summary>
        void Part1()
        {
            N = 20;
            int[,] board = new int[N, N];

            Number<int> MaxQ = new Number<int>(int.MinValue);

            if (FindSolution(board, 0, MaxQ, 1))
            {
                QueuedConsole.WriteImmediate("Number of queens : {0} for {1}x{1} chess board.", MaxQ.Value, N);
            }
            else { QueuedConsole.WriteImmediate("No solution found for NxN = {0}x{0} chess board.", N); }
        }

        int N { get; set; }

        bool Allowed(int[,] board, int x, int y, int maxPiecesInAttack = 0)
        {
            int qCount = 0;
            for (int i = 0; i <= x; i++)
            {
                if (board[i, y] == 1 || (i <= y && board[x - i, y - i] == 1) || (y + i < N && board[x - i, y + i] == 1))
                {
                    if (maxPiecesInAttack == 0) { return false; }

                    qCount++;
                }
            }
            if (maxPiecesInAttack == 0) return true;
            else return qCount <= maxPiecesInAttack + 2;
        }

        bool FindSolution(int[,] board, int x, Number<int> maxQ, int maxPiecesInAttack = 0)
        {
            for (int y = 0; y < N; y++)
            {
                if (Allowed(board, x, y, maxPiecesInAttack))
                {
                    board[x, y] = 1;
                    if (maxPiecesInAttack == 0)
                    {
                        if (x == N - 1 || FindSolution(board, x + 1, maxQ))
                        {
                            return true;
                        }
                    } else {
                        if (x < N - 1) FindSolution(board, x + 1, maxQ, maxPiecesInAttack);
                        else {
                            maxQ.Value = Math.Max(maxQ.Value, GetPieceCount(board));
                            return true;
                        }
                    }
                    board[x, y] = 0;
                }
            }
            return false;
        }

        int GetPieceCount(int [,] board)
        {
            int qCount = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    qCount += board[i, j] == 1 ? 1 : 0;
                }
            }
            return qCount;
        }
    }
}