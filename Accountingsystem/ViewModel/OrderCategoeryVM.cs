using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class OrderCategoeryVM
    {
        public int Id { get; set; }      
        public string CategoeryName { get; set; }
        public HttpPostedFileBase CategoeryImage { get; set; }
    }
}