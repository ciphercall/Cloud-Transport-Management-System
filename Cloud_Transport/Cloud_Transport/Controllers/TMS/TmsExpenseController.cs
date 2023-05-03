using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cloud_Transport.Models;
using Cloud_Transport.Models.ASL;
using Cloud_Transport.Models.DTO;
using Cloud_Transport.Models.TMS;

namespace Cloud_Transport.Controllers.TMS
{
    public class TmsExpenseController : AppController
    {

        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        CnfDbContext db = new CnfDbContext();
        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;


        public TmsExpenseController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            //td = DateTime.Now;
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            ViewData["HighLight_Menu_TripForm"] = "Heigh Light Menu";
        }




        public ASL_LOG aslLog = new ASL_LOG();
        public void Insert_TmsExpense_LogData(TMS_EXPENSE tmsExpense)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == tmsExpense.COMPID && n.USERID == tmsExpense.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(tmsExpense.COMPID);
            aslLog.USERID = tmsExpense.INSUSERID;
            aslLog.LOGTYPE = "INSERT";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = ipAddress.ToString();
            aslLog.LOGLTUDE = tmsExpense.INSLTUDE;
            aslLog.TABLEID = "TMS_EXPENSE";

            String vechicleNo = "";
            var findCostpoolName = (from m in db.GlCostPoolDbSet where m.COMPID == tmsExpense.COMPID && m.COSTPID == tmsExpense.COSTPID select new { m.COSTPID, m.COSTPNM }).ToList();
            foreach (var item in findCostpoolName)
            {
                vechicleNo = item.COSTPNM;
            }

            DateTime tDate = Convert.ToDateTime(tmsExpense.TRANSDT);
            String expenseDate = tDate.ToString("dd-MMM-yyyy");

            String DebitName = "";
            Int64 headCD = Convert.ToInt64(tmsExpense.COMPID + "401");
            var find_Accounthead = (from n in db.GlAcchartDbSet where n.COMPID == tmsExpense.COMPID && n.HEADCD == headCD && n.ACCOUNTCD == tmsExpense.DEBITCD select new { n.ACCOUNTCD, n.ACCOUNTNM }).ToList();
            foreach (var item in find_Accounthead)
            {
                DebitName = item.ACCOUNTNM;
            }

