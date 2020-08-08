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
    public class Customer_DetailsController : Controller
    {
        private TrainingEntities db = new TrainingEntities();
        [Authorize]
        // GET: Inventory/Customer_Details
        public ActionResult Index()
        {
            var customer_Details = db.Customer_Details.Include(c => c.Company_Details);
            return View(customer_Details.ToList());
        }

        // GET: Inventory/Customer_Details/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_Details customer_Details = db.Customer_Details.Find(id);
            if (customer_Details == null)
            {
                return HttpNotFound();
            }
            return View(customer_Details);
        }

        // GET: Inventory/Customer_Details/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name");
            return View();
        }

        // POST: Inventory/Customer_Details/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Customer_Details customer_Details)
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
                Customer_Details cust = customer_Details;
                cust.Company_Id =Convert.ToInt32(Company_Id);
                cust.Created_By = UserName;
                cust.Date_Created = DateTime.Now;
                db.Customer_Details.Add(cust);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name", customer_Details.Company_Id);
            return View(customer_Details);
        }

        // GET: Inventory/Customer_Details/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_Details customer_Details = db.Customer_Details.Find(id);
            if (customer_Details == null)
            {
                return HttpNotFound();
            }
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name", customer_Details.Company_Id);
            return View(customer_Details);
        }

        // POST: Inventory/Customer_Details/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Customer_Details customer_Details)
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
                Customer_Details cust = db.Customer_Details.Find(customer_Details.Customer_Id);
                cust.Address = customer_Details.Address;
                cust.City = customer_Details.City;
                cust.Country = customer_Details.Country;
                cust.Date_Modified = DateTime.Now;
                cust.Email = customer_Details.Email;
                cust.Mobile = customer_Details.Mobile;
                cust.Mobile1 = customer_Details.Mobile1;
                cust.Modified_By = UserName;
                cust.Name = customer_Details.Name;
                cust.State = customer_Details.State;
                db.Entry(cust).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name", customer_Details.Company_Id);
            return View(customer_Details);
        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            Customer_Details customer_Details = db.Customer_Details.Find(id);
            db.Customer_Details.Remove(customer_Details);
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
