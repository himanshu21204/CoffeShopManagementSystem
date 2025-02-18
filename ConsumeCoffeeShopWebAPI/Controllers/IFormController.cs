using Microsoft.AspNetCore.Mvc;

namespace ConsumeCoffeeShopWebAPI.Controllers
{
    public class IFormController : Controller
    {
        public IActionResult Form()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Form(IFormCollection f)
        {
            ViewBag.Name = f["name"];
            ViewBag.Age = f["Age"];
            return View("Form");
        }
    }
}