            aslLog.LOGDATA = Convert.ToString("Expense Date :" + expenseDate + ",\nExpense Month-Year:" + tmsExpense.TRANSMY + ",\nVehicle NO:" + vechicleNo + ",\nInvoice NO:" + tmsExpense.TRANSNO + ",\nTrip No:" + tmsExpense.TRIPNO + ",\nExpense Head Particulars:" + DebitName + ",\nAmount:" + tmsExpense.AMOUNT + ",\nRemarks: " + tmsExpense.REMARKS + ".");
            aslLog.USERPC = strHostName;
            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }



        public void update_TmsExpense_LogData(Tms_ExpenseDTO tmsExpense)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == tmsExpense.COMPID && n.USERID == tmsExpense.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(tmsExpense.COMPID);
            aslLog.USERID = tmsExpense.INSUSERID;
            aslLog.LOGTYPE = "UPDATE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = ipAddress.ToString();
            aslLog.LOGLTUDE = tmsExpense.INSLTUDE;
            aslLog.TABLEID = "TMS_EXPENSE";

            String vechicleNo = "";
            var findCostpoolName = (from m in db.GlCostPoolDbSet where m.COMPID == tmsExpense.COMPID && m.COSTPID == tmsExpense.COSTPID select new { m.COSTPID, m.COSTPNM }).ToList();
            foreach (var item in findCostpoolName)
            {
                vechicleNo = item.COSTPNM;
            }

            DateTime tDate = Convert.ToDateTime(tmsExpense.TRANSDT);
            String expenseDate = tDate.ToString("dd-MMM-yyyy");

            String DebitName = "";
            Int64 headCD = Convert.ToInt64(tmsExpense.COMPID + "401");
            var find_Accounthead = (from n in db.GlAcchartDbSet where n.COMPID == tmsExpense.COMPID && n.HEADCD == headCD && n.ACCOUNTCD == tmsExpense.DEBITCD select new { n.ACCOUNTCD, n.ACCOUNTNM }).ToList();
            foreach (var item in find_Accounthead)
            {
                DebitName = item.ACCOUNTNM;
            }

            aslLog.LOGDATA = Convert.ToString("Expense Date :" + expenseDate + ",\nExpense Month-Year:" + tmsExpense.TRANSMY + ",\nVehicle NO:" + vechicleNo + ",\nInvoice NO:" + tmsExpense.TRANSNO + ",\nTrip No:" + tmsExpense.TRIPNO + ",\nExpense Head Particulars:" + DebitName + ",\nAmount:" + tmsExpense.AMOUNT + ",\nRemarks: " + tmsExpense.REMARKS + ".");
            aslLog.USERPC = strHostName;
            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }



        public void Delete_TmsExpense_LogData(Tms_ExpenseDTO tmsExpense)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == tmsExpense.COMPID && n.USERID == tmsExpense.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(tmsExpense.COMPID);
            aslLog.USERID = tmsExpense.INSUSERID;
            aslLog.LOGTYPE = "DELETE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = ipAddress.ToString();
            aslLog.LOGLTUDE = tmsExpense.INSLTUDE;
            aslLog.TABLEID = "TMS_EXPENSE";

            String vechicleNo = "";
            var findCostpoolName = (from m in db.GlCostPoolDbSet where m.COMPID == tmsExpense.COMPID && m.COSTPID == tmsExpense.COSTPID select new { m.COSTPID, m.COSTPNM }).ToList();
            foreach (var item in findCostpoolName)
            {
                vechicleNo = item.COSTPNM;
            }

            DateTime tDate = Convert.ToDateTime(tmsExpense.TRANSDT);
            String expenseDate = tDate.ToString("dd-MMM-yyyy");

            String DebitName = "";
            Int64 headCD = Convert.ToInt64(tmsExpense.COMPID + "401");
            var find_Accounthead = (from n in db.GlAcchartDbSet where n.COMPID == tmsExpense.COMPID && n.HEADCD == headCD && n.ACCOUNTCD == tmsExpense.DEBITCD select new { n.ACCOUNTCD, n.ACCOUNTNM }).ToList();
            foreach (var item in find_Accounthead)
            {
                DebitName = item.ACCOUNTNM;
            }

            aslLog.LOGDATA = Convert.ToString("Expense Date :" + expenseDate + ",\nExpense Month-Year:" + tmsExpense.TRANSMY + ",\nVehicle NO:" + vechicleNo + ",\nInvoice NO:" + tmsExpense.TRANSNO + ",\nTrip No:" + tmsExpense.TRIPNO + ",\nExpense Head Particulars:" + DebitName + ",\nAmount:" + tmsExpense.AMOUNT + ",\nRemarks: " + tmsExpense.REMARKS + ".");
            aslLog.USERPC = strHostName;
            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }




        public ASL_DELETE AslDelete = new ASL_DELETE();
        public void Delete_TmsExpense_LogDelete(Tms_ExpenseDTO tmsExpense)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("HH:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == tmsExpense.COMPID && n.USERID == tmsExpense.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                AslDelete.DELSLNO = Convert.ToInt64("1");
            }
            else
            {
                AslDelete.DELSLNO = maxSerialNo + 1;
            }

            AslDelete.COMPID = Convert.ToInt64(tmsExpense.COMPID);
            AslDelete.USERID = tmsExpense.INSUSERID; 
            AslDelete.DELSLNO = AslDelete.DELSLNO;
            AslDelete.DELDATE = Convert.ToString(date);
            AslDelete.DELTIME = Convert.ToString(time);
            AslDelete.DELIPNO = ipAddress.ToString();
            AslDelete.DELLTUDE = tmsExpense.INSLTUDE;
            AslDelete.TABLEID = "TMS_EXPENSE";

            String vechicleNo = "";
            var findCostpoolName = (from m in db.GlCostPoolDbSet where m.COMPID == tmsExpense.COMPID && m.COSTPID == tmsExpense.COSTPID select new { m.COSTPID, m.COSTPNM }).ToList();
            foreach (var item in findCostpoolName)
            {
                vechicleNo = item.COSTPNM;
            }

            DateTime tDate = Convert.ToDateTime(tmsExpense.TRANSDT);
            String expenseDate = tDate.ToString("dd-MMM-yyyy");

            String DebitName = "";
            Int64 headCD = Convert.ToInt64(tmsExpense.COMPID + "401");
            var find_Accounthead = (from n in db.GlAcchartDbSet where n.COMPID == tmsExpense.COMPID && n.HEADCD == headCD && n.ACCOUNTCD == tmsExpense.DEBITCD select new { n.ACCOUNTCD, n.ACCOUNTNM }).ToList();
            foreach (var item in find_Accounthead)
            {
                DebitName = item.ACCOUNTNM;
            }

            AslDelete.DELDATA = Convert.ToString("Expense Date :" + expenseDate + ",\nExpense Month-Year:" + tmsExpense.TRANSMY + ",\nVehicle NO:" + vechicleNo + ",\nInvoice NO:" + tmsExpense.TRANSNO + ",\nTrip No:" + tmsExpense.TRIPNO + ",\nExpense Head Particulars:" + DebitName + ",\nAmount:" + tmsExpense.AMOUNT + ",\nRemarks: " + tmsExpense.REMARKS + ".");
            AslDelete.USERPC = strHostName;
            db.AslDeleteDbSet.Add(AslDelete);
            db.SaveChanges();
        }



        // GET: /TmsExpense/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Tms_ExpenseDTO model, string command)
        {
            if (command == "Complete")
            {
                return RedirectToAction("Index");
            }
            else if (command == "Print")
            {
                TempData["TmsExpenseModel"] = model;
                return RedirectToAction("GetTmsExpenseMemo", "TmsReport");
            }
            return RedirectToAction("Index");
        }







        // GET: /TmsExpense/
        public ActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Update(Tms_ExpenseDTO model, string command)
        {
            if (command == "Complete")
            {
                return RedirectToAction("Update");
            }
            else if (command == "Print")
            {
                TempData["TmsExpenseModel"] = model;
                return RedirectToAction("GetTmsExpenseMemo", "TmsReport");
            }
            return RedirectToAction("Update");
        }





        public JsonResult TripNoload(string ChangedText, string Cid)
        {
            Int64 comid = Convert.ToInt64(Cid);
            Int64 Costpid = Convert.ToInt64(ChangedText);

            List<SelectListItem> rtripidList = new List<SelectListItem>();
            var listTripmst = (from trip in db.TmsTripmstDbSet
                               where trip.COMPID == comid && trip.COSTPID == Costpid
                               select new
                               {
                                   trip.RTRIPNO
                               }
                           ).OrderByDescending(x => x.RTRIPNO)
                           .Distinct()
                           .ToList();

            if (listTripmst.Count != 0)
            {
                foreach (var f in listTripmst)
                {
                    rtripidList.Add(new SelectListItem { Text = f.RTRIPNO.ToString(), Value = f.RTRIPNO.ToString() });
                }
            }
            else
            {
                rtripidList.Add(new SelectListItem { Text = null, Value = null });
            }

            var listTrip = ((from trip in db.TmsTripDbSet
                             where trip.COMPID == comid && trip.COSTPID == Costpid
                             select new
                             {
                                 trip.TRIPNO
                             }
                            ).OrderByDescending(x => x.TRIPNO)
                            .Distinct()
                            .ToList());

            if (listTrip.Count != 0)
            {
                foreach (var f in listTrip)
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



        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetMonthYear(String dateText, String compid)
        {
            Int64 comapnyid = Convert.ToInt64(compid);
            string converttoString = Convert.ToString(dateText);
            string getYear = converttoString.Substring(9, 2);
            string getMonth = converttoString.Substring(3, 3).ToUpper();
            string monthYear = getMonth + "-" + getYear;//DEC-15 (Example)

            DateTime getDate = Convert.ToDateTime(dateText);
            string cm = getDate.ToString("MM"), cy = getDate.ToString("yyyy"), memoNo = "";
            var findTransNo = (from m in db.TmsExpenseDbSet
                               where m.COMPID == comapnyid && m.TRANSMY == monthYear
                               select m).ToList();
            if (findTransNo.Count == 0)
            {
                memoNo = cy + cm + "0001";
            }
            else
            {
                Int64 max_TransNO = Convert.ToInt64((from m in db.TmsExpenseDbSet where m.COMPID == comapnyid && m.TRANSMY == monthYear select m.TRANSNO).Max());
                Int64 R = Convert.ToInt64(cy + cm + "9999");
                if (max_TransNO <= R)
                {
                    memoNo = Convert.ToString(max_TransNO + 1);
                }
                else
                {
                    memoNo = "Not access!!!";
                }
            }

            var result = new { myear = monthYear, MemoNo = memoNo };
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        public JsonResult TrnasNoLoad(string theDate, string compid)
        {
            Int64 comid = Convert.ToInt64(compid);
            DateTime dt = Convert.ToDateTime(theDate);

            String myear = dt.ToString("MMM-yy").ToUpper();

            List<SelectListItem> memoNoList = new List<SelectListItem>();
            var list = (from n in db.TmsExpenseDbSet
                        where n.TRANSDT == dt && n.COMPID == comid && n.TRANSMY == myear
                        select new
                        {
                            n.TRANSNO
                        }
                            )
                            .Distinct()
                            .ToList();

            if (list.Count != 0)
            {
                foreach (var f in list)
                {
                    memoNoList.Add(new SelectListItem { Text = f.TRANSNO.ToString(), Value = f.TRANSNO.ToString() });
                }
            }
            else
            {
                memoNoList.Add(new SelectListItem { Text = null, Value = null });
            }
            return Json(new SelectList(memoNoList, "Value", "Text"));
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetTripNoAndCostPoolName(String transNo, String compid, String monthYear)
        {
            Int64 companyID = Convert.ToInt16(compid);
            Int64 transactionNo = Convert.ToInt64(transNo);
            Int64 Costpid = 0, tripNo = 0;
            String CostpoolName = "";
            var find_CostPID_tripNo = (from m in db.TmsExpenseDbSet where m.COMPID == companyID && m.TRANSNO == transactionNo && m.TRANSMY == monthYear select new { m.COSTPID, m.TRIPNO }).ToList();
            foreach (var a in find_CostPID_tripNo)
            {
                Costpid = Convert.ToInt64(a.COSTPID);
                tripNo = Convert.ToInt64(a.TRIPNO);
                break;
            }
            var findCostpoolName = (from m in db.GlCostPoolDbSet where m.COMPID == companyID && m.COSTPID == Costpid select new { m.COSTPNM }).ToList();
            foreach (var b in findCostpoolName)
            {
                CostpoolName = b.COSTPNM;
                break;
            }

            var result = new
            {
                CPname = CostpoolName,
                Cpid = Costpid,
                tripno = tripNo,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
