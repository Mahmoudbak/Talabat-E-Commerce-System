using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entity.Employees.Employee
{
    public class Depaetment:BaseEntity
    {
        public string Name { get; set; }
        public DateOnly DateOfCreation { get; set; }
    }
}
