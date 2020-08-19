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

        public PartialViewResult CreatePartial(int ? id)
        {
            string Company_Id = string.Empty;

            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
           
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                Company_Id = myCookie.Values["Company_Id"].ToString();
            }
            int company_Id = Convert.ToInt32(Company_Id);
            ViewBag.Category = db.Category_Details.Where(e => e.Company_Id == company_Id).ToList();
            if (id != null)
            {
                var prod = db.Products_Details.Find(id);
                ViewBag.Category_Id = prod.Category_Id;
                ViewBag.Name = prod.Name;
                ViewBag.Description = prod.Description;
            }
            return PartialView();
        }
        public string Add(string category, string name, string Description,int Id)
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
                    Products_Details prds = db.Products_Details.Find(Id);
                    prds.Category_Id = Convert.ToInt32(category);
                    prds.Modified_By = UserName;
                    prds.Date_Modified = DateTime.Now;
                    prds.Description = Description;
                    prds.Name = name;
                    db.Entry(prds).State = EntityState.Modified;
                    db.SaveChanges();
                    return "Product Upadated Successfully";
                }
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
       
        public ActionResult Delete(int id)
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
