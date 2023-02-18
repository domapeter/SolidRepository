using Solid.Models.Interfaces;
using System.Linq.Expressions;

namespace Solid.Models
{
    public class Specification<T> : ISpecification<T>
    {
        public Pagination Pagination { get; set; }
        public Expression<Func<T, bool>> Filter { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
        public List<KeyValuePair<Expression<Func<T, object>>, SortDirection>> OrderByList { get; set; }
    }

    public class Specification<T, TEndResult> : Specification<T>, ISpecification<T, TEndResult> where T : class, IEntity
    {
        public Func<IQueryable<T>, IQueryable<TEndResult>> Projection { get; set; }
    }
}