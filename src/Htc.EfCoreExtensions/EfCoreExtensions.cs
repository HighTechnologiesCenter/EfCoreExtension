using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Htc.EfCoreExtensions;
public static class EfCoreExtensions
{
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
}
