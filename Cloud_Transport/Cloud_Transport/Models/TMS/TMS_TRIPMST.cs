﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cloud_Transport.Models.TMS
{
    [Table("TMS_TRIPMST")]
    public class TMS_TRIPMST
    {
        //COMPID NUMBER(3),  	--101
        //COSTPID NUMBER(10),
        //TRIPDT DATE,
        //TRIPMY     VARCHAR2(6),
        //RTRIPNO NUMBER(6),
        //QTYFUEL NUMBER(15,2),
        //QTYMOBIL NUMBER(15,2),
        //REMARKS VARCHAR2(100),
        //..
        //PRIMARY KEY(COMPID,COSTPID, TRIPMY, RTRIPNO)


        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }

        [Key, Column(Order = 1)]
        public Int64 COMPID { get; set; }

        [Key, Column(Order = 2)]
        public Int64 COSTPID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? TRIPDT { get; set; }

        [Key, Column(Order = 3)]
        [StringLength(6, MinimumLength = 6)]
        public String TRIPMY { get; set; }

        [Key, Column(Order = 4)]
        public Int64 RTRIPNO { get; set; }

        public Decimal? QTYFUEL { get; set; }
        public Decimal? QTYMOBIL { get; set; }

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