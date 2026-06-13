using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ENyayPath.PICS.Core.Helpers
{
    /// <summary>
    /// Helper class for building predicates with AND/OR operations.
    /// Useful for combining multiple filter conditions.
    /// </summary>
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> And<T>(
            Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var param = Expression.Parameter(typeof(T));
            var exprBody = Expression.AndAlso(
                Expression.Invoke(expr1, param),
                Expression.Invoke(expr2, param)
            );
            return Expression.Lambda<Func<T, bool>>(exprBody, param);
        }

        public static Expression<Func<T, bool>> Or<T>(
            Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var param = Expression.Parameter(typeof(T));
            var exprBody = Expression.OrElse(
                Expression.Invoke(expr1, param),
                Expression.Invoke(expr2, param)
            );
            return Expression.Lambda<Func<T, bool>>(exprBody, param);
        }
    }
}
