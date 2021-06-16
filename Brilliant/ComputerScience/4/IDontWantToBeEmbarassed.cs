using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._4
{
    public class IDontWantToBeEmbarassed : ISolve, IBrilliant
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
            var stream = GenericDefs.DotNet.Utility.GetEmbeddedResourceStream("Brilliant.ComputerScience.data.41swaps.txt", true);
            List<string> swaps = new List<string>();
            using (StreamReader sr = new StreamReader(stream))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    swaps.Add(s.Replace(" ", ""));
                }
            }

            ArrayList p = new ArrayList();
            for (int i = 0; i < 8; i++)
            {
                if (i == 0) p.Add(0);
                else p.Add(1);
            }

            foreach (string s in swaps)
            {
                int posX = Convert.ToInt32(s[0].ToString());
                int posY = Convert.ToInt32(s[1].ToString());
                Swap(ref p, posX, posY);
            }

            for (int i = 0; i < 8; i++)
            {
                if ((int)p[i] == 0) Console.WriteLine("The ball is in position :: {0}", i);
            }
            Console.ReadKey();
        }

        void Swap(ref ArrayList l, int posX, int posY)
        {
            int t = (int)l[posX];
            l[posX] = l[posY];
            l[posY] = t;
        }
    }
}
