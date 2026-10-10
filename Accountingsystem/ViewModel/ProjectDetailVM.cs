using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class ProjectDetailVM
    {
        public int Id { get; set; }
        public string Description_Detail { get; set; }
        public decimal Amount { get; set; }
        public string Date_Amount { get; set; }
       
        public Boolean Payed { get; set; }

        public int Project_Id { get; set; }
    }
}