using GenericDefs.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._4
{
    /// <summary>
    /// A binary message, 24 digits long, is being transmitted via a satellite from the USA to Australia. This message is being sent 1 digit at a time. 
    /// However, after a some digits have passed, a glitch causes all the digits after it has activated to revert to their counterpart (1 becomes 0 and 0 becomes 1) 
    /// whilst being sent. This is the message received in Australia:
    /// 111010101000111001100111
    /// If there were originally 12 1's and 12 0's in the transmission(there are 14 1's and 10 0's now), in how many places could the glitch have started to affect the transmission?
    /// </summary>
    public class BinaryTransmissionGlitch : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/binary-transmission-glitch/");
        }
        
        void ISolve.Solve()
        {
            int[] transCode = new int[] { 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 1 };

            int[] actCode = new int[transCode.Count()];

            int sIndex = 0;
            int startPossibilities = 0;
            while (sIndex < transCode.Count()) { 
                transCode.CopyTo(actCode, 0);

                for (int i = sIndex; i < transCode.Count(); i++) {
                    if (actCode[i] == 1) actCode[i] = 0;
                    else if (actCode[i] == 0) actCode[i] = 1;
                }

                if (actCode.Sum(x => x) == 12) startPossibilities++;
                sIndex++;
                if (sIndex == transCode.Count()) break;
            }


            Console.WriteLine("Possible start positions :: {0}", startPossibilities);
            Console.ReadKey();
        }
    }
}