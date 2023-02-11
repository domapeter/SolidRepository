namespace Solid.Repository.Interfaces
{
    public interface ICommandRepository<T> where T : class, IEntity
    {
        T Create(T entity);
        ValueTask<T> CreateAsync(T entity);

        void Delete(params object[] ids);
        Task DeleteAsync(params object[] ids);

        void Delete(T entity);
        Task DeleteAsync(T entity);

        T Update(T entity);
        ValueTask<T> UpdateAsync(T entity);
    }
}