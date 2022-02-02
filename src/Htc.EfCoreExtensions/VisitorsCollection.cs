using System.Collections;
using System.Linq.Expressions;

namespace Htc.EfCoreExtensions;

public class VisitorsCollection : IEnumerable<ExpressionVisitor>
{
    private readonly IEnumerable<ExpressionVisitor> _visitors;

    public VisitorsCollection(IEnumerable<ExpressionVisitor> visitors)
    {
        _visitors = visitors;
    }

    public IEnumerator<ExpressionVisitor> GetEnumerator()
    {
        return _visitors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_visitors).GetEnumerator();
    }
}