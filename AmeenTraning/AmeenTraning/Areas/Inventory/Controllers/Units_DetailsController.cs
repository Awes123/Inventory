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
            string Company_Id = string.Empty;
          
            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            if (myCookie == null)
            {
                return Redirect("/Auth/Login/Logout");
            }
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                Company_Id = myCookie.Values["Company_Id"].ToString();
            }
           

            int company_Id = Convert.ToInt32(Company_Id);
            ViewBag.Units = db.Units_Details.Where(e => e.Company_Id == company_Id).ToList();
            return View();
        }

        public PartialViewResult IndexPartial()
        {
            string Company_Id = string.Empty;
            string UserId = string.Empty;
            string UserName = string.Empty;

            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            
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

            int company_Id = Convert.ToInt32(Company_Id);
            return PartialView(db.Units_Details.ToList());
        }
        // GET: Inventory/Units_Details/Details/5
        public PartialViewResult CreatePartial(int ? id)
        {
            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            string Company_Id = string.Empty;
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                Company_Id = myCookie.Values["Company_Id"].ToString();
            }
            Units_Details car = new Units_Details();
            if (id != null)
            {
                car = db.Units_Details.Where(c => c.Unit_Id == id).FirstOrDefault();
            }
            return PartialView(car);
        }
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
    //        ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name", units_Details.Company_Id);
            return View(units_Details);
        }

        public string Add(string name, int Id)
        {
            string Returns = "";
            if (ModelState.IsValid)
            {
                string Company_Id = string.Empty;
                string UserId = string.Empty;
                string UserName = string.Empty;

                HttpCookie myCookie = Request.Cookies["inventoryCookie"];
                if (myCookie == null)
                {
                    return "Logout";
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
                int company_Id = Convert.ToInt32(Company_Id);
             
               
                if (Id != 0)
                {
                    Units_Details units = db.Units_Details.Where(e => e.Unit_Id == Id).FirstOrDefault();
                    units.Company_Id = company_Id;
                    units.Created_By = UserName;
                    units.Date_Created = DateTime.Now;
                    units.Name = name;
                    db.Entry(units).State = EntityState.Modified;
                    db.SaveChanges();
                    return "Unit Updated Successfully";
                }
               else
                {
                    Units_Details units = db.Units_Details.Where(e => e.Name == name && e.Company_Id == company_Id).FirstOrDefault();
                    if (units == null)
                    {
                        Units_Details units_Details = new Units_Details();
                        units_Details.Company_Id = company_Id;
                        units_Details.Created_By = UserName;
                        units_Details.Date_Created = DateTime.Now;
                        units_Details.Date_Modified = DateTime.Now;
                        units_Details.Name = name;
                        db.Units_Details.Add(units_Details);
                        db.SaveChanges();
                        return "Unit Added Successfully";
                    }
                    else
                    {
                        return "Unit Already Exist";
                    }

                }
                
            }
            return Returns;
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
