using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class SubLedgerVM
    {
        public int SubLed_Id { get; set; }
    
     
        public string SubLed_Code { get; set; }
     
        public string SubLed_Name { get; set; }
    
        public string SubLed_Date { get; set; }
        
    
       
     
        public string SubLed_Type { get; set; }
       
    
        public string SubLed_Desc { get; set; }

        public string InvoiceNotes { get; set; }


        public string InvoiceDescribtion { get; set; }
        public string OfferNotes { get; set; }
        public string OfferDesc { get; set; }

        public Nullable<decimal> SubledgerBalanceCredit { get; set; }
      
        public Nullable<decimal> SubledgerBalanceDebit { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
       
        public string OpenBalance_Code { get; set; }

        public Nullable <int> ParentSubledger_Id { get; set; }
      
        public int DepartSubledger_Id { get; set; }
     
        public int BussinesSubledger_Id { get; set; }
       
        public int BranchSubledger_Id { get; set; }
        public SelectList SubledgerList { get; set; }
        public SelectList BranchSubList { get; set; }
        public SelectList DepartSubList { get; set; }
        public SelectList BussinessList { get; set; }
    }
    }
