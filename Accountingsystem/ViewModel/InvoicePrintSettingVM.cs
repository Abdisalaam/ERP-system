using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class InvoicePrintSettingVM
    {

        public int Id { get; set; }
        public string printWhite { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public int TypeId { get; set; }
        public string PrintLogo { get; set; }
        public bool Active { get; set; }
        public bool Package { get; set; }
        public string Description { get; set; }
    }

}