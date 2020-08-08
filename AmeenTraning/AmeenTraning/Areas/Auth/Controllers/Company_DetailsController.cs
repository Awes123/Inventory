using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AmeenTraning.Models;

namespace AmeenTraning.Areas.Auth.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class Company_DetailsController : Controller
    {
        private TrainingEntities db = new TrainingEntities();

        // GET: Auth/Company_Details
        public ActionResult Index()
        {
            return View(db.Company_Details.ToList());
        }

        // GET: Auth/Company_Details/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/Company_Details/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company_Details company_Details)
        {
            if (ModelState.IsValid)
            {
                Company_Details com = company_Details;
                com.Active_Status = "Active";
                com.Date_Created = DateTime.Now;
                db.Company_Details.Add(com);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(company_Details);
        }

        // GET: Auth/Company_Details/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company_Details company_Details = db.Company_Details.Find(id);
            if (company_Details == null)
            {
                return HttpNotFound();
            }
            return View(company_Details);
        }

        // POST: Auth/Company_Details/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company_Details company_Details)
        {
            if (ModelState.IsValid)
            {
                Company_Details com = db.Company_Details.Find(company_Details.Company_Id);
                com.Date_Modified = DateTime.Now;
                com.Address = company_Details.Address;
                com.City = company_Details.City;
                com.State = company_Details.State;
                com.Date_Of_Start = company_Details.Date_Of_Start;
                com.Date_Of_End = company_Details.Date_Of_End;
                com.Email = company_Details.Email;
                com.Mobile = company_Details.Mobile;
                com.Name = company_Details.Name;
                com.PrimaryContactPerson = company_Details.PrimaryContactPerson;
                db.Entry(com).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company_Details);
        }

        // GET: Auth/Company_Details/Delete/5
     
        // POST: Auth/Company_Details/Delete/5
        public ActionResult Delete(int id)
        {
            Company_Details company_Details = db.Company_Details.Find(id);
            db.Company_Details.Remove(company_Details);
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
