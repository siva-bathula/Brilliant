using System;
using System.Collections.Generic;

namespace OtherProjects
{
    class Person
    {
        private int _candidateIndex;
        public string Name { get; set; }
        public List<Person> Prefs { get; set; }
        public Person Fiance { get; set; }

        public Person(string name)
        {
            Name = name;
            Prefs = null;
            Fiance = null;
            _candidateIndex = 0;
        }
        public bool Prefers(Person p)
        {
            return Prefs.FindIndex(o => o == p) < Prefs.FindIndex(o => o == Fiance);
        }
        public Person NextCandidateNotYetProposedTo()
        {
            if (_candidateIndex >= Prefs.Count) return null;
            return Prefs[_candidateIndex++];
        }
        public void EngageTo(Person p)
        {
            if (p.Fiance != null) p.Fiance.Fiance = null;
            p.Fiance = this;
            if (Fiance != null) Fiance.Fiance = null;
            Fiance = p;
        }
    }

    static class StableMatchingProblem
    {
        static public bool IsStable(List<Person> men)
        {
            List<Person> women = men[0].Prefs;
            foreach (Person guy in men)
            {
                foreach (Person gal in women)
                {
                    if (guy.Prefers(gal) && gal.Prefers(guy))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// https://brilliant.org/problems/can-you-marry-them/
        /// </summary>
        static dynamic rankMen = new dynamic[,]
                    { { 7, 3, 8, 9, 6, 4, 2, 1, 5 },
                     { 5, 4, 8, 3, 1, 2, 6, 7, 9 },
                     { 4, 8, 3, 9, 7, 5, 6, 1, 2 },
                     { 9, 7, 4, 2, 5, 8, 3, 1, 6 },
                     { 2, 6, 4, 9, 8, 7, 5, 1, 3 },
                     { 2, 7, 8, 6, 5, 3, 4, 1, 9 },
                     { 1, 6, 2, 3, 8, 5, 4, 9, 7 },
                     { 5, 6, 9, 1, 2, 8, 4, 3, 7 },
                     { 6, 1, 4, 7, 5, 8, 3, 9, 2 }
                 };

        static dynamic rankWomen = new dynamic[,]
            { { 3, 1, 5, 2, 8, 7, 6, 9, 4 },
                     { 9, 4, 8, 1, 7, 6, 3, 2, 5 },
                     { 3, 1, 8, 9, 5, 4, 2, 6, 7 },
                     { 8, 7, 5, 3, 2, 6, 4, 9, 1 },
                     { 6, 9, 2, 5, 1, 4, 7, 3, 8 },
                     { 2, 4, 5, 1, 6, 8, 3, 9, 7 },
                     { 9, 3, 8, 2, 7, 5, 4, 6, 1 },
                     { 6, 3, 2, 1, 8, 4, 5, 9, 7 },
                     { 8, 2, 6, 4, 9, 1, 3, 7, 5 }
        };

        public static void DoMarriage()
        {
            Person abi = new Person("1");
            Person bea = new Person("2");
            Person cath = new Person("3");
            Person dee = new Person("4");
            Person eve = new Person("5");
            Person fay = new Person("6");
            Person gay = new Person("7");
            Person hope = new Person("8");
            Person jan = new Person("9");

            Person abe = new Person("1");
            Person bob = new Person("2");
            Person col = new Person("3");
            Person dan = new Person("4");
            Person ed = new Person("5");
            Person fred = new Person("6");
            Person gav = new Person("7");
            Person hal = new Person("8");
            Person ian = new Person("9");

            abe.Prefs = new List<Person>() { gay, cath, hope, jan, fay, dee, bea, abi, eve };
            bob.Prefs = new List<Person>() { eve, dee, hope, cath, abi, bea, fay, gay, jan };
            col.Prefs = new List<Person>() { dee, hope, cath, jan, gay, eve, fay, abi, bea };
            dan.Prefs = new List<Person>() { jan, gay, dee, bea, eve, hope, cath, abi, fay };
            ed.Prefs = new List<Person>() { bea, fay, dee, jan, hope, gay, eve, abi, cath };
            fred.Prefs = new List<Person>() { bea, gay, hope, fay, eve, cath, dee, abi, jan };
            gav.Prefs = new List<Person>() { abi, fay, bea, cath, hope, eve, dee, jan, gay };
            hal.Prefs = new List<Person>() { eve, fay, jan, abi, bea, hope, dee, cath, gay };
            ian.Prefs = new List<Person>() { fay, abi, dee, gay, eve, hope, cath, jan, bea };
            
            abi.Prefs = new List<Person>() { col, abe, ed, bob, hal, gav, fred, ian, dan };
            bea.Prefs = new List<Person>() { ian, dan, hal, abe, gav, fred, col, bob, ed };
            cath.Prefs = new List<Person>() { col, abe, hal, ian, ed, dan, bob, fred, gav };
            dee.Prefs = new List<Person>() { hal, gav, ed, col, bob, fred, dan, ian, abe };
            eve.Prefs = new List<Person>() { fred, ian, bob, ed, abe, dan, gav, col, hal };
            fay.Prefs = new List<Person>() { bob, dan, ed, abe, fred, hal, col, ian, gav };
            gay.Prefs = new List<Person>() { ian, col, hal, bob, gav, ed, dan, fred, abe };
            hope.Prefs = new List<Person>() { fred, col, bob, abe, hal, dan, ed, ian, gav };
            jan.Prefs = new List<Person>() { hal, bob, fred, dan, ian, abe, col, gav, ed };

            List<Person> men = new List<Person>(abi.Prefs);

            int freeMenCount = men.Count;
            while (freeMenCount > 0)
            {
                foreach (Person guy in men)
                {
                    if (guy.Fiance == null)
                    {
                        Person gal = guy.NextCandidateNotYetProposedTo();
                        if (gal.Fiance == null)
                        {
                            guy.EngageTo(gal);
                            freeMenCount--;
                        }
                        else if (gal.Prefers(guy))
                        {
                            guy.EngageTo(gal);
                        }
                    }
                }
            }

            foreach (Person guy in men)
            {
                Console.WriteLine("{0} is engaged to {1}", guy.Name, guy.Fiance.Name);
            }
            Console.WriteLine("Stable = {0}", IsStable(men));
        }
    }
}