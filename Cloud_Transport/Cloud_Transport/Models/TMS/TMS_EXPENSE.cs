using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cloud_Transport.Models.TMS
{
    [Table("TMS_EXPENSE")]
    public class TMS_EXPENSE
    {
        //COMPID NUMBER(3),  	--101
        //TRANSDT DATE,
        //TRANSMY        VARCHAR2(6),
        //TRANSNO NUMBER(10),		--2015010001(YYYYMM0001)
        //TRANSSL NUMBER(5),		--90001
        //TRIPNO NUMBER(8), 
        //COSTPID NUMBER(10),
        //DEBITCD NUMBER(10),		--1011010001, 1011020001
        //AMOUNT NUMBER(15,2),
        //REMARKS VARCHAR2(100),
        //..
        //PRIMARY KEY(COMPID, TRANSMY, TRANSNO, TRANSSL)


        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }

        [Key, Column(Order = 1)]
        public Int64 COMPID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? TRANSDT { get; set; }

        [Key, Column(Order = 2)]
        [StringLength(6, MinimumLength = 6)]
        public String TRANSMY { get; set; }

        [Key, Column(Order = 3)]
        public Int64 TRANSNO { get; set; }

        [Key, Column(Order = 4)]
        public Int64 TRANSSL { get; set; }
        public Int64? TRIPNO { get; set; }
        public Int64? COSTPID { get; set; }
        public Int64? DEBITCD { get; set; }
        public Decimal? AMOUNT { get; set; }
       
        [StringLength(100, MinimumLength = 0)]
        public String REMARKS { get; set; }







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
    }
}