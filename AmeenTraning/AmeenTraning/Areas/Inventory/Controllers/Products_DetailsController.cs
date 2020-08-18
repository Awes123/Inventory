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
    public class Products_DetailsController : Controller
    {
        private TrainingEntities db = new TrainingEntities();

        // GET: Inventory/Products_Details
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
            ViewBag.Category = db.Category_Details.Where(e => e.Company_Id == company_Id).ToList();

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
            var products_Details = db.Products_Details.Include(p => p.Category_Details).Include(p => p.Company_Details);
            return PartialView(products_Details.ToList());
        }

        // GET: Inventory/Products_Details/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products_Details products_Details = db.Products_Details.Find(id);
            if (products_Details == null)
            {
                return HttpNotFound();
            }
            return View(products_Details);
        }

        // GET: Inventory/Products_Details/Create
        public ActionResult Create()
        {
            ViewBag.Category_Id = new SelectList(db.Category_Details, "Category_Id", "Name");
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name");
            return View();
        }

        // POST: Inventory/Products_Details/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Product_Id,Company_Id,Category_Id,Name,Description,Created_By,Modified_By,Date_Created,Date_Modified")] Products_Details products_Details)
        {
            if (ModelState.IsValid)
            {
                db.Products_Details.Add(products_Details);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_Id = new SelectList(db.Category_Details, "Category_Id", "Name", products_Details.Category_Id);
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name", products_Details.Company_Id);
            return View(products_Details);
        }

        public string Add(string category, string name, string Description)
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

                Products_Details prd = db.Products_Details.Where(e => e.Company_Id == company_Id && e.Name == name).FirstOrDefault();

                if (prd == null)
                { 
                   Products_Details products_Details = new Products_Details();    
                   products_Details.Company_Id = company_Id;
                   products_Details.Category_Id = Convert.ToInt32(category);
                   products_Details.Created_By = UserName;
                   products_Details.Date_Created = DateTime.Now;
                   products_Details.Description = Description;
                   products_Details.Name = name;
                   db.Products_Details.Add(products_Details);
                   db.SaveChanges();
                   return "Product Added Successfully";
                } else
                {
                   return "Product Already Exist";
                }           
            }
            return Returns;
        }

        // GET: Inventory/Products_Details/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products_Details products_Details = db.Products_Details.Find(id);
            if (products_Details == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_Id = new SelectList(db.Category_Details, "Category_Id", "Name", products_Details.Category_Id);
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name", products_Details.Company_Id);
            return View(products_Details);
        }

        // POST: Inventory/Products_Details/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Product_Id,Company_Id,Category_Id,Name,Description,Created_By,Modified_By,Date_Created,Date_Modified")] Products_Details products_Details)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products_Details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_Id = new SelectList(db.Category_Details, "Category_Id", "Name", products_Details.Category_Id);
            ViewBag.Company_Id = new SelectList(db.Company_Details, "Company_Id", "Name", products_Details.Company_Id);
            return View(products_Details);
        }

        // GET: Inventory/Products_Details/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products_Details products_Details = db.Products_Details.Find(id);
            if (products_Details == null)
            {
                return HttpNotFound();
            }
            return View(products_Details);
        }

        // POST: Inventory/Products_Details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products_Details products_Details = db.Products_Details.Find(id);
            db.Products_Details.Remove(products_Details);
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
