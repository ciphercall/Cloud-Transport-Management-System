using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cloud_Transport.Models;

namespace Cloud_Transport.Controllers
{
    public class RegisterController : AppController
    {
        //
        // GET: /Register/

        public ActionResult ChequeRegisterIndex()
        {
            ViewData["HighLight_Menu_GL_Report"] = "heighlight";
            return View();
        }

        [HttpPost]
        public ActionResult ChequeRegisterIndex(PageModel model)
        {


            TempData["ChequeRegister"] = model;
            return RedirectToAction("ChequeRegisterReport");
        }
        public ActionResult ChequeRegisterReport()
        {
            PageModel model = (PageModel)TempData["ChequeRegister"];
            return View(model);
        }



        public ActionResult JournalRegisterIndex()
        {
            return View();
        }

    }
}
