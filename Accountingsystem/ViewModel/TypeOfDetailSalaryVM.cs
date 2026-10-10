using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class TypeOfDetailSalaryVM
    {
        public int Id { get; set; }
  
        public string SalaryDetail_Name { get; set; }
       
        public SalaryTypeValue Sal_Type { get; set; }
    }
}