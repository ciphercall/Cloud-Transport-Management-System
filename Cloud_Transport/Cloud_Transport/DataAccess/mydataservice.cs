using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Cloud_Transport.Models;
using Cloud_Transport.Models.TMS;

namespace Cloud_Transport.DataAccess
{
    public class mydataservice
    {
        public IEnumerable TopcategoriesListdata(Int64 loggedcompid, string todate, string frdate)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CnfDbContext"].ToString());

            var query = string.Format(@"SELECT GL_ACCHART.ACCOUNTNM AS CATNM, SUM(TMS_TRIP.AMTTOT) AS VALUE 
FROM  GL_ACCHART INNER JOIN 
TMS_TRIP ON GL_ACCHART.COMPID = TMS_TRIP.COMPID AND GL_ACCHART.ACCOUNTCD = TMS_TRIP.PARTYID
WHERE TMS_TRIP.COMPID='{0}' AND TMS_TRIP.TRIPDT  BETWEEN '{1}' AND  '{2}' 
GROUP BY GL_ACCHART.ACCOUNTNM ORDER BY VALUE DESC ", loggedcompid, todate, frdate);

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            IList<Outputclass> transList = new List<Outputclass>();
            // var GetList = ds.ToList();

            foreach (DataRow row in ds.Rows)
            {
                transList.Add(new Outputclass()
                {
                    PlanName = row["CATNM"].ToString(),
                    PaymentAmount = Convert.ToInt64(row["VALUE"])

                });
            }
            // var list = con.Query<Outputclass>("Usp_Getdata").AsEnumerable();
            // List of type Outputclass which it will return .
            return transList;
        }



        public IEnumerable TopItemsByQtyListdata(Int64 loggedcompid, string todate, string frdate)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CnfDbContext"].ToString());

            var query = string.Format(@"SELECT GL_COSTPOOL.COSTPNM AS ITEMNM, COUNT(TRIPNO) QTY 
FROM  GL_COSTPOOL INNER JOIN 
TMS_TRIP ON GL_COSTPOOL.COMPID = TMS_TRIP.COMPID AND GL_COSTPOOL.COSTPID = TMS_TRIP.COSTPID 
WHERE TMS_TRIP.COMPID='{0}' AND TMS_TRIP.TRIPDT  BETWEEN '{1}' AND  '{2}'
GROUP BY GL_COSTPOOL.COSTPNM ORDER BY QTY DESC", loggedcompid, todate, frdate);

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            IList<Outputclass> transList = new List<Outputclass>();
            // var GetList = ds.ToList();

            foreach (DataRow row in ds.Rows)
            {
                transList.Add(new Outputclass()
                {
                    PlanName = row["ITEMNM"].ToString(),
                    PaymentAmount = Convert.ToInt64(row["QTY"])

                });
            }
            // var list = con.Query<Outputclass>("Usp_Getdata").AsEnumerable();
            // List of type Outputclass which it will return .
            return transList;
        }




        public IEnumerable TopItemsByValueListdata(Int64 loggedcompid, string todate, string frdate)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CnfDbContext"].ToString());

            var query = string.Format(@"SELECT GL_COSTPOOL.COSTPNM AS ITEMNM, SUM(TMS_TRIP.AMTTOT) VALUE
FROM  GL_COSTPOOL INNER JOIN 
TMS_TRIP ON GL_COSTPOOL.COMPID = TMS_TRIP.COMPID AND GL_COSTPOOL.COSTPID = TMS_TRIP.COSTPID 
WHERE TMS_TRIP.COMPID='{0}' AND TMS_TRIP.TRIPDT  BETWEEN  '{1}' AND  '{2}'
GROUP BY GL_COSTPOOL.COSTPNM ORDER BY VALUE DESC", loggedcompid, todate, frdate);

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            IList<Outputclass> transList = new List<Outputclass>();
            // var GetList = ds.ToList();

            foreach (DataRow row in ds.Rows)
            {
                transList.Add(new Outputclass()
                {
                    PlanName = row["ITEMNM"].ToString(),
                    PaymentAmount = Convert.ToInt64(row["VALUE"])

                });
            }
            // var list = con.Query<Outputclass>("Usp_Getdata").AsEnumerable();
            // List of type Outputclass which it will return .
            return transList;
        }




        public IEnumerable CollectionDataListdata(Int64 loggedcompid, string todate, string frdate)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CnfDbContext"].ToString());

            var query = string.Format(@"SELECT  CONVERT(NVARCHAR(20),TRIPDT,103) AS TRIPDT, SUM(AMTTOT) COLLECT
FROM TMS_TRIP
WHERE COMPID='{0}' AND TRIPDT  BETWEEN  '{1}' AND  '{2}' GROUP BY TRIPDT", loggedcompid, todate, frdate);

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            IList<Outputclass> transList = new List<Outputclass>();
            // var GetList = ds.ToList();

            foreach (DataRow row in ds.Rows)
            {
                transList.Add(new Outputclass()
                {
                    PlanName = row["TRIPDT"].ToString(),
                    PaymentAmount = Convert.ToInt64(row["COLLECT"])

                });
            }
            // var list = con.Query<Outputclass>("Usp_Getdata").AsEnumerable();
            // List of type Outputclass which it will return .
            return transList;
        }



        //public IEnumerable TimeWiseSellDataListdata(Int64 loggedcompid, string todate, string frdate)
        //{
        //    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CnfDbContext"].ToString());

        //    var query = string.Format(@" SELECT DISTINCT CONVERT(NVARCHAR(20),dateadd(hour, datediff(hour, 0, dateadd(mi, 30, INSTIME)), 0) ,108) AS INSTIME, SUM(TOTGROSS) AMOUNT
        //        FROM STK_TRANSMST WHERE TRANSTP='SALE' AND COMPID='" + loggedcompid + "' AND TRANSDT  BETWEEN '" + todate + "' AND  '"
        //       + frdate + "' GROUP BY CONVERT(NVARCHAR(20),dateadd(hour, datediff(hour, 0, dateadd(mi, 30, INSTIME)), 0) ,108)");

        //    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
        //    conn.Open();

        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataTable ds = new DataTable();
        //    da.Fill(ds);

        //    IList<Outputclass> transList = new List<Outputclass>();
        //    // var GetList = ds.ToList();

        //    foreach (DataRow row in ds.Rows)
        //    {
        //        transList.Add(new Outputclass()
        //        {
        //            PlanName = row["INSTIME"].ToString(),
        //            PaymentAmount = Convert.ToInt64(row["AMOUNT"])

        //        });
        //    }
        //    // var list = con.Query<Outputclass>("Usp_Getdata").AsEnumerable();
        //    // List of type Outputclass which it will return .
        //    return transList;
        //}
    }
}