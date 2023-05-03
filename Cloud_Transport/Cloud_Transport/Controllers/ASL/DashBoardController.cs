using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cloud_Transport.DataAccess;
using Cloud_Transport.Models;

namespace Cloud_Transport.Controllers.ASL
{
    public class DashBoardController : AppController
    {
        public HttpCookie userCookie;
        public Int64 loggedcompid;

        public DashBoardController()
        {
            //if (System.Web.HttpContext.Current.Request.Cookies["user"] != null)
            //{
            //    userCookie = System.Web.HttpContext.Current.Request.Cookies["user"];
            //    loggedcompid = Convert.ToInt64(userCookie.Values["loggedCompID"]);
            //}
            //HttpCookie decodedCookie1 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["CI"]);
            //HttpCookie decodedCookie2 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UI"]);
            //HttpCookie decodedCookie3 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UT"]);
            //HttpCookie decodedCookie4 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UN"]);
            //HttpCookie decodedCookie5 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["US"]);
            //HttpCookie decodedCookie6 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["CS"]);

            if (System.Web.HttpContext.Current.Session["loggedCompID"] != null)
            {
                loggedcompid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            }
            else
            {
                Index();
            }

            ViewData["HighLight_Menu_DashBoard"] = "High Light DashBoard";

        }

        TimeZoneInfo timeZoneInfo;


        // GET: /GraphView/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexPost(String tN, String vn, String my)
        {
            String trip = tN.Substring(11, 8);
            String costpid = vn.Substring(11, 10);

            Int64 tripNo = Convert.ToInt64(trip);
            Int64 vehicleNumber = Convert.ToInt64(costpid);

            PageModel model = new PageModel();
            model.TmsTripmst.COMPID = loggedcompid;
            model.TmsTrip.TRIPMY = my;
            model.TmsTrip.COSTPID = vehicleNumber;
            model.TmsTrip.TRIPNO = tripNo;
            TempData["TripSummarymodel"] = model;
            return RedirectToAction("GetTripSummary", "TmsReport");
        }



        public ActionResult Today()
        {
            return View();
        }

        public ActionResult LastOneDay()
        {
            return View();
        }
        public ActionResult Last7Day()
        {
            return View();
        }
        public ActionResult LastOneMonth()
        {
            return View();
        }

        public ActionResult LastOneYear()
        {
            return View();
        }



        public JsonResult Topcategories(string d)
        {
            Int64 n = Convert.ToInt64(d);
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime dt = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            string frdate = dt.ToString("yyyy-MM-dd");
            string todate = "";

            if (n == 364)
            {
                int year = DateTime.Now.Year;
                DateTime firstDay = new DateTime(year, 1, 1);
                todate = firstDay.ToString("yyyy-MM-dd");
            }
            else
            {
                DateTime dt2 = TimeZoneInfo.ConvertTime(DateTime.Today.AddDays(-n), timeZoneInfo);
                todate = dt2.ToString("yyyy-MM-dd");
            }



            mydataservice objMD = new mydataservice();
            var chartsdata = objMD.TopcategoriesListdata(loggedcompid, todate, frdate); // calling method Listdata
            return Json(chartsdata, JsonRequestBehavior.AllowGet); // returning list from here.
        }





        public JsonResult TopItemsByQty(string d)
        {
            Int64 n = Convert.ToInt64(d);
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime dt = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            string frdate = dt.ToString("yyyy-MM-dd");
            string todate = "";

            if (n == 364)
            {
                int year = DateTime.Now.Year;
                DateTime firstDay = new DateTime(year, 1, 1);
                todate = firstDay.ToString("yyyy-MM-dd");
            }
            else
            {
                DateTime dt2 = TimeZoneInfo.ConvertTime(DateTime.Today.AddDays(-n), timeZoneInfo);
                todate = dt2.ToString("yyyy-MM-dd");
            }

            mydataservice objMD = new mydataservice();
            var chartsdata = objMD.TopItemsByQtyListdata(loggedcompid, todate, frdate); // calling method Listdata
            return Json(chartsdata, JsonRequestBehavior.AllowGet); // returning list from here.
        }








        public JsonResult TopItemsByValue(string d)
        {
            Int64 n = Convert.ToInt64(d);
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime dt = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            string frdate = dt.ToString("yyyy-MM-dd");
            string todate = "";

            if (n == 364)
            {
                int year = DateTime.Now.Year;
                DateTime firstDay = new DateTime(year, 1, 1);
                todate = firstDay.ToString("yyyy-MM-dd");
            }
            else
            {
                DateTime dt2 = TimeZoneInfo.ConvertTime(DateTime.Today.AddDays(-n), timeZoneInfo);
                todate = dt2.ToString("yyyy-MM-dd");
            }


            mydataservice objMD = new mydataservice();
            var chartsdata = objMD.TopItemsByValueListdata(loggedcompid, todate, frdate); // calling method Listdata
            return Json(chartsdata, JsonRequestBehavior.AllowGet); // returning list from here.
        }








        public JsonResult CollectionData(string d)
        {
            Int64 n = Convert.ToInt64(d);
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime dt = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            string frdate = dt.ToString("yyyy-MM-dd");
            string todate = "";

            if (n == 364)
            {
                int year = DateTime.Now.Year;
                DateTime firstDay = new DateTime(year, 1, 1);
                todate = firstDay.ToString("yyyy-MM-dd");
            }
            else
            {
                DateTime dt2 = TimeZoneInfo.ConvertTime(DateTime.Today.AddDays(-n), timeZoneInfo);
                todate = dt2.ToString("yyyy-MM-dd");
            }


            mydataservice objMD = new mydataservice();
            var chartsdata = objMD.CollectionDataListdata(loggedcompid, todate, frdate); // calling method Listdata
            return Json(chartsdata, JsonRequestBehavior.AllowGet); // returning list from here.
        }









        //public JsonResult TimeWiseSellData(string d)
        //{
        //    Int64 n = Convert.ToInt64(d);
        //    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        //    DateTime dt = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        //    string frdate = dt.ToString("yyyy-MM-dd");
        //    string todate = "";

        //    if (n == 364)
        //    {
        //        int year = DateTime.Now.Year;
        //        DateTime firstDay = new DateTime(year, 1, 1);
        //        todate = firstDay.ToString("yyyy-MM-dd");
        //    }
        //    else
        //    {
        //        DateTime dt2 = TimeZoneInfo.ConvertTime(DateTime.Today.AddDays(-n), timeZoneInfo);
        //        todate = dt2.ToString("yyyy-MM-dd");
        //    }

        //    mydataservice objMD = new mydataservice();
        //    var chartsdata = objMD.TimeWiseSellDataListdata(loggedcompid, todate, frdate); // calling method Listdata
        //    return Json(chartsdata, JsonRequestBehavior.AllowGet); // returning list from here.
        //}
    }
}
