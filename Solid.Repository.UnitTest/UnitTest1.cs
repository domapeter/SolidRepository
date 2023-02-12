using Solid.Repository.Base;
using System.Dynamic;

namespace Solid.Repository.UnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //var repo = new BaseRepository<DummyClass>(null);

            //var projects = repo.GetProjections(p => new DummyDTO()
            //{
            //    Id = p.Id,
            //    Name = p.Name,
            //}).Cast<DummyDTO>();

            Assert.Pass();
        }
    }
}