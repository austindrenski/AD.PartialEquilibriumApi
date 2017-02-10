using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to run <see cref="Simplex"/> operations in parallel.
    /// </summary>
    [PublicAPI]
    public static class MinimizeParallelExtensions
    {
        /// <summary>
        /// Solves for the minimum value solution in parallel loops.
        /// </summary>
        /// <returns>The solution that produces the minimum value.</returns>
        public static Solution Minimize(this Simplex simplex, int parallelLoops)
        {
            ConcurrentBag<Simplex> concurrentBag = new ConcurrentBag<Simplex>();

            for (int i = 0; i < parallelLoops; i++)
            {
                concurrentBag.Add(
                    new Simplex(
                        simplex.ObjectiveFunction.Clone() as Func<double[], double>,
                        simplex.LowerBound,
                        simplex.UpperBound,
                        simplex.Dimensions,
                        simplex.Iterations,
                        null,
                        simplex.TextWriter
                    )
                );
            }

            Parallel.ForEach(concurrentBag, x => x.Minimize());

            foreach (Simplex localSimplex in concurrentBag.OrderBy(x => x[0]))
            {
                simplex.TextWriter.WriteLine($"Parallel result: {localSimplex[0]}");
            }

            return concurrentBag.Min(x => x.Solutions[0]);
        }
    }
}