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
    public class Units_DetailsController : Controller
    {
        private TrainingEntities db = new TrainingEntities();

        // GET: Inventory/Units_Details
        public ActionResult Index()
        {
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
            return View();
        }

        // POST: Inventory/Units_Details/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Unit_Id,Company_Id,Name,Date_Created,Date_Modified,Created_By,Modified_By")] Units_Details units_Details)
        {
            if (ModelState.IsValid)
            {
                db.Units_Details.Add(units_Details);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
        public ActionResult Edit([Bind(Include = "Unit_Id,Company_Id,Name,Date_Created,Date_Modified,Created_By,Modified_By")] Units_Details units_Details)
        {
            if (ModelState.IsValid)
            {
                db.Entry(units_Details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(units_Details);
        }

        // GET: Inventory/Units_Details/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Inventory/Units_Details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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
