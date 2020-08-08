using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmeenTraning.Areas.Inventory.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        string Company_Id = string.Empty;
        string UserId = string.Empty;
       
        // GET: Inventory/Dashboard
        public ActionResult Index()
        {
            HttpCookie myCookie = Request.Cookies["inventoryCookie"];
            if (myCookie == null)
            {
                return Redirect("/Auth/Login/Logout");
            }
            if (!string.IsNullOrEmpty(myCookie.Values["Company_Id"]))
            {
                string Company_Id = myCookie.Values["Company_Id"].ToString();
            }
            if (!string.IsNullOrEmpty(myCookie.Values["UserId"]))
            {
                string UserId = myCookie.Values["UserId"].ToString();
            }
            return View();
        }
    }
}