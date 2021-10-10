using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.viewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        //Register controller
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterVM request)
        {
            ServiceResponse<int> response = await _authRepo.Register(
                new User { Username = request.Username, }, request.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        //Login controller
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginVM request)
        {
            ServiceResponse<string> response = await _authRepo.Login(
             request.Username, request.Password
         );

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
