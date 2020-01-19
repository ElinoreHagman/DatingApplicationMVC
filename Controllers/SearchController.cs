using Microsoft.AspNet.Identity;
using MVC_Dating_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Dating_Project.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult SearchProfile(string SearchText)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var ctx = new MVCDbContext();
            var profiles = ctx.Profile.Where(p => p.ProfileId != userId).ToList();

            var profileActive = ctx.Profile.FirstOrDefault(p => p.ProfileId == userId);
            if(!profileActive.IsActive)
            {
                return RedirectToAction("EditProfileData", "Profile");
            }

            if (!string.IsNullOrEmpty(SearchText))
            {

                string[] names = SearchText.Split(' ');
                List<Profile> result;
                if(names.Length == 1)
                {
                result = profiles.Where(
                    p => (p.FirstName.ToLower().Contains(SearchText.ToLower())
                    || p.LastName.ToLower().Contains(SearchText.ToLower()))
                    && p.IsActive == true
                    ).ToList();
                } else
                {
                    result = profiles.Where(p => (p.FirstName.ToLower().Contains(names[0].ToLower()) && p.LastName.ToLower().Contains(names[1].ToLower())) && p.IsActive == true).ToList();
                }


                return View(result);
            } else
            {
                profiles.Clear();
                return View(profiles);
            }
        }
    }
}