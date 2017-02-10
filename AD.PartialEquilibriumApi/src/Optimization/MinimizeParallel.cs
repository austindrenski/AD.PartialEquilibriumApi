using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to run <see cref="Simplex"/> operations in parallel.
    /// </summary>
    public static class MinimizeParallelExtensions
    {
        /// <summary>
        /// Solves for the minimum value solution in parallel loops.
        /// </summary>
        /// <returns>The solution that produces the minimum value.</returns>
        public static Solution Minimize(this Simplex simplex, int loops)
        {
            ConcurrentBag<Simplex> concurrentBag = new ConcurrentBag<Simplex>();

            for (int i = 0; i < loops; i++)
            {
                concurrentBag.Add(
                    new Simplex(
                        simplex.ObjectiveFunction,
                        simplex.LowerBound,
                        simplex.UpperBound,
                        simplex.Dimensions,
                        null,
                        simplex.TextWriter
                    )
                );
            }

            Parallel.For(0, loops, i => concurrentBag.ElementAt(i).Minimize());

            return concurrentBag.Min(x => x.Solutions[0]);
        }
    }
}