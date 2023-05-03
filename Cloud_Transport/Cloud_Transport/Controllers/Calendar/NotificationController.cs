﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cloud_Transport.Models;
using Cloud_Transport.Models.ASL;
using Cloud_Transport.Models.DTO;
using iTextSharp.text;

namespace Cloud_Transport.Controllers.Calendar
{
    public class NotificationController : Controller
    {
        //Datetime formet
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        Int64 LoggedCompId;
        Int64 loggedUserID;
        public NotificationController()
        {
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            try
            {
                LoggedCompId = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
                loggedUserID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
            }
            catch (Exception)
            {
                LoggedCompId = 0;
                loggedUserID = 0;
            }

        }

        public JsonResult GetNotificationContacts()
        {
            CnfDbContext db = new CnfDbContext();
            DateTime sevenDays = td.AddDays(6);
            DateTime yesterday = td.AddDays(-1);

            List<SchedulerCalendarDTO> list = new List<SchedulerCalendarDTO>();
            //var getData = db.SchedularCalendarDbSet.Where(a => a.COMPID == LoggedCompId && a.USERID == loggedUserID && a.StartDate > yesterday && a.StartDate<= sevenDays).OrderBy(a => a.StartDate).ToList();
            var getData = (from m in db.SchedularCalendarDbSet
                where m.COMPID == LoggedCompId && m.USERID == loggedUserID
                      && m.StartDate > yesterday && m.StartDate <= sevenDays && m.Status== "Active"
                           select m)
                .Union
                (from m in db.SchedularCalendarDbSet
                    where m.COMPID == LoggedCompId && m.USERID == loggedUserID
                          && m.EndDate > yesterday && m.EndDate <= sevenDays && m.Status == "Active"
                 select m).ToList();

            foreach (var get in getData)
            {
                DateTime start = Convert.ToDateTime(get.StartDate);
                String startDate = start.ToString("dd-MMM-yyyy hh:mm tt");

                DateTime end = Convert.ToDateTime(get.EndDate);
                String endDate = end.ToString("dd-MMM-yyyy hh:mm tt");

                SchedulerCalendarDTO subList = new SchedulerCalendarDTO();
                subList.Title = get.Title;
                subList.Text = get.Text;
                subList.StartDate = startDate;
                subList.EndDate = endDate;
                list.Add(subList);
            }
            //update session here for get only new added contacts (notification)
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetNotificationCount()
        {
            CnfDbContext db = new CnfDbContext();
            DateTime sevenDays = td.AddDays(6);
            DateTime yesterday = td.AddDays(-1);

            var list = (from m in db.SchedularCalendarDbSet
                        where m.COMPID == LoggedCompId && m.USERID == loggedUserID
                              && m.StartDate > yesterday && m.StartDate <= sevenDays && m.Status == "Active"
                        select m)
                .Union
                (from m in db.SchedularCalendarDbSet
                 where m.COMPID == LoggedCompId && m.USERID == loggedUserID
                       && m.EndDate > yesterday && m.EndDate <= sevenDays && m.Status == "Active"
                 select m).ToList();
            //update session here for get only new added contacts (notification)
            return new JsonResult { Data = list.Count, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}
