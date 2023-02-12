using Solid.Models.Interfaces;

namespace Solid.Repository.Interfaces;

public interface IRepository<T> : IQueryRespository<T>, ICommandRepository<T> where T : class, IEntity
{
}