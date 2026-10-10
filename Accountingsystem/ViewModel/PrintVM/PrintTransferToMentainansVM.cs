using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintTransferToMentainansVM
    {
        public int Master_Id { get; set; }
        public string Trans_Code { get; set; }
        public string Company_Id { get; set; }
        public decimal Total_Amount { get; set; }
        public string Trans_Date { get; set; }
        public List<PrintTransferMentainansDetailsVM> MyDetails { get; set; }
        public Company company { get; set; }
        public int StoreId { get; set; }
        public string describtion { get; set; }
        public string ExpenseAccountName { get; set; }
        public string StoreName { get; set; }
    }
}