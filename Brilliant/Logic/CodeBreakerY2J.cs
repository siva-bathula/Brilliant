using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.Logic
{
    public class CodeBreakerY2J : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/gibberish/");
        }

        void ISolve.Solve()
        {
            string[] codeArr = new string[] { "HOMY", "TREW", "ALRE", "INEE", "TNTS", "HAAE", "PRPS" };
            for (int i = 0; i < codeArr.Length; i++)
            {
                codeArr[i] = codeArr[i].ToLower();
            }

            string[] code2Arr = new string[4];

            foreach (string code in codeArr) {
                for (int i = 0; i < code.Length; i++) {
                    code2Arr[i] += code[i];
                }
            }

            for (int i = -28; i <= 28; i++)
            {
                string s = string.Empty;
                foreach (string code in codeArr) {
                    s += " " + GenericDefs.Functions.Crypto.Cypher.Caeser.Encipher(code, i);
                }
                Console.WriteLine("DeCoded :: {0}, Rotation :: {1}", s, i);
            }

            Console.WriteLine("--------------------------");
            Console.WriteLine("--------------------------");
            Console.WriteLine("--------------------------");
            Console.WriteLine(Environment.NewLine);

            for (int i = -28; i <= 28; i++)
            {
                string s = string.Empty;
                foreach (string code in code2Arr)
                {
                    s += " " + GenericDefs.Functions.Crypto.Cypher.Caeser.Encipher(code, i);
                }
                Console.WriteLine("DeCoded :: {0}, Rotation :: {1}", s, i);
            }
            Console.ReadKey();
        }
    }
}