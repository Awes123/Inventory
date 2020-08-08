using AmeenTraning.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmeenTraning.Areas.Auth.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class DefaultController : Controller
    {
        TrainingEntities db = new TrainingEntities();
        // GET: Auth/Default3
        
        public ActionResult Index()
        {
            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            if (myCookie == null)
            {
                //No cookie found or cookie expired.
                //Handle the situation here, Redirect the user or simply return;
            }

            //ok - cookie is found.
            //Gracefully check if the cookie has the key-value as expected.
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                string Company_Id = myCookie.Values["Company_Id"].ToString();
            }
            if (!string.IsNullOrEmpty(myCookie.Values["UserId"]))
            {
                string UserId = myCookie.Values["UserId"].ToString();
            }
            ViewBag.users = db.UserDetails.Where(e=>e.Role!="Super Admin").ToList();
            return View();
        }
        
        public ActionResult Create()
        {
            ViewBag.Companies = db.Company_Details.ToList();
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(string Company_Id, string Name, string Email, string Mobile, string Address, string Age, string Gender, string Password)
        {
            UserDetail user = new UserDetail()
            {
                Name = Name,
                Email = Email,
                Mobile = Mobile,
                Address = Address,
                Age = Convert.ToInt32(Age),
                Gender = Gender,
                Password = Password
            };
            db.UserDetails.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index", "Default", new { area = "Auth" });
        }
        
        public ActionResult Delete(string UserId)
        {
            int userid = Convert.ToInt32(UserId);
            UserDetail user = db.UserDetails.Find(userid);
            if (user != null)
            {
                db.UserDetails.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Default", new { area = "Auth" });
        }
        public ActionResult Edit(string UserId)
        {
            ViewBag.Companies = db.Company_Details.ToList();

            int userid = Convert.ToInt32(UserId);
            UserDetail user = db.UserDetails.Find(userid);

            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(UserDetail user)
        {
            var usr = db.UserDetails.Find(user.UserId);
            usr.Address = user.Address;
            usr.Age = user.Age;
            usr.Company_Id = user.Company_Id;
            usr.Date_Modified = DateTime.Now;
            usr.Email = user.Email;
            usr.Gender = user.Gender;
            usr.Mobile = user.Mobile;
            usr.Name = user.Name;
            usr.Password = user.Password;
            usr.Role = "Admin";
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Default", new { area = "Auth" });
        }
    }
}