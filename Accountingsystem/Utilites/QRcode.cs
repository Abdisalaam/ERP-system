using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using ZXing;
using ZXing.QrCode;

namespace Accountingsystem.Utilites
{
    public class QRcode
    {
        public static byte[] MakeBarcode (string code )
        {
           
                    //var codeWriter = new BarcodeWriterPixelData
                    //{
                    //    Format = BarcodeFormat.QR_CODE,
                    //    Options = new QrCodeEncodingOptions
                    //    {
                    //        Height = 50,
                    //        Width = 50,
                    //        Margin = 0
                    //    }

                    //}
                    //;
         
           


            
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
               
            };
            var bitmap = writer.Write(code);
            Bitmap bmp = bitmap;

            //RectangleF rectf = new RectangleF(70, 90, 90, 50);

            //Graphics g = Graphics.FromImage(bmp);

            //g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //g.DrawString("yourText", new Font("Tahoma", 40), Brushes.Black, rectf);

            //g.Flush();

            
            ImageConverter converter = new ImageConverter();
            var a= (byte[])converter.ConvertTo(bmp, typeof(byte[]));
            
            return a;
        }
        public static byte[] MakeQRcode(int code, string currentUrl)
        {


            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,

            };
          
            var db = new AccountingContainer();
            var masterqr = db.MasterQRInvoices.Find(code);
            if (masterqr.QrGuid == null)
            {
                masterqr.QrGuid = Guid.NewGuid();
                db.Entry(masterqr).State = EntityState.Modified;
                db.SaveChanges();
            }
            var splitter = currentUrl.Split('/');
            string domain = "https://www.mydataitc.com/";
            string client = splitter[3];
            string controllerName = "/InvoiceQRExternalUseres";
            var cust = db.Customers.Find(masterqr.Customer_Id);
            string actionName = "";
            if (cust != null && cust.Customer_TaxNumber != null)
            {
                actionName = "/Details/" + masterqr.QrGuid.ToString();
               
            }
            else
            {
                actionName = "/Detailsimple/" + masterqr.Id;
            }
            string url = domain + client + controllerName + actionName;

            var bitmap = writer.Write(url);
            Bitmap bmp = bitmap;



            ImageConverter converter = new ImageConverter();
            var a = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

            return a;
        }

        public static byte[] MakeQRcodeOffer(int code, string currentUrl)
        {


            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,

            };

            var db = new AccountingContainer();
          
            var splitter = currentUrl.Split('/');
            string domain = "https://www.mydataitc.com/";
            string client = splitter[3];
            string controllerName = "/Reports";
            string actionName = "/PrintOfferPriceNew/" + code.ToString();
            string url = domain + client + controllerName + actionName;

            var bitmap = writer.Write(url);
            Bitmap bmp = bitmap;



            ImageConverter converter = new ImageConverter();
            var a = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

            return a;
        }
        public static byte[] MakeQRcodeRecipt(int code, string currentUrl)
        {


            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,

            };

            var db = new AccountingContainer();

            var splitter = currentUrl.Split('/');
            string domain = "https://www.mydataitc.com/";
            string client = splitter[3];
            string controllerName = "/Reports";
            string actionName = "/PrintReceipt/" + code.ToString();
            string url = domain + client + controllerName + actionName;

            var bitmap = writer.Write(url);
            Bitmap bmp = bitmap;



            ImageConverter converter = new ImageConverter();
            var a = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

            return a;
        }
        public static byte[] MakeQRcodePayment(int code, string currentUrl)
        {


            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,

            };

            var db = new AccountingContainer();

            var splitter = currentUrl.Split('/');
            string domain = "https://www.mydataitc.com/";
            string client = splitter[3];
            string controllerName = "/Reports";
            string actionName = "/PrintPayment/" + code.ToString();
            string url = domain + client + controllerName + actionName;

            var bitmap = writer.Write(url);
            Bitmap bmp = bitmap;



            ImageConverter converter = new ImageConverter();
            var a = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

            return a;
        }
    }
}