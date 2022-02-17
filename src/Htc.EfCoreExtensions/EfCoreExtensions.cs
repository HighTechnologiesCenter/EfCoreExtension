using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Htc.EfCoreExtensions;
public static class EfCoreExtensions
{
    /// <summary>
    ///     Добавление посетителя для оригинального выражения запроса.
    /// </summary>
    /// <param name="optionsBuilder"> Построитель настроек контекста. </param>
    /// <param name="visitorsBuilderAction"> Процедура добавления посетителей. </param>
    /// <returns></returns>
    public static DbContextOptionsBuilder AddOriginalExpressionVisitors(this DbContextOptionsBuilder optionsBuilder, 
        Action<ExpressionVisitorsBuilder> visitorsBuilderAction)
    {
#pragma warning disable EF1001 // Internal EF Core API usage.
        optionsBuilder.ReplaceService<IQueryCompiler, ExtendedQueryCompiler>();
#pragma warning restore EF1001 // Internal EF Core API usage.

        var visitorsBuilder = new ExpressionVisitorsBuilder();
        visitorsBuilderAction(visitorsBuilder);

        var visitorsCollection = new VisitorsCollection(visitorsBuilder);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(
            new VisitorsDbContextExtension(visitorsCollection));
        return optionsBuilder;
    }

    /// <summary>
    ///     Добавить расширение.
    /// </summary>
    /// <typeparam name="TExtension"></typeparam>
    /// <param name="optionsBuilder"></param>
    /// <param name="extension"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder AddExtension<TExtension>(this DbContextOptionsBuilder optionsBuilder,
        TExtension extension)
        where TExtension : class, IDbContextOptionsExtension
    {
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }
}
