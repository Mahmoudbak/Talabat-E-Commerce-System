using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entity.Employee;
using Talabat.Core.Repository.content;
using Talabat.Core.specifications.employeeSpec;

namespace Talabat.APIs.Controllers
{

    public class EmployeeController : BaseApiController
    {
        private readonly IGenaricrepository<Employee> _employeesrepo;

        public EmployeeController(IGenaricrepository<Employee> employeesrepo)
        {
            _employeesrepo = employeesrepo;
        }
        [HttpGet]// Get: /api/Employee
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployee()
        {
            var spec = new EmployeeWithDepartmenr_Spec();
            var employee = await _employeesrepo.GetAllwithSpecAsync(spec);
            return Ok(employee);
        }
        [HttpGet("{Id}")]//Get : /api/Employee/1
        public async Task<ActionResult<Employee>> GetEmployeeById(int Id)
        {
            var spec = new EmployeeWithDepartmenr_Spec(Id);
            var employee = await _employeesrepo.GetByIdwithSpecAsync(spec);
            return Ok(employee);
        }
    }
}
