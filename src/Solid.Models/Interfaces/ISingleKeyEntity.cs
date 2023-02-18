namespace Solid.Models.Interfaces;

public interface IEntity<Key1> : IEntity
{
    public Key1 Id { get; set; }
}