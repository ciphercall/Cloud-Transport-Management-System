using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cloud_Transport.Models;
using Cloud_Transport.Models.ASL;
using Cloud_Transport.Models.GL;

namespace Cloud_Transport.Controllers.ASL
{
    public class ASL_TABLEController : Controller
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;
        CnfDbContext db = new CnfDbContext();

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public ASL_TABLEController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }



        //create asl_log object
        public ASL_LOG aslLog = new ASL_LOG();
        //public void Insert_ASL_TABLE_LogData(PageModel model)
        //{
        //    TimeZoneInfo timeZoneInfo;
        //    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        //    DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        //    var date = Convert.ToString(PrintDate.ToString("d"));
        //    var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

        //    Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.USERID == model.AslTable.INSUSERID select n.LOGSLNO).Max());

        //    if (maxSerialNo == 0)
        //    {
        //        aslLog.LOGSLNO = Convert.ToInt64("1");
        //    }
        //    else
        //    {
        //        aslLog.LOGSLNO = maxSerialNo + 1;
        //    }

        //    aslLog.COMPID = Convert.ToInt64(model.CNF_EXPMSTModel.COMPID);
        //    aslLog.USERID = model.CNF_EXPMSTModel.INSUSERID;
        //    aslLog.LOGTYPE = "INSERT";
        //    aslLog.LOGSLNO = aslLog.LOGSLNO;
        //    aslLog.LOGDATE = Convert.ToDateTime(date);
        //    aslLog.LOGTIME = Convert.ToString(time);
        //    aslLog.LOGIPNO = model.CNF_EXPMSTModel.INSIPNO;
        //    aslLog.LOGLTUDE = model.CNF_EXPMSTModel.INSLTUDE;
        //    aslLog.TABLEID = "CNF_EXPMST";
        //    aslLog.LOGDATA = Convert.ToString("EXPCID: " + model.CNF_EXPMSTModel.EXPCID + ",\nEXP CName: " + model.CNF_EXPMSTModel.EXPCNM + ".");
        //    aslLog.USERPC = model.CNF_EXPMSTModel.USERPC;
        //    db.AslLogDbSet.Add(aslLog);
        //}

        // GET: /ASL_TableReport/
        [AcceptVerbs("GET")]
        [ActionName("Index")]
        public ActionResult Index()
        {

            var dt = (PageModel)TempData["model"];
            return View(dt);
        }

        [AcceptVerbs("POST")]
        [ActionName("Index")]
        public ActionResult Index(PageModel model, string command)
        {
            if (command == "Submit")
            {
                if (model.AslTable.TABLEID != null && model.AslTable.TABLENAME != null)
                {

                    //Get Ip ADDRESS,Time & user PC Name
                    string strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                    IPAddress ipAddress = ipHostInfo.AddressList[0];


                    model.AslTable.USERPC = strHostName;
                    model.AslTable.INSIPNO = ipAddress.ToString();
                    model.AslTable.INSTIME = Convert.ToDateTime(td);

                    model.AslTable.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);







                    var result = db.AslTableDbSet.Count(d => d.TABLEID == model.AslTable.TABLEID
                                                              && d.TABLENAME == model.AslTable.TABLENAME);


                    if (result == 0)
                    {




                        //.........................................................Create Permission Check

                        var loggedUserID = System.Web.HttpContext.Current.Session["loggedUserID"].ToString();

                        var createStatus = "";

                        //System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CnfDbContext"].ToString());
                        //string query = string.Format("SELECT STATUS, INSERTR, UPDATER, DELETER FROM ASL_ROLE WHERE  CONTROLLERNAME='ExpenseInformation' AND COMPID='{0}' AND USERID = '{1}'", LoggedCompId, loggedUserID);
                        //System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
                        //conn.Open();
                        //SqlDataAdapter da = new SqlDataAdapter(cmd);
                        //DataTable ds = new DataTable();
                        //da.Fill(ds);

                        //foreach (DataRow row in ds.Rows)
                        //{
                        //    createStatus = row["INSERTR"].ToString();
                        //}

                        //conn.Close();

                        //if (createStatus == 'I'.ToString())
                        //{
                        //    TempData["cnfExpenseHead"] = aCnfExpenseHeadModel;
                        //    TempData["ExpCID"] = aCnfExpenseHeadModel.CNF_EXPMSTModel.EXPCID;

                        //    TempData["ShowAddButton"] = "Show Add Button";
                        //    TempData["message"] = "Permission not granted!";
                        //    return RedirectToAction("Index");
                        //}
                        //...............................................................................................


                        //Insert_ASL_TABLE_LogData(model);

                        db.AslTableDbSet.Add(model.AslTable);
                        db.SaveChanges();


                        TempData["message"] = "Table Name: '" + model.AslTable.TABLENAME + "' successfully saved.\n Please Create the Field List.";

                        TempData["ShowAddButton"] = "Show Add Button";
                        TempData["model"] = model;
                        TempData["TableID"] = model.AslTable.TABLEID;


                        return RedirectToAction("Index");

                    }
                    else
                    {
                        TempData["ShowAddButton"] = "Show Add Button";
                        TempData["model"] = model;
                        TempData["TableID"] = model.AslTable.TABLEID;
                    }





                }

            }
            if(command=="Add")
            {
                //Get Ip ADDRESS,Time & user PC Name
                string strHostName = System.Net.Dns.GetHostName();
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];

                model.AslTableField.USERPC = strHostName;
                model.AslTableField.INSIPNO = ipAddress.ToString();
                model.AslTableField.INSTIME = Convert.ToDateTime(td);

                model.AslTableField.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                model.AslTableField.INSLTUDE=model.AslTable.INSLTUDE;

                model.AslTableField.TABLEID = model.AslTable.TABLEID;

                db.AslTableFieldDbSet.Add(model.AslTableField);
                db.SaveChanges();

                TempData["ShowAddButton"] = "Show Add Button";
                TempData["model"] = model;
                TempData["TableID"] = model.AslTable.TABLEID;


                model.AslTableField.FIELDID = "";
                model.AslTableField.FIELDNAME = "";

                return RedirectToAction("Index");
            }
            if(command=="Update")
            {
                //Get Ip ADDRESS,Time & user PC Name
                string strHostName = System.Net.Dns.GetHostName();
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];

                model.AslTableField.USERPC = strHostName;
                model.AslTableField.INSIPNO = ipAddress.ToString();
                model.AslTableField.INSTIME = Convert.ToDateTime(td);

                model.AslTableField.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                model.AslTableField.INSLTUDE = model.AslTable.INSLTUDE;
              
             
               
              

                model.AslTableField.TABLEID = model.AslTable.TABLEID;
                Int64 id= Convert.ToInt64(model.AslTableField.ID);
                var result = (from n in db.AslTableFieldDbSet
                              where n.ID == id
                              select n).ToList();
                foreach(var v in result)
                {
                    model.AslTableField.ID = id;
                    v.TABLEID = model.AslTableField.TABLEID;
                    v.FIELDID = model.AslTableField.FIELDID;
                    v.FIELDNAME = model.AslTableField.FIELDNAME;
                    v.UPDUSERID = model.AslTableField.INSUSERID;
                    v.UPDTIME = model.AslTableField.INSTIME;
                    v.UPDLTUDE = model.AslTableField.INSLTUDE;
                    v.UPDIPNO = ipAddress.ToString();
                }
                model.AslTable.TABLEID = model.AslTableField.TABLEID;


               
                db.SaveChanges();

                TempData["ShowAddButton"] = "Show Add Button";
                TempData["model"] = model;
                TempData["TableID"] = model.AslTable.TABLEID;


                model.AslTableField.FIELDID = "";
                model.AslTableField.FIELDNAME = "";

                return RedirectToAction("Index");


            }
            return RedirectToAction("Index");

        }



        public ActionResult EditTableField(Int64 id, string tableid, string tablename, string fieldid, string fieldname, PageModel model)
        {
            

            //add the data from database to model
            var pid = from m in db.AslTableFieldDbSet where m.ID == id select m;
            foreach (var Result in pid)
            {
              
                model.AslTableField.ID = id;
                model.AslTableField.TABLEID = Result.TABLEID;
                model.AslTableField.FIELDID = Result.FIELDID;
                model.AslTableField.FIELDNAME = Result.FIELDNAME;
               

            }
            model.AslTable.TABLEID = model.AslTableField.TABLEID;
            model.AslTable.TABLENAME = tablename;

            TempData["ShowAddButton"] = null;
            TempData["model"] = model;
            TempData["TableID"] = model.AslTableField.TABLEID;
            return RedirectToAction("Index");

        }


        public ActionResult TableFieldDelete(Int64 id, string tableid, string tablename, string fieldid, string fieldname, PageModel model)
        {


            ASL_TABFIELD pid = db.AslTableFieldDbSet.Find(id);
            model.AslTable.TABLEID = tableid;
            model.AslTable.TABLENAME = tablename;


            db.AslTableFieldDbSet.Remove(pid);
            db.SaveChanges();

            TempData["ShowAddButton"] = "Show add button";
            TempData["model"] = model;
            TempData["TableID"] = model.AslTable.TABLEID;

            return RedirectToAction("Index");

        }


        ////AutoComplete
        //public JsonResult TagSearch(string term)
        //{          

        //    Int64 inuserid=Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
        //    var tags = from p in db.AslTableDbSet
        //               where p.INSUSERID==inuserid
        //               select p.TABLEID;

        //    return this.Json(tags.Where(t => t.StartsWith(term)),
        //               JsonRequestBehavior.AllowGet);
        //}




        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ItemNameChanged(string changedText)
        {
            Int64 inuserid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
            String tname = "";
           
            var rt = db.AslTableDbSet.Where(n => n.TABLEID == changedText &&
                                                         n.INSUSERID == inuserid).Select(n => new
                                                         {
                                                             Tname = n.TABLENAME
                                                             
                                                         });
            foreach (var n in rt)
            {
                tname = Convert.ToString(n.Tname);
                
            }

            var result = new { tablename = tname };
            return Json(result, JsonRequestBehavior.AllowGet);

        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
