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
        public ActionResult Create([Bind(Include = "Customer_Id,Company_Id,Name,Email,Mobile,Mobile1,Address,City,State,Country,Created_By,Modified_By,Date_Created,Date_Modified")] Customer_Details customer_Details)
        {
            if (ModelState.IsValid)
            {
                db.Customer_Details.Add(customer_Details);
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
        public ActionResult Edit([Bind(Include = "Customer_Id,Company_Id,Name,Email,Mobile,Mobile1,Address,City,State,Country,Created_By,Modified_By,Date_Created,Date_Modified")] Customer_Details customer_Details)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer_Details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name", customer_Details.Company_Id);
            return View(customer_Details);
        }

        // GET: Inventory/Customer_Details/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
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

        // POST: Inventory/Customer_Details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
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
