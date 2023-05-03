using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cloud_Transport.Models.TMS
{
    [Table("TMS_TRIP")]
    public class TMS_TRIP
    {

        //COMPID NUMBER(3),  	--101
        //COSTPID NUMBER(10),
        //TRIPDT DATE,
        //TRIPMY     VARCHAR2(6),
        //RTRIPNO NUMBER(6),
        //TRIPNO NUMBER(8),
        //TRIPFR VARCHAR2(50),
        //TRIPTO VARCHAR2(50),
        //TRIPTP VARCHAR2(10),	
        //PARTYID NUMBER(10),
        //CDESTN VARCHAR2(50),
        //AMTFARE NUMBER(15,2),
        //AMTDEMI NUMBER(15,2),
        //AMTOTC NUMBER(15,2),
        //AMTTOT NUMBER(15,2),
        //REMARKS VARCHAR2(100),	
        //..
        //PRIMARY KEY(COMPID, COSTPID, TRIPMY, RTRIPNO, TRIPNO)


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

        [Key, Column(Order = 5)]
        public Int64 TRIPNO { get; set; }

        [StringLength(50, MinimumLength = 0)]
        public String TRIPFR { get; set; }

        [StringLength(50, MinimumLength = 0)]
        public String TRIPTO { get; set; }

        [StringLength(10, MinimumLength = 0)]
        public String TRIPTP { get; set; }
        public Int64? PARTYID { get; set; }

        [StringLength(50, MinimumLength = 0)]
        public String CDESTN { get; set; }

        public Decimal? AMTFARE { get; set; }
        public Decimal? AMTDEMI { get; set; }
        public Decimal? AMTOTC { get; set; }
        public Decimal? AMTTOT { get; set; }

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