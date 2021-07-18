using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AVATI.Data
{
    [Route("/[controller]")]
    [ApiController]
    public class CookieController : ControllerBase
    {
        private int Id;
        private readonly IEmployeeService _employeeService;
        private readonly ILoginService _loginService;

        public CookieController(ILoginService loginService, IEmployeeService employeeService)
        {
            _loginService = loginService;
            _employeeService = employeeService;
        }
        
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromForm] string username, [FromForm] string password)
        {
            
            Id = _loginService.LogIn(username, password);
            if (Id == -1)
            {
                //return Redirect("/LoginFail");
                return Redirect("/Login/Fail");
            }
            if (Id == -2)
            {
                return Redirect("/profile/create/" + username);
            }
            Employee emp = _employeeService.GetEmployeeProfile(Id);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, emp.EmployeeID.ToString()),
                new(ClaimTypes.Role, emp.EmpType.ToString())
            }, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claims = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claims);
            return Redirect("/");
        }
        
        
        [HttpPost("Logout")]
        public async Task<ActionResult> LogOut()
        {
            if (HttpContext != null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Redirect("/");

                
            }
           
            return Redirect("/");
        }
        
        [HttpPost("Registration")]
        public async Task<ActionResult> Registration([FromForm] string username, [FromForm] string password, [FromForm] string passwordCheck)
        {
            if (password != passwordCheck)
            {
                return Redirect("/Registration/Fail");
            }

            if (!_loginService.CheckUsernameAvailable(username))
            {
                return Redirect("/Registration/Fail");
            }
            
            _loginService.CreateLogIn(username, password);
            return Redirect("/profile/create/" + username);
            
        }
    }
}