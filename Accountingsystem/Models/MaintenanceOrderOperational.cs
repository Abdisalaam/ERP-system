using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class MaintenanceOrderOperational
    {
        public static MaintenanceOrderOperational GetOrderById(int id)
        {
            var db = new AccountingContainer();
            var order = db.MaintenanceOrderOperationals.Find(id);
            return order;

        }
        public static List <MaintenanceOrderOperational> GetAllOrder()
        {
            var db = new AccountingContainer();
            var orders = db.MaintenanceOrderOperationals.ToList();
            return orders;

        }
        public static MaintenanceOrderOperationalVM PrepareForCreate()
        {

            var db = new AccountingContainer();
            MaintenanceOrderOperationalVM vm = new MaintenanceOrderOperationalVM();
            var lastSerial = db.MaintenanceOrderCodings.OrderByDescending(c => c.Id).FirstOrDefault();
            if (lastSerial == null)
            {
                vm.Order_Code = "1001";
            }
            else
            {
               
                try
                {
                    vm.Order_Code = Convert.ToString((Convert.ToInt32(lastSerial.Code) + 1));
                }
                catch
                {
                    vm.Order_Code = "1001";
                }
                //using string substring method to get the number of the last inserted
                //saleRecietMaster.Order_Code = "BAR-" + (Convert.ToInt32(lastSerial.Code.Substring(6, lastSerial.Code.Length - 6)) + 1).ToString("D3");
            }

            var newList = (from custo in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                           select new
                           {
                               Id = custo.Customer_Id,
                               Name = custo.Customer_Name + "        " + custo.Customer_Phone

                           }).ToList();
            vm.CustomerList = new SelectList(newList, "Id", "Name");
            vm.EmployeeList = new SelectList(db.Employees.ToList(), "Emp_Id", "Emp_Name");

            var masterOrdertemp = new MaintenanceOrderOperationalTemp();
            var Id = (from m in db.MaintenanceOrderOperationalTemps
                      select m).ToList().LastOrDefault();
            if (Id == null)
            { masterOrdertemp.Id = 1; }
            else
            { masterOrdertemp.Id = Id.Id + 1; }

            masterOrdertemp.AllowDiscount = 0;
         
            masterOrdertemp.Total_Amount = 0;
            db.MaintenanceOrderOperationalTemps.Add(masterOrdertemp);
            db.SaveChanges();
            vm.Id = masterOrdertemp.Id;
            vm.Master_Discount = 0;
            vm.AllowDiscount = 0;
            return vm;
        }

        public static MaintenanceOrderOperationalVM PrepareForEdit(int id,int stat)
        {
            var db = new AccountingContainer();
            MaintenanceOrderOperationalVM vm = new MaintenanceOrderOperationalVM();


            
            var Maintenance = db.MaintenanceOrderOperationals.Find(id);
            vm.AllowDiscount = Maintenance.AllowDiscount;
            vm.Checked = Maintenance.Checked;
            vm.Customer_Id = Maintenance.Customer_Id;
            vm.Date = Maintenance.Date;
            vm.Describtion = Maintenance.Describtion;
            vm.Emplyee_id = Maintenance.Emplyee_id;
            vm.EndGuaranteeDate = Maintenance.EndGuaranteeDate;
            vm.guaranteeDuration = Maintenance.guaranteeDuration;
            vm.Master_Discount = Maintenance.Master_Discount;
            vm.Model = Maintenance.Model;
            vm.Order_Code = Maintenance.Order_Code;
            vm.RequiredDescribtion = Maintenance.RequiredDescribtion;
            vm.SerialNumber = Maintenance.SerialNumber;
            vm.Status = stat;
            vm.Total_Amount = Maintenance.Total_Amount;
            vm.Total_Cost = Maintenance.Total_Cost;
            vm.Total_Tax = Maintenance.Total_Tax;
            vm.ExpectedAmount = Maintenance.ExpectedAmount;


            var newList = (from custo in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                           select new
                           {
                               Id = custo.Customer_Id,
                               Name = custo.Customer_Name + "        " + custo.Customer_Phone

                           }).ToList();
            vm.CustomerList = new SelectList(newList, "Id", "Name");
            vm.EmployeeList = new SelectList(db.Employees.ToList(), "Emp_Id", "Emp_Name");

            var ThereORN = db.MaintenanceOrderOperationalTemps.Find(id);
            if (ThereORN != null)
            {

            }
            else
            {
                var Temp = new MaintenanceOrderOperationalTemp();
                Temp.Id = Maintenance.Id;
                Temp.AllowDiscount = Maintenance.AllowDiscount.Value;

                db.MaintenanceOrderOperationalTemps.Add(Temp);
                db.SaveChanges();
            }
            vm.Id = id;
            return vm;
        }

        public static MaintenanceOrderOperationalDetailTempVM GetAllDetailForEditing(int MasterId)
        {
            var db = new AccountingContainer();
            var invoicedetalTemp = new MaintenanceOrderOperationalDetailTempVM();


            invoicedetalTemp.invoice_Id = MasterId;



            var detailTemp = db.MaintenanceOrderOperationalTemps.Find(MasterId);

            db.MaintenanceOrderOperationalDetailTemps.RemoveRange(detailTemp.MaintenanceOrderOperationalDetails);
            db.SaveChanges();
            var invoice = db.MaintenanceOrderOperationals.Find(MasterId);
            if (invoice.AllowDiscount == null)
            {
                invoice.AllowDiscount = 0;
            }
            invoicedetalTemp.AllowDiscount = invoice.AllowDiscount.Value;
            
          
            List<MaintenanceOrderOperationalDetailTemp> Lista = new List<MaintenanceOrderOperationalDetailTemp> { };
            foreach (var item in invoice.MaintenanceOrderOperationalDetails)
            {
                var invoiceTemp = new MaintenanceOrderOperationalDetailTemp();
                var pro = db.Products.Find(item.Product_Id);
                invoiceTemp.Batch_ID = item.Batch_ID.Value;
                invoiceTemp.Detail_Total_Tax = item.Detail_Total_Tax;
                invoiceTemp.Product_Id = item.Product_Id;
                invoiceTemp.Product_Price = item.Product_Price;
            
                invoiceTemp.Product_Quantity = item.Product_Quantity;
                invoiceTemp.Detail_Total_Cost = pro.Prod_Cost_Price.Value * item.Product_Quantity;
                invoiceTemp.Product_Tax = item.Product_Tax;
                invoiceTemp.Detail_Total_Tax = item.Detail_Total_Tax;
                invoiceTemp.Total_Amount = item.Total_Amount;
                invoiceTemp.MaintenanceOrderOperationalId = MasterId;
                Lista.Add(invoiceTemp);

            }
            db.MaintenanceOrderOperationalDetailTemps.AddRange(Lista);

            db.SaveChanges();
            invoicedetalTemp.invoiceTemp = detailTemp.MaintenanceOrderOperationalDetails.ToList();
            return invoicedetalTemp;
        }
        public static string editinvoice(MaintenanceOrderOperationalVM masterVM,string user_Id)
        {
            var db = new AccountingContainer();
            var invoice = db.MaintenanceOrderOperationals.Find(masterVM.Id);
            var mastertemp = db.MaintenanceOrderOperationalTemps.Find(masterVM.Id);
            invoice.AllowDiscount = masterVM.AllowDiscount;
            invoice.Checked = masterVM.Checked;
            invoice.Customer_Id = masterVM.Customer_Id;
            invoice.Date = DateTime.Now;
            invoice.Describtion = masterVM.Describtion;
            invoice.Emplyee_id = db.Employees.Where(a => a.Reg_ID == user_Id).FirstOrDefault().Emp_Id;

            invoice.guaranteeDuration = masterVM.guaranteeDuration;
            invoice.Master_Discount = masterVM.AllowDiscount;
            invoice.Model = masterVM.Model;
            invoice.RequiredDescribtion = masterVM.RequiredDescribtion;
            invoice.SerialNumber = masterVM.SerialNumber;
            invoice.ExpectedAmount = masterVM.ExpectedAmount;
            if (masterVM.Status != 0)
            {
                invoice.Status = masterVM.Status;
            }
            invoice.Model = masterVM.Model;
            invoice.Master_Discount = masterVM.Master_Discount;








            List<MaintenanceOrderOperationalDetailTemp> InvoicedtailListRemove = new List<MaintenanceOrderOperationalDetailTemp>();
            var oldDetail = db.MaintenanceOrderOperationalDetails.Where(a => a.MaintenanceOrderOperationalId == invoice.Id).ToList();





            var detailListTemp = db.MaintenanceOrderOperationalDetailTemps.Where(e => e.MaintenanceOrderOperationalId == mastertemp.Id).ToList();
            invoice.Total_Amount = 0;
            invoice.Total_Cost = 0;
            invoice.Total_Tax = 0;
            invoice.MaintenanceOrderOperationalDetails = new List<MaintenanceOrderOperationalDetail> { };

            foreach (var item in detailListTemp)
            {

                // Create new detail and map the values from detailVM to it
                // TODO: Use AutoMapper
                var details = new MaintenanceOrderOperationalDetail();
                details.Id = item.Id;
                details.Product_Quantity = item.Product_Quantity;
                details.Product_Price = item.Product_Price;

                decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                decimal discount = TotalBeforeDiscount * (invoice.Master_Discount.Value / 100);
                decimal TotalAfterDiscount = TotalBeforeDiscount - discount;

                details.Product_Tax = Convert.ToInt32(item.Product_Tax);

                details.Batch_ID = item.Batch_ID;

                if (details.Product_Tax == 0)
                {
                    details.Detail_Total_Tax = 0;
                }
                else
                {
                    var tax = Convert.ToDecimal(db.Taxes.Find(details.Product_Tax).Tax_Percentage);
                    details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                }

                // TEMP: Fix relations

                Product productss = db.Products.Find(item.Product_Id);
                details.Product_Id = productss.Prod_ID;
                // Calculate the Total_Cost

                details.Detail_Total_Cost = item.Detail_Total_Cost;
                details.Batch_ID = item.Batch_ID;
                var price = item.Product_Quantity * item.Product_Price;
                details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                invoice.Total_Amount += details.Total_Amount;
                invoice.Total_Cost += details.Detail_Total_Cost;
                invoice.Total_Tax += details.Detail_Total_Tax;

                invoice.MaintenanceOrderOperationalDetails.Add(details);
                db.MaintenanceOrderOperationalDetailTemps.Remove(item);



            }


            //The total amount after matching is initialized to the total amount
            invoice.Total_Amount = invoice.Total_Amount - invoice.AllowDiscount;
            if (masterVM.Status != 0)
            {
                var MainStatus = new MaintenanceOrderOperationalStatus();
               MainStatus.DateOfState = DateTime.Now;
               MainStatus.EmployeeId = invoice.Emplyee_id;
          
                MainStatus.Status = masterVM.Status;
          
            invoice.MaintenanceOrderOperationalStatus.Add(MainStatus);
            }
            db.MaintenanceOrderOperationalDetailTemps.RemoveRange(InvoicedtailListRemove);
            if (invoice.Model == null)
            {
                invoice.Model = "";
            }
            if (invoice.SerialNumber == null)
            {
                invoice.SerialNumber = "";
            }

            db.Entry(invoice).State = EntityState.Modified;
            db.MaintenanceOrderOperationalTemps.Remove(mastertemp);
            db.MaintenanceOrderOperationalDetails.RemoveRange(oldDetail);


            db.SaveChanges();

            return "success";
        }

        public static string DeliverToCustomer(int id , string user_Id,int status)
        {
            var db = new AccountingContainer();
            var invoice = db.MaintenanceOrderOperationals.Find(id);
            invoice.Status = status;
            var Emp = db.Employees.Where(a => a.Reg_ID == user_Id).FirstOrDefault();
            var MainStatus = new MaintenanceOrderOperationalStatus();
            MainStatus.DateOfState = DateTime.Now;
            MainStatus.EmployeeId = Emp.Emp_Id;

            MainStatus.Status = status;

            invoice.MaintenanceOrderOperationalStatus.Add(MainStatus);
            db.Entry(invoice).State = EntityState.Modified;
            db.SaveChanges();
            return Resources.MaintenanceOrder.DeliverDeviceDone;
        }

        public static Tuple<string, int> addnewSaleReciept(MaintenanceOrderOperationalVM saleMastVM, String UserId)
        {

            var db = new AccountingContainer();
            var saleRecietMaster = new MaintenanceOrderOperational();
            var saleRecietMasterTemp = db.MaintenanceOrderOperationalTemps.Find(saleMastVM.Id);
            if (saleMastVM != null)
            {
                var listOfSale = db.MaintenanceOrderCodings.OrderByDescending(c => c.Id).FirstOrDefault();
                //using string substring method to get the number of the last inserted

                //var listOfSale = db.SalesReceiptMasters.ToList();
                if (listOfSale == null)
                {
                    saleRecietMaster.Order_Code = "1001";
                }
                else
                {
                    var lastSerial = db.MaintenanceOrderCodings.OrderByDescending(c => c.Id).FirstOrDefault();
                    try
                    {
                        saleRecietMaster.Order_Code = Convert.ToString((Convert.ToInt32(lastSerial.Code) + 1));
                    }
                    catch
                    {
                        saleRecietMaster.Order_Code = "1001";
                    }
                    //using string substring method to get the number of the last inserted
                    //saleRecietMaster.Order_Code = "BAR-" + (Convert.ToInt32(lastSerial.Code.Substring(6, lastSerial.Code.Length - 6)) + 1).ToString("D3");
                }

                saleRecietMaster.Describtion = saleMastVM.Describtion;
                saleRecietMaster.Customer_Id = saleMastVM.Customer_Id;
                try
                {
                    saleRecietMaster.Emplyee_id = db.Employees.Where(a => a.Reg_ID == UserId).FirstOrDefault().Emp_Id;
                }
                catch
                {
                    saleRecietMaster.Emplyee_id = db.Employees.FirstOrDefault().Emp_Id;
                }
                saleRecietMaster.Date = DateTime.Now;
                if (saleMastVM.guaranteeDuration == null)
                {
                    saleMastVM.guaranteeDuration = 0;
                }
                saleRecietMaster.EndGuaranteeDate = saleRecietMaster.Date.AddDays(saleMastVM.guaranteeDuration.Value);
                saleRecietMaster.Model = saleMastVM.Model;

                saleRecietMaster.RequiredDescribtion = saleMastVM.RequiredDescribtion;
                saleRecietMaster.SerialNumber = saleMastVM.SerialNumber;
                saleRecietMaster.ExpectedAmount = saleMastVM.ExpectedAmount;
                if (saleRecietMaster.SerialNumber == null)
                {
                    saleRecietMaster.SerialNumber = "";
                }
                if (saleRecietMaster.Model == null)
                {
                    saleRecietMaster.Model = "";
                }
                saleRecietMaster.Status =1;

                saleRecietMaster.Total_Amount = 0;
                saleRecietMaster.Total_Tax = 0;
                saleRecietMaster.Total_Cost = 0;

                foreach (var item in saleRecietMasterTemp.MaintenanceOrderOperationalDetails.ToList())
                {
                    var details = new MaintenanceOrderOperationalDetail();
                    details.Id = item.Id;
                    details.Product_Quantity = item.Product_Quantity;
                    details.Product_Price = item.Product_Price;
                    details.Product_Tax = item.Product_Tax;
                    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    decimal discount = TotalBeforeDiscount * (saleMastVM.Master_Discount.Value / 100);
                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                    details.Batch_ID = item.Batch_ID;
                    if (details.Product_Tax == 0)
                    {
                        details.Detail_Total_Tax = 0;
                    }
                    else
                    {
                        var tax = Convert.ToDecimal(db.Taxes.Find(details.Product_Tax).Tax_Percentage);
                        details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                    }
                    // if the product in batch
                    if (item.Batch_ID > 0)
                    {
                        ProductBatches detailBatch = db.ProductBatches.Find(details.Batch_ID);
                        details.Product_Id = detailBatch.Product_ID;
                        // Checks for the products quantity if available or not
                        //if (item.Product_Quantity > detailBatch.Product_Quantity)
                        //    return "Not enough quantity of " + detailBatch.Product.Prod_Name;
                        // Calculate the Total_Cost

                        details.Detail_Total_Cost = item.Product_Quantity * detailBatch.Product_Cost;




                        var price = item.Product_Price * item.Product_Quantity;
                        details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                        saleRecietMaster.Total_Amount += details.Total_Amount;
                        saleRecietMaster.Total_Cost += details.Detail_Total_Cost;
                        saleRecietMaster.Total_Tax += details.Detail_Total_Tax;
                       
                        saleRecietMaster.MaintenanceOrderOperationalDetails.Add(details);
                        db.MaintenanceOrderOperationalDetailTemps.Remove(item);
                    }
                    else
                    {
                        Product productss = db.Products.Find(item.Product_Id);
                        details.Product_Id = productss.Prod_ID;

                        // Calculate the Total_Cost

                        details.Detail_Total_Cost = item.Product_Quantity * productss.Prod_Cost_Price.Value;



                        var price = item.Product_Price * item.Product_Quantity;
                        details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                        saleRecietMaster.Total_Amount += details.Total_Amount;
                        saleRecietMaster.Total_Cost += details.Detail_Total_Cost;
                        saleRecietMaster.Total_Tax += details.Detail_Total_Tax;

                        saleRecietMaster.MaintenanceOrderOperationalDetails.Add(details);
                        db.MaintenanceOrderOperationalDetailTemps.Remove(item);
                    }
                }
               

                saleRecietMaster.AllowDiscount = 0;
                saleRecietMaster.Master_Discount = saleMastVM.Master_Discount;
                saleRecietMaster.Total_Amount = saleRecietMaster.Total_Amount  - saleRecietMaster.AllowDiscount.Value;
                var MainStatus = new MaintenanceOrderOperationalStatus();
                MainStatus.DateOfState = DateTime.Now;
                MainStatus.EmployeeId = saleRecietMaster.Emplyee_id;
                MainStatus.Status = saleRecietMaster.Status;
                saleRecietMaster.MaintenanceOrderOperationalStatus.Add(MainStatus);
                var manCode = new MaintenanceOrderCoding();
                manCode.Code = saleRecietMaster.Order_Code;
                db.MaintenanceOrderCodings.Add(manCode);
                db.MaintenanceOrderOperationals.Add(saleRecietMaster);
                db.MaintenanceOrderOperationalTemps.Remove(saleRecietMasterTemp);
                db.SaveChanges();
                db.Dispose();

               

                return Tuple.Create("تم انشاء التسلم بنجاح", saleRecietMaster.Id);

            }
            return Tuple.Create("يوجد خطأ فى انشاء التسلم", 0);
        }
        public static MaintenanceOrderDetailVM prepareDetailOfOrder()
        {
            var db = new AccountingContainer();
            var DetailOfInvoice = new MaintenanceOrderDetailVM();


            List<object> newList2 = new List<object>();
            var members2 = db.Products.ToList().Where(a => a.IsActive!= false&&a.Prod_Type!=Prod_Type.اولي);
            foreach (var member in members2)
                newList2.Add(new
                {
                    Id = member.Prod_ID,
                    Name = member.Prod_Name + "         " + member.Bar_Code
                });
            DetailOfInvoice.ProductList = new SelectList(newList2, "Id", "Name");

        
            return DetailOfInvoice;
        }

        public static MaintenanceOrderOperationalDetailTempVM DetaildatatempList(int Masterid)
        {
            var db = new AccountingContainer();
            var invoicedetalTemp = new MaintenanceOrderOperationalDetailTempVM();

            var invoice = db.MaintenanceOrderOperationalTemps.Find(Masterid);
            invoicedetalTemp.invoiceTemp = invoice.MaintenanceOrderOperationalDetails.ToList();
            invoicedetalTemp.invoice_Id = Masterid;
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
     
            return invoicedetalTemp;
        }

        public static string Adddetailpostodetailtemp(MaintenanceOrderDetailVM detailVm)
        {
         
            var db = new AccountingContainer();





            var detailtemp = db.MaintenanceOrderOperationalTemps.Find(detailVm.Master_Id);
            var detailposttemp = new MaintenanceOrderOperationalDetailTemp();
            var product = db.Products.Where(pro => pro.Prod_ID == detailVm.Product_Id).FirstOrDefault();


            //var batch = db.ProductBatches.Find(detailVm.Product_Id);
            detailposttemp.Batch_ID =0;
            detailposttemp.Product_Id = product.Prod_ID;

            var detalpos = detailtemp.MaintenanceOrderOperationalDetails;
            var posdetailtemp = detalpos.Where(p => p.MaintenanceOrderOperationalId == detailtemp.Id && p.Product_Id == detailposttemp.Product_Id).ToList();
            if (posdetailtemp.FirstOrDefault() != null)
            {
                var old = posdetailtemp.FirstOrDefault();
                var oldstatic = posdetailtemp.FirstOrDefault().Total_Amount;
                old.Product_Price = posdetailtemp.FirstOrDefault().Product_Price;
                old.Product_Quantity += 1;
                





                detailposttemp.Detail_Total_Cost = product.Prod_Cost_Price.Value * old.Product_Quantity;
                if (product.Tax_Id != null && product.Tax_Id != 0)
                {
                    detailposttemp.Product_Tax = product.Tax_Id.Value;
                }

                decimal totalprice = old.Product_Price * old.Product_Quantity;
                decimal discound = totalprice * (detailVm.Discount / 100);
                decimal afterdec = totalprice - discound;
                old.TotalBeforeDisc = totalprice;
                old.TotalDiscount = discound;
                old.Detail_Total_Cost = old.Product_Quantity * product.Prod_Cost_Price.Value;
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


                //detailtemp.DetailPOSInvoiceTemps.Add(detailposttemp);
                db.Entry(old).State = EntityState.Modified;




                detailtemp.Total_Amount = (old.Total_Amount + detailtemp.Total_Amount) - oldstatic;


                db.Entry(detailtemp).State = EntityState.Modified;
                db.SaveChanges();

                return Resources.MasterPOSInvoice.AddSuccessfully;
            }

           
            else
            {
                detailposttemp.Product_Price = product.Sale_Price.Value;
                if (detailVm.CustomerID > 0)
                {
                    var CustomerSlice = db.Customers.Find(detailVm.CustomerID);
                    var slice = product.SlicesProducts.Where(a => a.SlicesSlice_Id == CustomerSlice.SlicesSlice_Id).FirstOrDefault();

                    if (slice != null)
                    {
                        detailposttemp.Product_Price = slice.Slice_Price;
                    }

                }
                //detailposttemp.Product_Price = batch.Product.Sale_Price.Value;
                detailposttemp.Product_Quantity = 1;
               

                decimal totalprice = detailposttemp.Product_Quantity * detailposttemp.Product_Price;
                decimal discound = totalprice * (detailVm.Discount / 100);
                decimal afterdec = totalprice - discound;
                detailposttemp.TotalBeforeDisc = totalprice;
                detailposttemp.TotalDiscount = discound;

                var prodtax = db.Taxes.Find(product.Tax_Id);
                if (prodtax == null)
                {
                    detailposttemp.Detail_Total_Tax = 0;
                }
                else
                {
                    detailposttemp.Detail_Total_Tax = afterdec * (prodtax.Tax_Percentage / 100);
                    detailposttemp.Product_Tax = prodtax.Tax_ID;
                }

                detailposttemp.Total_Amount = (totalprice + detailposttemp.Detail_Total_Tax) - discound;
                detailposttemp.Detail_Total_Cost = product.Prod_Cost_Price.Value * detailposttemp.Product_Quantity;
                detailtemp.MaintenanceOrderOperationalDetails.Add(detailposttemp);


                detailtemp.Total_Amount += detailposttemp.Total_Amount;



                db.Entry(detailtemp).State = EntityState.Modified;



                db.SaveChanges();
                return Resources.MasterPOSInvoice.AddSuccessfully;

            }
        }

        public static string changeSpecificPrice(int detail_Id, decimal Price, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.MaintenanceOrderOperationalDetailTemps.Find(detail_Id);
            var master = detail.MaintenanceOrderOperational;

            detail.Product_Price = Price;

            decimal totalprice = Price * detail.Product_Quantity;
            decimal discound = totalprice * (discount / 100);
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
            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;
            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return "";

        }
        public static string changeSpecificValueinPOs(int detail_Id, decimal quantity, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.MaintenanceOrderOperationalDetailTemps.Find(detail_Id);
            var master = detail.MaintenanceOrderOperational;
            var product = db.Products.Find(detail.Product_Id);
            detail.Product_Quantity = quantity;
            detail.Detail_Total_Cost = quantity * product.Prod_Cost_Price.Value;

            decimal totalprice = detail.Product_Price * detail.Product_Quantity;
            decimal discound = totalprice * (discount / 100);
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
            detail.Detail_Total_Cost = quantity * product.Prod_Cost_Price.Value;
            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;
            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return "";

        }
        public static string changeSpecificTax(int detail_Id, int product_Tax, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.MaintenanceOrderOperationalDetailTemps.Find(detail_Id);
            var master = detail.MaintenanceOrderOperational;




            decimal totalprice = detail.Product_Price * detail.Product_Quantity;
            decimal discound = totalprice * (discount / 100);
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


        public static string changeDetailOfInvoice(decimal discount, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.MaintenanceOrderOperationalTemps.Find(Invoice_Id);
            var detail = db.MaintenanceOrderOperationalDetailTemps.Where(a => a.MaintenanceOrderOperationalId == Invoice_Id).ToList();
            decimal total = 0;
            foreach (var item in detail)
            {
                item.TotalBeforeDisc = item.Product_Quantity * item.Product_Price;
                item.TotalDiscount = item.TotalBeforeDisc * (discount / 100);
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
            db.Entry(invoiceTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "تم تعديل الخصم بنجاح";

        }
        public static string DeleteTemp(int DetailID)
        {
            var db = new AccountingContainer();
            var InvoiceSalesTemp = db.MaintenanceOrderOperationalDetailTemps.Find(DetailID);
            db.MaintenanceOrderOperationalDetailTemps.Remove(InvoiceSalesTemp);
            db.SaveChanges();
            db.Dispose();
            return "تمت ازالة المنتج";
        }
        /// <summary>
        /// change invoice value depend on allow discount
        /// </summary>
        /// <param name="discount"></param>
        /// <param name="Invoice_Id"></param>
        /// <returns>string</returns>
        public static string changeDetailOfInvoiceForAllDiscount(decimal AllowDiscount, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.MaintenanceOrderOperationalTemps.Find(Invoice_Id);
            invoiceTemp.AllowDiscount = AllowDiscount;
            db.Entry(invoiceTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "تم عمل الخصم المسموح به";

        }
    }
}