using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Cloud_Transport.Models;

namespace Cloud_Transport.DataAccess
{
    public static class Tms_Expense_Process
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
                if (VARIABLE.TABLEID == "TMS_EXPENSE" && VARIABLE.TRANSDT == model.AGlMaster.TRANSDT)
                {
                    db.GlMasterDbSet.Remove(VARIABLE);
                }
            }
            db.SaveChanges();


           

            //GL Process for TMS_EXPENSE
            var checkDate_TMS_EXPENSE = (from n in db.TmsExpenseDbSet where n.TRANSDT == model.AGlMaster.TRANSDT && n.COMPID == compid select n).OrderBy(x => x.TRANSNO).ToList();

            if (checkDate_TMS_EXPENSE.Count != 0)
            {

                Int64 max_TMS_EXPENSE = Convert.ToInt64((from a in db.GlMasterDbSet
                                                         where a.COMPID == compid && a.TRANSTP == "MPAY" && a.TABLEID == "TMS_EXPENSE"
                                                         select a.TRANSSL).Max());
                if (max_TMS_EXPENSE == 0)
                {
                    max_TMS_EXPENSE = 90000;
                }

                foreach (var get in checkDate_TMS_EXPENSE)
                {
                    max_TMS_EXPENSE = max_TMS_EXPENSE + 1;
                    model.AGlMaster.TRANSSL = max_TMS_EXPENSE;
                    model.AGlMaster.TRANSDT = get.TRANSDT;
                    model.AGlMaster.COMPID = get.COMPID;
                    model.AGlMaster.TRANSTP = "MPAY";
                    model.AGlMaster.TRANSMY = Month_Year.ToUpper();
                    model.AGlMaster.TRANSNO = get.TRANSNO;
                    model.AGlMaster.TRIPNO = get.TRIPNO;
                    model.AGlMaster.TRANSFOR = "TRIP";
                    model.AGlMaster.TRANSMODE = "CASH";
                    model.AGlMaster.COSTPID = get.COSTPID;
                    model.AGlMaster.DEBITCD = Convert.ToInt64(get.DEBITCD);
                    model.AGlMaster.CREDITCD = Convert.ToInt64(compid + "1010001");
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
                    model.AGlMaster.TABLEID = "TMS_EXPENSE";

                    model.AGlMaster.DEBITAMT = get.AMOUNT;
                    model.AGlMaster.CREDITAMT = 0;

                    model.AGlMaster.USERPC = strHostName;
                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);
                    model.AGlMaster.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                    db.GlMasterDbSet.Add(model.AGlMaster);
                    db.SaveChanges();


                    
                    max_TMS_EXPENSE = max_TMS_EXPENSE + 1;
                    model.AGlMaster.TRANSSL = max_TMS_EXPENSE;
                    model.AGlMaster.TRANSDT = get.TRANSDT;
                    model.AGlMaster.COMPID = get.COMPID;
                    model.AGlMaster.TRANSTP = "MPAY";
                    model.AGlMaster.TRANSMY = Month_Year.ToUpper();
                    model.AGlMaster.TRANSNO = get.TRANSNO;
                    model.AGlMaster.TRIPNO = get.TRIPNO;
                    model.AGlMaster.TRANSFOR = "TRIP";
                    model.AGlMaster.TRANSMODE = "CASH";
                    model.AGlMaster.COSTPID = get.COSTPID;
                    model.AGlMaster.DEBITCD = Convert.ToInt64(compid + "1010001");
                    model.AGlMaster.CREDITCD = Convert.ToInt64(get.DEBITCD);
                    //model.AGlMaster.CHEQUENO = stk_Trans.CHEQUENO;
                    //model.AGlMaster.CHEQUEDT = stk_Trans.CHEQUEDT;
                    model.AGlMaster.REMARKS = Convert.ToString("Vehicle: " + @VehicleNo + ". Trip: " + get.TRIPNO + " -" + get.REMARKS);

                    model.AGlMaster.TRANSDRCR = "CREDIT";
                    model.AGlMaster.TABLEID = "TMS_EXPENSE";

                    model.AGlMaster.DEBITAMT = 0;
                    model.AGlMaster.CREDITAMT = get.AMOUNT;

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