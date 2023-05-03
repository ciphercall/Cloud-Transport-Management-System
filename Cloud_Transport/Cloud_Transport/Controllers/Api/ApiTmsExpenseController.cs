using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cloud_Transport.Controllers.TMS;
using Cloud_Transport.Models;
using Cloud_Transport.Models.DTO;
using Cloud_Transport.Models.TMS;

namespace Cloud_Transport.Controllers.Api
{
    public class ApiTmsExpenseController : ApiController
    {
        CnfDbContext db = new CnfDbContext();

        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public ApiTmsExpenseController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }




        [System.Web.Http.HttpGet]
        public IEnumerable<Tms_ExpenseDTO> GetData(Int64 companyID, String transMonthYear, Int64 transNo)
        {
            //Int64 compid = 0, rID = 0;
            //try
            //{
            //    compid = Convert.ToInt64(companyID);
            //    rID = Convert.ToInt64(regID);
            //}
            //catch
            //{
            //    compid = 0;
            //    rID = 0;
            //}

            var find_GridData = (from t1 in db.TmsExpenseDbSet
                                 where t1.COMPID == companyID && t1.TRANSMY == transMonthYear && t1.TRANSNO == transNo
                                 select new
                                 {
                                     t1.ID,
                                     t1.COMPID,
                                     t1.TRANSDT,
                                     t1.TRANSMY,
                                     t1.TRANSNO,
                                     t1.TRANSSL,
                                     t1.TRIPNO,
                                     t1.COSTPID,
                                     t1.DEBITCD,
                                     t1.AMOUNT,
                                     t1.REMARKS,


                                     t1.INSIPNO,
                                     t1.INSLTUDE,
                                     t1.INSTIME,
                                     t1.INSUSERID,
                                 }).OrderBy(e => e.TRANSSL).ToList();

            if (find_GridData.Count == 0)
            {
                yield return new Tms_ExpenseDTO
                {
                    count = 1, //no data found
                };
            }
            else
            {
                foreach (var s in find_GridData)
                {
                    String accountheadName = "";
                    Int64 headCD = Convert.ToInt64(companyID + "401");
                    var find_Accounthead = (from n in db.GlAcchartDbSet where n.COMPID == companyID && n.HEADCD == headCD && n.ACCOUNTCD==s.DEBITCD select new { n.ACCOUNTCD, n.ACCOUNTNM }).ToList();
                    foreach (var item in find_Accounthead)
                    {
                        accountheadName = item.ACCOUNTNM.ToString();
                    }
                    yield return new Tms_ExpenseDTO
                    {
                        ID = s.ID,
                        COMPID = Convert.ToInt64(s.COMPID),
                        TRANSDT = Convert.ToString(s.TRANSDT),
                        TRANSMY = Convert.ToString(s.TRANSMY),
                        TRANSNO = s.TRANSNO,
                        TRANSSL = s.TRANSSL,
                        TRIPNO = s.TRIPNO,
                        COSTPID = s.COSTPID,
                        DEBITCD = s.DEBITCD,
                        AccountHeadName = accountheadName,
                        AMOUNT = s.AMOUNT,
                        REMARKS = s.REMARKS,

                        INSUSERID = s.INSUSERID,
                        INSLTUDE = s.INSLTUDE,
                        INSTIME = s.INSTIME,
                        INSIPNO = s.INSIPNO,
                    };
                }
            }
        }









