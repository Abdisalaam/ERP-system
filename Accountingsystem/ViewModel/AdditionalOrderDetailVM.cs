using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class AdditionalOrderDetailVM
    {
        public int OrderDetailId { get; set; }
        public string AdditionalDetail { get; set; }
        public int SelectedAdditionalDetail { get; set; }
        public SelectList AdditionalDetailList { get; set; }

    }
}