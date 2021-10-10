using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.viewModels
{
    public class UpdateEmployeeVM
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public DateTime Dob { get; set; }
        public string Department { get; set; }
    }
}
