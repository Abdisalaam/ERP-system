using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class ProductQuantityVM
    {
      
       
   
        public int number_Of_Document{get;set;}
        public String Document_Serial { get; set; }
        public String Type_Document { get; set; }
        public DateTime DateOfDocument { get; set; }
        public Decimal StartQuantity { get; set; }
        public Decimal StartProCost { get; set; }
        public Decimal StartCost { get; set; }
        public Decimal importQuantity { get; set; }
        public Decimal importProCost { get; set; }
        public Decimal importCost { get; set; }
        public Decimal ExportQuantity { get; set; }
        public Decimal ExportProCost { get; set; }
        public Decimal ExportCost { get; set; }
        public Store store { get; set; }





    }
}