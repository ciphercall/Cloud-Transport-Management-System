using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Cloud_Transport.DataAccess;
using Cloud_Transport.Models;
using Cloud_Transport.Models.ASL;

namespace Cloud_Transport.Controllers.GL
{
    public class ProcessController : AppController
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
        public string Process_Done1, Process_Done2, Process_Done3, Process_Done4, Process_Done5, Process_Done6, Process_Done7;
        private Int64 K;


        public ProcessController()
        {
            ViewData["HighLight_Menu_GL_Form"] = "heighlight";
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);

            Process_Done1 = ""; Process_Done2 = ""; Process_Done3 = ""; Process_Done4 = ""; Process_Done5 = ""; Process_Done6 = ""; Process_Done7 = "";
            K = 0;
        }

        public ASL_LOG aslLog = new ASL_LOG();


        public void Insert_Process_LogData(PageModel model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("d"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.AGlMaster.COMPID && n.USERID == model.AGlMaster.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(model.AGlMaster.COMPID);
            aslLog.USERID = model.AGlMaster.INSUSERID;
            aslLog.LOGTYPE = "INSERT";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.AGlMaster.INSIPNO;
            aslLog.LOGLTUDE = model.AGlMaster.INSLTUDE;
            aslLog.TABLEID = "GL_MASTER PROCESS";

            string username = "";
            var UserNameFind = (from n in db.AslUsercoDbSet where n.USERID == aslLog.USERID select n).ToList();
            foreach (var name in UserNameFind)
            {
                username = name.USERNM;
            }

            aslLog.LOGDATA = Convert.ToString("Process: " + "Process to GlMaster" + "Time: " + aslLog.LOGTIME + ",\nDate: " + model.AGlMaster.TRANSDT + ",\nUserName: " + username + ".");
            aslLog.USERPC = model.AGlMaster.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }
        // GET: /Process/

        public ActionResult Index()
        {
            return View();
        }


        [AcceptVerbs("POST")]
        [ActionName("Index")]
        public ActionResult IndexPost(PageModel model, string command)
        {
            if (command == "Process")
            {
                if (model.AGlMaster.TRANSDT != null)
                {

                    Int64 compid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"]);


                    var checkDate = (from n in db.GlStransDbSet where n.TRANSDT == model.AGlMaster.TRANSDT && n.COMPID == compid select n).OrderBy(x => x.TRANSNO).ToList();
                   
                    var checkDate_Mtrans = (from n in db.MtransdbSet where n.TRANSDT == model.AGlMaster.TRANSDT && n.COMPID == compid select n).OrderBy(x => x.TRANSNO).ToList();

                    //var checkDataFromJobBill =
                    //    (from n in db.CnfJobbillsDbSet
                    //     join m in db.CnfJobDbSet on n.COMPID equals m.COMPID
                    //     join o in db.GlAcchartDbSet on m.COMPID equals o.COMPID
                    //        where n.COMPID == compid && n.BILLPDT == model.AGlMaster.TRANSDT
                    //        && m.COMPID==compid && m.JOBYY==n.JOBYY && m.JOBTP==n.JOBTP && n.JOBNO==m.JOBNO && m.PARTYID==o.ACCOUNTCD
                    //        && o.COMPID==compid && o.ACCOUNTNM==n.PARTYNM
                           
                    //        group new{n,m} by new {n.COMPID,n.JOBNO,n.JOBTP,n.JOBYY,n.PARTYNM,n.BILLPDT,m.COMMAMT}
                    //        into grp
                    //        select new
                    //        {
                    //            Count = grp.Count(),
                    //           jobno=grp.Key.JOBNO,
                    //           jobtp=grp.Key.JOBTP,
                    //           jobyy=grp.Key.JOBYY,
                    //          partyname=grp.Key.PARTYNM,
                    //            billpdt=grp.Key.BILLPDT,
                    //            compid = grp.Key.COMPID,
                    //            sumbillamount = grp.Sum(x => x.n.BILLAMT)+grp.Key.COMMAMT
                             
                            
                    //        }).ToList();
                    DateTime transdt = Convert.ToDateTime(model.AGlMaster.TRANSDT);

                   

                    if (checkDate.Count != 0)
                    {
                        var forremovedata = (from l in db.GlMasterDbSet
                                             where l.COMPID == compid && l.TRANSDT == model.AGlMaster.TRANSDT
                                             select l).ToList();

                        foreach (var VARIABLE in forremovedata)
                        {
                            if (VARIABLE.TABLEID == "GL_STRANS")
                            {
                                db.GlMasterDbSet.Remove(VARIABLE);
                            }
                        }



                        db.SaveChanges();

                        foreach (var x in checkDate)
                        {


                            if (x.TRANSTP == "MREC")
                            {
                                Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                    where a.COMPID == compid && a.TRANSTP == x.TRANSTP
                                                                    select a.TRANSSL).Max());

                                if (maxSlCheck == 0)
                                {
                                    model.AGlMaster.TRANSSL = 10001;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;
                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";


                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;


                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();

                                    model.AGlMaster.TRANSSL = 10002;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);

                                    Insert_Process_LogData(model);
                                    db.SaveChanges();

                                }
                                else
                                {
                                    model.AGlMaster.TRANSSL = maxSlCheck + 1;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;

                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();

                                    model.AGlMaster.TRANSSL = maxSlCheck + 2;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;

                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    Insert_Process_LogData(model);
                                    db.SaveChanges();
                                }

                            }

                            else if (x.TRANSTP == "MPAY")
                            {
                                Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                    where a.COMPID == compid && a.TRANSTP == x.TRANSTP
                                                                    select a.TRANSSL).Max());

                                if (maxSlCheck == 0)
                                {
                                    model.AGlMaster.TRANSSL = 20001;


                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;

                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();


                                    model.AGlMaster.TRANSSL = 20002;


                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;

                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    Insert_Process_LogData(model);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    model.AGlMaster.TRANSSL = maxSlCheck + 1;


                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;
                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();


                                    model.AGlMaster.TRANSSL = maxSlCheck + 2;


                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;

                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    Insert_Process_LogData(model);
                                    db.SaveChanges();
                                }
                            }
                            else if (x.TRANSTP == "JOUR")
                            {
                                Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                    where a.COMPID == compid && a.TRANSTP == x.TRANSTP
                                                                    select a.TRANSSL).Max());

                                if (maxSlCheck == 0)
                                {
                                    model.AGlMaster.TRANSSL = 30001;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;
                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();

                                    model.AGlMaster.TRANSSL = 30002;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;

                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    Insert_Process_LogData(model);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    model.AGlMaster.TRANSSL = maxSlCheck + 1;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;

                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";
                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();

                                    model.AGlMaster.TRANSSL = maxSlCheck + 2;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;
                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    Insert_Process_LogData(model);
                                    db.SaveChanges();
                                }
                            }
                            else if (x.TRANSTP == "CONT")
                            {
                                Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                    where a.COMPID == compid && a.TRANSTP == x.TRANSTP
                                                                    select a.TRANSSL).Max());

                                if (maxSlCheck == 0)
                                {
                                    model.AGlMaster.TRANSSL = 40001;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;

                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";


                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();

                                    model.AGlMaster.TRANSSL = 40002;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;
                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    Insert_Process_LogData(model);
                                    db.SaveChanges();


                                }
                                else
                                {
                                    model.AGlMaster.TRANSSL = maxSlCheck + 1;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;

                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();

                                    model.AGlMaster.TRANSSL = maxSlCheck + 2;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;

                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    Insert_Process_LogData(model);
                                    db.SaveChanges();
                                }
                            }


                            Process_Done1 = "complete";
                            K++;
                            TempData["message"] = "Processing Done";




                        }



                    }
                    else if (checkDate.Count == 0)
                    {
                        var forremovedata = (from l in db.GlMasterDbSet
                                             where l.COMPID == compid && l.TRANSDT == model.AGlMaster.TRANSDT
                                             select l).ToList();

                        foreach (var VARIABLE in forremovedata)
                        {
                            if (VARIABLE.TABLEID == "GL_STRANS")
                            {
                                db.GlMasterDbSet.Remove(VARIABLE);
                            }
                        }



                        db.SaveChanges();
                    }

                    

                    
                    if (checkDate_Mtrans.Count != 0)
                    {
                        var forremovedata = (from l in db.GlMasterDbSet
                                             where l.COMPID == compid && l.TRANSDT == model.AGlMaster.TRANSDT
                                             select l).ToList();

                        foreach (var VARIABLE in forremovedata)
                        {
                            if (VARIABLE.TABLEID == "GL_MTRANS")
                            {
                                db.GlMasterDbSet.Remove(VARIABLE);
                            }
                        }



                        db.SaveChanges();

                        foreach (var x in checkDate_Mtrans)
                        {


                            if (x.TRANSTP == "MREC")
                            {
                                Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                    where a.COMPID == compid && a.TRANSTP == x.TRANSTP && a.TABLEID == "GL_MTRANS"
                                                                    select a.TRANSSL).Max());

                                if (maxSlCheck == 0)
                                {
                                    model.AGlMaster.TRANSSL = 50001;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                 
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;
                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_MTRANS";


                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;


                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();

                                    model.AGlMaster.TRANSSL = 50002;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                   
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_MTRANS";

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);

                                    Insert_Process_LogData(model);
                                    db.SaveChanges();

                                }
                                else
                                {
                                    model.AGlMaster.TRANSSL = maxSlCheck + 1;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                   
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;

                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_MTRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();

                                    model.AGlMaster.TRANSSL = maxSlCheck + 2;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                  
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;

                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_MTRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    Insert_Process_LogData(model);
                                    db.SaveChanges();
                                }

                            }

                            else if (x.TRANSTP == "MPAY")
                            {
                                Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                    where a.COMPID == compid && a.TRANSTP == x.TRANSTP && a.TABLEID == "GL_MTRANS"
                                                                    select a.TRANSSL).Max());

                                if (maxSlCheck == 0)
                                {
                                    model.AGlMaster.TRANSSL = 60001;


                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                  
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;

                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_MTRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();


                                    model.AGlMaster.TRANSSL = 60002;


                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                               
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;

                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_MTRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    Insert_Process_LogData(model);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    model.AGlMaster.TRANSSL = maxSlCheck + 1;


                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                              
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;
                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_MTRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();


                                    model.AGlMaster.TRANSSL = maxSlCheck + 2;


                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                 
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;

                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_MTRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    Insert_Process_LogData(model);
                                    db.SaveChanges();
                                }
                            }
                            else if (x.TRANSTP == "JOUR")
                            {
                                Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                    where a.COMPID == compid && a.TRANSTP == x.TRANSTP && a.TABLEID == "GL_MTRANS"
                                                                    select a.TRANSSL).Max());

                                if (maxSlCheck == 0)
                                {
                                    model.AGlMaster.TRANSSL = 70001;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                               
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;
                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_MTRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();

                                    model.AGlMaster.TRANSSL = 70002;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                  
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;

                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_MTRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    Insert_Process_LogData(model);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    model.AGlMaster.TRANSSL = maxSlCheck + 1;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                 
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;

                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_STRANS";
                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();

                                    model.AGlMaster.TRANSSL = maxSlCheck + 2;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;
                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_MTRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    Insert_Process_LogData(model);
                                    db.SaveChanges();
                                }
                            }
                            else if (x.TRANSTP == "CONT")
                            {
                                Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                    where a.COMPID == compid && a.TRANSTP == x.TRANSTP && a.TABLEID == "GL_MTRANS"
                                                                    select a.TRANSSL).Max());

                                if (maxSlCheck == 0)
                                {
                                    model.AGlMaster.TRANSSL = 80001;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;

                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_MTRANS";


                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();

                                    model.AGlMaster.TRANSSL = 80002;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                              
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;
                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_MTRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    Insert_Process_LogData(model);
                                    db.SaveChanges();


                                }
                                else
                                {
                                    model.AGlMaster.TRANSSL = maxSlCheck + 1;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                                  
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.DEBITCD;
                                    model.AGlMaster.CREDITCD = x.CREDITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = x.AMOUNT;
                                    model.AGlMaster.CREDITAMT = 0;

                                    model.AGlMaster.TRANSDRCR = "DEBIT";
                                    model.AGlMaster.TABLEID = "GL_MTRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    db.SaveChanges();

                                    model.AGlMaster.TRANSSL = maxSlCheck + 2;

                                    model.AGlMaster.TRANSDT = x.TRANSDT;
                                    model.AGlMaster.COMPID = x.COMPID;
                            
                                    model.AGlMaster.TRANSTP = x.TRANSTP;
                                    model.AGlMaster.TRANSMY = x.TRANSMY;
                                    model.AGlMaster.TRANSNO = x.TRANSNO;
                                    model.AGlMaster.TRANSFOR = x.TRANSFOR;
                                    model.AGlMaster.TRANSMODE = x.TRANSMODE;
                                    model.AGlMaster.TRIPNO = x.TRIPNO;
                                    model.AGlMaster.COSTPID = x.COSTPID;
                                    model.AGlMaster.DEBITCD = x.CREDITCD;
                                    model.AGlMaster.CREDITCD = x.DEBITCD;
                                    model.AGlMaster.CHEQUENO = x.CHEQUENO;
                                    model.AGlMaster.CHEQUEDT = x.CHEQUEDT;
                                    model.AGlMaster.REMARKS = x.REMARKS;

                                    model.AGlMaster.DEBITAMT = 0;
                                    model.AGlMaster.CREDITAMT = x.AMOUNT;

                                    model.AGlMaster.TRANSDRCR = "CREDIT";
                                    model.AGlMaster.TABLEID = "GL_MTRANS";

                                    model.AGlMaster.USERPC = strHostName;
                                    model.AGlMaster.INSIPNO = ipAddress.ToString();
                                    model.AGlMaster.INSTIME = Convert.ToDateTime(td);

                                    model.AGlMaster.INSUSERID =
                                        Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                                    model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                                    db.GlMasterDbSet.Add(model.AGlMaster);
                                    Insert_Process_LogData(model);
                                    db.SaveChanges();
                                }
                            }

                            Process_Done2 = "complete";
                            K++;
                            TempData["message"] = "Processing Done";




                        }



                    }
                    else if (checkDate_Mtrans.Count == 0)
                    {
                        var forremovedata = (from l in db.GlMasterDbSet
                                             where l.COMPID == compid && l.TRANSDT == model.AGlMaster.TRANSDT
                                             select l).ToList();

                        foreach (var VARIABLE in forremovedata)
                        {
                            if (VARIABLE.TABLEID == "GL_MTRANS")
                            {
                                db.GlMasterDbSet.Remove(VARIABLE);
                            }
                        }



                        db.SaveChanges();
                    }
                    else 
                    {
                        TempData["message"] = "No no...process would not done";

                    }



                    //Tms Expense Process from DataAccess folder(class Tms_Expense_Process)
                    string TmsExpense = "";
                    TmsExpense = Tms_Expense_Process.process(model, compid);
                    if (TmsExpense == "True")
                    {
                        K++;
                        //if (K == 1)
                        //{
                        //    Insert_Process_LogData(logData);
                        //    db.SaveChanges();
                        //}
                        Process_Done3 = "complete";
                        TempData["message"] = "Processing Done";
                    }
                    else if (TmsExpense == "False")
                    {
                        TempData["message"] = "No no...process would not done";
                    }




                    //Tms Trip Process from DataAccess folder(class Tms_TRIP)
                    string TmsTrip = "";
                    TmsTrip = Tms_Trip_Process.process(model, compid);
                    if (TmsTrip == "True")
                    {
                        K++;
                        //if (K == 1)
                        //{
                        //    Insert_Process_LogData(logData);
                        //    db.SaveChanges();
                        //}
                        Process_Done4 = "complete";
                        TempData["message"] = "Processing Done";
                    }
                    else if (TmsTrip == "False")
                    {
                        TempData["message"] = "No no...process would not done";
                    }





                    if (Process_Done1 != "" || Process_Done2 != "" || Process_Done3 != "" || Process_Done4 != "" || Process_Done5 != "" || Process_Done6 != "" || Process_Done7 != "")
                    {
                        TempData["message"] = "Processing Done";
                    }
                    else
                    {
                        TempData["message"] = "No no...process would not done";
                    }

                }

            }
            else
            {
                TempData["message"] = null;
            }




            return RedirectToAction("Index");
        }


    }
}
