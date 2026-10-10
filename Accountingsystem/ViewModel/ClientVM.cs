using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class ClientVM
    {
        public string Client_Name { get; set; }
        public Nullable<int> Package_Id { get; set; }
        public SelectList PackageList { get; set; }
        public int Client_Id { get; set; }
        public string EndDate { get; set; }

    }
}