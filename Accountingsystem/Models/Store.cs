using Accountingsystem.ViewModel;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Accountingsystem.Models
{
    public partial class Store
    {
        public static readonly string StoreParentAccountCode = "010105";

        // use this method to  Get List of Store from Data base. 
        public static List<Store> GetAllStore()
        {
            var db = new AccountingContainer();
            var stors = db.Stores.Where(a => a.isActive != false).ToList();
            return stors;
        }
        public static List<Store> GetAllStoreNotActive()
        {
            var db = new AccountingContainer();
            var stors = db.Stores.Where(a=>a.isActive==false).ToList();
            return stors;
        }
        // use this method to  Get one Element from Store from Data base. 
        public static Store GetStoreById(int id)
        {
            var db = new AccountingContainer();
            var store = db.Stores.Find(id);
            return store;

         
        }
        public static Store GetStoreByIdForDetail(int id)
        {
            var db = new AccountingContainer();
            var store = db.Stores.Find(id);
           

            //gettheDifferenceOfAccountandstoreCard(id);
            //var thispatch = db.ProductBatches.Where(e => e.Store_ID == store.Store_ID).ToList();
            //store.Total_Batch_Cost = 0;
            ////foreach (var Bat in thispatch)
            ////{

            ////    store.Total_Batch_Cost += Bat.Batch_Cost;
            ////}
            //try
            //{
            //    store.Total_Batch_Cost = db.ProductBatches.Where(a => a.Store_ID == id).Sum(a => a.Batch_Cost);
            //    db.Entry(store).State = EntityState.Modified;
            //    db.SaveChanges();
            //}
            //catch { }
            var batchList = (from batch in store.ProductBatches
                             join card in db.StoreCards.Where(a => a.Store_Id == store.Store_ID) on batch.Product_ID equals card.Product_Id into Cards
                             join product in db.Products on batch.Product_ID equals product.Prod_ID



                             select new ProductBatches
                             {
                                 SalesPrice = batch.SalesPrice,
                                 Product_Quantity = Cards.Where(a => a.IncreaseQuantity == true).Select(a => a.Quantity).Sum() - Cards.Where(a => a.IncreaseQuantity == false).Select(a => a.Quantity).Sum(),
                                 Batch_Cost = Cards.Where(a => a.IncreaseQuantity == true).Select(a => a.TotalCost.Value).Sum() - Cards.Where(a => a.IncreaseQuantity == false).Select(a => a.TotalCost.Value).Sum(),
                                 Product = product
                             }).ToList();
            store.ProductBatches = new List<ProductBatches>();
            store.ProductBatches = batchList;
            return store;


        }
        // use this method to  Get one Element StoreByCode from Data base. 
        public static Store GetStoreByCode(string Store_AccountCode)
        {
            var db = new AccountingContainer();
            var stors = db.Stores.Where(c => c.Store_Code == Store_AccountCode);
            var localStore = new Store();
            if (stors != null)
            {
                localStore = stors.FirstOrDefault();
                return localStore;
            }
            return null;
        }
        // use this method For Creation new store and get Data from user.
        public static StoreVM CreateVMForCreation()
        {
            var db = new AccountingContainer();
            var StoreVM = new StoreVM();
            int lastCode = (from s in db.Stores
                            select s.Store_ID).ToList().LastOrDefault();
            StoreVM.Store_Code = (lastCode + 1).ToString();
            StoreVM.MyChartOfAccountList = new SelectList(db.ChartOfAccountings.Where(accCode => accCode.Account_Code.StartsWith(StoreParentAccountCode)), "Account_Id", "Account_Name");
            StoreVM.BranchList = new SelectList(db.Branches, "Branch_Id", "Branch_Name");
            StoreVM.EmployeeList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            var EmpListmember = db.Employees.Where(a => !db.StoreEmployees.Any(y => y.Employee_Id == a.Emp_Id)).ToList();
            StoreVM.EmPloyeeMemberList = new SelectList(EmpListmember, "Emp_Id", "Emp_Name");
            return StoreVM;
        }
        // use this method to  Create New Store and save it in DataBase.
        public static int CreateStore(StoreVM storeVM)
        {
            var db = new AccountingContainer();
            var _store = new Store();
            ChartOfAccountingVM accountVM = new ChartOfAccountingVM();
            accountVM.ChartOfAccountingAccount_Id = 32;
            accountVM.Account_Name = storeVM.Store_Name;
            accountVM.Account_Curenncy = 1;
            _store.Store_Code = ChartOfAccounting.CreateAccount(accountVM);

            _store.Store_Name = storeVM.Store_Name;
            _store.Store_Address = storeVM.Store_Address;
            var thisemployee = db.Employees.Find(storeVM.SelectedEmployeeId);
            if (thisemployee != null)
            {
                _store.Emp_Id = thisemployee.Emp_Id;
            }
            else
            {
                _store.Emp_Id = 0;
            }
            var thisBransh = db.Branches.Find(storeVM.SelectedBranshId);
            if (thisBransh != null)
            {
                _store.Branch_Id = thisBransh.Branch_Id;
            }
            else
            {
                _store.Branch_Id = 0;
            }
            _store.StoreEmployees= new List<StoreEmployee>();
            if (storeVM.EmployeesMember!=null) {
                foreach (var item in storeVM.EmployeesMember.ToList())
                {
                    var empstore = new StoreEmployee();
                    empstore.Employee_Id = Convert.ToInt32(item);

                    _store.StoreEmployees.Add(empstore);


                }
            }
         

            


            db.Stores.Add(_store);
            db.SaveChanges();
            var AllProductServices = db.Products.Where(a => a.Prod_Type == Prod_Type.خدمة || a.Prod_Type == Prod_Type.مجمع).ToList();
            List<ProductBatches> batchList = new List<ProductBatches>();
            foreach (var bat in AllProductServices)
            {
                var batch = new ProductBatches();
                batch.Batch_Date = DateTime.Now;
                batch.SalesPrice = bat.Sale_Price;
                batch.Product_ID = bat.Prod_ID;
                batch.Store_ID = _store.Store_ID;
                batch.Product_Quantity = 1;
                batch.IsActive = true;
                batchList.Add(batch);
            }
            db.ProductBatches.AddRange(batchList);
            db.SaveChanges();
            db.Dispose();
            return _store.Store_ID;
        }
        // use this method to load Details of Data for Store.
        public static StoreVM FillStoreVM(int id)
        {
            var db = new AccountingContainer();
            var store = db.Stores.Find(id);
            var storeVM = new StoreVM();
            var EmpListmember = db.Employees.Where(a => !db.StoreEmployees.Any(y => y.Employee_Id == a.Emp_Id)).ToList();

            //var emp = db.Employees.Where(a => !store.StoreEmployees.Any(y => y.Employee_Id == a.Emp_Id)).ToList();
            var empo = from item in db.Employees.ToList() where store.StoreEmployees.Select(a=>a.Employee_Id).Contains(item.Emp_Id) select item ;
            var emp = empo.ToList();
            emp.AddRange(EmpListmember);
            storeVM.MyChartOfAccountList = new SelectList(db.ChartOfAccountings.Where(accCode => accCode.Account_Code.StartsWith("010105")), "Account_Id", "Account_Name");
            if (store.Branch_Id != null)
            {
                storeVM.SelectedBranshId = (int)store.Branch_Id;
            }


            if (store.Emp_Id == null)
            { storeVM.SelectedEmployeeId = 0; }
            else
            { storeVM.SelectedEmployeeId = (int)store.Emp_Id; }

            storeVM.BranchList = new SelectList(db.Branches, "Branch_Id", "Branch_Name");
            storeVM.EmployeeList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
        
            storeVM.Store_ID = store.Store_ID;
            storeVM.Store_Code = store.Store_Code;
            storeVM.Store_Name = store.Store_Name;
            storeVM.Store_Address = store.Store_Address;
            storeVM.EmPloyeeMemberList = new SelectList(emp, "Emp_Id", "Emp_Name");
            storeVM.EmployeesMember = new List<string>();
            foreach (var item in store.StoreEmployees)
            {
                string empId = Convert.ToString(item.Employee_Id);
                storeVM.EmployeesMember.Add(empId);

            }
            return storeVM;

        }
        // use this method to Edite Store and save it in DataBase.
        public static int EditeStore(StoreVM storeVM)
        {
            var db = new AccountingContainer();
            var _store = db.Stores.Find(storeVM.Store_ID);
            if (_store != null)
            {
                var resultaccount = db.ChartOfAccountings.Find(storeVM.SelectedChartOfAccountId);
                _store.Store_Code = storeVM.Store_Code;
                _store.Store_Name = storeVM.Store_Name;
                _store.Store_Address = storeVM.Store_Address;
                _store.Emp_Id = storeVM.SelectedEmployeeId;
                _store.Branch_Id = storeVM.SelectedBranshId;
                var account = db.ChartOfAccountings.Where(a => a.Account_Code == storeVM.Store_Code).FirstOrDefault();
                account.Account_Name = storeVM.Store_Name;
                if (_store.StoreEmployees==null|| _store.StoreEmployees.ToList().Count() == 0)
                {
                  
                }
                else
                {
                    db.StoreEmployees.RemoveRange(_store.StoreEmployees);
                    db.SaveChanges();
                }
                _store.StoreEmployees = new List<StoreEmployee>();
                if (storeVM.EmployeesMember != null)
                {
                    foreach (var empId in storeVM.EmployeesMember)
                    {
                        var empo = new StoreEmployee();

                        empo.Employee_Id = Convert.ToInt32(empId);
                        _store.StoreEmployees.Add(empo);
                    }
                }
                db.Entry(account).State = EntityState.Modified;
                db.Entry(_store).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return _store.Store_ID;
            }


            return 0;
        }
        // use this method to Delete Store and save DataBase.
        public static string DeleteTransfer(int id)
        {

            var db = new AccountingContainer();
            var store = db.Stores.Find(id);
            if (store.isActive != false)
            {
                store.isActive = false;

            }
            else
            {
                store.isActive = true;
            }

            db.Entry(store).State = EntityState.Modified;
            db.SaveChanges();


            return Resources.ProductDetails.TransferSucces;

        }
        public static string DeleteStore(int id)
        {
            var db = new AccountingContainer();
            var str = db.Stores.Find(id);
            var account = db.ChartOfAccountings.Where(a => a.Account_Code == str.Store_Code).FirstOrDefault();
            var gl = db.GLDetails.Where(a => a.Account_Id == account.Account_Id).FirstOrDefault();

            if (str != null && gl == null)
            {
                db.ChartOfAccountings.Remove(account);
                db.Stores.Remove(str);
                db.SaveChanges();
                db.Dispose();
                return "تم حذف المخزن بنجاح";
            }
            return "لا يمكنك حذف المخزن لتواجده فى بعض الحسابات";
        }

        // Gets the Products available in a store as json array
        public static List<dynamic> getStoreBatches(int storeId)
        {
            var db = new AccountingContainer();
            var store = db.Stores.Find(storeId);
            List<dynamic> products = new List<dynamic>();

            var productBatches = store.ProductBatches;

            foreach (var batch in productBatches)
            {
                if (batch.Product_Quantity == 0)
                    continue;

                var product = new
                {
                    id = batch.Batch_ID,
                    text = batch.Product.Prod_Name + "\t\t\t" + batch.Product_Cost.ToString(),
                    quantity = batch.Product_Quantity
                };
                products.Add(product);
            }

            return products;
        }

        // Gets the Products available in a store as JArray
        //public static JArray getStoreProducts(int storeId)
        //{
        //    var db = new AccountingContainer();
        //    var store = db.Stores.Find(storeId);
        //    JArray productsJArr = new JArray();

        //    foreach (var distinctBatch in store.ProductBatches.Distinct(new Models.ProductBatches.ProductComparer()))
        //    {
        //        var batches = store.ProductBatches.Where(batch => batch.Product_ID == distinctBatch.Product_ID);
        //        var Product_Quantity = batches.Sum(batch => batch.Product_Quantity);
        //        var Batch_Cost = batches.FirstOrDefault().Batch_Cost;

        //        if (Product_Quantity == 0)
        //        {
        //            return productsJArr;
        //        }
        //        var Product_Avg_Cost = decimal.Round(batches.Sum(batch => batch.Product_Cost * batch.Product_Quantity) / Product_Quantity, 3, MidpointRounding.ToEven);
        //        JObject productJObject = JObject.FromObject(new
        //        {
        //            distinctBatch.Product_ID,
        //            distinctBatch.Product.Prod_Name,
        //            Product_Quantity,
        //            Product_Avg_Cost,
        //            Batch_Cost


        //        });
        //        productsJArr.Add(productJObject);
        //    }
        //    return productsJArr;
        //}


        /// <summary>
        /// get all Product that has quantity in that store
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns>List<ProductBatches></returns>
        public static List<ProductBatches> getStoreProducts(int storeId)
        {
            var db = new AccountingContainer();
            var store = db.Stores.Find(storeId);
            List<ProductBatches> productsJArr = new List<ProductBatches>();
            int counter = 0;
            foreach (var distinctBatch in store.ProductBatches.OrderBy(a=>a.Product_ID).Where(a=>a.IsActive!=false).Distinct(new Models.ProductBatches.ProductComparer()))
            {
                ProductBatches productJObject = new ProductBatches();

                var batches = store.ProductBatches.Where(batch => batch.Product_ID == distinctBatch.Product_ID);
                var Product_Quantity = batches.Sum(batch => batch.Product_Quantity);
                var Batch_Cost = batches.FirstOrDefault().Batch_Cost;

                if (Product_Quantity == 0)
                {

                    productJObject.Batch_ID = distinctBatch.Batch_ID;
                    productJObject.Product_ID = distinctBatch.Product_ID;
                    productJObject.Product_Quantity = Product_Quantity;
                    productJObject.Product_Cost = 0;
                    productJObject.Batch_Cost = Batch_Cost;
                }
                else
                {
                    var Product_Avg_Cost = batches.Sum(batch => batch.Product_Cost * batch.Product_Quantity) / Product_Quantity;

                    productJObject.Batch_ID = distinctBatch.Batch_ID;
                    productJObject.Product_ID = distinctBatch.Product_ID;
                    productJObject.Product_Quantity = Product_Quantity;

                    productJObject.Batch_Cost = Batch_Cost;
                    productJObject.Product_Cost = Batch_Cost / Product_Quantity;
                }
                if (distinctBatch.SalesPrice == null)
                {
                    productJObject.SalesPrice = distinctBatch.Product.Sale_Price;
                }
                else
                {
                    productJObject.SalesPrice = distinctBatch.SalesPrice;
                }
                productsJArr.Add(productJObject);

                counter++;

            }
            return productsJArr.Where(a => a.Product_Quantity != 0).ToList();
        }








        //this  is  list  of product for Settlement dropdown list 
        public static ProductBatchSelection ListOFProduct(int id)
        {
            var db = new AccountingContainer();
            ProductBatchSelection ps = new ProductBatchSelection();



            var productBatch = db.ProductBatches.Where(a => a.Store_ID == id && a.Product_Quantity != 0).ToList();
            var productMinus = productBatch.Select(a => a.Product_ID).Distinct();
            List<Product> AllProductList = db.Products.ToList();
            List<Product> MinusList = new List<Product>();

            foreach (var item in productMinus)
            {
                Product prod = new Product();
                prod = db.Products.Find(item);
                MinusList.Add(prod);
                AllProductList.Remove(prod);

            }

            ps.productList = new SelectList(MinusList.Where(a => a.Prod_Type == Prod_Type.منتج), "Prod_ID", "Prod_Name");
            ps.secondProductList = new SelectList(AllProductList.Where(a => a.Prod_Type == Prod_Type.منتج && a.Prod_Cost_Price != 0), "Prod_ID", "Prod_Name");


            ps.storeID = id;

            return ps;
        }
        /// <summary>
        /// get the quantity of product in the choseen store by its name and chosen store
        /// </summary>
        /// <param name="prodID"></param>
        /// <param name="storID"></param>
        /// <returns></returns>
        public static ProductBatchSelection PrepareBatchQuantity(int prodID, int storID)
        {
            var prepare = new ProductBatchSelection();
            var db = new AccountingContainer();
            var thisBatch = db.ProductBatches.Where(s => s.Product_ID == prodID && s.Store_ID == storID);
            var thisproduct = db.Products.Where(e => e.Prod_ID == prodID).ToList().FirstOrDefault();
            prepare.Quantity = thisBatch.FirstOrDefault().Product_Quantity;
            prepare.QuantityExact = thisBatch.FirstOrDefault().Product_Quantity;
            prepare.ProductName = thisproduct.Prod_Name;
            prepare.storeID = thisBatch.FirstOrDefault().Store_ID.Value;
            return prepare;
        }
        /// <summary>
        /// prepare for adding to list of Settlement
        /// </summary>
        /// <param name="prodID"></param>
        /// <param name="storID"></param>
        /// <returns></returns>
        public static ProductBatchSelection PrepareProducty(int prodID, int storID)
        {
            var prepare = new ProductBatchSelection();
            var db = new AccountingContainer();
            var thisProduct = db.Products.Find(prodID);
            prepare.Quantity = 0;
            prepare.QuantityExact = 0;
            prepare.ProductName = thisProduct.Prod_Name;
            prepare.storeID = storID;
            prepare.Product_Id = prodID;
            return prepare;
        }
        /// <summary>
        /// add to list in settlement 
        /// </summary>
        /// <param name="PPs"></param>
        /// <returns>ProductBatchesTemp</returns>
        public static ProductBatchesTemp AddInTemp(ProductBatchSelection PPs)
        {
            var db = new AccountingContainer();
            ProductBatchesTemp PPT = new ProductBatchesTemp();
            PPT.Product_Quantity = PPs.Quantity;
            PPT.Product_ID = PPs.Product_Id;
            PPT.Store_ID = PPs.storeID;
            if (PPs.pro_ID != 0)
            {
                PPT.Product_ID = PPs.pro_ID;
            }

            db.ProductBatchesTemps.Add(PPT);
            try
            { db.SaveChanges(); }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        var x = validationError.PropertyName;
                        var y = validationError.ErrorMessage;
                    }
                }
            }

            return PPT;
        }
        /// <summary>
        /// delete from list for Settlement by its batchid and productid
        /// </summary>
        /// <param name="BatchID"></param>
        /// <param name="ProductID"></param>
        /// <returns>string</returns>
        public static string DeleteFromTemp(int BatchID, int ProductID)
        {
            var db = new AccountingContainer();
            var thisBatchTemp = db.ProductBatchesTemps.Where(e => e.Batch_ID == BatchID && e.Product_ID == ProductID).ToList().FirstOrDefault();
            db.ProductBatchesTemps.Remove(thisBatchTemp);
            db.SaveChanges();
            return "تم ازالة الصنف";
        }
        /// <summary>
        /// get list for product batch
        /// </summary>
        /// <returns></returns>

        public static List<ProductBatchesTemp> batchList()
        {
            var db = new AccountingContainer();
            var allTemp = db.ProductBatchesTemps.ToList();
            return allTemp;
        }

        /// <summary>
        /// create old Settlement in gl  that method is canceled
        /// </summary>
        /// <returns>string</returns>
        public static string addTemptoBatchList()
        {
            var db = new AccountingContainer();
            var temp = db.ProductBatchesTemps.ToList();
            decimal totalofall = 0;
            List<ProductBatchesTemp> pss = new List<ProductBatchesTemp>();
            foreach (var item in temp)
            {
                decimal tax = 0;
                decimal total = 0;
                decimal totaltax = 0;
                var thisbatch = db.ProductBatches.Where(s => s.Store_ID == item.Store_ID && s.Product_ID == item.Product_ID).ToList().FirstOrDefault();
                if (thisbatch == null && item.Product_Quantity != 0)
                {
                    var thispor = db.Products.Find(item.Product_ID);

                    var taxpercentage = db.Taxes.Find(thispor.Tax_Id);
                    //tax = (decimal)(thispor.Prod_Cost_Price * (taxpercentage.Tax_Percentage / 100));
                    //totaltax = tax * item.Product_Quantity;
                    var newBatch = new ProductBatches();
                    newBatch.Product_ID = (int)item.Product_ID;
                    newBatch.Store_ID = (int)item.Store_ID;

                    newBatch.Product_Quantity = (int)item.Product_Quantity;
                    newBatch.Batch_Date = DateTime.Now;
                    if (thispor.Prod_Cost_Price == null)
                    {
                        thispor.Prod_Cost_Price = 0;
                    }
                    newBatch.Product_Cost = (decimal)thispor.Prod_Cost_Price;
                    newBatch.Batch_Cost = (decimal)(thispor.Prod_Cost_Price * item.Product_Quantity);
                    db.ProductBatches.Add(newBatch);

                    GLMaster glHeader = new GLMaster();

                    glHeader.GL_Date = DateTime.Now; ;
                    glHeader.GL_Desc = "تسوية مخزن";

                    glHeader.Company_Id = 1;
                    glHeader.GLType_Id = "زيادة";
                    glHeader.Currency_Id = "الريال السعودي";
                    glHeader.Auto = true;
                    // Initialize GLState to Pending when first created
                    glHeader.GLState = TransactionState.Pending;
                    glHeader.GLCode = thispor.Prod_Code;
                    // glHeader.Account_Child_ID = invoice.Supplier_Id;
                    GLDetail storeDep = new GLDetail();
                    GLDetail GainANdLose = new GLDetail();
                    var ChartOfAccounInventory = db.Stores.Find(item.Store_ID).Store_Code;
                    var storeChart = db.ChartOfAccountings.Where(a => a.Account_Code == ChartOfAccounInventory).FirstOrDefault();
                    storeDep.Account_Id = storeChart.Account_Id;
                    storeDep.Account_Name = storeChart.Account_Name;
                    storeDep.GLDetail_Debit = (decimal)(thispor.Prod_Cost_Price * item.Product_Quantity);
                    storeDep.GLDetail_Credit = 0;
                    storeDep.Bu_Id = 0;
                    storeDep.SubLedger_Id = 0;
                    storeDep.Asset_Id = 0;
                    storeDep.Account_Child_ID = 0;
                    var GainANdLoseaccount = db.ChartOfAccountings.Find(151);
                    GainANdLose.Account_Id = GainANdLoseaccount.Account_Id;
                    GainANdLose.Account_Name = GainANdLoseaccount.Account_Name;
                    GainANdLose.GLDetail_Debit = 0;
                    GainANdLose.GLDetail_Credit = (decimal)(thispor.Prod_Cost_Price * item.Product_Quantity);
                    GainANdLose.Bu_Id = 0;
                    GainANdLose.SubLedger_Id = 0;
                    GainANdLose.Asset_Id = 0;
                    GainANdLose.Account_Child_ID = 0;
                    glHeader.GLDetails.Add(storeDep);
                    glHeader.GLDetails.Add(GainANdLose);
                    db.GLMasters.Add(glHeader);
                    GLMaster.GetBalanceForAccount(glHeader);
                    db.ProductBatchesTemps.Remove(item);
                    db.SaveChanges();

                }
                else
                {
                    if (item.Product_Quantity > thisbatch.Product_Quantity)
                    {
                        var thisprod = db.Products.Find(item.Product_ID);
                        var taxpercentage = db.Taxes.Find(thisprod.Tax_Id);
                        if (taxpercentage == null)
                        {
                            taxpercentage = db.Taxes.FirstOrDefault();
                        }
                        total = item.Product_Quantity * thisbatch.Product_Cost;
                        if (thisprod.Prod_Cost_Price == null)
                        {
                            thisprod.Prod_Cost_Price = 0;
                        }
                        tax = (decimal)(thisprod.Prod_Cost_Price * (taxpercentage.Tax_Percentage / 100));
                        totaltax = tax * item.Product_Quantity;

                        var quantityDif = item.Product_Quantity - thisbatch.Product_Quantity;
                        thisbatch.Product_Quantity = item.Product_Quantity;
                        thisbatch.Batch_Cost = (thisbatch.Product_Cost * thisbatch.Product_Quantity);

                        totalofall += thisbatch.Batch_Cost;
                        db.Entry(thisbatch).State = EntityState.Modified;
                        // db.ProductBatchesTemps.RemoveRange(temp);
                        db.ProductBatchesTemps.Remove(item);
                        GLMaster glHeader = new GLMaster();

                        glHeader.GL_Date = DateTime.Now; ;
                        glHeader.GL_Desc = "تسوية مخزن";

                        glHeader.Company_Id = 1;
                        glHeader.GLType_Id = "زيادة";
                        glHeader.Currency_Id = "الريال السعودي";
                        glHeader.Auto = true;
                        // Initialize GLState to Pending when first created
                        glHeader.GLState = TransactionState.Pending;
                        glHeader.GLCode = thisprod.Prod_Code;
                        // glHeader.Account_Child_ID = invoice.Supplier_Id;
                        GLDetail storeDep = new GLDetail();
                        GLDetail GainANdLose = new GLDetail();
                        var ChartOfAccounInventory = db.Stores.Find(item.Store_ID).Store_Code;
                        var storeChart = db.ChartOfAccountings.Where(a => a.Account_Code == ChartOfAccounInventory).FirstOrDefault();
                        storeDep.Account_Id = storeChart.Account_Id;
                        storeDep.Account_Name = storeChart.Account_Name;
                        storeDep.GLDetail_Debit = (decimal)(quantityDif * thisbatch.Product_Cost);
                        storeDep.GLDetail_Credit = 0;
                        storeDep.Bu_Id = 0;
                        storeDep.SubLedger_Id = 0;
                        storeDep.Asset_Id = 0;
                        storeDep.Account_Child_ID = 0;
                        var GainANdLoseaccount = db.ChartOfAccountings.Find(151);
                        GainANdLose.Account_Id = GainANdLoseaccount.Account_Id;
                        GainANdLose.Account_Name = GainANdLoseaccount.Account_Name;
                        GainANdLose.GLDetail_Debit = 0;
                        GainANdLose.GLDetail_Credit = (decimal)(quantityDif * thisbatch.Product_Cost);
                        GainANdLose.Bu_Id = 0;
                        GainANdLose.SubLedger_Id = 0;
                        GainANdLose.Asset_Id = 0;
                        GainANdLose.Account_Child_ID = 0;
                        glHeader.GLDetails.Add(storeDep);
                        glHeader.GLDetails.Add(GainANdLose);
                        db.GLMasters.Add(glHeader);
                        GLMaster.GetBalanceForAccount(glHeader);
                        db.SaveChanges();
                    }
                    else if (item.Product_Quantity == null)
                    {
                        db.ProductBatchesTemps.RemoveRange(temp);
                        db.SaveChanges();
                        return "من فضلك ادخل كمية";
                    }
                    else
                    {
                        var thisprod = db.Products.Find(item.Product_ID);
                        var taxpercentage = db.Taxes.Find(thisprod.Tax_Id);
                        if (taxpercentage == null)
                        {
                            taxpercentage = db.Taxes.FirstOrDefault();
                        }
                        total = item.Product_Quantity * thisbatch.Product_Cost;
                        tax = (decimal)(thisprod.Prod_Cost_Price * (taxpercentage.Tax_Percentage / 100));
                        totaltax = tax * item.Product_Quantity;
                        var QuantityDif = thisbatch.Product_Quantity - item.Product_Quantity;

                        thisbatch.Product_Quantity = item.Product_Quantity;
                        thisbatch.Batch_Cost = (thisbatch.Product_Cost * item.Product_Quantity);
                        totalofall += thisbatch.Batch_Cost;
                        db.Entry(thisbatch).State = EntityState.Modified;
                        db.ProductBatchesTemps.Remove(item);
                        GLMaster glHeader = new GLMaster();

                        glHeader.GL_Date = DateTime.Now; ;
                        glHeader.GL_Desc = "تسوية مخزن";

                        glHeader.Company_Id = 1;
                        glHeader.GLType_Id = "نقصان";
                        glHeader.Currency_Id = "الريال السعودي";
                        glHeader.Auto = true;
                        // Initialize GLState to Pending when first created
                        glHeader.GLState = TransactionState.Pending;
                        glHeader.GLCode = thisprod.Prod_Code;
                        // glHeader.Account_Child_ID = invoice.Supplier_Id;
                        GLDetail storeDep = new GLDetail();
                        GLDetail GainANdLose = new GLDetail();
                        var ChartOfAccounInventory = db.Stores.Find(item.Store_ID).Store_Code;
                        var storeChart = db.ChartOfAccountings.Where(a => a.Account_Code == ChartOfAccounInventory).FirstOrDefault();
                        storeDep.Account_Id = storeChart.Account_Id;
                        storeDep.Account_Name = storeChart.Account_Name;
                        storeDep.GLDetail_Debit = 0;
                        storeDep.GLDetail_Credit = (decimal)(QuantityDif * thisbatch.Product_Cost);
                        storeDep.Bu_Id = 0;
                        storeDep.SubLedger_Id = 0;
                        storeDep.Asset_Id = 0;
                        storeDep.Account_Child_ID = 0;
                        var GainANdLoseaccount = db.ChartOfAccountings.Find(151);
                        GainANdLose.Account_Id = GainANdLoseaccount.Account_Id;
                        GainANdLose.Account_Name = GainANdLoseaccount.Account_Name;
                        GainANdLose.GLDetail_Debit = (decimal)(QuantityDif * thisbatch.Product_Cost);
                        GainANdLose.GLDetail_Credit = 0;
                        GainANdLose.Bu_Id = 0;
                        GainANdLose.SubLedger_Id = 0;
                        GainANdLose.Asset_Id = 0;
                        GainANdLose.Account_Child_ID = 0;
                        glHeader.GLDetails.Add(storeDep);
                        glHeader.GLDetails.Add(GainANdLose);
                        db.GLMasters.Add(glHeader);
                        GLMaster.GetBalanceForAccount(glHeader);
                        db.SaveChanges();
                    }

                }
                pss.Add(item);

            }
            // db.ProductBatchesTemps.RemoveRange(temp);
            db.SaveChanges();
            db.Dispose();
            return "تم تسوية المخزن";
        }
        /// <summary>
        /// immport excel file in store and check quantity that make start gl or not
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns>String</returns>
        public static String Import(HttpPostedFileBase excelfile)
        {
            var data = getdataFillter(excelfile);
            List<Store> StoreList = new List<Store>();
            if (data.Item2.Count() == 0)
            {
                foreach (var item in data.Item1)
                {
                    var sto = CreateStoreFromFile(item);
                    StoreList.Add(sto);

                }


                return "تم حفظ الملف بنجاح";

            }
            string combindedString = string.Join(" ,", data.Item2.ToArray());
            return "يوجد اخطاء فى الاسطر التالية" + combindedString;


        }
        /// <summary>
        /// filter Excel file and calculate the error and number of line that have error
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        public static Tuple<List<StoreVM>, List<String>> getdataFillter(HttpPostedFileBase excelfile)
        {
            var StoreList = new List<StoreVM> { };
            var messageList = new List<String> { };
            var db = new AccountingContainer();
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return Tuple.Create(StoreList, messageList);

            }

            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + excelfile.FileName);

                    //if (System.IO.File.Exists(path))
                    //{
                    //    excelfile.SaveAs(path + "1");

                    //    System.IO.File.Delete(path);


                    //}
                    //else
                    //{
                    excelfile.SaveAs(path);
                    //}
                    // read data from excel file

                    Application application = new Application();
                    Workbook workbook = application.Workbooks.Open(path);
                    Worksheet worksheet = workbook.ActiveSheet;
                    Range range = worksheet.UsedRange;
                    for (int row = 3; row <= range.Rows.Count; row++)
                    {
                        StoreVM storeVm = new StoreVM();
                        // cust.Customer_Id = int.Parse(((Range)range.Cells[row, 1]).Text);
                        storeVm.Store_Name = ((Range)range.Cells[row, 1]).Text;
                        if (storeVm.Store_Name == "")
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }






                        storeVm.Store_Address = ((Range)range.Cells[row, 2]).Text;

                        StoreList.Add(storeVm);





                    }
                    workbook.Save();
                    workbook.Close(path);



                    File.Delete(path);

                }
            }
            return Tuple.Create(StoreList, messageList);
        }

        /// <summary>
        /// create store by file that after filtering 
        /// </summary>
        /// <param name="storeVM"></param>
        /// <returns>Store</returns>
        public static Store CreateStoreFromFile(StoreVM storeVM)
        {
            var db = new AccountingContainer();
            var _store = new Store();
            ChartOfAccountingVM accountVM = new ChartOfAccountingVM();
            accountVM.ChartOfAccountingAccount_Id = 32;
            accountVM.Account_Name = storeVM.Store_Name;
            accountVM.Account_Curenncy = 1;
            _store.Store_Code = ChartOfAccounting.CreateAccount(accountVM);

            _store.Store_Name = storeVM.Store_Name;
            _store.Store_Address = storeVM.Store_Address;
            var thisemployee = db.Employees.Find(storeVM.SelectedEmployeeId);
            if (thisemployee != null)
            {
                _store.Emp_Id = thisemployee.Emp_Id;
            }
            else
            {
                _store.Emp_Id = 0;
            }
            var thisBransh = db.Branches.Find(storeVM.SelectedBranshId);
            if (thisBransh != null)
            {
                _store.Branch_Id = thisBransh.Branch_Id;
            }
            else
            {
                _store.Branch_Id = 0;
            }
            db.Stores.Add(_store);
            db.SaveChanges();
            db.Dispose();
            return _store;
        }
        /// <summary>
        /// that adding to store card if the excel have product 
        /// </summary>
        /// <param name="card"></param>

        public static void AddinginstorCard(StoreCard card)
        {

            var db = new AccountingContainer();
            db.StoreCards.Add(card);
            db.SaveChanges();




        }
        /// <summary>
        /// rebuild card of store by the quantity that exist in store
        /// </summary>
        /// <returns>string</returns>
        public static string makeStoreQuantity()
        {

            var db = new AccountingContainer();
            var stores = db.Stores.ToList();
            var storeCard = db.StoreCards.ToList();
            db.StoreCards.RemoveRange(storeCard);
            db.SaveChanges();
            foreach (var item in stores)
            {
                var batchList = db.ProductBatches.Where(a => a.Store_ID == item.Store_ID && a.Product_Quantity != 0).ToList();
                foreach (var detail in batchList)
                {
                    var storecard = new StoreCard();
                    storecard.CostPrice = detail.Product_Cost;
                    storecard.Date = DateTime.Now;
                    storecard.Store_Id = detail.Store_ID.Value;
                    storecard.Quantity = detail.Product_Quantity;
                    storecard.SalePrice = 0;
                    storecard.Product_Id = detail.Product_ID;
                    storecard.IncreaseQuantity = true;
                    storecard.TypeOfDocument = "قيم افتتاحية";
                    storecard.Document_Serial = "Start-001";
                    storecard.TotalCost = detail.Product_Quantity * detail.Product_Cost;
                    storecard.TotalAmount = 0;
                    storecard.Account_Id = 0;
                    storecard.Subledger_Id = 0;
                    Store.AddinginstorCard(storecard);




                }



            }

            return "تمت عمل افتتاحي اصناف";






        }



        /// <summary>
        /// final method that his function to check error if not make saving file with gl is quantity is exist
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        public static String ImportStartbalance(HttpPostedFileBase excelfile)
        {
            var data = getdataFillterForStartBalance(excelfile);
            List<ProductVM> ProductList = new List<ProductVM>();
            if (data.Item2.Count() == 0)
            {

                Accountingsystem.Utilites.OpenBalanceFile.MakeOpenBallanceForGroupOFProductForoneStore(data.Item1.Where(a => a.Product_Quantity > 0).ToList(), data.Item3);





                return "تم حفظ الملف بنجاح";

            }
            string combindedString = string.Join(" ,", data.Item2.ToArray());
            return "يوجد اخطاء فى الاسطر التالية" + combindedString;


        }
        /// <summary>
        /// check product validation that come from excel sheet
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns>Tuple<List<ProductVM>, List<String>,int > </returns>
        public static Tuple<List<ProductVM>, List<String>, int> getdataFillterForStartBalance(HttpPostedFileBase excelfile)
        {
            var productList = new List<ProductVM> { };
            var messageList = new List<String> { };
            int id = 0;
            String storeCode;
            int storeid = 0;
            var db = new AccountingContainer();
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return Tuple.Create(productList, messageList, id);

            }

            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    //string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + excelfile.FileName);
                    var nameOfFile = "OpenProduct" + DateTime.Now.ToBinary() + ".xlsx";
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + nameOfFile);

                    excelfile.SaveAs(path);

                    Application application = new Application();
                    Workbook workbook = application.Workbooks.Open(path);
                    Worksheet worksheet = workbook.ActiveSheet;
                    Range range = worksheet.UsedRange;
                    try
                    {
                        storeCode = ((Range)range.Cells[2, 1]).Text;
                        //storeCode = ((Range)range.Cells[2, 2]).Text;
                        //storeCode = ((Range)range.Cells[2, 3]).Text;
                        //storeCode = ((Range)range.Cells[2, 4]).Text;
                        //storeCode = ((Range)range.Cells[2, 5]).Text;
                        if (storeCode == "")
                        {
                            messageList.Add(3.ToString());

                        }
                        storeid = db.Stores.Where(a => a.Store_Code == storeCode).FirstOrDefault().Store_ID;
                    }
                    catch
                    {

                    }
                    for (int row = 5; row <= range.Rows.Count; row++)
                    {
                        ProductVM provm = new ProductVM();

                        provm.Prod_Code = ((Range)range.Cells[row, 2]).Text;
                        if (provm.Prod_Code == "")
                        {
                            var msgo = row + 1;
                            messageList.Add(msgo.ToString());
                            continue;
                        }

                        try
                        {

                            provm.Prod_Code = ((Range)range.Cells[row, 2]).Text;
                            provm.Prod_Code = db.Products.Where(a => a.Prod_Code == provm.Prod_Code).FirstOrDefault().Prod_Code;

                        }
                        catch
                        {
                            var msgo = row + 1;
                            messageList.Add(msgo.ToString());
                            continue;
                        }


                        provm.Prod_Name = ((Range)range.Cells[row, 3]).Text;
                        if (provm.Prod_Name == "")
                        {
                            var msgo = row + 1;
                            messageList.Add(msgo.ToString());
                            continue;
                        }




                        try
                        {
                            var a = ((Range)range.Cells[row, 4]).Text;
                            if (((Range)range.Cells[row, 4]).Text == "")
                            {
                                provm.Product_Quantity = 0;
                            }
                            else
                            {
                                provm.Product_Quantity = Convert.ToDecimal(((Range)range.Cells[row, 4]).Text);
                            }




                        }
                        catch
                        {
                            var msgo = row + 1;
                            messageList.Add(msgo.ToString());
                            continue;
                        }


                        productList.Add(provm);




                    }
                    workbook.Save();
                    workbook.Close(path);


                    //  File.Delete(path);

                }
            }
            return Tuple.Create(productList, messageList, storeid);
        }
        /// <summary>
        /// make store card like store quantity by adding in store card the diferrent betwwen them
        /// </summary>
        /// <returns></returns>
        public static int EditStoreCardLikeStore()
        {

            var db = new AccountingContainer();
            if (db.StoreCards.Where(a => a.Document_Serial == "01").FirstOrDefault() != null)
            {
                var totalplusa = db.StoreCards.Where(a => a.IncreaseQuantity == true).Sum(a => a.TotalCost);
                var totalminusa = db.StoreCards.Where(a => a.IncreaseQuantity != true).Sum(a => a.TotalCost);
                totalminusa = totalplusa - totalminusa;
                var batchcosta = db.ProductBatches.Sum(a => a.Batch_Cost);
                //return 0;
            }
            var batches = db.ProductBatches.Where(a => a.Store_ID != null).ToList();

            var StoreCardList = new List<StoreCard> { };
            foreach (var batch in batches)
            {
                var Cards = db.StoreCards.Where(a => a.Store_Id == batch.Store_ID && a.Product_Id == batch.Product_ID);

                //if (Cards.FirstOrDefault()==null)
                //{

                //}




                var totalCostPlus = Cards.Where(a => a.IncreaseQuantity == true).Sum(a => a.TotalCost);
                if (totalCostPlus == null)
                {
                    totalCostPlus = 0;
                }
                var totalCostMinus = Cards.Where(a => a.IncreaseQuantity == false).Sum(a => a.TotalCost);
                if (totalCostMinus == null)
                {
                    totalCostMinus = 0;
                }

                decimal totalquantityplus = 0;



                decimal totalquatityminus = 0;
                try
                {
                    totalquatityminus = Cards.Where(a => a.IncreaseQuantity == false).Sum(a => a.Quantity);
                }
                catch
                {
                    totalquatityminus = 0;

                }
                try
                {
                    totalquantityplus = Cards.Where(a => a.IncreaseQuantity == true).Sum(a => a.Quantity);
                }
                catch
                {
                    totalquantityplus = 0;

                }


                var finaCost = totalCostPlus - totalCostMinus;
                var finalquantity = totalquantityplus - totalquatityminus;
                //if (Math.Round( batch.Product_Quantity,5,MidpointRounding.AwayFromZero)==Math.Round( finalquantity,5,MidpointRounding.AwayFromZero) &&Math.Round( batch.Batch_Cost,5,MidpointRounding.AwayFromZero)==Math.Round(finaCost.Value,5,MidpointRounding.AwayFromZero))
                if (batch.Product_Quantity == finalquantity && batch.Batch_Cost == finaCost.Value)
                {
                    //db.ProductBatches.Remove(batch);
                    //db.SaveChanges();
                }
                else
                {
                    if (batch.Product_Quantity == 0)
                    {
                        batch.Batch_Cost = 0;
                    }

                    var storecard = new StoreCard();


                    storecard.Store_Id = batch.Store_ID.Value;

                    storecard.SalePrice = 0;
                    storecard.Product_Id = batch.Product_ID;


                    storecard.Document_Serial = "01";

                    storecard.TotalAmount = 0;
                    storecard.Account_Id = 0;
                    storecard.Subledger_Id = 0;
                    if (batch.Product_Quantity > finalquantity)
                    {
                        storecard.IncreaseQuantity = true;


                        storecard.Quantity = batch.Product_Quantity - finalquantity;


                    }
                    else
                    {
                        storecard.IncreaseQuantity = false;

                        storecard.Quantity = finalquantity - batch.Product_Quantity;


                    }
                    if (finaCost == batch.Batch_Cost)
                    {

                        storecard.TotalCost = 0;
                        storecard.CostPrice = batch.Product_Cost;

                    }
                    else if (finaCost > batch.Batch_Cost)
                    {
                        storecard.IncreaseQuantity = false;
                        storecard.TotalCost = finaCost - batch.Batch_Cost;
                        try
                        {
                            storecard.CostPrice = storecard.TotalCost.Value / storecard.Quantity;
                        }
                        catch
                        {
                            storecard.CostPrice = 0;
                        }
                    }
                    else
                    {

                        storecard.IncreaseQuantity = true;
                        storecard.TotalCost = batch.Batch_Cost - finaCost;
                        try
                        {
                            storecard.CostPrice = storecard.TotalCost.Value / storecard.Quantity;
                        }
                        catch
                        {
                            storecard.CostPrice = 0;
                        }
                    }
                    if (storecard.IncreaseQuantity == true)
                    {
                        try
                        {
                            var lastdate = db.StoreCards.Where(a => a.Product_Id == storecard.Product_Id && a.Store_Id == storecard.Store_Id).OrderBy(a => a.Date).FirstOrDefault().Date;
                            storecard.Date = lastdate.AddDays(-1);
                        }
                        catch
                        {
                            storecard.Date = new DateTime(2018, 12, 30);
                        }
                        storecard.TypeOfDocument = "رصيد افتتاحي";

                    }
                    else
                    {
                        storecard.TypeOfDocument = "تسويه";
                        storecard.Date = DateTime.Now;
                    }
                    StoreCardList.Add(storecard);








                }



            }


            db.StoreCards.AddRange(StoreCardList);
            var k = StoreCardList.Where(z => z.Quantity > 1 / 5).ToList();
            db.SaveChanges();
            var totalplus = db.StoreCards.Where(a => a.IncreaseQuantity == true).Sum(a => a.TotalCost);
            var totalminus = db.StoreCards.Where(a => a.IncreaseQuantity == false).Sum(a => a.TotalCost);
            totalminus = totalplus - totalminus;
            var batchcost = db.ProductBatches.Sum(a => a.Batch_Cost);

            return 0;

        }


        public static int EditProductCardLikeStore(int store, int product)
        {

            var db = new AccountingContainer();
           
            var batches = db.ProductBatches.Where(a => a.Store_ID != null).ToList();


            var Cards = db.StoreCards.Where(a => a.Store_Id == store && a.Product_Id == product);






            var totalCostPlus = Cards.Where(a => a.IncreaseQuantity == true).Sum(a => a.TotalCost);
            if (totalCostPlus == null)
            {
                totalCostPlus = 0;
            }
            var totalCostMinus = Cards.Where(a => a.IncreaseQuantity == false).Sum(a => a.TotalCost);
            if (totalCostMinus == null)
            {
                totalCostMinus = 0;
            }

            decimal totalquantityplus = 0;



            decimal totalquatityminus = 0;
            try
            {
                totalquatityminus = Cards.Where(a => a.IncreaseQuantity == false).Sum(a => a.Quantity);
            }
            catch
            {
                totalquatityminus = 0;

            }
            try
            {
                totalquantityplus = Cards.Where(a => a.IncreaseQuantity == true).Sum(a => a.Quantity);
            }
            catch
            {
                totalquantityplus = 0;

            }


            var finaCost = totalCostPlus - totalCostMinus;
            var finalquantity = totalquantityplus - totalquatityminus;
            var batch = db.ProductBatches.Where(a => a.Product_ID == product && a.Store_ID == store).FirstOrDefault();
            if (batch == null)
            {

            }
            //if (Math.Round( batch.Product_Quantity,5,MidpointRounding.AwayFromZero)==Math.Round( finalquantity,5,MidpointRounding.AwayFromZero) &&Math.Round( batch.Batch_Cost,5,MidpointRounding.AwayFromZero)==Math.Round(finaCost.Value,5,MidpointRounding.AwayFromZero))
            else
            {
                if (batch.Product_Quantity == finalquantity && batch.Batch_Cost == finaCost.Value)
                {
                    //db.ProductBatches.Remove(batch);
                    //db.SaveChanges();
                }

                else
                {
                    if (db.StoreCards.Where(a => a.Document_Serial == "01" && a.Store_Id == store && a.Product_Id == product).FirstOrDefault() != null)
                    {
                        db.StoreCards.RemoveRange(db.StoreCards.Where(a => a.Document_Serial == "01" && a.Store_Id == store && a.Product_Id == product));
                        db.SaveChanges();

                    }
                    if (batch.Product_Quantity == 0)
                    {
                        batch.Batch_Cost = 0;
                    }

                    var storecard = new StoreCard();


                    storecard.Store_Id = batch.Store_ID.Value;

                    storecard.SalePrice = 0;
                    storecard.Product_Id = batch.Product_ID;


                    storecard.Document_Serial = "01";

                    storecard.TotalAmount = 0;
                    storecard.Account_Id = 0;
                    storecard.Subledger_Id = 0;
                    if (batch.Product_Quantity > finalquantity)
                    {
                        storecard.IncreaseQuantity = true;


                        storecard.Quantity = batch.Product_Quantity - finalquantity;


                    }
                    else
                    {
                        storecard.IncreaseQuantity = false;

                        storecard.Quantity = finalquantity - batch.Product_Quantity;


                    }
                    if (finaCost == batch.Batch_Cost)
                    {

                        storecard.TotalCost = 0;
                        storecard.CostPrice = batch.Product_Cost;

                    }
                    else if (finaCost > batch.Batch_Cost)
                    {
                        storecard.IncreaseQuantity = false;
                        storecard.TotalCost = finaCost - batch.Batch_Cost;
                        try
                        {
                            storecard.CostPrice = storecard.TotalCost.Value / storecard.Quantity;
                        }
                        catch
                        {
                            storecard.CostPrice = 0;
                        }
                    }
                    else
                    {

                        storecard.IncreaseQuantity = true;
                        storecard.TotalCost = batch.Batch_Cost - finaCost;
                        try
                        {
                            storecard.CostPrice = storecard.TotalCost.Value / storecard.Quantity;
                        }
                        catch
                        {
                            storecard.CostPrice = 0;
                        }
                    }
                    if (storecard.IncreaseQuantity == true)
                    {
                        try
                        {
                            var lastdate = db.StoreCards.Where(a => a.Product_Id == storecard.Product_Id && a.Store_Id == storecard.Store_Id).OrderBy(a => a.Date).FirstOrDefault().Date;
                            storecard.Date = lastdate.AddDays(-1);
                        }
                        catch
                        {
                            storecard.Date = new DateTime(2018, 12, 30);
                        }
                        storecard.TypeOfDocument = "رصيد افتتاحي";

                    }
                    else
                    {
                        storecard.TypeOfDocument = "تسوية";
                        try
                        {
                            var lastdate = db.StoreCards.Where(a => a.Product_Id == storecard.Product_Id && a.Store_Id == storecard.Store_Id).OrderByDescending(a => a.Date).FirstOrDefault().Date;
                            storecard.Date = lastdate.AddDays(1);
                        }
                        catch
                        {
                            storecard.Date = new DateTime(2018, 12, 30);
                        }
                    }
                    // StoreCardList.Add(storecard);
                    db.StoreCards.Add(storecard);



                    db.SaveChanges();
                    //var totalplus = db.StoreCards.Where(a => a.IncreaseQuantity == true).Sum(a => a.TotalCost);
                    //var totalminus = db.StoreCards.Where(a => a.IncreaseQuantity == false).Sum(a => a.TotalCost);
                    //totalminus = totalplus - totalminus;
                    //var batchcost = db.ProductBatches.Sum(a => a.Batch_Cost);

                    return 0;

                }
            }
            return 0;


        }
        //public static void MakeProductBatch(List<ProductVM> ProductList,int storeid)
        //{
        //    var db = new AccountingContainer();

        //    decimal totalcost = 0;
        //    var batchList = new List<ProductBatches> { };

        //    foreach (var item in ProductList)
        //    {
        //        var ProductBatch = new ProductBatches();


        //        var poduct = db.Products.Where(a => a.Prod_Code == item.Prod_Code).FirstOrDefault();
        //        ProductBatch.Store_ID = storeid;
        //        ProductBatch.Product_ID = poduct.Prod_ID;
        //        ProductBatch.Product_Quantity = item.Product_Quantity;
        //        ProductBatch.Product_Cost = poduct.Prod_Cost_Price.Value;
        //        ProductBatch.Batch_Cost = ProductBatch.Product_Quantity * ProductBatch.Product_Cost;
        //        totalcost += ProductBatch.Batch_Cost;
        //        batchList.Add(ProductBatch);





        //    }
        //    db.ProductBatches.AddRange(batchList);
        //    db.SaveChanges();




        //}

     
        public static int MakeProductBatchLikeStoreCard()
        {
            var db = new AccountingContainer();
            var stellment = db.StoreCards.Where(a => a.Date > new DateTime(2021, 1, 1) && a.Document_Serial == "01").ToList();
            if (stellment.Count > 0)
            {
                db.StoreCards.RemoveRange(stellment);
                db.SaveChanges();
            }
            var cardquantity = db.StoreCards.ToList();
            var Batches = db.ProductBatches.Where(a=>a.Product.Prod_Type==Prod_Type.منتج||a.Product.Prod_Type==Prod_Type.اولي).ToList();
          
                var z= cardquantity.Where(a => a.IncreaseQuantity == true).Select(a => a.Quantity).Sum() - cardquantity.Where(a => a.IncreaseQuantity == false).Select(a => a.Quantity).ToList().Sum();
                var b= Batches.Select(a => a.Product_Quantity).ToList().Sum();

            var h = cardquantity.Where(a => a.IncreaseQuantity == true).Select(a => a.TotalCost).Sum() - cardquantity.Where(a => a.IncreaseQuantity == false).Select(a => a.TotalCost).ToList().Sum();
            var f = Batches.Select(a => a.Batch_Cost).ToList().Sum();

            if (cardquantity.Where(a => a.IncreaseQuantity == true).Select(a => a.Quantity).Sum() 
                    - cardquantity.Where(a => a.IncreaseQuantity == false).Select(a => a.Quantity).Sum() 
                    == Batches.Select(a => a.Product_Quantity).Sum())
                {
                    return 0;

                }
                else
                {
                    foreach(var batch in Batches)
                    {
                    if (batch.Product.Prod_Code == "PRO-028")
                    {
                        var yy = 66;
                    }
                        var a = getQuantityInStoreCard(batch.Batch_ID);

                    }
                }

            
            
            return 0;

        }
        public static int getQuantityInStoreCard(int batchID)
        {
            using (var db = new AccountingContainer())
            {
                var batch = db.ProductBatches.Find(batchID);

                var Card = db.StoreCards.Where(a => a.Product_Id == batch.Product_ID && a.Store_Id == batch.Store_ID).ToList();
                var aaa = Card.Where(a => a.IncreaseQuantity == true).Select(a => a.Quantity).ToList().Sum() - Card.Where(a => a.IncreaseQuantity == false).Select(a => a.Quantity).ToList().Sum();
                var bbb = Card.Where(a => a.IncreaseQuantity == true).Select(a => a.TotalCost.Value).ToList().Sum() - Card.Where(a => a.IncreaseQuantity == false).Select(a => a.TotalCost.Value).ToList().Sum();
                if(aaa== batch.Product_Quantity&& Math.Round( bbb,2,MidpointRounding.AwayFromZero) == Math.Round(batch.Batch_Cost, 2, MidpointRounding.AwayFromZero))
                {

                }
                else
                {
                    var product = batch.Product.Bar_Code;
                    if (Card.Where(a => a.IncreaseQuantity == false).Count() > 0)
                    {
                        aaa = Card.Where(a => a.IncreaseQuantity == true).Select(a => a.Quantity).Sum() - Card.Where(a => a.IncreaseQuantity == false).Select(a => a.Quantity).Sum();
                        bbb = Card.Where(a => a.IncreaseQuantity == true).Select(a => a.TotalCost.Value).Sum() - Card.Where(a => a.IncreaseQuantity == false).Select(a => a.TotalCost.Value).Sum();
                        batch.Product_Quantity = Card.Where(a => a.IncreaseQuantity == true).Select(a => a.Quantity).Sum() - Card.Where(a => a.IncreaseQuantity == false).Select(a => a.Quantity).Sum();
                        batch.Batch_Cost = Card.Where(a => a.IncreaseQuantity == true).Select(a => a.TotalCost.Value).Sum() - Card.Where(a => a.IncreaseQuantity == false).Select(a => a.TotalCost.Value).Sum();
                    }
                    else
                    {
                        batch.Product_Quantity = Card.Where(a => a.IncreaseQuantity == true).Select(a => a.Quantity).ToList().Sum();
                        batch.Batch_Cost = Card.Where(a => a.IncreaseQuantity == true).Select(a => a.TotalCost.Value).ToList().Sum();
                    }
                    if (batch.Product_Quantity == 0)
                    {
                        batch.Product_Cost = 0;
                    }
                    else
                    {
                        batch.Product_Cost = batch.Batch_Cost / batch.Product_Quantity;
                    }

                    db.Entry(batch).State = EntityState.Modified;


                    db.SaveChanges();
                }


            }
            return 00;


        }
        public static void Editbatch(ProductBatches batch)
        {

            var db = new AccountingContainer();
            db.Entry(batch).State = EntityState.Modified;
            db.SaveChanges();


        }

        public static StorePriceRenew getAllProductInThatStore(int StoreId)
        {
            var db = new AccountingContainer();
            List<ProductBatches> batches = new List<ProductBatches>();
            foreach (var NotBach in db.Products.Where(a => a.ProductBatches.Count() == 0))
            {
                var batch = new ProductBatches();
                batch.Store_ID = StoreId;
                batch.Product_ID = NotBach.Prod_ID;
                batch.SalesPrice = NotBach.Sale_Price;
                if (NotBach.Prod_Type == Prod_Type.اولي || NotBach.Prod_Type == Prod_Type.منتج)
                {
                    batch.Product_Quantity = 0;
                }
                else
                {
                    batch.Product_Quantity = 1;
                }
                batch.Product_Cost = 0;
               
                batch.Batch_Cost = 0;
                batch.IsActive = true;
              
                batches.Add(batch);
           
            }
            db.ProductBatches.AddRange(batches);
            db.SaveChanges();
            var storPrice = new StorePriceRenew();

            storPrice.CatList = db.Categories.ToList();
            //storPrice.CatList = new SelectList(categoryList, "Cat_ID", "Cat_Descrip");
          var TaxList = db.Taxes.ToList();
            storPrice.StoreId = StoreId;
            storPrice.ListProduct = new List<StoreSalesPricesProduct>();
            int count = 1;
            
            foreach(var batch in db.ProductBatches.Where(a=>a.Store_ID== StoreId || a.Store_ID == 0).OrderByDescending(a=>a.IsActive))
            {
                var productSale = new StoreSalesPricesProduct();
                productSale.count = count;
                //productSale.CatList = storPrice.CatList;
                productSale.batchId = batch.Batch_ID;
                productSale.ProductCode = batch.Product.Bar_Code;
                productSale.ProductId= batch.Product.Prod_ID;
                productSale.ProductName = batch.Product.Prod_Name;
                productSale.batch = batch;
                productSale.CategoryName = "";
                if (batch.Product.Category_ID>0) {

                    productSale.CategoryId = batch.Product.Category_ID.Value;
                }
                productSale.IsActive = true;
                if (batch.IsActive != null)
                {
                    productSale.IsActive = batch.IsActive.Value;
                }
            
                if (batch.SalesPrice != null && batch.SalesPrice != 0)
                {
                    productSale.SalesPrice = batch.SalesPrice.Value;
                }
                else
                {
                    productSale.SalesPrice = batch.Product.Sale_Price.Value;
                }
                productSale.SalesPriceCommon = productSale.SalesPrice;
                if (batch.Product_Quantity != 0) {
                    productSale.CostPrice = batch.Batch_Cost / batch.Product_Quantity;
                }
                if (batch.Product.Tax_Id > 0)
                {
                    productSale.taxPercentage = TaxList.Where(a => a.Tax_ID == batch.Product.Tax_Id).FirstOrDefault().Tax_Percentage;

                }
                else
                {
                    productSale.taxPercentage = 0;
                }

                productSale.SalesPriceWithTax = (productSale.taxPercentage / 100) * productSale.SalesPriceCommon + productSale.SalesPriceCommon;
                storPrice.ListProduct.Add(productSale);

                count++;



            }


            return storPrice;
        }



        public static int PostNewSalesForStore(dynamic json)
        {
            var db = new AccountingContainer();
            var storeId = 0;
            foreach (var batchvm in json)
            {
                ProductBatches batch = db.ProductBatches.Find(Convert.ToInt32(batchvm.batchId));
                storeId = batch.Store_ID.Value;
                batch.IsActive =Convert.ToBoolean( batchvm.IsActive);
                batch.SalesPrice = Convert.ToDecimal( batchvm.SalesPrice);
                var product = batch.Product;
                product.Category_ID= Convert.ToInt32(batchvm.Cat_ID);
                product.Bar_Code = batchvm.ProductCode;
                db.Entry(batch).State = EntityState.Modified;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

            }
            return storeId;




        }


        public static string updateAllServicesToStore()
        {

            var db = new AccountingContainer();
            var ServicesBatch = db.ProductBatches.Where(a => a.Product.Prod_Type == Prod_Type.خدمة || a.Product.Prod_Type == Prod_Type.مجمع).ToList();
            
            if (ServicesBatch.FirstOrDefault() != null&& ServicesBatch.FirstOrDefault().Store_ID>0)
            {
                if (ServicesBatch.Count() == ServicesBatch.Select(a => a.Product_Quantity).Sum())
                {
                    return "";
                }
                else
                {
                    foreach (var serv in ServicesBatch)
                    {
                        
                        serv.Product_Quantity = 1;
                        db.Entry(serv).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return "done";
                }

            }
            var StoreList = db.Stores.ToList();
            var FirstStore = StoreList.FirstOrDefault();
            foreach(var serv in ServicesBatch)
            {
                serv.Store_ID = FirstStore.Store_ID;
                serv.SalesPrice = serv.Product.Sale_Price;
                serv.IsActive = true;
                serv.Product_Quantity = 1;
                db.Entry(serv).State = EntityState.Modified;
                db.SaveChanges();
            }
            StoreList.Remove(FirstStore);

            List<ProductBatches> batchList = new List<ProductBatches>();
            foreach (var newbatch in db.Products.Where(a => a.Prod_Type == Prod_Type.خدمة || a.Prod_Type == Prod_Type.مجمع))
            {
                foreach (var store in StoreList)
                {
                    var batch = new ProductBatches();
                    batch.Batch_Date = DateTime.Now;
                    batch.SalesPrice = newbatch.Sale_Price;
                    batch.Product_ID = newbatch.Prod_ID;
                    batch.Store_ID = store.Store_ID;
                    batch.Product_Quantity = 1;
                    batch.IsActive = true;
                    batchList.Add(batch);
                }
            }
            db.ProductBatches.AddRange(batchList);
            db.SaveChanges();

            return "Success";
        }

        public static bool gettheDifferenceOfAccountandstoreCard()
        {
            var db = new AccountingContainer();

            var reaalstore = db.Stores.FirstOrDefault();
            var storeCardSun = db.StoreCards.Where(a => a.Store_Id == reaalstore.Store_ID);
            //var gl = GLMasterList.Where(q => Math.Round(q.Details.Where(a=>a.Account_Id==239).Select(b => b.GLDetail_Debit- b.GLDetail_Credit).Sum(), 3, MidpointRounding.AwayFromZero) != Math.Round(q.Ccards.Where(a => a.IncreaseQuantity == true ).ToList()
            //    .Select(r => r.TotalCost.Value).Sum() - q.cards.Where(a =>
            //    a.IncreaseQuantity == false ).ToList().Select(r => r.TotalCost.Value).Sum())).ToList();
            //var GLMasterList = (from glmaster in db.GLMasters
            //                 join detail in db.GLDetails on glmaster.GLMaster_Id equals detail.GLMasterGLMaster_Id into details
            //                 join card in storeCardSun on glmaster.GLCode equals card.Document_Serial into Cards


                              
            //                 select new  
            //                 {
            //                     GLMaster = glmaster,
            //                     Ccards = Cards,
            //                        Details= details

            //                 }).ToList();

            //GLMasterList = GLMasterList.Where(e => e.Ccards.Count() > 0&&e.GLMaster.GLCode.StartsWith("SETSTO-")).ToList();
         

            //foreach (var gl in GLMasterList)
            //{
            //   var sumgldetail= gl.GLMaster.GLDetails.Where(a => a.Account_Id == 239).Select(a => a.GLDetail_Debit - a.GLDetail_Credit).Sum();

            //    var sumcard = gl.Ccards.Where(a => a.IncreaseQuantity == true).ToList()
            //    .Select(r => r.TotalCost.Value).Sum() - gl.Ccards.Where(a =>
            //    a.IncreaseQuantity == false).ToList().Select(r => r.TotalCost.Value).Sum();

            //    if (sumgldetail == sumcard)
            //    {
            //        var g = gl;
            //    }
            //    else { 

            //        var rr = gl;
            //    }
            //}

            var sum = storeCardSun.Where(a => a.IncreaseQuantity == true &&a.Date.Year==2022).Select(a => a.TotalCost).ToList().Sum() - storeCardSun.Where(a => a.IncreaseQuantity == false && a.Date.Year == 2022).Select(a => a.TotalCost).ToList().Sum();
            //var s = db.GLDetails.Where(a => a.Account_Id == 239).ToList();
            var s = db.GLDetails.Where(a => a.ChartOfAccounting.Account_Code == reaalstore.Store_Code && a.GLMaster.GL_Date.Year == 2022).ToList();
            var sumgl = s.Select(a => a.GLDetail_Debit).ToList().Sum() - s.Select(a => a.GLDetail_Credit).ToList().Sum();
            var difference = sum - sumgl;
            if (Math.Round(difference.Value, 2, MidpointRounding.AwayFromZero) == 0)
            {
                return true;
            }
            var storeList = db.Stores.ToList();
            int count = 0;

            foreach (var store in storeList)
            {

                var storeCard = db.StoreCards.Where(a => a.Store_Id == store.Store_ID).ToList();
                var Account = db.ChartOfAccountings.Where(a => a.Account_Code == store.Store_Code).FirstOrDefault();
                var gldetail = db.GLDetails.Where(a => a.Account_Id == Account.Account_Id).ToList();
                var List = new List<GLDetail>();
           
                var b = 0;
                var n = 0;
                var m = 0;
                foreach (var detail in gldetail.Where(a=>a.GLMaster.GL_Date.Year == 2022))
                {
                    var storecar = storeCard.Where(a => a.Document_Serial == detail.GLMaster.GLCode).Select(a => a.TotalCost).Sum();
                    var storecardec = storeCard.Where(a => a.Document_Serial == detail.GLMaster.GLCode && a.IncreaseQuantity == false).Select(a => a.TotalCost).Sum();
                    var storecarinc = storeCard.Where(a => a.Document_Serial == detail.GLMaster.GLCode && a.IncreaseQuantity == true).Select(a => a.TotalCost).Sum();
                    var gldeta = detail.GLDetail_Credit + detail.GLDetail_Debit;
                    if (Math.Round(storecar.Value, 2, MidpointRounding.AwayFromZero) == Math.Round(gldeta, 2, MidpointRounding.AwayFromZero) || Math.Round(storecar.Value / 2, 2, MidpointRounding.AwayFromZero) == Math.Round(gldeta, 2, MidpointRounding.AwayFromZero))
                    {



                    }
                    else
                    {

                        var a = detail.GLDetail_Id;
                        if (detail.GLMaster.GL_Date.Year == 2019)
                        {

                            b++;
                        }

                        if (detail.GLMaster.GL_Date.Year == 2020)
                        {

                            b++;
                        }
                        if (detail.GLMaster.GL_Date.Year == 2021)
                        {
                            n++;
                        }
                        if (detail.GLMaster.GL_Date.Year == 2022)
                        {

                            m++;
                        }
                        //if (detail.GLMaster.GL_Date.Year == 2020 || detail.GLMaster.GL_Date.Year == 2022 || detail.GLMaster.GL_Date.Year == 2021)
                        //{

                            if (detail.GLMaster.GLCode.StartsWith("SAL-") || detail.GLMaster.GLCode.StartsWith("SALRC-")|| detail.GLMaster.GLCode.StartsWith("POS-")||detail.GLMaster.GLCode.StartsWith("ORD-") )
                            {
                                if (detail.GLDetail_Credit > 0)
                                {
                                    detail.GLDetail_Credit = storecar.Value;
                                    detail.GLMaster.GLDetails.Where(g => g.Account_Id == 105).FirstOrDefault().GLDetail_Debit = storecar.Value;
                                    if (storecardec > 0 && storecarinc > 0)
                                    {

                                        detail.GLDetail_Credit = storecarinc.Value;
                                        detail.GLMaster.GLDetails.Where(g => g.Account_Id == 105).FirstOrDefault().GLDetail_Debit = storecarinc.Value;

                                    }
                                }
                                else
                                {
                                    detail.GLDetail_Debit = storecar.Value;
                                    detail.GLMaster.GLDetails.Where(g => g.Account_Id == 105).FirstOrDefault().GLDetail_Credit = storecar.Value;
                                    if (storecardec > 0 && storecarinc > 0)
                                    {

                                        detail.GLDetail_Debit = storecardec.Value;
                                        detail.GLMaster.GLDetails.Where(g => g.Account_Id == 105).FirstOrDefault().GLDetail_Credit = storecardec.Value;

                                    }
                                }
                                db.SaveChanges();
                            }

                        //else if (detail.GLMaster.GLCode.StartsWith("SETSTO-"))
                        //{
                           
                        //    if (detail.GLMaster.GLDetails.Where(y => y.GLDetail_Credit > 0).Count() > 1)
                        //    {
                        //        if (detail.GLDetail_Credit > 0)
                        //        {
                        //            detail.GLMaster.GLDetails.Where(h => h.Account_Id != Account.Account_Id && h.GLDetail_Debit == detail.GLDetail_Credit && h.GLDetail_Credit == detail.GLDetail_Debit).FirstOrDefault().GLDetail_Debit = storecardec.Value;
                        //            detail.GLDetail_Credit = storecardec.Value;
                        //        }
                        //        else
                        //        {
                        //            detail.GLMaster.GLDetails.Where(h => h.Account_Id != Account.Account_Id && h.GLDetail_Credit == detail.GLDetail_Debit && h.GLDetail_Credit == detail.GLDetail_Debit).FirstOrDefault().GLDetail_Credit = storecarinc.Value;
                        //            detail.GLDetail_Debit = storecarinc.Value;
                        //        }

                        //    }
                        //    else
                        //    {
                        //        if (storecardec.Value > storecarinc.Value)
                        //        {
                        //            if (detail.GLDetail_Credit - (storecardec.Value - storecarinc.Value) == 0)
                        //            {
                        //                var nd6 = detail.GLDetail_Credit - (storecardec.Value - storecarinc.Value) == 0;
                        //            }
                        //            else
                        //            {
                        //                var nd6 = detail.GLDetail_Credit - (storecardec.Value - storecarinc.Value);
                        //                detail.GLDetail_Credit = storecardec.Value - storecarinc.Value;
                        //                detail.GLMaster.GLDetails.Where(h => h.Account_Id != Account.Account_Id).FirstOrDefault().GLDetail_Debit = storecardec.Value - storecarinc.Value;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (detail.GLDetail_Debit - (storecarinc.Value - storecardec.Value) == 0)
                        //            {
                        //                var nd = detail.GLDetail_Debit - (storecarinc.Value - storecardec.Value);

                        //            }
                        //            else
                        //            {
                        //                var nd = detail.GLDetail_Debit - (storecarinc.Value - storecardec.Value);
                        //                detail.GLDetail_Debit = storecarinc.Value - storecardec.Value;
                        //                detail.GLMaster.GLDetails.Where(h => h.Account_Id != Account.Account_Id).FirstOrDefault().GLDetail_Credit = storecarinc.Value - storecardec.Value;
                        //            }
                        //        }
                        //    }

                        //    db.SaveChanges();
                          

                        //}

                        else
                            {
                                var l = 0;
                                count++;

                            }
                        


           
                    }


                }
      
                var ty = n;

                

            }


            //foreach(var gh in db.GLDetails.Where(a => a.Account_Id == 239))
            //{
            //    if(gh== storeCardSun.Where(a=>a.Document_Serial== gh.GLMaster.GLCode).Select(a=>a.))



            //}
            return false;
        }
        public static decimal LastPriceSale(int storeId, int productId)
        {
            var db = new AccountingContainer();
            var invoice = db.DetailQRInvoices.Where(a => a.MasterQRInvoice.Store_ID == storeId && a.Product_Id == productId).ToList();
            if (invoice.Count() > 0)
            {
                return invoice.OrderByDescending(a => a.Detail_Id).FirstOrDefault().Product_Price;
            }



            else
            {
                return 0;

            }
        }
        public static decimal LastPricePurchase(int storeId, int productId)
        {
            var db = new AccountingContainer();
            var invoice = db.PurchaseDetails.Where(a => a.InvoiceMaster.Store_ID == storeId && a.Product_ID == productId).ToList();
            if (invoice.Count() > 0)
            {
                return invoice.OrderByDescending(a=>a.Details_Id).FirstOrDefault().Product_Price;
            }



            else
            {
                return 0;

            }
        }

        public static decimal getStorebalance(int? store)
        {
            var db = new AccountingContainer();
            var storcard = db.StoreCards.ToList();
                if (store > 0) {
                storcard = storcard.Where(a => a.Store_Id == store).ToList();
            }
            decimal balance = storcard.Where(a => a.IncreaseQuantity==true).Select(a => a.TotalCost.Value).Sum()- storcard.Where(a => a.IncreaseQuantity==false).Select(a => a.TotalCost.Value).Sum();
            return balance;

        }
      


    }
    }

