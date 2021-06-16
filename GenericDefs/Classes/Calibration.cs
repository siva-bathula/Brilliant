using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDefs.Classes
{
    /// <summary>
    /// Use it in calibrating l.h.s. and r.h.s. with weights. 
    /// Store the weights in arraylist so you dont have to recheck again.
    /// Use this only for T of type numbers only.
    /// </summary>
    public class Calibration<T>
    {
        List<Failed<T>> failedSoFar = new List<Failed<T>>();
        HashSet<string> failed = new HashSet<string>();

        public bool WillThisFail(T[] lhs, T[] rhs) {
            if (failedSoFar.Count == 0) return false;

            bool willFail = false;
            Failed<T> fnew = new Failed<T>();
            fnew.Lhs = lhs;
            fnew.Rhs = rhs;

            if (failed.Contains(GetHashCode(fnew))) {
                willFail = true;
            }

            return willFail;
        }

        private string GetHashCode(Failed<T> f) {
            return f.GetHashLhs() + "--" + f.GetHashRhs();
        }

        public bool AddFailure(T[] lhs, T[] rhs)
        {
            Failed<T> fnew = new Failed<T>();
            fnew.Lhs = lhs;
            fnew.Rhs = rhs;
            bool retVal = false;
            if (!failed.Contains(GetHashCode(fnew)))
            {
                failedSoFar.Add(fnew);
                failed.Add(GetHashCode(fnew));
                retVal = true;
            }

            return retVal;
        }

        class Failed<T1> {
            internal T1[] Lhs;
            internal T1[] Rhs;

            string hashKeyLhs;
            string hashKeyRhs;

            internal string GetHashLhs() {
                if (!string.IsNullOrEmpty(hashKeyLhs)) { hashKeyLhs = string.Join<T1>("$%", Lhs); }
                return hashKeyLhs;
            }

            internal string GetHashRhs()
            {
                if (!string.IsNullOrEmpty(hashKeyRhs)) { hashKeyRhs = string.Join<T1>("$%", Rhs); }
                return hashKeyLhs;
            }
        }
    }
}