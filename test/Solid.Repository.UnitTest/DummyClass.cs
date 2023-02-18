using Solid.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.Repository.UnitTest
{
    public class DummyClass : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}