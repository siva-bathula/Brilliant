using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Brilliant.ComputerScience
{
    public class WealthDisparity : ISolve, IBrilliant, IProblemName
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get
            {
                return thisProblem;
            }
        }

        string IProblemName.GetName()
        {
            return "INOI 2016, Wealth Disparity";
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/inoi-2016-wealth-disparity/");
        }

        internal class Employee
        {
            internal int ID { get; set; }
            internal int ManagerID { get; set; }
            internal bool IsBoss { get; set; }
            internal long Wealth { get; set; }
            internal List<int> Subordinates { get; set; }
        }

        Dictionary<int, Employee> Employees { get; set; }

        void ISolve.Solve()
        {
            Employees = new Dictionary<int, Employee>();

            string html = Http.Request.GetHtmlResponse("http://pastebin.com/raw/PjsVyXZR");
            string[] input = html.Splitter(StringSplitter.SplitUsing.LineBreak);
            int N = int.Parse(input[0]);

            string[] eWealth = input[1].Splitter(StringSplitter.SplitUsing.Space);
            Enumerable.Range(1, N).ForEach(x =>
            {
                Employee e = new Employee() { ID = x, Wealth = long.Parse(eWealth[x - 1]) };
                Employees.Add(x, e);
            });

            string[] managers = input[2].Splitter(StringSplitter.SplitUsing.Space);
            HashSet<int> DistinctManagers = new HashSet<int>();
            Enumerable.Range(1, N).ForEach(x =>
            {
                Employee e = Employees[x];
                int mId = int.Parse(managers[x - 1]);
                if (mId == -1) { e.IsBoss = true; }
                else { e.ManagerID = mId; }

                DistinctManagers.Add(mId);
            });

            Employees.ForEach(x =>
            {
                if (!x.Value.IsBoss)
                {
                    Employee manager = Employees[x.Value.ManagerID];
                    if(manager.Subordinates == null) { manager.Subordinates = new List<int>(); }
                    manager.Subordinates.Add(x.Value.ID);
                }
            });

            long MaxPossibleDisparity = long.MinValue;
            Action<long, List<int>> CheckDisparity = null;
            CheckDisparity = delegate (long ai, List<int> subOrdinates) {
                subOrdinates.ForEach(x =>
                {
                    Employee ej = Employees[x];
                    long d = ai - ej.Wealth;
                    MaxPossibleDisparity = Math.Max(MaxPossibleDisparity, d);
                    if (ej.Subordinates != null) CheckDisparity(ai, ej.Subordinates);
                });
            };

            Employees.ForEach(x => {
                Employee ei = x.Value;
                QueuedConsole.WriteImmediate("Checking for id : {0}", ei.ID);
                long ai = ei.Wealth;
                if (ei.Subordinates != null) { CheckDisparity(ai, ei.Subordinates); }
                QueuedConsole.WriteImmediate("Maximum disparity so far ...... : {0}", MaxPossibleDisparity);
            });
            QueuedConsole.WriteImmediate("Maximum possible disparity : {0}", MaxPossibleDisparity);
        }
    }
}