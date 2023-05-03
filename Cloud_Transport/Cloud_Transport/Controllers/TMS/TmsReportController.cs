using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cloud_Transport.Models;
using Cloud_Transport.Models.DTO;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Cloud_Transport.Controllers.TMS
{
    public class TmsReportController : AppController
    {
        private CnfDbContext db = new CnfDbContext();



        //TMS Report (FUEL CONSUMPTION)
        public ActionResult FuelConsumption()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FuelConsumption(PageModel model)
        {
            model.TmsTripmst.TRIPMY = model.Report_FromDate.ToString().ToUpper();
            TempData["FuelConsumption_model"] = model;
            return RedirectToAction("GetFuelConsumption");
        }

        public ActionResult GetFuelConsumption()
        {
            var model = (PageModel)TempData["FuelConsumption_model"];
            return View(model);
        }






        //TMS Report (VEHICLE-TRIP EXPENSE)
        public ActionResult VehicleTripExpense()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VehicleTripExpense(PageModel model)
        {
            model.AGlMaster.TRANSMY = model.Report_FromDate.ToString().ToUpper();
            TempData["VehicleTripExpense_model"] = model;
            return RedirectToAction("GetVehicleTripExpense");
        }

        public ActionResult GetVehicleTripExpense()
        {
            var model = (PageModel)TempData["VehicleTripExpense_model"];
            return View(model);
        }





        //TMS Report (VEHICLE-OTHER EXPENSE - VEHICLE_EXP_ST)
        public ActionResult VehicleOtherExpense()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VehicleOtherExpense(PageModel model)
        {
            model.TmsTripmst.TRIPMY = model.Report_FromDate.ToString().ToUpper();
            TempData["model"] = model;
            return RedirectToAction("GetVehicleOtherExpense");
        }

        public ActionResult GetVehicleOtherExpense()
        {
            var model = (PageModel)TempData["model"];
            return View(model);
        }




        //TMS Report ( VEHICLE WISE TRIP INFORMATION)
        public ActionResult VehicleWiseTripInformation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VehicleWiseTripInformation(PageModel model)
        {
            model.TmsTrip.TRIPMY = model.Report_FromDate.ToString().ToUpper();
            TempData["model"] = model;
            return RedirectToAction("GetVehicleWiseTripInformation");
        }

        public ActionResult GetVehicleWiseTripInformation()
        {
            var model = (PageModel)TempData["model"];
            return View(model);
        }






        //TMS Report (VEHICLE-TRIP OP. P/L)
        public ActionResult VehicleTripOp_PL()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VehicleTripOp_PL(PageModel model)
        {
            model.TmsTrip.TRIPMY = model.Report_FromDate.ToString().ToUpper();
            TempData["model"] = model;
            return RedirectToAction("GetVehicleTripOp_PL");
        }

        public ActionResult GetVehicleTripOp_PL()
        {
            var model = (PageModel)TempData["model"];
            return View(model);
        }




        //TMS Report (VEHICLE-SUMMARY OP. P/L)
        public ActionResult VehicleSummaryOp_PL()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VehicleSummaryOp_PL(PageModel model)
        {
            model.TmsTrip.TRIPMY = model.Report_FromDate.ToString().ToUpper();
            TempData["model"] = model;
            return RedirectToAction("GetVehicleSummaryOp_PL");
        }

        public ActionResult GetVehicleSummaryOp_PL()
        {
            var model = (PageModel)TempData["model"];
            return View(model);
        }



        //TMS Report (TRIP SUMMARY)
        public ActionResult TripSummary()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TripSummary(PageModel model)
        {
            model.TmsTrip.TRIPMY = model.Report_FromDate.ToString().ToUpper();
            TempData["TripSummarymodel"] = model;
            return RedirectToAction("GetTripSummary");
        }

        public ActionResult GetTripSummary()
        {
            var model = (PageModel)TempData["TripSummarymodel"];
            return View(model);
        }

        public JsonResult TripNoLoad(string compid, string vno, string monthyear)
        {
            Int64 comid = Convert.ToInt64(compid);
            String myear = monthyear.ToUpper();
            Int64 Costpid = Convert.ToInt64(vno);

            List<SelectListItem> rtripidList = new List<SelectListItem>();
            var list = (from n in db.TmsTripDbSet
                        where n.COMPID == comid && n.COSTPID == Costpid && n.TRIPMY == myear
                        select new
                        {
                            n.TRIPNO
                        }
                            )
                            .Distinct()
                            .ToList();

            if (list.Count != 0)
            {
                foreach (var f in list)
                {
                    rtripidList.Add(new SelectListItem { Text = f.TRIPNO.ToString(), Value = f.TRIPNO.ToString() });
                }
            }
            else
            {
                rtripidList.Add(new SelectListItem { Text = null, Value = null });
            }
            return Json(new SelectList(rtripidList, "Value", "Text"));
        }






        

        //From Create & Update Tms_Expense Page
        public ActionResult GetTmsExpenseMemo()
        {
            var model = (Tms_ExpenseDTO)TempData["TmsExpenseModel"];
            return View(model);
        }

      
    }
}
