using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVC_Dating_Project.Models;

namespace MVC_Dating_Project.Controllers
{
    public class MatchMakerController : Controller
    {
        // GET: MatchMaker
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var ctx = new MVCDbContext();
            var user = ctx.Profile.FirstOrDefault(p => p.ProfileId == userid);

            //Hämtar alla profiler
            var profilelist = ctx.Profile.Where(p => p.ProfileId != userid).ToList();

            //Lista som kommer lagra de relevanta profilerna
            var compatibleList = new List<Profile>();

            // variabler som håller koll på om profilerna matchar med varandra
            var userGender = user.Gender;
            bool sameGender = false;
            bool isCompatible = false;

            foreach (var p in profilelist)
            {
                if (userGender == p.Gender)
                {
                    if (user.Gender != "Non-Binary")
                    {
                        sameGender = true;
                    }
                }

                //Båda heterosexuella och olika kön
                if ((user.Sexuality == "Heterosexual" && p.Sexuality == "Heterosexual") && sameGender == false)
                {
                    isCompatible = true;
                }

                //Båda homosexuella och samma kön
                if ((user.Sexuality == "Homosexual" && p.Sexuality == "Homosexual") && sameGender == true)
                {
                    isCompatible = true;
                }

                //Båda personerna är Non-Binary (fick bli så)
                if (user.Gender == "Non-Binary" && p.Gender == "Non-Binary")
                {
                    isCompatible = true;
                }

                //Båda personerna är bisexuella (fick bli så)
                if ((user.Sexuality == "Bisexual" && p.Sexuality == "Bisexual") && (sameGender == true || sameGender == false))
                {
                    isCompatible = true;
                }

                if(isCompatible)
                {
                    compatibleList.Add(p);
                }
                sameGender = false;
                isCompatible = false;
            }

            compatibleList.RemoveAll(p => p.IsActive == false);

            var viewmodel = new ProfileIndexViewModel
            {
                Profiles = compatibleList
            };

            return View(viewmodel);
        }
    }
}