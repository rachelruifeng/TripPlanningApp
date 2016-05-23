using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace TheWorld.Controllers.Web
{
    using TheWorld.Models;
    using TheWorld.Services;
    using TheWorld.ViewModels;

    /// <summary>
    /// The app controller.
    /// </summary>
    public class AppController : Controller
    {
        private IMailService _mailService;

        private IWorldRepository _repository;

        public AppController(IMailService mailService, IWorldRepository repository)
        {
            _mailService = mailService;
            this._repository = repository; 
        }

        public IActionResult Index()
        {
            var trips = this._repository.GetAllTrips();

            return View(trips);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = Startup.Configuration["AppSettings:SiteEmailAddress"];

                if (string.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("","Could not send email, configuration problem.");
                }

                if (this._mailService.SendMail(
                    email,
                    email,
                    $"Contact page from {model.Name} ({model.Email})",
                    model.Message))
                {
                    ModelState.Clear();

                    ViewBag.Message = "Mail Send. Thanks";
                }
            }
            
            return View();
        }
    }
}
