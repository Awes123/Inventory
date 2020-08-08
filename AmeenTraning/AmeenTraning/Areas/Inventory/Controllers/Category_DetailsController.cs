using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AmeenTraning.Models;

namespace AmeenTraning.Areas.Inventory.Controllers
{
    public class Category_DetailsController : Controller
    {
        private TrainingEntities db = new TrainingEntities();

        

        [Authorize]
        // GET: Inventory/Category_Details
        public ActionResult Index()
        {
            string Company_Id = string.Empty;
            string UserId = string.Empty;
            string UserName = string.Empty;

            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            if (myCookie == null)
            {
                return Redirect("/Auth/Login/Logout");
            }
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                  Company_Id = myCookie.Values["Company_Id"].ToString();
            }
            if (!string.IsNullOrEmpty(myCookie.Values["UserId"]))
            {
                 UserId = myCookie.Values["UserId"].ToString();
            }
            if (!string.IsNullOrEmpty(myCookie.Values["UserName"]))
            {
                 UserName = myCookie.Values["UserName"].ToString();
            }
            var category_Details = db.Category_Details.Include(c => c.Company_Details);
            return View(category_Details.ToList());
        }
        [Authorize]
        // GET: Inventory/Category_Details/Create
        public ActionResult Create()
        {
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name");
            return View();
        }

        // POST: Inventory/Category_Details/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Category_Details category_Details)
        {
            if (ModelState.IsValid)
            {
                string Company_Id = string.Empty;
                string UserId = string.Empty;
                string UserName = string.Empty;

                HttpCookie myCookie = Request.Cookies["inventoryCookie"];
                if (myCookie == null)
                {
                    return Redirect("/Auth/Login/Logout");
                }
                if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
                {
                     Company_Id = myCookie.Values["Company_Id"].ToString();
                }
                if (!string.IsNullOrEmpty(myCookie.Values["UserId"]))
                {
                     UserId = myCookie.Values["UserId"].ToString();
                }
                if (!string.IsNullOrEmpty(myCookie.Values["UserName"]))
                {
                     UserName = myCookie.Values["UserName"].ToString();
                }

                Category_Details cart = new Category_Details();
                cart.Added_By = UserName;
                cart.Date_Created = DateTime.Now;
                cart.Company_Id = Convert.ToInt32(Company_Id);
                cart.Name = category_Details.Name;
                db.Category_Details.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name", category_Details.Company_Id);
            return View(category_Details);
        }

        // GET: Inventory/Category_Details/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category_Details category_Details = db.Category_Details.Find(id);
            if (category_Details == null)
            {
                return HttpNotFound();
            }
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name", category_Details.Company_Id);
            return View(category_Details);
        }

        // POST: Inventory/Category_Details/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Category_Details category_Details)
        {
            if (ModelState.IsValid)
            {
                string Company_Id = string.Empty;
                string UserId = string.Empty;
                string UserName = string.Empty;

                HttpCookie myCookie = Request.Cookies["inventoryCookie"];
                if (myCookie == null)
                {
                    return Redirect("/Auth/Login/Logout");
                }
                if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
                {
                    Company_Id = myCookie.Values["Company_Id"].ToString();
                }
                if (!string.IsNullOrEmpty(myCookie.Values["UserId"]))
                {
                    UserId = myCookie.Values["UserId"].ToString();
                }
                if (!string.IsNullOrEmpty(myCookie.Values["UserName"]))
                {
                    UserName = myCookie.Values["UserName"].ToString();
                }

                Category_Details cart = db.Category_Details.Find(category_Details.Category_Id);
                cart.Modified_By = UserName;
                cart.Date_Modified = DateTime.Now;
                cart.Name = category_Details.Name;
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name", category_Details.Company_Id);
            return View(category_Details);
        }
        // POST: Inventory/Category_Details/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Category_Details category_Details = db.Category_Details.Find(id);
            db.Category_Details.Remove(category_Details);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
