using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cloud_Transport.Models;
using Cloud_Transport.Models.ASL;
using Cloud_Transport.Models.TMS;

namespace Cloud_Transport.Controllers.TMS
{
    public class TmsTripController : AppController
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


        public TmsTripController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            //td = DateTime.Now;
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            ViewData["HighLight_Menu_TripForm"] = "Heigh Light Menu";
        }




        public ASL_LOG aslLog = new ASL_LOG();
        public void Insert_TmsTripMaster_LogData(TMS_TRIPMST tripmst)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == tripmst.COMPID && n.USERID == tripmst.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(tripmst.COMPID);
            aslLog.USERID = tripmst.INSUSERID;
            aslLog.LOGTYPE = "INSERT";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = ipAddress.ToString();
            aslLog.LOGLTUDE = tripmst.INSLTUDE;
            aslLog.TABLEID = "TMS_TRIPMST";

            String vechicleNo = "";
            var findCostpoolName = (from m in db.GlCostPoolDbSet where m.COMPID == tripmst.COMPID && m.COSTPID == tripmst.COSTPID select new { m.COSTPID, m.COSTPNM }).ToList();
            foreach (var item in findCostpoolName)
            {
                vechicleNo = item.COSTPNM;
            }

            DateTime tDate = Convert.ToDateTime(tripmst.TRIPDT);
            String TripDate = tDate.ToString("dd-MMM-yyyy");

            aslLog.LOGDATA = Convert.ToString("Vehicle NO:" + vechicleNo + ",\nTrip Date:" + TripDate + ",\nTrip Month-Year:" + tripmst.TRIPMY + ",\nRound Trip No:" + tripmst.RTRIPNO + ",\nQty-Fuel:" + tripmst.QTYFUEL + ",\nQty-Mobil:" + tripmst.QTYMOBIL + ",\nRemarks: " + tripmst.REMARKS + ".");
            aslLog.USERPC = strHostName;
            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }


        public void Insert_TmsTrip_LogData(TMS_TRIP trip)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == trip.COMPID && n.USERID == trip.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(trip.COMPID);
            aslLog.USERID = trip.INSUSERID;
            aslLog.LOGTYPE = "INSERT";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = ipAddress.ToString();
            aslLog.LOGLTUDE = trip.INSLTUDE;
            aslLog.TABLEID = "TMS_TRIP";

            String vechicleNo = "";
            var findCostpoolName = (from m in db.GlCostPoolDbSet where m.COMPID == trip.COMPID && m.COSTPID == trip.COSTPID select new { m.COSTPID, m.COSTPNM }).ToList();
            foreach (var item in findCostpoolName)
            {
                vechicleNo = item.COSTPNM;
            }

            DateTime tDate = Convert.ToDateTime(trip.TRIPDT);
            String TripDate = tDate.ToString("dd-MMM-yyyy");

            String partyName = "";
            Int64 headCD = Convert.ToInt64(trip.COMPID + "103");
            var find_Partyname = (from n in db.GlAcchartDbSet where n.COMPID == trip.COMPID && n.HEADCD == headCD && n.ACCOUNTCD== trip.PARTYID select new { n.ACCOUNTCD, n.ACCOUNTNM }).ToList();
            foreach (var item in find_Partyname)
            {
                partyName = item.ACCOUNTNM.ToString();
            }

            aslLog.LOGDATA = Convert.ToString("Vehicle NO:" + vechicleNo + ",\nTrip Date:" + TripDate + ",\nTrip Month-Year:" + trip.TRIPMY + ",\nRound Trip No:" + trip.RTRIPNO + ",\nTrip-No:" + trip.TRIPNO + ",\nTrip From:" + trip.TRIPFR + ",\nTrip To:" + trip.TRIPTO + ",\nTrip Type: " + trip.TRIPTP + ",\nParty name: " + partyName + ",\nFare: " + trip.AMTFARE + ",\nDemurrage: " + trip.AMTDEMI + ",\nOther Charge: " + trip.AMTOTC + ",\nTotal Receivable: " + trip.AMTTOT + ",\nCollect Destination: " + trip.CDESTN + ".");
            aslLog.USERPC = strHostName;
            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }


        public void Update_TmsTrip_LogData(TMS_TRIP trip)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == trip.COMPID && n.USERID == trip.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(trip.COMPID);
            aslLog.USERID = trip.UPDUSERID;
            aslLog.LOGTYPE = "UPDATE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = ipAddress.ToString();
            aslLog.LOGLTUDE = trip.UPDLTUDE;
            aslLog.TABLEID = "TMS_TRIP";
            String vechicleNo = "";
            var findCostpoolName = (from m in db.GlCostPoolDbSet where m.COMPID == trip.COMPID && m.COSTPID == trip.COSTPID select new { m.COSTPID, m.COSTPNM }).ToList();
            foreach (var item in findCostpoolName)
            {
                vechicleNo = item.COSTPNM;
            }

            DateTime tDate = Convert.ToDateTime(trip.TRIPDT);
            String TripDate = tDate.ToString("dd-MMM-yyyy");

            String partyName = "";
            Int64 headCD = Convert.ToInt64(trip.COMPID + "103");
            var find_Partyname = (from n in db.GlAcchartDbSet where n.COMPID == trip.COMPID && n.HEADCD == headCD && n.ACCOUNTCD == trip.PARTYID select new { n.ACCOUNTCD, n.ACCOUNTNM }).ToList();
            foreach (var item in find_Partyname)
            {
                partyName = item.ACCOUNTNM.ToString();
            }

            aslLog.LOGDATA = Convert.ToString("Vehicle NO:" + vechicleNo + ",\nTrip Date:" + TripDate + ",\nTrip Month-Year:" + trip.TRIPMY + ",\nRound Trip No:" + trip.RTRIPNO + ",\nTrip-No:" + trip.TRIPNO + ",\nTrip From:" + trip.TRIPFR + ",\nTrip To:" + trip.TRIPTO + ",\nTrip Type: " + trip.TRIPTP + ",\nParty name: " + partyName + ",\nFare: " + trip.AMTFARE + ",\nDemurrage: " + trip.AMTDEMI + ",\nOther Charge: " + trip.AMTOTC + ",\nTotal Receivable: " + trip.AMTTOT + ",\nCollect Destination: " + trip.CDESTN + ".");
            aslLog.USERPC = strHostName;
            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }



        public void Delete_TmsTrip_LogData(TMS_TRIP trip)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == trip.COMPID && n.USERID == trip.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(trip.COMPID);
            aslLog.USERID = trip.UPDUSERID;
            aslLog.LOGTYPE = "DELETE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = ipAddress.ToString();
            aslLog.LOGLTUDE = trip.UPDLTUDE;
            aslLog.TABLEID = "TMS_TRIP";
            String vechicleNo = "";
            var findCostpoolName = (from m in db.GlCostPoolDbSet where m.COMPID == trip.COMPID && m.COSTPID == trip.COSTPID select new { m.COSTPID, m.COSTPNM }).ToList();
            foreach (var item in findCostpoolName)
            {
                vechicleNo = item.COSTPNM;
            }

            DateTime tDate = Convert.ToDateTime(trip.TRIPDT);
            String TripDate = tDate.ToString("dd-MMM-yyyy");

            String partyName = "";
            Int64 headCD = Convert.ToInt64(trip.COMPID + "103");
            var find_Partyname = (from n in db.GlAcchartDbSet where n.COMPID == trip.COMPID && n.HEADCD == headCD && n.ACCOUNTCD == trip.PARTYID select new { n.ACCOUNTCD, n.ACCOUNTNM }).ToList();
            foreach (var item in find_Partyname)
            {
                partyName = item.ACCOUNTNM.ToString();
            }

            aslLog.LOGDATA = Convert.ToString("Vehicle NO:" + vechicleNo + ",\nTrip Date:" + TripDate + ",\nTrip Month-Year:" + trip.TRIPMY + ",\nRound Trip No:" + trip.RTRIPNO + ",\nTrip-No:" + trip.TRIPNO + ",\nTrip From:" + trip.TRIPFR + ",\nTrip To:" + trip.TRIPTO + ",\nTrip Type: " + trip.TRIPTP + ",\nParty name: " + partyName + ",\nFare: " + trip.AMTFARE + ",\nDemurrage: " + trip.AMTDEMI + ",\nOther Charge: " + trip.AMTOTC + ",\nTotal Receivable: " + trip.AMTTOT + ",\nCollect Destination: " + trip.CDESTN + ".");
            aslLog.USERPC = strHostName;
            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }




        public ASL_DELETE AslDelete = new ASL_DELETE();
        public void Delete_TmsTrip_ASLDELETE(TMS_TRIP trip)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("HH:mm:ss tt"));
            
            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == trip.COMPID && n.USERID == trip.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                AslDelete.DELSLNO = Convert.ToInt64("1");
            }
            else
            {
                AslDelete.DELSLNO = maxSerialNo + 1;
            }

            AslDelete.COMPID = Convert.ToInt64(trip.COMPID);
            AslDelete.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
            AslDelete.DELSLNO = AslDelete.DELSLNO;
            AslDelete.DELDATE = Convert.ToString(date);
            AslDelete.DELTIME = Convert.ToString(time);
            AslDelete.DELIPNO = ipAddress.ToString();
            AslDelete.DELLTUDE = trip.UPDLTUDE;
            AslDelete.TABLEID = "TMS_TRIP";

            String vechicleNo = "";
            var findCostpoolName = (from m in db.GlCostPoolDbSet where m.COMPID == trip.COMPID && m.COSTPID == trip.COSTPID select new { m.COSTPID, m.COSTPNM }).ToList();
            foreach (var item in findCostpoolName)
            {
                vechicleNo = item.COSTPNM;
            }

            DateTime tDate = Convert.ToDateTime(trip.TRIPDT);
            String TripDate = tDate.ToString("dd-MMM-yyyy");

            String partyName = "";
            Int64 headCD = Convert.ToInt64(trip.COMPID + "103");
            var find_Partyname = (from n in db.GlAcchartDbSet where n.COMPID == trip.COMPID && n.HEADCD == headCD && n.ACCOUNTCD == trip.PARTYID select new { n.ACCOUNTCD, n.ACCOUNTNM }).ToList();
            foreach (var item in find_Partyname)
            {
                partyName = item.ACCOUNTNM.ToString();
            }

            AslDelete.DELDATA = Convert.ToString("Vehicle NO:" + vechicleNo + ",\nTrip Date:" + TripDate + ",\nTrip Month-Year:" + trip.TRIPMY + ",\nRound Trip No:" + trip.RTRIPNO + ",\nTrip-No:" + trip.TRIPNO + ",\nTrip From:" + trip.TRIPFR + ",\nTrip To:" + trip.TRIPTO + ",\nTrip Type: " + trip.TRIPTP + ",\nParty name: " + partyName + ",\nFare: " + trip.AMTFARE + ",\nDemurrage: " + trip.AMTDEMI + ",\nOther Charge: " + trip.AMTOTC + ",\nTotal Receivable: " + trip.AMTTOT + ",\nCollect Destination: " + trip.CDESTN + ".");
            AslDelete.USERPC = strHostName;
            db.AslDeleteDbSet.Add(AslDelete);
            db.SaveChanges();
        }




        // GET: /TmsTrip/
        [AcceptVerbs("GET")]
        [ActionName("Index")]
        public ActionResult Index()
        {
            var dt = (PageModel)TempData["TripModel"];
            return View(dt);
        }




        [AcceptVerbs("POST")]
        [ActionName("Index")]
        public ActionResult IndexPost(PageModel model, string command)
        {
            if (model.TripDate == null || model.TmsTripmst.TRIPMY == null)
            {
                TempData["tripmasterSuccessMessage"] = "** Please input valid Data ";
                TempData["TripModel"] = model;
                return RedirectToAction("Index");
            }
            if (command == "Submit")
            {
                model.TmsTripmst.TRIPDT = Convert.ToDateTime(model.TripDate);
                model.TmsTripmst.USERPC = strHostName;
                model.TmsTripmst.INSIPNO = ipAddress.ToString();
                model.TmsTripmst.INSTIME = Convert.ToDateTime(td);
                model.TmsTripmst.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                var check_tripMaster = (from m in db.TmsTripmstDbSet
                                        where m.COMPID == model.TmsTripmst.COMPID && m.COSTPID == model.TmsTripmst.COSTPID && m.TRIPMY == model.TmsTripmst.TRIPMY
                                        select m).ToList();

                DateTime convert_dateTime = Convert.ToDateTime(model.TripDate);
                string CurrentYear = convert_dateTime.ToString("yy");
                string CurrentMonth = convert_dateTime.ToString("MM");
                string CurrentmonthYear = CurrentYear + "" + CurrentMonth;
                if (check_tripMaster.Count == 0)
                {
                    model.TmsTripmst.RTRIPNO = Convert.ToInt64(CurrentmonthYear + "01");
                }
                else
                {
                    Int64 max_RTRIPNO = Convert.ToInt64((from m in db.TmsTripmstDbSet
                                                         where m.COMPID == model.TmsTripmst.COMPID && m.COSTPID == model.TmsTripmst.COSTPID && m.TRIPMY == model.TmsTripmst.TRIPMY
                                                         select m.RTRIPNO).Max());
                    Int64 R = Convert.ToInt64(CurrentmonthYear + "99");
                    if (max_RTRIPNO <= R)
                    {
                        model.TmsTripmst.RTRIPNO = Convert.ToInt64(max_RTRIPNO + 1);
                    }
                    else
                    {
                        model.TmsTripmst.RTRIPNO = 0;
                        TempData["tripmasterSuccessMessage"] = "Maximum TripNo id blocked!!!!!";
                        TempData["SubmitButton"] = "Hide";
                        TempData["TripModel"] = model;
                        return RedirectToAction("Index");
                    }
                }
                db.TmsTripmstDbSet.Add(model.TmsTripmst);
                db.SaveChanges();
                Insert_TmsTripMaster_LogData(model.TmsTripmst);

                TempData["tripmasterSuccessMessage"] = "Successfully saved Round-Trip No: " + model.TmsTripmst.RTRIPNO + ".Please Create the Trip List.";
                TempData["Costpid"] = model.TmsTripmst.COSTPID;
                TempData["tripMonthYear"] = model.TmsTripmst.TRIPMY;
                TempData["R_TripNo"] = model.TmsTripmst.RTRIPNO;
                TempData["ShowAddButton"] = "Show add button";
                TempData["SubmitButton"] = "Hide";
                TempData["TripModel"] = model;

                TempData["latitute_delete"] = model.TmsTripmst.INSLTUDE;
                return RedirectToAction("Index");

            }
            if (command == "Add")
            {
                model.TmsTrip.COMPID = model.TmsTripmst.COMPID;
                model.TmsTrip.COSTPID = model.TmsTripmst.COSTPID;
                model.TmsTrip.TRIPDT = Convert.ToDateTime(model.TripChildDate);
                model.TmsTrip.TRIPMY = model.TmsTripmst.TRIPMY;
                model.TmsTrip.RTRIPNO = model.TmsTripmst.RTRIPNO;

                model.TmsTrip.USERPC = strHostName;
                model.TmsTrip.INSIPNO = ipAddress.ToString();
                model.TmsTrip.INSTIME = td;
                model.TmsTrip.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                model.TmsTrip.INSLTUDE = model.TmsTripmst.INSLTUDE;

                Int64 maxData;
                try
                {
                    maxData = Convert.ToInt64((from n in db.TmsTripDbSet
                                               where n.COMPID == model.TmsTrip.COMPID && n.COSTPID == model.TmsTrip.COSTPID
&& n.TRIPMY == model.TmsTrip.TRIPMY && n.RTRIPNO == model.TmsTrip.RTRIPNO
                                               select n.TRIPNO).Max());
                }
                catch
                {
                    maxData = 0;
                }

                Int64 R = Convert.ToInt64(model.TmsTrip.RTRIPNO + "99");
                if (maxData == 0)
                {
                    model.TmsTrip.TRIPNO = Convert.ToInt64(model.TmsTrip.RTRIPNO + "01");
                }
                else if (maxData <= R)
                {
                    model.TmsTrip.TRIPNO = Convert.ToInt64(maxData + 1);
                }
                else
                {
                    model.TmsTrip.TRIPNO = 0;
                    TempData["tripSuccessMessage"] = "Maximum TripNo id blocked!!!!!";
                    TempData["SubmitButton"] = "Hide";
                    TempData["TripModel"] = model;
                    return RedirectToAction("Index");
                }

                db.TmsTripDbSet.Add(model.TmsTrip);
                db.SaveChanges();
                Insert_TmsTrip_LogData(model.TmsTrip);

                TempData["tripSuccessMessage"] = "Successfully saved TripNo: " + model.TmsTrip.TRIPNO + ".";
                TempData["Costpid"] = model.TmsTripmst.COSTPID;
                TempData["tripMonthYear"] = model.TmsTripmst.TRIPMY;
                TempData["R_TripNo"] = model.TmsTripmst.RTRIPNO;
                TempData["ShowAddButton"] = "Show add button";
                TempData["TripModel"] = model;
                TempData["SubmitButton"] = "Hide";
                model.TripChildDate = "";
                model.TmsTrip.TRIPFR = "";
                model.TmsTrip.TRIPTO = "";
                model.TmsTrip.CDESTN = "";
                model.TmsTrip.PARTYID = 0;
                model.TmsTrip.AMTFARE = 0;
                model.TmsTrip.AMTDEMI = 0;
                model.TmsTrip.AMTOTC = 0;
                model.TmsTrip.AMTTOT = 0;
                model.TmsTrip.REMARKS = "";
                return RedirectToAction("Index");
            }
            if (command == "Update")
            {
                var find_gridLevel_TmsTripData =
                    from a in db.TmsTripDbSet
                    where (a.COMPID == model.TmsTripmst.COMPID && a.COSTPID == model.TmsTripmst.COSTPID && a.TRIPMY == model.TmsTripmst.TRIPMY && a.RTRIPNO == model.TmsTripmst.RTRIPNO && a.TRIPNO == model.TmsTrip.TRIPNO)
                    select a;

                TMS_TRIP logData_tripModel = new TMS_TRIP();

                foreach (TMS_TRIP a in find_gridLevel_TmsTripData)
                {
                    // Insert any additional changes to column values.
                    if (model.TripChildDate != null)
                    {
                        a.TRIPDT = Convert.ToDateTime(model.TripChildDate);
                    }
                    a.TRIPFR = model.TmsTrip.TRIPFR;
                    a.TRIPTO = model.TmsTrip.TRIPTO;
                    a.TRIPTP = model.TmsTrip.TRIPTP;
                    a.PARTYID = model.TmsTrip.PARTYID;
                    a.CDESTN = model.TmsTrip.CDESTN;
                    a.AMTFARE = model.TmsTrip.AMTFARE;
                    a.AMTDEMI = model.TmsTrip.AMTDEMI;
                    a.AMTOTC = model.TmsTrip.AMTOTC;
                    a.AMTTOT = model.TmsTrip.AMTTOT;
                    a.REMARKS = model.TmsTrip.REMARKS;


                    a.UPDIPNO = ipAddress.ToString();
                    a.UPDTIME = td;
                    a.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    a.UPDLTUDE = model.TmsTripmst.INSLTUDE;

                    logData_tripModel = a;
                }
                db.SaveChanges();
                Update_TmsTrip_LogData(logData_tripModel);

                TempData["tripSuccessMessage"] = "Successfully updated Trip-No: " + model.TmsTrip.TRIPNO + ".";
                TempData["Costpid"] = model.TmsTripmst.COSTPID;
                TempData["tripMonthYear"] = model.TmsTripmst.TRIPMY;
                TempData["R_TripNo"] = model.TmsTripmst.RTRIPNO;
                TempData["ShowAddButton"] = "Show add button";
                TempData["TripModel"] = model;
                TempData["SubmitButton"] = "Hide";
                model.TripChildDate = "";
                model.TmsTrip.TRIPFR = "";
                model.TmsTrip.TRIPTO = "";
                model.TmsTrip.CDESTN = "";
                model.TmsTrip.PARTYID = 0;
                model.TmsTrip.AMTFARE = 0;
                model.TmsTrip.AMTDEMI = 0;
                model.TmsTrip.AMTOTC = 0;
                model.TmsTrip.AMTTOT = 0;
                model.TmsTrip.REMARKS = "";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }







        public ActionResult UpdateTrip_Create(Int64 id, Int64 cid, Int64 Costpid, String tripdate, String tripMonth, Int64 r_tripNo, Int64 tripno, PageModel model)
        {
            model.TmsTripmst.COSTPID = Costpid;
            DateTime TRPdate = Convert.ToDateTime(tripdate);
            model.TripDate = TRPdate.ToString("dd-MMM-yyyy");
            model.TmsTripmst.TRIPMY = tripMonth;
            model.TmsTripmst.RTRIPNO = r_tripNo;
            var findMasterData = (from m in db.TmsTripmstDbSet
                                  where m.COMPID == cid && m.COSTPID == model.TmsTripmst.COSTPID && m.TRIPMY == model.TmsTripmst.TRIPMY &&
       m.RTRIPNO == model.TmsTripmst.RTRIPNO
                                  select m).ToList();
            foreach (var getData in findMasterData)
            {
                model.TmsTripmst.QTYFUEL = getData.QTYFUEL;
                model.TmsTripmst.QTYMOBIL = getData.QTYMOBIL;
                model.TmsTripmst.REMARKS = getData.REMARKS;
            }

            TempData["Costpid"] = model.TmsTripmst.COSTPID;
            TempData["tripMonthYear"] = model.TmsTripmst.TRIPMY;
            TempData["R_TripNo"] = model.TmsTripmst.RTRIPNO;
            TempData["ShowUpdateButton"] = "Show Update button";
            TempData["SubmitButton"] = "Hide";
            TempData["TripModel"] = model;

            model.TmsTrip.ID = id;
            model.TmsTrip.COMPID = cid;
            model.TmsTrip.COSTPID = Costpid;
            model.TmsTrip.TRIPMY = tripMonth;
            model.TmsTrip.RTRIPNO = r_tripNo;
            model.TmsTrip.TRIPNO = tripno;
            model.TmsTrip = db.TmsTripDbSet.Find(model.TmsTrip.ID, model.TmsTrip.COMPID, model.TmsTrip.COSTPID, model.TmsTrip.TRIPMY, model.TmsTrip.RTRIPNO, model.TmsTrip.TRIPNO);
            DateTime griddate = Convert.ToDateTime(model.TmsTrip.TRIPDT);
            model.TripChildDate = griddate.ToString("dd-MMM-yyyy");
            return RedirectToAction("Index");

        }



        public ActionResult DeleteTrip_Create(Int64 id, Int64 cid, Int64 Costpid, String tripdate, String tripMonth, Int64 r_tripNo, Int64 tripno, PageModel model)
        {
            model.TmsTripmst.COSTPID = Costpid;
            DateTime TRPdate = Convert.ToDateTime(tripdate);
            model.TripDate = TRPdate.ToString("dd-MMM-yyyy");
            model.TmsTripmst.TRIPMY = tripMonth;
            model.TmsTripmst.RTRIPNO = r_tripNo;
            var findMasterData = (from m in db.TmsTripmstDbSet
                                  where
m.COMPID == cid && m.COSTPID == model.TmsTripmst.COSTPID && m.TRIPMY == model.TmsTripmst.TRIPMY &&
m.RTRIPNO == model.TmsTripmst.RTRIPNO
                                  select m).ToList();
            foreach (var getData in findMasterData)
            {
                model.TmsTripmst.QTYFUEL = getData.QTYFUEL;
                model.TmsTripmst.QTYMOBIL = getData.QTYMOBIL;
                model.TmsTripmst.REMARKS = getData.REMARKS;
            }

            TempData["Costpid"] = model.TmsTripmst.COSTPID;
            TempData["tripMonthYear"] = model.TmsTripmst.TRIPMY;
            TempData["R_TripNo"] = model.TmsTripmst.RTRIPNO;
            TempData["ShowAddButton"] = "Show Add button";
            TempData["SubmitButton"] = "Hide";
            TempData["TripModel"] = model;

            model.TmsTrip.ID = id;
            model.TmsTrip.COMPID = cid;
            model.TmsTrip.COSTPID = Costpid;
            model.TmsTrip.TRIPMY = tripMonth;
            model.TmsTrip.RTRIPNO = r_tripNo;
            model.TmsTrip.TRIPNO = tripno;

            TMS_TRIP tmsTripChild = db.TmsTripDbSet.Find(model.TmsTrip.ID, model.TmsTrip.COMPID, model.TmsTrip.COSTPID, model.TmsTrip.TRIPMY, model.TmsTrip.RTRIPNO, model.TmsTrip.TRIPNO);
            tmsTripChild.USERPC = strHostName;
            tmsTripChild.UPDIPNO = ipAddress.ToString();
            tmsTripChild.UPDTIME = Convert.ToDateTime(td);
            tmsTripChild.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

            if (TempData["latitute_delete"] != null)
            {
                //Get current LOGLTUDE data 
                tmsTripChild.UPDLTUDE = TempData["latitute_delete"].ToString();
            }

            TMS_TRIP logData_tripModel = new TMS_TRIP();
            logData_tripModel = tmsTripChild;

            db.TmsTripDbSet.Remove(tmsTripChild);
            db.SaveChanges();
            Delete_TmsTrip_LogData(logData_tripModel);
            Delete_TmsTrip_ASLDELETE(logData_tripModel);


            model.TripChildDate = "";
            model.TmsTrip.TRIPFR = "";
            model.TmsTrip.TRIPTO = "";
            model.TmsTrip.CDESTN = "";
            model.TmsTrip.PARTYID = 0;
            model.TmsTrip.AMTFARE = 0;
            model.TmsTrip.AMTDEMI = 0;
            model.TmsTrip.AMTOTC = 0;
            model.TmsTrip.AMTTOT = 0;
            model.TmsTrip.REMARKS = "";
            TempData["tripSuccessMessage"] = "Successfully deleted Trip-No: " + tripno + ".";
            return RedirectToAction("Index");
        }








        [AcceptVerbs("GET")]
        [ActionName("Update")]
        public ActionResult Update()
        {
            var dt = (PageModel)TempData["TripModel"];
            return View(dt);
        }




        [AcceptVerbs("POST")]
        [ActionName("Update")]
        public ActionResult UpdatePost(PageModel model, string command)
        {
            if (model.TripDate == null || model.TmsTripmst.TRIPMY == null)
            {
                TempData["tripmasterSuccessMessage"] = "** Please input valid Data ";
                TempData["TripModel"] = model;
                return RedirectToAction("Update");
            }
            if (command == "Search")
            {
                model.TmsTripmst.TRIPDT = Convert.ToDateTime(model.TripDate);


                var findGetData = (from m in db.TmsTripmstDbSet where m.COMPID == model.TmsTripmst.COMPID && m.COSTPID == model.TmsTripmst.COSTPID && m.TRIPMY == model.TmsTripmst.TRIPMY && m.RTRIPNO == model.TmsTripmst.RTRIPNO select new { m.REMARKS, m.QTYFUEL, m.QTYMOBIL }).ToList();
                foreach (var a in findGetData)
                {
                    model.TmsTripmst.REMARKS = a.REMARKS;
                    model.TmsTripmst.QTYFUEL = Convert.ToDecimal(a.QTYFUEL.ToString());
                    model.TmsTripmst.QTYMOBIL = Convert.ToDecimal(a.QTYMOBIL.ToString());
                    break;
                }

                TempData["tripmasterSuccessMessage"] = "Get the Trip List.";
                TempData["Costpid"] = model.TmsTripmst.COSTPID;
                TempData["tripMonthYear"] = model.TmsTripmst.TRIPMY;
                TempData["R_TripNo"] = model.TmsTripmst.RTRIPNO;
                TempData["ShowAddButton"] = "Show add button";
                TempData["SubmitButton"] = "Hide";
                TempData["TripModel"] = model;

                TempData["latitute_delete"] = model.TmsTripmst.UPDLTUDE;
                return RedirectToAction("Update");

            }
            if (command == "Add")
            {
                model.TmsTrip.COMPID = model.TmsTripmst.COMPID;
                model.TmsTrip.COSTPID = model.TmsTripmst.COSTPID;
                model.TmsTrip.TRIPDT = Convert.ToDateTime(model.TripChildDate);
                model.TmsTrip.TRIPMY = model.TmsTripmst.TRIPMY;
                model.TmsTrip.RTRIPNO = model.TmsTripmst.RTRIPNO;

                model.TmsTrip.USERPC = strHostName;
                model.TmsTrip.INSIPNO = ipAddress.ToString();
                model.TmsTrip.INSTIME = td;
                model.TmsTrip.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                model.TmsTrip.INSLTUDE = model.TmsTripmst.UPDLTUDE;

                Int64 maxData;
                try
                {
                    maxData = Convert.ToInt64((from n in db.TmsTripDbSet
                                               where n.COMPID == model.TmsTrip.COMPID && n.COSTPID == model.TmsTrip.COSTPID
&& n.TRIPMY == model.TmsTrip.TRIPMY && n.RTRIPNO == model.TmsTrip.RTRIPNO
                                               select n.TRIPNO).Max());
                }
                catch
                {
                    maxData = 0;
                }

                Int64 R = Convert.ToInt64(model.TmsTrip.RTRIPNO + "99");
                if (maxData == 0)
                {
                    model.TmsTrip.TRIPNO = Convert.ToInt64(model.TmsTrip.RTRIPNO + "01");
                }
                else if (maxData <= R)
                {
                    model.TmsTrip.TRIPNO = Convert.ToInt64(maxData + 1);
                }
                else
                {
                    model.TmsTrip.TRIPNO = 0;
                    TempData["tripSuccessMessage"] = "Maximum TripNo id blocked!!!!!";
                    TempData["SubmitButton"] = "Hide";
                    TempData["TripModel"] = model;
                    return RedirectToAction("Update");
                }

                db.TmsTripDbSet.Add(model.TmsTrip);
                db.SaveChanges();
                Insert_TmsTrip_LogData(model.TmsTrip);

                TempData["tripSuccessMessage"] = "Successfully saved Trip No: " + model.TmsTrip.TRIPNO + ".";
                TempData["Costpid"] = model.TmsTripmst.COSTPID;
                TempData["tripMonthYear"] = model.TmsTripmst.TRIPMY;
                TempData["R_TripNo"] = model.TmsTripmst.RTRIPNO;
                TempData["ShowAddButton"] = "Show add button";
                TempData["TripModel"] = model;
                TempData["SubmitButton"] = "Hide";
                model.TripChildDate = "";
                model.TmsTrip.TRIPFR = "";
                model.TmsTrip.TRIPTO = "";
                model.TmsTrip.CDESTN = "";
                model.TmsTrip.PARTYID = 0;
                model.TmsTrip.AMTFARE = 0;
                model.TmsTrip.AMTDEMI = 0;
                model.TmsTrip.AMTOTC = 0;
                model.TmsTrip.AMTTOT = 0;
                model.TmsTrip.REMARKS = "";
                return RedirectToAction("Update");
            }
            if (command == "Update")
            {
                var find_gridLevel_TmsTripData =
                    from a in db.TmsTripDbSet
                    where (a.COMPID == model.TmsTripmst.COMPID && a.COSTPID == model.TmsTripmst.COSTPID && a.TRIPMY == model.TmsTripmst.TRIPMY && a.RTRIPNO == model.TmsTripmst.RTRIPNO && a.TRIPNO == model.TmsTrip.TRIPNO)
                    select a;

                TMS_TRIP logData_tripModel = new TMS_TRIP();

                foreach (TMS_TRIP a in find_gridLevel_TmsTripData)
                {
                    // Insert any additional changes to column values.
                    if (model.TripChildDate != null)
                    {
                        a.TRIPDT = Convert.ToDateTime(model.TripChildDate);
                    }
                    a.TRIPFR = model.TmsTrip.TRIPFR;
                    a.TRIPTO = model.TmsTrip.TRIPTO;
                    a.TRIPTP = model.TmsTrip.TRIPTP;
                    a.PARTYID = model.TmsTrip.PARTYID;
                    a.CDESTN = model.TmsTrip.CDESTN;
                    a.AMTFARE = model.TmsTrip.AMTFARE;
                    a.AMTDEMI = model.TmsTrip.AMTDEMI;
                    a.AMTOTC = model.TmsTrip.AMTOTC;
                    a.AMTTOT = model.TmsTrip.AMTTOT;
                    a.REMARKS = model.TmsTrip.REMARKS;


                    a.UPDIPNO = ipAddress.ToString();
                    a.UPDTIME = td;
                    a.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    a.UPDLTUDE = model.TmsTripmst.UPDLTUDE;

                    logData_tripModel = a;
                }
                db.SaveChanges();
                Update_TmsTrip_LogData(logData_tripModel);

                TempData["tripSuccessMessage"] = "Successfully updated Trip No: " + model.TmsTrip.TRIPNO + ".";
                TempData["Costpid"] = model.TmsTripmst.COSTPID;
                TempData["tripMonthYear"] = model.TmsTripmst.TRIPMY;
                TempData["R_TripNo"] = model.TmsTripmst.RTRIPNO;
                TempData["ShowAddButton"] = "Show add button";
                TempData["TripModel"] = model;
                TempData["SubmitButton"] = "Hide";
                model.TripChildDate = "";
                model.TmsTrip.TRIPFR = "";
                model.TmsTrip.TRIPTO = "";
                model.TmsTrip.CDESTN = "";
                model.TmsTrip.PARTYID = 0;
                model.TmsTrip.AMTFARE = 0;
                model.TmsTrip.AMTDEMI = 0;
                model.TmsTrip.AMTOTC = 0;
                model.TmsTrip.AMTTOT = 0;
                model.TmsTrip.REMARKS = "";
                return RedirectToAction("Update");
            }
            return RedirectToAction("Update");
        }





        public ActionResult UpdateTrip_update(Int64 id, Int64 cid, Int64 Costpid, String tripdate, String tripMonth, Int64 r_tripNo, Int64 tripno, PageModel model)
        {
            //.........................................................Create Permission Check
            var LoggedCompId = System.Web.HttpContext.Current.Session["loggedCompID"].ToString();
            var loggedUserID = System.Web.HttpContext.Current.Session["loggedUserID"].ToString();

            var updateStatus = "";

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CnfDbContext"].ToString());
            string query1 = string.Format("SELECT STATUS, INSERTR, UPDATER, DELETER FROM ASL_ROLE WHERE  CONTROLLERNAME='TmsTrip' AND COMPID='{0}' AND USERID = '{1}'", LoggedCompId, loggedUserID);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query1, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            foreach (DataRow row in ds.Rows)
            {
                updateStatus = row["UPDATER"].ToString();
            }
            conn.Close();

            model.TmsTripmst.COSTPID = Costpid;
            DateTime TRPdate = Convert.ToDateTime(tripdate);
            model.TripDate = TRPdate.ToString("dd-MMM-yyyy");
            model.TmsTripmst.TRIPMY = tripMonth;
            model.TmsTripmst.RTRIPNO = r_tripNo;
            var findMasterData = (from m in db.TmsTripmstDbSet
                                  where m.COMPID == cid && m.COSTPID == model.TmsTripmst.COSTPID && m.TRIPMY == model.TmsTripmst.TRIPMY &&
                                  m.RTRIPNO == model.TmsTripmst.RTRIPNO
                                  select m).ToList();
            foreach (var getData in findMasterData)
            {
                model.TmsTripmst.QTYFUEL = getData.QTYFUEL;
                model.TmsTripmst.QTYMOBIL = getData.QTYMOBIL;
                model.TmsTripmst.REMARKS = getData.REMARKS;
            }

            TempData["Costpid"] = model.TmsTripmst.COSTPID;
            TempData["tripMonthYear"] = model.TmsTripmst.TRIPMY;
            TempData["R_TripNo"] = model.TmsTripmst.RTRIPNO;
            TempData["ShowUpdateButton"] = "Show Update button";
            TempData["SubmitButton"] = "Hide";
            TempData["TripModel"] = model;

            if (updateStatus == 'I'.ToString())
            {
                TempData["tripSuccessMessage"] = "Permission not granted!";
                return RedirectToAction("Update");
            }
            //...............................................................................................
            model.TmsTrip.ID = id;
            model.TmsTrip.COMPID = cid;
            model.TmsTrip.COSTPID = Costpid;
            model.TmsTrip.TRIPMY = tripMonth;
            model.TmsTrip.RTRIPNO = r_tripNo;
            model.TmsTrip.TRIPNO = tripno;
            model.TmsTrip = db.TmsTripDbSet.Find(model.TmsTrip.ID, model.TmsTrip.COMPID, model.TmsTrip.COSTPID, model.TmsTrip.TRIPMY, model.TmsTrip.RTRIPNO, model.TmsTrip.TRIPNO);
            DateTime griddate = Convert.ToDateTime(model.TmsTrip.TRIPDT);
            model.TripChildDate = griddate.ToString("dd-MMM-yyyy");

            return RedirectToAction("Update");

        }



        public ActionResult DeleteTrip__update(Int64 id, Int64 cid, Int64 Costpid, String tripdate, String tripMonth, Int64 r_tripNo, Int64 tripno, PageModel model)
        {
            //.........................................................Create Permission Check
            var LoggedCompId = System.Web.HttpContext.Current.Session["loggedCompID"].ToString();
            var loggedUserID = System.Web.HttpContext.Current.Session["loggedUserID"].ToString();

            var deleteStatus = "";

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CnfDbContext"].ToString());
            string query = string.Format("SELECT STATUS, INSERTR, UPDATER, DELETER FROM ASL_ROLE WHERE  CONTROLLERNAME='TmsTrip' AND COMPID='{0}' AND USERID = '{1}'", LoggedCompId, loggedUserID);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            foreach (DataRow row in ds.Rows)
            {
                deleteStatus = row["DELETER"].ToString();
            }
            conn.Close();


            model.TmsTripmst.COSTPID = Costpid;
            DateTime TRPdate = Convert.ToDateTime(tripdate);
            model.TripDate = TRPdate.ToString("dd-MMM-yyyy");
            model.TmsTripmst.TRIPMY = tripMonth;
            model.TmsTripmst.RTRIPNO = r_tripNo;
            var findMasterData = (from m in db.TmsTripmstDbSet
                                  where
m.COMPID == cid && m.COSTPID == model.TmsTripmst.COSTPID && m.TRIPMY == model.TmsTripmst.TRIPMY &&
m.RTRIPNO == model.TmsTripmst.RTRIPNO
                                  select m).ToList();
            foreach (var getData in findMasterData)
            {
                model.TmsTripmst.QTYFUEL = getData.QTYFUEL;
                model.TmsTripmst.QTYMOBIL = getData.QTYMOBIL;
                model.TmsTripmst.REMARKS = getData.REMARKS;
            }

            TempData["Costpid"] = model.TmsTripmst.COSTPID;
            TempData["tripMonthYear"] = model.TmsTripmst.TRIPMY;
            TempData["R_TripNo"] = model.TmsTripmst.RTRIPNO;
            TempData["ShowAddButton"] = "Show Add button";
            TempData["SubmitButton"] = "Hide";
            TempData["TripModel"] = model;

            if (deleteStatus == 'I'.ToString())
            {
                model.TripChildDate = "";
                model.TmsTrip.TRIPFR = "";
                model.TmsTrip.TRIPTO = "";
                model.TmsTrip.CDESTN = "";
                model.TmsTrip.PARTYID = 0;
                model.TmsTrip.AMTFARE = 0;
                model.TmsTrip.AMTDEMI = 0;
                model.TmsTrip.AMTOTC = 0;
                model.TmsTrip.AMTTOT = 0;
                model.TmsTrip.REMARKS = "";
                TempData["tripSuccessMessage"] = "Update Permission not granted!";
                return RedirectToAction("Update");

            }
            //...............................................................................................
            model.TmsTrip.ID = id;
            model.TmsTrip.COMPID = cid;
            model.TmsTrip.COSTPID = Costpid;
            model.TmsTrip.TRIPMY = tripMonth;
            model.TmsTrip.RTRIPNO = r_tripNo;
            model.TmsTrip.TRIPNO = tripno;

            TMS_TRIP tmsTripChild = db.TmsTripDbSet.Find(model.TmsTrip.ID, model.TmsTrip.COMPID, model.TmsTrip.COSTPID, model.TmsTrip.TRIPMY, model.TmsTrip.RTRIPNO, model.TmsTrip.TRIPNO);
            tmsTripChild.USERPC = strHostName;
            tmsTripChild.UPDIPNO = ipAddress.ToString();
            tmsTripChild.UPDTIME = Convert.ToDateTime(td);
            tmsTripChild.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

            if (TempData["latitute_delete"] != null)
            {
                //Get current LOGLTUDE data 
                tmsTripChild.UPDLTUDE = TempData["latitute_delete"].ToString();
            }

            TMS_TRIP logData_tripModel = new TMS_TRIP();
            logData_tripModel = tmsTripChild;

            db.TmsTripDbSet.Remove(tmsTripChild);
            db.SaveChanges();

            Delete_TmsTrip_LogData(logData_tripModel);
            Delete_TmsTrip_ASLDELETE(logData_tripModel);

            TempData["tripSuccessMessage"] = "Successfully deleted Trip-NO: " + tripno + ".";
            model.TripChildDate = "";
            model.TmsTrip.TRIPFR = "";
            model.TmsTrip.TRIPTO = "";
            model.TmsTrip.CDESTN = "";
            model.TmsTrip.PARTYID = 0;
            model.TmsTrip.AMTFARE = 0;
            model.TmsTrip.AMTDEMI = 0;
            model.TmsTrip.AMTOTC = 0;
            model.TmsTrip.AMTTOT = 0;
            model.TmsTrip.REMARKS = "";
            return RedirectToAction("Update");
        }
















        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetMonthYear(String dateText)
        {
            string converttoString = Convert.ToString(dateText);
            string getYear = converttoString.Substring(9, 2);
            string getMonth = converttoString.Substring(3, 3).ToUpper();
            string monthYear = getMonth + "-" + getYear;//DEC-15 (Example)
            var result = new { myear = monthYear };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult R_TripIDload(string compid, string theDate, string monthyear, string cpid)
        {
            Int64 comid = Convert.ToInt64(compid);
            DateTime dt = Convert.ToDateTime(theDate);

            String myear = dt.ToString("MMM-yy").ToUpper();
            Int64 Costpid = Convert.ToInt64(cpid);

            List<SelectListItem> rtripidList = new List<SelectListItem>();
            var list = (from n in db.TmsTripmstDbSet
                        where n.COMPID == comid && n.COSTPID == Costpid && n.TRIPMY == myear && n.TRIPDT == dt
                        select new
                        {
                            n.RTRIPNO
                        }
                            )
                            .Distinct()
                            .ToList();

            if (list.Count != 0)
            {
                foreach (var f in list)
                {
                    rtripidList.Add(new SelectListItem { Text = f.RTRIPNO.ToString(), Value = f.RTRIPNO.ToString() });
                }
            }
            else
            {
                rtripidList.Add(new SelectListItem { Text = null, Value = null });
            }
            return Json(new SelectList(rtripidList, "Value", "Text"));
        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
