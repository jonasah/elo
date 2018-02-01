using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Elo.DbHandler
{
    static class ExtensionMethods
    {
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortOrder sortOrder)
        {
            if (sortOrder == SortOrder.Descending)
            {
                return source.OrderByDescending(keySelector);
            }

            // ascending or unspecified
            return source.OrderBy(keySelector);
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, SortOrder sortOrder)
        {
            if (sortOrder == SortOrder.Descending)
            {
                return source.OrderByDescending(keySelector);
            }

            // ascending or unspecified
            return source.OrderBy(keySelector);
        }
    }
}
