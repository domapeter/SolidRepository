using Solid.Models.Interfaces;

namespace Solid.Models;

public class SingleKeyEntity<K> : IEntity<K>
{
    public K Id { get; set; }
}