using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class ContactVM
    {
        public int Contact_Id { get; set; }
        public string Contact_Name { get; set; }
        public string Contact_Company { get; set; }
        public string Contact_Email { get; set; }
        public int Customer_Id { get; set; }
        public SelectList MyCustomerList { get; set; }
    }
}