using Solid.Models;
using Solid.Repository.test.Common;

namespace Solid.Repository.UnitTest
{
    public class Student : SingleKeyEntity<Guid>
    {
        public string Name { get; set; }

        public ICollection<Class> Classes { get; set; }
    }
}