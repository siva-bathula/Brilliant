using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericDefs.Classes.Logic
{
    /// <summary>
    /// https://brilliant.org/practice/arithmetic-puzzles-level-3-4-challenges/?p=3
    /// Example. N candles are burning. With a given overlap between candles.
    /// </summary>
    public class SimpleSlider
    {
        /// <summary>
        /// Find overlap with slides, given input of other overlaps(Parameter k is the one to solve for).
        /// </summary>
        /// <param name="lengths"></param>
        /// <param name="overlap">Overlapping dictionaries.</param>
        /// <param name="k">If k = 1, then parts of slides for which there is no over lap with other slides. 
        /// If k = 2, meaning parts of all slides for which there is overlap with one other slide.</param>
        public static void SolveKthOverlap(int[] lengths, Dictionary<int, int> overlap, int k)
        {
            int tL = lengths.Sum();
            Array.Sort(lengths);
            lengths.Reverse();

            List<Slider> sliders = new List<Slider>();
            foreach (int n in lengths)
            {
                sliders.Add(new Slider(n, 0, tL));
            }
            Recursion(1, ref sliders, overlap, k);
        }

        private static bool Recursion(int depth, ref List<Slider> sliders, Dictionary<int, int> overlap, int k)
        {
            if (depth == sliders.Count)
            {
                bool conditionsMet = true;
                foreach (KeyValuePair<int, int> kvp in overlap)
                {
                    if (sliders.Sum(x => x.GetPartitionsSharedWithN(kvp.Key - 1)) != kvp.Value) { conditionsMet = false; break; }
                }
                if (conditionsMet) return true;
            } else {
                Recursion(depth + 1, ref sliders, overlap, k);
            }
            return false;
        }
    }

    internal class Slider
    {
        public int Length { get; set; }
        public string Name { get; private set; }
        public SliderPos1D CurrentPosition { get; set; }
        int _startPos = 0;
        int _maxPos = 0;
        SliderPartitions _partitions;
        public Slider(int length, int start, int end)
        {
            CurrentPosition = new SliderPos1D();
            Name = Guid.NewGuid().ToString();
            Length = length;
            _startPos = start;
            CurrentPosition.Start = start;
            CurrentPosition.End = start + Length;
            _maxPos = end;
            _partitions = new SliderPartitions(this);
        }

        internal int GetPartitionsSharedWithN(int N)
        {
            return _partitions.GetPartitionsSharedWithN(N);
        }

        /// <summary>
        /// Slides by 1 unit. Returns false, if cannot slide, end reached.
        /// </summary>
        /// <returns></returns>
        public bool SlideDefault()
        {
            if (CurrentPosition.End < _maxPos)
            {
                CurrentPosition.Start += 1;
                CurrentPosition.End += 1;
                return true;
            }
            else return false;
        }

        public void Reset()
        {
            CurrentPosition.Start = _startPos;
            CurrentPosition.End = _startPos + Length;
        }
    }

    internal class SliderPartitions
    {
        public SliderPartitions(Slider s)
        {
            S = s;
        }
        Slider S { get; set; }
        List<Partition> Partitions = new List<Partition>();
        internal void CompareAndUpdatePartitions(Slider s2)
        {
            Overlap ov = new Overlap();
            if (S.CurrentPosition.Start <= s2.CurrentPosition.Start)
            {
                if (S.CurrentPosition.End <= s2.CurrentPosition.Start) return;
                else if (S.CurrentPosition.End <= s2.CurrentPosition.End)
                {
                    ov.Start = s2.CurrentPosition.Start;
                    ov.End = S.CurrentPosition.End;
                }
                else if (S.CurrentPosition.End >= s2.CurrentPosition.End)
                {
                    ov.Start = s2.CurrentPosition.Start;
                    ov.End = s2.CurrentPosition.End;
                }
            }
            else if (S.CurrentPosition.Start >= s2.CurrentPosition.Start)
            {
                if (s2.CurrentPosition.End <= S.CurrentPosition.Start) return;
                else if (S.CurrentPosition.End <= s2.CurrentPosition.End)
                {
                    ov.Start = S.CurrentPosition.Start;
                    ov.End = s2.CurrentPosition.End;
                }
                else if (S.CurrentPosition.End >= s2.CurrentPosition.End)
                {
                    ov.Start = S.CurrentPosition.Start;
                    ov.End = S.CurrentPosition.End;
                }
            }
        }

        /// <summary>
        /// Returns lengths of partitions shared with 'N' other partitions/slides.
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        internal int GetPartitionsSharedWithN(int N)
        {
            return Partitions.Where(x => x.SharedWith == N).Sum(x => x.Length);
        }
    }

    internal class Overlap
    {
        public int Start { get; set; }
        public int End { get; set; }
    }

    internal class Partition
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Length { get; set; }
        public int SharedWith { get; set; }
    }

    internal class SliderPos1D
    {
        public int Start { get; set; }
        public int End { get; set; }
    }
}