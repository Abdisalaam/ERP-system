using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Accountingsystem.Utilites
{
    public class OpenBalanceFile
    {
        public static void MakeOpenBallanceForGroupOFCutomer(List<Customer> ListCutomer)
        {
            var glMAster = StanderGlmaster();
            var db = new AccountingContainer();
            var Wasset = new GLDetail();
            Wasset.Account_Id = 131;
            Wasset.Account_Name = db.ChartOfAccountings.Find(131).Account_Name;
            decimal totalCredit = 0;
            decimal totalDepit = 0;

            foreach (var item in ListCutomer)
            {
                if (item.Customer_Balance == 0)
                {

                }
                else
                {
                    var customer = new GLDetail();
                    customer.Account_Id = 37;
                    customer.Account_Name = db.ChartOfAccountings.Find(37).Account_Name;
                    if (item.Customer_Balance > 0)
                    {
                        customer.GLDetail_Debit = item.Customer_Balance.Value;
                        customer.GLDetail_Credit = 0;
                    }
                    else
                    {
                        customer.GLDetail_Credit = item.Customer_Balance.Value * -1;
                        customer.GLDetail_Debit = 0;
                    }

                    totalCredit += customer.GLDetail_Credit;
                    totalDepit += customer.GLDetail_Debit;
                    customer.Account_Child_ID = db.Accounts.Where(a => a.Account_Code == item.Customer_Code).FirstOrDefault().Accounts_Id;
                    customer.Account_CHild_Name = db.Accounts.Where(a => a.Account_Code == item.Customer_Code).FirstOrDefault().Account_Name;
                    glMAster.GLDetails.Add(customer);
                }
            }
                if(totalCredit> totalDepit)
                {

                    Wasset.GLDetail_Debit = totalCredit - totalDepit;
                }
                else
                {
                    Wasset.GLDetail_Credit = totalDepit - totalCredit;
                }
                glMAster.GLDetails.Add(Wasset);
                db.GLMasters.Add(glMAster);
            GLMaster.GetBalanceForAccount(glMAster);
            GLMaster.GetBalanceForSubLedger(glMAster);
            db.SaveChanges();

        



        }

        public static void MakeOpenBallanceForGroupOFSupplier(List<Supplier> ListSuplier)
        {
            var glMAster = StanderGlmaster();
            var db = new AccountingContainer();
            var Wasset = new GLDetail();
            Wasset.Account_Id = 131;
            Wasset.Account_Name = db.ChartOfAccountings.Find(131).Account_Name;
            decimal totalCredit = 0;
            decimal totalDepit = 0;

            foreach (var item in ListSuplier)
            {
                if (item.Supp_Balance == 0)
                {

                }
                else
                {
                    var supplier = new GLDetail();
                    supplier.Account_Id = 45;
                    supplier.Account_Name = db.ChartOfAccountings.Find(45).Account_Name;
                    if (item.Supp_Balance > 0)
                    {
                        supplier.GLDetail_Debit = item.Supp_Balance.Value;
                        supplier.GLDetail_Credit = 0;
                    }
                    else
                    {
                        supplier.GLDetail_Credit = item.Supp_Balance.Value * -1;
                        supplier.GLDetail_Debit = 0;
                    }

                    totalCredit += supplier.GLDetail_Credit;
                    totalDepit += supplier.GLDetail_Debit;
                    supplier.Account_Child_ID = db.Accounts.Where(a => a.Account_Code == item.Supp_Code).FirstOrDefault().Accounts_Id;
                    supplier.Account_CHild_Name = db.Accounts.Where(a => a.Account_Code == item.Supp_Code).FirstOrDefault().Account_Name;
                    glMAster.GLDetails.Add(supplier);
                }
            }
            if (totalCredit > totalDepit)
            {

                Wasset.GLDetail_Debit = totalCredit - totalDepit;
            }
            else
            {
                Wasset.GLDetail_Credit = totalDepit - totalCredit;
            }
            glMAster.GLDetails.Add(Wasset);
            db.GLMasters.Add(glMAster);
            GLMaster.GetBalanceForAccount(glMAster);
            GLMaster.GetBalanceForSubLedger(glMAster);
            db.SaveChanges();





        }

        public static void MakeOpenBallanceForGroupOFBank(List<Bank> ListBank)
        {
            var glMAster = StanderGlmaster();
            var db = new AccountingContainer();
            var Wasset = new GLDetail();
            Wasset.Account_Id = 131;
            Wasset.Account_Name = db.ChartOfAccountings.Find(131).Account_Name;
            decimal totalCredit = 0;
            decimal totalDepit = 0;

            foreach (var item in ListBank)
            {
                if (item.Balance_Amount == 0)
                {

                }
                else
                {
                    var bank = new GLDetail();
                    bank.Account_Id = db.ChartOfAccountings.Where(a=>a.Account_Code==item.Bank_Code).FirstOrDefault().Account_Id;
                    bank.Account_Name = db.ChartOfAccountings.Where(a => a.Account_Code == item.Bank_Code).FirstOrDefault().Account_Name;
                    if (item.Balance_Amount > 0)
                    {
                        bank.GLDetail_Debit = item.Balance_Amount.Value;
                        bank.GLDetail_Credit = 0;
                    }
                    else
                    {
                        bank.GLDetail_Credit = item.Balance_Amount.Value * -1;
                        bank.GLDetail_Debit = 0;
                    }

                    totalCredit += bank.GLDetail_Credit;
                    totalDepit += bank.GLDetail_Debit;
                   
                    glMAster.GLDetails.Add(bank);
                }
            }
            if (totalCredit > totalDepit)
            {

                Wasset.GLDetail_Debit = totalCredit - totalDepit;
            }
            else
            {
                Wasset.GLDetail_Credit = totalDepit - totalCredit;
            }
            glMAster.GLDetails.Add(Wasset);
            db.GLMasters.Add(glMAster);
            GLMaster.GetBalanceForAccount(glMAster);
            GLMaster.GetBalanceForSubLedger(glMAster);
            db.SaveChanges();


        }


        public static void MakeOpenBallanceForGroupOFTreasury(List<Treasury> ListTreasurey)
        {
            var glMAster = StanderGlmaster();
            var db = new AccountingContainer();
            var Wasset = new GLDetail();
            Wasset.Account_Id = 131;
            Wasset.Account_Name = db.ChartOfAccountings.Find(131).Account_Name;
            decimal totalCredit = 0;
            decimal totalDepit = 0;

            foreach (var item in ListTreasurey)
            {
                if (item.Balance_Amount == 0)
                {

                }
                else
                {
                    var bank = new GLDetail();
                    bank.Account_Id = db.ChartOfAccountings.Where(a => a.Account_Code == item.Trea_Code).FirstOrDefault().Account_Id;
                    bank.Account_Name = db.ChartOfAccountings.Where(a => a.Account_Code == item.Trea_Code).FirstOrDefault().Account_Name;
                    if (item.Balance_Amount > 0)
                    {
                        bank.GLDetail_Debit = item.Balance_Amount.Value;
                        bank.GLDetail_Credit = 0;
                    }
                    else
                    {
                        bank.GLDetail_Credit = item.Balance_Amount.Value * -1;
                        bank.GLDetail_Debit = 0;
                    }

                    totalCredit += bank.GLDetail_Credit;
                    totalDepit += bank.GLDetail_Debit;

                    glMAster.GLDetails.Add(bank);
                }
            }
            if (totalCredit > totalDepit)
            {

                Wasset.GLDetail_Debit = totalCredit - totalDepit;
            }
            else
            {
                Wasset.GLDetail_Credit = totalDepit - totalCredit;
            }
            glMAster.GLDetails.Add(Wasset);
            db.GLMasters.Add(glMAster);
            GLMaster.GetBalanceForAccount(glMAster);
            GLMaster.GetBalanceForSubLedger(glMAster);
            db.SaveChanges();

        }

        public static GLMaster StanderGlmaster()

        {
            var glmas = new GLMaster();
            var db = new AccountingContainer();
            glmas.Company_Id = db.Companies.FirstOrDefault().Comp_Id;
            glmas.Currency_Id = db.Currencies.FirstOrDefault().Currency_Type;
            glmas.GL_Date = DateTime.Now;
            var lastCode = db.GLMasters.OrderByDescending(c => c.GLMaster_Id).Where(a => a.GLCode.StartsWith("GLST")).FirstOrDefault();
            if (lastCode == null)
            {
                glmas.GLCode = "GLST-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                glmas.GLCode = "GLST-" + (Convert.ToInt32(lastCode.GLCode.Substring(5, lastCode.GLCode.Length - 5)) + 1).ToString("D3");
            }

            glmas.GL_Desc = "تمت اضافة القيد عن طريق ملف اكسل";
            glmas.GLState = TransactionState.Post;
            glmas.GLType_Id = "قيد افتتاحى";
            glmas.Auto = true;

            return glmas;
        }

        public static void MakeOpenBallanceForGroupOFpartner(List<Partner> ListPartner)
        {
            var glMAster = StanderGlmaster();
            var db = new AccountingContainer();
            var Wasset = new GLDetail();
            Wasset.Account_Id = 131;
            Wasset.Account_Name = db.ChartOfAccountings.Find(131).Account_Name;
            decimal totalCredit = 0;
            decimal totalDepit = 0;

            foreach (var item in ListPartner)
            {
                if (item.Balance_Amount == 0)
                {

                }
                else
                {
                    var partner = new GLDetail();
                    partner.Account_Id = db.ChartOfAccountings.Where(a => a.Account_Code == item.Partner_Code).FirstOrDefault().Account_Id;
                    partner.Account_Name = db.ChartOfAccountings.Where(a => a.Account_Code == item.Partner_Code).FirstOrDefault().Account_Name;
                    if (item.Balance_Amount > 0)
                    {
                        partner.GLDetail_Debit = item.Balance_Amount.Value;
                        partner.GLDetail_Credit = 0;
                    }
                    else
                    {
                        partner.GLDetail_Credit = item.Balance_Amount.Value * -1;
                        partner.GLDetail_Debit = 0;
                    }

                    totalCredit += partner.GLDetail_Credit;
                    totalDepit += partner.GLDetail_Debit;

                    glMAster.GLDetails.Add(partner);
                }
            }
            if (totalCredit > totalDepit)
            {

                Wasset.GLDetail_Debit = totalCredit - totalDepit;
            }
            else
            {
                Wasset.GLDetail_Credit = totalDepit - totalCredit;
            }
            glMAster.GLDetails.Add(Wasset);
            db.GLMasters.Add(glMAster);
            GLMaster.GetBalanceForAccount(glMAster);
            GLMaster.GetBalanceForSubLedger(glMAster);
            db.SaveChanges();

        }

        public static void MakeOpenBallanceForGroupOFProduct(List<Product> ListProduct)
        {
            var glMAster = StanderGlmaster();
            var db = new AccountingContainer();
            var Wasset = new GLDetail();
            Wasset.Account_Id = 131;
            Wasset.Account_Name = db.ChartOfAccountings.Find(131).Account_Name;

            decimal totalDepit = 0;
            var DistincStore = ListProduct.Select(a => a.Store_Id).Distinct().ToList();
            foreach (var item in DistincStore.Where(a => a.Value != 0 || a.Value != null))
            {
                var glDetail = new GLDetail();

                if (item != 0)
                {
                    var iteminThatStore = ListProduct.Where(a => a.Store_Id == item && a.Quantity != 0).ToList();
                    foreach (var prod in iteminThatStore)
                    {

                        glDetail.GLDetail_Debit += prod.Quantity.Value * prod.Prod_Cost_Price.Value;
                        glDetail.GLDetail_Credit = 0;
                        var Store = db.Stores.Find(item);
                        glDetail.Account_Id = db.ChartOfAccountings.Where(a => a.Account_Code == Store.Store_Code).FirstOrDefault().Account_Id;
                        glDetail.Account_Name = db.ChartOfAccountings.Where(a => a.Account_Code == Store.Store_Code).FirstOrDefault().Account_Name;

                        makeProductBatch(prod);


                    }

                    glMAster.GLDetails.Add(glDetail);
                    totalDepit += glDetail.GLDetail_Debit;


                }
            }
            Wasset.GLDetail_Credit = totalDepit;
            glMAster.GLDetails.Add(Wasset);
            db.GLMasters.Add(glMAster);
            GLMaster.GetBalanceForAccount(glMAster);
            GLMaster.GetBalanceForSubLedger(glMAster);
            db.SaveChanges();
        }

        public static void makeProductBatch(Product Prod)
        {
            var db = new AccountingContainer();
            var batch = new ProductBatches();
            batch.Product_Quantity = Prod.Quantity.Value;
            batch.Product_Cost = Prod.Prod_Cost_Price.Value;
            batch.Batch_Cost = Prod.Prod_Cost_Price.Value * Prod.Quantity.Value;
            batch.Store_ID = Prod.Store_Id.Value;
            batch.Product_ID = Prod.Prod_ID;
            db.ProductBatches.Add(batch);
            var storecard = new StoreCard();
            storecard.CostPrice = Prod.Prod_Cost_Price.Value;
            storecard.Date = DateTime.Now;
            storecard.Store_Id = batch.Store_ID.Value;
            storecard.Quantity = Prod.Quantity.Value;
            storecard.SalePrice = 0;
            storecard.Product_Id = batch.Product_ID;
            storecard.IncreaseQuantity = true;
            storecard.TypeOfDocument = "رصيد افتتاحي من ملف اكسيل";
            storecard.Document_Serial = "Exsel_Product";
            storecard.TotalCost = Prod.Prod_Cost_Price.Value*Prod.Quantity.Value;
            storecard.TotalAmount = 0;
            storecard.Account_Id = 0;
            storecard.Subledger_Id = 0;
            db.StoreCards.Add(storecard);
            db.SaveChanges();
            db.Dispose();


        }


        //public static void MakeOpenBallanceForGroupOFAsset(List<FixedAsset> fixedAssetsList)
        //{
        //    var glMAster = StanderGlmaster();
        //    var db = new AccountingContainer();
        //    var Wasset = new GLDetail();
        //    Wasset.Account_Id = 131;
        //    Wasset.Account_Name = db.ChartOfAccountings.Find(131).Account_Name;

        //    decimal totalDepit = 0;
        //    decimal totalCredit = 0;
        //    var DistincfixTyye = fixedAssetsList.Select(a => a.FixedAssetTypeFAsset_TypeID).Distinct().ToList();
        //    foreach (var item in DistincfixTyye)
        //    {
        //        var glDetail = new GLDetail();

        //        if (item != 0)
        //        {
        //            var iteminThatStore = fixedAssetsList.Where(a => a.FixedAssetTypeFAsset_TypeID == item).ToList();
        //            foreach (var asset in iteminThatStore)
        //            {
        //                if (asset.BookValue.Value > 0)
        //                {
        //                    glDetail.GLDetail_Debit += Convert.ToDecimal(asset.BookValue.Value);
        //                    glDetail.GLDetail_Credit += 0;

        //                }
        //                else
        //                {
        //                    glDetail.GLDetail_Credit += Convert.ToDecimal(asset.BookValue.Value);
        //                    glDetail.GLDetail_Debit += 0;

        //                }


        //                glDetail.Account_Id = db.ChartOfAccountings.Find(asset.FixedAssetTypeFAsset_TypeID).Account_Id;
        //                glDetail.Account_Name = db.ChartOfAccountings.Find(asset.FixedAssetTypeFAsset_TypeID).Account_Name;




        //            }
        //            if (glDetail.GLDetail_Debit >= glDetail.GLDetail_Credit)
        //            {
        //                glDetail.GLDetail_Debit = glDetail.GLDetail_Debit - glDetail.GLDetail_Credit;

        //                glDetail.GLDetail_Credit = 0;
        //            }
        //            else
        //            {
        //                glDetail.GLDetail_Credit = glDetail.GLDetail_Credit - glDetail.GLDetail_Debit;
        //                glDetail.GLDetail_Debit = 0;
        //            }

        //            glMAster.GLDetails.Add(glDetail);
        //            totalDepit += glDetail.GLDetail_Debit;
        //            totalCredit += glDetail.GLDetail_Credit;

        //        }
        //    }

        //    Wasset.GLDetail_Credit = totalDepit;
        //    Wasset.GLDetail_Debit = totalCredit;




        //    glMAster.GLDetails.Add(Wasset);
        //    db.GLMasters.Add(glMAster);
        //    GLMaster.GetBalanceForAccount(glMAster);
        //    GLMaster.GetBalanceForSubLedger(glMAster);
        //    db.SaveChanges();




        //}
        public static void MakeOpenBallanceForGroupOFAsset(List<FixedAsset> fixedAssetsList)
        {
            var glMAster = StanderGlmaster();
            var db = new AccountingContainer();
            var Wasset = new GLDetail();
            Wasset.Account_Id = 131;
            Wasset.Account_Name = db.ChartOfAccountings.Find(131).Account_Name;

            decimal totalDepit = 0;
            var DistincfixTyye = fixedAssetsList.Select(a => a.FixedAssetTypeFAsset_TypeID).Distinct().ToList();
            foreach (var item in DistincfixTyye)
            {
                var glDetail = new GLDetail();

                if (item != 0)
                {
                    var iteminThatStore = fixedAssetsList.Where(a => a.FixedAssetTypeFAsset_TypeID == item).ToList();
                    foreach (var asset in iteminThatStore)
                    {

                        glDetail.GLDetail_Debit += Convert.ToDecimal(asset.BookValue.Value);
                        glDetail.GLDetail_Credit = 0;

                        glDetail.Account_Id = db.ChartOfAccountings.Find(asset.FixedAssetTypeFAsset_TypeID).Account_Id;
                        glDetail.Account_Name = db.ChartOfAccountings.Find(asset.FixedAssetTypeFAsset_TypeID).Account_Name;




                    }

                    glMAster.GLDetails.Add(glDetail);
                    totalDepit += glDetail.GLDetail_Debit;


                }
            }
            Wasset.GLDetail_Credit = totalDepit;
            glMAster.GLDetails.Add(Wasset);
            db.GLMasters.Add(glMAster);
            GLMaster.GetBalanceForAccount(glMAster);
            GLMaster.GetBalanceForSubLedger(glMAster);
            db.SaveChanges();




        }



        public static void MakeOpenBallanceForGroupOFProductForoneStore(List<ProductVM> ListProduct,int storeId)
        {
            var glMAster = StanderGlmaster();
            var db = new AccountingContainer();
            var Wasset = new GLDetail();
            Wasset.Account_Id = 131;
            Wasset.Account_Name = db.ChartOfAccountings.Find(131).Account_Name;
            var storecode = db.Stores.Find(storeId).Store_Code;
            
            var storegl = new GLDetail();
            storegl.Account_Id = db.ChartOfAccountings.Where(a => a.Account_Code == storecode).FirstOrDefault().Account_Id;
            storegl.Account_Name = db.ChartOfAccountings.Where(a => a.Account_Code == storecode).FirstOrDefault().Account_Name;
            decimal total = 0;
           
            foreach (var item in ListProduct)
            {




                var totalcost = makeProductBatchforOneStore(item, storeId);
                total += totalcost;


            }
            Wasset.GLDetail_Credit = total;
            Wasset.GLDetail_Debit = 0;

            storegl.GLDetail_Credit = 0;
            storegl.GLDetail_Debit = total;

            glMAster.GLDetails.Add(Wasset);
            glMAster.GLDetails.Add(storegl);
            db.GLMasters.Add(glMAster);
            GLMaster.GetBalanceForAccount(glMAster);
            GLMaster.GetBalanceForSubLedger(glMAster);
            db.SaveChanges();
        }

        public static decimal makeProductBatchforOneStore(ProductVM Prod,int storeid)
        {
            var db = new AccountingContainer();
            var batch = new ProductBatches();
            if (db.ProductBatches.Where(a => a.Product.Prod_Code == Prod.Prod_Code && a.Store_ID == storeid).FirstOrDefault() != null)
            {
                var old = db.ProductBatches.Where(a => a.Product.Prod_Code == Prod.Prod_Code && a.Store_ID == storeid).FirstOrDefault();
                old.Product_Quantity += Prod.Product_Quantity;
                old.Product_Cost += Prod.Prod_cost_Price;
                old.Batch_Cost += Prod.Product_Quantity * old.Product_Cost;
                db.Entry(old).State = EntityState.Modified;
            }
            else
            {
                var poduct = db.Products.Where(a => a.Prod_Code == Prod.Prod_Code).FirstOrDefault();
                batch.Product_Quantity = Prod.Product_Quantity;
                batch.Product_Cost = poduct.Prod_Cost_Price.Value;
                batch.Batch_Cost = batch.Product_Quantity * batch.Product_Cost;
                batch.Store_ID = storeid;
                batch.Product_ID = poduct.Prod_ID;
                db.ProductBatches.Add(batch);
            }
            var storecard = new StoreCard();
            storecard.CostPrice = batch.Product_Cost;
            storecard.Date = DateTime.Now;
            storecard.Store_Id = storeid;
            storecard.Quantity = batch.Product_Quantity;
            storecard.SalePrice = 0;
            storecard.Product_Id = batch.Product_ID;
            storecard.IncreaseQuantity = true;
            storecard.TypeOfDocument = "رصيد افتتاحي من ملف اكسيل";
            storecard.Document_Serial = "Exsel_Product";
            storecard.TotalCost = batch.Batch_Cost;
            storecard.TotalAmount = 0;
            storecard.Account_Id = 0;
            storecard.Subledger_Id = 0;
            db.StoreCards.Add(storecard);
            db.SaveChanges();
            db.Dispose();
           return batch.Batch_Cost;


        }

    }
}