using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Accountingsystem.Models;

namespace Accountingsystem.ViewModel
{
    public class EmpVM
    {
      
        public int Emp_Id { get; set; }
        public int SystemRole_ID { get; set; }
        public String Regist_Id { get; set; }
     
        public string Emp_Code { get; set; }
        public SelectList Employlist { get; set; }

  
        public string Emp_Name { get; set; }

 
        public string Emp_Address { get; set; }

      
        public string Emp_Mail { get; set; }



        public string Emp_Phone { get; set; }


        public string Emp_Title { get; set; }

  
        public string Emp_DOB { get; set; }

        public Nullable<int> SelectedBU_Id { get; set; }


        public SelectList BusinessUnit_List { get; set; }

       
        public Nullable<int> Branch_Id { get; set; }
        public SelectList Branch_List { get; set; }
   
        public string Residency_number { get; set; }
   
        public string Passport_number { get; set; }

    
        public string Start_Date_Passp { get; set; }
     
        public string End_Date_Passp { get; set; }

        public string Start_Date_Residency { get; set; }

        public string End_Date_Residency { get; set; }
        public bool ViewAllCustomer { get; set; }

        public Nullable<decimal> fixed_sallary { get; set; }
        public int vacation_Number { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        public HttpPostedFileBase Emp_Image { get; set; }
        //مركز التكلفة الجديد

        public Nullable<int> DepartSubledger_Id { get; set; }

        public Nullable<int> BussinesSubledger_Id { get; set; }
        public Nullable<int> Subledger_Id { get; set; }
        public Nullable<int> BranchSubledger_Id { get; set; }
        public SelectList SubledgerList { get; set; }
        public SelectList BranchSubList { get; set; }
        public SelectList DepartSubList { get; set; }
        public SelectList BussinessList { get; set; }

        public Boolean Emp_Subledger { get; set; }
        public bool User { get; set; }
        public bool Active { get; set; }
        public Nullable<int> Company_ID { get; set; }
        public SelectList compList { get; set; }
        public Nullable<Social_Status> Social_status { get; set; }
        public string Nationality { get; set; }
        public string Contract_Begin { get; set; }
        public string Contract_End { get; set; }
        public string Work_Actual_Start { get; set; }
        public Nullable<Type_of_work> Type_OF_Work { get; set; }
        public string Social_insurance_number { get; set; }
        public string Medical_Insurance_Number { get; set; }
        public string End_medical_Insurance { get; set; }
        public Nullable<Sex> Sex { get; set; }
        public Nullable<decimal> Total_sal { get; set; }
        public Boolean Check { get; set; }
        public Nullable<int> Vacation_Pervious { get; set; }
        public Nullable<int> TotalVacation { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string AccountNum { get; set; }
        public string EbanNumb { get; set; }
    }
}