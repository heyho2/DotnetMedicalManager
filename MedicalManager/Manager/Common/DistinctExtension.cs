using System;
using System.Collections.Generic;

namespace GD.Manager.Common
{
    public static class DistinctExtension
    {
        public static IEnumerable<TSource> DistinctBy<TSource, Tkey>(
            this IEnumerable<TSource> source, Func<TSource, Tkey> selector) where TSource : class
        {
            var hashSet = new HashSet<Tkey>();

            foreach (var item in source)
            {
                if (hashSet.Add(selector(item)))
                {
                    yield return item;
                }
            }
        }
    }
}
