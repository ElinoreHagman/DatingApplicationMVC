using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVC_Dating_Project.Models;

namespace MVC_Dating_Project.Controllers
{
    public class RequestController : Controller
    {
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();
            if (user == null)
                return RedirectToAction("Login", "Account");

            var ctx = new MVCDbContext();

            var profileActive = ctx.Profile.FirstOrDefault(p => p.ProfileId == user);
            if (!profileActive.IsActive)
            {
                return RedirectToAction("EditProfileData", "Profile");
            }

            var viewmodel = new ProfileIndexViewModel();
            // Hämtar de förfrågningar som hör till användaren.
            viewmodel.Requests = ctx.Request.Where(p => p.GetterId == user).ToList();
            var profiles = new List<Profile>();

            //Hämtar ut de profiler som skrivit meddelandena.
            foreach(var r in viewmodel.Requests)
            {
                if (r.GetterId.Equals(user))
                {
                    var profile = ctx.Profile.FirstOrDefault(p => p.ProfileId == r.AskerId);
                    profiles.Add(profile);
                }
            }

            // Tar bort de profiler som är avaktiverade.
            profiles.RemoveAll(x => x.IsActive == false);
            viewmodel.Profiles = profiles;

            return View(viewmodel);

        }

        public ActionResult Accept(string profileId)
        {
            var ctx = new MVCDbContext();
            var req = ctx.Request.FirstOrDefault(p => p.AskerId == profileId);

            var relation = new Relation();
            relation.UserId = User.Identity.GetUserId();
            relation.FriendId = profileId;

            // Tar bort förfrågan från request-tabellen och lägger in i relation-tabellen istället.
            ctx.Request.Remove(req);
            ctx.Relation.Add(relation);
            ctx.SaveChanges();

            return RedirectToAction("Index", "Request");
        }

        public ActionResult Dismiss(string profileId)
        {
            var ctx = new MVCDbContext();
            var userId = User.Identity.GetUserId();
            var profil = new Profile();
            var request = ctx.Request.FirstOrDefault(r => r.AskerId == profileId && r.GetterId == userId);
            ctx.Request.Remove(request);
            ctx.SaveChanges();

            return RedirectToAction("Index", "Request");
        }

        //Räknar hur många förfrågningar den inloggade användaren har och returnerar antalet till headerns notisfunktion.
        public int CountRequest()
        {
            var count = 0;
            var ctx = new MVCDbContext();
            var userID = User.Identity.GetUserId();
            var request = ctx.Request.Where(r => r.GetterId == userID).ToList();
            var profiles = ctx.Profile.ToList();
            count = request.Count();

            foreach (var r in request)
            {
                // Tar bort från antalet om det finns profiler som är avaktiverade som skickat förfrågan till användaren.
                if(r.Asker.IsActive == false)
                {
                    count--;
                }
            }

            return count;
        }
    }
}