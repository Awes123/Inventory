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
    [Authorize]
    public class Units_DetailsController : Controller
    {
        private TrainingEntities db = new TrainingEntities();

        // GET: Inventory/Units_Details
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
            return View(db.Units_Details.ToList());
        }

        // GET: Inventory/Units_Details/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Units_Details units_Details = db.Units_Details.Find(id);
            if (units_Details == null)
            {
                return HttpNotFound();
            }
            return View(units_Details);
        }


        // GET: Inventory/Units_Details/Create
        public ActionResult Create()
        {
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name");
            return View();
        }

        // POST: Inventory/Units_Details/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Units_Details units_Details)
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

                Units_Details cart = new Units_Details();
                cart.Created_By = UserName;
                cart.Date_Created = DateTime.Now;
                cart.Company_Id = Convert.ToInt32(Company_Id);
                cart.Name = units_Details.Name;
                db.Units_Details.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");

                //db.units_details.add(units_details);
                //db.savechanges();
                //return redirecttoaction("index");
            }
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name", units_Details.Company_Id);
            return View(units_Details);
        }

        // GET: Inventory/Units_Details/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Units_Details units_Details = db.Units_Details.Find(id);
            if (units_Details == null)
            {
                return HttpNotFound();
            }
            return View(units_Details);
        }

        // POST: Inventory/Units_Details/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Units_Details units_Details)
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

                Units_Details cart = db.Units_Details.Find(units_Details.Unit_Id);
                cart.Modified_By = UserName;
                cart.Date_Modified = DateTime.Now;
                cart.Name = units_Details.Name;
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(units_Details);
        }

        // GET: Inventory/Units_Details/Delete/5
        
        public ActionResult Delete(int id)
        {
            Units_Details units_Details = db.Units_Details.Find(id);
            db.Units_Details.Remove(units_Details);
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
