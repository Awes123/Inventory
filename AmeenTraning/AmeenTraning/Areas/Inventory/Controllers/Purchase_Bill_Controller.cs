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
            string Company_Id = string.Empty;
            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                Company_Id = myCookie.Values["Company_Id"].ToString();
            }
            var compid = Convert.ToInt32(Company_Id);
            ViewBag.Products = db.Products_Details.Where(e => e.Company_Id == compid).ToList();
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
        public int Add(decimal GST, decimal GSTamount, decimal Discount, decimal GrandTotal, int Supplier_ID, decimal Quantity, decimal Transport_Charge, decimal OtherExpences, string Comment, string PaymentMode)
        {
            string Company_Id = string.Empty;
            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                Company_Id = myCookie.Values["Company_Id"].ToString();
            }
            var compid = Convert.ToInt32(Company_Id);


            Purchase_Bill purchase = new Purchase_Bill();
            purchase.Date_Created = DateTime.Now;
            purchase.Company_Id = compid;
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
            var Bill_Id = db.Purchase_Bill.Where(e => e.Company_Id == compid && e.Supplier_ID == Supplier_ID && e.Total_Amount == GrandTotal).Select(e => e.Bill_No).FirstOrDefault();
          
            return Bill_Id;
        }

        public string Add_Item(decimal qty, decimal gst, decimal price, int prodocut_Id, decimal discount, string unit, decimal amount, int bill_No)
        {
            string Company_Id = string.Empty;
            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                Company_Id = myCookie.Values["Company_Id"].ToString();
            }
            var compid = Convert.ToInt32(Company_Id);
            Purchase_Bill_Item bill_Item = new Purchase_Bill_Item();
            bill_Item.Purchase_Bill_ID = bill_No;
            bill_Item.Product_id = prodocut_Id;
            bill_Item.Quantity = qty;
            bill_Item.Unit = unit;
            bill_Item.Price = price;
            bill_Item.Discount = discount;
            bill_Item.Amount = amount;
            db.Purchase_Bill_Item.Add(bill_Item);
            db.SaveChanges();

            return "Bill Added";
        }

        public ActionResult Delete(int id)
        {
            Purchase_Bill purchase_Bill = db.Purchase_Bill.Find(id);
            db.Purchase_Bill.Remove(purchase_Bill);
            db.SaveChanges();
            Purchase_Bill_Item bill_Item = db.Purchase_Bill_Item.Where(e=>e.Purchase_Bill_ID == id).FirstOrDefault();
            db.Purchase_Bill_Item.Remove(bill_Item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public PartialViewResult Edit(int? id)
		{
            string Company_Id = string.Empty;
            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                Company_Id = myCookie.Values["Company_Id"].ToString();
            }
            var compid = Convert.ToInt32(Company_Id);
            var supplier = db.Purchase_Bill.Where(e => e.Bill_No == id).FirstOrDefault();
            var suppname = db.Supplier_Details.Where(e=>e.Supplier_Id == supplier.Supplier_ID).FirstOrDefault();
            ViewBag.Billno = supplier.Bill_No;
            ViewBag.Date = supplier.Date_Created;
            ViewBag.selctedsup = suppname.Name;
            ViewBag.Supplier = db.Supplier_Details.Where(e => e.Company_Id == compid).ToList();
            ViewBag.Products = db.Products_Details.Where(e => e.Company_Id == compid).ToList();
            return PartialView();
        }

    }
}