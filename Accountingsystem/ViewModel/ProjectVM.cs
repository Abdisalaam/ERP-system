using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Accountingsystem.Models;

namespace Accountingsystem.ViewModel
{
    public class ProjectVM
    {
        public int Project_Id { get; set; }
  
        public string Project_Code { get; set; }
      
        public string Project_Name { get; set; }


      
        public string Project_Description { get; set; }


        public string Project_Location { get; set; }
     
       
        public int CustomerCustomer_Id { get; set; }
     
      
        public string StartTime { get; set; }
    
        public string deadline { get; set; }
    
        public decimal Estimate_Amount { get; set; }
        
        public SelectList CustomerList { get; set; }
        public int SelectedChartOfAccountId { get; set; }
        public SelectList MyChartOfAccountList { get; set; }
       
      
        public int SelectedEmpId { get; set; }
        public SelectList MyEmpList { get; set; }
       
        
        public int SelectedReveId { get; set; }
        
        public SelectList MyRevenuelList { get; set; }

      
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

       
     
        public Nullable<int> Payment_Method { get; set; }
     
        public SelectList MyPaymentPolicy { get; set; }
   
        public ProjectState Project_State { get; set; }
  

        public Nullable<int> DepartSubledger_Id { get; set; }

        public Nullable<int> BussinesSubledger_Id { get; set; }
        public Nullable<int> Subledger_Id { get; set; }
        public Nullable<int> BranchSubledger_Id { get; set; }
        public Nullable<int> Company_ID { get; set; }
        public SelectList CompList { get; set; }
        public SelectList SubledgerList { get; set; }
        public SelectList BranchSubList { get; set; }
        public SelectList DepartSubList { get; set; }
        public SelectList BussinessList { get; set; }
      
        public Nullable< int> SelectEmp_Team_Id { get; set; }

        public SelectList MyEmpTeam { get; set; }
        public string notes { get; set; }
        public string detailOfProject { get; set; }
    }
}