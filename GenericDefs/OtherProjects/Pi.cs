using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace GenericDefs.OtherProjects
{
    public static class Pi
    {
        public static decimal ParallelPartitionerPi(int steps)
        {
            decimal sum = 0;
            decimal step = 1 / (decimal)steps;
            object obj = new object();

            Parallel.ForEach(
                Partitioner.Create(0, steps),
                () => 0.0,
                (range, state, partial) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        decimal x = (decimal)(1.0 * i - 0.5) * step;
                        partial += (double)(4 / (1 + x * x));
                    }

                    return partial;
                },
                partial => { lock (obj) sum += (decimal)partial; });

            return step * sum;
        }
    }
}