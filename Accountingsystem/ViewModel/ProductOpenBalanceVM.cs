using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using Accountingsystem.Models;

namespace Accountingsystem.ViewModel
{
    public class ProductOpenBalanceVM
    {
    
        public int Prod_ID { get; set; }
        public SelectList ProductList { get; set; }

    
        public string Prod_Name { get; set; }

     
        public string Prod_Code { get; set; }
     
    
        public string Prod_Descrip { get; set; }


     

        public Nullable<int> SelectedCategory_ID { get; set; }
        public SelectList CategoryList { get; set; }
    
    
        public Nullable<int> MeasuringUnit_ID { get; set; }
        public int SelectedMeasuringUnit_ID { get; set; }
        public SelectList MeasruingUnitList { get; set; }
        public Nullable<int> Category_ID { get; set; }
       
        public string Prod_Mark { get; set; }
        
        public Nullable<decimal> Sale_Price { get; set; }
 
     
        public Nullable<int> Demand_Limit { get; set; }
     
        public int Store_Id { get; set; }
    
   
        public bool Prod_Stored { get; set; }
   
      
        public string Bar_Code { get; set; }
  
        public int? SelectedTaxId { get; set; }
  
        public SelectList MyTaxList { get; set; }
 
      
        public Product_Sale_Price Prod_Sale_Price { get; set; }
      
        public int SelectedStore_ID { get; set; }
    
        public SelectList StoreList { get; set; }
      
        public decimal Product_Quantity { get; set; }

        public decimal Product_Cost { get; set; }

      
        public string Batch_Date { get; set; }
  
      
        public string Batch_ExpireDate { get; set; }
  
        public decimal Batch_Cost { get; set; }
     
        public int Selectedcomp_ID { get; set; }
 
        public SelectList CompList { get; set; }
    
      
        public int SelectedCurrency_ID { get; set; }
       
        public SelectList CurrencyList { get; set; }
        public decimal Prod_Cost_Price { get; set; }
        public Nullable<Prod_Type> Prod_Type { get; set; }
 
        public string OpenBalance_Code { get; set; }

        public int? SelectedTax_Purch { get; set; }

 
        public SelectList MyTaxListPurchas { get; set; }
    }
}