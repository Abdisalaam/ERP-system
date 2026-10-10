using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class SewingVM
    {
        public int Id { get; set; }
        public int SelectedCustomerId { get; set; }
        public SelectList CustList { get; set; }
        public string Date { get; set; }
        public string DueDate { get; set; }
        public string Desc { get; set; }
        public string Notes { get; set; }
        public decimal Amount { get; set; }
        public List<Question> QuestionList { get; set; }

        public int ProductId { get; set; }
        public SelectList ProdductList { get; set; }
        public int NumberOfSewing { get; set; }
        public int order { get; set; }
        public int? iscopy { get; set; }
        public int TaxId { get; set; }
        public SelectList TaxList { get; set; }
        public decimal Quantity { get; set; }
        public DateTime Dated { get; set; }
        public DateTime DueDated { get; set; }
        public Customer customer { get; set; }
        public MasterPOSInvoice invoice { get; set; }
        public int orderNumber { get; set; }
        public SewingStatus status { get; set; }
    }
}