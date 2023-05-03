using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud_Transport.Models.DTO
{
    public class MTransDTO
    {
        public Int64 ID { get; set; }
        public Int64 CompanyID { get; set; }
        public Int64 BranchID { get; set; }
        public string BranchName { get; set; }
        public string TransType { get; set; }
     
        public string TransDate { get; set; }
     
        public string TransMonthYear { get; set; }
       
        public Int64? Transno { get; set; }

        public Int64? TransSerial { get; set; }
       
        public string TransFor { get; set; }
      
        public string TransactionMode { get; set; }
        public Int64? TripNo { get; set; }

        public Int64? CostPoolID { get; set; }
        public string CostPoolName { get; set; }
   
        public Int64? CreditCode { get; set; }
        public string CreditAccount { get; set; }
      
        public Int64? DebitCode { get; set; }
        public string DebitAccount { get; set; }
      
        public string CHEQUENO { get; set; }
     
        public string CHEQUEDT { get; set; }
       
        public string REMARKS { get; set; }


      



        public decimal? Amount { get; set; }









     
        public string USERPC { get; set; }
        public Int64? INSUSERID { get; set; }

     
        public DateTime? INSTIME { get; set; }

   
        public string INSIPNO { get; set; }
        public string INSLTUDE { get; set; }
        public Int64? UPDUSERID { get; set; }

    
        public DateTime? UPDTIME { get; set; }

     
        public string UPDIPNO { get; set; }
        public string UPDLTUDE { get; set; }



    }
}