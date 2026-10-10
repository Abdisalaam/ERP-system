using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class SpentReciptOrderVM
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
      
        public int Employee_Id { get; set; }
        public SelectList EmpList { get; set; }
        public int SpentReciptOrderTypeId { get; set; }
    }
}