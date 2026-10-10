using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Accountingsystem.ViewModel
{
    public class ContentPersonDetailsVM
    {
        
        public int ContentPerson_ID { get; set; }
      
        public string Content_Name { get; set; }
     
        public string Content_Number { get; set; }
       
        public string Content_Mail { get; set; }
     
        public string Content_Job { get; set; }
        public int CustomerCustomer_Id { get; set; }
    }
}