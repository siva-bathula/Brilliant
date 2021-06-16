using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._5
{
    public class CalvinHatesHisHomework : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/calvin-hates-his-homework/");
        }

        void ISolve.Solve()
        {
            List<string> expressions = new List<string>();
            LoadProblems(ref expressions);
            List<Equation> eqs = new List<Equation>();
            foreach (string s in expressions)
            {
                string[] sarr = s.Replace(" ", "").Split(new char[] { '=' });

                Equation eq = new Equation();
                eq.ParsedEquation = s;
                eq.LHS.Expression = sarr[0];
                eq.LHS.ParseToObjects();
                eq.RHS.Expression = sarr[1];
                eq.RHS.ParseToObjects();

                eqs.Add(eq);
            }

            double lhsNValue = 0, rhsNValue = 0;
            double lhsXprefix = 0, rhsXprefix = 0;
            double xSum = 0;
            foreach (Equation eq in eqs)
            {
                lhsNValue = 0; rhsNValue = 0;
                lhsXprefix = 0; rhsXprefix = 0;

                foreach (ParsedObject p in eq.LHS.objects) {
                    if (p.IsNumber) { lhsNValue += p.Number; }
                    else if (p.IsVariable) { lhsXprefix += p.VariablePrefix; }
                }
                foreach (ParsedObject p in eq.RHS.objects)
                {
                    if (p.IsNumber) { rhsNValue += p.Number; }
                    else if (p.IsVariable) { rhsXprefix += p.VariablePrefix; }
                }

                if (rhsXprefix == lhsXprefix) continue;
                double eqX = (lhsNValue - rhsNValue) / (rhsXprefix - lhsXprefix);
                xSum += eqX;
            }

            Console.WriteLine("Sum of all x is :: {0}", xSum);
            Console.ReadKey();
        }

        void LoadProblems(ref List<string> expressions) {
            var stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.problems.txt", true);
            using (StreamReader sr = new StreamReader(stream))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    expressions.Add(s);
                }
            }
        }

        private class Equation {
            internal string ParsedEquation = string.Empty;
            internal SideEquation LHS = new SideEquation();
            internal SideEquation RHS = new SideEquation();
        }

        private class SideEquation
        {
            internal List<ParsedObject> objects = new List<ParsedObject>();
            internal string Expression;

            internal void ParseToObjects()
            {
                List<string> ao = new List<string>();

                string s = string.Empty;
                int index = -1;
                foreach (char c in Expression)
                {
                    ++index;
                    if (char.IsDigit(c))
                    {
                        if (!string.IsNullOrEmpty(s)) { s = s + "" + c; }
                        else { s = "" + c; }

                        if (Expression.Length - 1 == index)
                        {
                            ao.Add(s);
                            s = string.Empty;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            ao.Add(s);
                            s = string.Empty;
                        }
                        ao.Add("" + c);
                    }
                }

                int n, nu = 0;
                for (int i = 0; i < ao.Count; i++)
                {
                    if (int.TryParse(ao[i], out n))
                    {
                        if (i - 1 >= 0 && ao[i - 1] == "*")
                        {
                            this.objects.Add(new ParsedObject() { IsNumber = false, IsVariable = true, VariablePrefix = n });
                            n = -1; nu = -1;
                        }
                        else if ((i + 1 <= ao.Count - 1 && (ao[i + 1] == "x" || ao[i + 1] == "*"))) {
                            if (i - 1 >= 0 && ao[i - 1] == "-") { n = -n; }
                            nu = n;
                            //do nothing, unused n.
                        }
                        else
                        {
                            if (i > 0 && ao[i - 1] == "-")
                            {
                                this.objects.Add(new ParsedObject() { IsNumber = true, Number = -n });
                            }
                            else {
                                this.objects.Add(new ParsedObject() { IsNumber = true, Number = n });
                            }
                            n = -1; nu = -1;
                        }
                    }
                    else if (ao[i] == "x")
                    {
                        if (i - 1 >= 0 && ao[i - 1] == "*")
                        {
                            this.objects.Add(new ParsedObject() { IsNumber = false, IsVariable = true, VariablePrefix = nu });
                            n = -1; nu = -1;
                        }
                        else if (i + 1 <= ao.Count - 1 && ao[i + 1] == "*") {
                            //do nothing
                            nu = -1;
                        }
                        else if (i > 0 && ao[i - 1] == "-")
                        {
                            this.objects.Add(new ParsedObject() { IsNumber = false, IsVariable = true, VariablePrefix = -1 });
                        }
                        else
                        {
                            this.objects.Add(new ParsedObject() { IsNumber = false, IsVariable = true, VariablePrefix = 1 });
                        }
                    }
                    else
                    {
                        //do nothing
                    }
                }

                Console.WriteLine("Expression :: {0}, objects(incl. operators) :: {1}", Expression, ao.Count);
            }
        }

        private class ParsedObject {
            internal bool IsVariable;
            internal bool IsNumber;

            internal int VariablePrefix;
            internal int Number;
        }
    }
}