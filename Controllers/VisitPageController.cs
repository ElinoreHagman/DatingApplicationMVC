using Microsoft.AspNet.Identity;
using MVC_Dating_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Dating_Project.Controllers
{
    public class VisitPageController : Controller
    {
        public ActionResult Index(string profileId)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Sparar ner profilid i en variabel som kan nås i SendRequest-metoden när man skickar vänförfrågan.
            Session["usid"] = profileId;
            var ctx = new MVCDbContext();

            var profileActive = ctx.Profile.FirstOrDefault(p => p.ProfileId == userId);
            // Om ens konto är inaktiverat kommer man redirectas till ens redigeringssida.
            if (!profileActive.IsActive)
            {
                return RedirectToAction("EditProfileData", "Profile");
            }

            var profileList = ctx.Profile.ToList();
            var chosenProfile = new ChosenProfileView();
            var profileExist = ctx.Profile.FirstOrDefault(p => p.ProfileId == profileId);

            // Tre olika variabler som kollar ifall man redan har skickat förfrågan, har fått en förfrågan, eller är vänner redan.
            var alreadyFriend = ctx.Relation.FirstOrDefault
                (p => (p.FriendId == profileId || p.UserId == profileId) && (p.FriendId == userId || p.UserId == userId));

            var pendingRequest = ctx.Request.FirstOrDefault
               (p => (p.GetterId == userId) && (p.AskerId == profileId));

            var alreadySent = ctx.Request.FirstOrDefault
               (p => (p.AskerId == userId) && (p.GetterId == profileId));

            chosenProfile.Relation = alreadyFriend;
            chosenProfile.Requests = pendingRequest;
            chosenProfile.SentRequest = alreadySent;

            if (profileExist != null)
            {
                chosenProfile.Messages = ctx.Message.ToList();
                chosenProfile.Profile = profileExist;
                chosenProfile.personalMessages = ctx.Message.Where(m => m.RecieverId == profileId).OrderByDescending(m => m.Date).ToList();
            }

            return View(chosenProfile);
        }

        public ActionResult SendRequest()
        {
            // Tar värdet som tidigare lagrats i usid-variabeln från metoden som laddade in sidan.
            var profilevis = (Session["usid"]).ToString();
            var userId = User.Identity.GetUserId();
            var ctx = new MVCDbContext();
            // Kollar om det redan finns en förfrågan.
            var RequestPending = ctx.Request.FirstOrDefault(r => r.GetterId == profilevis && r.AskerId == userId);

            if (RequestPending == null)
            {
                var request = new FriendRequest
                {
                    AskerId = userId,
                    GetterId = profilevis
                };

                ctx.Request.Add(request);
                ctx.SaveChanges();
            }

            return RedirectToAction("Index", "VisitPage", new { profileId = profilevis });
        }
    }
}