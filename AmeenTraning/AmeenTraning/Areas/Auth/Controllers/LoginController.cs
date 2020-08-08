using AmeenTraning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AmeenTraning.Areas.Auth.Controllers
{
    public class LoginController : Controller
    {
        TrainingEntities db = new TrainingEntities();
        // GET: Auth/Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(UserDetail User, string Password)
        {
            UserDetail users = db.UserDetails.Where(e => e.Email == User.Email && e.Password == User.Password).FirstOrDefault();
            if (users != null)
            {
                HttpCookie myCookie = new HttpCookie("inventoryCookie");

                //Add key-values in the cookie
                myCookie.Values.Add("Company_Id", users.Company_Id.ToString());
                myCookie.Values.Add("UserId", users.UserId.ToString());

                //set cookie expiry date-time. Made it to last for next 12 hours.
                myCookie.Expires = DateTime.Now.AddHours(1200);

                //Most important, write the cookie to client.
                Response.Cookies.Add(myCookie);
                FormsAuthentication.SetAuthCookie(User.Email, false);

                if (users.Role == "Super Admin")
                {
                    return RedirectToAction("Index", "Default", new { area = "Auth" });
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Inventory" });
                }
            }
            else
            {
                return View(User);
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Auth/Login/Index");
        }

    }
}