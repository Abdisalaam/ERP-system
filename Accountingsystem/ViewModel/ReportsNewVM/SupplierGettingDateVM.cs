using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class SupplierGettingDateVM
    {
        [Required(ErrorMessage =" ادخل التاريخ")]
        public string startDate { get; set; }
    }
}