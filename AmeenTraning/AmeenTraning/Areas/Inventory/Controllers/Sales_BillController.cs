using AmeenTraning.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AmeenTraning.Areas.Inventory.Controllers
{
    public class Sales_BillController : Controller
    {
        private TrainingEntities db = new TrainingEntities();
        // GET: Inventory/Sales_Bill
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

        // Index Partial 
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
            return PartialView(db.Sales_Bill.ToList());          
        }


        // Create Partial
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
            //ViewBag.Customer = db.Customer_Details.Where(e => e.Customer_Id == compid).ToList();
            ViewBag.Customers = db.Customer_Details.Where(e => e.Company_Id == compid).ToList();
            ViewBag.Products = db.Products_Details.Where(e => e.Company_Id == compid).ToList();
            Sales_Bill sales_Bill = new Sales_Bill();
            if (id != null)
            {
                sales_Bill = db.Sales_Bill.Where(c => c.Sales_Bill_No == id).FirstOrDefault();
            }
            return PartialView(sales_Bill);
        }

        // Add Function
        public int Add(decimal GSTamount, decimal Discount, decimal GrandTotal, int Customer_ID, decimal Quantity, decimal delivery, decimal OtherExpences, string Comment, string PaymentMode, decimal Subtotal, decimal GST,int?Id)
        {
            string Company_Id = string.Empty;
            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                Company_Id = myCookie.Values["Company_Id"].ToString();
            }
            var compid = Convert.ToInt32(Company_Id);
                
            Sales_Bill sales_Bill = new Sales_Bill();
            if (Id != 0)
            {
                sales_Bill = db.Sales_Bill.Where(e => e.Sales_Bill_No == Id).FirstOrDefault();

                sales_Bill.Date_Modified = DateTime.Now;
            }
            else
            {
                sales_Bill.Date_Created = DateTime.Now;
            }
            sales_Bill.Company_ID = compid;
            sales_Bill.Delivery = delivery;
            sales_Bill.Discount = Discount;
            sales_Bill.Customer_ID = Customer_ID;
            sales_Bill.GST_Amount = GSTamount;
            sales_Bill.Quantity = Quantity;
            sales_Bill.Other_Expenses = OtherExpences;
            sales_Bill.Payment_Mode = PaymentMode;
            sales_Bill.Comment = Comment;
            sales_Bill.Total_Amount = GrandTotal;
            sales_Bill.GST = GST;
            sales_Bill.SubTotal = Subtotal;
            if (Id != 0) {
                db.Entry(sales_Bill).State = EntityState.Modified;
            }
            else
            {
                db.Sales_Bill.Add(sales_Bill);

            }
            db.SaveChanges();

            var Sales_Bill_Id = db.Sales_Bill.Where(e => e.Company_ID == compid && e.Customer_ID == Customer_ID && e.Total_Amount == GrandTotal).Select(e => e.Sales_Bill_No).FirstOrDefault();

            return Sales_Bill_Id;
        }

        // Add item Function
        public string Add_Item(decimal qty, decimal gst, decimal price, int prodocut_Id, decimal discount, string unit, decimal amount, int bill_No, int? itemid)
        {
            string Company_Id = string.Empty;
            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                Company_Id = myCookie.Values["Company_Id"].ToString();
            }
            var compid = Convert.ToInt32(Company_Id);

            Sales_Bill_item sales_Bill_Item = new Sales_Bill_item();

            if(itemid != 0)
            {
                sales_Bill_Item = db.Sales_Bill_item.Where(e => e.Sales_Bill_item_ID == itemid).FirstOrDefault();
                sales_Bill_Item.Date_Modified = DateTime.Now;
            }
            else
            {
                sales_Bill_Item.Date_Created = DateTime.Now;
            }
            sales_Bill_Item.Sales_Bill_No = bill_No;
            sales_Bill_Item.Product_ID = prodocut_Id;
            sales_Bill_Item.Quantity = qty;
            sales_Bill_Item.Unit = unit;
            sales_Bill_Item.Price = price;
            sales_Bill_Item.Discount = discount;
            sales_Bill_Item.GST = gst;
            sales_Bill_Item.Amount = amount;
            db.Sales_Bill_item.Add(sales_Bill_Item);
            if(itemid != 0)
            {
                db.Entry(sales_Bill_Item).State = EntityState.Modified;
                db.SaveChanges();
                AddtoStock(Convert.ToInt32(sales_Bill_Item.Sales_Bill_No), Convert.ToInt32(sales_Bill_Item.Product_ID), qty, sales_Bill_Item.Sales_Bill_item_ID, "update");
            }
            else
            {
                db.Sales_Bill_item.Add(sales_Bill_Item);
                db.SaveChanges();
                AddtoStock(Convert.ToInt32(sales_Bill_Item.Sales_Bill_No), Convert.ToInt32(sales_Bill_Item.Product_ID), qty, sales_Bill_Item.Sales_Bill_item_ID, "Add");
            }
            

            return " Sales Bill Item Added";
        }

        // Stock 
        public string AddtoStock(int Bill_Id, int Product_Id, decimal qty, int ItemId, string request)
        {
            var stock = db.Stock_Details.Where(e => e.Product_ID == Product_Id).ToList();
            decimal tQty = 0; 
            if (request == "update")
            {
                if (stock != null)
                {
                    //    var stockq = stock.Where(e => e.Item_ID != ItemId).Select(e => e.Total_Quantity).Sum();
                    var stockq = stock.Where(e => e.Product_ID == Product_Id).ToList();
                    if (stockq.Count() != 0)
                    {
                        tQty = Convert.ToDecimal(stockq) - Convert.ToDecimal(qty);
                    }
                    else {
                        tQty = 0;
                    }
                }
                var stocks = db.Stock_Details.Where(e => e.Item_ID == ItemId).FirstOrDefault();
                stocks.Quantity = qty;
                stocks.Total_Quantity = tQty;
                stocks.Date_Modified = DateTime.Now;
                stocks.Type = "Sales";
                stocks.Product_ID = Product_Id;
                db.Entry(stocks).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                if (stock != null)
                {
                    var stockq = stock.Where(e => e.Item_ID != ItemId).Select(e => e.Total_Quantity).Sum();
                    if (stockq != 0)
                    {

                        tQty = Convert.ToDecimal(stockq) - Convert.ToDecimal(qty);
                    }
                }

                db.Stock_Details.Add(new Stock_Details
                {
                    Quantity = qty,
                    Total_Quantity = tQty,
                    Product_ID = Product_Id,
                    Date_Created = DateTime.Now,
                    Sales_Bill_ID = Bill_Id,
                    Item_ID = ItemId,
                    Type = "Sales",


                });
                db.SaveChanges();
            }
            return "";
        }
        // Delete 
        public ActionResult Delete(int id)
        {
            var salesitem = db.Sales_Bill_item.Where(e => e.Sales_Bill_No == id).ToList();
            foreach (var item in salesitem)
            {
                var items = db.Sales_Bill_item.Where(e => e.Sales_Bill_item_ID == item.Sales_Bill_item_ID).FirstOrDefault();
                db.Sales_Bill_item.Remove(items);
                db.SaveChanges();
            }
            Sales_Bill sales_Bill = db.Sales_Bill.Find(id);
            db.Sales_Bill.Remove(sales_Bill);
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
            var sales = db.Sales_Bill.Where(e => e.Sales_Bill_No == id).FirstOrDefault();
            var customer = db.Customer_Details.Where(e => e.Customer_Id == sales.Customer_ID).FirstOrDefault();
            ViewBag.Billno = sales.Sales_Bill_No;
            ViewBag.Date = sales.Date_Created;
            ViewBag.selctedCustomer = customer.Name;
            ViewBag.Comment = sales.Comment;
            ViewBag.OtherExpense = sales.Other_Expenses;
            ViewBag.Delivery = sales.Delivery;
            ViewBag.Payment = sales.Payment_Mode;

            ViewBag.GSTAmount = sales.GST_Amount;
            ViewBag.Discount = sales.Discount;
            ViewBag.GrantTotal = sales.Total_Amount;
            ViewBag.Subtotal = sales.SubTotal;
            ViewBag.GST = sales.GST;
            ViewBag.Customers = db.Customer_Details.Where(e => e.Company_Id == compid).ToList();
            ViewBag.Products = db.Products_Details.Where(e => e.Company_Id == compid).ToList();
            ViewBag.SalesBillItem = db.Sales_Bill_item.Where(e => e.Sales_Bill_No == id).ToList();
            return PartialView();
        }
    }
}