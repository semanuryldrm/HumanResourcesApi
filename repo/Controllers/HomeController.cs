using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}