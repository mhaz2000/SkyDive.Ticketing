using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SkyDiveTicketing.Infrastructure.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static void ApplyGlobalFilter<T>(this ModelBuilder modelBuilder, Expression<Func<T, bool>> expression)
        {
            var entityTypes = modelBuilder.Model.GetEntityTypes().Select(e => e.ClrType).ToList();

            foreach (var entityType in entityTypes)
            {
                if (!(entityType.BaseType == typeof(T) || entityType.GetInterface(typeof(T).Name) != null))
                    continue;

                if (entityTypes.Any(et => entityType.GetParentTypes().Contains(et)))
                    continue;

                var newParam = Expression.Parameter(entityType);
                var newBody =
                    ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
                modelBuilder.Entity(entityType).HasQueryFilter(Expression.Lambda(newBody, newParam));
            }
        }

        private static IEnumerable<Type> GetParentTypes(this Type type)
        {
            var currentBaseType = type.BaseType;
            while (currentBaseType != null)
            {
                yield return currentBaseType;
                currentBaseType = currentBaseType.BaseType;
            }
        }
    }
}
