using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class Sales_SettingsVM
    {
        public int Id { get; set; }
        public int Employee_ID { get; set; }
        public int sliceID { get; set; }
        public SelectList SlicesList { get; set; }
        public bool SalesMan { get; set; }
        public bool Collector { get; set; }
        public bool Cant_Controle { get; set; }
        public bool Can_Increase_Price { get; set; }
        public bool Can_Decrease_Price { get; set; }
        public bool Can_Sell_InLow { get; set; }
    }
}