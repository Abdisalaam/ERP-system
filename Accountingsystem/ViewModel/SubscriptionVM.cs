using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Accountingsystem.ViewModel
{
    public class SubscriptionVM
    {
    
        public int Subscrip_Id { get; set; }
       
        public string Subscrip_Code { get; set; }
 
        public string Customer_Name { get; set; }
        public string Subscrip_Name { get; set; }
       
        public string Subscrip_Description { get; set; }
       
       
        public string DeadLine { get; set; }
    
        public string Sub_Date { get; set; }
  
        public string StartTime { get; set; }
      
        public decimal Estimate_Amount { get; set; }
   
        public int Customer_Id { get; set; }
   
        public SelectList MyCusromerList { get; set; }
    

        //public int Durationsubscription { get; set; }
        public TransactionState Subscrip_State { get; set; }


     
        public int SelectedSubscrip_Type { get; set; }

     
        public Nullable<int> SelectEmp_Team { get; set; }
        public SelectList MyEmpTeam { get; set; }
       

        public SelectList MySubTypeList { get; set; }
        public SubType_State SubType_State { get; set; }
        public Nullable<int> SelecltPayment_Method { get; set; }
        public SelectList MyPayMethod { get; set; }
        public SelectList MySupscrList { get; set; }

        public string tel_customer { get; set; }
        public Nullable<System.DateTime> Subscrip_DateEnd { get; set; }
        public int Sub_Type_ID { get; set; }
        public string Check_Number { get; set; }
        public Nullable<int> Taxe_ID { get; set; }
        public SelectList tax_List { get; set; }

       

     
        public int Comp_Id { get; set; }

   
        public SelectList MyCompList { get; set; }
 
        public Nullable<int> Subscrip_BU { get; set; }

        public SelectList MyBuList { get; set; }
        public Nullable<int> Subscrip_Pranch { get; set; }
        public SelectList MySubscripPranch { get; set; }
        public Nullable<int> Subledger_ID { get; set; }
        public SelectList MySubList { get; set; }

        public Nullable<int> Department_Id { get; set; }
        public SelectList MyDepartmentList { get; set; }
        public Nullable<bool> sub_Stop { get; set; }
        public Nullable<decimal> AMountAFterTax { get; set; }
        public decimal totalinPageonly { get; set; }
        public Nullable<decimal> Payed_Amount { get; set; }
       
        public decimal Discount { get; set; }
        public decimal AllowDiscount { get; set; }

  
        public Nullable<int> Emp_Id { get; set; }
        public SelectList EmpList { get; set; }
    }

}
