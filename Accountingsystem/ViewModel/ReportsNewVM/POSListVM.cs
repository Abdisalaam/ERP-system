using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class POSListVM
    {
        public Company company { get; set; }
        public Nullable< int> Pos_Id { get; set; }

        public SelectList POSList { get; set; }
        public string StartDate{ get; set; }

        public string End_Date { get; set; }

        public List<MasterPOSInvoice> AllInvoice { get; set; }


    }
}