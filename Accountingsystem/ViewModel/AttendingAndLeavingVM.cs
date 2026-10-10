using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class AttendingAndLeavingVM
    {
        public List<AttendingAndLeavingDetailsVM> AttendingAndLeavingDetailsList { get; set; }
        public int ShiftCodingId { get; set; }
        public SelectList ShiftCodingList { get; set; }
        public  string Date { get; set; }
        public ShiftCoding shiftCoding { get; set; }
        public int BranchId { get; set; }
        public SelectList BranchList { get; set; }
        public string BranchName { get; set; }
    }
}