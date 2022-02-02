using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Htc.EfCoreExtensions;

public class VisitorsDbContextExtension : IDbContextOptionsExtension
{
    private readonly VisitorsCollection _visitorsCollection;

    public VisitorsDbContextExtension(VisitorsCollection visitorsCollection)
    {
        _visitorsCollection = visitorsCollection;
    }

    /// <inheritdoc />
    public void ApplyServices(IServiceCollection services)
    {
        services.AddSingleton(_visitorsCollection);
    }

    /// <inheritdoc />
    public void Validate(IDbContextOptions options)
    {
        
    }

    /// <inheritdoc />
    public DbContextOptionsExtensionInfo Info => new VisitorsDbContextExtensionInfo(this);
}