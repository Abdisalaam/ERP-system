using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class ShiftCodingVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int NumberOfHour { get; set; }
        public TimeSpan NumberOfHourPerDay { get; set; }
        public SelectList WeekHolidayList { get; set; }
        public List<string> WeekHollidays { get; set; }
        public decimal PercentageOfAdditionHours { get; set; }

        public List<EmployeeShiftCheckVM>EmployeeList{ get; set; }
    }
}