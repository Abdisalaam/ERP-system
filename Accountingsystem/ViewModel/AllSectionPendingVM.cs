using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class AllSectionPendingVM
    {
        public string Serial { get; set; }
        public int MasterID { get; set; }
        public int DebitAccountId { get; set; }
        public int CreditAccountId { get; set; }
        public int ChildAccountId { get; set; }
        public int TresuryOrBankId { get; set; }
        public int SupplierId { get; set; }
        public int CustomerId { get; set; }
        public bool treasuryCredit { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalAmountForChild { get; set; }
        public bool Checked { get; set; }
        [System.ComponentModel.DataAnnotations.DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime date { get; set; }
        public bool checkAll { get; set; }
        public TransactionState status { get; set; }
        
    }

}