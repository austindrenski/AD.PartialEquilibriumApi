using System;
using JetBrains.Annotations;
// ReSharper disable LoopCanBeConvertedToQuery

namespace AD.PartialEquilibriumApi.Matrix
{
    /// <summary>
    /// Calculates the dot product.
    /// </summary>
    [PublicAPI]
    public static class DotProductExtensions
    {
        /// <summary>
        /// Calculates the dot product.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [Pure]
        public static double DotProduct(this double[] a, double[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentOutOfRangeException("Vectors must be equal in length.");
            }
            double result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result += a[i] * b[i];
            }
            return result;
        }

        /// <summary>
        /// Calculates the dot product.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [Pure]
        public static double[] DotProduct(this double[] a, double[][] b)
        {
            if (a.Length != b[0].Length)
            {
                throw new ArgumentOutOfRangeException("The count of rows in A must equal the count of columns in B.");
            }
            double[] result = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < b[0].Length; j++)
                {
                    result[i] += a[j] * b[j][i];
                }
            }
            return result;
        }

        /// <summary>
        /// Calculates the dot product.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [Pure]
        public static double[] DotProduct(this double[][] a, double[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentOutOfRangeException("The count of rows in A must equal the count of columns in B.");
            }
            double[] result = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    result[i] += a[i][j] * b[j];
                }
            }
            return result;
        }

        /// <summary>
        /// Calculates the dot product.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [Pure]
        public static double[][] DotProduct(this double[][] a, double[][] b)
        {
            if (a.Length != b[0].Length)
            {
                throw new ArgumentOutOfRangeException("The count of rows in A must equal the count of columns in B.");
            }
            double[][] result = new double[a.Length][];
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = new double[b[0].Length];
                for (int j = 0; j < b[0].Length; j++)
                {
                    for (int k = 0; k < b[0].Length; k++)
                    {
                        result[i][j] += a[i][k] * b[k][j];
                    }
                }
            }
            return result;
        }
    }
}
