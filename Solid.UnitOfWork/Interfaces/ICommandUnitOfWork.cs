using Solid.Models.Interfaces;

namespace Solid.UnitOfWork.Interfaces
{
    public interface ICommandUnitOfWork<T> where T : IEntity
    {
    }
}