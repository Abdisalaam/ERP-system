using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class ProjectDetailsVM
    {
        public int ProjectId { get; set; }
        public Company company { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public ProjectState Statu { get; set; }
        public decimal ProjectAMount { get; set; }
        public decimal PastPayedAmount { get; set; }
        public decimal PorjectInvAMount { get; set; }
        public decimal PayedAMount { get; set; }
        public decimal Balance { get; set; }
        public string ProjectStartDate { get; set; }
        public string ProjectEndDate { get; set; }

        public string From_Date { get; set; }
        public string To_Date { get; set; }
        public SelectList ProjectList { get; set; }

        public List<ProjectRecipt> projectReciptsList { get; set; }
        public List<ProjectInvoiceMaster> projectInvoiceMastersList { get; set; }

        public List<ProjectDetailsGLVM> ProjectDetailsGLVMs { get; set; }

        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }

        public decimal ReceivedAmount { get; set; }
    }
}