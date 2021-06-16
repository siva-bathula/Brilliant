using System.Collections.Generic;
using System.Linq;
using GenericDefs.DotNet;

namespace Brilliant.ComputerScience._5
{
    public class RubiksCube : ISolve, IBrilliant
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get { return thisProblem; }
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("");
        }

        void ISolve.Solve()
        {
            RubiksCube2x2x2();
        }

        class UnitCube
        {
            internal int Id { get; set; }
            internal int[] Faces { get; set; }
            internal string Key { get { return GetFaceKey(); } }

            string GetFaceKey()
            {
                return Faces[0] + ":" + Faces[1] + ":" + Faces[2];
            }

            internal string GetConfig()
            {
                return Id + "#" + GetFaceKey();
            }
        }

        /// <summary>
        /// https://brilliant.org/practice/computer-science-warmups-level-5-challenges/?problem=mboxr_2mboxu_2-back-to-the-future-2
        /// </summary>
        internal void RubiksCube2x2x2()
        {
            Cubes = new List<UnitCube>();
            int id = 0;
            Cubes.Add(new UnitCube() { Id = ++id, Faces = new int[] { 1, 4, 5 } }); //1
            Cubes.Add(new UnitCube() { Id = ++id, Faces = new int[] { 1, 2, 5 } }); //2
            Cubes.Add(new UnitCube() { Id = ++id, Faces = new int[] { 1, 4, 6 } }); //3
            Cubes.Add(new UnitCube() { Id = ++id, Faces = new int[] { 1, 2, 6 } }); //4
            Cubes.Add(new UnitCube() { Id = ++id, Faces = new int[] { 2, 3, 5 } }); //5
            Cubes.Add(new UnitCube() { Id = ++id, Faces = new int[] { 3, 4, 5 } }); //6
            Cubes.Add(new UnitCube() { Id = ++id, Faces = new int[] { 2, 3, 6 } }); //7
            Cubes.Add(new UnitCube() { Id = ++id, Faces = new int[] { 3, 4, 6 } }); //8

            CubeKeys = new Dictionary<string, UnitCube>();
            Keys = new Dictionary<int, string>();
            Faces = new Dictionary<int, int[]>();
            Enumerable.Range(0, 8).ForEach(x => {
                CubeKeys.Add(Cubes[x].Key, Cubes[x]);
                Keys.Add(Cubes[x].Id, Cubes[x].Key);
                Faces.Add(Cubes[x].Id, Cubes[x].Faces);
            });

            string InitConfig = GetConfig();
            QueuedConsole.WriteImmediate("Init Config : {0}", InitConfig);
            int n = 0;
            while (true)
            {
                n++;
                PerformR2();
                PerformU2();
                string config = GetConfig();
                QueuedConsole.WriteImmediate("curr. Config : {0}", config);
                if (config.Equals(InitConfig)) { break; }
            }
            QueuedConsole.WriteImmediate("Number of R2U2 permutations : {0}", n);
        }

        List<UnitCube> Cubes { get; set; }
        Dictionary<string, UnitCube> CubeKeys { get; set; }
        Dictionary<int, string> Keys { get; set; }
        Dictionary<int, int[]> Faces { get; set; }

        void PerformR2()
        {
            //2 and 7.
            UnitCube c2 = CubeKeys[Keys[2]];
            UnitCube c7 = CubeKeys[Keys[7]];
            c2.Faces = Faces[7];
            c7.Faces = Faces[2];
            CubeKeys[Keys[2]] = c7;
            CubeKeys[Keys[7]] = c2;
            //4 and 5.
            UnitCube c4 = CubeKeys[Keys[4]];
            UnitCube c5 = CubeKeys[Keys[5]];
            c4.Faces = Faces[5];
            c5.Faces = Faces[4];
            CubeKeys[Keys[4]] = c5;
            CubeKeys[Keys[5]] = c4;
        }

        void PerformU2()
        {
            //1 and 5.
            UnitCube c1 = CubeKeys[Keys[1]];
            UnitCube c5 = CubeKeys[Keys[5]];
            c1.Faces = Faces[5];
            c5.Faces = Faces[1];
            CubeKeys[Keys[1]] = c5;
            CubeKeys[Keys[5]] = c1;
            //2 and 6.
            UnitCube c2 = CubeKeys[Keys[2]];
            UnitCube c6 = CubeKeys[Keys[6]];
            c2.Faces = Faces[6];
            c6.Faces = Faces[2];
            CubeKeys[Keys[2]] = c6;
            CubeKeys[Keys[6]] = c2;
        }

        string GetConfig()
        {
            string retVal = string.Empty;
            Enumerable.Range(0, Cubes.Count).ForEach(x => {
                retVal += Cubes[x].Key + "%";
            });
            return retVal;
        }
    }
}