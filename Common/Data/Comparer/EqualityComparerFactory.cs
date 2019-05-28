using System;
using System.Collections.Generic;

namespace CommonLibs.Data
{
    public static class EqualityComparerFactory
    {
        /// <summary>
        /// Creates new <see cref="IEqualityComparer{T}"/> class
        /// </summary>
        /// <typeparam name="T">compare values type</typeparam>
        /// <param name="getHashFunc">get hash code func</param>
        /// <param name="equalFunc">compare func</param>
        /// <returns></returns>
        public static EqualityComparer<T> Create<T>(Func<T, int> getHashFunc, Func<T, T, bool> equalFunc)
        {
            if (getHashFunc == null)
            {
                throw new ArgumentNullException(nameof(getHashFunc));
            }

            if (equalFunc == null)
            {
                throw new ArgumentNullException(nameof(equalFunc));
            }

            return new EqualityComparer<T>(getHashFunc, equalFunc);
        }


        public class EqualityComparer<T> : IEqualityComparer<T>
        {
            Func<T, int> getHashFunc;
            Func<T, T, bool> equalFunc;

            public EqualityComparer(Func<T, int> getHashFunc, Func<T, T, bool> equalFunc)
            {
                this.equalFunc = equalFunc;
                this.getHashFunc = getHashFunc;
            }

            public bool Equals(T x, T y) => equalFunc(x, y);

            public int GetHashCode(T obj) => getHashFunc(obj);
        }

    }
}
