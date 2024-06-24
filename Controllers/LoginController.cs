using Microsoft.AspNetCore.Mvc;
using PTManagementSystem.Models;
using PTManagementSystem.Services;

namespace PTManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessLogin(UserLogin UserLogin)
        {
            SecurityService securityService = new SecurityService();
            if (securityService.IsValid(UserLogin))
            {
                return View("LoginSuccess", UserLogin);
            } 
            else
            {
                return View("LoginFailure", UserLogin);
            }
            
        }
    }
}
