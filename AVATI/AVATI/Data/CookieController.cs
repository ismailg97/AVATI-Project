using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
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
        
        [HttpPost]
        public async Task<ActionResult> Login([FromForm] string username, [FromForm] string password)
        {
            
            Id = _loginService.LogIn(username, password);
            if (Id == -1)
            {
                return Redirect("/LoginFail");
            }
            Employee emp = _employeeService.GetEmployeeProfile(Id);
            Console.WriteLine(emp.FirstName + emp.LastName);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, emp.EmployeeID.ToString()),
                new(ClaimTypes.Role, emp.EmpType.ToString())
            }, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claims = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claims);
            return Redirect("/");
        }
        
        
        [HttpDelete]
        public void LogOut()
        {
            Console.WriteLine("TEST");
            if (HttpContext != null)
            {
                //_httpContextAccessor.HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies", new CookieOptions()
                //{
                //    Secure = true,
                //});
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(-1);
                option.Secure = true;
                option.IsEssential = true;
                HttpContext.Response.Cookies.Append(".AspNetCore.Cookies", string.Empty, option);
                //Then delete the cookie
                HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
            }
            //return Redirect("login"); 
        }
    }
}