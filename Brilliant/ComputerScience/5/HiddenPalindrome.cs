using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._5
{
    public class HiddenPalindrome : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/havannahhannah/");
        }

        void ISolve.Solve()
        {
            //string str = "79776995125591966288822764239979864294919636529599153749317118101911111012111163616102113319219811581118511891291331120161636111121011111910181171394735199592563691949246897993246722888266919552159967797";

            string str = "abcdeeeertyudcbad";
            //char[] lhp = GenericDefs.Functions.Algorithms.DP.Palindrome.LongestHiddenPalindrome(str);
            
            Console.WriteLine("Approach 1 :: Longest hidden palindrome length is : {0}", GenericDefs.Functions.Algorithms.DP.Palindrome.LongestHiddenPalindrome(str));
            Console.WriteLine("Approach 2 :: Longest hidden palindrome length is : {0}", new string(GenericDefs.Functions.Algorithms.DP.Palindrome.HiddenPalindrome(str)).Length);
            Console.ReadKey();
        }
    }
}