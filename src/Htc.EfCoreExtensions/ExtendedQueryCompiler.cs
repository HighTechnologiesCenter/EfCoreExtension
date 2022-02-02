using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Htc.EfCoreExtensions;

#pragma warning disable EF1001 // Internal EF Core API usage.
public class ExtendedQueryCompiler : QueryCompiler

{
    private readonly VisitorsCollection _visitorsCollection;

    public ExtendedQueryCompiler(
        IQueryContextFactory queryContextFactory, 
        ICompiledQueryCache compiledQueryCache,
        ICompiledQueryCacheKeyGenerator compiledQueryCacheKeyGenerator, 
        IDatabase database,
        IDiagnosticsLogger<DbLoggerCategory.Query> logger, 
        ICurrentDbContext currentContext,
        IEvaluatableExpressionFilter evaluatableExpressionFilter, 
        IModel model,
        VisitorsCollection visitorsCollection) 
        : base(
            queryContextFactory,
            compiledQueryCache, 
            compiledQueryCacheKeyGenerator, 
            database, 
            logger, 
            currentContext,
            evaluatableExpressionFilter, 
            model)
    {
        _visitorsCollection = visitorsCollection;
    }

    /// <inheritdoc />
    public override TResult Execute<TResult>(Expression query)
    {
        foreach (var visitor in _visitorsCollection)
        {
            query = visitor.Visit(query);
        }
        return base.Execute<TResult>(query);
    }

    /// <inheritdoc />
    public override TResult ExecuteAsync<TResult>(Expression query, CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var visitor in _visitorsCollection)
        {
            query = visitor.Visit(query);
        }
        return base.ExecuteAsync<TResult>(query, cancellationToken);
    }
}
#pragma warning restore EF1001 // Internal EF Core API usage.