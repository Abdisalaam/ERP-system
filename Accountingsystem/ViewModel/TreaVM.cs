using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Accountingsystem.ViewModel
{
    public class TreaVM
    {
        public int Trea_Id { get; set; }

      
        public string Trea_Name { get; set; }

      
        public string Trea_Code { get; set; }


        public Nullable<int> selectedEmp_Id { get; set; }


        public SelectList EmployeeList { get; set; }
    
    
        public Nullable<int> selectedDep_Id { get; set; }


        public SelectList DepartmentList { get; set; }

  
       
        public string Trea_Desc { get; set; }
        
      
        public int SelectedChartOfAccountId { get; set; }
        public SelectList MyChartOfAccountList { get; set; }
        public decimal treasuryBallance { get; set; }
    }
}