        [HttpPost]
        public HttpResponseMessage AddData(Tms_ExpenseDTO model)
        {
            TMS_EXPENSE tmsExpense = new TMS_EXPENSE();

            var check_data = (from n in db.TmsExpenseDbSet where n.COMPID == model.COMPID && n.TRANSMY == model.TRANSMY && n.TRANSNO == model.TRANSNO && n.DEBITCD == model.DEBITCD select n).ToList();
            if (check_data.Count == 0)
            {
                Int64 R = Convert.ToInt64("99999");
                Int64 maxData = 0;
                try
                {
                    maxData = Convert.ToInt64((from n in db.TmsExpenseDbSet where n.COMPID == model.COMPID && n.TRANSMY == model.TRANSMY && n.TRANSNO == model.TRANSNO select n.TRANSSL).Max());
                }
                catch
                {
                    maxData = 0;
                }
                if (maxData == 0)
                {
                    tmsExpense.TRANSSL = Convert.ToInt64("90001");
                }
                else if (maxData <= R)
                {
                    tmsExpense.TRANSSL = maxData + 1;
                }
                else
                {
                    HttpResponseMessage breakresponse = Request.CreateResponse(HttpStatusCode.Created, model);
                    return breakresponse;
                }
                tmsExpense.COMPID = model.COMPID;
                tmsExpense.TRANSDT = Convert.ToDateTime(model.TRANSDT);
                tmsExpense.TRANSMY = model.TRANSMY;
                tmsExpense.TRANSNO = model.TRANSNO;
                tmsExpense.TRANSSL = tmsExpense.TRANSSL;
                tmsExpense.TRIPNO = model.TRIPNO;
                tmsExpense.COSTPID = model.COSTPID;
                tmsExpense.DEBITCD = model.DEBITCD;
                tmsExpense.AMOUNT = model.AMOUNT;
                tmsExpense.REMARKS = model.REMARKS;

                tmsExpense.INSIPNO = ipAddress.ToString();
                tmsExpense.INSTIME = Convert.ToDateTime(td);
                tmsExpense.INSUSERID = model.INSUSERID;
                tmsExpense.INSLTUDE = Convert.ToString(model.INSLTUDE);

                db.TmsExpenseDbSet.Add(tmsExpense);
                db.SaveChanges();

                //model.ID = regComplementaryItem.ID;
                //model.COMPID = regComplementaryItem.COMPID;
                //model.REGNID = regComplementaryItem.REGNID;
                //model.CITEMID = regComplementaryItem.CITEMID;
                //model.USERPC = regComplementaryItem.USERPC;
                model.TRANSSL = tmsExpense.TRANSSL;
                //model.INSIPNO = regComplementaryItem.INSIPNO;
                //model.INSTIME = regComplementaryItem.INSTIME;
                //model.INSUSERID = regComplementaryItem.INSUSERID;
                //model.INSLTUDE = Convert.ToString(regComplementaryItem.INSLTUDE);

                //Log data save from TMS_EXPENSE Tabel
                TmsExpenseController controller = new TmsExpenseController();
                controller.Insert_TmsExpense_LogData(tmsExpense);

                HttpResponseMessage response1 = Request.CreateResponse(HttpStatusCode.Created, model);
                return response1;
            }
            else
            {
                model.TRANSSL = 0;
                HttpResponseMessage response2 = Request.CreateResponse(HttpStatusCode.Created, model);
                return response2;
            }

        }





        [HttpPost]
        public HttpResponseMessage UpdateData(Tms_ExpenseDTO model)
        {
            var data_find = (from n in db.TmsExpenseDbSet where n.ID == model.ID && n.COMPID == model.COMPID && n.TRANSMY == model.TRANSMY && n.TRANSNO == model.TRANSNO && n.TRANSSL == model.TRANSSL select n).ToList();
            foreach (var item in data_find)
            {
                item.TRIPNO = model.TRIPNO;
                item.COSTPID = model.COSTPID;
                item.DEBITCD = model.DEBITCD;
                item.AMOUNT = model.AMOUNT;
                item.REMARKS = model.REMARKS;

                item.USERPC = strHostName;
                item.UPDIPNO = ipAddress.ToString();
                item.UPDLTUDE = Convert.ToString(model.INSLTUDE);
                item.UPDTIME = Convert.ToDateTime(td);
                item.UPDUSERID = Convert.ToInt16(model.INSUSERID);

            }
            db.SaveChanges();

            //Log data save from TMS_EXPENSE Tabel
            TmsExpenseController controller = new TmsExpenseController();
            controller.update_TmsExpense_LogData(model);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
            return response;

        }






        [HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(Tms_ExpenseDTO model)
        {
            TMS_EXPENSE deleteModel = new TMS_EXPENSE();
            deleteModel.ID = model.ID;
            deleteModel.COMPID = model.COMPID;
            deleteModel.TRANSMY = Convert.ToString(model.TRANSMY);
            deleteModel.TRANSNO = Convert.ToInt64(model.TRANSNO);
            deleteModel.TRANSSL = Convert.ToInt64(model.TRANSSL);

            deleteModel = db.TmsExpenseDbSet.Find(deleteModel.ID, deleteModel.COMPID, deleteModel.TRANSMY, deleteModel.TRANSNO, deleteModel.TRANSSL);
            db.TmsExpenseDbSet.Remove(deleteModel);
            db.SaveChanges();

            //Log data save from TMS_EXPENSE Tabel
            TmsExpenseController controller = new TmsExpenseController();
            controller.Delete_TmsExpense_LogData(model);
            controller.Delete_TmsExpense_LogDelete(model);

            Tms_ExpenseDTO Obj = new Tms_ExpenseDTO();
            return Request.CreateResponse(HttpStatusCode.OK, Obj);
        }
    }
}
