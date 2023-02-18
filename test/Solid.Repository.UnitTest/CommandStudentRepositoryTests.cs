using Microsoft.EntityFrameworkCore;
using Solid.Repository.Base;
using Solid.Repository.Interfaces;
using Solid.Repository.test.Common;

namespace Solid.Repository.UnitTest;

public class CommandStudentRepositoryTests
{
    public DbContextOptions<SchoolContext> dbContextOptions;
    public SchoolContext context;
    public ICommandRepository<Student> repository;

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
    public void CreateRepositoryCallNotSavingObject()
    {
        var guid = Guid.NewGuid();
        var name = "john";

        var student = new Student() { Id = guid, Name = name, Classes = null };

        var createdStudent = repository.Create(student);

        Assert.NotNull(createdStudent);
        Assert.AreEqual(0, context.Set<Student>().Count());
    }

    [Test]
    public void CreateHasTheObjectAsEntry()
    {
        var guid = Guid.NewGuid();
        var name = "john";

        var student = new Student() { Id = guid, Name = name, Classes = null };

        var createdStudent = repository.Create(student);

        Assert.NotNull(createdStudent);
        Assert.AreEqual(createdStudent, context.Entry(student).Entity);
        Assert.AreEqual(EntityState.Added, context.Entry(student).State);
    }
}