using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace Accountingsystem.ViewModel
{
    public class InvoiceMasterDetailsVM
    {
        public int Details_Id { get; set; }


       
        public decimal Product_Quantity { get; set; }

   
     
        public decimal Product_Price { get; set; }

      
        public decimal Total_Amount { get; set; }


        public decimal Product_Tax { get; set; }

       
        public decimal Detail_Total_Tax { get; set; }      

      
   
        public int SelectedBtachtId { get; set; }
        public SelectList MyBatchsList { get; set; }


      
        public int InvoiceMaster_Id { get; set; }
        public SelectList ProducttaxList { get; set; }
        public int CustomerID { get; set; }
        public int store { get; set; }
        public int productID { get; set; }
        public decimal Master_Discount { get; set; }
        public int productNotBatched_ID { get; set; }
        public SelectList ProductNotBatchList { get; set; }
    }
}