using Microsoft.EntityFrameworkCore;
using Solid.Repository.Base;
using Solid.Repository.Interfaces;
using Solid.Repository.test.Common;

namespace Solid.Repository.UnitTest
{
    public class QueryStudentRepositoryTests
    {
        public DbContextOptions<SchoolContext> dbContextOptions;
        public SchoolContext context;
        public IQueryRespository<Student> repository;

        [SetUp]
        public void Setup()
        {
            dbContextOptions = new DbContextOptionsBuilder<SchoolContext>()
            .UseInMemoryDatabase(databaseName: "StudentDb" + Guid.NewGuid())
            .Options;
            context = new SchoolContext(dbContextOptions);

            repository = new BaseRepository<Student>(context);
        }

        [Test]
        public void GetStudentsEmpty()
        {
            var students = repository.Get();

            Assert.AreEqual(0, students.Count());
        }

        [Test]
        public void GetStudentsContainsElements()
        {
            context.Add(new Student() { Id = Guid.NewGuid(), Name = "john", Classes = null });
            context.Add(new Student() { Id = Guid.NewGuid(), Name = "Doe", Classes = null });
            context.SaveChanges();

            var students = repository.Get();

            Assert.AreNotEqual(0, students.Count());
            Assert.AreEqual(2, students.Count());
        }

        [Test]
        public void FindStudentSuccessFull()
        {
            var guid = Guid.NewGuid();
            var name = "john";

            context.Add(new Student() { Id = guid, Name = name, Classes = null });
            context.SaveChanges();

            var student = repository.Find(guid);

            Assert.NotNull(student);
            Assert.AreEqual(name, student.Name);
        }
    }
}