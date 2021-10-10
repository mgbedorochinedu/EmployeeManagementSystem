using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.viewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.IServices
{
    public interface IEmployeeService
    {
        Task<ServiceResponse<List<GetEmployeeVM>>> GetAllEmployee();
        Task<ServiceResponse<GetEmployeeVM>> GetEmployeeById(int id);
        Task<ServiceResponse<List<GetEmployeeVM>>> AddEmployee(AddEmployeeVM newEmployee);
        Task<ServiceResponse<GetEmployeeVM>> UpdateEmployee(UpdateEmployeeVM updatedEmployee);
        Task<ServiceResponse<List<GetEmployeeVM>>> DeleteEmployee(int id);

    }
}
