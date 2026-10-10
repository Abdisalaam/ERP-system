using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class printGlDetailsVM
    {
        public int GLDetail_Id { get; set; }
        public decimal GLDetail_Debit { get; set; }
        public decimal GLDetail_Credit { get; set; }
        public decimal totaDebit { get; set; }
        public decimal totaCredit { get; set; }

    }
}