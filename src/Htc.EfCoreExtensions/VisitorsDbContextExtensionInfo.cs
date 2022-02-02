using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Htc.EfCoreExtensions;

public class VisitorsDbContextExtensionInfo : DbContextOptionsExtensionInfo
{
    /// <inheritdoc />
    public VisitorsDbContextExtensionInfo(IDbContextOptionsExtension extension) : base(extension)
    {
        IsDatabaseProvider = false;
        LogFragment = "Ef Core Extensions";
    }

    /// <inheritdoc />
    public override int GetServiceProviderHashCode()
    {
        return 0;
    }

    /// <inheritdoc />
    public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
    {
        return false;
    }

    /// <inheritdoc />
    public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
    {
        
    }

    /// <inheritdoc />
    public override bool IsDatabaseProvider { get; }

    /// <inheritdoc />
    public override string LogFragment { get; }
}