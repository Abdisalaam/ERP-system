using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class CachierOpenAndCloseShiftVM
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public System.DateTime OpenDate { get; set; }
        public System.DateTime CloseDate { get; set; }
    }
}