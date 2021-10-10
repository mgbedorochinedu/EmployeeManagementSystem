using AutoMapper;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.IServices;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.viewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly AppDataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeService(IMapper mapper, AppDataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        //Create ID for the authenticated User method
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));


        //Create authenticated User Role method
        private string GetUserRole() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);


        //Add Employee method
        public async Task<ServiceResponse<List<GetEmployeeVM>>> AddEmployee(AddEmployeeVM newEmployee)
        {
            ServiceResponse<List<GetEmployeeVM>> serviceResponse = new ServiceResponse<List<GetEmployeeVM>>();
            Employee employee = _mapper.Map<Employee>(newEmployee);

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            serviceResponse.Data = (_context.Employees.Where(e => e.User.Id == GetUserId()).Select(e => _mapper.Map<GetEmployeeVM>(e))).ToList();

            return serviceResponse;

        }


        //Delete Employee by ID method
        public async Task<ServiceResponse<List<GetEmployeeVM>>> DeleteEmployee(int id)
        {
            ServiceResponse<List<GetEmployeeVM>> serviceResponse = new ServiceResponse<List<GetEmployeeVM>>();
            try
            {
                Employee employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id && e.User.Id == GetUserId());

                if (employee != null)
                {
                    _context.Employees.Remove(employee);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = (_context.Employees.Where(c => c.User.Id == GetUserId()).Select(c => _mapper.Map<GetEmployeeVM>(c))).ToList();
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Employee not found.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }


        //Get all Employee method
        public async Task<ServiceResponse<List<GetEmployeeVM>>> GetAllEmployee()
        {
            ServiceResponse<List<GetEmployeeVM>> serviceResponse = new ServiceResponse<List<GetEmployeeVM>>();
            List<Employee> dbEmployees =
            GetUserRole().Equals("Admin") ?
            await _context.Employees.ToListAsync() :
            await _context.Employees
            .Where(c => c.User.Id == GetUserId()).ToListAsync();

            serviceResponse.Data = (dbEmployees.Select(c => _mapper.Map<GetEmployeeVM>(c))).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetEmployeeVM>> GetEmployeeById(int id)
        {
            ServiceResponse<GetEmployeeVM> serviceResponse = new ServiceResponse<GetEmployeeVM>();
            Employee dbEmployee = await _context.Employees
            .FirstOrDefaultAsync(c => c.EmployeeId == id && c.User.Id == GetUserId());

            serviceResponse.Data = _mapper.Map<GetEmployeeVM>(dbEmployee);
            return serviceResponse;
        }

        //Update Employee method
        public async Task<ServiceResponse<GetEmployeeVM>> UpdateEmployee(UpdateEmployeeVM updatedEmployee)
        {
            ServiceResponse<GetEmployeeVM> serviceResponse = new ServiceResponse<GetEmployeeVM>();
            try
            {
                Employee employee = await _context.Employees.Include(c => c.User).FirstOrDefaultAsync(e => e.EmployeeId == updatedEmployee.EmployeeId);
                if (employee.User.Id == GetUserId())
                {
                    employee.FirstName = updatedEmployee.FirstName;
                    employee.LastName = updatedEmployee.LastName;
                    employee.Dob = updatedEmployee.Dob;
                    employee.Department = updatedEmployee.Department;

                    _context.Employees.Update(employee);
                    await _context.SaveChangesAsync();

                    serviceResponse.Data = _mapper.Map<GetEmployeeVM>(employee);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Employee not found.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
