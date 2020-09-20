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

        public int Add(decimal GST, decimal GSTamount, decimal Discount, decimal GrandTotal, int Supplier_ID, decimal Quantity, decimal Transport_Charge, decimal OtherExpences, string Comment, string PaymentMode, decimal Subtotal, int Id)
        {
            string Company_Id = string.Empty;
            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                Company_Id = myCookie.Values["Company_Id"].ToString();
            }
            var compid = Convert.ToInt32(Company_Id);

            Purchase_Bill purchase = new Purchase_Bill();
            if (Id != 0)
            {
                purchase = db.Purchase_Bill.Where(e => e.Bill_No == Id).FirstOrDefault();
                purchase.Date_Modified = DateTime.Now;
            }
            else
            {
                purchase.Date_Created = DateTime.Now;
            }
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
            purchase.GST = GST;
            purchase.SubTotal = Subtotal;
            db.Purchase_Bill.Add(purchase);
            if (Id != 0)
            {
                db.Entry(purchase).State = EntityState.Modified;
            }
            else
            {
                db.Purchase_Bill.Add(purchase);
            }
            db.SaveChanges();
            var Bill_Id = db.Purchase_Bill.Where(e => e.Company_Id == compid && e.Supplier_ID == Supplier_ID && e.Total_Amount == GrandTotal).Select(e => e.Bill_No).FirstOrDefault();

            return Bill_Id;
        }

        public string Add_Item(decimal qty, decimal gst, decimal price, int prodocut_Id, decimal discount, string unit, decimal amount, int bill_No, int? itemid)
        {
            string Company_Id = string.Empty;
            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                Company_Id = myCookie.Values["Company_Id"].ToString();
            }
            var compid = Convert.ToInt32(Company_Id);
            Purchase_Bill_Item bill_Item = new Purchase_Bill_Item();
            if (itemid != null)
            {
                bill_Item = db.Purchase_Bill_Item.Where(e => e.Purchase_item_ID == itemid).FirstOrDefault();
                bill_Item.Date_Modified = DateTime.Now;
            }
            else
            {
                bill_Item.Date_Created = DateTime.Now;
            }
            bill_Item.Purchase_Bill_ID = bill_No;
            bill_Item.Product_id = prodocut_Id;
            bill_Item.Quantity = qty;
            bill_Item.Unit = unit;
            bill_Item.Price = price;
            bill_Item.Discount = discount;
            bill_Item.Amount = amount;
            bill_Item.GST = gst;
            db.Purchase_Bill_Item.Add(bill_Item);
            if (itemid != null)
            {
                db.Entry(bill_Item).State = EntityState.Modified;
                db.SaveChanges();
                AddtoStock(Convert.ToInt32(bill_Item.Purchase_Bill_ID), Convert.ToInt32(bill_Item.Product_id), qty, bill_Item.Purchase_item_ID, "update");
            }
            else
            {
                db.Purchase_Bill_Item.Add(bill_Item);
                db.SaveChanges();
                AddtoStock(Convert.ToInt32(bill_Item.Purchase_Bill_ID), Convert.ToInt32(bill_Item.Product_id), qty, bill_Item.Purchase_item_ID,"Add");
            }
            // Stock code


            return "Bill Added";
        }

        public string AddtoStock(int Bill_Id, int Product_Id, decimal qty,int ItemId,string request)
        {
            var stock = db.Stock_Details.Where(e => e.Product_ID == Product_Id).ToList(); 
            decimal tQty = qty;
            if (request == "update")
            {
                if (stock.Count() != 0)
                {
                    var stockq = stock.Where(e => e.Item_ID != ItemId).Select(e => e.Total_Quantity).Sum();
                    if (stockq != 0)
                    {
                        tQty += Convert.ToDecimal(stockq);
                    }
                }
                var stocks = db.Stock_Details.Where(e => e.Item_ID == ItemId).FirstOrDefault();
                stocks.Quantity = qty;
                stocks.Total_Quantity = tQty;
                stocks.Date_Modified = DateTime.Now;
                stocks.Type = "Purchase";
                stocks.Product_ID = Product_Id;
                db.Entry(stocks).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                if (stock.Count() != 0)
                {
                    //var stockq = stock.Where(e => e.Item_ID != ItemId).Select(e => e.Total_Quantity).Sum();
                    var stockq = stock.Where(e => e.Product_ID == Product_Id).ToList();
                    if (stockq.Count() != 0)
                    {
                        tQty += Convert.ToDecimal(stockq);
                    }
                }

                db.Stock_Details.Add(new Stock_Details
                {
                    Quantity = qty,
                    Total_Quantity=tQty,
                    Product_ID=Product_Id,
                    Date_Created=DateTime.Now,
                    Purchase_Bill_ID= Bill_Id,
                    Item_ID=ItemId,
                    Type="Purchase",
                    
                    
                });
                db.SaveChanges();
            }
            return "";
        }
        public ActionResult Delete(int id)
        {
            
            var PurchaseBill_item = db.Purchase_Bill_Item.Where(e => e.Purchase_Bill_ID == id).ToList();
            foreach(var item in PurchaseBill_item)
            {
                var items = db.Purchase_Bill_Item.Where(e => e.Purchase_item_ID == item.Purchase_item_ID).FirstOrDefault();
                db.Purchase_Bill_Item.Remove(items);
                db.SaveChanges();

            }

            Purchase_Bill purchase_Bill = db.Purchase_Bill.Find(id);
            db.Purchase_Bill.Remove(purchase_Bill);
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
            ViewBag.Comment = supplier.Comment;
            ViewBag.OtherExpense = supplier.Other_Expenses;
            ViewBag.TransportCharge = supplier.Transport_Charges;
            ViewBag.Payment = supplier.Payment_Mode;
            ViewBag.SubTotal = supplier.SubTotal;
            ViewBag.GST = supplier.GST;
            ViewBag.GSTAmount = supplier.GST_Amount;
            ViewBag.Discount = supplier.Discount;
            ViewBag.GrandTotal = supplier.Total_Amount;
            ViewBag.Supplier = db.Supplier_Details.Where(e => e.Company_Id == compid).ToList();
            ViewBag.Products = db.Products_Details.Where(e => e.Company_Id == compid).ToList();
            ViewBag.PurchaseBillItem = db.Purchase_Bill_Item.Where(e => e.Purchase_Bill_ID == id).ToList();
            return PartialView();
        }

    }
}