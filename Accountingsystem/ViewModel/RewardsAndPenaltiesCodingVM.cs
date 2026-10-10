using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class RewardsAndPenaltiesCodingVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RewardsAndPenaltiesCodingCategoryId { get; set; }
        public SelectList RewardsAndPenaltiesCodingCategoryList { get; set; }

        public int DurationInDay { get; set; }
        public int DurationInhour { get; set; }
        public int DurationInMinute { get; set; }
        public int RewordInDay { get; set; }
        public int RewordinHour { get; set; }
        public int TypeOfDetailSalaryId { get; set; }
        public TypeOfDetailSalary typeOfDetailSalary{ get; set; }
        public RewardsAndPenaltiesCodingCategory rewardsAndPenaltiesCodingCategory { get; set; }
        public SelectList TypeOfDetailSalaryList { get; set; }
    }
}