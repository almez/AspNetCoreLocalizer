using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Localizer.Site.Models;

namespace Localizer.Site.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            var model = new AboutViewModel()
            {
                CompanyName = "ALMEZ",
                Address = "M City, Ampang",
                Description = "Here you can find a short description about our company and what we do in general."
            };

            ViewData["Message"] = "Your application description page.";

            return View(model);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
