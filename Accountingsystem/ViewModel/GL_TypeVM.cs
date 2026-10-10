using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Accountingsystem.ViewModel
{
    public class GL_TypeVM
    {
        public int GL_Type_Id { get; set; }
     
        public string GL_Type_Name { get; set; }

      
        public string GL_Type_Description { get; set; }
    }
}