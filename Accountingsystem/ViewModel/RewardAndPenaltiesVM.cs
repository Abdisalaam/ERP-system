using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class RewardAndPenaltiesVM
    {
        public RewardsAndPenalties reward { get; set; }
        public int  hourOFPenaltiy { get; set; }
        public string Description  { get; set; }
    }
}