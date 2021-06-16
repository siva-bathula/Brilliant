using GenericDefs.Classes.NumberTypes;
using System;

namespace OtherProjects.Problems
{
    public class Pi
    {
        public static void Calculate()
        {
            BigNumber x = new BigNumber(1016);
            BigNumber y = new BigNumber(1016);
            x.ArcTan(16, 5);
            y.ArcTan(4, 239);
            x.Subtract(y);

            Console.WriteLine(x.PrintAsTable(4));
        }
    }
}