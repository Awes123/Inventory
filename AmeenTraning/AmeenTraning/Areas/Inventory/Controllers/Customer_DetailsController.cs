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
            var customer_Details = db.Customer_Details.Include(c => c.Company_Details);
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
            return PartialView(db.Customer_Details.ToList());
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
        // Add function 
        public string Add(string name, string email, string mobile, string address)
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
   Customer_Details customer = db.Customer_Details.Where(e => e.Company_Id == company_Id && e.Name == name).FirstOrDefault();

                if (customer == null)
                {
                    Customer_Details customer_Details = new Customer_Details();
                    customer_Details.Company_Id = company_Id;
                    customer_Details.Created_By = UserName;
                    customer_Details.Date_Created = DateTime.Now;
                    customer_Details.Name = name;
                    customer_Details.Email = email;
                    customer_Details.Mobile = mobile;
                    customer_Details.Address = address;
                    db.Customer_Details.Add(customer_Details);
                    db.SaveChanges();
                    return "Product Added Successfully";
                }
                else
                {
                    return "Product Already Exist";
                }
            }
            return Returns;
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
