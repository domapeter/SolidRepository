namespace Solid.Repository.Interfaces
{
    public interface IEntity
    {
    }

    public interface IEntity<T>
    {
        public T Id { get; set; }
    }
}