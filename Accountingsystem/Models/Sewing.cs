using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class Sewing
    {
        public static List<SewingVM> GetAllsewing(int ? all)
        {
            var db = new AccountingContainer();
            var sewingList = db.Sewings.ToList();
            var InvoiceMasterList = (from sewing in sewingList
                                     join customer in db.Customers on sewing.CustomerId equals customer.Customer_Id
                                    
                                     join status in db.SewingStatus on sewing.SewingStatusId equals status.Id


                                     select new SewingVM
                                     {
                                         orderNumber = sewing.OrderNumber,
                                         customer = customer,
                                         invoice = sewing.MasterPOSInvoice,
                                         Id = sewing.Id,
                                         Dated = sewing.Date,
                                         DueDated = sewing.DueDate,
                                         status = status
                                        

                                     }).ToList();
            if (all == null)
            {
                sewingList.Reverse();
                sewingList = sewingList.Take(100).ToList();
            }
            return InvoiceMasterList;
        }

        // use this method to   Get one element CategoryById from Database.  
        public static Sewing GetsewingById(int id)
        {
            var db = new AccountingContainer();
            var sewing = db.Sewings.Find(id);
            return sewing;
        }
        public static SewingVM FillForCreate()
        {
            var db = new AccountingContainer();
            SewingVM vm = new SewingVM();
            vm.QuestionList = new List<Question>();
           vm.QuestionList = db.Questions.OrderBy(a=>a.Sort).Where(a => a.IsActive == true).ToList();
            var newList = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                           select new
                           {
                               Id = cust.Customer_Id,
                               Name = cust.Customer_Name + "        " + cust.Customer_Phone

                           }).ToList();
            vm.CustList = new SelectList(newList, "Id", "Name");
            vm.NumberOfSewing = 1;
            var ProductList = (from pro in db.Products.Where(a => a.IsActive != false)
                           select new
                           {
                               Id = pro.Prod_ID,
                               Name = pro.Prod_Name + "        " + pro.Bar_Code

                           }).ToList();
            vm.ProdductList = new SelectList(ProductList, "Id", "Name");
            var thistaxinchart = db.ChartOfAccountings.Find(48);
            var anewList = (from Tax in db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false)
                            select new
                            {

                                Id = Tax.Tax_ID,
                                Name = Tax.Tax_Name + "   " + Tax.Tax_Percentage + "%"

                            }).ToList();
            vm.TaxList = new SelectList(anewList, "Id", "Name");
            return vm;
        }
        public static int CreatePost(dynamic json, string Date, string DueDate, string CustomerId, string ProductId, string NumberOfSewing, string order,string Amount,string quantity)
        {
            var db = new AccountingContainer();
     
            var orderId= Convert.ToInt32(order);
        
            var sewingModel = new Sewing();
        
            sewingModel.Date = Convert.ToDateTime(Date);
            sewingModel.DueDate = Convert.ToDateTime(DueDate);
            sewingModel.CustomerId = Convert.ToInt32(CustomerId);
            sewingModel.Desc = "";
            sewingModel.Notes = "";
            sewingModel.NumebrOfDesign = Convert.ToInt32(NumberOfSewing);
            sewingModel.Amount = Convert.ToDecimal(Amount);
            if (ProductId != "")
            {
                sewingModel.ProductId = Convert.ToInt32(ProductId);
            }
            if (quantity != "")
            {
                sewingModel.Quantity = Convert.ToDecimal(quantity);
            }
            else
            {
                sewingModel.Quantity = 1;
            }
            sewingModel.SewingDetails = new List<SewingDetail>();
            foreach (var batchvm in json)
            {
                SewingDetail detail = new SewingDetail();
                if (batchvm.AnswerId != null)
                {
                    detail.AnswerId = Convert.ToInt32(batchvm.AnswerId);
                }
                detail.QuestionId = Convert.ToInt32(batchvm.QuestionId);
                if (batchvm.AnswerDesc != null)
                {
                    detail.Description = batchvm.AnswerDesc;
                }
                else
                {
                    detail.Description = "";
                }
                sewingModel.SewingDetails.Add(detail);

            }
            sewingModel.OrderNumber = db.Sewings.Count() + 1;
            sewingModel.SewingStatusId = 1;
            db.Sewings.Add(sewingModel);
            db.SaveChanges();
            return sewingModel.Id;




        }
        public static SewingVM FillForEdit(int id ,int ? Iscopy)
        {
            var db = new AccountingContainer();
            var sewing = db.Sewings.Find(id);
            SewingVM vm = new SewingVM();
            vm.Id = sewing.Id;
            vm.Amount = sewing.Amount;
            vm.NumberOfSewing = sewing.NumebrOfDesign;
            vm.SelectedCustomerId = sewing.CustomerId;
            vm.Date = sewing.Date.ToString("dd-MM-yyyy   HH:mm:ss");
            vm.DueDate = sewing.DueDate.ToString("dd-MM-yyyy   HH:mm:ss");
            vm.ProductId = sewing.ProductId;
            if (Iscopy == 1)
            {
                vm.iscopy = 1;
                   
            }
            else
            {
                vm.iscopy = 0;
            }
            vm.QuestionList = new List<Question>();
            vm.QuestionList = db.Questions.OrderBy(a => a.Sort).Where(a => a.IsActive == true).ToList();
            var newList = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                           select new
                           {
                               Id = cust.Customer_Id,
                               Name = cust.Customer_Name + "        " + cust.Customer_Phone

                           }).ToList();
            vm.CustList = new SelectList(newList, "Id", "Name");
            vm.NumberOfSewing = 1;
            var ProductList = (from pro in db.Products.Where(a => a.IsActive != false)
                               select new
                               {
                                   Id = pro.Prod_ID,
                                   Name = pro.Prod_Name + "        " + pro.Bar_Code

                               }).ToList();
            vm.ProdductList = new SelectList(ProductList, "Id", "Name");
            var thistaxinchart = db.ChartOfAccountings.Find(48);
            var anewList = (from Tax in db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false)
                            select new
                            {

                                Id = Tax.Tax_ID,
                                Name = Tax.Tax_Name + "   " + Tax.Tax_Percentage + "%"

                            }).ToList();
            vm.TaxList = new SelectList(anewList, "Id", "Name");
            if (sewing.Quantity > 0)
            {
                vm.Quantity = sewing.Quantity;
            }
            return vm;
        }
        public static int EditPost(dynamic json, string Date, string DueDate, string CustomerId, string ProductId, string NumberOfSewing, string order, string Amount,string SewingId,string quantity)
        {
            var db = new AccountingContainer();

            

            var sewingModel =db.Sewings.Find(Convert.ToInt32(SewingId));

            sewingModel.Date = Convert.ToDateTime(Date);
            sewingModel.DueDate = Convert.ToDateTime(DueDate);
            sewingModel.CustomerId = Convert.ToInt32(CustomerId);
            sewingModel.Desc = "";
            sewingModel.Notes = "";
            sewingModel.NumebrOfDesign = Convert.ToInt32(NumberOfSewing);
            sewingModel.Amount = Convert.ToDecimal(Amount);
            List<SewingDetail> oldList = sewingModel.SewingDetails.ToList();
            if (ProductId != "")
            {
                sewingModel.ProductId = Convert.ToInt32(ProductId);
            }
            if (quantity != "")
            {
                sewingModel.Quantity = Convert.ToDecimal(quantity);
            }
            else
            {
                sewingModel.Quantity =1;
            }
            sewingModel.SewingDetails = new List<SewingDetail>();
            foreach (var batchvm in json)
            {
                SewingDetail detail = new SewingDetail();
                if (batchvm.AnswerId != null)
                {
                    detail.AnswerId = Convert.ToInt32(batchvm.AnswerId);
                }
                detail.QuestionId = Convert.ToInt32(batchvm.QuestionId);
                if (batchvm.AnswerDesc != null)
                {
                    detail.Description = batchvm.AnswerDesc;
                }
                else
                {
                    detail.Description = "";
                }
                sewingModel.SewingDetails.Add(detail);

            }
            db.SewingDetails.RemoveRange(oldList);
            db.Entry(sewingModel).State = EntityState.Modified;
           
            db.SaveChanges();
            return sewingModel.Id;




        }
        public static int Editstaus(int id , int status)
        {
            var db = new AccountingContainer();
            var sewing = db.Sewings.Find(id);
            sewing.SewingStatusId = status;
            db.Entry(sewing).State = EntityState.Modified;

            db.SaveChanges();
            return sewing.Id;

        }
        public static string ConvertDateCalendar(DateTime DateConv, string Calendar, string DateLangCulture)
        {
            System.Globalization.DateTimeFormatInfo DTFormat;
            DateLangCulture = DateLangCulture.ToLower();
            /// We can't have the hijri date writen in English. We will get a runtime error - LAITH - 11/13/2005 1:01:45 PM -

            if (Calendar == "Hijri" && DateLangCulture.StartsWith("en-"))
            {
                DateLangCulture = "ar-sa";
            }

            /// Set the date time format to the given culture - LAITH - 11/13/2005 1:04:22 PM -
            DTFormat = new System.Globalization.CultureInfo(DateLangCulture, false).DateTimeFormat;

            /// Set the calendar property of the date time format to the given calendar - LAITH - 11/13/2005 1:04:52 PM -
            switch (Calendar)
            {
                case "Hijri":
                    DTFormat.Calendar = new System.Globalization.HijriCalendar();
                    break;

                case "Gregorian":
                    DTFormat.Calendar = new System.Globalization.GregorianCalendar();
                    break;

                default:
                    return "";
            }

            /// We format the date structure to whatever we want - LAITH - 11/13/2005 1:05:39 PM -
            DTFormat.ShortDatePattern = "dd-MM-yyyy";
            var date = DateConv.Date.ToString("f", DTFormat);
            return (DateConv.Date.ToString("f", DTFormat));
        }
    }
}