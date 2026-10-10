using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class PurchaseOrderMaster
    {
        /// <summary>
        /// make unique  code for that contain code
        /// </summary>
        /// <param name="Serial_Number"></param>
        /// <returns></returns>
        public static bool containCode(string Serial_Number)
        {
            var db = new AccountingContainer();
            var allinData = db.PurchaseOrderMasters.ToList();
            foreach (var item in allinData)
            {

                if (item.Serial_Number == Serial_Number || !Serial_Number.StartsWith("PORD-"))
                {
                    return true;
                }

                else
                { }

            }
            return false;
        }

        public static string changeSpecificValueinPOs(int detail_Id, decimal quantity, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.PurchaseDetailsTemps.Find(detail_Id);
            var master = detail.PurInvoiceMasterTemp;
            var batch = db.Products.Where(pro => pro.Prod_ID == detail.Product_ID).FirstOrDefault();

            if (quantity > 0)
            {
                detail.Product_Quantity = quantity;
            }
            else
            {
                detail.Product_Quantity = 1;
            }

            decimal totalprice = detail.Product_Price * detail.Product_Quantity;
            decimal discound = totalprice * (discount / 100);
            decimal afterdec = totalprice - discound;
            detail.TotalBeforeDisc = totalprice;
            detail.TotalDiscount = discound;
            if (detail.Product_Tax == 0|| detail.Product_Tax==null)
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
            detail.Detail_Total_Cost = (totalprice + detail.Detail_Total_Tax) - discound;
            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return "";

        }
        public static string changeSpecificPrice(int detail_Id, decimal Price, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.PurchaseDetailsTemps.Find(detail_Id);
            var master = detail.PurInvoiceMasterTemp;
            if (Price >= 0)
            {
                detail.Product_Price = Price;
            }
            else
            {
                detail.Product_Price = 1;
            }
       

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
            detail.Detail_Total_Cost= (totalprice + detail.Detail_Total_Tax) - discound;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return "";

        }
        public static string changeSpecificTax(int detail_Id, int product_Tax, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.PurchaseDetailsTemps.Find(detail_Id);
            var master = detail.PurInvoiceMasterTemp;




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
            detail.Detail_Total_Cost = (totalprice + detail.Detail_Total_Tax) - discound;
            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return "";

        }
        public static string changeSpecificTotalCost(int detail_Id, decimal TotalCost, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.PurchaseDetailsTemps.Find(detail_Id);
            var master = detail.PurInvoiceMasterTemp;
            if (TotalCost >= 0)
            {
                detail.Detail_Total_Cost = TotalCost;
            }
            else
            {
                detail.Detail_Total_Cost = 1;
            }
            var prodtax = db.Taxes.Find(detail.Product_Tax);
            decimal taxPercentage = 0;
            if (prodtax == null)
            {
                detail.Detail_Total_Tax = 0;
            }
            else
            {
                taxPercentage = prodtax.Tax_Percentage / 100;

            }
            decimal Price = (TotalCost / (detail.Product_Quantity*(1+taxPercentage)*(1-(discount/100))));

            decimal totalprice = Price * detail.Product_Quantity;
            decimal discound = totalprice * (discount / 100);
            decimal afterdec = totalprice - discound;
           
            detail.TotalBeforeDisc = totalprice;
            detail.TotalDiscount = discound;
            detail.Product_Price = Price;
            detail.Detail_Total_Tax = afterdec * taxPercentage;
            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = afterdec + detail.Detail_Total_Tax;
            master.Total_Amount += detail.Total_Amount;
            detail.Detail_Total_Cost = afterdec + detail.Detail_Total_Tax;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return "";

        }
        /// <summary>
        /// get all purchase order that transform ton invoice
        /// </summary>
        /// <returns> List<PurchaseOrderMaster></returns>
        public static List<PurchaseOrderMaster> GetAllInvoicesPosted(int ?all)
        {
            var db = new AccountingContainer();
            var thisMasters = db.PurInvoiceMasters.Where(e => e.Purch_Order_Code != null).ToList();
            List<PurchaseOrderMaster> listofOrders = new List<PurchaseOrderMaster>();
            if (thisMasters.Count > 0)
            {
                foreach (var master in thisMasters)
                {
                    var thisorders = db.PurchaseOrderMasters.Where(e => e.Serial_Number == master.Purch_Order_Code).FirstOrDefault();
                    if (thisorders != null)
                    {
                        listofOrders.Add(thisorders);
                    }

                }
                if (all == null)
                {
                    listofOrders.Reverse();
                    listofOrders = listofOrders.Take(100).ToList();
                }
                return listofOrders;
            }
            else
            {
                var orders = db.PurchaseOrderMasters.ToList();
                if (all == null)
                {
                    orders.Reverse();
                    orders = orders.Take(100).ToList();
                }
                return orders;
            }

        }
        /// <summary>
        /// get All Purchase order that not transfer to invoices
        /// </summary>
        /// <returns></returns>
        public static List<PurchaseOrderMaster> GetAllInvoices(int ?all)
        {
            var db = new AccountingContainer();
            var thisMasters = db.PurInvoiceMasters.Where(e => e.Purch_Order_Code != null).ToList();
            List<PurchaseOrderMaster> listofOrders = new List<PurchaseOrderMaster>();

            var orders = db.PurchaseOrderMasters.ToList();
            foreach (var item in orders)
            {
                var thismaster = db.PurInvoiceMasters.Where(e => e.Purch_Order_Code == item.Serial_Number).ToList();
                if (thismaster.Count > 0)
                {

                }
                else
                {
                    listofOrders.Add(item);
                }

            }
            if (all == null)
            {
                listofOrders.Reverse();
                listofOrders = listofOrders.Take(100).ToList();
            }
            return listofOrders;
        }

        /// <summary>
        /// get the detail of invoices by its id
        /// </summary>
        /// <param name="detailId"></param>
        /// <returns>PurchaseOrderMaster</returns>

        public static PurchaseOrderMaster GetInvoiceDetailsById(int detailId)
        {
            var db = new AccountingContainer();
            var detail = db.PurchaseOrderMasters.Find(detailId);
            return detail;

        }
        /// <summary>
        /// get all temp for that invoice temp by the master_Id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns></returns>
        public static PurchaseDetailsTempVM GetAllTempInvoiceDetails(int masterVmId)
        {
            var db = new AccountingContainer();
            var detailtemp = new PurchaseDetailsTempVM();

            var Purchase = db.PurInvoiceMasterTemps.Where(invoice => invoice.Master_Id == masterVmId).FirstOrDefault();
          
            detailtemp.invoiceTemp = Purchase.PurchaseDetailsTemps.ToList(); 
            detailtemp.invoice_Id = masterVmId;
            if(Purchase.AllowDiscount== null)
            {
                detailtemp.AllowDiscount = 0;
            }
            return detailtemp;
        }
        /// <summary>
        /// prepare detail for creation 
        /// </summary>
        /// <returns>PurchaseOrderDetailVM</returns>
        public static PurchaseOrderDetailVM PrepareInvoiceMasterDetail()
        {
            var db = new AccountingContainer();
            var PurchaseDetailsVm = new PurchaseOrderDetailVM();
            //PurchaseDetailsVm.MyProductList = new SelectList(db.Products.Where(a => a.Prod_Type != Prod_Type.خدمة), "Prod_ID", "Prod_Name");
            var newListpro = (from prod in db.Products.Where(a => a.IsActive != false && a.Prod_Type != Prod_Type.خدمة&&a.ProductCompounds.Count() <= 1)
                              select new
                              {

                                  Id = prod.Prod_ID,
                                  Name = prod.Prod_Name + "   " + prod.Bar_Code

                              })
            ;

            PurchaseDetailsVm.MyProductList = new SelectList(newListpro, "Id", "Name");
        
            var thischartoftax = db.ChartOfAccountings.Find(35);
            var newList = from Taxes in db.Taxes.Where(e => e.Tax_Code.Contains(thischartoftax.Account_Code)).Where(a => a.IsActive != false)
                          select new
                          {

                              Id = Taxes.Tax_ID,
                              Name = Taxes.Tax_Name + "   " + Taxes.Tax_Percentage + "%"


                          };
            PurchaseDetailsVm.MyProduct_TaxList = new SelectList(newList, "Id", "Name");
            return PurchaseDetailsVm;

        }
        /// <summary>
        /// add to List and check the validation of that data 
        /// </summary>
        /// <param name="PrchaseInvoiceMasterDetail"></param>
        /// <returns>string</returns>
        public static string AddInvoiceDetailsTemp(PurchaseOrderDetailVM PrchaseInvoiceMasterDetail)
        {
            var db = new AccountingContainer();
            if (PrchaseInvoiceMasterDetail.Product_ID == null || PrchaseInvoiceMasterDetail.Product_ID == 0)
            {
                return "من فضلك اختر الصنف";
            }
            
            if (PrchaseInvoiceMasterDetail.Product_Quantity == null || PrchaseInvoiceMasterDetail.Product_Quantity <= 0)
            {
                PrchaseInvoiceMasterDetail.Product_Quantity = 1;
            }
            var purchaseInvoiceMasterTemp = db.PurInvoiceMasterTemps.Find(PrchaseInvoiceMasterDetail.InvoiceMaster_Id);
            var product = db.Products.Find(PrchaseInvoiceMasterDetail.Product_ID);
            var purchaseInvoiceMasterDetailsTemp = new PurchaseDetailsTemp();
            purchaseInvoiceMasterDetailsTemp.Product_ID = PrchaseInvoiceMasterDetail.Product_ID;
            purchaseInvoiceMasterDetailsTemp.Product_Price = product.Prod_Cost_Price.Value;
            purchaseInvoiceMasterDetailsTemp.Product_Tax = product.Tax_Id_Purch;
          var details = purchaseInvoiceMasterTemp.PurchaseDetailsTemps;
            var duplecatTemp = details.Where(p => p.PurInvoiceMasterTempMaster_Id == purchaseInvoiceMasterTemp.Master_Id && p.Product_ID == PrchaseInvoiceMasterDetail.Product_ID).ToList();
            if (duplecatTemp.FirstOrDefault() != null)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.thesameProduct;
            }
            else
            {
                purchaseInvoiceMasterDetailsTemp.Product_Quantity = PrchaseInvoiceMasterDetail.Product_Quantity;
                try
                {
                    var resultTax = db.Taxes.Find(purchaseInvoiceMasterDetailsTemp.Product_Tax);
                    if (resultTax != null)
                    {
                        purchaseInvoiceMasterDetailsTemp.Product_Tax = resultTax.Tax_ID;
                    }
                    else
                    {
                        purchaseInvoiceMasterDetailsTemp.Product_Tax = 0;
                    }
                }
                catch
                {
                    purchaseInvoiceMasterDetailsTemp.Product_Tax = 0;
                }
                var a = db.Taxes.Find(purchaseInvoiceMasterDetailsTemp.Product_Tax);
                purchaseInvoiceMasterDetailsTemp.Total_Amount = (purchaseInvoiceMasterDetailsTemp.Product_Price * purchaseInvoiceMasterDetailsTemp.Product_Quantity);
                var dicount = (PrchaseInvoiceMasterDetail.Master_Discount / 100) * purchaseInvoiceMasterDetailsTemp.Total_Amount;
                if (dicount == 0 || dicount == null)
                {

                }
                else
                {
                    purchaseInvoiceMasterDetailsTemp.Total_Amount = purchaseInvoiceMasterDetailsTemp.Total_Amount - dicount;
                }
                if (a == null)
                {
                    purchaseInvoiceMasterDetailsTemp.Detail_Total_Tax = 0;
                }
                else
                {
                    purchaseInvoiceMasterDetailsTemp.Detail_Total_Tax = (purchaseInvoiceMasterDetailsTemp.Total_Amount) * (Convert.ToDecimal(a.Tax_Percentage) / 100);
                }
                purchaseInvoiceMasterDetailsTemp.Detail_Total_Cost = (purchaseInvoiceMasterDetailsTemp.Total_Amount) + (purchaseInvoiceMasterDetailsTemp.Detail_Total_Tax);
                purchaseInvoiceMasterDetailsTemp.Total_Amount = purchaseInvoiceMasterDetailsTemp.Product_Quantity * purchaseInvoiceMasterDetailsTemp.Product_Price;
                purchaseInvoiceMasterTemp.PurchaseDetailsTemps.Add(purchaseInvoiceMasterDetailsTemp);

                db.Entry(purchaseInvoiceMasterTemp).State = EntityState.Modified;
                db.SaveChanges();

                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Addedsuccessfully;
            }

            
        }
        /// <summary>
        /// Prepare Creation purchase Order
        /// </summary>
        /// <returns>PurchaseOrderMasterVM</returns>
        public static PurchaseOrderMasterVM AddNewForCreation()
        {
            var db = new AccountingContainer();
            var masterVM = new PurchaseOrderMasterVM();
            masterVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            masterVM.MySupplierList = new SelectList(db.Suppliers.Where(a=>a.Supplier_state!=Customer_State.غير_نشط), "Supp_Id", "Supp_Name");
            masterVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            masterVM.MyBussinesUnitList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            masterVM.MyStoresList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            masterVM.BU_List = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            masterVM.MyEmpList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            masterVM.MyBranchList = new SelectList(db.Branches, "Branch_Id", "Branch_Name");
            var purchaseMasterTemp = new PurInvoiceMasterTemp();

            var id = (from s in db.PurInvoiceMasterTemps
                      select s).ToList().LastOrDefault();
            if (id == null)
            { purchaseMasterTemp.Master_Id = 1; }
            else
            {
                purchaseMasterTemp.Master_Id = id.Master_Id + 1;
            }
            var lastSerial = db.PurchaseOrderMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                masterVM.Serial_Number = "PORD-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                masterVM.Serial_Number = "PORD-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(5, lastSerial.Serial_Number.Length - 5)) + 1).ToString("D3");
            }
            db.PurInvoiceMasterTemps.Add(purchaseMasterTemp);
            masterVM.Payment_Policy = 0;
            db.SaveChanges();
            masterVM.Master_Id = purchaseMasterTemp.Master_Id;
            return masterVM;
        }
        /// <summary>
        /// add new Purchase order and save it in data base
        /// </summary>
        /// <param name="masterVM"></param>
        /// <param name="UserId"></param>
        /// <returns>string</returns>
        public static Tuple< string,int> AddNewInvoice(PurchaseOrderMasterVM masterVM , String UserId)
        {
            var db = new AccountingContainer();
            var master = new PurchaseOrderMaster();

            var PurchaseMasterTemp = db.PurInvoiceMasterTemps.Find(masterVM.Master_Id);
            //var SelectedInvoice_SupplierSerial = db.PurchaseOrderMasters.Where(i => masterVM.Supplier_Id == master.Supplier_Id);
            if (masterVM != null)
            {
                master.Description = masterVM.Description;
                var lastSerial = db.PurchaseOrderMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                if (lastSerial == null)
                {
                    master.Serial_Number = "PORD-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    master.Serial_Number = "PORD-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(5, lastSerial.Serial_Number.Length - 5)) + 1).ToString("D3");
                }
                master.ExchangeRate = masterVM.ExchangeRate;
                master.Subledger_Id = masterVM.Subledger_Id;
                var resultSupplier = db.Suppliers.Find(masterVM.Supplier_Id);
                if (resultSupplier != null)
                {
                    master.Supplier_Id = resultSupplier.Supp_Id;
                }

                var resultCompany = db.Companies.FirstOrDefault();
                if (resultCompany != null)
                {
                    master.Company_Id = resultCompany.Comp_Id;
                }
            
                var resultCurrency = db.Currencies.FirstOrDefault();
                if (resultCurrency != null)
                {
                    master.Invoice_Currency = resultCurrency.Currency_Type;
                }

                //var resultAccount = db.ChartOfAccountings.Find(masterVM.SelectedAccount_Id);
                //if (resultAccount != null)
                //{
                //    master.Account_Id = resultAccount.Account_Id;
                //}

                var resultBU = db.BussinesUnits.Find(masterVM.BU_Id);
                if (resultBU != null)
                {
                    master.BU_Id = resultBU.BU_Id;
                }

                var resultStore = db.Stores.Find(masterVM.Store_Id);
                if (resultStore != null)
                {
                    master.Store_Id = resultStore.Store_ID;
                }
                else
                {
                    master.Store_Id = db.Stores.FirstOrDefault().Store_ID;
                }

                var resultBranch = db.Branches.FirstOrDefault();
                if (resultBranch != null)
                {
                    master.Branch_Id = resultBranch.Branch_Id;
                }


                var resultEmp = db.Employees.Find(masterVM.Emp_Id);
                if (resultEmp != null)
                {
                    master.Emp_Id = resultEmp.Emp_Id;
                }



                master.Master_Discount = masterVM.Master_Discount;
                if (masterVM.Payment_Policy == null)
                {
                    master.Payment_Policy = 0;
                }
                else
                {
                    master.Payment_Policy = masterVM.Payment_Policy.Value;
                }
             
                master.Reference = masterVM.Reference;

                master.OrderDate =Convert.ToDateTime( masterVM.orderDate);

                master.Document_Type = Document_Type.Purchase_Invoice; //////////// Purchase Invoice



                if (masterVM.Files.FirstOrDefault() != null)
                {
                    foreach (var file in masterVM.Files)
                    {
                        var filedetails = new PurchaseFileOrder();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.FileContent = data;
                        filedetails.FileType = file.ContentType;
                        if (data.Length < 1000000)
                        {
                            master.PurchaseFileOrders.Add(filedetails);
                        }
                       
                    }
                }          
                if(PurchaseMasterTemp.PurchaseDetailsTemps.Count()==0)
                {
                    return Tuple.Create("محتويات امر الشراء فارغة",0) ;
                }

                foreach (var item in PurchaseMasterTemp.PurchaseDetailsTemps.ToList())
                {
                    var details = new PurchaseOrderDetail();
                    // details.Details_Id = item.Details_Id;
                    details.Product_ID = item.Product_ID;
                    details.Product_Quantity = item.Product_Quantity;
                    details.Product_Price = item.Product_Price;
                    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    decimal discount = TotalBeforeDiscount * (masterVM.Master_Discount / 100);
                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                    decimal tax = 0;
                    if (item.Product_Tax ==0|item.Product_Tax==null)
                    {
                        details.Detail_Total_Tax = 0;
                    }
                    else
                    {
                        details.Product_Tax = Convert.ToInt32(item.Product_Tax);
                        tax = Convert.ToDecimal(db.Taxes.Find(details.Product_Tax).Tax_Percentage);
                        details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                    }

                    // details.InvoiceMaster_Id = masterVM.Master_Id;
                    details.Total_Amount = TotalBeforeDiscount;

                   
                    details.Detail_Total_Cost = TotalAfterDiscount+ details.Detail_Total_Tax;


                    master.Total_Cost += details.Detail_Total_Cost;
                    master.Total_Amount += details.Total_Amount;
                    master.Total_Tax += details.Detail_Total_Tax;
                    master.PurchaseOrderDetails.Add(details);
                    db.PurchaseDetailsTemps.Remove(item);

                }


                db.PurchaseOrderMasters.Add(master);
                db.PurInvoiceMasterTemps.Remove(PurchaseMasterTemp);
                var traBatch = new TransactionBatch();

                traBatch.OperationName = "امر شراء";
                traBatch.Description = null;
                traBatch.Date = Convert.ToDateTime(master.OrderDate);
                traBatch.UserId = UserId;
                traBatch.OperationId = master.Master_Id;
                traBatch.Operation_Code = master.Serial_Number;
                TransactionBatch.AddNewTransaction(traBatch);
                db.SaveChanges();
                db.Dispose();


                return Tuple.Create(Resources.validate.CreateSucc,master.Master_Id);
            }

            return Tuple.Create(Resources.validate.Error, 0);
        }
        /// <summary>
        /// get that file by its id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>PurchaseFileOrder</returns>
        public static PurchaseFileOrder getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.PurchaseFileOrders.Find(fileId);
            return file;
        }
        public static string deleteFile(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.PurchaseFileOrders.Find(fileId);
            db.PurchaseFileOrders.Remove(file);
            db.SaveChanges();
            return Resources.validate.DeleteSuccessful;
        }
        /// <summary>
        /// get all file for purchase order by its id
        /// </summary>
        /// <param name="invoiceMasterId"></param>
        /// <returns> List<PurchaseFileOrder></returns>
        public static List<PurchaseFileOrder> GetAllFiles(int invoiceMasterId)
        {
            var db = new AccountingContainer();
            var files = db.PurchaseFileOrders.Where(emp => emp.Id == invoiceMasterId).ToList();
            return files;
        }

        public static IEnumerable<SelectListItem> GetProudectList()
        {
            var db = new AccountingContainer();
            var prod = from prodc in db.Products
                       select new SelectListItem
                       {
                           Text = prodc.Prod_Name + " " + prodc.Bar_Code,
                           Value = prodc.Prod_ID.ToString()
                       };


            return prod;
        }

        /// <summary>
        /// get All supplier for dropdownList
        /// </summary>
        /// <returns></returns>

        public static IEnumerable<SelectListItem> GetSupplierList()
        {
            var db = new AccountingContainer();
            PurMasterVM vm = new PurMasterVM();

            var supplierList = from supp in db.Suppliers.Where(a => a.Supplier_state == Customer_State.نشط)
                               select new SelectListItem
                               {
                                   Text = supp.Supp_Name,
                                   Value = supp.Supp_Id.ToString()
                               };

            return supplierList;

        }


        //fill for Edit Purchase Order

        public static PurchaseOrderMasterVM fillForEditingInvoice(int invoice,int isCopy)
        {
            var db = new AccountingContainer();
            var PurOrder = db.PurchaseOrderMasters.Find(invoice);
            var PurOrderVM = new PurchaseOrderMasterVM();
            PurOrderVM.Description = PurOrder.Description;
            PurOrderVM.Master_Discount = PurOrder.Master_Discount;
            PurOrderVM.orderDate = PurOrder.OrderDate.ToString("dd-MM-yyyy   HH:mm:ss");
            var cur = db.Currencies.Where(a => a.Currency_Type == PurOrder.Invoice_Currency).FirstOrDefault();
            if (cur!=null)
            {
                PurOrderVM.Invoice_Currency = cur.Currency_Type;
            }
            PurOrderVM.isCopy = isCopy;
            PurOrderVM.Invoice_Currency = PurOrder.Invoice_Currency;
            PurOrderVM.Document_Type = PurOrder.Document_Type;
            PurOrderVM.Master_Id = PurOrder.Master_Id;
            PurOrderVM.Supplier_Id = PurOrder.Supplier_Id;
            PurOrderVM.Store_Id = PurOrder.Store_Id;
            PurOrderVM.Company_Id = PurOrder.Company_Id;
          
            PurOrderVM.Subledger_Id = PurOrder.Subledger_Id;
            PurOrderVM.BU_Id = PurOrder.BU_Id;
            PurOrderVM.Account_Id = PurOrder.Account_Id;
            PurOrderVM.Branch_Id = PurOrder.Branch_Id;
            PurOrderVM.Emp_Id = PurOrder.Emp_Id;
            PurOrderVM.Payment_Policy = PurOrder.Payment_Policy;
            PurOrderVM.Reference = PurOrder.Reference;
            PurOrderVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            PurOrderVM.MySupplierList = new SelectList(db.Suppliers.Where(a=>a.Supplier_state!=Customer_State.غير_نشط), "Supp_Id", "Supp_Name");
            PurOrderVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            PurOrderVM.MyBussinesUnitList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            PurOrderVM.MyStoresList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            PurOrderVM.BU_List = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            PurOrderVM.MyEmpList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            PurOrderVM.MyBranchList = new SelectList(db.Branches.Where(a=>a.IsActive!=false), "Branch_Id", "Branch_Name");
            var PurchaseTemp = new PurInvoiceMasterTemp();
            PurchaseTemp.Description = PurOrder.Description;
            PurchaseTemp.Master_Discount = PurOrder.Master_Discount;

            PurchaseTemp.Invoice_Currency = PurOrder.Invoice_Currency;

            PurchaseTemp.Document_Type = PurOrder.Document_Type;
            PurchaseTemp.Supplier_Id = PurOrder.Supplier_Id;
            PurchaseTemp.Store_ID = PurOrder.Store_Id;
            PurchaseTemp.Company_Id = PurOrder.Company_Id;
          
            PurchaseTemp.Subledger_Id = PurOrder.Subledger_Id;
            PurchaseTemp.BU_Id = PurOrder.BU_Id;
            PurchaseTemp.Account_Id = PurOrder.Account_Id;
            PurchaseTemp.Branch_Id = PurOrder.Branch_Id;
            PurchaseTemp.Emp_Id = PurOrder.Emp_Id;
            //PurchaseTemp.Payment_Policy = PurOrder.Payment_Policy;
            PurchaseTemp.Reference = PurOrder.Reference;
            var ThereORN = db.PurInvoiceMasterTemps.Find(PurOrder.Master_Id);
            if (ThereORN != null)
            {

            }
            else
            {
                PurchaseTemp.Master_Id = PurOrder.Master_Id;

                db.PurInvoiceMasterTemps.Add(PurchaseTemp);
            }
            PurOrderVM.Serial_Number = PurOrder.Serial_Number;
            if (isCopy == 1)
            {
                var lastSerial = db.PurchaseOrderMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                if (lastSerial == null)
                {
                    PurOrderVM.Serial_Number = "PORD-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    PurOrderVM.Serial_Number = "PORD-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(5, lastSerial.Serial_Number.Length - 5)) + 1).ToString("D3");
                }
            }
            db.SaveChanges();
            
            return PurOrderVM;

        }
        /// <summary>
        /// get all detail of Purchase order by order master id
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns>List<PurchaseOrderDetail> </returns>
        public static List<PurchaseOrderDetail> getAlldetailInInvoice(int masterId)
        {
            var db = new AccountingContainer();
            var detail = db.PurchaseOrderDetails.Where(a => a.InvoiceMaster_Id == masterId).ToList();

            return detail;



        }
        /// <summary>
        /// get the detail of Purchase order by its id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static PurchaseOrderMaster GetpurchInvByID(int ID)
        {
            var db = new AccountingContainer();
            var invoiceMaster = db.PurchaseOrderMasters.Where(c => c.Master_Id == ID);
            var invoice = new PurchaseOrderMaster();

            if (invoiceMaster != null)
            {
                invoice = invoiceMaster.FirstOrDefault();
                return invoice;
            }
            return null;
        }
        /// <summary>
        /// prepare the detail vm for editing
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns>PurchaseOrderDetailVM</returns>
        public static PurchaseOrderDetailVM PreparpurchMasterDetail2(int masterId)
        {
            var db = new AccountingContainer();
            var purInvoiceDVM = new PurchaseOrderDetailVM();
            if (masterId <= 0)
            {
                var PurchaseDetailsVm = new PurchaseOrderDetailVM();
                //PurchaseDetailsVm.MyProductList = new SelectList(db.Products.Where(a => a.Prod_Type != Prod_Type.خدمة), "Prod_ID", "Prod_Name");
                List<object> newListpro = new List<object>();
                var membersPro = db.Products.Where(a => a.Prod_Type != Prod_Type.خدمة&& a.IsActive != false&& a.ProductCompounds.Count() <= 1);
                foreach (var member in membersPro)
                    newListpro.Add(new
                    {
                        Id = member.Prod_ID,
                        Name = member.Prod_Name + "   " + member.Bar_Code
                    });

                PurchaseDetailsVm.MyProductList = new SelectList(newListpro, "Id", "Name");
                List<object> newList = new List<object>();
                var thischartoftax = db.ChartOfAccountings.Find(35);
                var members = db.Taxes.Where(e=>e.Tax_Code.Contains(thischartoftax.Account_Code)).Where(a => a.IsActive != false).ToList();
                foreach (var member in members)
                    newList.Add(new
                    {
                        Id = member.Tax_ID,
                        Name = member.Tax_Name + "   " + member.Tax_Percentage + "%"
                    });
                PurchaseDetailsVm.MyProduct_TaxList = new SelectList(newList, "Id", "Name");
                return PurchaseDetailsVm;
            }
            else
            {
                var thisdetails = GetAllTempPURCHInvoiceDetailsDetails(masterId).ToList().FirstOrDefault();
                var product = thisdetails.Product_ID;
                purInvoiceDVM.Product_Quantity = thisdetails.Product_Quantity;
                purInvoiceDVM.Id = thisdetails.Details_Id;
                purInvoiceDVM.Product_Price = thisdetails.Product_Price;
                if (thisdetails.Product_Tax != null)
                {
                    purInvoiceDVM.Product_Tax = thisdetails.Product_Tax.Value;
                }
                purInvoiceDVM.Detail_Total_Tax = thisdetails.Detail_Total_Tax;
                purInvoiceDVM.InvoiceMaster_Id = masterId;
                List<object> newList = new List<object>();
                var members = db.Taxes.ToList().Where(a => a.IsActive != false);
                foreach (var member in members)
                    newList.Add(new
                    {
                        Id = member.Tax_ID,
                        Name = member.Tax_Name + "   " + member.Tax_Percentage + "%"
                    });
                purInvoiceDVM.MyProduct_TaxList = new SelectList(newList, "Id", "Name");
                //purInvoiceDVM.MyProductList = new SelectList(db.Products.Where(a => a.Prod_Type != Prod_Type.خدمة), "Prod_ID", "Prod_Name");
                List<object> newListpro = new List<object>();
                var membersPro = db.Products.Where(a => a.Prod_Type != Prod_Type.خدمة && a.Prod_Type != Prod_Type.مجمع);
                foreach (var member in membersPro)
                    newListpro.Add(new
                    {
                        Id = member.Prod_ID,
                        Name = member.Prod_Name + "   " + member.Bar_Code
                    });

                purInvoiceDVM.MyProductList = new SelectList(newListpro, "Id", "Name");
                var prod = db.Products.Where(c => c.Prod_ID == product);
                if (prod.Any())
                {
                    purInvoiceDVM.Product_ID = prod.FirstOrDefault().Prod_ID;
                }
                return purInvoiceDVM;
            }
            
        }
        /// <summary>
        /// get detail of purchase by master id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>List<PurchaseOrderDetail> </returns>
        public static List<PurchaseOrderDetail> GetAllTempPURCHInvoiceDetailsDetails(int masterVmId)
        {
            var db = new AccountingContainer();
            var DetailsTemp = db.PurchaseOrderDetails.Where(invoice => invoice.Details_Id == masterVmId).ToList();
            return DetailsTemp;
        }
        /// <summary>
        /// get detail of purchase by master id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>List<PurchaseOrderDetail> </returns>

        public static List<PurchaseOrderDetail> GetAllTempPURCHInvoiceDetails2(int masterVmId)

        {
            var db = new AccountingContainer();
            var Details = db.PurchaseOrderDetails.Where(invoice => invoice.InvoiceMaster_Id == masterVmId).ToList();
            return Details;
        }
        /// <summary>
        /// get all temp detail by master id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>List<PurchaseDetailsTemp></returns>
        public static List<PurchaseDetailsTemp> GetAllTempPurchInvoiceDetailsresult2(int masterVmId)
        {
            var db = new AccountingContainer();
            var InvoiceSalesTemp = db.PurInvoiceMasterTemps.Find(masterVmId);
            return InvoiceSalesTemp.PurchaseDetailsTemps.ToList();
        }

        /// <summary>
        /// add to list for detail temp and check the validation
        /// </summary>
        /// <param name="Detail"></param>
        /// <returns></returns>
        public static string AddOrderDetailsTemp(PurchaseOrderDetailVM Detail)
        {
            var db = new AccountingContainer();
            if (Detail.Product_ID == null || Detail.Product_ID == 0)
            {
                return "من فضلك اختر الصنف";
            }
            if (Detail.Product_Price == null || Detail.Product_Price == 0)
            {
                return "من فضلك ادخل السعر ";
            }
            if (Detail.Product_Quantity == null || Detail.Product_Quantity == 0)
            {
                return "من فضلك ادخل الكمية ";
            }
            var purOrderTemp = db.PurInvoiceMasterTemps.Find(Detail.InvoiceMaster_Id);
            var PurOrderDetailsTemp = new PurchaseDetailsTemp();
            PurOrderDetailsTemp.Product_ID = Detail.Product_ID;
            PurOrderDetailsTemp.Product_Price = Detail.Product_Price;

            var details = purOrderTemp.PurchaseDetailsTemps;
            var duplecatTemp = details.Where(p => p.PurInvoiceMasterTempMaster_Id == purOrderTemp.Master_Id && p.Product_ID == Detail.Product_ID).ToList();
            if (duplecatTemp.FirstOrDefault() != null)
            {
                return "هذا المنتج موجود بالفعل فى القائمة لايمكن اضافته مرة اخرى";
            }
            else
            {
                PurOrderDetailsTemp.Product_Quantity = Detail.Product_Quantity;
                var resultTax = db.Taxes.Find(Detail.Product_Tax);
                if (resultTax != null)
                {

                    PurOrderDetailsTemp.Product_Tax = resultTax.Tax_ID;

                }
                var a = db.Taxes.Find(PurOrderDetailsTemp.Product_Tax);
                PurOrderDetailsTemp.Detail_Total_Tax = (Detail.Product_Price * Detail.Product_Quantity) * (Convert.ToDecimal(a.Tax_Percentage) / 100);

                PurOrderDetailsTemp.Total_Amount = (Detail.Product_Price * Detail.Product_Quantity) + PurOrderDetailsTemp.Detail_Total_Tax;
                PurOrderDetailsTemp.Detail_Total_Cost = PurOrderDetailsTemp.Product_Price * PurOrderDetailsTemp.Product_Quantity;
                purOrderTemp.PurchaseDetailsTemps.Add(PurOrderDetailsTemp);
                db.Entry(purOrderTemp).State = EntityState.Modified;
                db.SaveChanges();
                return "تم الاضافة  بنجاح ";
            }
           
        }
        /// <summary>
        /// saveing that Edit for that order
        /// </summary>
        /// <param name="masterVM"></param>
        /// <returns></returns>

        public static Tuple<string,int> editinvoice(PurchaseOrderMasterVM masterVM)
        {
            var db = new AccountingContainer();
            var Order = db.PurchaseOrderMasters.Find(masterVM.Master_Id);
            var Ordertemp = db.PurInvoiceMasterTemps.Find(masterVM.Master_Id);
            Order.Master_Id = masterVM.Master_Id;
            Order.Description = masterVM.Description;
            Order.OrderDate = Convert.ToDateTime( masterVM.orderDate);
            Order.Master_Discount = masterVM.Master_Discount;
            if (masterVM.Files.FirstOrDefault() != null)
            {
                foreach (var file in masterVM.Files)
                {
                    var filedetails = new PurchaseFileOrder();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.FileContent = data;
                    filedetails.FileType = file.ContentType;
                    if (data.Length < 1000000)
                    {
                        Order.PurchaseFileOrders.Add(filedetails);
                    }
                    
                   
                }
            }
            var currResult = db.Currencies.Where(a => a.Currency_Type == masterVM.Invoice_Currency).FirstOrDefault();
            if (currResult != null)
            {
                Order.Invoice_Currency = currResult.Currency_Type;

            }
            Order.Document_Type = masterVM.Document_Type;
            Order.Supplier_Id = masterVM.Supplier_Id;
            if (masterVM.Store_Id > 0)
            {
                Order.Store_Id = masterVM.Store_Id;
            }
            Order.Company_Id = db.Companies.FirstOrDefault().Comp_Id;
            Order.ExchangeRate = masterVM.ExchangeRate;
            Order.Subledger_Id = masterVM.Subledger_Id;
            Order.BU_Id = masterVM.BU_Id;
            Order.Account_Id = masterVM.Account_Id;

            Order.Branch_Id = masterVM.Branch_Id;
            Order.Emp_Id = masterVM.Emp_Id;
            Order.Payment_Policy = masterVM.Payment_Policy;
            Order.Reference = masterVM.Reference;
            
            decimal Sumtax = 0;
            decimal SumtAmount = 0;
            decimal SumTotalCost = 0;
            decimal tax = 0;

            var oldDetail = db.PurchaseOrderDetails.Where(a => a.InvoiceMaster_Id == Order.Master_Id).ToList();
            var detailListTemp = db.PurchaseDetailsTemps.Where(e => e.PurInvoiceMasterTempMaster_Id == Ordertemp.Master_Id).ToList();
            List<PurchaseDetailsTemp> ListTempRemove = new List<PurchaseDetailsTemp> { };
            List<PurchaseOrderDetail> ListOrderDetails = new List<PurchaseOrderDetail> { };
            if (detailListTemp.Count() == 0)
            {
                foreach (var detail in oldDetail)
                {
                    var item = new PurchaseOrderDetail();

                    item.Product_ID = detail.Product_ID;
                    item.Product_Price = detail.Product_Price;
                    item.Product_Quantity = detail.Product_Quantity;


                    // Deduct the discount from the detail Total_Amount 
                    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    decimal discount = TotalBeforeDiscount * (Order.Master_Discount / 100);
                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                    if (detail.Product_Tax == 0)
                    {
                        item.Detail_Total_Tax = 0;

                    }
                    else
                    {
                        item.Product_Tax = Convert.ToInt32(detail.Product_Tax);
                        tax = Convert.ToDecimal(db.Taxes.Find(detail.Product_Tax).Tax_Percentage);
                        item.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                    }




                    item.Total_Amount = TotalBeforeDiscount;
                    item.Detail_Total_Cost = (TotalBeforeDiscount + item.Detail_Total_Tax) - discount;

                    SumtAmount += (decimal)item.Total_Amount;

                    Sumtax += (decimal)item.Detail_Total_Tax;
                    SumTotalCost += (decimal)item.Detail_Total_Cost;
                    Order.Total_Tax = Sumtax;
                    Order.Total_Amount = SumtAmount;
                    Order.Total_Cost = SumTotalCost;

                    Order.PurchaseOrderDetails.Add(item);
                    ListOrderDetails.Add(detail);
                    //db.PurchaseDetailsTemps.Remove(detail);
                    //db.SaveChanges();

                }
                
                db.Entry(Order).State = EntityState.Modified;                
                db.PurchaseOrderDetails.RemoveRange(oldDetail);
                db.SaveChanges();
                return Tuple.Create(Resources.validate.CreateSucc,Order.Master_Id);
            }
            else
            {
                foreach (var detail in detailListTemp)
                {
                    var item = new PurchaseOrderDetail();

                    item.Product_ID = detail.Product_ID;
                    item.Product_Price = detail.Product_Price;
                    item.Product_Quantity = detail.Product_Quantity;


                    // Deduct the discount from the detail Total_Amount 
                    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    decimal discount = TotalBeforeDiscount * (Order.Master_Discount / 100);
                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                    if (detail.Product_Tax == 0|| detail.Product_Tax==null)
                    {
                        item.Detail_Total_Tax = 0;

                    }
                    else
                    {
                        item.Product_Tax = Convert.ToInt32(detail.Product_Tax);
                        tax = Convert.ToDecimal(db.Taxes.Find(detail.Product_Tax).Tax_Percentage);
                        item.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                    }




                    item.Total_Amount = TotalBeforeDiscount;
                    item.Detail_Total_Cost = (TotalBeforeDiscount + item.Detail_Total_Tax) - discount;

                    SumtAmount += (decimal)item.Total_Amount;

                    Sumtax += (decimal)item.Detail_Total_Tax;
                    SumTotalCost += (decimal)item.Detail_Total_Cost;
                    Order.Total_Tax = Sumtax;
                    Order.Total_Amount = SumtAmount;
                    Order.Total_Cost = SumTotalCost;

                    Order.PurchaseOrderDetails.Add(item);
                    ListTempRemove.Add(detail);
                    //db.PurchaseDetailsTemps.Remove(detail);
                    //db.SaveChanges();

                }
                db.PurchaseDetailsTemps.RemoveRange(ListTempRemove);
                db.Entry(Order).State = EntityState.Modified;
                db.PurInvoiceMasterTemps.Remove(Ordertemp);
                db.PurchaseOrderDetails.RemoveRange(oldDetail);
                db.SaveChanges();
                return Tuple.Create(Resources.validate.CreateSucc, Order.Master_Id);
            }
           
        
        }
        /// <summary>
        /// put the detail of order in detail of order temp for Editing detail
        /// </summary>
        /// <param name="MasterVMID"></param>
        /// <returns>List<PurchaseDetailsTemp></returns>
        public static PurchaseDetailsTempVM GetOneListofDetailsFirst(int MasterVMID)
        {
            var db = new AccountingContainer();
            PurchaseDetailsTempVM vm = new PurchaseDetailsTempVM();
            var thiadetailtemp = db.PurchaseDetailsTemps.ToList();
            if (thiadetailtemp.Count > 0)
            {
                db.PurchaseDetailsTemps.RemoveRange(thiadetailtemp);
                db.SaveChanges();
            }
            var thisAcutauldetail = db.PurchaseOrderDetails.Where(e => e.InvoiceMaster_Id == MasterVMID).ToList();
            vm.AllowDiscount = 0;
            vm.invoice_Id = MasterVMID;
            if (thisAcutauldetail.Count > 0)
            {
                foreach (var detail in thisAcutauldetail)
                {
                    var purchasedetiltemp = new PurchaseDetailsTemp();
                    purchasedetiltemp.Product_Quantity = detail.Product_Quantity;
                    purchasedetiltemp.Product_Price = detail.Product_Price;
                    purchasedetiltemp.Total_Amount = detail.Total_Amount;
                    if (detail.Product_Tax == null)
                    {

                    }
                    else
                    {
                        purchasedetiltemp.Product_Tax = (int)detail.Product_Tax;
                    }
                    purchasedetiltemp.Detail_Total_Tax = detail.Detail_Total_Tax;
                    purchasedetiltemp.Detail_Total_Cost = detail.Detail_Total_Cost;
                    purchasedetiltemp.Product_ID = detail.Product_ID;
                    purchasedetiltemp.PurInvoiceMasterTempMaster_Id = detail.InvoiceMaster_Id;
                    var thisporduct = db.Products.Find(detail.Product_ID);
                    purchasedetiltemp.Product_Code = thisporduct.Prod_Code;
                    db.PurchaseDetailsTemps.Add(purchasedetiltemp);

                }

            }
            else
            {
            }
            db.SaveChanges();
          vm.invoiceTemp = db.PurchaseDetailsTemps.ToList();
            
            return vm;

        }
        /// <summary>
        /// get detail temp for that master
        /// </summary>
        /// <param name="mastervmID"></param>
        /// <returns> List<PurchaseDetailsTemp> </returns>
        public static List<PurchaseDetailsTemp> GetOneListofDetailssecond(int mastervmID)
        {
            var db = new AccountingContainer();
          
            var finshedtemp = db.PurchaseDetailsTemps.Where(e=>e.PurInvoiceMasterTempMaster_Id==mastervmID).ToList();
            return finshedtemp;
        }
        /// <summary>
        /// delete detail temp by its id
        /// </summary>
        /// <param name="DetailID"></param>
        /// <param name="masterID"></param>
        /// <returns>string</returns>
        public static string DeleteFromORderTemp(int DetailID,int  masterID)
        {
            var db = new AccountingContainer();
            var allDetails = db.PurchaseDetailsTemps.Where(e => e.PurInvoiceMasterTempMaster_Id == masterID).ToList();
          
                var finshedtemp = db.PurchaseDetailsTemps.Find(DetailID);
                db.PurchaseDetailsTemps.Remove(finshedtemp);
                db.SaveChanges();
                db.Dispose();
                return Resources.validate.DeleteSuccessful;
            
          
       
        }
        /// <summary>
        /// delete detail temp by its id
        /// </summary>
        /// <param name="DetailID"></param>
        /// <param name="masterID"></param>
        /// <returns>string</returns>

        public static string DeleteFromORderTemp2(int iD)
        {
            var db = new AccountingContainer();
            
       
                var finshedtemp = db.PurchaseDetailsTemps.Find(iD);
                db.PurchaseDetailsTemps.Remove(finshedtemp);
                db.SaveChanges();
                db.Dispose();
            return Resources.validate.DeleteSuccessful;

        }



        //delete the purchase order by id 
        public static int DeletePurchaseOrder(int orderId)
        {
            var db = new AccountingContainer();
            var purchaseOrder = db.PurchaseOrderMasters.Find(orderId);
            if (PurchaseOrderIsExist(orderId))
            {
                return 0;
            }
            else
            {
                db.PurchaseOrderDetails.RemoveRange(purchaseOrder.PurchaseOrderDetails);
                db.PurchaseFileOrders.RemoveRange(purchaseOrder.PurchaseFileOrders);
                db.PurchaseOrderMasters.Remove(purchaseOrder);
                db.SaveChanges();
                db.Dispose();
                return 1;

            }
        }

        /// <summary>
        /// check that order transform to invoice or not
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static bool PurchaseOrderIsExist(int orderId)
        {
            var db = new AccountingContainer();
            var purchaseOrder = db.PurchaseOrderMasters.Find(orderId);
            var purchaseInvoices = db.PurInvoiceMasters.Where(p => p.Purch_Order_Code == purchaseOrder.Serial_Number && (p.Invoice_State == TransactionState.Pending || p.Invoice_State == TransactionState.Post)).ToList();
            if (purchaseInvoices.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// change detail of invoice by change of discount
        /// </summary>
        /// <param name="discount"></param>
        /// <param name="Invoice_Id"></param>
        /// <returns>string</returns>

        public static string changeDetailOfInvoice(decimal discount, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.PurInvoiceMasterTemps.Find(Invoice_Id);
            var detail = db.PurchaseDetailsTemps.Where(a => a.PurInvoiceMasterTempMaster_Id == Invoice_Id).ToList();
            decimal total = 0;
            foreach (var item in detail)
            {
                item.TotalDiscount = item.Total_Amount * (discount / 100);
                if (item.Product_Tax == null || item.Product_Tax == 0)
                {
                    item.Detail_Total_Tax = 0;
                }
                else
                {
                    var tax = db.Taxes.Find(item.Product_Tax).Tax_Percentage;


                    item.Detail_Total_Tax = (item.Total_Amount- item.TotalDiscount.Value) * (tax / 100);
                }
                item.Detail_Total_Cost = (item.Total_Amount - item.TotalDiscount.Value) + item.Detail_Total_Tax;

                db.Entry(item).State = EntityState.Modified;
                total += item.Total_Amount;



            }
            invoiceTemp.Total_Amount = total;
            db.Entry(invoiceTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "تم تعديل الخصم بنجاح";

        }



    }
}

