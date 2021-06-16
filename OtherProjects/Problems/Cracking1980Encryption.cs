using GenericDefs.Classes;
using GenericDefs.DotNet;
using GenericDefs.OtherProjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OtherProjects.Problems
{
    public class Cracking1980Encryption
    {
        static Random random = new Random(DateTime.Now.Millisecond);

        public static void CrackingEncryptionProblem()
        {
            //26390 85015 23339 22027 40949 38630 97432 59521 51779 38861 43240 989

            BigInteger number = BigInteger.Parse("2639085015233392202740949386309743259521517793886143240989");

            Primality primality = new Primality();
            if (primality.IsPrimeMillerRabin(number))
            {
                Console.WriteLine("Its a Miller Rabin Prime!!");
            }
            else { Console.WriteLine("Not a miller rabin prime."); }

            List<BigInteger> bl = new List<BigInteger>();
            bl.Add(BigInteger.Parse("618970019642690137449562111"));
            bl.Add(BigInteger.Parse("162259276829213363391578010288127"));

            foreach (BigInteger b in bl)
            {
                if (number % b == 0)
                {
                    Console.WriteLine("This is a factor! b :: {0}", b.ToString());
                    number /= b;
                }
            }

            int trials = 500000;
            int counter = 0;
            SpecialConsole q = new SpecialConsole(1000, 1000);
            q.LazyWriteLastMessage = true;

            List<BigInteger> factors = new List<BigInteger>();

            string logDump = string.Empty;
            while (counter <= trials)
            {
                BigInteger b = GenerateLargePrime(random.Next(15, 20));
                if (primality.IsPrimeMillerRabin(b))
                {
                    counter++;
                    q.LazySuppressWrite(string.Format("Primes tried :: {0} ", counter));
                    if (number % b == 0)
                    {
                        factors.Add(b);
                        string s1 = string.Format("This is a factor! b :: {0}", b.ToString());
                        Console.WriteLine(s1);
                        number /= b;
                        string s2 = string.Format("Number factored by b :: {0}", number.ToString());
                        Console.WriteLine(s2);
                    }
                }
            }
            Logger.Log(logDump);

            Console.ReadKey();
        }

        static BigInteger GenerateLargePrime(int length)
        {
            Primality primality = new Primality();
            string numbers = "";

            for (int i = 0; i < length; i++)
            {
                numbers += random.Next(0, 10);
            }

            BigInteger number = BigInteger.Parse(numbers);

            if (primality.IsPrimeMillerRabin(number))
            {
                return number;
            }
            else
            {
                return GenerateLargePrime(length);
            }
        }
    }
}