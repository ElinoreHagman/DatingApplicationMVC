using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Dating_Project.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MVC_Dating_Project.Controllers
{
    public class ProfileController : Controller
    {
        public ActionResult AddAndEditProfileData()
        {
            return View();
        }

        public ActionResult EditProfileData()
        {
            var ctx = new MVCDbContext();
            var userid = User.Identity.GetUserId();
            var profile = new Profile();
            profile = ctx.Profile.FirstOrDefault(u => u.ProfileId == userid);

            return View(profile);
        }

        //Metod för att ändra sin nuvarande profildata.
        [HttpPost]
        public ActionResult EditProfileData(Profile model)
        {
            var ctx = new MVCDbContext();
            var userID = User.Identity.GetUserId();
            Profile pro = ctx.Profile.FirstOrDefault(u => u.ProfileId == userID);

            pro.FirstName = model.FirstName;
            pro.LastName = model.LastName;
            pro.Gender = model.Gender;
            pro.Age = model.Age;
            pro.Sexuality = model.Sexuality;
            ctx.SaveChanges();

            return RedirectToAction("ListProfileData");
        }

        //Metod för att lägga till profilinformation + bild. 
        //Denna metod används endast direkt efter att man registrerat ett nytt konto.
        [HttpPost]
        public ActionResult AddAndEditProfileData(Profile model, HttpPostedFileBase ImagePath)
        {
            var ctx = new MVCDbContext();
            var p = new Profile
            {
                ProfileId = User.Identity.GetUserId(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age,
                Gender = model.Gender,
                Sexuality = model.Sexuality,
                //Alla nya medlemmar får värdet true i databasen för att visa att de har aktiverade konton.
                IsActive = true
            };

            try
            {
                var checkextension = Path.GetExtension(ImagePath.FileName).ToLower();
                if (checkextension.ToLower().Contains(".jpg") || checkextension.ToLower().Contains(".jpeg") || checkextension.Contains(".png"))
                {
                    // path skapar sökväg för att lägga in bilden i projektmappen Images
                    string path = System.IO.Path.Combine(Server.MapPath("~/Images"), System.IO.Path.GetFileName(ImagePath.FileName));
                    // relativePath skapar den relativa sökvägen som läggs in i databasen
                    string relativePath = System.IO.Path.Combine("~/Images/" + ImagePath.FileName);

                    var userId = User.Identity.GetUserId();
                    p.ImagePath = relativePath;
                    ctx.Profile.Add(p);
                    ImagePath.SaveAs(path);
                    ctx.SaveChanges();
                    ViewBag.FileStatus = "Photo uploaded successfully.";
                }

                else
                {
                    ViewBag.FileStatus = "Only .JPEG and .PNG allowed";
                }

            }
            catch (Exception)
            {
                ViewBag.FileStatus = "Error while photo uploading.";
            }

            ViewBag.EditStatus = "Successful registration";
            return RedirectToAction("ListProfileData");
        }

        //Metod som hämtar profilinfo när en användare är inloggad samt om profilen är aktiverad.
        public ActionResult ListProfileData()
        {
            var user = User.Identity.GetUserId();

            // Ser till att man inte kommer åt sidan om man är utloggad.
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var ctx = new MVCDbContext();
            
            var profileActive = ctx.Profile.FirstOrDefault(p => p.ProfileId == user);
            //Kollar så att profilen inte är avaktiverad.
            if (!profileActive.IsActive)
            {
                return RedirectToAction("EditProfileData", "Profile");
            }

            // Lägger till de meddelanden vars mottagare är den inloggade användaren.
            var viewModel = new ImageIndexViewModel
            {
                Profiles = ctx.Profile.ToList(),
                Messages = ctx.Message.Where(m => m.RecieverId == user).ToList()
            };

            return View(viewModel);
        }

        //Metod som sköter uppladdning av profilbild.
        [HttpPost]
        public ActionResult UploadPhoto(HttpPostedFileBase ImagePath)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ImagePath != null)
                    {   
                        //Validering som ser till att filformatet måste vara JPEG, JPG eller PNG.
                        var checkextension = Path.GetExtension(ImagePath.FileName).ToLower();

                        if (checkextension.ToLower().Contains(".jpg") || checkextension.ToLower().Contains(".jpeg") || checkextension.Contains(".png"))
                        {
                            //Ser till att sökvägen för bilden sparas likadant 
                            string path = System.IO.Path.Combine(Server.MapPath("~/Images"), System.IO.Path.GetFileName(ImagePath.FileName));
                            string relativePath = System.IO.Path.Combine("~/Images/" + ImagePath.FileName);

                            var ctx = new MVCDbContext();
                            var userId = User.Identity.GetUserId();
                            Profile profile = ctx.Profile.FirstOrDefault(p => p.ProfileId == userId);

                            if (profile != null)
                            {
                                profile.ImagePath = relativePath;
                                ImagePath.SaveAs(path);
                                ctx.SaveChanges();
                                ViewBag.FileStatus = "File uploaded successfully.";
                            }
                            else
                            {
                                ViewBag.FileStatus = "Write in your information first.";
                            }
                        }
                        else
                        {
                            return RedirectToAction("EditProfileData");
                        }
                    }
                }
                catch (Exception)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                }
            }
            return RedirectToAction("ListProfileData");
        }

        //Metod som hämtar profildata för den inloggade profilen samt sparar ner denna info i en XML-fil.
        public ActionResult DownloadProfile()
        {

            var ctx = new MVCDbContext();
            var userid = User.Identity.GetUserId();
            var profile = ctx.Profile.FirstOrDefault(p => p.ProfileId == userid);

            //Skapar namnet på filen och sökvägen där filen ska skapas
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/SavedProfiles/"+userid+".txt");

            var serializer = new XmlSerializer(typeof(Profile));
            var writer = new StreamWriter(path);

            serializer.Serialize(writer, profile);
            writer.Close();

            return RedirectToAction("EditProfileData");
        }
    }
}







