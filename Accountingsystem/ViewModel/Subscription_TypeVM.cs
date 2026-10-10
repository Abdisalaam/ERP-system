using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Accountingsystem.ViewModel
{
    public class Subscription_TypeVM
    {
     

        public int Subscription_Type_ID { get; set; }
   
        public string Subscrip_Code { get; set; }
   
        public string Subscrip_Name { get; set; }
    

        public string Subscrip_Description { get; set; }
  
        public int Durationsubscription { get; set; }
  

        public int SelectedSubscrip_BU { get; set; }
        public SelectList MyBuList { get; set; }
     

        public int SelectedSubscrip_Comp { get; set; }
        public SelectList MyCompList { get; set; }
       

        public int SelectedRvenuo_ID { get; set; }
     
        public SelectList MyRvenuoList { get; set; }



    }
}