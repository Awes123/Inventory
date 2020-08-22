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
            var supplier_Details = db.Supplier_Details.Include(s => s.Company_Details);
            return View();
        }

        // Partial View
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
            //var products_Details = db.Products_Details.Include(p => p.Category_Details).Include(p => p.Company_Details);
            return PartialView(db.Supplier_Details.ToList());
        }
        // GET: Inventory/Supplier_Details/Details/5

        public PartialViewResult CreatePartial(int? id)
        {
            string Company_Id = string.Empty;

            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                Company_Id = myCookie.Values["Company_Id"].ToString();
            }
            Supplier_Details car = new Supplier_Details();
            if (id != null)
            {
                car = db.Supplier_Details.Where(c => c.Supplier_Id == id).FirstOrDefault();
            }
            return PartialView(car);
        }
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

        // Add function 
        public string Add(string name, string email, string mobile, string address,int Id)
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
                    Supplier_Details supplier = db.Supplier_Details.Where(e => e.Supplier_Id == Id).FirstOrDefault();
                    supplier.Company_Id = company_Id;
                    supplier.Created_By = UserName;
                    supplier.Date_Created = DateTime.Now;
                    supplier.Name = name;
                    supplier.Email = email;
                    supplier.Date_Modified = DateTime.Now;
                    supplier.Mobile = mobile;
                    supplier.Address = address;
                    db.Entry(supplier).State = EntityState.Modified;
                    db.SaveChanges();
                    return "Supplier Updated Successfully";
                }
                else
                {
                    Supplier_Details supplier = db.Supplier_Details.Where(e => e.Name == name && e.Company_Id == company_Id).FirstOrDefault();
                    if (supplier == null)
                    {
                        Supplier_Details supplier_Details = new Supplier_Details();
                        supplier_Details.Company_Id = company_Id;
                        supplier_Details.Created_By = UserName;
                        supplier_Details.Date_Created = DateTime.Now;
                        supplier_Details.Name = name;
                        supplier_Details.Email = email;
                        supplier_Details.Mobile = mobile;
                        supplier_Details.Address = address;
                        db.Supplier_Details.Add(supplier_Details);
                        db.SaveChanges();
                        return "Supplier Added Successfully";
                    }
                    else
                    {
                        return "Supplier Already Exist";
                    }

                }

               
            }
            return Returns;
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
