using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ConsumeCoffeeShopWebAPI;
using ConsumeCoffeeShopWebAPI.Models;

namespace ConsumeCoffeeShopWebAPI.Controllers
{
	[CheckAccess]
	public class MailController : Controller
	{
        private readonly IEmailSender _emailSender;
        public MailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
		#region Email Form(Get)
		[HttpGet]
		public IActionResult Form()
		{
			return View();
		}
        #endregion
        #region Post method
        [HttpPost]
		public async Task<IActionResult> Form(Mail model)
		{
			if (ModelState.IsValid)
			{
				try
				{
                    string message = $"<h1>{model.Subject}</h1><br /><p>{model.Message}</p>";
                    await _emailSender.SendEmail(model.Email, model.Subject, message);
                }
				catch (Exception e)
				{
                    TempData["Error"] = e.Message;
                }


                TempData["MailSent"] = "Success";
                return RedirectToAction("Form");
			}

			return View(model);
		}
        #endregion
	}
}
