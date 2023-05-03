using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Cloud_Transport.Models;
using Cloud_Transport.Models.ASL;
using Cloud_Transport.Models.GL;
using Cloud_Transport.Models.TMS;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;

namespace Cloud_Transport.Models
{
    public class PageModel
    {

        public PageModel()
        {
            this.aslMenumst = new ASL_MENUMST();
            this.aslMenu = new ASL_MENU();
            this.aslUserco = new AslUserco();
            this.aslCompany = new AslCompany();
            this.aslLog = new ASL_LOG();
            this.AGL_accharmst = new GL_ACCHARMST();
            this.AGL_acchart = new GL_ACCHART();
            
            this.AslTable = new ASL_TABLE();
            this.AslTableField = new ASL_TABFIELD();



            //GL models
            this.AGlCostPMST = new GL_COSTPMST();
            this.AGlCostPool = new GL_COSTPOOL();
            this.AGlStrans = new GL_STRANS();
            this.AGlMaster = new GL_MASTER();


            //Trnasport models
            this.TmsTripmst = new TMS_TRIPMST();
            this.TmsTrip = new TMS_TRIP();
            this.TmsExpense = new TMS_EXPENSE();
        }

        public ASL_MENUMST aslMenumst { get; set; }
        public ASL_MENU aslMenu { get; set; }
        public AslUserco aslUserco { get; set; }
        public AslCompany aslCompany { get; set; }
        public ASL_LOG aslLog { get; set; }


        public ASL_TABLE AslTable { get; set; }
        public ASL_TABFIELD AslTableField { get; set; }

        // GL
        public GL_ACCHARMST AGL_accharmst { get; set; }
        public GL_ACCHART AGL_acchart { get; set; }
        public GL_COSTPMST AGlCostPMST { get; set; }
        public GL_COSTPOOL AGlCostPool { get; set; }
        public GL_STRANS AGlStrans { get; set; }
        public GL_MASTER AGlMaster { get; set; }


        // Transport
        public TMS_TRIPMST TmsTripmst { get; set; }
        public TMS_TRIP TmsTrip { get; set; }
        public TMS_EXPENSE TmsExpense { get; set; }





        [Display(Name = "HeadType")]
        public string HeadType { get; set; }


        [Required(ErrorMessage = "From date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessage = "To Date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
        public string JOBTP { get; set; }

        public string EXPNM { get; set; }



        //Costpool
        public string BuyDate { get; set; }
        public string RegDate { get; set; }



        //Tms  - TripMaster
        public string TripDate { get; set; }
        public string TripChildDate { get; set; }



        //ReportController
        [Required(ErrorMessage = "From date field can not be empty!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string Report_FromDate { get; set; }

        [Required(ErrorMessage = "To Date field can not be empty!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string Report_ToDate { get; set; }




        //Schedular Calendar
        public Int64? Userid { get; set; }
    }
}