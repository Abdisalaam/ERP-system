using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class InvoiceSerial
    {

        public static string generateSerialForInvoice()
        {
            var db = new AccountingContainer();
            var Package = db.Packages.FirstOrDefault();
            var lastSerial= db.InvoiceSerials.ToList();
            lastSerial.Reverse();
            int serial = 0;

            if (lastSerial.FirstOrDefault() != null)
            {
               
                serial = lastSerial.FirstOrDefault().Number + 1;
              
            }
            else
            {
                serial = db.InvoiceMasters.Count() + db.SalesReceiptMasters.Count() + db.Orders.Count() + db.MasterPOSInvoices.Count() + db.ProjectInvoiceMasters.Count() + db.Subscriptions.Count();
                if (serial != 0)
                {
                  
                    serial = serial + 1;
                 
                }
                else if (Package.NumberOfInvoice != null&&Package.NumberOfInvoice != 0)
                {
                    serial = Package.NumberOfInvoice.Value;
                }
                else
                {
                    serial = 1;
                }
                
               
            }
            var ser = new InvoiceSerial();
            ser.Number = serial;
            db.InvoiceSerials.Add(ser);
            db.SaveChanges();
            return Package.StartOfInvoice + serial;
        }
        public static string generateSerialForReverse()
        {
            var db = new AccountingContainer();
            var Package = db.Packages.FirstOrDefault();
            var lastSerial = db.ReveseInvoiceSerials.ToList();
            lastSerial.Reverse();
            int serial = 0;

            if (lastSerial.FirstOrDefault() != null)
            {

                serial = lastSerial.FirstOrDefault().Number + 1;

            }
            else
            {
                serial = db.ReverseOrders.Count() + db.ReversePOSInvoices.Count() + db.ReverseSalesInvoiceMasters.Count() + db.ReverseSalesReceiptMasters.Count();
                if (serial != 0)
                {

                    serial = serial + 1;

                }
                else if (Package.NumberOfReverse != null&& Package.NumberOfReverse != 0)
                {
                    serial = Package.NumberOfReverse.Value;
                }
                else
                {
                    serial = 1;
                }


            }
            var ser = new ReveseInvoiceSerial();
            ser.Number = serial;
            db.ReveseInvoiceSerials.Add(ser);
            db.SaveChanges();
            return Package.StartOfInReverse + serial;
        }
 
    }
}