using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class StoreVM
    {
        public int Store_ID { get; set; }

       
        public string Store_Code { get; set; }

 
        public string Store_Name { get; set; }


      
        public string Store_Address { get; set; }
        public SelectList MyChartOfAccountList { get; set; }
      
        public int SelectedChartOfAccountId { get; set; }
        public SelectList BranchList { get; set; }

       
        public Nullable<int> SelectedEmployeeId { get; set; }
        public SelectList EmployeeList { get; set; }

        public SelectList EmPloyeeMemberList { get; set; }
        public List<string> EmployeesMember { get; set; }

        public Nullable<int> SelectedBranshId { get; set; }
      
    }
}