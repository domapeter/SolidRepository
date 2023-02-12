using Solid.Models.Interfaces;
using Solid.Repository.Interfaces;
using Solid.UnitOfWork.Interfaces;

namespace Solid.UnitOfWork.Base
{
    public class BaseUnitOfWork<T> : IUnitOfWork<T> where T : class, IEntity
    {
        protected IRepository<T> repository;

        public BaseUnitOfWork(IRepository<T> repository)
        {
            this.repository = repository;
        }

        public T Find(params object[] ids) => repository.Find(ids);

        public ValueTask<T> FindAsync(CancellationToken cancellationToken, params object[] ids) => repository.FindAsync(cancellationToken, ids);

        public IReadOnlyList<T> Get(ISpecification<T> specification = null) => repository.Get(specification);

        public ValueTask<IReadOnlyList<T>> GetAsync(CancellationToken cancellationToken, ISpecification<T> specification = null) => repository.GetAsync(cancellationToken, specification);

        public IPageResult<T> GetPagedResult(ISpecification<T> specification) => repository.GetPagedResult(specification);

        public ValueTask<IPageResult<T>> GetPagedResultAsync(CancellationToken cancellationToken, ISpecification<T> specification) => repository.GetPagedResultAsync(cancellationToken, specification);
    }
}