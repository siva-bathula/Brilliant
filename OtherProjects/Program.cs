using System;
namespace OtherProjects
{
    class Program
    {
        static void Main(string[] args)
        {
            //Cracking1980Encryption.CrackingEncryptionProblem();

            //new Knapsack().Solve();
            //Problems.Pi.Calculate();
            //Problems.Sudoku.Solve();

            //BernoulliNumbers.Generate();

            AOC();

            //EfficiencyExperiments();

            //StableMatchingProblem.DoMarriage();
            Console.ReadKey();
        }

        static void EfficiencyExperiments()
        {
            (new EfficiencyExperiments.Counting()).Run();
        }

        static void AOC()
        {
            //Day21to25.Solve();
            AdventOfCode._2016.Day6to10.Solve();
        }

    }
}