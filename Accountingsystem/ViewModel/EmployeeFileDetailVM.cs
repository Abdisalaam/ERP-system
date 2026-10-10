using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class EmployeeFileDetailVM
    {

        public int Id { get; set; }
        public int ContentLength { get; set; }
        public string FileName { get; set; }
        public byte[] ProImg { get; set; }
        public int EmployeeEmp_Id { get; set; }


    }
}