using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Routing;
using MVC_Dating_Project.Models;
using Microsoft.AspNet.Identity;

namespace MVC_Dating_Project.Controllers
{

    [RoutePrefix("controller")]
    public class MessageAPIController : ApiController
    {

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
        //Metod som sparar meddelandet i databasen. Data skickas från javascript.
        [Route("addMessage")]
        [HttpPost]
        public void AddMessage(string textObj, string recieverObj)
        {
            var ctx = new MVCDbContext();
            var message = new Message
            {
                AuthorId = User.Identity.GetUserId(),
                RecieverId = recieverObj,
                Date = DateTime.Now,
                MessageText = textObj
            };
            ctx.Message.Add(message);
            ctx.SaveChanges();
        }

        [Route("deletemessage")]
        [HttpPost]
        public void DeleteMessage(string messageid)
        {
            var ctx = new MVCDbContext();
            int messageInt = Int32.Parse(messageid);
            var message = ctx.Message.FirstOrDefault(u => u.MessageId == messageInt);
            ctx.Message.Remove(message);
            ctx.SaveChanges();
        }

        [Route("removeaccount")]
        [HttpPost]
        public void RemoveAccount(string userid)
        {
            var ctx = new MVCDbContext();
            var profile = ctx.Profile.FirstOrDefault(u => u.ProfileId == userid);
            profile.IsActive = false;
            ctx.SaveChanges();
        }

        [Route("activateaccount")]
        [HttpPost]
        public void ActivateAccount(string userid)
        {
            var ctx = new MVCDbContext();
            var profile = ctx.Profile.FirstOrDefault(u => u.ProfileId == userid);
            profile.IsActive = true;
            ctx.SaveChanges();
        }

        [Route("changecategory")]
        [HttpPost]
        public void ChangeCategory(string category, string id)
        {
            var ctx = new MVCDbContext();
            var user = User.Identity.GetUserId();
            var newCategory = new Category
            {
                CategorizerId = user,
                TheCategorizedId = id,
                CategoryName = category
            };

            var categoryExist = ctx.Category.FirstOrDefault(c => c.CategorizerId == user && c.TheCategorizedId == id);
            if (categoryExist != null)
            {
                ctx.Category.Remove(categoryExist);
            }
            ctx.Category.Add(newCategory);
            ctx.SaveChanges();
        }

        [Route("insertvisit")]
        [HttpPost]
        public void InsertVisit(string profileid)
        {
            var ctx = new MVCDbContext();
            var userid = User.Identity.GetUserId();
            var visitExist = ctx.Visitor.FirstOrDefault(v => v.UserId == profileid && v.VisitorId == userid);

            if (visitExist != null)
            {
                ctx.Visitor.Remove(visitExist);
            }
            var newVisit = new Visit
            {
                VisitorId = userid,
                UserId = profileid
            };

            ctx.Visitor.Add(newVisit);
            ctx.SaveChanges();
        }
        //Listar de senaste besökarna på din profil.
        [Route("getvisits")]
        [HttpGet]
        public List<Profile> GetVisits()
        {
            var userid = User.Identity.GetUserId();
            var ctx = new MVCDbContext();
            var visitlist = ctx.Visitor.ToList();
            var profilelist = new List<Profile>();

            foreach (var v in visitlist)
            {
                if (v.UserId == userid)
                {
                    var profile = ctx.Profile.FirstOrDefault(p => p.ProfileId == v.VisitorId);
                    if (profilelist.Count < 5)
                    {
                        profilelist.Add(profile);
                    }
                }
            }

            profilelist.Reverse();

            return profilelist;
        }

        [Route("checkcomp")]
        [HttpGet]
        public bool CheckCompatibility(string profileid)
        {
            var ctx = new MVCDbContext();
            var userid = User.Identity.GetUserId();
            var user = ctx.Profile.FirstOrDefault(p => p.ProfileId == userid);
            var profil1 = ctx.Profile.FirstOrDefault(p =>  p.ProfileId == profileid);

            var userGender = user.Gender;
            var profileGender = profil1.Gender;
            bool sameGender = false;
            bool isCompatible = false;
            //Validering som matchar de olika profilerna med varandra.
            if(userGender == profileGender)
            {
                if(user.Gender != "Non-Binary")
                {
                    sameGender = true;
                }
            }

            if ((user.Sexuality == "Heterosexual" && profil1.Sexuality == "Heterosexual") && sameGender == false)
            {
                isCompatible = true;
            }

            if ((user.Sexuality == "Homosexual" && profil1.Sexuality == "Homosexual") && sameGender == true)
            {
                isCompatible = true;
            }

            if (user.Gender == "Non-Binary" && profil1.Gender == "Non-Binary")
            {
                isCompatible = true;
            }

            if ((user.Sexuality == "Bisexual" && profil1.Sexuality == "Bisexual") && (sameGender == true || sameGender == false))
            {
                isCompatible = true;
            }

            return isCompatible;
        }
    }
}


