using Microsoft.EntityFrameworkCore;
using Solid.Repository.UnitTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.Repository.test.Common
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var currentAssembly = GetType().Assembly;

            modelBuilder.ApplyConfigurationsFromAssembly(currentAssembly);
            modelBuilder.Entity<Student>();
            modelBuilder.Entity<Class>();
        }
    }
}