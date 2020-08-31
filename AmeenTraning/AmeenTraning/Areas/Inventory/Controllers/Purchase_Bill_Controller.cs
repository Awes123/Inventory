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
            //var PurchaseBills = db.Purchase_Bill.Where(e => e.Company_Id == company_Id).ToList();
            return PartialView(db.Purchase_Bill.ToList());
        }
        public PartialViewResult CreatePartial(int? id)
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
            Purchase_Bill bill = new Purchase_Bill();
            if (id != null)
            {
                bill = db.Purchase_Bill.Where(c => c.Bill_No == id).FirstOrDefault();
            }
            return PartialView(bill);
        }
        public string Add(decimal GST, decimal GSTamount, decimal Discount, decimal GrandTotal, int Supplier_ID, decimal Quantity, decimal Transport_Charge, decimal OtherExpences, string Comment, string PaymentMode)
        {
            Purchase_Bill purchase = new Purchase_Bill();

            purchase.Date_Created = DateTime.Now;
            purchase.Supplier_ID = Supplier_ID;
            purchase.Quantity = Quantity;
            purchase.Discount = Discount;
            purchase.GST_Amount = GSTamount;
            purchase.Total_Amount = GrandTotal;
            purchase.Comment = Comment;
            purchase.Transport_Charges = Transport_Charge;
            purchase.Other_Expenses = OtherExpences;
            purchase.Payment_Mode = PaymentMode;

            db.Purchase_Bill.Add(purchase);
            db.SaveChanges();

            return "Bill Added";
        }
    }
}