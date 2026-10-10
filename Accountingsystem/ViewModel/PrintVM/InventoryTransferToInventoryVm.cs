using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class InventoryTransferToInventoryVm
    {
        public string Date { get; set; }
        public decimal Total_Cost { get; set; }
        public string CurrencyId { get; set; }
        public string SelectedCompanyId { get; set; }
        public string store_Id { get; set; }
        public string Inv_Code { get; set; }
        public decimal TotalAmount { get; set; }
        public Company company { get; set; }
        public string StoreTo { get; set; }
        public string Decribtion { get; set; }

        public List<InventoryTranferDetailsVm> MyDetails { get; set; }

    }
}