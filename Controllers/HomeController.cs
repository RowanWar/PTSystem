using Microsoft.AspNetCore.Mvc;
using PTManagementSystem.Models;
using System.Diagnostics;
using Npgsql;
using PTManagementSystem.Helpers;

namespace PTManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //string username = Npgsql. 
            UserHelper.ConnectDB();
            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.Message = "Security is important";
            ViewBag.MyFavouriteColor = "Green";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
