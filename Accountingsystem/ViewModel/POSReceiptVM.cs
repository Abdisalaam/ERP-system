using Accountingsystem.Models;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
 
    public class POSReceiptVM
    {
        public int Rec_ID { get; set; }


        public int RecComp_ID { get; set; }
        public SelectList MyCompanyList { get; set; }


        public decimal Rec_Amount { get; set; }



        public Nullable<decimal> Exchange_Rate { get; set; }


        public string Rec_Number { get; set; }


        public int RecBank_Account { get; set; }



        public string Rec_Description { get; set; }



        //[DataType(DataType.Date)]
        public string RecGl_Date { get; set; }



        // [DataType(DataType.Date)]
        public string Rec_Date { get; set; }


        public TransactionState Rec_state { get; set; }


        //[Required(ErrorMessage = "تحديد النشاط من القائمه")]
        public int Rec_BU { get; set; }
        public SelectList MySubLdgerList { get; set; }



        public decimal TotalAmountAfMat { get; set; }

        //[DisplayName("رقم الشيك")]
        //[Required(ErrorMessage = "رقم الشيك")]
        //public string RecCheck_Number { get; set; }


        public int SelectedCurrencyId { get; set; }


        public SelectList MyCurrencyList { get; set; }


        public int SelectedCustomer { get; set; }


        public SelectList MyCustomerList { get; set; }

        public decimal CustomerTotalBalance { get; set; }

        public int SelectedChartOfAccountId { get; set; }
        public SelectList MyChartOfAccountList { get; set; }

        public Nullable<int> SelectedBranch_Id { get; set; }
        public SelectList BranchList { get; set; }

        public int SelectPayment_Method { get; set; }
        public SelectList PaymentMethodList { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }


        public int selectInvoiceID { get; set; }
        public SelectList InvoiceList { get; set; }

        public SelectList EmpList { get; set; }
        public int Emp_Id { get; set; }

    }
}