using AutoMapper;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.viewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Employee, GetEmployeeVM>();
            CreateMap<AddEmployeeVM, Employee>();
        }
    }
}
