using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._4
{
    public class CoordinatingFlights : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/i-dont-want-to-be-embarassed-2/");
        }

        /// <summary>
        /// 
        /// </summary>
        void ISolve.Solve()
        {
            var stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.flight-times.txt", true);
            List<DateTime> flightTimes = new List<DateTime>();
            using (StreamReader sr = new StreamReader(stream))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    flightTimes.Add(DateTime.Parse(s.Replace(" ", "")));
                }
            }

            flightTimes.Sort();

            foreach (DateTime d in flightTimes)
            {
                Console.WriteLine("{0}", d.ToString("HH:mm"));
            }
            Console.ReadKey();
        }
    }
}
