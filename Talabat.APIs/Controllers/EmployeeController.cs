using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications.Employee_Specs;
using Talabat.Core.Specifications.Products_Specs;

namespace Talabat.APIs.Controllers
{
	public class EmployeeController : BaseApiController
	{
		private readonly IGenricRepository<Employee> _employeeRepo;

		public EmployeeController(IGenricRepository<Employee> employeeRepo)
		{
			_employeeRepo = employeeRepo;
		}
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
		{
			var spec = new EmployeeWithDepartmentSpecifications();
			var employees = await _employeeRepo.GetAllWithSpecAsync(spec);
			return Ok(employees);
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<Employee>> GetEmployee(int id)
		{
			var spec = new EmployeeWithDepartmentSpecifications(id);

			var employee = await _employeeRepo.GetwithSpecAsync(spec);

			if (employee == null)
				return NotFound(new { Message = "Not Found", StatusCode = 404 });

			return Ok(employee);
		}
	}
}



