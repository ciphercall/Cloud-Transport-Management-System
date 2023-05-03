using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cloud_Transport.Models.DTO
{
    public class Tms_ExpenseDTO
    {
        public Int64 ID { get; set; }
        public Int64 COMPID { get; set; }
        public String TRANSDT { get; set; }
        public String TRANSMY { get; set; }
        public Int64 TRANSNO { get; set; }
        public Int64 TRANSSL { get; set; }
        public Int64? TRIPNO { get; set; }
        public Int64? COSTPID { get; set; }
        public Int64? DEBITCD { get; set; }
        public Decimal? AMOUNT { get; set; }
        public String REMARKS { get; set; }




        public String AccountHeadName { get; set; }



        public String USERPC { get; set; }
        public Int64 INSUSERID { get; set; }

        //[Display(Name = "Insert Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? INSTIME { get; set; }
        public String INSIPNO { get; set; }
        public String INSLTUDE { get; set; }
        public Int64 UPDUSERID { get; set; }

        //[Display(Name = "Update Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UPDTIME { get; set; }
        public String UPDIPNO { get; set; }
        public String UPDLTUDE { get; set; }

        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }


        public Int64 count { get; set; }

    }
}