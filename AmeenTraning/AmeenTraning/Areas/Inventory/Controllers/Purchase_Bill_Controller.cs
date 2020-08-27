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
    public class Purchase_Bill_Controller : Controller
    {
        private TrainingEntities db = new TrainingEntities();
        // GET: Inventory/Purchase_Bill_
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult IndexPartial()
        {

            return PartialView();
        }
        public PartialViewResult CreatePartial()
        {
            
            string Company_Id = string.Empty; 
            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                Company_Id = myCookie.Values["Company_Id"].ToString();
            }
			var compid = Convert.ToInt32(Company_Id);
            ViewBag.Date = DateTime.Now.ToString("MM/dd/yyyy");
            ViewBag.Supplier = db.Supplier_Details.Where(e => e.Company_Id == compid).ToList();
            ViewBag.Products = db.Products_Details.Where(e => e.Company_Id == compid).ToList();
            return PartialView();
        }
    }
}