using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Models;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class SubscriptionTeamVM
    {
        public string startDate { get; set; }

        public string EndDate { get; set; }

        public int SupTeam { get; set; }
        public string SupTeamName { get; set; }

        public SelectList MySupTeamList { get; set; }

        public List<Subscription> Subscrib { get; set; }
        public Company company { get; set; }
    }
}