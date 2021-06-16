using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDefs.Functions.Calculus
{
    internal class Constants
    {
        public static double Epsilon = 1E-6;
        public static double PolynomialEpsilon = Epsilon / 1000;
        public static int DecimalPlaces = 6;
    }

    /// <summary>
    /// Using Sturm Sequences.
    /// Define a*x^2 + b*x + c) make these calls:
    /// Polynomial poly = new Polynomial(); poly.Add(2, a); poly.Add(1, b); poly.Add(0, c);
    /// To find the roots, create a new SturmSequence object with the polynomial, then run FindRoots over a specific range.
    /// And that’s it, your polynomial is ready. You can now use it in arithmetic with other polynomials, or evaluate it, find derivatives or find roots.
    /// </summary>
    public class Polynomial
    {
        Dictionary<uint, double> _polynomials = new Dictionary<uint, double>();
        private uint? _degree = null;
        public uint Degree
        {
            get
            {
                if (!_degree.HasValue)
                {
                    _degree = 0;
                    foreach (uint factor in _polynomials.Keys)
                    {
                        _degree = Math.Max(_degree.Value, factor);
                    }
                }
                return _degree.Value;
            }
        }

        public double GetFactor(uint power)
        {
            double factor;
            if (_polynomials.TryGetValue(power, out factor))
                return factor;
            throw new ArgumentException("power");
        }

        public void Add(uint power, double factor)
        {
            if (_polynomials.ContainsKey(power))
            {
                _polynomials[power] += factor;
                if (Math.Abs(_polynomials[power]) < Constants.PolynomialEpsilon)
                {
                    _polynomials.Remove(power);
                }
            }
            else if (factor != 0)
            {
                _polynomials[power] = factor;
            }

            _degree = null;
        }

        public void Add(Polynomial polynomial)
        {
            foreach (uint power in polynomial._polynomials.Keys)
            {
                Add(power, polynomial._polynomials[power]);
            }
            _degree = null;
        }

        public void Sub(uint power, double factor)
        {
            Add(power, -factor);
            _degree = null;
        }

        public void Sub(Polynomial polynomial)
        {
            foreach (uint power in polynomial._polynomials.Keys)
            {
                Sub(power, polynomial._polynomials[power]);
    }
            _degree = null;

        }

        public Polynomial Mult(uint power, double factor)
        {
            Polynomial newPolynomial = new Polynomial();
            foreach (KeyValuePair<uint, double> kvp in _polynomials)
            {
                newPolynomial.Add(kvp.Key + power, kvp.Value * factor);
            }
            return newPolynomial;
        }

        public Polynomial Mult(Polynomial polynomial)
        {
            Polynomial sum = new Polynomial();
            foreach (KeyValuePair<uint, double> kvp in _polynomials)
            {
                sum.Add(polynomial.Mult(kvp.Key, kvp.Value));
            }
            return sum;
        }

        public Polynomial Power(uint power)
        {
            Polynomial result = this.Copy();
            for (int i = 0; i < power - 1; i++)
            {
                result = result.Mult(this);
            }
            return result;
        }

        public void Div(Polynomial polynomial, out Polynomial result, out Polynomial remainder)
        {
            Div(this, polynomial, out result, out remainder);
        }

        public static void Div(Polynomial a, Polynomial b, out Polynomial result, out Polynomial remainder)
        {
            Polynomial numerator = a.Copy();    // thing being divided
            Polynomial denominator = b.Copy();  // thing dividing
            result = new Polynomial();
            // numerator / denominator = result + remainder / denominator

            uint currentDegree;
            double currentFactor;
            do
            {
                currentDegree = numerator.Degree - denominator.Degree;
                currentFactor = numerator.GetFactor(numerator.Degree) / denominator.GetFactor(denominator.Degree);

                numerator.Sub(denominator.Mult(currentDegree, currentFactor));
                result.Add(currentDegree, currentFactor);
            } while (numerator.Degree >= denominator.Degree);

            remainder = numerator.Copy();
        }

        public Polynomial Derivative()
        {
            Polynomial newPolynomial = new Polynomial();
            foreach (KeyValuePair<uint, double> kvp in _polynomials)
            {
                if (kvp.Key != 0)
                {
                    newPolynomial.Add(kvp.Key - 1, kvp.Value * (kvp.Key));
                }
            }

            return newPolynomial;
        }

        public double Eval(double t)
        {
            double sum = 0;
            foreach (KeyValuePair<uint, double> kvp in _polynomials)
            {
                sum += Math.Pow(t, kvp.Key) * kvp.Value;
            }
            return sum;
        }

        public Polynomial Eval(Polynomial polynomial)
        {
            Polynomial sum = new Polynomial();
            foreach (KeyValuePair<uint, double> kvp in _polynomials)
            {
                if (kvp.Key != 0)
                {
                    sum.Add(polynomial.Power(kvp.Key).Mult(0, kvp.Value));
                }
                else
                {
                    Polynomial constPolynomial = new Polynomial();
                    constPolynomial.Add(kvp.Key, kvp.Value);
                    sum.Add(constPolynomial);
                }
            }
            return sum;
        }

        public Polynomial Copy()
        {
            Polynomial newPolynomial = new Polynomial();

            foreach (uint power in _polynomials.Keys)
            {
                newPolynomial._polynomials[power] = _polynomials[power];
            }
            newPolynomial._degree = _degree;

            return newPolynomial;
        }
    }

    public class SturmSequence
    {
        List<Polynomial> _polynomials = new List<Polynomial>();
        public SturmSequence(Polynomial polynomial)
        {
            if (polynomial.Degree == 0)
            {
                throw new ArgumentException("polynomial");
            }
            _polynomials.Add(polynomial.Copy());
            _polynomials.Add(polynomial.Derivative());
            Polynomial atK = _polynomials[1];
            Polynomial atKMinus1 = _polynomials[0];
            Polynomial newPolynomial;
            Polynomial result;
            while (atK.Degree > 0)
            {
                Polynomial.Div(atKMinus1, atK, out result, out newPolynomial);
                newPolynomial = newPolynomial.Mult(0, -1);
                _polynomials.Add(newPolynomial);
                atKMinus1 = atK;
                atK = newPolynomial;
            }
        }

        private int FindNumberOfSignChanges(double value)
        {
            int numberOfSignChanges = 0;
            double currentValue;
            double oldValue = Math.Round(_polynomials[0].Eval(value), Constants.DecimalPlaces);
            double oldSign = Math.Sign(oldValue);
            double currentSign;
            for (int i = 1; i < _polynomials.Count; i++)
            {
                currentValue = Math.Round(_polynomials[i].Eval(value), Constants.DecimalPlaces);
                currentSign = Math.Sign(currentValue);
                if (currentSign * oldSign < 0)
                {
                    numberOfSignChanges++;
                }
                if (currentSign != 0)
                {
                    oldSign = currentSign;
                }
            }
            return numberOfSignChanges;
        }

        private int NumberOfRootsInRange(double min, double max)
        {
            return Math.Abs(FindNumberOfSignChanges(min) - FindNumberOfSignChanges(max));
        }

        public List<double> FindRoots(double min, double max)
        {
            int numberOfRoots = NumberOfRootsInRange(min, max);
            if (numberOfRoots == 0)
            {
                return null;
            }
            double difference = max - min;
            double midpoint = (min + max) / 2.0;

            if (Math.Abs(difference) < Constants.PolynomialEpsilon)
            {
                if (numberOfRoots == 1)
                {
                    List<double> singleRoot = new List<double>();
                    singleRoot.Add(Math.Round(midpoint, Constants.DecimalPlaces));
                    return singleRoot;
                }
                else
                {
                    return null;
                }
            }

            List<double> foundRoots;
            List<double> roots = new List<double>();

            foundRoots = FindRoots(min, midpoint);
            if (foundRoots != null)
            {
                roots.AddRange(foundRoots);
                if (roots.Count == numberOfRoots)
                {
                    return roots;
                }
            }
            foundRoots = FindRoots(midpoint, max);
            if (foundRoots != null)
            {
                roots.AddRange(foundRoots);
            }
            if (roots.Count != 0)
            {
                return roots;
            }
            return null;
        }
    }
}