using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class Maintenance_Invoice_MasterVM
    {
        public int Main_Invoice_Id { get; set; }

        public string Customer_Name { get; set; }

        public String Starting_Date { get; set; }

        public String Ending_Date { get; set; }
 
        public string Subscription_Code { get; set; }
  
        public string Main_Invoice_Code { get; set; }
  
        public string Describtion { get; set; }
  
        public int Subscription_Id { get; set; }
    }
}