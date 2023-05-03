using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloud_Transport.Controllers.ASL
{
    public class LogoutController : AppController
    {
        public ActionResult Index()
        {
            Session.Abandon();
            Global.GlobalVariable = 1;
            return RedirectToAction("Index", "Login");
        }
    }
}
