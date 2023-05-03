using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Cloud_Transport.Models;

namespace Cloud_Transport.DataAccess
{
    public static class Tms_Trip_Process
    {
        public static string process(PageModel model, Int64 compid)
        {

            //Datetime formet
            IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime td;


            CnfDbContext db = new CnfDbContext();
            //Get Ip ADDRESS,Time & user PC Name
            string strHostName;
            IPHostEntry ipHostInfo;
            IPAddress ipAddress;

            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);

            string date = Convert.ToString(model.AGlMaster.TRANSDT);
            DateTime MyDateTime = DateTime.Parse(date);
            string converttoString = MyDateTime.ToString("dd-MMM-yyyy");
            string getYear = converttoString.Substring(9, 2);
            string getMonth = converttoString.Substring(3, 3);
            string Month_Year = getMonth + "-" + getYear;


            var forRemovedata = (from l in db.GlMasterDbSet
                                 where l.COMPID == compid && l.TRANSDT == model.AGlMaster.TRANSDT
                                 select l).ToList();
            foreach (var VARIABLE in forRemovedata)
            {
                if (VARIABLE.TABLEID == "TMS_TRIP" && VARIABLE.TRANSDT == model.AGlMaster.TRANSDT)
                {
                    db.GlMasterDbSet.Remove(VARIABLE);
                }
            }
            db.SaveChanges();
            

            //GL Process for TMS_TRIP
            var checkDate_TMS_TRIP = (from n in db.TmsTripDbSet where n.TRIPDT == model.AGlMaster.TRANSDT && n.COMPID == compid select n).OrderBy(x => x.TRIPNO).ToList();

            if (checkDate_TMS_TRIP.Count != 0)
            {
                Int64 max_TMS_TRIP = Convert.ToInt64((from a in db.GlMasterDbSet
                                                         where a.COMPID == compid && a.TRANSTP == "JOUR" && a.TABLEID == "TMS_TRIP"
                                                      select a.TRANSSL).Max());
                if (max_TMS_TRIP == 0)
                {
                    max_TMS_TRIP = 95000;
                }
                foreach (var get in checkDate_TMS_TRIP)
                {
                    //maximum TransNO generate.
                    Int64 max_TransNO = Convert.ToInt64((from a in db.GlMasterDbSet
                                                          where a.COMPID == compid && a.TRANSTP == "JOUR" && a.TABLEID == "TMS_TRIP"
                                                         select a.TRANSNO).Max());
                    if (max_TransNO == 0)
                    {
                        DateTime getDate = Convert.ToDateTime(get.TRIPDT);
                        string cm = getDate.ToString("MM"), cy = getDate.ToString("yyyy"), memoNo = "";
                        memoNo = cy + cm + "0001";
                        max_TransNO = Convert.ToInt64(memoNo);
                    }
                    else
                    {
                        max_TransNO = max_TransNO + 1;
                    }



                    max_TMS_TRIP = max_TMS_TRIP + 1;
                    model.AGlMaster.TRANSSL = max_TMS_TRIP;
                    model.AGlMaster.TRANSDT = get.TRIPDT;
                    model.AGlMaster.COMPID = get.COMPID;
                    model.AGlMaster.TRANSTP = "JOUR";
                    model.AGlMaster.TRANSMY = Month_Year.ToUpper();
                    model.AGlMaster.TRANSNO = max_TransNO;
                    model.AGlMaster.TRIPNO = get.TRIPNO;
                    model.AGlMaster.TRANSFOR = "TRIP";
                    //model.AGlMaster.TRANSMODE = get.TRMODE;
                    model.AGlMaster.COSTPID = get.COSTPID;
                    model.AGlMaster.DEBITCD = Convert.ToInt64(get.PARTYID);
                    model.AGlMaster.CREDITCD = Convert.ToInt64(compid + "3010001");
                    //model.AGlMaster.CHEQUENO = stk_Trans.CHEQUENO;
                    //model.AGlMaster.CHEQUEDT = stk_Trans.CHEQUEDT;

                    String VehicleNo = "";
                    var findVehicleName = (from m in db.GlCostPoolDbSet where m.COMPID == get.COMPID && m.COSTPID == get.COSTPID select new { m.COSTPID, m.COSTPNM }).ToList();
                    foreach (var item in findVehicleName)
                    {
                        VehicleNo = item.COSTPNM;
                        break;
                    }

                    model.AGlMaster.REMARKS = Convert.ToString("Vehicle: " + @VehicleNo + ". Trip: " + get.TRIPNO + " -" + get.REMARKS);
                    model.AGlMaster.TRANSDRCR = "DEBIT";
                    model.AGlMaster.TABLEID = "TMS_TRIP";

                    model.AGlMaster.DEBITAMT = get.AMTTOT;
                    model.AGlMaster.CREDITAMT = 0;

                    model.AGlMaster.USERPC = strHostName;
                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);
                    model.AGlMaster.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                    db.GlMasterDbSet.Add(model.AGlMaster);
                    db.SaveChanges();



                    max_TMS_TRIP = max_TMS_TRIP + 1;
                    model.AGlMaster.TRANSSL = max_TMS_TRIP;
                    model.AGlMaster.TRANSDT = get.TRIPDT;
                    model.AGlMaster.COMPID = get.COMPID;
                    model.AGlMaster.TRANSTP = "JOUR";
                    model.AGlMaster.TRANSMY = Month_Year.ToUpper();
                    model.AGlMaster.TRANSNO = max_TransNO;
                    model.AGlMaster.TRIPNO = get.TRIPNO;
                    model.AGlMaster.TRANSFOR = "TRIP";
                    //model.AGlMaster.TRANSMODE = get.TRMODE;
                    model.AGlMaster.COSTPID = get.COSTPID;
                    model.AGlMaster.DEBITCD = Convert.ToInt64(compid + "3010001");
                    model.AGlMaster.CREDITCD = Convert.ToInt64(get.PARTYID);
                    //model.AGlMaster.CHEQUENO = stk_Trans.CHEQUENO;
                    //model.AGlMaster.CHEQUEDT = stk_Trans.CHEQUEDT;
                    model.AGlMaster.REMARKS = Convert.ToString("Vehicle: " + @VehicleNo + ". Trip: " + get.TRIPNO + " -" + get.REMARKS);

                    model.AGlMaster.TRANSDRCR = "CREDIT";
                    model.AGlMaster.TABLEID = "TMS_TRIP";

                    model.AGlMaster.DEBITAMT = 0;
                    model.AGlMaster.CREDITAMT = get.AMTTOT;

                    model.AGlMaster.USERPC = strHostName;
                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);
                    model.AGlMaster.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                    db.GlMasterDbSet.Add(model.AGlMaster);
                    db.SaveChanges();


                }
                return "True";
            }
            else
            {
                return "False";
            }

        }
    }
}