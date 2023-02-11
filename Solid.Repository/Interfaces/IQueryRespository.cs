namespace Solid.Repository.Interfaces
{
    public interface IQueryRespository<T> where T : class, IEntity
    {
        T Find(params object[] ids);

        ValueTask<T> FindAsync(params object[] ids);

        IReadOnlyList<T> Get();

        ValueTask<IReadOnlyList<T>> GetAsync();
    }
}