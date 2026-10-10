using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Accountingsystem.ViewModel
{
    public class BranchDeptVM
    {

        public int Dept_Id { get; set; }

     
        public string Dept_Name { get; set; }



        public string Dept_Phone { get; set; }


    
        public string Dept_Description { get; set; }


        public int BranchBranch_Id { get; set; }

    }
}