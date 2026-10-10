using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Accountingsystem.ViewModel
{
    public class SupInvoiceDetailsVM
    {
        public int Id { get; set; }
   
        public string Serial_Number { get; set; }



        public string Description { get; set; }

        [DataType(DataType.Date)]
      
        public System.DateTime Invoice_Date { get; set; }



        public decimal Total_Amount { get; set; }


   
        public decimal TotalAmountAfMat { get; set; }
    }
}