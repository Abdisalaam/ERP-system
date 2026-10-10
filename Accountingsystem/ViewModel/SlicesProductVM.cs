using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class SlicesProductVM
    {
        public int SlicesProduct_Id { get; set; }
        public decimal Slice_Price { get; set; }
        public int Product_ID { get; set; }
        public int SlicesSlice_Id { get; set; }
       
        public SelectList MySliceList { get; set; }
    }
}