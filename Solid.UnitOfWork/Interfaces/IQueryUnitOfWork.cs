using Solid.Models.Interfaces;

namespace Solid.UnitOfWork.Interfaces
{
    public interface IQueryUnitOfWork<T> where T : class, IEntity
    {
        T Find(params object[] ids);

        IReadOnlyList<T> Get(ISpecification<T> specification = null);

        IPageResult<T> GetPagedResult(ISpecification<T> specification);

        ValueTask<T> FindAsync(CancellationToken cancellationToken, params object[] ids);

        ValueTask<IReadOnlyList<T>> GetAsync(CancellationToken cancellationToken, ISpecification<T> specification = null);

        ValueTask<IPageResult<T>> GetPagedResultAsync(CancellationToken cancellationToken, ISpecification<T> specification);
    }
}