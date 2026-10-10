using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class PointOfSaleVM
    {
        public int POS_Id { get; set; }
      
        public string POS_Name { get; set; }
        public int Company_Id { get; set; }
        public int Currency_Id { get; set; }
  
        public string Adress { get; set; }
     
        public int Store_Id { get; set; }
      
        public int Employee_Id { get; set; }
        public SelectList CompanyList { get; set; }
        public SelectList StoreList { get; set; }
        public SelectList CurrencyList { get; set; }
      
       public SelectList EmpList { get; set; }


        public Nullable<int> Subledger_Id { get; set; }
        public Nullable<int> DepartSubledger_Id { get; set; }

        public Nullable<int> BussinesSubledger_Id { get; set; }
      
        public Nullable<int> BranchSubledger_Id { get; set; }
        public SelectList SubledgerList { get; set; }
        public SelectList BranchSubList { get; set; }
        public SelectList DepartSubList { get; set; }
        public SelectList BussinessList { get; set; }

    }
}