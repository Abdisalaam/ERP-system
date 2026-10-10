using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace Accountingsystem.ViewModel
{
    public class CustomerVM
    {

        public int _Customer_Id { get; set; }

        public int? CustEmployeeId { get; set; }
        public SelectList EmployeeList { get; set; }
        public string Customer_Name { get; set; }

        public string Customer_Address { get; set; }
        public string Comment { get; set; }


        public string Customer_Phone { get; set; }


        public string Customer_Mail { get; set; }

        public Nullable<int> SelectedCountryId { get; set; }
        public SelectList MyCountryList { get; set; }

        public Nullable<int> SelectedProvinceId { get; set; }
        public SelectList MyProvinceList { get; set; }

        public Nullable<int> SelectedCityId { get; set; }
        public SelectList MyCitiesList { get; set; }


        public string Customer_Fax { get; set; }


        public string Customer_Code { get; set; }

        public int SelectedChartOfAccountId { get; set; }
        public string Customer_Mobile { get; set; }
        public string customer_TaxNumber { get; set; }

        public Nullable<int> Payment_Policy_Id { get; set; }
        public SelectList MyPaymentPolicy { get; set; }
        public string Customer_Website { get; set; }

        public Nullable<int> CustomerBranch_Id { get; set; }
        public SelectList BranchList { get; set; }


        public SelectList MyChartOfAccountList { get; set; }
        public Nullable<decimal> Customer_Balance { get; set; }
        public string CommercialRegistrationNo { get; set; }
        public string IdentificationNumber { get; set; }
        public int CustomerSelectSlice_Id { get; set; }
        public SelectList SlicesList { get; set; }
        public HttpFileCollectionBase customerFiles { get; set; }
        public IEnumerable<HttpPostedFileBase> customerFiles2 { get; set; }

   
        public SlicesVM slice { get; set; }

        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
    }
}