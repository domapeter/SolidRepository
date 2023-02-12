using System.Linq.Expressions;

namespace Solid.Models.Interfaces;

public interface ISpecification<T>
{
    Pagination Pagination { get; set; }
    Expression<Func<T, bool>> Filter { get; set; }
    List<Expression<Func<T, object>>> Includes { get; set; }

    List<KeyValuePair<Expression<Func<T, object>>, SortDirection>> OrderByList { get; set; }
}