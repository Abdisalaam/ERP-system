using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace Accountingsystem.ViewModel
{
    public class GLMasterVM
    {
    
        public int GLMaster_Id { get; set; }


    
        public string GL_Date { get; set; }

      

        public string GL_Desc { get; set; }
     
        public int Company_Id { get; set; }
        public decimal debit { get; set; }
        public decimal Credit { get; set; }

        public int GLType_Id { get; set; }
    
        public int Currency_Id { get; set; }

        public int SubLed_Id { get; set; }
  

        public SelectList SubLedgerList { get; set; }


        public SelectList CompanyList { get; set; }
        public SelectList GLTypeList { get; set; }
        public SelectList CurrencyList { get; set; }
        public List<GLDetailVM> detailForThatMaster { get; set; }


        public int Account_Child_ID { get; set; }
        public SelectList childList { get; set; }
        public int selectedAccount { get; set; }
       

        public SelectList accountList { get; set; }

        public int selctedBu { get; set; }
 

        public SelectList BUList { get; set; }

        public int SelectAsset { get; set; }
        public SelectList AssetList { get; set; }
        public Nullable<decimal> Exchange_Rate { get; set; }

        public string GLCode { get; set; }

        public String AccountName { get; set; }

        public int AccountSettlement { get; set; }
        public decimal RealBallance { get; set; }

        public SelectList AccountSettlementList { get; set; }
    }
}