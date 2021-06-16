using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericDefs.DotNet;

namespace GenericDefs.Classes
{
    public class ParityObjects
    {
        int _nObjects = 0;
        public int NObjects { get { return _nObjects; } internal set { _nObjects = value; } }

        bool[] Objects { get; set; }
        public ParityObjects(int nObjects)
        {
            NObjects = nObjects;
            Objects = new bool[nObjects];
        }

        public void Reset()
        {
            Array.Clear(Objects, 0, Objects.Length);
        }

        public void SetParity(int index, bool parity)
        {
            Objects[index] = parity;
        }

        public bool IsAllEven()
        {
            return !Objects.Contains(false);
        }

        public bool IsAllOdd()
        {
            return !Objects.Contains(true);
        }

        public int CountEvenParity()
        {
            return Objects.Count(x => { return x == true; });
        }

        public int CountOddParity()
        {
            return Objects.Count(x => { return x == false; });
        }
    }
}