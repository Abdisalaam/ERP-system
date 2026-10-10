using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class GlMasterDetaislVM
    {
        public int GLMaster_Id { get; set; }
        public string Currency_Id { get; set; }
        public string GLCode { get; set; }
        public DateTime GL_Date { get; set; }
        public int Company_Id { get; set; }
        public decimal totaDebit { get; set; }
        public decimal totaCredit { get; set; }
        public List<printGlDetailsVM> myDetauls { get; set; }
        public Company company { get; set; }

    }
}