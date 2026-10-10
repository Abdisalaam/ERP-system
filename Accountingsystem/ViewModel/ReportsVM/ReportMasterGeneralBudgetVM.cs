using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class ReportMasterGeneralBudgetVM
    {
       


        public String StartDate { get; set; }

        public String EndDate { get; set; }
        public Boolean WithOutOFFGLs { get; set; }
        public List<ChartOfAccountingVM> AssetList { get; set; }
        public List<ChartOfAccountingVM> Libality { get; set; }
        public List<ChartOfAccountingVM> PropertyRights { get; set; }
        public decimal Profit { get; set; }
        public decimal LibalityAndPropertyRightsAndProfit { get; set; }
        public Company company { get; set; }

    }
}