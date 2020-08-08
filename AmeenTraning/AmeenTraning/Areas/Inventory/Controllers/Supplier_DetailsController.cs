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
    public class Supplier_DetailsController : Controller
    {
        private TrainingEntities db = new TrainingEntities();

        // GET: Inventory/Supplier_Details
        public ActionResult Index()
        {
            var supplier_Details = db.Supplier_Details.Include(s => s.Company_Details);
            return View(supplier_Details.ToList());
        }

        // GET: Inventory/Supplier_Details/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier_Details supplier_Details = db.Supplier_Details.Find(id);
            if (supplier_Details == null)
            {
                return HttpNotFound();
            }
            return View(supplier_Details);
        }

        // GET: Inventory/Supplier_Details/Create
        public ActionResult Create()
        {
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name");
            return View();
        }

        // POST: Inventory/Supplier_Details/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Supplier_Id,Company_Id,Name,Email,Mobile,Mobile1,Address,City,State,Country,Created_By,Modified_By,Date_Created,Date_Modified")] Supplier_Details supplier_Details)
        {
            if (ModelState.IsValid)
            {
                db.Supplier_Details.Add(supplier_Details);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name", supplier_Details.Company_Id);
            return View(supplier_Details);
        }

        // GET: Inventory/Supplier_Details/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier_Details supplier_Details = db.Supplier_Details.Find(id);
            if (supplier_Details == null)
            {
                return HttpNotFound();
            }
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name", supplier_Details.Company_Id);
            return View(supplier_Details);
        }

        // POST: Inventory/Supplier_Details/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Supplier_Id,Company_Id,Name,Email,Mobile,Mobile1,Address,City,State,Country,Created_By,Modified_By,Date_Created,Date_Modified")] Supplier_Details supplier_Details)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier_Details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name", supplier_Details.Company_Id);
            return View(supplier_Details);
        }

        // GET: Inventory/Supplier_Details/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier_Details supplier_Details = db.Supplier_Details.Find(id);
            if (supplier_Details == null)
            {
                return HttpNotFound();
            }
            return View(supplier_Details);
        }

        // POST: Inventory/Supplier_Details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier_Details supplier_Details = db.Supplier_Details.Find(id);
            db.Supplier_Details.Remove(supplier_Details);
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
