using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity.Employee;

namespace Talabat.Core.specifications.employeeSpec
{
    public class EmployeeWithDepartmenr_Spec:BaseSpecifcations<Employee>
    {

        public EmployeeWithDepartmenr_Spec():base()
        {
            Includes.Add(e => e.Depaetment);
        }
        public EmployeeWithDepartmenr_Spec(int id) : base(e => e.Id==id)
        {
            Includes.Add(e => e.Depaetment);
        }
    }
}
