using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Accountingsystem.Models;

namespace Accountingsystem.ViewModel
{
    public class ProductVM
     {
      
        public HttpFileCollectionBase Pro_Img { get; set; }
        public int Prod_ID { get; set; }

    
        public string Prod_Name { get; set; }
        public string Notess { get; set; }
   
        public string Prod_Code { get; set; }

        public string Prod_Descrip { get; set; }

    

        public Nullable<int> SelectedCategory_ID { get; set; }
        public SelectList CategoryList { get; set; }
     

        public Nullable<int> SelectedMeasuringUnit_ID { get; set; }
        public SelectList MeasruingUnitList { get; set; }     
     
       
        public string Prod_Mark { get; set; }
    
    
        public decimal Sale_Price { get; set; }
        public Nullable<int> Demand_Limit { get; set; }
  
        public int Store_Id { get; set; }

        public bool Prod_Stored { get; set; }
        public string Bar_Code { get; set; }
     
      
   
        public decimal Prod_cost_Price { get; set; }
        public int? SelectedTaxId { get; set; }



       
        public SelectList MyTaxList { get; set; }
        public int? SelectedSlieceID { get; set; }
   
        public SelectList MySliceList { get; set; }

        public Nullable<Prod_Type> Prod_Type { get; set; }

        public string Prod_Category { get; set; }
        public string Prod_Unet { get; set; }

        public int? SelectedTax_Purch { get; set; }



       
        public SelectList MyTaxListPurchas { get; set; }
        public int Slieces_Id { get; set; }
        public int quantity { get; set; }
        public decimal Product_Quantity { get; set; }
        public Nullable<int> Validity { get; set; }
        public IEnumerable<HttpPostedFileBase> Pro_Img2 { get; set; }
    }
}