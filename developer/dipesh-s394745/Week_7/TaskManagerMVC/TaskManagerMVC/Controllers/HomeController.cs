using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new HomeIndexViewModel());
        }

        public IActionResult Privacy()
        {
            return View(new PrivacyViewModel());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult StatusCodeHandler(int code)
        {
            var model = new StatusCodeViewModel
            {
                StatusCode = code,
                Message = code switch
                {
                    404 => "The page you're looking for could not be found.",
                    _ => "Something went wrong while processing your request."
                }
            };

            return View("StatusCode", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
