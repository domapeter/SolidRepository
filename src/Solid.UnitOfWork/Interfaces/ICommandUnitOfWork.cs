using Solid.Models.Interfaces;

namespace Solid.UnitOfWork.Interfaces;

public interface ICommandUnitOfWork<T> where T : IEntity
{
    T Create(T entity);

    ValueTask<T> CreateAsync(CancellationToken cancellationToken, T entity);

    void Delete(params object[] ids);

    Task DeleteAsync(CancellationToken cancellationToken, params object[] ids);

    void Delete(T entity);

    Task DeleteAsync(CancellationToken cancellationToken, T entity);

    T Update(T entity);

    ValueTask<T> UpdateAsync(CancellationToken cancellationToken, T entity);
}