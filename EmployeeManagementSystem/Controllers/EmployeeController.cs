using EmployeeManagementSystem.IServices;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using EmployeeManagementSystem.viewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _employeeService.GetAllEmployee());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            return Ok(await _employeeService.GetEmployeeById(id));
        }

        
        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeVM newEmployee)
        {
            return Ok(await _employeeService.AddEmployee(newEmployee));
        }


        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeVM updateEmployee)
        {
            ServiceResponse<GetEmployeeVM> response = await _employeeService.UpdateEmployee(updateEmployee);

            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse<List<GetEmployeeVM>> response = await _employeeService.DeleteEmployee(id);

            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

    }
}
