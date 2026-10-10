using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using ZXing;
using ZXing.Common;

namespace Accountingsystem.Models
{
    public class TLVCls
    {
        byte[] Seller;
        byte[] VatNo;
        byte[] dateTime;
        byte[] Total;
        byte[] Tax;

        public TLVCls(String Seller, String TaxNo, string dateTime, Double Total, Double Tax)
        {
            this.Seller = Encoding.UTF8.GetBytes(Seller);
            this.VatNo = Encoding.UTF8.GetBytes(TaxNo);

            this.dateTime = Encoding.UTF8.GetBytes(dateTime.ToString());
            this.Total = Encoding.UTF8.GetBytes(Total.ToString());
            this.Tax = Encoding.UTF8.GetBytes(Tax.ToString());
        }

        private String getasText(int Tag, byte[] Value)
        {
            return (Tag).ToString("X2") + (Value.Length).ToString("X2") + BitConverter.ToString(Value).Replace("-", string.Empty);
        }

        private byte[] getBytes(int id, byte[] Value)
        {
            byte[] val = new byte[2 + Value.Length];
            val[0] = (byte)id;
            val[1] = (byte)Value.Length;
            Value.CopyTo(val, 2);
            return val;
        }

        private String getString()
        {
            String TLV_Text = "";
            TLV_Text += this.getasText(1, this.Seller);
            TLV_Text += this.getasText(2, this.VatNo);
            TLV_Text += this.getasText(3, this.dateTime);
            TLV_Text += this.getasText(4, this.Total);
            TLV_Text += this.getasText(5, this.Tax);
            return TLV_Text;
        }

        public override string ToString()
        {
            return this.getString();
        }

        public String ToBase64()
        {
            List<byte> TLV_Bytes = new List<byte>();
            TLV_Bytes.AddRange(getBytes(1, this.Seller));
            TLV_Bytes.AddRange(getBytes(2, this.VatNo));
            TLV_Bytes.AddRange(getBytes(3, this.dateTime));
            TLV_Bytes.AddRange(getBytes(4, this.Total));
            TLV_Bytes.AddRange(getBytes(5, this.Tax));
            return Convert.ToBase64String(TLV_Bytes.ToArray());
        }

        public Bitmap toQrCode(int width = 250, int height = 250)
        {

            BarcodeWriter barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = width,
                    Height = height
                }
            };
            Bitmap QrCode = barcodeWriter.Write(this.ToBase64());

            return QrCode;
        }
        public static byte[] GenerateInvoiceQR(int invoiceqrId)
        {
            var db = new AccountingContainer();
            var invoice = db.MasterQRInvoices.Find(invoiceqrId);
            var company = db.Companies.FirstOrDefault();
            string Seller =company.Comp_Name ;
            if (invoice.Branch_Id > 0)
            {
                Seller = db.SubLedgers.Find(invoice.Branch_Id).SubLed_Name;
            }
            string VatNo = company.Tax_Number;
            if (company.Tax_Number == null)
            {
                VatNo = "   ";
            }
            string dTime = "";
            if (invoice.DateOfCreate != null)
            {
                dTime = invoice.DateOfCreate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            }
            else
            {
                dTime = invoice.Invoice_Date.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            }
            Double Total = Convert.ToDouble(Math.Round(invoice.Total_Amount - invoice.ReverseQRInvoices.Where(a => a.Invoice_State == TransactionState.Post).Select(a => a.Total_Amount).ToList().Sum(), 2, MidpointRounding.AwayFromZero));
            Double Tax = Convert.ToDouble(Math.Round(invoice.Total_Tax + invoice.TotalNewTaxs - (invoice.ReverseQRInvoices.Where(a => a.Invoice_State == TransactionState.Post).Select(a => a.Total_Tax).ToList().Sum()), 2, MidpointRounding.AwayFromZero));

            TLVCls tlv = new TLVCls(Seller, VatNo, dTime, Total, Tax);

            // Hex_txt.Text = tlv.ToString();
          var based = tlv.ToBase64();

            var imag = tlv.toQrCode();

            //Qr_Box.Image = tlv.toQrCode();
            ImageConverter converter = new ImageConverter();
            var z = (byte[])converter.ConvertTo(imag, typeof(byte[]));
            return z;
            //return imag;
        }
        public static string GenerateInvoiceQRbase(int invoiceqrId)
        {
            var db = new AccountingContainer();
            var invoice = db.MasterQRInvoices.Find(invoiceqrId);
            var company = db.Companies.FirstOrDefault();
            string Seller = company.Comp_Name;
            if (invoice.Branch_Id > 0)
            {
                Seller = db.SubLedgers.Find(invoice.Branch_Id).SubLed_Name;
            }
           
            string VatNo = company.Tax_Number;
            if (company.Tax_Number==null)
            {
                VatNo = "   ";
            }
            string dTime = "";
            if (invoice.DateOfCreate != null)
            {
                dTime = invoice.DateOfCreate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            }
            else
            {
                 dTime = invoice.Invoice_Date.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            }
            Double Total = Convert.ToDouble(Math.Round(invoice.Total_Amount - invoice.ReverseQRInvoices.Where(a => a.Invoice_State == TransactionState.Post).Select(a => a.Total_Amount).ToList().Sum(), 2, MidpointRounding.AwayFromZero));
            Double Tax = Convert.ToDouble(Math.Round(invoice.Total_Tax + invoice.TotalNewTaxs - (invoice.ReverseQRInvoices.Where(a=>a.Invoice_State==TransactionState.Post).Select(a => a.Total_Tax).ToList().Sum()), 2, MidpointRounding.AwayFromZero));

            TLVCls tlv = new TLVCls(Seller, VatNo, dTime, Total, Tax);

            // Hex_txt.Text = tlv.ToString();
            var based = tlv.ToBase64();

            var imag = tlv.toQrCode();

            //Qr_Box.Image = tlv.toQrCode();
            ImageConverter converter = new ImageConverter();
            var z = (byte[])converter.ConvertTo(imag, typeof(byte[]));
            return Convert.ToBase64String(z);
            //return imag;
        }

    }

}