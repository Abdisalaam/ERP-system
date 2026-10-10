using Accountingsystem.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class MasterPOSInvoice
    {
        /// <summary>
        /// chek that date is suitable
        /// </summary>
        /// <returns></returns>
        public static bool Checkdate(string date)
        {
            var datetime = Convert.ToDateTime(date);
            var db = new AccountingContainer();
            var Comstartdate = db.Companies.FirstOrDefault().StartFinancialDate;
            if (Comstartdate != null)
            {
                if (datetime < Comstartdate.Value)
                {
                    return false;
                }


            }

            return true;


        }


        /// <summary>
        /// make crate of POS invoice master that depend on Employee Register if has no Access in system he can not create POsinvoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MasterPOSInvoiceVM Create(String id,int ? sewingId)
        {
            var db = new AccountingContainer();
            
            var masterPOS = new MasterPOSInvoiceVM();
            var Emp = db.Employees.Where(a => a.Reg_ID == id).FirstOrDefault();
            if (Emp!=null)
            {
                var Point = db.PointOfSales.Where(a => a.Employee_Id == Emp.Emp_Id).FirstOrDefault();
                if (Point != null)
                {
                    if (Point.Subledger_Id > 0)
                    {
                        var subLedger= SalesSetting.getSubledgerByIdForJson(Point.Subledger_Id);
                        masterPOS.Description = subLedger.InvoiceDesc;
                        masterPOS.Notes = subLedger.InvoiceNotes;
                    }

                    var lastSerial = db.MasterPOSInvoices.OrderByDescending(c => c.MasterPOSInvoiceId).FirstOrDefault();
                    if (lastSerial == null)
                    {
                        masterPOS.Serial_Number = "POS-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        masterPOS.Serial_Number = "POS-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
                    }
                    String StartCodeOfrevenue = "04";
                    var masterpostemp = new MasterPOSInvoiceTemp();
                    var Id = (from m in db.MasterPOSInvoiceTemps
                              select m).ToList().LastOrDefault();
                    if (id == null)
                    {
                        masterpostemp.MasterPOSInvoiceId = Id.MasterPOSInvoiceId + 1;
                    }
                    masterpostemp.AllowDiscount = 0;
                    masterpostemp.TotalNewTaxs = 0;
                    masterpostemp.Total_Amount = 0;
                    masterpostemp.PointOfSalePOS_Id = Point.POS_Id;
                    db.MasterPOSInvoiceTemps.Add(masterpostemp);
                    db.SaveChanges();
                    masterPOS.MasterPOSInvoiceId = masterpostemp.MasterPOSInvoiceId;


                    var members1 = GetAllCustomerForEmpList(id);
                    List<object> newList = new List<object>();
                    foreach (var member in members1)
                        newList.Add(new
                        {
                            Id = member.Customer_Id,
                            Name = member.Customer_Name + "        " + member.Customer_Phone
                        });

                    masterPOS.CustomerList = new SelectList(newList, "Id","Name");
                  
                    var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0&&a.isActive!=false).ToList();
              
                    var thistaxinchart = db.ChartOfAccountings.Find(48);
                    //var memberso = db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false).ToList();
                    //foreach (var membero in memberso)
                    //    anewList.Add(new
                    //    {
                    //        Id = membero.Tax_ID,
                    //        Name = membero.Tax_Name + "   " + membero.Tax_Percentage + "%"
                    //    });
                    var anewList = (from Tax in db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false)
                                    select new
                                    {

                                        Id = Tax.Tax_ID,
                                        Name = Tax.Tax_Name + "   " + Tax.Tax_Percentage + "%"

                                    }).ToList();
                    masterPOS.TaxList = new SelectList(anewList, "Id", "Name");
                    masterPOS.RevenueList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfrevenue)), " Account_Id", "Account_Name");
                    masterPOS.Invoice_Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
                    masterPOS.RevenueAccount = 76;

                    masterPOS.PointSaleList = new SelectList(db.PointOfSales.Where(a => a.Employee_Id == Emp.Emp_Id).ToList(), "POS_Id", "POS_Name");
                    masterPOS.EmpList = new SelectList(db.Employees.Where(a => a.Emp_Id == Emp.Emp_Id).ToList(), "Emp_Id", "Emp_Name");
                    masterPOS.PointOfSalePOS_Id = Point.POS_Id;
                    if (sewingId > 0)
                    {
                        masterPOS.sewingId = sewingId.Value;
                        masterPOS.Customer_Id = db.Sewings.Find(sewingId.Value).CustomerId;
                    }
                    return masterPOS;
                }
            }
          
                return null;
            
           
    
            
            
        }
        /// <summary>
        /// Retern all data that user fill if make mistike in create Invoice
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Tempid"></param>
        /// <returns>MasterPOSInvoiceVM</returns>
        public static MasterPOSInvoiceVM CreateForMaintenance(String id,int MainId)
        {
            var db = new AccountingContainer();
          

            var masterPOS = new MasterPOSInvoiceVM();
            var Main = db.MaintenanceOrderOperationals.Find(MainId);
            masterPOS.Customer_Id = Main.Customer_Id;
            masterPOS.MasterPOSInvoiceId = MainId;
            masterPOS.Description = Main.RequiredDescribtion;
            var Emp = db.Employees.Where(a => a.Reg_ID == id).FirstOrDefault();
            if (Emp != null)
            {
                var Point = db.PointOfSales.Where(a => a.Employee_Id == Emp.Emp_Id).FirstOrDefault();
                if (Point != null)
                {

                    var lastSerial = db.MasterPOSInvoices.OrderByDescending(c => c.MasterPOSInvoiceId).FirstOrDefault();
                    if (lastSerial == null)
                    {
                        masterPOS.Serial_Number = "POS-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        masterPOS.Serial_Number = "POS-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
                    }
                    String StartCodeOfrevenue = "04";
                    var masterpostemp = new MasterPOSInvoiceTemp();
                    var Id = (from m in db.MasterPOSInvoiceTemps
                              select m).ToList().LastOrDefault();
                    if (id == null)
                    {
                        masterpostemp.MasterPOSInvoiceId = Id.MasterPOSInvoiceId + 1;
                    }
                    masterpostemp.AllowDiscount = 0;
                    masterpostemp.TotalNewTaxs = 0;
                    masterpostemp.Total_Amount = Main.Total_Amount;
                    db.MasterPOSInvoiceTemps.Add(masterpostemp);
                    db.SaveChanges();
                    masterPOS.MasterPOSInvoiceId = masterpostemp.MasterPOSInvoiceId;



                    var members1 = GetAllCustomerForEmpList(id);
                    List<object> newList = new List<object>();
                    foreach (var member in members1)
                        newList.Add(new
                        {
                            Id = member.Customer_Id,
                            Name = member.Customer_Name + "        " + member.Customer_Phone
                        });

                    masterPOS.CustomerList = new SelectList(newList, "Id", "Name");

                    var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0&&a.isActive!=false).ToList();

                    var thistaxinchart = db.ChartOfAccountings.Find(48);
                   
                    var anewList = (from Tax in db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false)
                                    select new
                                    {

                                        Id = Tax.Tax_ID,
                                        Name = Tax.Tax_Name + "   " + Tax.Tax_Percentage + "%"

                                    }).ToList();
                    masterPOS.TaxList = new SelectList(anewList, "Id", "Name");
                    masterPOS.RevenueList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfrevenue)), " Account_Id", "Account_Name");
                    masterPOS.Invoice_Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
                    masterPOS.RevenueAccount = 76;

                    masterPOS.PointSaleList = new SelectList(db.PointOfSales.Where(a => a.Employee_Id == Emp.Emp_Id).ToList(), "POS_Id", "POS_Name");
                    masterPOS.EmpList = new SelectList(db.Employees.Where(a => a.Emp_Id == Emp.Emp_Id).ToList(), "Emp_Id", "Emp_Name");
                    masterPOS.PointOfSalePOS_Id = Point.POS_Id;
                    masterPOS.Master_Discount = Main.Master_Discount.Value;
                    masterPOS.MaintenanceOrder_Id = MainId;
                    return masterPOS;
                }
            }

            return null;





        }
        public static MasterPOSInvoiceDetailTempVM PrepareListdetailOfOrdertoPOS(int masterid, int Offerid)
        {
            var db = new AccountingContainer();
            var offer = db.MaintenanceOrderOperationals.Find(Offerid);
            var invoicedetalTemp = new MasterPOSInvoiceDetailTempVM();

            var Mastertemp = db.MasterPOSInvoiceTemps.Find(masterid);
           

            var DetailList = new List<DetailPOSInvoiceTemp>();
            foreach (var item in offer.MaintenanceOrderOperationalDetails.ToList())
            {
                var Detail = new DetailPOSInvoiceTemp();
                Detail.Batch_ID = item.Batch_ID;

                Detail.Detail_Total_Cost = 0;
                Detail.Detail_Total_Tax = item.Detail_Total_Tax;
                Detail.Product_Id = item.Product_Id;
                var pro = db.Products.Find(Detail.Product_Id);
               
                Detail.Detail_Total_Cost = pro.Prod_Cost_Price.Value * item.Product_Quantity;
                Detail.Product_Price = item.Product_Price;
                Detail.Product_Quantity = item.Product_Quantity;
                Detail.Product_Tax = item.Product_Tax;
                Detail.Total_Amount = item.Total_Amount;
                Detail.TotalBeforeDisc = item.Product_Quantity* item.Product_Price;
                Detail.TotalDiscount = (offer.Master_Discount / 100) * Detail.TotalBeforeDisc;
                Detail.Detail_Discount = offer.Master_Discount;
                Detail.Detail_Discount_Amount = Detail.TotalDiscount;
                Detail.MasterPOSInvoiceTempMasterPOSInvoiceId = Mastertemp.MasterPOSInvoiceId;
                DetailList.Add(Detail);

            }
            if (Mastertemp.DetailPOSInvoiceTemps.Count != 0)
            {
                db.DetailPOSInvoiceTemps.RemoveRange(Mastertemp.DetailPOSInvoiceTemps);
                db.SaveChanges();
            }
            db.DetailPOSInvoiceTemps.AddRange(DetailList);

            db.SaveChanges();
            invoicedetalTemp.invoiceTemp = DetailList;
            if (offer.AllowDiscount != null)
            {
                invoicedetalTemp.AllowDiscount = offer.AllowDiscount.Value;
                   
            }
            else
            {
                invoicedetalTemp.AllowDiscount = 0;
            }

            invoicedetalTemp.invoice_Id = masterid;
            return invoicedetalTemp;
        }
        public static MasterPOSInvoiceDetailTempVM PrepareListdetailOfSewing(int masterid, int? SewingId)
        {
            var db = new AccountingContainer();
            var sewing = db.Sewings.Find(SewingId);
            var SewingList = db.Sewings.Where(a => a.CustomerId == sewing.CustomerId && a.MasterPOSInvoiceMasterPOSInvoiceId == null).ToList();
          


            var invoicedetalTemp = new MasterPOSInvoiceDetailTempVM();

            var Mastertemp = db.MasterPOSInvoiceTemps.Find(masterid);
            var pos = db.PointOfSales.Find(Mastertemp.PointOfSalePOS_Id);

            var DetailList = new List<DetailPOSInvoiceTemp>();
            foreach (var item in SewingList.Where(a=>a.ProductId>0).Select(a=>a.ProductId).Distinct())
            {
                var Detail = new DetailPOSInvoiceTemp();
                var product = db.Products.Find(item);
                var batch = product.ProductBatches.Where(a => a.Store_ID == pos.Store_Id).FirstOrDefault();
                if (batch == null)
                {
                    continue;
                }
                Detail.Batch_ID = product.ProductBatches.Where(a=>a.Store_ID==pos.Store_Id).FirstOrDefault().Batch_ID;
                Detail.Product_Quantity = SewingList.Where(a => a.ProductId == item).Select(a => a.Quantity).Sum();
                Detail.Product_Price= product.ProductBatches.Where(a => a.Store_ID == pos.Store_Id).FirstOrDefault().SalesPrice.Value;
                decimal tax =0;
                if (product.Tax_Id > 0)
                {
                    Detail.Product_Tax = product.Tax_Id.Value;
                    tax = db.Taxes.Find(Detail.Product_Tax).Tax_Percentage;
                }
                else
                {
                    Detail.Product_Tax = 0;
                    tax = 0;
                }
                Detail.Detail_Total_Cost = 0;
                Detail.Detail_Total_Tax = (Detail.Product_Quantity* Detail.Product_Price )*(tax / 100);
                Detail.Product_Id = product.Prod_ID;
            

                Detail.Detail_Total_Cost = product.Prod_Cost_Price.Value * Detail.Product_Quantity;
            
               
                Detail.Total_Amount =( Detail.Product_Quantity*Detail.Product_Price )+ Detail.Detail_Total_Tax;
                Detail.TotalBeforeDisc = (Detail.Product_Quantity * Detail.Product_Price);
                Detail.TotalDiscount = 0;
                Detail.Detail_Discount =0;
                Detail.Detail_Discount_Amount = 0;
                Detail.MasterPOSInvoiceTempMasterPOSInvoiceId = Mastertemp.MasterPOSInvoiceId;
                DetailList.Add(Detail);

            }
            if (Mastertemp.DetailPOSInvoiceTemps.Count != 0)
            {
                db.DetailPOSInvoiceTemps.RemoveRange(Mastertemp.DetailPOSInvoiceTemps);
                db.SaveChanges();
            }
            db.DetailPOSInvoiceTemps.AddRange(DetailList);

            db.SaveChanges();
            invoicedetalTemp.invoiceTemp = DetailList;
            

            invoicedetalTemp.invoice_Id = masterid;
            return invoicedetalTemp;
        }
        public static MasterPOSInvoiceVM ErrorCreate(String id,int Tempid,int ? MainId)
        {
            var db = new AccountingContainer();
            var masterPOS = new MasterPOSInvoiceVM();
            if (MainId != 0 && MainId != null)
            {

                var Main = db.MaintenanceOrderOperationals.Find(MainId);
                masterPOS.Customer_Id = Main.Customer_Id;
                masterPOS.Description = Main.RequiredDescribtion;
            }
           
            var Emp = db.Employees.Where(a => a.Reg_ID == id).FirstOrDefault();
            if (Emp != null)
            {
                masterPOS.PointSaleList = new SelectList(db.PointOfSales.Where(a => a.Employee_Id == Emp.Emp_Id).ToList(), "POS_Id", "POS_Name");
                masterPOS.EmpList = new SelectList(db.Employees.Where(a => a.Emp_Id == Emp.Emp_Id).ToList(), "Emp_Id", "Emp_Name");
            }
            else
            {
                masterPOS.PointSaleList = new SelectList(db.PointOfSales.ToList(), "POS_Id", "POS_Name");
                masterPOS.EmpList = new SelectList(db.Employees.ToList(), "Emp_Id", "Emp_Name");
            }


            var lastSerial = db.MasterPOSInvoices.OrderByDescending(c => c.MasterPOSInvoiceId).FirstOrDefault();
            if (lastSerial == null)
            {
                masterPOS.Serial_Number = "POS-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                masterPOS.Serial_Number = "POS-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
            }
            String StartCodeOfrevenue = "04";
        
            masterPOS.MasterPOSInvoiceId = Tempid;

            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0&&a.isActive!=false).ToList();

            var members1 = GetAllCustomerForEmpList(id);
            List<object> newList = new List<object>();
            foreach (var member in members1)
                newList.Add(new
                {
                    Id = member.Customer_Id,
                    Name = member.Customer_Name + "        " + member.Customer_Phone
                });

            masterPOS.CustomerList = new SelectList(newList, "Id", "Name");
            masterPOS.RevenueList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfrevenue)), " Account_Id", "Account_Name");
           
            masterPOS.RevenueAccount = 76;
            masterPOS.Invoice_Date= DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
       
            var thistaxinchart = db.ChartOfAccountings.Find(48);
            var anewList = (from Tax in db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false)
                            select new
                            {

                                Id = Tax.Tax_ID,
                                Name = Tax.Tax_Name + "   " + Tax.Tax_Percentage + "%"

                            }).ToList();
            masterPOS.TaxList = new SelectList(anewList, "Id", "Name");
            return masterPOS;
        }
        /// <summary>
        /// make post of create and save that invoice in Data base
        /// </summary>
        /// <param name="MasterVM"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static Tuple<int, string, int> CreatePost(MasterPOSInvoiceVM MasterVM, String UserId)
        {


            var db = new AccountingContainer();
            MasterPOSInvoice Master = new MasterPOSInvoice();
            Master.Total_Payment_Method_Cost = 0;
            Master.Total_Amount = 0;
            Master.Total_Cost = 0;
            Master.Total_Tax = 0;
            decimal paymentAmount = 0;

            if (MasterVM.Invoice_Date == null || MasterVM.Invoice_Date == "")
            {
                Master.Invoice_Date = DateTime.Now;
            }
            else
            {
                Master.Invoice_Date = Convert.ToDateTime(MasterVM.Invoice_Date);
            }


            Master.PointOfSalePOS_Id = MasterVM.PointOfSalePOS_Id;
            var Point = db.PointOfSales.Find(MasterVM.PointOfSalePOS_Id);
            Master.Company_Id = Point.Company_Id;
            Master.Notes = MasterVM.Notes;
            Master.Description = MasterVM.Description;
            Master.Employee_Id = Point.Employee_Id;
            Master.Invoice_Currency ="ريال سعودي";
            Master.Customer_Id = MasterVM.Customer_Id;
            Master.MainrenanceOrder_Id = MasterVM.MaintenanceOrder_Id;
            var lastSerial = db.MasterPOSInvoices.OrderByDescending(c => c.MasterPOSInvoiceId).FirstOrDefault();
            if (lastSerial == null)
            {
                Master.Serial_Number = "POS-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                Master.Serial_Number = "POS-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
            }
            Master.Master_Discount = MasterVM.Master_Discount;
            Master.Invoice_State = TransactionState.Pending;
            if (MasterVM.RevenueAccount == 0 || MasterVM.RevenueAccount == null)
            {
                Master.Account_Id = 76;
            }
            else
            {
                Master.Account_Id = MasterVM.RevenueAccount;
            }
            Master.Store_ID = Point.Store_Id;
            Master.Subledger_Id = Point.Subledger_Id;
            var MasterTemp = db.MasterPOSInvoiceTemps.Find(MasterVM.MasterPOSInvoiceId);
            var invoiceDetailTemp = new List<DetailPOSInvoiceTemp> { };
            var PaymentTemp = new List<PosPaymentMethodTemp> { };
            if (MasterTemp.DetailPOSInvoiceTemps.FirstOrDefault() == null)
            {
                return Tuple.Create(MasterTemp.MasterPOSInvoiceId, Accountingsystem.Resources.validate.invoiceProductMust, 0);


            }

            var qrMaster = new MasterQRInvoice();
            qrMaster.QrGuid = Guid.NewGuid();
            qrMaster.DetailQRInvoices = new List<DetailQRInvoice>();
            qrMaster.QRPaymentMethods = new List<QRPaymentMethod>();
            qrMaster.TaxQRInvoices = new List<TaxQRInvoice>();

            foreach (var item in MasterTemp.PosPaymentMethodTemps.ToList())
            {
                var PaymentDetail = new PosPaymentMethod();
                PaymentDetail.PaymentMethod_Id = item.PaymentMethod_Id;
                PaymentDetail.Amount = item.Amount;
                PaymentDetail.Commution = item.Commution.Value;
                Master.Total_Payment_Method_Cost += PaymentDetail.Commution;
                paymentAmount += item.Amount;
                Master.PosPaymentMethods.Add(PaymentDetail);
                PaymentTemp.Add(item);
                ////qr POS Generator
                var PaymentQRDetail = new QRPaymentMethod();
                PaymentQRDetail.PaymentMethod_Id = item.PaymentMethod_Id;
                PaymentQRDetail.Amount = item.Amount;
                PaymentQRDetail.Commution = item.Commution.Value;


                qrMaster.QRPaymentMethods.Add(PaymentQRDetail);
             

            }
            foreach (var item in MasterTemp.DetailPOSInvoiceTemps.ToList())
            {
                var invoiceDetail = new DetailPOSInvoice();
                var invoiceQRDetail = new DetailQRInvoice();

                if (item.Batch_ID == 0)
                {
                    var product = db.Products.Find(item.Product_Id);
                    if (product.Prod_Type == Prod_Type.خدمة|| product.Prod_Type == Prod_Type.مجمع)
                    {
                        item.Batch_ID = product.ProductBatches.FirstOrDefault().Batch_ID;
                    }
                    else if(product.ProductBatches.Where(a=>a.Store_ID==Master.Store_ID).FirstOrDefault()==null)
                    {
                        var msg = Accountingsystem.Resources.MasterPOSInvoice.Thereisnotenoughproduct + " " + product.Prod_Name;
                        return Tuple.Create(MasterTemp.MasterPOSInvoiceId, msg, 0);
                    }
                    else if(product.ProductBatches.Where(a => a.Store_ID == Master.Store_ID).FirstOrDefault().Product_Quantity< item.Product_Quantity)
                    {
                        var msg = Accountingsystem.Resources.MasterPOSInvoice.Thereisnotenoughproduct + " " + product.Prod_Name;
                        return Tuple.Create(MasterTemp.MasterPOSInvoiceId, msg, 0);
                    }
                    else
                    {
                        item.Batch_ID = product.ProductBatches.Where(a => a.Store_ID == Master.Store_ID).FirstOrDefault().Batch_ID;
                    }

                }
                invoiceDetail.Batch_ID = item.Batch_ID;
                invoiceDetail.Product_Id = item.Product_Id;
                invoiceDetail.Product_Quantity = item.Product_Quantity;
                invoiceDetail.Product_Tax = item.Product_Tax;
                var batch = db.ProductBatches.Find(item.Batch_ID);

                invoiceDetail.Detail_Total_Cost = item.Product_Quantity * batch.Product_Cost;
                invoiceDetail.Reverse_Total_Tax = item.Detail_Total_Tax;
                invoiceDetail.Reverse_Quantity = item.Product_Quantity;
                invoiceDetail.Total_Amount = item.Total_Amount;

                invoiceDetail.Detail_Discount = item.Detail_Discount;
                invoiceDetail.Detail_Discount_Amount = item.Detail_Discount_Amount;

                invoiceDetail.Reverse_Total_Amount = item.Total_Amount;
                invoiceDetail.Product_Price = item.Product_Price;
                invoiceDetail.Detail_Total_Tax = item.Detail_Total_Tax;
                invoiceDetail.ProductPriceCost = db.ProductBatches.Find(invoiceDetail.Batch_ID).Product_Cost;
                Master.Total_Tax += invoiceDetail.Detail_Total_Tax;
                Master.Total_Cost += invoiceDetail.Detail_Total_Cost;
                Master.Total_Amount += invoiceDetail.Total_Amount;
                Master.Invoice_State = TransactionState.Post;

                Master.DetailPOSInvoices.Add(invoiceDetail);
                invoiceDetailTemp.Add(item);

                ///QR Detail Master
                ///
                invoiceQRDetail.Batch_ID = invoiceDetail.Batch_ID;
                invoiceQRDetail.Product_Id = invoiceDetail.Product_Id;
                invoiceQRDetail.Product_Quantity = invoiceDetail.Product_Quantity;
                invoiceQRDetail.Product_Tax = invoiceDetail.Product_Tax;
                invoiceQRDetail.Detail_Discount = item.Detail_Discount;
                invoiceQRDetail.Detail_Discount_Amount = item.Detail_Discount_Amount;

                invoiceQRDetail.Detail_Total_Cost = invoiceDetail.Detail_Total_Cost;

                invoiceQRDetail.Total_Amount = invoiceDetail.Total_Amount;

                invoiceQRDetail.Product_Price = invoiceDetail.Product_Price;
                invoiceQRDetail.Detail_Total_Tax = invoiceDetail.Detail_Total_Tax;
                invoiceQRDetail.ProductPriceCost = invoiceDetail.ProductPriceCost;
                qrMaster.DetailQRInvoices.Add(invoiceQRDetail);
            }
            var realdetails = getMaterialOCompoundProduct(Master.DetailPOSInvoices.ToList(), Master.Store_ID.Value);
            foreach (var item in realdetails)
            {
                var productBatch = db.ProductBatches.Find(item.Batch_ID);
                if (productBatch.Product_Quantity < item.Product_Quantity && (productBatch.Product.Prod_Type == Prod_Type.منتج || productBatch.Product.Prod_Type == Prod_Type.اولي))
                {
                    var productName = db.Products.Find(item.Product_Id).Prod_Name;

                    var msg = Accountingsystem.Resources.MasterPOSInvoice.Thereisnotenoughproduct + " " + productName;
                    return Tuple.Create(MasterTemp.MasterPOSInvoiceId, msg, 0);

                }

            }

            Master.TotalNewTaxs = 0;
            if (MasterVM.Taxes != null)
            {
                foreach (var tax in MasterVM.Taxes)
                {
                    var taxo = new TaxPOSInvoice();
                    taxo.Tax_Id = tax;

                    taxo.Amount = (db.Taxes.Find(Convert.ToInt32(tax)).Tax_Percentage / 100) * Master.DetailPOSInvoices.Select(a => a.Total_Amount).Sum();
                    Master.TotalNewTaxs += taxo.Amount;

                    Master.TaxPOSInvoices.Add(taxo);

                    ///qr TAx Adding
                    var qrtax = new TaxQRInvoice();
                    qrtax.Tax_Id = tax;

                    qrtax.Amount = taxo.Amount;
                    qrMaster.TaxQRInvoices.Add(qrtax);

                }
            }
            if (MasterTemp.PosPaymentMethodTemps.FirstOrDefault() == null)
            {

                var PaymentDetail = new PosPaymentMethod();
                if (MasterTemp.Exchange_Rate == 0 || MasterTemp.Exchange_Rate == null)
                {
                    PaymentDetail.PaymentMethod_Id = Point.POSDetailPayments.FirstOrDefault().PaymentMethod_Id;
                }
                else
                {
                    PaymentDetail.PaymentMethod_Id = (int)MasterTemp.Exchange_Rate.Value;
                }
                PaymentDetail.Amount = Master.Total_Amount.Value + Master.TotalNewTaxs.Value;
                var payment = db.PaymentMethods.Find(PaymentDetail.PaymentMethod_Id);
                if (payment.Expense_Account_Id == 0 || payment.Expense_Account_Id == null)
                {
                    PaymentDetail.Commution = 0;

                }
                else
                {
                    PaymentDetail.Commution = ((payment.Percentage.Value / 100) * PaymentDetail.Amount) + payment.Operatin_Cost.Value;
                }

                Master.Total_Payment_Method_Cost = PaymentDetail.Commution;
                paymentAmount = Master.Total_Amount.Value + Master.TotalNewTaxs.Value;
                Master.PosPaymentMethods.Add(PaymentDetail);

                ///qr Payment
                ///
                var PaymentQRDetail = new QRPaymentMethod();
                PaymentQRDetail.PaymentMethod_Id = PaymentDetail.PaymentMethod_Id;
                PaymentQRDetail.Amount = PaymentDetail.Amount;
                PaymentQRDetail.Commution = PaymentDetail.Commution;


                qrMaster.QRPaymentMethods.Add(PaymentQRDetail);




            }
            var RealDisc = (Master.DetailPOSInvoices.Select(a => a.Detail_Discount_Amount.Value).Sum() / Master.DetailPOSInvoices.Select(h => h.Product_Price * h.Product_Quantity).Sum()) * 100;
            if (Math.Round(RealDisc, 4, MidpointRounding.AwayFromZero) != Math.Round(Master.Master_Discount.Value, 4, MidpointRounding.AwayFromZero))
            {
                Master.Master_Discount = RealDisc;
            }
            Master.AllowDiscount = MasterVM.AllowDiscount;
            Master.Total_Amount = (Master.Total_Amount + Master.TotalNewTaxs) - Master.AllowDiscount;
            if (Math.Round(Master.Total_Amount.Value, 2, MidpointRounding.AwayFromZero) > Math.Round(paymentAmount, 2, MidpointRounding.AwayFromZero))
            {
                return Tuple.Create(MasterTemp.MasterPOSInvoiceId, Accountingsystem.Resources.MasterPOSInvoice.TotalinvoiceisBigger, 0);
            }
            if (Math.Round(Master.Total_Amount.Value, 2, MidpointRounding.AwayFromZero) < Math.Round(paymentAmount, 2, MidpointRounding.AwayFromZero))
            {
                foreach (var pay in Master.PosPaymentMethods)
                {
                    var dif = paymentAmount - Master.Total_Amount.Value;

                    if (pay.Amount - dif > 0)
                    {
                        pay.Amount = pay.Amount - dif;

                        break;
                    }



                }

            }
            paymentAmount = Master.PosPaymentMethods.Select(a => a.Amount).Sum();
            if (Master.Total_Amount > paymentAmount)
            {
                Master.PosPaymentMethods.FirstOrDefault().Amount = Master.PosPaymentMethods.FirstOrDefault().Amount + (Master.Total_Amount.Value - paymentAmount);
            }
            if (paymentAmount > Master.Total_Amount)
            {
                Master.PosPaymentMethods.FirstOrDefault().Amount = Master.PosPaymentMethods.FirstOrDefault().Amount - (paymentAmount - Master.Total_Amount.Value);
            }

            ///qrForMaster
           
            qrMaster.AllowDiscount = Master.AllowDiscount.Value;
            qrMaster.ApprovalCode = Master.ApprovalCode;
            qrMaster.Company_Id = Master.Company_Id;
            qrMaster.Customer_Id = Master.Customer_Id;
            qrMaster.Description = Master.Description;
            qrMaster.Notes = Master.Notes;
            qrMaster.Employee_Id = Master.Employee_Id.Value;
            qrMaster.Invoice_Date = Master.Invoice_Date;
            qrMaster.Invoice_State = Master.Invoice_State;
            qrMaster.InvoiceType_Id = 3;
            qrMaster.MainrenanceOrder_Id = Master.MainrenanceOrder_Id;
            qrMaster.Master_Discount = Master.Master_Discount.Value;
            qrMaster.PointOfSalePOS_Id = Master.PointOfSalePOS_Id;
            qrMaster.Reference = Master.Reference;
            qrMaster.Serial_Number = Master.Serial_Number;
            qrMaster.Store_ID = Master.Store_ID;
            qrMaster.Subledger_Id = Master.Subledger_Id;
            qrMaster.TotalNewTaxs = Master.TotalNewTaxs.Value;
            qrMaster.Account_Id = Master.Account_Id;
            qrMaster.Total_Amount = Master.Total_Amount.Value;
            qrMaster.InvoiceType = "فاتورة مبيعات نقطة بيع";
            qrMaster.Total_Payment_Method_Cost = Master.Total_Payment_Method_Cost;
            qrMaster.Total_Tax = Master.Total_Tax.Value;
            Master.Total_Cost = realdetails.Select(a => a.Detail_Total_Cost).Sum();
            qrMaster.Total_Cost = Master.Total_Cost.Value;
            qrMaster.Notes = "";
            qrMaster.Number = InvoiceSerial.generateSerialForInvoice();
            qrMaster.DateOfCreate = DateTime.Now;          
            Master.Number = qrMaster.Number;
            Master.Serial_Number = "POS-" + System.Text.RegularExpressions.Regex.Match(Master.Number, @"\d+").Value;
            qrMaster.Serial_Number = Master.Serial_Number;
            db.MasterQRInvoices.Add(qrMaster);
            db.SaveChanges();








            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                
                    Master.QRNumber = qrMaster.Id;
                  
                    db.MasterPOSInvoices.Add(Master);
                    db.PosPaymentMethodTemps.RemoveRange(PaymentTemp);
                    db.DetailPOSInvoiceTemps.RemoveRange(invoiceDetailTemp);
                    db.MasterPOSInvoiceTemps.Remove(MasterTemp);
                    db.SaveChanges();

                    CreateTransactionBatch(Master, UserId);
                    var gl = CreateInvoiceGL(Master);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    db.Dispose();
                }

            }
            return Tuple.Create(0, Resources.MasterPOSInvoice.Therefundinvoicehasbeencreatedsuccessfully, Master.MasterPOSInvoiceId);

        }



        public static Tuple<int, string, int> CreatePostForInvoiceMaster(MasterPOSInvoiceVM MasterVM, String UserId)
        {


            var db = new AccountingContainer();
            MasterPOSInvoice Master = new MasterPOSInvoice();
            Master.Total_Payment_Method_Cost = 0;
            Master.Total_Amount = 0;
            Master.Total_Cost = 0;
            Master.Total_Tax = 0;
            decimal paymentAmount = 0;

            if (MasterVM.Invoice_Date == null || MasterVM.Invoice_Date == "")
            {
                Master.Invoice_Date = DateTime.Now;
            }
            else
            {
                Master.Invoice_Date = Convert.ToDateTime(MasterVM.Invoice_Date);
            }


            Master.PointOfSalePOS_Id = MasterVM.PointOfSalePOS_Id;
            var Point = db.PointOfSales.Find(MasterVM.PointOfSalePOS_Id);
            Master.Company_Id = Point.Company_Id;
            Master.Notes = MasterVM.Notes;
            Master.Description = MasterVM.Description;
            Master.Employee_Id = Point.Employee_Id;
            Master.Invoice_Currency = "ريال سعودي";
            Master.Customer_Id = MasterVM.Customer_Id;
            Master.MainrenanceOrder_Id = MasterVM.MaintenanceOrder_Id;
            Master.Notes = MasterVM.Notes;
            var lastSerial = db.MasterPOSInvoices.OrderByDescending(c => c.MasterPOSInvoiceId).FirstOrDefault();
            if (lastSerial == null)
            {
                Master.Serial_Number = "POS-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                Master.Serial_Number = "POS-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
            }
          
            Master.Master_Discount = MasterVM.Master_Discount;
            Master.Invoice_State = TransactionState.Pending;
            if (MasterVM.RevenueAccount == 0 || MasterVM.RevenueAccount == null)
            {
                Master.Account_Id = 76;
            }
            else
            {
                Master.Account_Id = MasterVM.RevenueAccount;
            }
            Master.Store_ID = Point.Store_Id;
            Master.Subledger_Id = Point.Subledger_Id;
            var MasterTemp = db.MasterPOSInvoiceTemps.Find(MasterVM.MasterPOSInvoiceId);
            var invoiceDetailTemp = new List<DetailPOSInvoiceTemp> { };
            var PaymentTemp = new List<PosPaymentMethodTemp> { };
            if (MasterTemp.DetailPOSInvoiceTemps.FirstOrDefault() == null)
            {
                return Tuple.Create(MasterTemp.MasterPOSInvoiceId, Accountingsystem.Resources.validate.invoiceProductMust, 0);


            }
            Master.Notes = MasterVM.Notes;
            var qrMaster = new MasterQRInvoice();
            qrMaster.QrGuid = Guid.NewGuid();
            qrMaster.DetailQRInvoices = new List<DetailQRInvoice>();
   
            qrMaster.TaxQRInvoices = new List<TaxQRInvoice>();

          
            foreach (var item in MasterTemp.DetailPOSInvoiceTemps.ToList())
            {
                var invoiceDetail = new DetailPOSInvoice();
                var invoiceQRDetail = new DetailQRInvoice();

                if (item.Batch_ID == 0)
                {
                    var product = db.Products.Find(item.Product_Id);
                    if (product.Prod_Type == Prod_Type.خدمة || product.Prod_Type == Prod_Type.مجمع)
                    {
                        item.Batch_ID = product.ProductBatches.FirstOrDefault().Batch_ID;
                    }
                    else if (product.ProductBatches.Where(a => a.Store_ID == Master.Store_ID).FirstOrDefault() == null)
                    {
                        var msg = Accountingsystem.Resources.MasterPOSInvoice.Thereisnotenoughproduct + " " + product.Prod_Name;
                        return Tuple.Create(MasterTemp.MasterPOSInvoiceId, msg, 0);
                    }
                    else if (product.ProductBatches.Where(a => a.Store_ID == Master.Store_ID).FirstOrDefault().Product_Quantity < item.Product_Quantity)
                    {
                        var msg = Accountingsystem.Resources.MasterPOSInvoice.Thereisnotenoughproduct + " " + product.Prod_Name;
                        return Tuple.Create(MasterTemp.MasterPOSInvoiceId, msg, 0);
                    }
                    else
                    {
                        item.Batch_ID = product.ProductBatches.Where(a => a.Store_ID == Master.Store_ID).FirstOrDefault().Batch_ID;
                    }

                }
                invoiceDetail.Batch_ID = item.Batch_ID;
                invoiceDetail.Product_Id = item.Product_Id;
                invoiceDetail.Product_Quantity = item.Product_Quantity;
                invoiceDetail.Detail_Discount = item.Detail_Discount;
                invoiceDetail.Detail_Discount_Amount = item.Detail_Discount_Amount;

                invoiceDetail.Product_Tax = item.Product_Tax;
                var batch = db.ProductBatches.Find(item.Batch_ID);

                invoiceDetail.Detail_Total_Cost = item.Product_Quantity * batch.Product_Cost;
                invoiceDetail.Reverse_Total_Tax = item.Detail_Total_Tax;
                invoiceDetail.Reverse_Quantity = item.Product_Quantity;
                invoiceDetail.Total_Amount = item.Total_Amount;
                invoiceDetail.Reverse_Total_Amount = item.Total_Amount;
                invoiceDetail.Product_Price = item.Product_Price;
                invoiceDetail.Detail_Total_Tax = item.Detail_Total_Tax;
                invoiceDetail.ProductPriceCost = db.ProductBatches.Find(invoiceDetail.Batch_ID).Product_Cost;
                Master.Total_Tax += invoiceDetail.Detail_Total_Tax;
                Master.Total_Cost += invoiceDetail.Detail_Total_Cost;
                Master.Total_Amount += invoiceDetail.Total_Amount;
                Master.Invoice_State = TransactionState.Post;

                Master.DetailPOSInvoices.Add(invoiceDetail);
                invoiceDetailTemp.Add(item);

                ///QR Detail Master
                ///
                invoiceQRDetail.Batch_ID = invoiceDetail.Batch_ID;
                invoiceQRDetail.Product_Id = invoiceDetail.Product_Id;
                invoiceQRDetail.Product_Quantity = invoiceDetail.Product_Quantity;
                invoiceQRDetail.Product_Tax = invoiceDetail.Product_Tax;

                invoiceQRDetail.Detail_Discount = item.Detail_Discount;
                invoiceQRDetail.Detail_Discount_Amount = item.Detail_Discount_Amount;
                invoiceQRDetail.Detail_Total_Cost = invoiceDetail.Detail_Total_Cost;

                invoiceQRDetail.Total_Amount = invoiceDetail.Total_Amount;

                invoiceQRDetail.Product_Price = invoiceDetail.Product_Price;
                invoiceQRDetail.Detail_Total_Tax = invoiceDetail.Detail_Total_Tax;
                invoiceQRDetail.ProductPriceCost = invoiceDetail.ProductPriceCost;
                qrMaster.DetailQRInvoices.Add(invoiceQRDetail);
            }
            var realdetails = getMaterialOCompoundProduct(Master.DetailPOSInvoices.ToList(), Master.Store_ID.Value);
            foreach (var item in realdetails)
            {
                var productBatch = db.ProductBatches.Find(item.Batch_ID);
                if (productBatch.Product_Quantity < item.Product_Quantity && (productBatch.Product.Prod_Type == Prod_Type.منتج || productBatch.Product.Prod_Type == Prod_Type.اولي))
                {
                    var productName = db.Products.Find(item.Product_Id).Prod_Name;

                    var msg = Accountingsystem.Resources.MasterPOSInvoice.Thereisnotenoughproduct + " " + productName;
                    return Tuple.Create(MasterTemp.MasterPOSInvoiceId, msg, 0);

                }

            }

            Master.TotalNewTaxs = 0;
            if (MasterVM.Taxes != null)
            {
                foreach (var tax in MasterVM.Taxes)
                {
                    var taxo = new TaxPOSInvoice();
                    taxo.Tax_Id = tax;

                    taxo.Amount = (db.Taxes.Find(Convert.ToInt32(tax)).Tax_Percentage / 100) * Master.DetailPOSInvoices.Select(a => a.Total_Amount).Sum();
                    Master.TotalNewTaxs += taxo.Amount;

                    Master.TaxPOSInvoices.Add(taxo);

                    ///qr TAx Adding
                    var qrtax = new TaxQRInvoice();
                    qrtax.Tax_Id = tax;

                    qrtax.Amount = taxo.Amount;
                    qrMaster.TaxQRInvoices.Add(qrtax);

                }
            }
      

            Master.AllowDiscount = MasterVM.AllowDiscount;
            Master.Total_Amount = (Master.Total_Amount + Master.TotalNewTaxs) - Master.AllowDiscount;



            ///qrForMaster
            qrMaster.Notes = MasterVM.Notes;
            qrMaster.AllowDiscount = Master.AllowDiscount.Value;
            qrMaster.ApprovalCode = Master.ApprovalCode;
            qrMaster.Company_Id = Master.Company_Id;
            qrMaster.Customer_Id = Master.Customer_Id;
            qrMaster.Description = Master.Description;
            qrMaster.Employee_Id = Master.Employee_Id.Value;
            qrMaster.Invoice_Date = Master.Invoice_Date;
            qrMaster.Invoice_State = Master.Invoice_State;
            qrMaster.InvoiceType_Id = 3;
            qrMaster.MainrenanceOrder_Id = Master.MainrenanceOrder_Id;
            qrMaster.Master_Discount = Master.Master_Discount.Value;
            qrMaster.PointOfSalePOS_Id = Master.PointOfSalePOS_Id;
            qrMaster.Reference = Master.Reference;
            qrMaster.Serial_Number = Master.Serial_Number;
            qrMaster.Store_ID = Master.Store_ID;
            qrMaster.Subledger_Id = Master.Subledger_Id;
            qrMaster.TotalNewTaxs = Master.TotalNewTaxs.Value;
            qrMaster.Account_Id = Master.Account_Id;
            qrMaster.Total_Amount = Master.Total_Amount.Value;
            qrMaster.InvoiceType = "فاتورة مبيعات نقطة بيع";
            qrMaster.Total_Payment_Method_Cost = Master.Total_Payment_Method_Cost;
            qrMaster.Total_Tax = Master.Total_Tax.Value;
            Master.Total_Cost = realdetails.Select(a => a.Detail_Total_Cost).Sum();
            qrMaster.Total_Cost = Master.Total_Cost.Value;
            qrMaster.Notes = "";
            qrMaster.Number = InvoiceSerial.generateSerialForInvoice();
            Master.Number = qrMaster.Number;
            qrMaster.DateOfCreate = DateTime.Now;
            db.MasterQRInvoices.Add(qrMaster);
            db.SaveChanges();








            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {

                    Master.QRNumber = qrMaster.Id;

                    db.MasterPOSInvoices.Add(Master);
                    db.PosPaymentMethodTemps.RemoveRange(PaymentTemp);
                    db.DetailPOSInvoiceTemps.RemoveRange(invoiceDetailTemp);
                    db.MasterPOSInvoiceTemps.Remove(MasterTemp);
                    db.SaveChanges();

                    CreateTransactionBatch(Master, UserId);
                    var gl = CreateInvoiceGL(Master);
                    if (MasterVM.sewingId > 0)
                    {
                        var sewing = db.Sewings.Find(MasterVM.sewingId);
                        var SewingList = db.Sewings.Where(a => a.CustomerId == sewing.CustomerId && a.MasterPOSInvoiceMasterPOSInvoiceId == null).ToList();
                        foreach(var sew in SewingList)
                        {
                            sew.MasterPOSInvoiceMasterPOSInvoiceId = Master.MasterPOSInvoiceId;
                            db.Entry(sew).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    db.Dispose();
                }

            }
            return Tuple.Create(0, Resources.MasterPOSInvoice.Therefundinvoicehasbeencreatedsuccessfully, Master.MasterPOSInvoiceId);

        }
        public static bool AddmuchTax(List<string> taxList, int master_Id)
        {
            var db = new AccountingContainer();
            var Master = db.MasterPOSInvoiceTemps.Find(master_Id);
            Master.TotalNewTaxs = 0;
            if (taxList != null)
            {
                foreach (var tax in taxList)
                {
                    var totalAmount = Master.DetailPOSInvoiceTemps.Select(a => a.Total_Amount).Sum();
                    var percntage = (db.Taxes.Find(Convert.ToInt32(tax)).Tax_Percentage / 100);

                    Master.TotalNewTaxs += totalAmount * percntage;
                }
            }
            else
            {
                Master.TotalNewTaxs = 0;
            }
            db.Entry(Master).State = EntityState.Modified;
            db.SaveChanges();
            return true;

        }


        /// <summary>
        /// make atransaction batch for that in invoice
        /// </summary>
        /// <param name="master"></param>
        /// <param name="UserId"></param>
        public static void CreateTransactionBatch(MasterPOSInvoice master, String UserId)
        {
            var trans = new TransactionBatch();
            trans.Description = "فاتورة فورية";
            trans.Date = DateTime.Now;
            trans.OperationName = "فاتورة نقطة بيع";
            // TODO: Edit UserId
            trans.UserId = UserId;
            trans.OperationId = master.PointOfSalePOS_Id;
            trans.Operation_Code = master.Serial_Number;
            TransactionBatch.AddNewTransaction(trans);
        }

        /// <summary>
        /// preapare paymentmethod that depend on point of sale that choosing 
        /// </summary>
        /// <param name="POS_ID"></param>
        /// <returns>PosPaymentMethodVM</returns>
        public static PosPaymentMethodVM PreparePaymentMethod(int POS_ID)
        {
            var db = new AccountingContainer();
            var paymentMethodVM = new PosPaymentMethodVM();
            var POS = db.PointOfSales.Find(POS_ID);

            var batchs = db.POSDetailPayments.Where(batch => batch.PointOfSalePOS_Id == POS_ID).ToList();
            paymentMethodVM.PaymentList = new SelectList(batchs, "PaymentMethod_Id", "PaymentMethod_Name");
            paymentMethodVM.Amount = 0;
            return paymentMethodVM;

        }
        /// <summary>
        /// get all all data of payment temp by invoice id
        /// </summary>
        /// <param name="paymenttemp"></param>
        /// <returns>MasterPOSInvoicePaymentTempVM</returns>
        public static MasterPOSInvoicePaymentTempVM Getalldatapaymenttemp(int paymenttemp)
        {
            var db = new AccountingContainer();
            var paymentDetail = new MasterPOSInvoicePaymentTempVM();


            paymentDetail.paymentvm = db.PosPaymentMethodTemps.Where(p => p.MasterPOSInvoiceTempMasterPOSInvoiceId == paymenttemp).ToList();
            paymentDetail.invoice_Id = paymenttemp;
            return paymentDetail;
        }



        /// <summary>
        /// prepare of detail of POS invoice depend on Point of sale data 
        /// </summary>
        /// <param name="POS_ID"></param>
        /// <returns>DetailPOSInvoiceVM</returns>
        public static DetailPOSInvoiceVM prepareDetailOfInvoice(int POS_ID)
        {
            var db = new AccountingContainer();
            var DetailOfInvoice = new DetailPOSInvoiceVM();
          
            List<object> newList = new List<object>();
            var members = db.Taxes.Where(a => a.Tax_Code.StartsWith("02")&&a.IsActive!=false);
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.Tax_ID,
                    Name = member.Tax_Name + " % " + member.Tax_Percentage
                });
            DetailOfInvoice.ProducttaxList = new SelectList(newList, "Id", "Name");
            var POS = db.PointOfSales.Find(POS_ID);


   
            var productList = (from ep in db.ProductBatches.Where(a => a.IsActive != false && a.Product_Quantity >0&&a.Store_ID== POS.Store_Id&& a.Product.IsActive != false && a.Product.Prod_Type!=Prod_Type.اولي)
                             


                               select new
                               {
                                   Id = ep.Batch_ID,
                                   Name = ep.Product.Prod_Name + "  " + ep.Product.Bar_Code,
                                   //quantity = ep.Product_Quantity,
                                   //isproduct = ep.Product.Prod_Type,
                                   //StoreID = ep.Store_ID

                               }).ToList();
            DetailOfInvoice.BatchList = new SelectList(productList, "Id", "Name");

            DetailOfInvoice.Product_Quantity = 1;
            return DetailOfInvoice;
        }
        /// <summary>
        /// get all temp of that invoice by using its id
        /// </summary>
        /// <param name="DetalposId"></param>
        /// <returns></returns>
        public static MasterPOSInvoiceDetailTempVM DetailPosdatatemp(int DetalposId)
        {
            var db = new AccountingContainer();
            var invoicedetalTemp = new MasterPOSInvoiceDetailTempVM();

            var invoice = db.MasterPOSInvoiceTemps.Find(DetalposId);
            invoicedetalTemp.invoiceTemp = invoice.DetailPOSInvoiceTemps.ToList();
            invoicedetalTemp.invoice_Id = DetalposId;
            List<object> newList = new List<object>();

            var members = db.Taxes.Where(a => a.Tax_Code.StartsWith("02"));
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.Tax_ID,
                    Name = member.Tax_Name + " % " + member.Tax_Percentage
                });
            invoicedetalTemp.TaxList = new SelectList(newList, "Id", "Name");
            invoicedetalTemp.AllowDiscount = invoice.AllowDiscount.Value;
            invoicedetalTemp.TotalNewTax = invoice.TotalNewTaxs.Value;
            return invoicedetalTemp;
        }

        /// <summary>
        /// add to list of Payment temp
        /// </summary>
        /// <param name="PosPayment"></param>
        /// <returns>string</returns>
        public static string CreatePayment(PosPaymentMethodVM PosPayment)
        {
            var db = new AccountingContainer();
            if (PosPayment.PaymentMethod_Id == null || PosPayment.PaymentMethod_Id == 0)
            {
                return Resources.MasterPOSInvoice.ChoosePayment;
            }
            if (PosPayment.Amount == 0)
            {
                return Resources.MasterPOSInvoice.EnterAmount;
            }
            var Pos = db.PosPaymentMethodTemps.Where(a => a.PaymentMethod_Id == PosPayment.PaymentMethod_Id && a.MasterPOSInvoiceTempMasterPOSInvoiceId == PosPayment.MasterPOSInvoiceId).ToList();
            if (Pos.FirstOrDefault() != null)
            {
                return Resources.MasterPOSInvoice.ThisPaymentIsOnList;
            }



                var paytemp = new PosPaymentMethodTemp();
                 paytemp.MasterPOSInvoiceTempMasterPOSInvoiceId = PosPayment.MasterPOSInvoiceId;
                 paytemp.Amount = PosPayment.Amount;
                   
                 paytemp.PaymentMethod_Id = PosPayment.PaymentMethod_Id.Value;
            var payment = db.PaymentMethods.Find(PosPayment.PaymentMethod_Id);
            if (payment.Expense_Account_Id == 0 || payment.Expense_Account_Id == null)
            {
                paytemp.Commution = 0;

            }
            else
            {
                paytemp.Commution=((payment.Percentage/100)*paytemp.Amount)+payment.Operatin_Cost.Value;
            }

                db.PosPaymentMethodTemps.Add(paytemp);
                db.SaveChanges();
                return Resources.MasterPOSInvoice.AddSuccessfully;
         
        }

        /// <summary>
        /// add to list of invoice temp
        /// </summary>
        /// <param name="detailVm"></param>
        /// <returns>string</returns>

        public static string Adddetailpostodetailtemp(DetailPOSInvoiceVM detailVm)
        {
            //if (detailVm.Batch_ID == 0 || detailVm.Batch_ID == null)
            //{
            //    return "ادخل الصنف";
            //}
            var db = new AccountingContainer();
          



           
            var detailtemp = db.MasterPOSInvoiceTemps.Find(detailVm.Master_Id);
            var detailposttemp = new DetailPOSInvoiceTemp();
            var batch = db.ProductBatches.Find(detailVm.Batch_ID);

         
                //var batch = db.ProductBatches.Find(detailVm.Product_Id);
                detailposttemp.Batch_ID = batch.Batch_ID;
                detailposttemp.Product_Id = batch.Product_ID;
            
            var detalpos = detailtemp.DetailPOSInvoiceTemps;
            var posdetailtemp = detalpos.Where(p => p.MasterPOSInvoiceTempMasterPOSInvoiceId == detailtemp.MasterPOSInvoiceId && p.Product_Id == detailposttemp.Product_Id).ToList();
            if (posdetailtemp.FirstOrDefault() !=null)
            {
                var old = posdetailtemp.FirstOrDefault();
                var oldstatic= posdetailtemp.FirstOrDefault().Total_Amount;
                old.Product_Price = posdetailtemp.FirstOrDefault().Product_Price;
                old.Product_Quantity += 1;
                if (batch.Product_Quantity < old.Product_Quantity && batch.Product.Prod_Type == Prod_Type.منتج)
                {
                    return Resources.MasterPOSInvoice.QuantityIsBigger;

                }
                
                    

                

                detailposttemp.Detail_Total_Cost= batch.Product.Prod_Cost_Price.Value * old.Product_Quantity;
                if(batch.Product.Tax_Id!=null&& batch.Product.Tax_Id != 0)
                {
                    detailposttemp.Product_Tax = batch.Product.Tax_Id.Value;
                }
               
                decimal totalprice = old.Product_Price * old.Product_Quantity;
                decimal discound = totalprice * (detailVm.Discount / 100);
                decimal afterdec = totalprice - discound;
                old.TotalBeforeDisc = totalprice;
                old.TotalDiscount = discound;
                old.Detail_Total_Cost = old.Product_Quantity * batch.Product_Cost;
                var prodtax = db.Taxes.Find(old.Product_Tax);
                if (prodtax == null)
                {
                    prodtax = new Tax();
                    prodtax.Tax_Percentage = 0;
                    prodtax.Tax_ID = 0;
                    old.Detail_Total_Tax = 0;
                    old.Total_Amount = (totalprice + 0) - discound;
                }
                else
                {
                    old.Detail_Total_Tax = afterdec * (prodtax.Tax_Percentage / 100);
                    old.Total_Amount = (totalprice + (afterdec * (prodtax.Tax_Percentage / 100))) - discound;
                }
                old.Detail_Discount = detailVm.Discount;
                old.Detail_Discount_Amount = discound;

                //detailtemp.DetailPOSInvoiceTemps.Add(detailposttemp);
                db.Entry(old).State = EntityState.Modified;




                detailtemp.Total_Amount = (old.Total_Amount + detailtemp.Total_Amount) - oldstatic ;
               

                db.Entry(detailtemp).State = EntityState.Modified;
                db.SaveChanges();

                return Resources.MasterPOSInvoice.AddSuccessfully;
            }
            
            if(batch.Product_Quantity< detailVm.Product_Quantity&&batch.Product.Prod_Type==Prod_Type.منتج)
            {
                return Resources.MasterPOSInvoice.QuantityIsBigger;

            }
            else
            {
                detailposttemp.Product_Price = InvoiceMaster.GetPriceSale(batch.Store_ID.Value, batch.Product_ID, detailVm.CustomerID);

                //detailposttemp.Product_Price = batch.Product.Sale_Price.Value;
                detailposttemp.Product_Quantity =1;
                if(batch.Product.Tax_Id==null|| batch.Product.Tax_Id == 0)
                {
                    detailVm.Product_Tax = 0;
                }
                else
                {
                    detailVm.Product_Tax = batch.Product.Tax_Id;
                }
                
                    detailposttemp.Product_Tax = detailVm.Product_Tax.Value;
               
                
                decimal totalprice = detailposttemp.Product_Quantity * detailposttemp.Product_Price;
                decimal discound = totalprice * (detailVm.Discount / 100);
                decimal afterdec = totalprice - discound;
                detailposttemp.TotalBeforeDisc= totalprice;
                detailposttemp.TotalDiscount = discound;
                detailposttemp.Detail_Discount = detailVm.Discount;
                detailposttemp.Detail_Discount_Amount = discound;
                var prodtax = db.Taxes.Find(detailVm.Product_Tax);
                if (prodtax == null)
                {
                    detailposttemp.Detail_Total_Tax = 0;
                }
                else
                {
                    detailposttemp.Detail_Total_Tax = afterdec * (prodtax.Tax_Percentage / 100);
                    detailposttemp.Product_Tax = detailVm.Product_Tax.Value;
                }
               
                detailposttemp.Total_Amount = (totalprice + detailposttemp.Detail_Total_Tax) - discound;
                detailposttemp.Detail_Total_Cost = batch.Product_Cost * detailposttemp.Product_Quantity;
                detailtemp.DetailPOSInvoiceTemps.Add(detailposttemp);
               
               
                    detailtemp.Total_Amount += detailposttemp.Total_Amount;
                
                
               
                db.Entry(detailtemp).State = EntityState.Modified;
                


                db.SaveChanges();
                return Resources.MasterPOSInvoice.AddSuccessfully;

            }
        }
     
        /// <summary>
        /// delete from invoice temp by its id
        /// </summary>
        /// <param name="Detailid"></param>
        /// <returns></returns>
        public static String DeleteFromTemp(int Detailid)

        {
            var db = new AccountingContainer();
            var Detail = db.PosPaymentMethodTemps.Find(Detailid);
           
            db.PosPaymentMethodTemps.Remove(Detail);
            db.SaveChanges();
            return Resources.MasterPOSInvoice.DeleteSuccessfully;
        }

        /// <summary>
        /// delete payment method temp by it id
        /// </summary>
        /// <param name="DetailTempid"></param>
        /// <returns></returns>

        public static decimal DeleteTempForPay(int DetailTempid)

        {
            var db = new AccountingContainer();
            var Detail = db.DetailPOSInvoiceTemps.Find(DetailTempid);
            var master = db.MasterPOSInvoiceTemps.Find(Detail.MasterPOSInvoiceTempMasterPOSInvoiceId);

            master.Total_Amount -= Detail.Total_Amount;
           
           
            db.Entry(master).State = EntityState.Modified;






            db.DetailPOSInvoiceTemps.Remove(Detail);
            db.SaveChanges();
            decimal discount = 0;
            var salestemp = db.MasterPOSInvoiceTemps.Find(master.MasterPOSInvoiceId);
            try
            {
                discount = salestemp.DetailPOSInvoiceTemps.Select(a => a.Detail_Discount_Amount.Value).Sum() / salestemp.DetailPOSInvoiceTemps.Select(a => a.Product_Quantity * a.Product_Price).Sum();
            }
            catch (Exception ex)
            {
                return 0;
            }
            return discount * 100;


        }
        /// <summary>
        /// make update of invoice on change discount
        /// </summary>
        /// <param name="discount"></param>
        /// <param name="Invoice_Id"></param>
        /// <returns>string</returns>
        public static string changeDetailOfInvoice(decimal discount, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.MasterPOSInvoiceTemps.Find(Invoice_Id);
            var detail = db.DetailPOSInvoiceTemps.Where(a => a.MasterPOSInvoiceTempMasterPOSInvoiceId == Invoice_Id).ToList();
            decimal total = 0;
            foreach (var item in detail)
            {
                item.TotalBeforeDisc = item.Product_Quantity * item.Product_Price;
                item.TotalDiscount = item.TotalBeforeDisc * (discount / 100);
                item.Detail_Discount = discount;
                item.Detail_Discount_Amount = item.TotalDiscount;
                var beforeTax = item.TotalBeforeDisc - item.TotalDiscount;
                if (item.Product_Tax == 0)
                {
                    item.Detail_Total_Tax = 0;
                }
                else
                {
                    var tax = db.Taxes.Find(item.Product_Tax).Tax_Percentage;

                    item.Detail_Total_Tax = beforeTax.Value * (tax / 100);
                }

                item.Total_Amount = (item.TotalBeforeDisc.Value - item.TotalDiscount.Value) + item.Detail_Total_Tax;
                db.Entry(item).State = EntityState.Modified;
                total += item.Total_Amount;
            }
            invoiceTemp.Total_Amount = total;
            db.Entry(invoiceTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "تم تعديل الخصم بنجاح";

        }

        public static decimal changeSpecificDiscount(int detail_Id, decimal discountdetail)
        {
            var db = new AccountingContainer();
            var detail = db.DetailPOSInvoiceTemps.Find(detail_Id);
            var master = detail.MasterPOSInvoiceTemp;

            detail.Detail_Discount = discountdetail;

            decimal totalprice = detail.Product_Price * detail.Product_Quantity;
            decimal discound = totalprice * (discountdetail / 100);
            decimal afterdec = totalprice - discound;
            detail.TotalBeforeDisc = totalprice;
            detail.TotalDiscount = discound;
            if (detail.Product_Tax == 0)
            {
                detail.Detail_Total_Tax = 0;
            }
            else
            {
                var prodtax = db.Taxes.Find(detail.Product_Tax);
                detail.Detail_Total_Tax = afterdec * (prodtax.Tax_Percentage / 100);
            }
            detail.Detail_Discount = discountdetail;
            detail.Detail_Discount_Amount = detail.TotalDiscount;
            detail.Detail_Discount_Amount = (detail.Product_Price * detail.Product_Quantity) * (detail.Detail_Discount / 100);
            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;

            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            var ee = master.DetailPOSInvoiceTemps.Select(a => a.Product_Price * a.Product_Quantity * (a.Detail_Discount.Value / 100)).Sum();
            var bb = master.DetailPOSInvoiceTemps.Select(a => a.Product_Price * a.Product_Quantity).Sum();
            decimal masdiscount = (ee / bb) * 100;
            return masdiscount;

        }
        /// <summary>
        /// make change of invoice on change allow discount
        /// </summary>
        /// <param name="AllowDiscount"></param>
        /// <param name="Invoice_Id"></param>
        /// <returns></returns>
        public static decimal updateDiscountDebendOnDiscountAmount(decimal discount, int Invoice_Id)
        {
            var db = new AccountingContainer();
        var invoiceTemp = db.MasterPOSInvoiceTemps.Find(Invoice_Id);
        var pecentage = (discount / invoiceTemp.DetailPOSInvoiceTemps.Sum(a => a.Product_Price * a.Product_Quantity)) * 100;
        var detail = db.DetailPOSInvoiceTemps.Where(a => a.MasterPOSInvoiceTempMasterPOSInvoiceId == Invoice_Id).ToList();
        decimal total = 0;
            foreach (var item in detail)
            {
                item.TotalBeforeDisc = item.Product_Quantity* item.Product_Price;
        item.TotalDiscount = item.TotalBeforeDisc* (pecentage / 100);
                item.Detail_Discount = pecentage;

             
                item.Detail_Discount_Amount = item.TotalDiscount;
                decimal? beforeTax = 0;
                if (item.Product_Tax == 0)
                {
                    item.Detail_Total_Tax = 0;
                }
                else
                {
                    var tax = db.Taxes.Find(item.Product_Tax).Tax_Percentage;
                    beforeTax = item.TotalBeforeDisc - item.TotalDiscount;
                    item.Detail_Total_Tax = beforeTax.Value* (tax / 100);
                }


            item.Total_Amount = (item.TotalBeforeDisc.Value - item.TotalDiscount.Value) + item.Detail_Total_Tax;
                db.Entry(item).State = EntityState.Modified;
                total += item.Total_Amount;
            }
            invoiceTemp.Total_Amount = total;
            db.Entry(invoiceTemp).State = EntityState.Modified;
            db.SaveChanges();
            return pecentage;

        }
        public static string changeDetailOfInvoiceForAllDiscount(decimal AllowDiscount, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.MasterPOSInvoiceTemps.Find(Invoice_Id);
            invoiceTemp.AllowDiscount = AllowDiscount;
            db.Entry(invoiceTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "تم عمل الخصم المسموح به";






        }
        /// <summary>
        /// get all detail of invoice temp by it id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
            public static MasterPOSInvoiceTemp getTempById(int id)
        {
            var db = new AccountingContainer();
            var temp = db.MasterPOSInvoiceTemps.Find(id);
            return temp;



        }
        /// <summary>
        /// get detail of invoice by using its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MasterPOSInvoice</returns>
        public static MasterPOSInvoice getinvoiceByid(int id)
        {
            var db = new AccountingContainer();
            var invoice = db.MasterPOSInvoices.Find(id);
            return invoice;



        }
        /// <summary>
        /// get all invoices of pos if user is admin else see his invoices only
        /// </summary>
        /// <param name="user_Id"></param>
        /// <returns></returns>
        public static List<MasterPOSInvoice> getAllDataForInvoices(string user_Id,bool differentInvoice)
        {
            var db = new AccountingContainer();
            var List = new List<MasterPOSInvoice> { };
            var Emp = db.Employees.Where(a => a.Reg_ID == user_Id).FirstOrDefault();

            if (Emp != null && Emp.SystemRoleId!=6)
            {

                var Point = db.PointOfSales.Where(a => a.Employee_Id == Emp.Emp_Id).FirstOrDefault();


                if (Point != null)
                {
                    List = Point.MasterPOSInvoices.ToList();

                }
                else
                {

                    List = db.MasterPOSInvoices.ToList();
                }
            }
            else
            {

                List = db.MasterPOSInvoices.ToList();
            }
           
            
            return List;



        }
        
        /// <summary>
        /// get the Price of product depend on customer slice
        /// </summary>
        /// <param name="proId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static SliceHelper getProductPrice(int proId, int customer)
        {
            var db = new AccountingContainer();

           
            var slice = new SliceHelper();
            if (proId==0)
            {
                return slice;
            }
            var data = db.ProductBatches.Find(proId).Product_ID;
            var product = db.Products.Find(data);
            if (customer == null || customer == 0)
            {
                slice.price = product.Sale_Price.Value;
                
                
            }
            else
            {
                var cus = db.Customers.Find(customer).SlicesSlice_Id;
                var priceSlice = db.SlicesProducts.Where(a => a.SlicesSlice_Id == cus && a.Product_ID == data).FirstOrDefault();
                if (priceSlice == null)
                {
                    slice.price = product.Sale_Price.Value;
                   

                }
                else
                {
                    slice.price = priceSlice.Slice_Price;
                  
                }

                

               
            }
            if (product.Tax_Id != null)
            {
                slice.Tax = product.Tax_Id.Value;
            }
            else
            {
                slice.Tax = db.Taxes.Where(a => a.Tax_Code.StartsWith("02")).FirstOrDefault().Tax_ID;
            }
            return slice;
        }
        /// <summary>
        /// Preparing for make gl for invoice 
        /// </summary>
        /// <param name="masterVM"></param>
        /// <returns></returns>
        public static String UpdateInvoiceMaster(MasterPOSInvoiceVM masterVM)
        {
            var db = new AccountingContainer();


            MasterPOSInvoice Master = db.MasterPOSInvoices.Find(masterVM.MasterPOSInvoiceId);
            OperationRun.Addnewoperation(Master.Serial_Number);
            var operation = db.OperationRuns.Where(a => a.Code == Master.Serial_Number).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(Master.Serial_Number);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }
            // Edit master invoice state to the new state

            if (Master.Invoice_State == masterVM.Invoice_State)
            {
                OperationRun.DeleteOperation(Master.Serial_Number);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + Master.Invoice_State + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            else
            {
                Master.Invoice_State = masterVM.Invoice_State;
            }
            db.Entry(Master).State = EntityState.Modified;

          
            var msg = MasterPOSInvoice.CreateInvoiceGL(Master);
            db.SaveChanges();
            OperationRun.DeleteOperation(Master.Serial_Number);
            db.Dispose();
            return msg;
        }
        /// <summary>
        /// make gl for POS invoice master
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        public static String CreateInvoiceGL(MasterPOSInvoice master)
        {
            var msg = "";
            var db = new AccountingContainer();
            GLMaster glMaster = new GLMaster();
            glMaster.GLCode = master.Serial_Number;
            glMaster.GL_Date = master.Invoice_Date.Value;
            glMaster.GL_Desc = master.Description;
            glMaster.Company_Id = master.Company_Id.Value;
            glMaster.Currency_Id = db.Currencies.FirstOrDefault().Currency_Type;
            glMaster.GLType_Id = "مبيعات نقطة بيع";
            glMaster.GLState = TransactionState.Pending;
            glMaster.Auto = true;
            decimal TotalPaymentAmount = 0; 
          
            GLDetail AccountReceivableGL = new GLDetail(),
              revenueGL = new GLDetail(),
              
              inventoryGL = new GLDetail(),
              totalCostGL = new GLDetail(),
            AccountCustomerGL = new GLDetail(),
                AccountCustomerGLDebit = new GLDetail(),
            discount = new GLDetail(),
            fixedDiscount = new GLDetail();

           

            var ChartOfAccounRevenue = db.ChartOfAccountings.Find(master.Account_Id.Value);
            revenueGL.Account_Id = ChartOfAccounRevenue.Account_Id;
            revenueGL.Account_Name = ChartOfAccounRevenue.Account_Name;
            revenueGL.SubLedger_Id = master.Subledger_Id;
           
            

            var store = db.Stores.Find(master.Store_ID);
            var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).First();
            inventoryGL.Account_Id = ChartOfAccounInventory.Account_Id;
            inventoryGL.Account_Name = ChartOfAccounInventory.Account_Name;

            var ChartOfAccounCost = db.ChartOfAccountings.Find(105);
            totalCostGL.Account_Id = ChartOfAccounCost.Account_Id;
            totalCostGL.Account_Name = ChartOfAccounCost.Account_Name;
            totalCostGL.SubLedger_Id = master.Subledger_Id;

            var DiscountCost = db.ChartOfAccountings.Find(78);
            discount.Account_Id = DiscountCost.Account_Id;
            discount.Account_Name = DiscountCost.Account_Name;
            discount.SubLedger_Id = master.Subledger_Id;

            var chartFixedDiscount = db.ChartOfAccountings.Find(184);
            fixedDiscount.Account_Id = chartFixedDiscount.Account_Id;
            fixedDiscount.Account_Name = chartFixedDiscount.Account_Name;
            fixedDiscount.SubLedger_Id = master.Subledger_Id;

            if (master.Customer_Id != null)
            {
                var ChartOfAccounPay = db.ChartOfAccountings.Find(37);
                AccountCustomerGL.Account_Id = ChartOfAccounPay.Account_Id;
                AccountCustomerGL.Account_Name = ChartOfAccounPay.Account_Name;
                AccountCustomerGLDebit.Account_Id = ChartOfAccounPay.Account_Id;
                AccountCustomerGLDebit.Account_Name = ChartOfAccounPay.Account_Name;
                var customer = db.Customers.Find(master.Customer_Id);
                var customerSubledger = db.Accounts.Where(k => k.Account_Name == customer.Customer_Name).FirstOrDefault();
                AccountCustomerGL.Account_Child_ID = customerSubledger.Accounts_Id;
                AccountCustomerGL.Account_CHild_Name = customerSubledger.Account_Name;
                AccountCustomerGLDebit.Account_Child_ID = customerSubledger.Accounts_Id;
                AccountCustomerGLDebit.Account_CHild_Name = customerSubledger.Account_Name;
            }
            var RealDetailList = getMaterialOCompoundProduct(master.DetailPOSInvoices.ToList(), master.Store_ID.Value);

            switch (master.Invoice_State)
            {
                //Post Invoice
                case TransactionState.Post:
                    // Customer GL

                    // Revenue GL

                    // Taxes GL

                    glMaster.GLState = TransactionState.Post;
                    var gl = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode && a.GLState == glMaster.GLState).FirstOrDefault();
                    if (gl != null)
                    {
                        return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                    }
                    if (master.PosPaymentMethods.FirstOrDefault() != null)
                    {
                        AccountCustomerGL.GLDetail_Credit = master.Total_Amount.Value;
                    }
                    AccountCustomerGLDebit.GLDetail_Debit = master.Total_Amount.Value; 
                    
                    // Inventory GL
                    inventoryGL.GLDetail_Credit = master.Total_Cost.Value;
                   
                   
                    fixedDiscount.GLDetail_Debit = master.AllowDiscount.Value;
                    foreach (var detail in master.DetailPOSInvoices)
                    {
                        var TotalBefore = detail.Product_Price * detail.Product_Quantity;
                        revenueGL.GLDetail_Credit += TotalBefore;
                    }
                    discount.GLDetail_Debit = revenueGL.GLDetail_Credit * (master.Master_Discount.Value / 100);

                    db.ProductBatches.Load();
                    var errorMessage = new List<string> { };
                    foreach (var detail in RealDetailList)
                    {

                        ProductBatches detailBatch = db.ProductBatches.Find(detail.Batch_ID);
                        if (detailBatch.Product.Prod_Type == Prod_Type.اولي||detailBatch.Product.Prod_Type == Prod_Type.منتج)
                        {
                            if (detail.Product_Quantity > detailBatch.Product_Quantity)
                            {
                                var productName = db.Products.Find(detail.Product_Id).Prod_Name;
                                master.Invoice_State = TransactionState.Pending;
                                db.SaveChanges();
                                errorMessage.Add(Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Thereisnotenoughproduct + productName);

                            }
                        }
                    }
                    if (errorMessage.Count() != 0)
                    {


                        string combindedString = string.Join("\n", errorMessage.ToArray());
                        return combindedString;
                    }
                    foreach (var detail in RealDetailList)
                    {

                        ProductBatches detailBatch = db.ProductBatches.Find(detail.Batch_ID);
                        if (detailBatch.Product.Prod_Type == Prod_Type.اولي || detailBatch.Product.Prod_Type == Prod_Type.منتج)
                        {
                            
                            detailBatch.Product_Quantity -= detail.Product_Quantity;
                            detailBatch.Batch_Cost -= detail.ProductPriceCost * detail.Product_Quantity;
                            if (detailBatch.Product_Quantity == 0)
                            {
                                detailBatch.Product_Cost = 0;
                            }
                            else
                            {
                                detailBatch.Product_Cost = detailBatch.Batch_Cost / detailBatch.Product_Quantity;
                            }
                            db.Entry(detailBatch).State = EntityState.Modified;


                            var storecard = new StoreCard();
                            storecard.CostPrice = detail.Detail_Total_Cost / detail.Product_Quantity;
                            storecard.Date = DateTime.Now;
                            storecard.Store_Id = master.Store_ID.Value;
                            storecard.Quantity = detail.Product_Quantity;
                            storecard.SalePrice = detail.Product_Price;
                            storecard.Product_Id = detail.Product_Id;
                            storecard.IncreaseQuantity = false;
                            storecard.TypeOfDocument = "فاتورة مبيعات نقطة بيع";
                            storecard.Document_Serial = master.Serial_Number;
                            storecard.TotalCost = detail.Detail_Total_Cost;
                            storecard.TotalAmount = detail.Total_Amount;
                            storecard.Account_Id = master.Customer_Id;
                            storecard.Subledger_Id = master.Subledger_Id;
                            Store.AddinginstorCard(storecard);
                        }

                        else
                        {

                        }
                    }
                    // Total Cost GL
                    totalCostGL.GLDetail_Debit = master.Total_Cost.Value;
                    foreach (var Payment in master.PosPaymentMethods)
                    {
                        var paymentGL = new GLDetail();
                        var bankAccount = db.PaymentMethods.Find(Payment.PaymentMethod_Id);
                        var ChartOfAccounRecive = db.ChartOfAccountings.Find(bankAccount.Bank_Account_Id);
                        paymentGL.Account_Id = ChartOfAccounRecive.Account_Id;
                        paymentGL.Account_Name = ChartOfAccounRecive.Account_Name;                   
                        paymentGL.GLDetail_Debit = Payment.Amount - Payment.Commution;
                        paymentGL.GLDetail_Credit = 0;
                        TotalPaymentAmount += Payment.Amount;
                        glMaster.GLDetails.Add(paymentGL);
                        if (Payment.Commution != 0)
                        {
                            var bankcomution = new GLDetail();
                            var a = db.PaymentMethods.Find(Payment.PaymentMethod_Id).Expense_Account_Id;
                            var bank = db.ChartOfAccountings.Find(a);
                            bankcomution.Account_Id = bank.Account_Id;
                            bankcomution.Account_Name = bank.Account_Name;
                            bankcomution.SubLedger_Id = master.Subledger_Id;
                            bankcomution.GLDetail_Debit = Payment.Commution;
                            bankcomution.GLDetail_Credit = 0;
                            glMaster.GLDetails.Add(bankcomution);
                        }
                    }
                    if (master.Total_Amount< TotalPaymentAmount) {
                        var differentOfPayment = new GLDetail();
                         differentOfPayment.GLDetail_Credit = TotalPaymentAmount - master.Total_Amount.Value;
                        differentOfPayment.GLDetail_Debit = 0;
                        differentOfPayment.Account_Id = 73;
                        differentOfPayment.Account_Name = db.ChartOfAccountings.Find(73).Account_Name;
                        differentOfPayment.SubLedger_Id = master.Subledger_Id;
                        glMaster.GLDetails.Add(differentOfPayment);

                    }
                    if (master.TaxPOSInvoices.FirstOrDefault() != null)
                    {
                        foreach (var tax in master.TaxPOSInvoices)
                        {
                            var gldetailTax = new GLDetail();
                            var taxs = db.Taxes.Find(Convert.ToInt32(tax.Tax_Id));
                            var AccountTax = db.ChartOfAccountings.Where(z => z.Account_Code == taxs.Tax_Code).FirstOrDefault();
                            gldetailTax.Account_Id = AccountTax.Account_Id;
                            gldetailTax.Account_Name = AccountTax.Account_Name;
                            gldetailTax.GLDetail_Credit = tax.Amount;
                            gldetailTax.GLDetail_Debit = 0;
                            glMaster.GLDetails.Add(gldetailTax);


                        }



                    }
                    break;
                // Void Invoice
                case TransactionState.Void:
                    // Customer GL
                    glMaster.GLType_Id = "عكس مبيعات نقطة بيع";
                    glMaster.GLState = TransactionState.Void;
                    var glcheck = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode && a.GLState == glMaster.GLState).FirstOrDefault();
                    if (glcheck != null)
                    {
                        return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                    }

                    if (master.ReversePOSInvoices.FirstOrDefault() != null)
                    {
                        msg = Resources.MasterPOSInvoice.RelatedInvoice;
                        master.Invoice_State = TransactionState.Post;
                        db.SaveChanges();
                        return msg;
                    }
                    if (master.HasOff == true)
                    {

                        msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.UnderCloseing;
                        master.Invoice_State = TransactionState.Post;
                        db.SaveChanges();
                        return msg;
                    }
                    else
                    {
                        return MakeVoidForSales(master.MasterPOSInvoiceId);
                        //// Revenue GL
                        //if (master.PosPaymentMethods.Count() > 0)
                        //{
                        //    AccountCustomerGLDebit.GLDetail_Debit = master.Total_Amount.Value;
                        //}
                        //AccountCustomerGL.GLDetail_Credit = master.Total_Amount.Value;

                        
                        //// Taxes GL
                        
                        //// Inventory GL
                        //inventoryGL.GLDetail_Debit = master.Total_Cost.Value;
                        
                        //fixedDiscount.GLDetail_Credit = master.AllowDiscount.Value;
                        //foreach (var detail in master.DetailPOSInvoices)
                        //{
                        //    var TotalBefore = detail.Product_Price * detail.Product_Quantity;
                        //    revenueGL.GLDetail_Debit += TotalBefore;
                        //}
                        //discount.GLDetail_Credit = revenueGL.GLDetail_Debit * (master.Master_Discount.Value / 100);
                        //// Update store quantity
                        //foreach (var detail in RealDetailList)
                        //{
                        //    ProductBatches detailBatch = db.ProductBatches.Find(detail.Batch_ID);
                        //    if (detailBatch.Product.Prod_Type == Prod_Type.اولي || detailBatch.Product.Prod_Type == Prod_Type.منتج)
                        //    {
                        //        var storeCardOLd = db.StoreCards.Where(a => a.Product_Id == detail.Product_Id && a.Document_Serial == master.Serial_Number).FirstOrDefault();
                        //        detailBatch.Product_Quantity += storeCardOLd.Quantity;
                        //        detailBatch.Batch_Cost += storeCardOLd.CostPrice * storeCardOLd.Quantity;
                        //        if (detailBatch.Product_Quantity == 0)
                        //        {
                        //            detailBatch.Product_Cost = 0;
                        //        }
                        //        else
                        //        {
                        //            detailBatch.Product_Cost = detailBatch.Batch_Cost / detailBatch.Product_Quantity;
                        //        }
                        //        db.Entry(detailBatch).State = EntityState.Modified;


                        //        var storecard = new StoreCard();
                        //        storecard.CostPrice = storeCardOLd.CostPrice;
                        //        storecard.Date = DateTime.Now;
                        //        storecard.Store_Id = master.Store_ID.Value;
                        //        storecard.Quantity = detail.Product_Quantity;
                        //        storecard.SalePrice = detail.Product_Price;
                        //        storecard.Product_Id = detail.Product_Id;
                        //        storecard.IncreaseQuantity = true;
                        //        storecard.TypeOfDocument = "استبعاد فاتورة مبيعات نقطة بيع";
                        //        storecard.Document_Serial = master.Serial_Number;
                        //        storecard.TotalCost = storeCardOLd.CostPrice * storeCardOLd.Quantity;
                        //        storecard.TotalAmount = detail.Total_Amount;
                        //        storecard.Account_Id = master.Customer_Id;
                        //        storecard.Subledger_Id = master.Subledger_Id;
                        //        Store.AddinginstorCard(storecard);
                        //    }
                        //    else
                        //    {

                        //    }
                        //    }
                        
                   
                        //// Total Cost GL
                        //totalCostGL.GLDetail_Credit = master.Total_Cost.Value;


                    }
                    //foreach (var Payment in master.PosPaymentMethods)
                    //{
                    //    var paymentGL = new GLDetail();
                    //    var bankAccount = db.PaymentMethods.Find(Payment.PaymentMethod_Id);
                    //    var ChartOfAccounRecive = db.ChartOfAccountings.Find(bankAccount.Bank_Account_Id);
                    //    paymentGL.Account_Id = ChartOfAccounRecive.Account_Id;
                    //    paymentGL.Account_Name = ChartOfAccounRecive.Account_Name;
                    //    paymentGL.GLDetail_Credit = Payment.Amount - Payment.Commution;
                    //    paymentGL.GLDetail_Debit = 0;
                    //    TotalPaymentAmount += Payment.Amount;
                    //    glMaster.GLDetails.Add(paymentGL);
                    //    if (Payment.Commution != 0)
                    //    {
                    //        var bankcomution = new GLDetail();
                    //        var a = db.PaymentMethods.Find(Payment.PaymentMethod_Id).Expense_Account_Id;
                    //        var bank = db.ChartOfAccountings.Find(a);
                    //        bankcomution.Account_Id = bank.Account_Id;
                    //        bankcomution.Account_Name = bank.Account_Name;
                    //        bankcomution.SubLedger_Id = master.Subledger_Id;
                    //        bankcomution.GLDetail_Credit = Payment.Commution;
                    //        bankcomution.GLDetail_Debit = 0;
                            
                    //        glMaster.GLDetails.Add(bankcomution);
                    //    }
                    //}
                    //if (master.Total_Amount < TotalPaymentAmount)
                    //{
                    //    var differentOfPayment = new GLDetail();
                    //    differentOfPayment.GLDetail_Credit =0 ;
                    //    differentOfPayment.GLDetail_Debit = TotalPaymentAmount - master.Total_Amount.Value;
                    //    differentOfPayment.Account_Id = 73;
                    //    differentOfPayment.Account_Name = db.ChartOfAccountings.Find(73).Account_Name;
                    //    differentOfPayment.SubLedger_Id = master.Subledger_Id;
                    //    glMaster.GLDetails.Add(differentOfPayment);

                    //}
                    //if (master.TaxPOSInvoices.FirstOrDefault() != null)
                    //{
                    //    foreach (var tax in master.TaxPOSInvoices)
                    //    {
                    //        var gldetailTax = new GLDetail();
                    //        var taxs = db.Taxes.Find(Convert.ToInt32(tax.Tax_Id));
                    //        var AccountTax = db.ChartOfAccountings.Where(z => z.Account_Code == taxs.Tax_Code).FirstOrDefault();
                    //        gldetailTax.Account_Id = AccountTax.Account_Id;
                    //        gldetailTax.Account_Name = AccountTax.Account_Name;
                    //        gldetailTax.GLDetail_Credit = 0;
                    //        gldetailTax.GLDetail_Debit = tax.Amount;
                    //        glMaster.GLDetails.Add(gldetailTax);


                    //    }



                    //}
                    break;
                default:
                    break;
            }
            if (msg == "")
            {

                // Add all GL details to the master

                
                glMaster.GLDetails.Add(revenueGL);
                
                glMaster.GLDetails.Add(inventoryGL);
                glMaster.GLDetails.Add(totalCostGL);
                
                if (discount.GLDetail_Credit == 0 && discount.GLDetail_Debit == 0)
                {

                }
                else
                {
                    glMaster.GLDetails.Add(discount);

                }
                if (fixedDiscount.GLDetail_Credit == 0 && fixedDiscount.GLDetail_Debit == 0)
                {

                }
                else
                {
                    glMaster.GLDetails.Add(fixedDiscount);
                }
                if (master.Customer_Id != null)
                {if (AccountCustomerGL.GLDetail_Credit + AccountCustomerGL.GLDetail_Debit > 0)
                    {
                        glMaster.GLDetails.Add(AccountCustomerGL);
                    }
                    if (AccountCustomerGLDebit.GLDetail_Credit + AccountCustomerGLDebit.GLDetail_Debit > 0)
                    {
                        glMaster.GLDetails.Add(AccountCustomerGLDebit);
                    }
                }
                glMaster = addglDetailTax(master,glMaster, master.MasterPOSInvoiceId);
                var credit = glMaster.GLDetails.Sum(a => a.GLDetail_Credit);
                var depit = glMaster.GLDetails.Sum(a => a.GLDetail_Debit);
              
                //if (Math.Round(credit, 5, MidpointRounding.AwayFromZero) != Math.Round(depit, 5, MidpointRounding.AwayFromZero))
                //{
                //    {
                //        if (credit > depit)
                //        {
                //            var q = revenueGL.GLDetail_Debit;
                //            revenueGL.GLDetail_Debit= q + (credit - depit);
                //        }
                //        {
                //            var p = revenueGL.GLDetail_Credit;
                //            revenueGL.GLDetail_Credit = p + (depit - credit);
                //            ;

                //        }
                //    }
                //}
                        db.GLMasters.Add(glMaster);
               
                GLMaster.GetBalanceForAccount(glMaster);
                
                GLMaster.GetBalanceForSubLedger(glMaster);
                msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                db.SaveChanges();
            }
            if (master.Invoice_State == TransactionState.Post)
            {
                msg = Resources.validate.GLDone;
            }
            else
            {
                msg = Resources.validate.GLReverse;
            }

            return msg;




        }
        public static String MakeVoidForSales(int invoiceId)
        {
            var db = new AccountingContainer();
            var invoiceMaster = db.MasterPOSInvoices.Find(invoiceId);

            var Allreverse = db.ReversePOSInvoices.Where(a => a.Master_Id == invoiceId).FirstOrDefault();
            if (Allreverse != null)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.ThierMoreInvoiceForThat;
            }
            else
            {

                var master = new ReversePOSInvoice();


                var lastCodebalance = db.ReversePOSInvoices.OrderByDescending(c => c.Master_Id).Where(e => e.Serial_Number != null).FirstOrDefault();
                if (lastCodebalance == null)
                {
                    master.Serial_Number = "REVPOS-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    master.Serial_Number = "REVPOS-" + (Convert.ToInt32(lastCodebalance.Serial_Number.Substring(7, lastCodebalance.Serial_Number.Length - 7)) + 1).ToString("D3");
                }


                var comp = db.Companies.FirstOrDefault();
                if (comp != null)
                {
                    var date = Convert.ToDateTime(DateTime.Now);
                    if (date < comp.StartFinancialDate)
                    {
                        master.Invoice_Date = DateTime.Now;
                    }
                    else
                    {
                        master.Invoice_Date = DateTime.Now;
                    }
                }
                else
                {
                    master.Invoice_Date = DateTime.Now;
                }




                if (master != null)
                {
                    master.MasterPOSInvoiceMasterPOSInvoiceId = invoiceMaster.MasterPOSInvoiceId;
              
                    master.Master_Discount = invoiceMaster.Master_Discount;
                    master.SubLedger_ID = invoiceMaster.PointOfSale.Subledger_Id;
                   
                    master.Customer_Id = invoiceMaster.Customer_Id;


                    master.Company_Id = invoiceMaster.Company_Id.Value;


                    master.Invoice_Currency = invoiceMaster.Invoice_Currency;


                    master.Store_ID = invoiceMaster.Store_ID.Value;
                    master.POS_ID = invoiceMaster.PointOfSalePOS_Id;
                }



                master.Invoice_State = TransactionState.Post;







                decimal Sumtax = 0;
                decimal SumtAmount = 0;
                decimal SumTotalCost = 0;

                foreach (var item in invoiceMaster.DetailPOSInvoices.ToList())
                {
                    var details = new ReversePOSDetail();
                    details.Product_Id = item.Product_Id;
                    details.Product_Quantity = item.Product_Quantity;

                    details.Product_Price = item.Product_Price;
                    details.Detail_Discount = item.Detail_Discount;
                    details.Detail_Discount_Amount = item.Detail_Discount_Amount;
                    details.Batch_ID = item.Batch_ID.Value;




                    item.Detail_Total_Cost = item.Detail_Total_Cost / item.Product_Quantity;

                    item.Reverse_Quantity = 0;

                    details.Product_Tax = item.Product_Tax;


                    var salesInvoiceDetail = db.DetailPOSInvoices.Where(a => a.Product_Id == item.Product_Id).FirstOrDefault();
                    var productBatch = db.ProductBatches.Where(a => a.Product_ID == salesInvoiceDetail.Product_Id && a.Store_ID == master.Store_ID).FirstOrDefault();


                    //var mySingelCost = item.Detail_Total_Cost / item.Product_Quantity;


                    details.Detail_Total_Cost = (item.Detail_Total_Cost * item.Product_Quantity);


                    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    if (item.Detail_Discount == null)
                    {
                        item.Detail_Discount = 0;
                    }
                    decimal discount = TotalBeforeDiscount * (item.Detail_Discount.Value / 100);
                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                    if (item.Product_Tax == 0)
                    {
                        details.Detail_Total_Tax = 0;
                    }
                    else
                    {
                        var tax = Convert.ToDecimal(db.Taxes.Find(item.Product_Tax).Tax_Percentage);
                        details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                    }



                    var price = details.Product_Price * details.Product_Quantity;
                    details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                    SumtAmount += (decimal)details.Total_Amount;

                    Sumtax += (decimal)details.Detail_Total_Tax;
                    SumTotalCost += (decimal)details.Detail_Total_Cost;
                    master.Total_Tax = Sumtax;
                    master.Total_Amount = SumtAmount;
                    master.Total_Cost = SumTotalCost;


                    master.ReversePOSDetails.Add(details);
                    master.Invoice_State = TransactionState.Pending;


                    db.Entry(item).State = EntityState.Modified;




              


                }

                master.Customer_Id = invoiceMaster.Customer_Id;
                master.Company_Id = invoiceMaster.Company_Id.Value;
                if (invoiceMaster.PosPaymentMethods.FirstOrDefault() != null)
                {
                    master.PaymentMethod_Id = invoiceMaster.PosPaymentMethods.FirstOrDefault().PaymentMethod_Id;
                }
             
                master.Store_ID = invoiceMaster.Store_ID.Value;
                master.Invoice_Currency = invoiceMaster.Invoice_Currency;
                master.Master_Discount = (master.ReversePOSDetails.Select(a => a.Detail_Discount_Amount).Sum() / master.ReversePOSDetails.Select(a => a.Product_Quantity * a.Product_Price).Sum()) * 100;
                master.Number = InvoiceSerial.generateSerialForReverse();
                db.ReversePOSInvoices.Add(master);

                db.SaveChanges();

                DateTime DateNow = DateTime.Now;




                var transBatch = new TransactionBatch();
                transBatch.Date = DateNow;
                transBatch.OperationName = "مردود مبيعات نقطة بيع ";
                transBatch.OperationId = master.Master_Id;
                transBatch.Operation_Code = master.Serial_Number;
                transBatch.Description = "عكس  مبيعات نقطة بيع";
                transBatch.UserId = "noon";
                db.TransactionBatches.Add(transBatch);
                ReversePOSInvoiceVM reverseVM = new ReversePOSInvoiceVM();
                reverseVM.Master_Id = master.Master_Id;
                reverseVM.Invoice_State = TransactionState.Post;
                MakeSalesinQrSales(master);
                var msg = ReversePOSInvoice.UpdateInvoiceMaster(reverseVM);
                db.SaveChanges();
                db.Dispose();

                return msg;




            }



        }
        public static List<DetailPOSInvoice> getMaterialOCompoundProduct(List<DetailPOSInvoice> Details, int Store_Id)
        {
            var db = new AccountingContainer();
            var ProductAll = db.Products.ToList();
            var project = ProductAll.Where(a => a.Prod_Type == Prod_Type.مجمع).Select(a => a.Prod_ID).ToList();
            var ProList = Details.Where(s => project.Contains(s.Product_Id)).ToList();
            if (ProList.Count() == 0)
            {

            }
            else
            {
                List<DetailPOSInvoice> Details2 = new List<DetailPOSInvoice>();

                foreach (var item in ProList)
                {
                    var SubProductList = ProductAll.Where(a => a.Prod_ID == item.Product_Id).FirstOrDefault().ProductCompounds;
                    foreach (var subProduct in SubProductList)
                    {
                        var dublicate = Details.Where(a => a.Product_Id == subProduct.Product_Id).FirstOrDefault();
                        var batch = db.ProductBatches.Where(a => a.Product_ID == subProduct.Product_Id && a.Store_ID == Store_Id).FirstOrDefault();
                        if (dublicate == null)
                        {
                            var Detailnew = new DetailPOSInvoice();
                            Detailnew.Product_Id = subProduct.Product_Id;
                            Detailnew.Product_Quantity = subProduct.Quantity * item.Product_Quantity;
                            Detailnew.Reverse_Quantity = subProduct.Quantity * item.Product_Quantity;
                            if (batch != null)
                            {
                                Detailnew.Batch_ID = batch.Batch_ID;
                            }
                            if (batch != null && batch.Product_Quantity > 0)
                            {
                                Detailnew.Detail_Total_Cost = (batch.Batch_Cost / batch.Product_Quantity ) * Detailnew.Product_Quantity;
                                Detailnew.ProductPriceCost = (batch.Batch_Cost / batch.Product_Quantity );
                            }
                            else
                            {
                                Detailnew.Detail_Total_Cost = 0;
                                Detailnew.ProductPriceCost = 0;

                            }
                            Details.Add(Detailnew);

                        }
                        else
                        {


                            var Detailnew = new DetailPOSInvoice();
                            Detailnew.Product_Id = subProduct.Product_Id;
                            Detailnew.Product_Quantity = dublicate.Product_Quantity + (item.Product_Quantity * subProduct.Quantity);
                            Detailnew.Reverse_Quantity = Detailnew.Product_Quantity;
                            if (batch != null)
                            {
                                Detailnew.Batch_ID = batch.Batch_ID;
                            }
                            if (batch != null && batch.Product_Quantity > 0)
                            {
                                Detailnew.Detail_Total_Cost = (batch.Batch_Cost / batch.Product_Quantity ) * Detailnew.Product_Quantity;
                                Detailnew.ProductPriceCost = (batch.Batch_Cost / batch.Product_Quantity );
                            }
                            else
                            {
                                Detailnew.Detail_Total_Cost = 0;
                                Detailnew.ProductPriceCost = 0;

                            }
                            Details.Remove(dublicate);
                            Details.Add(Detailnew);

                        }
                        var Delete = Details.Where(a => a.Product_Id == subProduct.Master_Product_Id).FirstOrDefault();

                        Details.Remove(Delete);

                    }





                }

            }

            return Details;
        }
        public static GLMaster addglDetailTax(MasterPOSInvoice master,GLMaster gLMaster, int invoiceId)
        {
            var db = new AccountingContainer();
            var invoiceDetail =master.DetailPOSInvoices;
            var numberofdifferentTax = invoiceDetail.Where(a => a.Product_Tax != 0).Select(a => a.Product_Tax).Distinct();
            if (numberofdifferentTax.Count() == 0)
            {
                return gLMaster;
            }
            foreach (var tax in numberofdifferentTax)
            {
                var gldetail = new GLDetail();
                var thistax = db.Taxes.Find(tax).Tax_Code;
                var taxChart = db.ChartOfAccountings.Where(a => a.Account_Code == thistax).FirstOrDefault();

                gldetail.Account_Id = taxChart.Account_Id;
                gldetail.Account_Name = taxChart.Account_Name;
                if (gLMaster.GLState == TransactionState.Post)
                {
                    gldetail.GLDetail_Credit = invoiceDetail.Where(a => a.Product_Tax == tax).Select(a => a.Detail_Total_Tax).Sum();
                    gldetail.GLDetail_Debit = 0;
                }
                else
                {
                    gldetail.GLDetail_Credit = 0;
                    gldetail.GLDetail_Debit = invoiceDetail.Where(a => a.Product_Tax == tax).Select(a => a.Detail_Total_Tax).Sum();
                }

                gLMaster.GLDetails.Add(gldetail);
            }

            return gLMaster;
        }
        public static decimal changeSpecificValueinPOs(int detail_Id, decimal quantity, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.DetailPOSInvoiceTemps.Find(detail_Id);
            var master = detail.MasterPOSInvoiceTemp;
            var batch = db.Products.Where(pro => pro.Prod_ID == detail.Product_Id).FirstOrDefault();

            //if (batch.Product_Quantity < quantity && batch.Product.Prod_Type == Prod_Type.منتج)
            //{
            //    return "الكمية اكبر من الموجود في المخزن";

            //}
            detail.Product_Quantity = quantity;
            //detail.Detail_Total_Cost = quantity * batch.Product_Cost;

            decimal totalprice = detail.Product_Price * detail.Product_Quantity;
            decimal discound = totalprice * (detail.Detail_Discount.Value / 100);
            decimal afterdec = totalprice - discound;
            detail.TotalBeforeDisc = totalprice;
            detail.TotalDiscount = discound;
            detail.Detail_Discount_Amount = detail.TotalDiscount;


            if (detail.Product_Tax == 0)
            {
                detail.Detail_Total_Tax = 0;
            }
            else
            {
                var prodtax = db.Taxes.Find(detail.Product_Tax);
                detail.Detail_Total_Tax = afterdec * (prodtax.Tax_Percentage / 100);
            }
            detail.Detail_Total_Cost = batch.Prod_Cost_Price.Value * quantity;
            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;
            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.SaveChanges();
            var ee = master.DetailPOSInvoiceTemps.Select(a => a.Product_Quantity * a.Product_Price * (a.Detail_Discount.Value / 100)).Sum();
            var bb = master.DetailPOSInvoiceTemps.Select(a => a.Product_Quantity * a.Product_Price).Sum();
            decimal masdiscount = (ee / bb) * 100;
            master.Master_Discount = masdiscount;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return masdiscount;

        }
        public static string changeSpecificTax(int detail_Id, int product_Tax, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.DetailPOSInvoiceTemps.Find(detail_Id);
            var master = detail.MasterPOSInvoiceTemp;




            decimal totalprice = detail.Product_Price * detail.Product_Quantity;
            decimal discound = 0;
            if (detail.Detail_Discount != null)
            {
                discound = totalprice * (detail.Detail_Discount.Value / 100);
            }
            else
            {
                discound = totalprice * (discount / 100);
            }
            decimal afterdec = totalprice - discound;
            detail.TotalBeforeDisc = totalprice;
            detail.TotalDiscount = discound;
            if (product_Tax == 0)
            {
                detail.Detail_Total_Tax = 0;
                detail.Product_Tax = 0;
            }
            else
            {
                var prodtax = db.Taxes.Find(product_Tax);
                detail.Product_Tax = product_Tax;
                detail.Detail_Total_Tax = afterdec * (prodtax.Tax_Percentage / 100);

            }

            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;
            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return "";

        }
        public static decimal changeSpecificDiscountAmount(int detail_Id, decimal discountdetailAmount)
        {
            var db = new AccountingContainer();
            var detail = db.DetailPOSInvoiceTemps.Find(detail_Id);
            var master = detail.MasterPOSInvoiceTemp;

            detail.Detail_Discount_Amount = discountdetailAmount;

            decimal totalprice = detail.Product_Price * detail.Product_Quantity;
            detail.Detail_Discount = (discountdetailAmount / totalprice) * 100;
            decimal discound = totalprice * (detail.Detail_Discount.Value / 100);
            decimal afterdec = totalprice - discound;
            detail.TotalBeforeDisc = totalprice;
            detail.TotalDiscount = discound;
            if (detail.Product_Tax == 0)
            {
                detail.Detail_Total_Tax = 0;
            }
            else
            {
                var prodtax = db.Taxes.Find(detail.Product_Tax);
                detail.Detail_Total_Tax = afterdec * (prodtax.Tax_Percentage / 100);
            }

            detail.Detail_Discount_Amount = detail.TotalDiscount;
            detail.Detail_Discount_Amount = (detail.Product_Price * detail.Product_Quantity) * (detail.Detail_Discount / 100);
            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;

            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            var ee = master.DetailPOSInvoiceTemps.Select(a => a.Product_Price * a.Product_Quantity * (a.Detail_Discount.Value / 100)).Sum();
            var bb = master.DetailPOSInvoiceTemps.Select(a => a.Product_Price * a.Product_Quantity).Sum();
            decimal masdiscount = (ee / bb) * 100;
            return masdiscount;

        }
        public static decimal changeSpecificTotalAmount(int detail_Id, decimal totalAmount)
        {

            var db = new AccountingContainer();
            var detail = db.DetailPOSInvoiceTemps.Find(detail_Id);
            var master = detail.MasterPOSInvoiceTemp;
            decimal tax = 0;
            if (detail.Product_Tax > 0)
            {
                tax = db.Taxes.Find(detail.Product_Tax).Tax_Percentage / 100;
            }

            var price = (totalAmount) / ((1 + tax) * (1 - (detail.Detail_Discount.Value / 100)) * detail.Product_Quantity);





            decimal totalprice = price * detail.Product_Quantity;
            decimal discound = totalprice * (detail.Detail_Discount.Value / 100);
            decimal afterdec = totalprice - discound;
            detail.Product_Price = price;
            detail.TotalBeforeDisc = totalprice;
            detail.TotalDiscount = discound;
            if (detail.Product_Tax == 0)
            {
                detail.Detail_Total_Tax = 0;
            }
            else
            {
                var prodtax = db.Taxes.Find(detail.Product_Tax);
                detail.Detail_Total_Tax = afterdec * (prodtax.Tax_Percentage / 100);
            }

            detail.Detail_Discount_Amount = detail.TotalDiscount;
            detail.Detail_Discount_Amount = (detail.Product_Price * detail.Product_Quantity) * (detail.Detail_Discount / 100);
            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;

            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            var ee = master.DetailPOSInvoiceTemps.Select(a => a.Product_Price * a.Product_Quantity * (a.Detail_Discount.Value / 100)).Sum();
            var bb = master.DetailPOSInvoiceTemps.Select(a => a.Product_Price * a.Product_Quantity).Sum();
            decimal masdiscount = (ee / bb) * 100;
            return price;

        }
        public static void changePaymentMethod(int masterId,int PaymentMethod)
        {

            var db = new AccountingContainer();
            var invoice = db.MasterPOSInvoiceTemps.Find(masterId);
            invoice.Exchange_Rate = PaymentMethod;
            db.Entry(invoice).State = EntityState.Modified;
            db.SaveChanges();
        }
        public static bool checkthatEmployeisSalesPerson(string UserId)
        {
            var db = new AccountingContainer();
            var Emp = db.Employees.Where(a => a.Reg_ID == UserId).FirstOrDefault();
            if (Emp != null)
            {
                var POs = db.PointOfSales.Where(a => a.Employee_Id == Emp.Emp_Id).FirstOrDefault();
                if (POs != null)
                {

                    return true;

                }
                return false;

            }

            return false;


        }
        public static Employee getTheEmployeeByRegisterId( string UserId)
        {
            var db = new AccountingContainer();

            var Emp = db.Employees.Where(a => a.Reg_ID == UserId).FirstOrDefault();
            return Emp;

        }
        public static decimal changeSpecificPrice(int detail_Id, decimal Price, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.DetailPOSInvoiceTemps.Find(detail_Id);
            var master = detail.MasterPOSInvoiceTemp;

            detail.Product_Price = Price;

            decimal totalprice = Price * detail.Product_Quantity;
            decimal discound = totalprice * (discount / 100);
            decimal afterdec = totalprice - discound;
            detail.TotalBeforeDisc = totalprice;
            detail.TotalDiscount = discound;
            detail.Detail_Discount_Amount = detail.TotalDiscount;
            if (detail.Product_Tax == 0)
            {
                detail.Detail_Total_Tax = 0;
            }
            else
            {
                var prodtax = db.Taxes.Find(detail.Product_Tax);
                detail.Detail_Total_Tax = afterdec * (prodtax.Tax_Percentage / 100);
            }
            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;
            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            var ee = master.DetailPOSInvoiceTemps.Select(a => a.Product_Price * a.Product_Quantity * (a.Detail_Discount.Value / 100)).Sum();
            var bb = master.DetailPOSInvoiceTemps.Select(a => a.Product_Price * a.Product_Quantity).Sum();
            decimal masdiscount = (ee / bb) * 100;
            master.Master_Discount = masdiscount;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return masdiscount;

        }
        public static IEnumerable<SelectListItem> GetAllCustomerForEmp(string userId)
        {
            var db = new AccountingContainer();
            var emp = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
            if (emp == null || emp.ViewAllCustomer == true)
            {
                var newList = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                               select new
                               {
                                   Id = cust.Customer_Id,
                                   Name = cust.Customer_Name + "        " + cust.Customer_Phone

                               }).ToList();
                return new SelectList(newList, "Id", "Name");
            }
            else
            {
                var newList = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط && (a.Employee_Id == emp.Emp_Id || a.Employee_Id == null))
                               select new
                               {
                                   Id = cust.Customer_Id,
                                   Name = cust.Customer_Name + "        " + cust.Customer_Phone

                               }).ToList();
                return new SelectList(newList, "Id", "Name");
            }


        }
        public static List<Customer> GetAllCustomerForEmpList(string userId)
        {
            var db = new AccountingContainer();
            var emp = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
            if (emp == null || emp.ViewAllCustomer == true)
            {
                var newList = db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                               .ToList();
                return newList;
            }
            else
            {
                var newList = db.Customers.Where(a => a.Customer_State == Customer_State.نشط && (a.Employee_Id == emp.Emp_Id || a.Employee_Id == null))
                               .ToList();
                return newList;
            }


        }
        public static decimal updateDiscountDebendOnNet(decimal net, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.MasterPOSInvoiceTemps.Find(Invoice_Id);


            if (invoiceTemp.DetailPOSInvoiceTemps.Count() == 0 || invoiceTemp.DetailPOSInvoiceTemps.Select(a => a.Product_Tax).Distinct().Count() > 1)
            {
                try
                {
                    return invoiceTemp.Master_Discount.Value;
                }
                catch
                {
                    return 0;
                }
            }
            var detail = invoiceTemp.DetailPOSInvoiceTemps.ToList();
            var taxId = invoiceTemp.DetailPOSInvoiceTemps.FirstOrDefault().Product_Tax;
            decimal invoiceTax = 0;
            if (taxId > 0)
            {
                invoiceTax = db.Taxes.Find(taxId).Tax_Percentage / 100;
            }
            decimal totalPrice = invoiceTemp.DetailPOSInvoiceTemps.Select(a => a.Product_Quantity * a.Product_Price).Sum();
            decimal discountTotal = (totalPrice + (invoiceTax * totalPrice) - net) / (1 + invoiceTax);
            decimal percentage = (discountTotal / totalPrice) * 100;
            decimal total = 0;
            foreach (var item in detail)
            {
                item.TotalBeforeDisc = item.Product_Quantity * item.Product_Price;
                item.TotalDiscount = item.TotalBeforeDisc * (percentage / 100);
                item.Detail_Discount = percentage;
                item.Detail_Discount_Amount = item.TotalDiscount;
                decimal? beforeTax = 0;
                if (item.Product_Tax == 0)
                {
                    item.Detail_Total_Tax = 0;
                }
                else
                {
                    var tax = db.Taxes.Find(item.Product_Tax).Tax_Percentage;
                    beforeTax = item.TotalBeforeDisc - item.TotalDiscount;
                    item.Detail_Total_Tax = beforeTax.Value * (tax / 100);
                }


                item.Total_Amount = (item.TotalBeforeDisc.Value - item.TotalDiscount.Value) + item.Detail_Total_Tax;
                db.Entry(item).State = EntityState.Modified;
                total += item.Total_Amount;
            }
            invoiceTemp.Total_Amount = total;
            invoiceTemp.Master_Discount = percentage;
            db.Entry(invoiceTemp).State = EntityState.Modified;
            db.SaveChanges();
            return percentage;

        }
        public static bool MakeSalesinQrSales(ReversePOSInvoice revmaster)
        {
            var db = new AccountingContainer();


            var qrmaster = new ReverseQRInvoice();
            var master = db.MasterPOSInvoices.Find(revmaster.MasterPOSInvoiceMasterPOSInvoiceId);
            if (master.Account_Id > 0)
            {
                qrmaster.Account_Id = master.Account_Id.Value;
            }
            else { master.Account_Id = 76; }
            qrmaster.Number = revmaster.Number;
            qrmaster.DateOfCreate = DateTime.Now;
            qrmaster.ApprovalCode = revmaster.ApprovalCode;
            qrmaster.Company_Id = revmaster.Company_Id;
            qrmaster.Customer_Id = revmaster.Customer_Id;
            qrmaster.Description = revmaster.Description;
            qrmaster.Exchange_Rate = 0;
            qrmaster.HasOff = revmaster.HasOff;
            qrmaster.Invoice_Currency = revmaster.Invoice_Currency;
            qrmaster.Invoice_Date = revmaster.Invoice_Date;
            qrmaster.Invoice_State = revmaster.Invoice_State;
            qrmaster.MasterQRInvoiceId = master.QRNumber.Value;
            qrmaster.Master_Discount = revmaster.Master_Discount;
            qrmaster.PaymentMethod_Id = revmaster.PaymentMethod_Id;
            qrmaster.Reference = revmaster.Reference;
            qrmaster.Serial_Number = revmaster.Serial_Number;
            qrmaster.Store_ID = revmaster.Store_ID;
            qrmaster.SubLedger_ID = master.PointOfSale.Subledger_Id;
            qrmaster.Total_Amount = revmaster.Total_Amount;
            qrmaster.Total_Cost = revmaster.Total_Cost;
            qrmaster.Total_Payment_Method_Cost = 0;
            qrmaster.Total_Tax = revmaster.Total_Tax;
            var qrmmaster = db.MasterQRInvoices.Find(qrmaster.MasterQRInvoiceId);
            qrmaster.ReversePOSDetails = new List<ReverseQRDetail>();
            foreach (var item in revmaster.ReversePOSDetails)
            {
                var qrdetail = new ReverseQRDetail();
                if (item.Batch_ID != null)
                {
                    qrdetail.Batch_ID = item.Batch_ID;
                }
                qrdetail.DetailQRInvoiceDetail_Id = qrmmaster.DetailQRInvoices.Where(a => a.Product_Id == item.Product_Id).FirstOrDefault().Detail_Id;
                qrdetail.Detail_Discount = item.Detail_Discount;
                qrdetail.Detail_Discount_Amount = item.Detail_Discount_Amount;
                qrdetail.Detail_Total_Cost = item.Detail_Total_Cost;
                qrdetail.Detail_Total_Tax = item.Detail_Total_Tax;
                qrdetail.Product_Id = item.Product_Id;
                qrdetail.Product_Price = item.Product_Price;
                qrdetail.Product_Quantity = item.Product_Quantity;
                qrdetail.Product_Tax = Convert.ToInt32(item.Product_Tax);
                qrdetail.Total_Amount = item.Total_Amount;

                qrmaster.ReversePOSDetails.Add(qrdetail);

            }
            qrmaster.Invoice_State = TransactionState.Post;
            db.ReverseQRInvoices.Add(qrmaster);
            db.SaveChanges();

            return true;
        }
        //public static object GetAllCustomerForEmpList(string userId)
        //{
        //    var db = new AccountingContainer();
        //    var emp = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
        //    if (emp == null || emp.ViewAllCustomer == true)
        //    {
        //        var newList = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
        //                       select new
        //                       {
        //                           Id = cust.Customer_Id,
        //                           Name = cust.Customer_Name + "        " + cust.Customer_Phone

        //                       }).ToList();
        //        return newList;
        //    }
        //    else
        //    {
        //        var newList = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط && (a.Employee_Id == emp.Emp_Id || a.Employee_Id == null))
        //                       select new
        //                       {
        //                           Id = cust.Customer_Id,
        //                           Name = cust.Customer_Name + "        " + cust.Customer_Phone

        //                       }).ToList();
        //        return newList;
        //    }


        //}

    }

}