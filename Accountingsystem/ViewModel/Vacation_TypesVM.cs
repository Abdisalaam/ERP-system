using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class Vacation_TypesVM
    {
      
        public int Id { get; set; }
   
        public string Vacation_Type { get; set; }
        
       
      
       
        public decimal penalityPercentage { get; set; }
        
        public decimal PenalityPercentageAfter { get; set; }
      
        public decimal PenalityDay { get; set; }
      
        public decimal PenalityDayAfter { get; set; }
    
        public bool TransferToNewYear { get; set; }
    }
}