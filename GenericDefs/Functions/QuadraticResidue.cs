using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDefs.Functions
{
    public class QuadraticResidue
    {
        public static List<long> GetAllQuadraticResidues(long p) {
            List<long> residues = new List<long>();
            long Nmax = (long)Math.Floor(p / 2.0);
            long iter = 0, rem = 0;
            while (iter <= Nmax+1) {
                rem = (iter * iter) % p;
                if (!residues.Contains(rem)) { 
                    residues.Add(rem);
                }
                iter++;
            }
            return residues;
        }
    }
}