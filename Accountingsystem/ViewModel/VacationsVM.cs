using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class VacationsVM
    {
        public int Vacation_Id { get; set; }
       
        public string Start_Date_Vacations { get; set; }
   
        public string End_Date_Vacations { get; set; }
        public int Employee_Id { get; set; }
        public SelectList EmpList { get; set; }
        public SelectList VacationTypeList { get; set; }
       
        public int Vacation_Type_Id { get; set; }
        public int NumberOfDay { get; set; }
  
        public bool Status { get; set; }
        public string Reason { get; set; }
        public string MangerDescription { get; set; }
       
        public HttpPostedFileBase Attachment { get; set; }
        public byte[] AttachmentByte { get; set; }
        public SelectList StatusList { get; set; }
    }
}