using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVC_Dating_Project.Models;

namespace MVC_Dating_Project.Controllers
{
    public class FriendListController : Controller
    {
        // GET: FriendList
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var ctx = new MVCDbContext();

            var profileActive = ctx.Profile.FirstOrDefault(p => p.ProfileId == userId);
            if (!profileActive.IsActive)
            {
                return RedirectToAction("EditProfileData", "Profile");
            }

            var viewmodel = new FriendIndexViewModel();
            //Fyller listan med de rader där användaren är den som kategoriserar.
            viewmodel.Categories = ctx.Category.Where(p => p.CategorizerId == userId).ToList();
            viewmodel.Relations = ctx.Relation.ToList();
            var friendList = new List<Profile>();

            foreach (var r in viewmodel.Relations)
            {
                if (r.UserId.Equals(userId) || r.FriendId.Equals(userId))
                {
                    //Lägger till de profiler som personen har en relation med.
                    var profile = ctx.Profile.FirstOrDefault(
                        p => (p.ProfileId == r.FriendId || p.ProfileId == r.UserId) && (p.ProfileId != userId));
                    friendList.Add(profile);
                }
            }

            friendList.RemoveAll(x => x.IsActive == false);

            viewmodel.Profiles = friendList;

            return View(viewmodel);
        }

        public ActionResult RemoveFriend(string profileId)
        {
            var ctx = new MVCDbContext();
            var userId = User.Identity.GetUserId();
            //Hämtar den specifika relationen så den kan raderas.
            var person = ctx.Relation.FirstOrDefault(p => (p.UserId == profileId || p.FriendId == profileId) && (p.UserId == userId || p.FriendId == userId));

            // Tar bort kategorin från databasen om användaren har gett en till den andra personen.
            var category = ctx.Category.FirstOrDefault(p => p.CategorizerId == userId && p.TheCategorizedId == profileId);
            if(category != null)
            {
                ctx.Category.Remove(category);
            }

            ctx.Relation.Remove(person);
            ctx.SaveChanges();

            return RedirectToAction("Index", "FriendList");
        }

        public ActionResult CategoryPage()
        {
            var userid = User.Identity.GetUserId();

            if (userid == null)
                return RedirectToAction("Login", "Account");

            var viewmodel = new FriendIndexViewModel();
            var ctx = new MVCDbContext();
            var profiles = new List<Profile>();
            //Kollar om variabeln existerar ännu, och isåfall ta ut värdet därifrån.
            if (Session["categorySaved"] != null) {
               var chosencategory = (Session["categorySaved"]).ToString();
                viewmodel.Category = chosencategory;
                //Hämtar ut de kategori-rader som har den specifika kategorin
                var categories = ctx.Category.Where(c => c.CategoryName == chosencategory).ToList();
                foreach (var c in categories)
                {
                    //Plockar ut profilerna som finns i category-tabellen och den som kategoriserat dem är användaren.
                    if (c.CategorizerId.Equals(userid))
                    {
                        var profile = ctx.Profile.FirstOrDefault(p => p.ProfileId == c.TheCategorizedId);
                        profiles.Add(profile);
                    }
                }
            }
            profiles.RemoveAll(x => x.IsActive == false);
            viewmodel.Profiles = profiles;

            return View(viewmodel);
        }

        public ActionResult ReturnItems(FriendIndexViewModel model)
        {
            //Lagrar kategorin i en variabel som kan användas i metoden ovanför.
            Session["categorySaved"] = model.Category;

            return RedirectToAction("CategoryPage", "FriendList");
        }
    }
}