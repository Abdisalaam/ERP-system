using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class MaintenanceStatus
    {
        public static MaintenanceStatus getstatusById(int id)
        {
            var db = new AccountingContainer();
            var stat = db.MaintenanceStatus.Find(id);
            return stat;

        }


    }
}