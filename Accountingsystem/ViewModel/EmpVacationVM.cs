using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class EmpVacationVM
    {
        public int Id { get; set; }
        public int Vacation_TypesId { get; set; }
        public int Employee_Id { get; set; }
        public int DaysInyear { get; set; }
        public int TotalAfterAbsent { get; set; }
        public SelectList VacationTypeList { get; set; }
    }
}