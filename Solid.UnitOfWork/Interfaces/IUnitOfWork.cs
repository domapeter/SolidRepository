using Solid.Models.Interfaces;

namespace Solid.UnitOfWork.Interfaces;

public interface IUnitOfWork<T> : IQueryUnitOfWork<T>, ICommandUnitOfWork<T> where T : class, IEntity
{
}