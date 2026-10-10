using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class StoreChoose
    {
        [DisplayName("اسم المخزن")]
        [Required(ErrorMessage = " ادخل المخزن")]
        public int Store_Id { get; set; }
        public SelectList StoreList { get; set; }


    }
}