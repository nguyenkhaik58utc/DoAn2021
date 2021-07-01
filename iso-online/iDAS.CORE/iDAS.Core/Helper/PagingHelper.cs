using System.Collections.Generic;
using System.Linq;

namespace iDAS.Core
{
    /// <summary>
    /// Paging Extension
    /// </summary>
    public static class PagingExtensions
    {
        /// <summary>
        /// Paging for IQueryable
        /// </summary>
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int pageIndex, int pageSize, out int totalCount)
        {
            totalCount = source.Count();
            return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        /// <summary>
        /// Paging for IEnumerable
        /// </summary>
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int pageIndex, int pageSize, out int totalCount)
        {
            totalCount = source.Count();
            return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}
