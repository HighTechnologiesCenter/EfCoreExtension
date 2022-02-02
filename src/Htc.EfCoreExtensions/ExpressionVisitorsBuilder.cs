using System.Collections;
using System.Linq.Expressions;

namespace Htc.EfCoreExtensions;

public class ExpressionVisitorsBuilder : ICollection<ExpressionVisitor>
{
    private ICollection<ExpressionVisitor> _visitors = new List<ExpressionVisitor>();

    int ICollection<ExpressionVisitor>.Count => _visitors.Count;

    bool ICollection<ExpressionVisitor>.IsReadOnly => _visitors.IsReadOnly;

    public void Add(ExpressionVisitor item)
    {
        _visitors.Add(item);
    }

    void ICollection<ExpressionVisitor>.Clear()
    {
        _visitors.Clear();
    }

    bool ICollection<ExpressionVisitor>.Contains(ExpressionVisitor item)
    {
        return _visitors.Contains(item);
    }

    void ICollection<ExpressionVisitor>.CopyTo(ExpressionVisitor[] array, int arrayIndex)
    {
        _visitors.CopyTo(array, arrayIndex);
    }

    IEnumerator<ExpressionVisitor> IEnumerable<ExpressionVisitor>.GetEnumerator()
    {
        return _visitors.GetEnumerator();
    }

    bool ICollection<ExpressionVisitor>.Remove(ExpressionVisitor item)
    {
        return _visitors.Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_visitors).GetEnumerator();
    }
}