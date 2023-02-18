using Solid.Models;
using Solid.Repository.UnitTest;

namespace Solid.Repository.test.Common
{
    public class Class : SingleKeyEntity<Guid>
    {
        public string Name { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}