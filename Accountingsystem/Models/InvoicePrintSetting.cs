using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class InvoicePrintSetting
    {
        public static List<InvoicePrintSetting> getAllInvoicePrint()
        {
            var db = new AccountingContainer();
            var invoiceSetting = db.InvoicePrintSettings.ToList();
            return invoiceSetting;


        }

        public static List<InvoicePrintSetting> getAllInvoicePrintForCompanyClient(int type)
        {
            var db = new AccountingContainer();
            var invoiceSetting = db.InvoicePrintSettings.Where(a=>a.TypeId== type&&a.Active==true).ToList();
            var package = db.Packages.FirstOrDefault();
            if (package.PrintSpecial == false)
            {
                invoiceSetting = invoiceSetting.Where(a => a.Package == false).ToList();
            }

            return invoiceSetting;


        }
       public static InvoicePrintSetting getCompanyPrint(int type)
        {
            var db = new AccountingContainer();
            var com = db.Companies.FirstOrDefault();
            var invoiceSetting = db.InvoicePrintSettings.Where(a => a.TypeId == type).ToList();
            if (type == 1)
            {
                if (com.InvoicePrintOrdanary > 0)
                {
                    return invoiceSetting.Where(a => a.Id == com.InvoicePrintOrdanary).FirstOrDefault();
                }
                else
                {
                    return invoiceSetting.Where(a => a.TypeId == 1).FirstOrDefault();
                }
            }
            else if (type == 2)
            {
                if (com.InvoicePrintSimple > 0)
                {
                    return invoiceSetting.Where(a => a.Id == com.InvoicePrintSimple).FirstOrDefault();
                }
                else
                {
                    return invoiceSetting.Where(a => a.TypeId == 2).FirstOrDefault();
                }
            }
           else if (type == 3)
            {
                if (com.InvoicePrintOfferPrice > 0)
                {
                    return invoiceSetting.Where(a => a.Id == com.InvoicePrintOfferPrice).FirstOrDefault();
                }
                else
                {
                    return invoiceSetting.Where(a => a.TypeId == 3).FirstOrDefault();
                }
            }
            return null;
        }
        public static InvoicePrintSetting GetCategoryById(int id)
        {
            var db = new AccountingContainer();
            var model = db.InvoicePrintSettings.Find(id);
            return model;
        }

        // use this method to   Get one element CategoryByCode from Database.  
     

        // use this method For Creation new Catagery and get Data from user.
        public static InvoicePrintSettingVM CreateVMForCreation()
        {
            var db = new AccountingContainer();
            var vm = new InvoicePrintSettingVM();


        
       
            return vm;
        }

        // use this method to  Create New Catagery and save it in DataBase.
        public static bool CreateInvoicePrint(InvoicePrintSettingVM VM)
        {
            var db = new AccountingContainer();

            var printsetting = new InvoicePrintSetting();

            printsetting.PrintLogo = VM.PrintLogo;
            printsetting.printWhite = VM.printWhite;
            printsetting.TypeId = VM.TypeId;
            printsetting.Active = VM.Active;
            printsetting.Description = VM.Description;
            printsetting.Package = VM.Package;
            if (VM.Image != null)
            {
                //var data = new byte[catgorVM.Cat_Img.];
                //Categr.Cat_Img = data;
                var length = VM.Image.InputStream.Length; //Length: 103050706
                MemoryStream target = new MemoryStream();
                VM.Image.InputStream.CopyTo(target); // generates problem in this line
                byte[] data = target.ToArray();
                printsetting.Image = data;

            }
         
            db.InvoicePrintSettings.Add(printsetting);
            db.SaveChanges();
            db.Dispose();
            return true;
        }

        // use this method to load Details of Data for Category.
        public static InvoicePrintSettingVM FillTheViewModel(int id)
        {
            var db = new AccountingContainer();
            var PrintSetting = db.InvoicePrintSettings.Find(id);
            var VM = new InvoicePrintSettingVM();

            VM.PrintLogo = PrintSetting.PrintLogo;
            VM.printWhite = PrintSetting.printWhite;
            VM.TypeId = PrintSetting.TypeId;
            VM.Active = PrintSetting.Active;
            VM.Description = PrintSetting.Description;
            VM.Package = PrintSetting.Package;
            return VM;

        }

        // use this method to Edite Catgery and save it in DataBase.
        public static InvoicePrintSetting Edit(InvoicePrintSettingVM VM)
        {
            var db = new AccountingContainer();
            var InvoicePrint = db.InvoicePrintSettings.Find(VM.Id);
            if (InvoicePrint != null)
            {


                InvoicePrint.PrintLogo = VM.PrintLogo;
                InvoicePrint.printWhite = VM.printWhite;
                InvoicePrint.TypeId = VM.TypeId;
                InvoicePrint.Active = VM.Active;
                InvoicePrint.Description = VM.Description;
                InvoicePrint.Package = VM.Package;
                if (VM.Image != null)
                {
                    //var data = new byte[catgorVM.Cat_Img.];
                    //Categr.Cat_Img = data;
                    var length = VM.Image.InputStream.Length; //Length: 103050706
                    MemoryStream target = new MemoryStream();
                    VM.Image.InputStream.CopyTo(target); // generates problem in this line
                    byte[] data = target.ToArray();
                    InvoicePrint.Image = data;

                }
                db.Entry(InvoicePrint).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return InvoicePrint;
            }
            return null;
        }
        public static int PostNewSalesForStore(dynamic json,int id)
        {
            var db = new AccountingContainer();
            var Company = db.Companies.FirstOrDefault();
            var InvoicePrint = db.InvoicePrintSettings.Find(id);
            if (InvoicePrint.TypeId == 1)
            {
                Company.InvoicePrintOrdanary = id;
            }
            else if (InvoicePrint.TypeId == 2)
            {
                Company.InvoicePrintSimple = id;
            }
            else if (InvoicePrint.TypeId == 3)
            {
                Company.InvoicePrintOfferPrice = id;
            }
            db.Entry(Company).State = EntityState.Modified;

            db.SaveChanges();
            return 1;
            //foreach (var batchvm in json)
            //{
            //    var  IsActive = Convert.ToBoolean(batchvm.Active);
            //    var type= Convert.ToInt32(batchvm.TypeId);
            //    if (IsActive == true&& type == 1)
            //    {
            //        Company.InvoicePrintOrdanary = batchvm.Id;
            //    }
            //    else if (IsActive == true && type == 2)
            //    {
            //        Company.InvoicePrintSimple = batchvm.Id;
            //    }
            //    else if(IsActive == true && type == 3)
            //    {
            //        Company.InvoicePrintSpecial = batchvm.Id;
            //    }
            //    else
            //    {
            //        continue;
            //    }
               
          

            }
       




        }
    }
