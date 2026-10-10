using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class CostDistributionPrintVM
    {
      
        public int id { get; set; }
        public string Cost_Distr_Code { get; set; }
        
        public PurInvoiceMaster purchase { get; set; }

       
      
        public SubLedger subLedger { get; set; }
        public Company company { get; set; }
        public Supplier supplier { get; set; }
        public List<CostDistributionDetailPrintVM> detail { get; set; }
            public decimal total { get; set; }
        }


    }

