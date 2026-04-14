using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity.Employees.Employee;

namespace Talabat.Core.Entity.Employee
{
    public class Employee:BaseEntity
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public decimal Salary { get; set; }
        public Depaetment Depaetment { get; set; }//Negtional prop {one}
    }

}
