using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class AttendingAndLeavingDetailsVM
    {
        public int Id { get; set; }
        public int EmployeeEmp_Id { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int ShiftCodingId { get; set; }
        public Employee Employee { get; set; }
        public AttendingAndLeaving Attending { get; set; }
        public bool Active { get; set; }


    }
}