using Solid.Models.Interfaces;
using Solid.Repository.Interfaces;
using Solid.UnitOfWork.Interfaces;

namespace Solid.UnitOfWork.Base;

public class BaseUnitOfWork<T> : IUnitOfWork<T> where T : class, IEntity
{
    protected IRepository<T> repository;

    public BaseUnitOfWork(IRepository<T> repository)
    {
        this.repository = repository;
    }

    public T Create(T entity)
    {
        repository.Create(entity);
        repository.SaveChanges();
        return entity;
    }

    public async ValueTask<T> CreateAsync(CancellationToken cancellationToken, T entity)
    {
        await repository.CreateAsync(cancellationToken, entity);
        await repository.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public void Delete(params object[] ids)
    {
        repository.Delete(ids);
        repository.SaveChanges();
    }

    public void Delete(T entity)
    {
        repository.Delete(entity);
        repository.SaveChanges();
    }

    public async Task DeleteAsync(CancellationToken cancellationToken, params object[] ids)
    {
        await repository.DeleteAsync(cancellationToken, ids);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(CancellationToken cancellationToken, T entity)
    {
        await repository.DeleteAsync(cancellationToken, entity);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public T Update(T entity)
    {
        repository.Update(entity);
        repository.SaveChanges();
        return entity;
    }

    public async ValueTask<T> UpdateAsync(CancellationToken cancellationToken, T entity)
    {
        await repository.UpdateAsync(cancellationToken, entity);
        await repository.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public T Find(params object[] ids) => repository.Find(ids);

    public ValueTask<T> FindAsync(CancellationToken cancellationToken, params object[] ids) => repository.FindAsync(cancellationToken, ids);

    public IReadOnlyList<T> Get(ISpecification<T> specification = null) => repository.Get(specification);

    public ValueTask<IReadOnlyList<T>> GetAsync(CancellationToken cancellationToken, ISpecification<T> specification = null) => repository.GetAsync(cancellationToken, specification);

    public IPageResult<T> GetPagedResult(ISpecification<T> specification) => repository.GetPagedResult(specification);

    public ValueTask<IPageResult<T>> GetPagedResultAsync(CancellationToken cancellationToken, ISpecification<T> specification) => repository.GetPagedResultAsync(cancellationToken, specification);
}