using System;
using System.Collections.Generic;

namespace GenericDefs.Functions.Calculus
{
    public class BisectionRootApproximation
    {
        double error = 1e-9;
        Func<double,double> func = null;

        double xL = 0.0, xU = 0.0;
        double fL = 0.0, fU = 0.0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="f">Function to invoke to calculate f(x) at 'x'.</param>
        /// <param name="xLower">Initial lower bound of root.</param>
        /// <param name="xUpper">Initial upper bound of root.</param>
        /// <param name="err"></param>
        public BisectionRootApproximation(Func<double, double> f, double xLower, double xUpper, double errorTolerance = 1e-9)
        {
            error = errorTolerance;
            func = f;

            xL = xLower;
            xU = xUpper;
            
            fL = func.Invoke(xLower);
            fU = func.Invoke(xUpper);

            if (fL * fU >= 0)
            {
                throw new Exception(string.Format("Initial lower and upper estimates for the root are wrong. x = {0}, f(x) = {1}; x = {2}, f(x) = {3}", xLower, fL, xUpper, fU));
            }

            Solve();
        }

        public double Root
        {
            get; private set;
        }

        int nMax = 10000;
        void Solve()
        {
            double diff = xU - xL;
            double x = (xL + xU) / 2.0;
            int n = 0;
            while (diff > error)
            {
                x = (xL + xU) / 2.0;
                double fx = func.Invoke(x);
                if(fx == 0)
                {
                    Root = x;
                    break;
                }

                if(fL*fx < 0)
                {
                    fU = fx;
                    xU = x;
                } else if(fU*fx < 0) {
                    fL = fx;
                    xL = x;
                } else {
                    throw new Exception("Error in bisection estimate.");
                }

                diff = xU - xL;

                if(++n > nMax) { DotNet.QueuedConsole.WriteImmediate("No solution found after {0} bisections.", n); break; }
            }

            Root = x;
        }
    }
}