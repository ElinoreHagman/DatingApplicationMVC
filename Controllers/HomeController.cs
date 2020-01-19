using Microsoft.AspNet.Identity;
using MVC_Dating_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Dating_Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var ctx = new MVCDbContext();
            //Hämtar ut alla profiler förutom användaren som är inloggad, och randomizar listan)
            var chosenProfiles = ctx.Profile.Where(p => p.ProfileId != userId).OrderBy(p => Guid.NewGuid()).ToList();
            var viewModel = new ProfileIndexViewModel
            {
                Profiles = chosenProfiles,
            };

            viewModel.Profiles.RemoveAll(x => x.IsActive == false);

            return View(viewModel);
        }
    }
}