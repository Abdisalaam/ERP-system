using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class MasterQRInvoice
    {
        public static MasterQRInvoice GetMasterQRInvoiceById(string id)
        {
            var db = new AccountingContainer();
            var invoice = db.MasterQRInvoices.Where(a => a.QrGuid.ToString() == id).FirstOrDefault();
            return invoice;
        }
        public static MasterQRInvoice GetMasterQRInvoiceById(int id)
        {
            var db = new AccountingContainer();
            var invoice = db.MasterQRInvoices.Find(id);
            if (invoice.Serial_Number.StartsWith("PINV-")&&invoice.Branch_Id==null)
            {
                var invo = db.ProjectInvoiceMasters.Where(a => a.Serial_Number == invoice.Serial_Number).FirstOrDefault();
                var poj = db.Projects.Find(invo.Project_Id);
                invoice.Branch_Id = poj.BranchSubledger_Id;
            }
            return invoice;
        }
        public static ReverseQRInvoice GetReversQRInvoiceById(string serial)
        {
            var db = new AccountingContainer();
            var invoice = db.ReverseQRInvoices.Where(a => a.Serial_Number == serial).OrderByDescending(s => s.Master_Id).FirstOrDefault();

            return invoice;
        }
        public static MasterQRInvoice GetQRMasterByGLCode(string Code)
        {
            var db = new AccountingContainer();
            var invoice = db.MasterQRInvoices.Where(a => a.Serial_Number == Code||a.Number==Code).FirstOrDefault();
            return invoice;
        }
    }
}