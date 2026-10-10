using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using System.Web.Mvc;

using System.IO;
using System.Runtime.InteropServices;
using Rotativa;
using System.Text;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data.Entity.Validation;
using System.Data.Entity;
using ZXing;
using System.Drawing;
using ZXing.QrCode;

namespace Accountingsystem.Models
{
    public partial class Product
    {
        public static bool containCodeOpen(string OpenBalance_Code)
        {
            var db = new AccountingContainer();
            var allinData = db.Products.ToList();
            if (!OpenBalance_Code.StartsWith("OPP-"))
            {
                return true;
            }
            else
            {
                foreach (var item in allinData)
                {
                    if (item.OpenBalance_Code == OpenBalance_Code || !OpenBalance_Code.StartsWith("OPP-"))
                    {
                        return true;
                    }
                    else
                    {

                    }

                }
               
            }
            return false;

        }
        public static bool containCode(string Prod_Code)
        {
            var db = new AccountingContainer();
            var allinData = db.Products.ToList();
            if (!Prod_Code.StartsWith("PRO-"))
            {
                return true;
            }
            else
            {
                foreach (var item in allinData)
                {
                    if (item.Prod_Code == Prod_Code || !Prod_Code.StartsWith("PRO-"))
                    {
                        return true;
                    }
                    else
                    {

                    }

                }
               
            }
            return false;
        }
        // use this method to  Get List of Pordect from Data base. 
        public static List<Product> GetAllPordect()
        {
            var db = new AccountingContainer();
            var prodect = db.Products.Where(a=>a.IsActive!=false).ToList();
        
          
            return prodect;
        }
        public static List<Product> GetAllPordectNotActive()
        {
            var db = new AccountingContainer();
            var prodect = db.Products.Where(a => a.IsActive == false).ToList();
         

            return prodect;
        }
        // use this method to  Get one Element PordectById from Data base. 
        public static Product GetProdectById(int id)
        {
            var db = new AccountingContainer();
            var prodect = db.Products.Find(id);
            return prodect;
        }
        // use this method to  Get one Element PordectByCode from Data base. 
        public static Product GetVandorByCode(string ProdectCode)
        {
            var db = new AccountingContainer();
            var prod = db.Products.Where(c => c.Prod_Code == ProdectCode);
            var localprod = new Product();
            if (prod != null)
            {
                localprod = prod.FirstOrDefault();
                return localprod;
            }
            return null;
        }

        public static List<SlicesTem> GetAllTempSliceProductDetails(int productVmId)
        {
            var db = new AccountingContainer();

            var _SlicesTem = db.SlicesTems.Where(slice => slice.ProductTemProd_ID == productVmId).ToList();
            return _SlicesTem;
        }
        public static List<ProductCompoundTemp> GetAllTempCompondint (int productVmId)
        {
            var db = new AccountingContainer();

            var Compound = db.ProductCompoundTemps.Where(slice => slice.Master_Product_Id == productVmId).ToList();
            return Compound;
        }

        public static SlicesProductVM PrepareSliceProductDetail()
        {
            var db = new AccountingContainer();
            var _SliceProductVm = new SlicesProductVM();

            _SliceProductVm.MySliceList = new SelectList(db.Slices, "Slice_Id", "Slice_Name");
            return _SliceProductVm;

        }
        public static ProductCompoundVM PrepareProductCompound()
        {
            var db = new AccountingContainer();
            var compound = new ProductCompoundVM();
            List<object> newList = new List<object>();
            var members = db.Products.Where(a => a.Prod_Type != Models.Prod_Type.مجمع&&a.IsActive!=false);
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.Prod_ID,
                    Name = member.Prod_Name + "   " + member.Bar_Code 
                });
            compound.ProductList = new SelectList(newList, "Id", "Name");

