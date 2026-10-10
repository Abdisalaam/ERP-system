using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class DepartmentBussinesUnitVM
    {
        public int BussinessUnit_Id { get; set; }
   
        public string BussinesUnit_Name { get; set; }
        public string BussinesUnit_Description { get; set; }
        public int BranchDepartmentDept_Id { get; set; }
        public SelectList departList { get; set; } 
    }
}