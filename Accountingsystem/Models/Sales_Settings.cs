using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.Entity.Validation;

namespace Accountingsystem.Models
{
    public partial class Sales_Settings
    {
        /// <summary>
        /// get all List Setting 
        /// </summary>
        /// <returns> List<Sales_Settings> </returns>
        public static List<Sales_Settings> getAllSettings()
        {
            var db = new AccountingContainer();
            var allsetting = db.Sales_Settings.ToList();

            return allsetting;
        }
        /// <summary>
        /// get All Sales setting for that Employee by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> List<Sales_Settings></returns>
        public static List<Sales_Settings> getallSalesSetingForEmployee(int id)
        {
            var db = new AccountingContainer();
            var list = db.Sales_Settings.Where(a => a.Employee_ID == id).ToList();
            return list;

        }
        /// <summary>
        /// get sales Setting slice by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List<Sales_Setting_Slice></returns>
        public static List<Sales_Setting_Slice> Getslicess(int id)
        {
            var db = new AccountingContainer();
            var list = db.Sales_Setting_Slice.Where(a => a.Sales_SettingsId == id).ToList();
            if (list == null)
            { List<Sales_Setting_Slice> ss = new List<Sales_Setting_Slice>();
                return ss;
            }
            else
            { return list; }
            

        }

    }
}