            //compound.ProductList = new SelectList(db.Products.Where(a=>a.Prod_Type!=Models.Prod_Type.مجمع), "Prod_ID", "Prod_Name");
            return compound;

        }

        //public static string AddNewSliceDetailsTemp(SlicesProductVM slicesDetail)
        //{
        //    var db = new AccountingContainer();
        //    var sliceTemp = db.ProductTems.Find(slicesDetail.Product_ID);
        //    var sliceDetailsTemp = new SlicesTem();
        //    sliceDetailsTemp.ProductTemProd_ID = slicesDetail.Product_ID;
        //    sliceDetailsTemp.SlicesSlice_Id = slicesDetail.SlicesSlice_Id;
        //    var slice_Name = db.Slices.Where(s => s.Slice_Id == slicesDetail.SlicesSlice_Id).FirstOrDefault().Slice_Name;
        //    sliceDetailsTemp.Slice_Name = slice_Name;
        //    sliceDetailsTemp.Slice_Price = slicesDetail.Slice_Price;


        //    sliceTemp.SlicesTems.Add(sliceDetailsTemp);            
        //    db.Entry(sliceTemp).State = EntityState.Modified;
        //    db.SaveChanges();

        //    return "تم اضافة الشريحة بنجاح";
        //}

        public static string AddNewSliceDetailsTemp(SlicesProductVM slicesDetail)
        {
            var db = new AccountingContainer();
            if (slicesDetail.SlicesSlice_Id == null || slicesDetail.SlicesSlice_Id == 0)
            {
                return Resources.validate.SliceMust;
            }
            if (slicesDetail.Slice_Price == null || slicesDetail.Slice_Price == 0)
            {
                return Resources.validate.PriceMust;
            }

            var sliceTemp = db.ProductTems.Find(slicesDetail.Product_ID);
            var sliceDetailsTemp = new SlicesTem();
            sliceDetailsTemp.ProductTemProd_ID = slicesDetail.Product_ID;
            sliceDetailsTemp.SlicesSlice_Id = slicesDetail.SlicesSlice_Id;
            var slice_Name = db.Slices.Where(s => s.Slice_Id == slicesDetail.SlicesSlice_Id).FirstOrDefault().Slice_Name;
            sliceDetailsTemp.Slice_Name = slice_Name;
            sliceDetailsTemp.Slice_Price = slicesDetail.Slice_Price;
            var dtails = sliceTemp.SlicesTems;

            var dublicateinTemp = dtails.Where(z => z.ProductTemProd_ID == slicesDetail.Product_ID && z.SlicesSlice_Id == slicesDetail.SlicesSlice_Id).ToList();
            if (dublicateinTemp.FirstOrDefault() != null)
            {
                return Resources.validate.SliceIsExist;
            }

            sliceTemp.SlicesTems.Add(sliceDetailsTemp);
            db.Entry(sliceTemp).State = EntityState.Modified;
            db.SaveChanges();

            return Resources.validate.SuccessfullAdd;
        }

        public static string AddNewProductCompoundTemp(ProductCompoundVM vm)
        {
            var db = new AccountingContainer();
            if (vm.ProductId == null || vm.ProductId == 0)
            {
                return Resources.validate.ChooseProduct;
            }
            if (vm.quantity == null || vm.quantity == 0)
            {
                return Resources.InvoiceMasterAddNewInvoice.Quantity;
            }

            var sliceTemp = db.ProductTems.Find(vm.Main_Product_Id);
            var Compound = new ProductCompoundTemp();
            Compound.Product_Id = vm.ProductId;
            Compound.Master_Product_Id = vm.Main_Product_Id;
            Compound.Quantity = vm.quantity;
            Compound.Master_Product_Id = vm.Main_Product_Id;
           
            var Compounds = sliceTemp.ProductCompoundTemps;

            var dublicateinTemp = Compounds.Where(z => z.Product_Id == vm.ProductId ).ToList();
            if (dublicateinTemp.FirstOrDefault() != null)
            {
                return Resources.OfferPriceMasterCreateOffer.ProductExisitInListModelMessage;
            }

            sliceTemp.ProductCompoundTemps.Add(Compound);
            db.Entry(sliceTemp).State = EntityState.Modified;
            db.SaveChanges();

            return Resources.validate.SuccessfullAdd;
        }
        public static string DeleteCompoundFromTemp(int CompondId)
        {
            var db = new AccountingContainer();
            ProductCompoundTemp vm = db.ProductCompoundTemps.Find(CompondId);
            db.ProductCompoundTemps.Remove(vm);
            db.SaveChanges();
            db.Dispose();
            return Resources.validate.DeleteSuccessful;
        }
        public static string DeleteSliceFromTemp(int sliceId)
        {
            var db = new AccountingContainer();
            SlicesTem vm = db.SlicesTems.Find(sliceId);
            db.SlicesTems.Remove(vm);
            db.SaveChanges();
            db.Dispose();
            return Resources.validate.DeleteSuccessful;
        }
        // use this method For Creation new Product and get Data from user.
        public static ProductVM CreateVMForCreation()
        {
            var db = new AccountingContainer();
            var prodectvm = new ProductVM();
        

            prodectvm.CategoryList = new SelectList(db.Categories, "Cat_ID", "Cat_Descrip");
            prodectvm.MeasruingUnitList = new SelectList(db.MeasuringUnits, "MUnit_ID", "MUnit_Descrip");
           
            prodectvm.Prod_Type = Models.Prod_Type.منتج;
            prodectvm.Prod_cost_Price = 1 ;
            prodectvm.MySliceList = new SelectList(db.Slices.Where(a=>a.Slice_Id==1), "Slice_Id", "Slice_Name");
            //sort the Invoices and get the last insert Invoice sales.
            var lastSerial = db.Products.OrderByDescending(c => c.Prod_ID).FirstOrDefault();
            if (lastSerial == null)
            {
                prodectvm.Prod_Code = "PRO-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                prodectvm.Prod_Code = "PRO-" + (Convert.ToInt32(lastSerial.Prod_Code.Substring(4, lastSerial.Prod_Code.Length - 4)) + 1).ToString("D3");
            }

            List<object> newList2 = new List<object>();
            var members2 = db.Taxes.Where(t=>t.Tax_Code.StartsWith("020104001")).Where(a => a.IsActive != false).ToList();
            foreach (var member1 in members2)
                newList2.Add(new
                {
                    Id = member1.Tax_ID,
                    Name = member1.Tax_Name + "  % " + member1.Tax_Percentage
                });
            prodectvm.MyTaxList = new SelectList(newList2, "Id", "Name");
            prodectvm.SelectedTaxId = members2.FirstOrDefault().Tax_ID;
            List<object> newList = new List<object>();
            var members = db.Taxes.Where(t => t.Tax_Code.StartsWith("010108001")).Where(a => a.IsActive != false).ToList();
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.Tax_ID,
                    Name = member.Tax_Name + "  % " + member.Tax_Percentage
                });
            prodectvm.MyTaxListPurchas = new SelectList(newList, "Id", "Name");
            prodectvm.SelectedTax_Purch = members.FirstOrDefault().Tax_ID;
            var productTemp = new ProductTem();
            var lastID = (from s in db.ProductTems
                          select s).ToList().LastOrDefault();


            if (lastID == null)
            { productTemp.Prod_ID = 1; }
            else
            { productTemp.Prod_ID = lastID.Prod_ID + 1; }

            db.ProductTems.Add(productTemp);
            db.SaveChanges();

            prodectvm.Prod_ID = productTemp.Prod_ID;

           

            return prodectvm;
        }
        // use this method to  Create New Product and save it in DataBase.
        public static  int CreateProdect(ProductVM prodVM)
        {
            var db = new AccountingContainer();
            var product = new Product();
            var _ProductTemp = db.ProductTems.Find(prodVM.Prod_ID);

            try
            {
                if (prodVM != null)
                {
                    // create selectList from Database to entity MeasuringUnit.
                    var resultmeasruingunit = db.MeasuringUnits.Find(prodVM.SelectedMeasuringUnit_ID);
                    if (resultmeasruingunit != null)
                    {
                        product.MeasuringUnit_ID = resultmeasruingunit.MUnit_ID;
                        product.Prod_Unet = resultmeasruingunit.MUnit_Descrip;
                    }
                    // create selectList from Database to entity Catger.
                    var resultCatger = db.Categories.Find(prodVM.SelectedCategory_ID);
                    if (resultCatger != null)
                    {
                        product.Category_ID = resultCatger.Cat_ID;
                        product.Prod_Category = resultCatger.Cat_Descrip;
                    }

                    var resultTax = db.Taxes.Find(prodVM.SelectedTaxId);
                    if (resultTax != null)
                    {
                        product.Tax_Id = resultTax.Tax_ID;
                    }
                    var resultTaxPurchas = db.Taxes.Find(prodVM.SelectedTax_Purch);
                    if (resultTaxPurchas != null)
                    {
                        product.Tax_Id_Purch = resultTaxPurchas.Tax_ID;
                    }
                    var lastSerial = db.Products.OrderByDescending(c => c.Prod_ID).FirstOrDefault();
                    if (lastSerial == null)
                    {
                        product.Prod_Code = "PRO-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        product.Prod_Code = "PRO-" + (Convert.ToInt32(lastSerial.Prod_Code.Substring(4, lastSerial.Prod_Code.Length - 4)) + 1).ToString("D3");
                    }
                    var barcheck = db.Products.Where(a => a.Bar_Code == prodVM.Bar_Code).FirstOrDefault();

                    if (prodVM.Bar_Code != ""&& prodVM.Bar_Code!=null&& barcheck==null)
                    {
                        product.Bar_Code = prodVM.Bar_Code;
                    }
                    else
                    {
                        product.Bar_Code = product.Prod_Code;
                    }
                   //var barcode= Utilites.QRcode.MakeBarcode(product.Bar_Code);
                    product.Validity = prodVM.Validity;
                    product.Notes = prodVM.Notess;
                    product.Demand_Limit = prodVM.Demand_Limit;
                    //product.Bar_Code_one = barcode;
                    if (prodVM.Pro_Img != null)
                    {
                        for (int i = 0; i < prodVM.Pro_Img.Count; i++)
                        {


                            var filedetails = new ProductFileDetails();
                            var length = prodVM.Pro_Img[i].InputStream.Length; //Length: 103050706
                            MemoryStream target = new MemoryStream();
                            prodVM.Pro_Img[i].InputStream.CopyTo(target); // generates problem in this line
                            byte[] data = target.ToArray();
                            filedetails.FileName = prodVM.Pro_Img[i].FileName;
                            filedetails.File_Type = prodVM.Pro_Img[i].ContentType;
                            filedetails.ContentLength = prodVM.Pro_Img[i].ContentLength;
                            filedetails.ProImg = data;
                            if (data.Length < 1000000)
                            {
                                if (i == 0)
                                {
                                    product.Pro_Img = data;
                                }
                                else
                                {
                                    product.ProductFileDetails.Add(filedetails);
                                }
                            }

                        }
                    }
                    if (prodVM.Pro_Img2 != null && prodVM.Pro_Img2.FirstOrDefault() != null)
                    {
                        foreach (var file in prodVM.Pro_Img2)
                        {
                            var filedetails = new ProductFileDetails();
                            var length = file.InputStream.Length; //Length: 103050706
                            MemoryStream target = new MemoryStream();
                            file.InputStream.CopyTo(target); // generates problem in this line
                            byte[] data = target.ToArray();
                            filedetails.FileName = file.FileName;
                            filedetails.File_Type = file.ContentType;
                            filedetails.ContentLength = file.ContentLength;
                            filedetails.ProImg = data;

                            if (data.Length < 1000000)
                            {
                                if (prodVM.Pro_Img2.Count() == 1)
                                {
                                    product.Pro_Img = data;
                                }
                                else
                                {
                                    product.ProductFileDetails.Add(filedetails);
                                }
                            }
                        }
                    }

                    product.Prod_Name = prodVM.Prod_Name;                   
                    product.Prod_Descrip = prodVM.Prod_Descrip;
                    product.Prod_Mark = prodVM.Prod_Mark;
                    product.Demand_Limit = prodVM.Demand_Limit;
                  
                    product.Sale_Price = prodVM.Sale_Price;
                    product.Prod_Cost_Price = prodVM.Prod_cost_Price;
                    product.Prod_Stored = prodVM.Prod_Stored;
                    product.Store_Id = prodVM.Store_Id;
                    product.Tax_Id = prodVM.SelectedTaxId;
                    product.Prod_Type= prodVM.Prod_Type;
                    var slicetempdata = new List<SlicesTem> { };

                    try
                    {

                         slicetempdata = db.SlicesTems.Where(s => s.ProductTemProd_ID == _ProductTemp.Prod_ID).ToList();
                    }
                    catch
                    {
                        slicetempdata = new List<SlicesTem> { };
                    }
                    if (slicetempdata.FirstOrDefault() != null)
                    {


                        foreach (var item in _ProductTemp.SlicesTems.ToList())
                        {
                            var sliceDetail = new SlicesProduct();
                            sliceDetail.Product_ID = item.ProductTemProd_ID;
                            sliceDetail.Slice_Price = item.Slice_Price;
                            sliceDetail.SlicesSlice_Id = item.SlicesSlice_Id;
                        
                            product.SlicesProducts.Add(sliceDetail);
                            db.SlicesTems.Remove(item);
                            db.SaveChanges();

                        }
                        

                    }
                    else
                    {
                        var sliceDetail = new SlicesProduct();
                        if (prodVM.SelectedSlieceID > 0&& prodVM.SelectedSlieceID!=null)
                        {
                           

                            sliceDetail.Slice_Price = prodVM.Sale_Price;
                            sliceDetail.SlicesSlice_Id = db.Slices.FirstOrDefault().Slice_Id;

                            
                        }
                        else
                        {
                            var sliceProductFromePartialView = db.Slices.FirstOrDefault();
                            sliceDetail.Slice_Price = prodVM.Sale_Price;
                            sliceDetail.SlicesSlice_Id = sliceProductFromePartialView.Slice_Id;
                        }
                        product.SlicesProducts.Add(sliceDetail);


                    }
                    var Copound = new List<ProductCompoundTemp>();
                    try
                    {

                        Copound = db.ProductCompoundTemps.Where(s => s.Master_Product_Id == _ProductTemp.Prod_ID).ToList();
                    }
                    catch
                    {

                        Copound = new List<ProductCompoundTemp>();
                    }

                        
                    if (Copound.FirstOrDefault() != null)
                    {


                        foreach (var item in Copound.ToList())
                        {
                            var compound = new ProductCompound();
                            compound.Product_Id = item.Product_Id;
                            compound.Quantity = item.Quantity;
                            

                            product.ProductCompounds.Add(compound);
                            db.ProductCompoundTemps.Remove(item);
                        
                            db.SaveChanges();
                            product.Prod_Type = Models.Prod_Type.مجمع;
                        }


                    }
                    if (Copound.Count() == 0 && product.Prod_Type == Models.Prod_Type.مجمع)
                    {
                        product.Prod_Type = Models.Prod_Type.منتج;
                    }
                    db.Products.Add(product);
                    if (_ProductTemp != null)
                    {
                        db.ProductTems.Remove(_ProductTemp);
                    }
                    db.SaveChanges();
                    if (product.Prod_Type == Models.Prod_Type.مجمع || product.Prod_Type == Models.Prod_Type.خدمة)
                    {
                        var AllStore = db.Stores.ToList(); ;
                        List<ProductBatches> batchList = new List<ProductBatches>();
                        foreach (var bat in AllStore)
                        {
                            var batch = new ProductBatches();
                            batch.Batch_Date = DateTime.Now;
                            batch.SalesPrice = product.Sale_Price;
                            batch.Product_ID = product.Prod_ID;
                            batch.Store_ID = bat.Store_ID;
                            batch.Product_Quantity = 1;
                            batch.IsActive = true;
                            batchList.Add(batch);
                        }
                        db.ProductBatches.AddRange(batchList);
                        db.SaveChanges();
                    }
                    db.Dispose();
                    return product.Prod_ID;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var result = ex.Data;
                var tt = result;
                throw;
            }

            return 0;



        }
        ////////////////////////// For Edit product
        public static SlicesProductVM PreparSliceProductDetailForEdit(int productId)
        {

            var db = new AccountingContainer();
            var _SliceProductVM = new SlicesProductVM();

            //var thisdetails = GetAllTempSliceProductDetailsForEdit2(productId).ToList().FirstOrDefault();

            //var slice = thisdetails.SlicesSlice_Id;

            //_SliceProductVM.Slice_Price = thisdetails.Slice_Price;
            //_SliceProductVM.SlicesProduct_Id = thisdetails.SlicesProduct_Id;
            //_SliceProductVM.SlicesSlice_Id = thisdetails.SlicesSlice_Id;
            //_SliceProductVM.Product_ID = productId;

            _SliceProductVM.MySliceList = new SelectList(db.Slices, "Slice_Id", "Slice_Name");

            //var _slice = db.Slices.Where(c => c.Slice_Id == slice);
            //if (_slice.Any())
            //{
            //    _SliceProductVM.SlicesSlice_Id = _slice.FirstOrDefault().Slice_Id;
            //}
            return _SliceProductVM;


        }

        public static List<SlicesProduct> GetAllTempSliceProductDetailsForEdit(int prodId)
        {
            var db = new AccountingContainer();
            var DetailsTemp = db.SlicesProducts.Where(slice => slice.Product_ID == prodId).ToList();
            return DetailsTemp;
        }

        public static List<SlicesProduct> GetAllTempSliceProductDetailsForEdit2(int prodId)
        {
            var db = new AccountingContainer();
            var DetailsTemp = db.SlicesProducts.Where(slice => slice.SlicesProduct_Id == prodId).ToList();
            return DetailsTemp;
        }
        public static List<SlicesTem> GetAllTempSlicesProductDetailsresult(int prodId)
        {
            var db = new AccountingContainer();
            var _sliceProductTemp = db.ProductTems.Find(prodId);
            return _sliceProductTemp.SlicesTems.ToList();
        }

        public static ProductVM fillForEditingProduct(int productId)
        {
            var db = new AccountingContainer();
            var prodvm = new ProductVM();
            var prodect = db.Products.Find(productId);
            prodvm.Validity = prodect.Validity;
            var productCate = prodect.Category_ID;
            var ProductMeasur = prodect.MeasuringUnit_ID;
            var Tax_Id = prodect.Tax_Id;
            var storeId = prodect.Store_Id;
            var Tax_Id_Purch = prodect.Tax_Id_Purch;

            //  select Categrorie from entity Categrorie.
            var Categrories = db.Categories.Where(c => c.Cat_ID == productCate);
            if (Categrories.Any())
            {
                prodvm.SelectedCategory_ID = Categrories.FirstOrDefault().Cat_ID;
            }
            //  select Measure from entity Measure.
            var Measure = db.MeasuringUnits.Where(c => c.MUnit_ID == ProductMeasur);
            if (Measure.Any())
            {
                prodvm.SelectedMeasuringUnit_ID = Measure.FirstOrDefault().MUnit_ID;
            }
            var Tax = db.Taxes.Where(c => c.Tax_ID == Tax_Id);
            if (Tax.Any())
            {
                prodvm.SelectedTaxId = Tax.FirstOrDefault().Tax_ID;
            }
            var Tax_Purc = db.Taxes.Where(c => c.Tax_ID == Tax_Id_Purch);
            if (Tax.Any())
            {
                prodvm.SelectedTax_Purch = Tax.FirstOrDefault().Tax_ID;
            }
            var store = db.Stores.Where(c => c.Store_ID == storeId);
            if (store.Any())
            {
                prodvm.Store_Id = store.FirstOrDefault().Store_ID;
            }
            prodvm.Prod_Type = prodect.Prod_Type;
            prodvm.Demand_Limit = prodect.Demand_Limit;
            prodvm.Prod_ID = prodect.Prod_ID;
            prodvm.Prod_Name = prodect.Prod_Name;
            prodvm.Prod_Code = prodect.Prod_Code;
            prodvm.Bar_Code = prodect.Bar_Code;
            prodvm.Prod_Descrip = prodect.Prod_Descrip;
            prodvm.Notess= prodect.Notes;
            if (prodect.Prod_Cost_Price == null)
            {
                prodvm.Prod_cost_Price = 0;
            }
            else
            {
                prodvm.Prod_cost_Price = prodect.Prod_Cost_Price.Value;
            }
            
            prodvm.Prod_Mark = prodect.Prod_Mark;
            prodvm.Demand_Limit = prodect.Demand_Limit;
           if( prodect.Sale_Price==null)
            {
                prodvm.Sale_Price = 0;
            }
            else
            {
                prodvm.Sale_Price = prodect.Sale_Price.Value;
            }
           
            prodvm.Prod_Stored = prodect.Prod_Stored;           
            prodvm.SelectedTaxId = prodect.Tax_Id;
            prodvm.SelectedTax_Purch = prodect.Tax_Id_Purch;


            prodvm.CategoryList = new SelectList(db.Categories, "Cat_ID", "Cat_Descrip");
            prodvm.MeasruingUnitList = new SelectList(db.MeasuringUnits, "MUnit_ID", "MUnit_Descrip");
           
            List<object> newList2 = new List<object>();
            var members2 = db.Taxes.Where(t => t.Tax_Code.StartsWith("020104001")).Where(a => a.IsActive != false).ToList();
            foreach (var member1 in members2)
                newList2.Add(new
                {
                    Id = member1.Tax_ID,
                    Name = member1.Tax_Name + "  % " + member1.Tax_Percentage
                });
            prodvm.MyTaxList = new SelectList(newList2, "Id", "Name");

            List<object> newList = new List<object>();
            var members = db.Taxes.Where(t => t.Tax_Code.StartsWith("010108001")).Where(a => a.IsActive != false).ToList();
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.Tax_ID,
                    Name = member.Tax_Name + "  % " + member.Tax_Percentage
                });
            prodvm.MyTaxListPurchas = new SelectList(newList, "Id", "Name");

            var ProductTemp = new ProductTem();
            ProductTemp.MeasuringUnit_ID = prodvm.SelectedMeasuringUnit_ID;
            ProductTemp.Category_ID = prodect.Prod_ID;
            ProductTemp.Tax_Id = prodvm.SelectedTaxId;
            ProductTemp.Store_Id = prodvm.Store_Id;
            ProductTemp.Prod_Descrip = prodvm.Prod_Descrip;
            ProductTemp.Prod_ID = prodect.Prod_ID;
            ProductTemp.Prod_Mark = prodect.Prod_Mark;
            ProductTemp.Prod_Code = prodect.Prod_Code;
            
            ProductTemp.Demand_Limit = prodect.Demand_Limit;
            ProductTemp.Prod_Category = prodect.Prod_Category;
            ProductTemp.Prod_Name = prodect.Prod_Name;
            ProductTemp.Prod_Stored = prodect.Prod_Stored;
            ProductTemp.Sale_Price = prodect.Sale_Price;
            ProductTemp.Prod_Unet = prodect.Prod_Unet;
            ProductTemp.Prod_Type = prodect.Prod_Type;
            ProductTemp.Prod_Cost_Price = prodect.Prod_Cost_Price;

            var ThereORN = db.ProductTems.Find(prodect.Prod_ID);
            if (ThereORN != null)
            {

            }
            else
            {
                ProductTemp.Prod_ID = prodect.Prod_ID;

                db.ProductTems.Add(ProductTemp);
            }
            db.SaveChanges();



            return prodvm;


        }
        public static int EditForProduct(ProductVM prodVM)
        {
            var db = new AccountingContainer();
            var product = db.Products.Find(prodVM.Prod_ID);
            var _ProductTemp = db.ProductTems.Find(prodVM.Prod_ID);
            try
            {
                if (prodVM != null)
                {
                    // create selectList from Database to entity MeasuringUnit.
                    var resultmeasruingunit = db.MeasuringUnits.Find(prodVM.SelectedMeasuringUnit_ID);
                    if (resultmeasruingunit != null)
                    {
                        product.MeasuringUnit_ID = resultmeasruingunit.MUnit_ID;
                        product.Prod_Unet = resultmeasruingunit.MUnit_Descrip;
                    }
                    else
                    {
                        product.MeasuringUnit_ID = 0;
                        product.Prod_Unet = null;
                    }
                    product.Validity = prodVM.Validity;
                    if (prodVM.Pro_Img != null)
                    {
                        for (int i = 0; i < prodVM.Pro_Img.Count; i++)
                        {


                            var filedetails = new ProductFileDetails();
                            var length = prodVM.Pro_Img[i].InputStream.Length; //Length: 103050706
                            MemoryStream target = new MemoryStream();
                            prodVM.Pro_Img[i].InputStream.CopyTo(target); // generates problem in this line
                            byte[] data = target.ToArray();
                            filedetails.FileName = prodVM.Pro_Img[i].FileName;
                            filedetails.File_Type = prodVM.Pro_Img[i].ContentType;
                            filedetails.ContentLength = prodVM.Pro_Img[i].ContentLength;
                            filedetails.ProImg = data;
                            if (data.Length < 1000000)
                            {
                                if (i == 0)
                                {
                                    product.Pro_Img = data;
                                }
                                else
                                {
                                    product.ProductFileDetails.Add(filedetails);
                                }
                            }

                        }
                    }
                    if (prodVM.Pro_Img2 != null && prodVM.Pro_Img2.FirstOrDefault() != null)
                    {
                        foreach (var file in prodVM.Pro_Img2)
                        {
                            var filedetails = new ProductFileDetails();
                            var length = file.InputStream.Length; //Length: 103050706
                            MemoryStream target = new MemoryStream();
                            file.InputStream.CopyTo(target); // generates problem in this line
                            byte[] data = target.ToArray();
                            filedetails.FileName = file.FileName;
                            filedetails.File_Type = file.ContentType;
                            filedetails.ContentLength = file.ContentLength;
                            filedetails.ProImg = data;

                            if (data.Length < 1000000)
                            {
                                if (prodVM.Pro_Img2.Count() == 1)
                                {
                                    product.Pro_Img = data;
                                }
                                else
                                {
                                    product.ProductFileDetails.Add(filedetails);
                                }
                            }
                        }
                    }
                    // create selectList from Database to entity Catger.
                    var resultCatger = db.Categories.Find(prodVM.SelectedCategory_ID);
                    if (resultCatger != null)
                    {
                        product.Category_ID = resultCatger.Cat_ID;
                        product.Prod_Category = resultCatger.Cat_Descrip;
                    }
                    else
                    {
                        product.Category_ID = 0;
                        product.Prod_Category = null;
                    }

                  
                   
                        product.Tax_Id = prodVM.SelectedTaxId;
                    
                 
                        product.Tax_Id_Purch = prodVM.SelectedTax_Purch;

                    product.Demand_Limit = prodVM.Demand_Limit;
                    product.Prod_Name = prodVM.Prod_Name;
                    product.Prod_Descrip = prodVM.Prod_Descrip;
                    product.Prod_Mark = prodVM.Prod_Mark;
                    //product.Demand_Limit = prodVM.Demand_Limit;
                    product.Notes = prodVM.Notess;
                    //product.Sale_Price = prodVM.Sale_Price;
                    //product.Prod_Cost_Price = prodVM.Prod_cost_Price;
                    product.Prod_Stored = prodVM.Prod_Stored;
                    product.Store_Id = prodVM.Store_Id;
                    product.Tax_Id = prodVM.SelectedTaxId;
                  
                    var barcheck = db.Products.Where(a => a.Bar_Code == prodVM.Bar_Code).FirstOrDefault();
                    if (prodVM.Bar_Code != product.Bar_Code&& prodVM.Bar_Code!=null&& prodVM.Bar_Code!=""&& barcheck==null)
                    {
                        product.Bar_Code = prodVM.Bar_Code;
                        //var barcode = Utilites.QRcode.MakeBarcode(product.Bar_Code);



                        //product.Bar_Code_one = barcode;
                    }
                    
                    

                    var oldDetail = db.SlicesProducts.Where(a => a.Product_ID == product.Prod_ID).ToList();
                    var detailListTemp = db.SlicesTems.Where(e => e.ProductTemProd_ID == _ProductTemp.Prod_ID).ToList();
                    List<SlicesTem> ListTempRemove = new List<SlicesTem> { };
                    foreach (var detail in detailListTemp)
                    {
                        var item = new SlicesProduct();
                      
                        item.Slice_Price = detail.Slice_Price;
                        item.SlicesSlice_Id = detail.SlicesSlice_Id;
                        item.Product_ID = product.Prod_ID;
                        db.SlicesProducts.Add(item);
                        
                        ListTempRemove.Add(detail);
                        //db.SlicesTems.Remove(detail);
                        //db.SaveChanges();
                    }




                    db.Entry(product).State = EntityState.Modified;
                
                    db.SlicesTems.RemoveRange(ListTempRemove);
                
                    db.ProductTems.Remove(_ProductTemp);
                  
                    db.SlicesProducts.RemoveRange(oldDetail);
                    
                    db.SaveChanges();
                   
                    db.Dispose();
                    return product.Prod_ID;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var result = ex.Data;
                var tt = result;
                throw;
            }

            return 0;


        }
       // use this method to Delete Product and save DataBase.
        public static string Deleteprode(int id)
        {

            var db = new AccountingContainer();
            var prd = db.Products.Find(id);
            var card = db.StoreCards.Where(a => a.Product_Id == prd.Prod_ID).FirstOrDefault();
            if (card != null)
            {
                return "يوجد حركات لهذا المنتج لايمكنك حذفه";
            }
            
            if (prd != null)
            {

                db.SlicesProducts.RemoveRange(prd.SlicesProducts);
                db.Products.Remove(prd);
                if (prd.Prod_Type ==Models.Prod_Type.خدمة)
                {
                    var batch = db.ProductBatches.Where(a => a.Product_ID == prd.Prod_ID).FirstOrDefault();

                    db.ProductBatches.Remove(batch);
                    db.SaveChanges();
                }
                db.SaveChanges();
               
                db.Dispose();
                return "تم حذف المنتج بنجاح";
            }

            return "يوجد حركات لهذا المنتج لايمكنك حذفه";

        }
        // Create open balance for new product
        public static List<SlicesTem> GetAllTempSliceProductDetailsForOpen(int productVmId)
        {
            var db = new AccountingContainer();

            var _SlicesTem = db.SlicesTems.Where(slice => slice.ProductTemProd_ID == productVmId).ToList();
            return _SlicesTem;
        }
        public static SlicesProductVM PrepareSliceProductDetailForOpen()
        {
            var db = new AccountingContainer();
            var _SliceProductVm = new SlicesProductVM();

            _SliceProductVm.MySliceList = new SelectList(db.Slices, "Slice_Id", "Slice_Name");
            return _SliceProductVm;

        }
        public static string AddNewSliceDetailsTempForOpen(SlicesProductVM slicesDetail)
        {
            var db = new AccountingContainer();
            var sliceTemp = db.ProductTems.Where(e => e.Category_ID == slicesDetail.Product_ID).ToList().FirstOrDefault();
            var sliceDetailsTemp = new SlicesTem();
            sliceDetailsTemp.ProductTemProd_ID = slicesDetail.Product_ID;
            sliceDetailsTemp.SlicesSlice_Id = slicesDetail.SlicesSlice_Id;
            var slice_Name = db.Slices.Where(s => s.Slice_Id == slicesDetail.SlicesSlice_Id).FirstOrDefault().Slice_Name;
            sliceDetailsTemp.Slice_Name = slice_Name;
            sliceDetailsTemp.Slice_Price = slicesDetail.Slice_Price;


            sliceTemp.SlicesTems.Add(sliceDetailsTemp);

            db.Entry(sliceTemp).State = EntityState.Modified;
            db.SaveChanges();

            return "تم اضافة الشريحة بنجاح";
        }
        public static SlicesTem DeleteSliceFromTempForOpen(int sliceId)
        {
            var db = new AccountingContainer();
            SlicesTem vm = db.SlicesTems.Find(sliceId);
            db.SlicesTems.Remove(vm);
            db.SaveChanges();
            db.Dispose();
            return vm;
        }
        //public static ProductOpenBalanceVM CreateVMForCreationForOpen()
        //{
        //    var db = new AccountingContainer();
        //    var prodectvm = new ProductOpenBalanceVM();          
        //    //prodectvm.CategoryList = new SelectList(db.Categories, "Cat_ID", "Cat_Descrip");
        //    //prodectvm.MeasruingUnitList = new SelectList(db.MeasuringUnits, "MUnit_ID", "MUnit_Descrip");
           
        //    prodectvm.StoreList = new SelectList(db.Stores, "Store_ID", "Store_Name");
        //    prodectvm.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
        //    prodectvm.CompList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
        //    prodectvm.ProductList = new SelectList(db.Products.Where(p=>p.Prod_Type != Models.Prod_Type.خدمة), "Prod_ID", "Prod_Name");
           
        //    var lastCodebalance = db.Products.OrderByDescending(c => c.Prod_ID).Where(e => e.OpenBalance_Code != null).FirstOrDefault();
        //    if (lastCodebalance == null)
        //    {
        //        prodectvm.OpenBalance_Code = "OPP-001";
        //    }
        //    else
        //    {
        //        //using string substring method to get the number of the last inserted
        //        prodectvm.OpenBalance_Code = "OPP-" + (Convert.ToInt32(lastCodebalance.OpenBalance_Code.Substring(4, lastCodebalance.OpenBalance_Code.Length - 4)) + 1).ToString("D3");
        //    }
          
        //    //List<object> newList2 = new List<object>();
        //    //var members2 = db.Taxes.Where(t => t.Tax_Code.StartsWith("020104001")).ToList();
        //    //foreach (var member1 in members2)
        //    //    newList2.Add(new
        //    //    {
        //    //        Id = member1.Tax_ID,
        //    //        Name = member1.Tax_Name + "  % " + member1.Tax_Percentage
        //    //    });
        //    //prodectvm.MyTaxList = new SelectList(newList2, "Id", "Name");
        //    //List<object> newList = new List<object>();
        //    //var members = db.Taxes.Where(t => t.Tax_Code.StartsWith("010108001")).ToList();
        //    //foreach (var member in members)
        //    //    newList.Add(new
        //    //    {
        //    //        Id = member.Tax_ID,
        //    //        Name = member.Tax_Name + "  % " + member.Tax_Percentage
        //    //    });
        //    //prodectvm.MyTaxListPurchas = new SelectList(newList, "Id", "Name");


        //    //var productTemp = new ProductTem();
        //    //var lastID = (from s in db.ProductTems
        //    //              select s).ToList().LastOrDefault();
        //    //if (lastID == null)
        //    //{ productTemp.Prod_ID = 1; }
        //    //else
        //    //{ productTemp.Prod_ID = lastID.Prod_ID + 1; }

        //    //db.ProductTems.Add(productTemp);
        //    //db.SaveChanges();

        //    //prodectvm.Prod_ID = productTemp.Prod_ID;

        //    return prodectvm;
        //}

       //public static string CreateProdectForOpen(ProductOpenBalanceVM prodVM)
       // {
       //     var db = new AccountingContainer();
       //     var product = new Product();
       //     var batch = new ProductBatches();
       //     var _ProductTemp = db.ProductTems.Find(prodVM.Prod_ID);

       //     try
       //     {
       //         if (prodVM != null)
       //         {
       //             // create selectList from Database to entity MeasuringUnit.
       //             var resultmeasruingunit = db.MeasuringUnits.Find(prodVM.SelectedMeasuringUnit_ID);
       //             if (resultmeasruingunit != null)
       //             {
       //                 product.MeasuringUnit_ID = resultmeasruingunit.MUnit_ID;
       //                 product.Prod_Unet = resultmeasruingunit.MUnit_Descrip;
       //             }
       //             // create selectList from Database to entity Catger.
       //             var resultCatger = db.Categories.Find(prodVM.SelectedCategory_ID);
       //             if (resultCatger != null)
       //             {
       //                 product.Category_ID = resultCatger.Cat_ID;
       //                 product.Prod_Category = resultCatger.Cat_Descrip;
       //             }

       //             var resultTax = db.Taxes.Find(prodVM.SelectedTaxId);
       //             if (resultTax != null)
       //             {
       //                 product.Tax_Id = resultTax.Tax_ID;
       //             }
       //             var resultTaxpurch = db.Taxes.Find(prodVM.SelectedTax_Purch);
       //             if (resultTaxpurch != null)
       //             {
       //                 product.Tax_Id_Purch = resultTaxpurch.Tax_ID;
       //             }
       //             var resultstore = db.Stores.Find(prodVM.SelectedStore_ID);
       //             if (resultTax != null)
       //             {
       //                 product.Store_Id = resultstore.Store_ID;
       //             }

       //             var lastSerial = db.Products.OrderByDescending(c => c.Prod_ID).FirstOrDefault();
       //             if (lastSerial == null)
       //             {
       //                 product.Prod_Code = "PRO-001";
       //             }
       //             else
       //             {
       //                 //using string substring method to get the number of the last inserted
       //                 product.Prod_Code = "PRO-" + (Convert.ToInt32(lastSerial.Prod_Code.Substring(4, lastSerial.Prod_Code.Length - 4)) + 1).ToString("D3");
       //             }


       //             var lastCodebalance = db.Products.OrderByDescending(c => c.Prod_ID).Where(e => e.OpenBalance_Code != null).FirstOrDefault();
       //             if (lastCodebalance == null)
       //             {
       //                 product.OpenBalance_Code = "OPP-001";
       //             }
       //             else
       //             {
       //                 //using string substring method to get the number of the last inserted
       //                 product.OpenBalance_Code = "OPP-" + (Convert.ToInt32(lastCodebalance.OpenBalance_Code.Substring(4, lastCodebalance.OpenBalance_Code.Length - 4)) + 1).ToString("D3");
       //             }

       //             product.Prod_Name = prodVM.Prod_Name;
       //             product.Prod_Descrip = prodVM.Prod_Descrip;
       //             product.Prod_Mark = prodVM.Prod_Mark;
       //             product.Demand_Limit = prodVM.Demand_Limit;
       //             product.Bar_Code = prodVM.Bar_Code;
       //             product.Sale_Price = prodVM.Sale_Price;                    
       //             product.Prod_Stored = prodVM.Prod_Stored;
       //             product.Store_Id = product.Store_Id;
       //             product.Tax_Id = prodVM.SelectedTaxId;
       //             product.Prod_Type = prodVM.Prod_Type;
               

       //             var slicetempdata = db.SlicesTems.Where(s => s.ProductTemProd_ID == _ProductTemp.Prod_ID).FirstOrDefault();
       //             if (slicetempdata != null)
       //             {


       //                 foreach (var item in _ProductTemp.SlicesTems.ToList())
       //                 {
       //                     var sliceDetail = new SlicesProduct();
       //                     sliceDetail.Product_ID = item.ProductTemProd_ID;
       //                     sliceDetail.Slice_Price = item.Slice_Price;
       //                     sliceDetail.SlicesSlice_Id = item.SlicesSlice_Id;

       //                     product.SlicesProducts.Add(sliceDetail);
       //                     db.SlicesTems.Remove(item);
       //                     db.SaveChanges();

       //                 }


       //             }
       //             else
       //             {
       //                 var sliceDetail = new SlicesProduct();
       //                 sliceDetail.Product_ID = _ProductTemp.Prod_ID;
       //                 sliceDetail.Slice_Price = prodVM.Sale_Price.Value;
       //                 sliceDetail.SlicesSlice_Id = db.Slices.FirstOrDefault().Slice_Id;

       //                 product.SlicesProducts.Add(sliceDetail);


       //             }

       //             db.Products.Add(product);
       //             db.ProductTems.Remove(_ProductTemp);
       //             db.SaveChanges();



       //             //////////////////
       //             GLMaster glHeader = new GLMaster();
       //             if (Convert.ToDateTime(prodVM.Batch_Date).Year < 1990)
       //             { glHeader.GL_Date = DateTime.Now; }
       //             else
       //             { glHeader.GL_Date = Convert.ToDateTime(prodVM.Batch_Date); }
                   
       //             glHeader.GL_Desc = "رصيد افتتاحي اصناف";
       //             glHeader.Company_Id = prodVM.Selectedcomp_ID;
       //             glHeader.GLType_Id = "قيد افتتاحى";
       //             var cur = db.Currencies.Find(prodVM.SelectedCurrency_ID);
       //             glHeader.Currency_Id = cur.Currency_Type;
       //             // Initialize GLState to Pending when first created
       //             glHeader.GLState = TransactionState.Pending;
       //             glHeader.GLCode = prodVM.OpenBalance_Code;
       //             glHeader.Auto = true;
       //             GLDetail AccountsPayable = new GLDetail();
       //             AccountsPayable.GLDetail_Credit = 0;
       //             AccountsPayable.GLDetail_Debit = ((decimal)prodVM.Product_Cost) * prodVM.Product_Quantity;
       //             var store = db.Stores.Find(prodVM.SelectedStore_ID);
       //             var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).First();

       //             AccountsPayable.Account_Id = ChartOfAccounInventory.Account_Id;
       //             AccountsPayable.Account_Name = ChartOfAccounInventory.Account_Name;
       //             AccountsPayable.Bu_Id = 0;
       //             AccountsPayable.SubLedger_Id = prodVM.SelectedStore_ID;
       //             AccountsPayable.Asset_Id = 0;
       //             glHeader.GLDetails.Add(AccountsPayable);
       //             GLDetail wsite = new GLDetail();
       //             wsite.GLDetail_Credit = ((decimal)prodVM.Product_Cost) * prodVM.Product_Quantity; ;
       //             wsite.GLDetail_Debit = 0;

       //             var ChartOfwsite = db.ChartOfAccountings.Where(s => s.Account_Code == "0601" && s.Account_Level == 2 && s.ChartOfAccountingAccount_Id == 130).ToList().LastOrDefault();

       //             wsite.Account_Id = ChartOfwsite.Account_Id;
       //             wsite.Account_Name = ChartOfwsite.Account_Name;
       //             wsite.Bu_Id = 0;
       //             wsite.SubLedger_Id = prodVM.SelectedStore_ID;
       //             wsite.Asset_Id = 0;
       //             glHeader.GLDetails.Add(wsite);
       //             db.GLMasters.Add(glHeader);
       //             GLMaster.GetBalanceForAccount(glHeader);
       //             GLMaster.GetBalanceForSubLedger(glHeader);
       //             db.SaveChanges();
       //             ////////////////////////
       //             batch.Product_ID = product.Prod_ID;
       //             batch.Store_ID = Convert.ToInt32(prodVM.SelectedStore_ID);
       //             batch.Product_Quantity = prodVM.Product_Quantity;
       //             batch.Product_Cost = prodVM.Product_Cost;
       //             if (Convert.ToDateTime(prodVM.Batch_Date).Year < 1990)
       //             { batch.Batch_Date = null; }
       //             else
       //             { batch.Batch_Date = Convert.ToDateTime(prodVM.Batch_Date); }
       //             if (Convert.ToDateTime(prodVM.Batch_ExpireDate).Year < 1990)
       //             { batch.Batch_ExpireDate = null; }
       //             else
       //             { batch.Batch_ExpireDate = Convert.ToDateTime(prodVM.Batch_ExpireDate); }
                    
                   
       //             batch.Batch_Cost = prodVM.Product_Cost*prodVM.Product_Quantity;
       //             db.ProductBatches.Add(batch);
       //             try
       //             { db.SaveChanges(); }
       //             catch (DbEntityValidationException ex)
       //             {
       //                 foreach (var entityValidationErrors in ex.EntityValidationErrors)
       //                 {
       //                     foreach (var validationError in entityValidationErrors.ValidationErrors)
       //                     {
       //                         var x = validationError.PropertyName;
       //                         var y = validationError.ErrorMessage;
       //                     }
       //                 }
       //             }
       //             db.Dispose();
       //             return "تم حفظ الصنف";
       //         }
       //     }
       //     catch (Exception ex)
       //     {
       //         var msg = ex.Message;
       //         var result = ex.Data;
       //         var tt = result;
       //         throw;
       //     }

       //     return "Failed";
       // }
        public static string CreateProdectForOpen(ProductOpenBalanceVM prodVM)
        {
            var db = new AccountingContainer();
            var product = new Product();
            var batch = new ProductBatches();
            var _ProductTemp = db.ProductTems.Find(prodVM.Prod_ID);

            try
            {
                if (prodVM != null)
                {
                    // create selectList from Database to entity MeasuringUnit.
                    var resultmeasruingunit = db.MeasuringUnits.Find(prodVM.SelectedMeasuringUnit_ID);
                    if (resultmeasruingunit != null)
                    {
                        product.MeasuringUnit_ID = resultmeasruingunit.MUnit_ID;
                        product.Prod_Unet = resultmeasruingunit.MUnit_Descrip;
                    }
                    // create selectList from Database to entity Catger.
                    var resultCatger = db.Categories.Find(prodVM.SelectedCategory_ID);
                    if (resultCatger != null)
                    {
                        product.Category_ID = resultCatger.Cat_ID;
                        product.Prod_Category = resultCatger.Cat_Descrip;
                    }

                    var resultTax = db.Taxes.Find(prodVM.SelectedTaxId);
                    if (resultTax != null)
                    {
                        product.Tax_Id = resultTax.Tax_ID;
                    }
                    var resultTaxpurch = db.Taxes.Find(prodVM.SelectedTax_Purch);
                    if (resultTaxpurch != null)
                    {
                        product.Tax_Id_Purch = resultTaxpurch.Tax_ID;
                    }
                    var resultstore = db.Stores.Find(prodVM.SelectedStore_ID);
                    if (resultTax != null)
                    {
                        product.Store_Id = resultstore.Store_ID;
                    }

                    var lastSerial = db.Products.OrderByDescending(c => c.Prod_ID).FirstOrDefault();
                    if (lastSerial == null)
                    {
                        product.Prod_Code = "PRO-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        product.Prod_Code = "PRO-" + (Convert.ToInt32(lastSerial.Prod_Code.Substring(4, lastSerial.Prod_Code.Length - 4)) + 1).ToString("D3");
                    }


                    var lastCodebalance = db.Products.OrderByDescending(c => c.Prod_ID).Where(e => e.OpenBalance_Code != null).FirstOrDefault();
                    if (lastCodebalance == null)
                    {
                        product.OpenBalance_Code = "OPP-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        product.OpenBalance_Code = "OPP-" + (Convert.ToInt32(lastCodebalance.OpenBalance_Code.Substring(4, lastCodebalance.OpenBalance_Code.Length - 4)) + 1).ToString("D3");
                    }

                    product.Prod_Name = prodVM.Prod_Name;
                    product.Prod_Descrip = prodVM.Prod_Descrip;
                    product.Prod_Mark = prodVM.Prod_Mark;
                    product.Demand_Limit = prodVM.Demand_Limit;
                   
                    product.Sale_Price = prodVM.Sale_Price;
                    product.Prod_Stored = prodVM.Prod_Stored;
                    product.Store_Id = product.Store_Id;
                    product.Tax_Id = prodVM.SelectedTaxId;
                    product.Prod_Type = prodVM.Prod_Type;


                    //var slicetempdata = db.SlicesTems.Where(s => s.ProductTemProd_ID == _ProductTemp.Prod_ID).FirstOrDefault();
                    //if (slicetempdata != null)
                    //{


                    //    foreach (var item in _ProductTemp.SlicesTems.ToList())
                    //    {
                    //        var sliceDetail = new SlicesProduct();
                    //        sliceDetail.Product_ID = item.ProductTemProd_ID;
                    //        sliceDetail.Slice_Price = item.Slice_Price;
                    //        sliceDetail.SlicesSlice_Id = item.SlicesSlice_Id;

                    //        product.SlicesProducts.Add(sliceDetail);
                    //        db.SlicesTems.Remove(item);
                    //        db.SaveChanges();

                    //    }


                    //}
                    //else
                    //{
                    //    var sliceDetail = new SlicesProduct();
                    //    sliceDetail.Product_ID = _ProductTemp.Prod_ID;
                    //    sliceDetail.Slice_Price = prodVM.Sale_Price.Value;
                    //    sliceDetail.SlicesSlice_Id = db.Slices.FirstOrDefault().Slice_Id;

                    //    product.SlicesProducts.Add(sliceDetail);


                    //}

                    //db.Products.Add(product);
                    //db.ProductTems.Remove(_ProductTemp);
                    //db.SaveChanges();



                    //////////////////
                    GLMaster glHeader = new GLMaster();
                    if (Convert.ToDateTime(prodVM.Batch_Date).Year < 1990)
                    { glHeader.GL_Date = DateTime.Now; }
                    else
                    { glHeader.GL_Date = Convert.ToDateTime(prodVM.Batch_Date); }

                    glHeader.GL_Desc = "رصيد افتتاحي اصناف";
                    glHeader.Company_Id = prodVM.Selectedcomp_ID;
                    glHeader.GLType_Id = "قيد افتتاحى";
                    var cur = db.Currencies.Find(prodVM.SelectedCurrency_ID);
                    glHeader.Currency_Id = cur.Currency_Type;
                    // Initialize GLState to Pending when first created
                    glHeader.GLState = TransactionState.Pending;
                    glHeader.GLCode = prodVM.OpenBalance_Code;
                    glHeader.Auto = true;
                    GLDetail AccountsPayable = new GLDetail();
                    AccountsPayable.GLDetail_Credit = 0;
                    AccountsPayable.GLDetail_Debit = ((decimal)prodVM.Product_Cost) * prodVM.Product_Quantity;
                    var store = db.Stores.Find(prodVM.SelectedStore_ID);
                    var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).First();

                    AccountsPayable.Account_Id = ChartOfAccounInventory.Account_Id;
                    AccountsPayable.Account_Name = ChartOfAccounInventory.Account_Name;
                    AccountsPayable.Bu_Id = 0;
                    AccountsPayable.SubLedger_Id = prodVM.SelectedStore_ID;
                    AccountsPayable.Asset_Id = 0;
                    glHeader.GLDetails.Add(AccountsPayable);
                    GLDetail wsite = new GLDetail();
                    wsite.GLDetail_Credit = ((decimal)prodVM.Product_Cost) * prodVM.Product_Quantity; ;
                    wsite.GLDetail_Debit = 0;

                    var ChartOfwsite = db.ChartOfAccountings.Where(s => s.Account_Code == "0601" && s.Account_Level == 2 && s.ChartOfAccountingAccount_Id == 130).ToList().LastOrDefault();

                    wsite.Account_Id = ChartOfwsite.Account_Id;
                    wsite.Account_Name = ChartOfwsite.Account_Name;
                    wsite.Bu_Id = 0;
                    wsite.SubLedger_Id = prodVM.SelectedStore_ID;
                    wsite.Asset_Id = 0;
                    glHeader.GLDetails.Add(wsite);
                    db.GLMasters.Add(glHeader);
                    GLMaster.GetBalanceForAccount(glHeader);
                    GLMaster.GetBalanceForSubLedger(glHeader);
                    db.SaveChanges();
                    ////////////////////////
                    batch.Product_ID = product.Prod_ID;
                    batch.Store_ID = Convert.ToInt32(prodVM.SelectedStore_ID);
                    batch.Product_Quantity = prodVM.Product_Quantity;
                    batch.Product_Cost = prodVM.Product_Cost;
                    if (Convert.ToDateTime(prodVM.Batch_Date).Year < 1990)
                    { batch.Batch_Date = null; }
                    else
                    { batch.Batch_Date = Convert.ToDateTime(prodVM.Batch_Date); }
                    if (Convert.ToDateTime(prodVM.Batch_ExpireDate).Year < 1990)
                    { batch.Batch_ExpireDate = null; }
                    else
                    { batch.Batch_ExpireDate = Convert.ToDateTime(prodVM.Batch_ExpireDate); }


                    batch.Batch_Cost = prodVM.Product_Cost * prodVM.Product_Quantity;
                    db.ProductBatches.Add(batch);
                 


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
                    db.Dispose();
                    return "تم حفظ الصنف";
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var result = ex.Data;
                var tt = result;
                throw;
            }

            return "Failed";
        }
      
    
        public static IEnumerable<SelectListItem> GetCategoryList()
        {
            var db = new AccountingContainer();
            ProductVM vm = new ProductVM();

           
            var cat = from prodc in db.Categories
                       select new SelectListItem
                       {
                           Text = prodc.Cat_Descrip ,
                           Value = prodc.Cat_ID.ToString()
                       };

            return cat;
        }

        public static IEnumerable<SelectListItem> GetMeasurList()
        {
            var db = new AccountingContainer();
         
            var Meature = from prodc in db.MeasuringUnits
                      select new SelectListItem
                      {
                          Text = prodc.MUnit_Descrip,
                          Value = prodc.MUnit_ID.ToString()
                      };

            return Meature;
        }
        public static IEnumerable<SelectListItem> GetSliceList()
        {
            var db = new AccountingContainer();

            var Slice = from prodc in db.Slices
                          select new SelectListItem
                          {
                              Text = prodc.Slice_Name,
                              Value = prodc.Slice_Id.ToString()
                          };

            return Slice;
        }
        public static List<Product> getAllproductFordrobDownList()
        {
            var db = new AccountingContainer();

            var lista = db.Products.ToList();

            return lista;




        }
        public static List<ProductBatches> getAllproductbatchFordrobDownList()
        {
            var db = new AccountingContainer();

            var lista = db.ProductBatches.ToList();
             
            return lista;




        }

        public static String Import(HttpPostedFileBase excelfile)
        {
            var data = getdataFillter(excelfile);
            List<Product> ProductList = new List<Product>();
            if (data.Item2.Count() == 0)
            {
                foreach (var item in data.Item1)
                {
                    var Product = CreateProdectfromFile(item);
                    ProductList.Add(Product);

                }
               //var ProductFilterList = ProductList.Where(a => a.Store_Id != 0 && a.Quantity != 0).ToList();

               // if (ProductFilterList.Count()!=0)
               // {
               //     Utilites.OpenBalanceFile.MakeOpenBallanceForGroupOFProduct(ProductFilterList);
               // }
               

                return "تم حفظ الملف بنجاح";

            }
            string combindedString = string.Join(" ,", data.Item2.ToArray());
            return "يوجد اخطاء فى الاسطر التالية" + combindedString;



        }
        public static Tuple<List<ProductVM>, List<String>> getdataFillter(HttpPostedFileBase excelfile)
        {
            var ProductList = new List<ProductVM> { };
            var messageList = new List<String> { };
            var db = new AccountingContainer();
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return Tuple.Create(ProductList, messageList);

            }

            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    //string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + excelfile.FileName);
                    var nameOfFile = "Product" + DateTime.Now.ToBinary() + ".xlsx";
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + nameOfFile);
                    //if (System.IO.File.Exists(path))
                    //{


                    //    File.Delete(path);




                    //}

                    excelfile.SaveAs(path);
                    // read data from excel file

                    Application application = new Application();
                    Workbook workbook = application.Workbooks.Open(path);
                    Worksheet worksheet = workbook.ActiveSheet;
                    Range range = worksheet.UsedRange;
                    for (int row = 3; row <= range.Rows.Count; row++)
                    {
                        ProductVM prodvm = new ProductVM();
                        // cust.Customer_Id = int.Parse(((Range)range.Cells[row, 1]).Text);
                        prodvm.Prod_Name = ((Range)range.Cells[row, 1]).Text+" "+ ((Range)range.Cells[row, 11]).Text;
                        if (prodvm.Prod_Name == "")
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }
                        try
                        {
                            var productcost = ((Range)range.Cells[row, 2]).Text;
                            var salePrice = ((Range)range.Cells[row, 3]).Text;
                            var measureunit = ((Range)range.Cells[row, 5]).Text;
                            var sliece = ((Range)range.Cells[row, 6]).Text;
                            var catigory= ((Range)range.Cells[row, 9]).Text;
                            var productType= ((Range)range.Cells[row, 10]).Text;
                            

                            if (productcost=="")
                            {
                                productcost = 0;
                            }
                            if (salePrice == "")
                            {
                                salePrice = 0;
                            }
                            
                            if(measureunit=="")
                            {
                                measureunit = 0;
                            }
                            if (catigory == "")
                            {
                                catigory = 0;
                            }
                           


                            prodvm.Prod_cost_Price = Convert.ToDecimal(productcost);
                            prodvm.Sale_Price = Convert.ToDecimal(salePrice);
                            prodvm.SelectedMeasuringUnit_ID = Convert.ToInt32(measureunit);
                            prodvm.Slieces_Id = Convert.ToInt32(sliece);
                            prodvm.SelectedCategory_ID = Convert.ToInt32(catigory);
                            var prodType= Convert.ToInt32(productType);
                            
                           
                            //if ((prodvm.Prod_cost_Price <=0 )&&( prodType != 2))
                            //{
                            //    messageList.Add(row.ToString());
                            //    continue;

                            //}
                            //if (( prodvm.Sale_Price < 0) && (prodType != 2))
                            //{
                            //    messageList.Add(row.ToString());
                            //    continue;

                            //}
                            //if (( prodvm.quantity < 0) && (prodType != 2))
                            //{
                            //    messageList.Add(row.ToString());
                            //    continue;

                            //}


                            if (prodvm.SelectedMeasuringUnit_ID != 0) {
                                var istrueMeatureUnit = db.MeasuringUnits.Find(prodvm.SelectedMeasuringUnit_ID).MUnit_ID;
                            }
                            if (prodvm.SelectedCategory_ID != 0)
                            {
                                var istrueMeatureUnit = db.Categories.Find(prodvm.SelectedCategory_ID).Cat_ID;
                            }
                            if (prodvm.Store_Id != 0)
                            {
                                var istrueStore = db.Stores.Find(prodvm.Store_Id).Store_ID;
                            }
                            if (prodType == 1)
                            {
                                prodvm.Prod_Type = Models.Prod_Type.منتج;

                            }
                            else
                            {
                                prodvm.Prod_Type = Models.Prod_Type.خدمة;
                                prodvm.Store_Id = 0;
                            }

                            var isttrueslices = db.Slices.Find(prodvm.Slieces_Id).Slice_Id;
                           


                        }
                        catch
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }

                        prodvm.Prod_Descrip = ((Range)range.Cells[row, 4]).Text+ " "+ ((Range)range.Cells[row, 12]).Text;
                        prodvm.Bar_Code= ((Range)range.Cells[row, 7]).Text;
                        prodvm.Prod_Mark = ((Range)range.Cells[row, 8]).Text;
                       


                        ProductList.Add(prodvm);

                    }

                    workbook.Save();
                    workbook.Close(path);

                 //   File.Delete(path);

                }
            }
            return Tuple.Create(ProductList, messageList);
        }
        public static Product CreateProdectfromFile(ProductVM prodVM)
        {
            var db = new AccountingContainer();
            var product = new Product();
           

            try
            {
                if (prodVM != null)
                {
                    // create selectList from Database to entity MeasuringUnit.
                    var resultmeasruingunit = db.MeasuringUnits.Find(prodVM.SelectedMeasuringUnit_ID);
                    if (resultmeasruingunit != null)
                    {
                        product.MeasuringUnit_ID = resultmeasruingunit.MUnit_ID;
                        product.Prod_Unet = resultmeasruingunit.MUnit_Descrip;
                    }
                    // create selectList from Database to entity Catger.
                    var resultCatger = db.Categories.Find(prodVM.SelectedCategory_ID);
                    if (resultCatger != null)
                    {
                        product.Category_ID = resultCatger.Cat_ID;
                        product.Prod_Category = resultCatger.Cat_Descrip;
                    }

                    var resultTax = db.Taxes.Find(prodVM.SelectedTaxId);
                    if (resultTax != null)
                    {
                        product.Tax_Id = resultTax.Tax_ID;
                    }
                    var resultTaxPurchas = db.Taxes.Find(prodVM.SelectedTax_Purch);
                    if (resultTaxPurchas != null)
                    {
                        product.Tax_Id_Purch = resultTaxPurchas.Tax_ID;
                    }
                    var lastSerial = db.Products.OrderByDescending(c => c.Prod_ID).FirstOrDefault();
                    if (lastSerial == null)
                    {
                        product.Prod_Code = "PRO-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        product.Prod_Code = "PRO-" + (Convert.ToInt32(lastSerial.Prod_Code.Substring(4, lastSerial.Prod_Code.Length - 4)) + 1).ToString("D3");
                    }
                   
                    var barcheck = db.Products.Where(a => a.Bar_Code == prodVM.Bar_Code).FirstOrDefault();

                    if (prodVM.Bar_Code != "" && prodVM.Bar_Code != null && barcheck == null)
                    {
                        product.Bar_Code = prodVM.Bar_Code;
                    }
                    else
                    {
                        product.Bar_Code = product.Prod_Code;
                    }
                    product.Bar_Code_one = Utilites.QRcode.MakeBarcode(product.Bar_Code);
                    product.Prod_Name = prodVM.Prod_Name;
                    product.Prod_Descrip = prodVM.Prod_Descrip;
                    product.Prod_Mark = prodVM.Prod_Mark;
                    product.Demand_Limit = prodVM.Demand_Limit;
                    
                    product.Sale_Price = prodVM.Sale_Price;
                    product.Prod_Cost_Price = prodVM.Prod_cost_Price;
                    product.Prod_Stored = prodVM.Prod_Stored;
                    product.Store_Id = prodVM.Store_Id;
                    product.Tax_Id = prodVM.SelectedTaxId;
                    product.Prod_Type = prodVM.Prod_Type;
                    product.Quantity = prodVM.quantity;


                        var sliceDetail = new SlicesProduct();
                       
                        sliceDetail.Slice_Price = prodVM.Sale_Price;
                        sliceDetail.SlicesSlice_Id = db.Slices.FirstOrDefault().Slice_Id;

                        product.SlicesProducts.Add(sliceDetail);


                    

                    db.Products.Add(product);
                   
                    db.SaveChanges();
                  
                    if (product.Prod_Type == Models.Prod_Type.مجمع || product.Prod_Type == Models.Prod_Type.خدمة)
                    {
                        var AllStore = db.Stores.ToList(); ;
                        List<ProductBatches> batchList = new List<ProductBatches>();
                        foreach (var bat in AllStore)
                        {
                            var batch = new ProductBatches();
                            batch.Batch_Date = DateTime.Now;
                            batch.SalesPrice = product.Sale_Price;
                            batch.Product_ID = product.Prod_ID;
                            batch.Store_ID = bat.Store_ID;
                            batch.Product_Quantity = 1;
                            batch.IsActive = true;
                            batchList.Add(batch);
                        }
                        db.ProductBatches.AddRange(batchList);
                        db.SaveChanges();
                    }
                    db.Dispose();
                    db.Dispose();
                    return product;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var result = ex.Data;
                var tt = result;
                throw;
            }

            return product;



        }

        public static List<SlicesTem> GetAllTempSlicesProductForEdit(int prodId)
        {
          
            var db = new AccountingContainer();
            var detailTemp = db.ProductTems.Find(prodId);
            db.SlicesTems.RemoveRange(detailTemp.SlicesTems);
            db.SaveChanges();
            var Product = db.Products.Find(prodId);
            List<SlicesTem> Lista = new List<SlicesTem> { };
            foreach (var item in Product.SlicesProducts)
            {
                var temp = new SlicesTem();
                temp.SlicesSlice_Id = item.SlicesSlice_Id;
                temp.Slice_Name = db.Slices.Find(item.SlicesSlice_Id).Slice_Name;
                temp.Slice_Price = item.Slice_Price;

                temp.ProductTemProd_ID = prodId;
                Lista.Add(temp);

            }
            db.SlicesTems.AddRange(Lista);

            db.SaveChanges();

            return detailTemp.SlicesTems.ToList();
        }

        public static Product getTotalQuantityandCost(int id)
        {
            var db = new AccountingContainer();
            var batchList = db.ProductBatches.Where(a => a.Product_ID == id).ToList();
            var Prod = new Product();
            Prod.Prod_Cost_Price = 0;
            Prod.Sale_Price = 0;
            foreach(var batch in batchList)
            {

                Prod.Sale_Price += batch.Product_Quantity;
                Prod.Prod_Cost_Price += batch.Product_Quantity*batch.Product_Cost;


            }
            var storeCard = db.StoreCards.Where(a => a.Product_Id == id).ToArray();
            if (Prod.Prod_Cost_Price == 0&& storeCard.Count()>0)
            {
                var singlcard = storeCard[storeCard.Count()- 1];
                if (singlcard.Quantity > 0)
                {
                    Prod.Prod_Cost_Price = singlcard.TotalCost / singlcard.Quantity;
                }
            }
            return Prod;
        }
        public static bool makeBarcodeForAllProduct()
        {
            var db = new AccountingContainer();
            var prodList = db.Products.Where(a => a.Prod_Type == Models.Prod_Type.منتج);
            if (prodList.FirstOrDefault() == null)
            {
                return false;
            }
            if (prodList.FirstOrDefault().Bar_Code == null)
            {
                return false;
            }
            foreach(var prod in prodList)
            {
                var barcod = Utilites.QRcode.MakeBarcode(prod.Prod_Code);
                prod.Bar_Code = prod.Bar_Code;
                prod.Bar_Code_one = barcod;
                db.Entry(prod).State = EntityState.Modified;
            }
            db.SaveChanges();
            return true;
        }
        public static ProductFileDetails getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.ProductFileDetails.Find(fileId);
            return file;
        }
        public static string deleteFile(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.ProductFileDetails.Find(fileId);
            db.ProductFileDetails.Remove(file);
            db.SaveChanges();
            return "تم حذف الملف ";
        }
        public static string DeleteProductTransfer(int id)
        {

            var db = new AccountingContainer();
            var Prod = db.Products.Find(id);
            if (Prod.IsActive != false)
            {
                Prod.IsActive = false;

            }
            else
            {
                Prod.IsActive = true;
            }

            db.Entry(Prod).State = EntityState.Modified;
            db.SaveChanges();


            return Resources.ProductDetails.TransferSucces;

        }
        public static decimal getQuantityinPendingInvoice(int prod_Id,int StoreId)
        {
            var db = new AccountingContainer();
            decimal invoicemasterQuantity = db.InvoiceDetails.Where(a => a.Product_ID == prod_Id && a.InvoiceMaster.Invoice_State == TransactionState.Pending && a.InvoiceMaster.Store_ID == StoreId).ToList().Select(a => a.Product_Quantity).Sum();
            decimal SalesReciptQuantity = db.SalesReceiptDetails.Where(a => a.Product_Id == prod_Id && a.SalesReceiptMaster.Invoice_State == TransactionState.Pending && a.SalesReceiptMaster.Store_ID == StoreId).ToList().Select(a => a.Product_Quantity).Sum();
            return invoicemasterQuantity + SalesReciptQuantity;

        }

        public static decimal getQuantitySaleProduct(int ProductId)
        {
            var db = new AccountingContainer();

            decimal invoice = 0;

            decimal sales = 0;
            decimal pos = 0;
            decimal cashier = 0;
            var invoicedetail = db.InvoiceDetails.Where(a => a.Product_ID == ProductId && a.InvoiceMaster.Invoice_State == TransactionState.Post);
            var invoiceSale = db.SalesReceiptDetails.Where(a => a.Product_Id == ProductId && a.SalesReceiptMaster.Invoice_State == TransactionState.Post);
            var invoicePOS = db.DetailPOSInvoices.Where(a => a.Product_Id == ProductId);
            var Invoicecasher = db.OrderDetails.Where(a => a.ProductID == ProductId);
            if (invoicedetail.Count() != 0)
            {
                invoice = invoicedetail.Select(a => a.Reverse_Quantity).ToList().Sum();
            }
            if (invoiceSale.Count() != 0)
            {
                sales = invoiceSale.Select(a => a.Reverse_Quantity).ToList().Sum();
            }
            if (invoicePOS.Count() != 0)
            {
                pos = invoicePOS.Select(a => a.Reverse_Quantity).ToList().Sum();
            }
            if (Invoicecasher.Count() != 0)
            {
                cashier = Invoicecasher.Select(a => a.Reverse_Quantity).ToList().Sum();
            }

            return invoice + sales + pos + cashier;


        }

        public static decimal getQuantitySaleProductForStore(int ProductId, int store)
        {
            var db = new AccountingContainer();

            decimal invoice = 0;

            decimal sales = 0;
            decimal pos = 0;
            decimal cashier = 0;
            var invoicedetail = db.InvoiceDetails.Where(a => a.Product_ID == ProductId &&a.InvoiceMaster.Store_ID== store && a.InvoiceMaster.Invoice_State == TransactionState.Post);
            var invoiceSale = db.SalesReceiptDetails.Where(a => a.Product_Id == ProductId && a.SalesReceiptMaster.Store_ID == store && a.SalesReceiptMaster.Invoice_State == TransactionState.Post);
            var invoicePOS = db.DetailPOSInvoices.Where(a => a.Product_Id == ProductId&& a.MasterPOSInvoice.PointOfSale.Store_Id == store );
            var Invoicecasher = db.OrderDetails.Where(a => a.ProductID == ProductId&&a.Order.StoreId == store );
            if (invoicedetail.Count() != 0)
            {
                invoice = invoicedetail.Select(a => a.Reverse_Quantity).ToList().Sum();
            }
            if (invoiceSale.Count() != 0)
            {
                sales = invoiceSale.Select(a => a.Reverse_Quantity).ToList().Sum();
            }
            if (invoicePOS.Count() != 0)
            {
                pos = invoicePOS.Select(a => a.Reverse_Quantity).ToList().Sum();
            }
            if (Invoicecasher.Count() != 0)
            {
                cashier = Invoicecasher.Select(a => a.Reverse_Quantity).ToList().Sum();
            }

            return invoice + sales + pos + cashier;


        }
        public static List<ProductBatches> getAllProductBatchesForThatProduct(int ProductId)
        {
            var db = new AccountingContainer();
            var Product = db.Products.Find(ProductId);
            var storeList = db.Stores.Where(a => a.isActive != false).ToList();
            
            if(Product.ProductBatches.Count()< storeList.Count())
            {
                foreach(var store in storeList)
                {
                    if (Product.ProductBatches.Where(a => a.Store_ID == store.Store_ID).Count() == 0)
                    {
                        var batch = new ProductBatches();
                        batch.Batch_Cost = 0;
                        batch.IsActive = true;
                        batch.Product_ID = ProductId;
                        batch.SalesPrice = Product.Sale_Price;
                        batch.Product_Cost = 0;
                        if(Product.Prod_Type==Models.Prod_Type.اولي|| Product.Prod_Type == Models.Prod_Type.منتج)
                        {
                            batch.Product_Quantity = 0;
                        }
                        else
                        {
                            batch.Product_Quantity = 1;
                        }
                        batch.Store_ID = store.Store_ID;
                        db.ProductBatches.Add(batch);
                        db.SaveChanges();
                    }


                }


            }
            var ProductNew = db.Products.Find(ProductId);

            return ProductNew.ProductBatches.ToList();
        }
        public static int PostNewSalesForStore(dynamic json)
        {
            var db = new AccountingContainer();
            var storeId = 0;
            var productId = 0;
            foreach (var batchvm in json)
            {
                ProductBatches batch = db.ProductBatches.Find(Convert.ToInt32(batchvm.batchId));
                productId = batch.Product.Prod_ID;
                storeId = batch.Store_ID.Value;
                batch.IsActive = Convert.ToBoolean(batchvm.IsActive);
                batch.SalesPrice = Convert.ToDecimal(batchvm.SalesPrice);
                var product = batch.Product;
                product.Prod_Cost_Price = Convert.ToDecimal(batchvm.Prod_Cost_Price);

                db.Entry(batch).State = EntityState.Modified;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

            }
            return productId;




        }

    }

        }
    
