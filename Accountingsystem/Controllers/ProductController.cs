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
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class ProductController : Controller
        {
        public ActionResult Getexcel()
        {
            return File("~/FiLeDownLoad/Product.xlsx", "application/xlsx", Server.UrlEncode("~/FiLeDownLoad/Product.xlsx"));
        }
        public async System.Threading.Tasks.Task< ActionResult> Export()
        {
            HttpPostedFileBase file = Request.Files["ProductForStore.xlsx"];
            string pathxcel = null;
            try
            {
                Application application = new Application();
                Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Worksheet worksheet = workbook.ActiveSheet;
                Product product = new Product();

                worksheet.Cells[1, 1] = "الزامى";
                worksheet.Cells[1, 2] = "الزامى";
                worksheet.Cells[1, 3] = "الزامى";
                

                worksheet.Cells[2, 1] = "كود الصنف";
                worksheet.Cells[2, 2] = "اسم الصنف";
                
                worksheet.Cells[2, 3] = " الكمية ";

                

                int row = 3;
                foreach (Product item in Product.GetAllPordect())
                {
                    worksheet.Cells[row, 1] = item.Prod_Code;
                    worksheet.Cells[row, 2] = item.Prod_Name;
                                     
                    worksheet.Cells[row, 3] = null;
                    row++;
                }
                string path = System.Web.HttpContext.Current.Server.MapPath("~/FiLeDownLoad/" + "ProductForStore.xlsx");
      
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);

                }
                    pathxcel = path;
                workbook.SaveAs(path);
                
                //workbook.get_Range("A1", "O1").EntireColumn.AutoFite();
               
                //workbook.SaveAs("~\\Files excel\\myCustomersList.xls");

                Marshal.ReleaseComObject(workbook);
                application.Quit();
                Marshal.FinalReleaseComObject(workbook);
               
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            await Task.Delay(2000);
            return File(pathxcel, "application/xlsx", Server.UrlEncode(pathxcel)); 
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            try
            {
                var msg = Product.Import(excelfile);
                return RedirectToAction("Index", new { msg = msg });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { msg = ex.Message });

            }

        }

        public JsonResult CheckCode(string Prod_Code)
           {
            if (Product.containCode(Prod_Code))
            {
                var msg = "PRO- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
            public JsonResult CheckCodeOpen(string OpenBalance_Code)
           {
            if (Product.containCodeOpen(OpenBalance_Code))
            {
                var msg = "OPP- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
           }
            // GET: Product
            public ActionResult Index(String msg)
            {
                var models = Product.GetAllPordect();
                ViewBag.Message = msg;
                return View(models);
            }
        public ActionResult IndexNotActive(String msg)
        {
            var models = Product.GetAllPordectNotActive();
            ViewBag.Message = msg;
            return View(models);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
            {
                var model = Product.GetProdectById(id);
                return View(model);
            }


            [HttpGet]
            public ActionResult GetAllTempSlicesDetails(int tempProductId)
            {
            var SliceProductDetailVm = Product.GetAllTempSliceProductDetails(tempProductId);
            return PartialView("SliceProductDetailTemp", SliceProductDetailVm);
            }

            public ActionResult PrepareSliceProductDetail()
        {
            var sliceProduct = Product.PrepareSliceProductDetail();
            return PartialView("SliceProductDetailCreation", sliceProduct);
        }

            [HttpPost]
            public virtual JsonResult AddSliceToProduct(SlicesProductVM slicesDetail)
        {
            string msg = "";
            try
            {
                msg = Product.AddNewSliceDetailsTemp(slicesDetail);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }
         
            return Json(msg, JsonRequestBehavior.AllowGet);
           
        }

        /// <summary>
        /// Compound  Product
        /// </summary>
        /// <param name="iD"></param>
        /// <returns></returns>
        /// 
        public ActionResult GetAllTempCompoundDetails(int tempProductId)
        {
            var AllTemp = Product.GetAllTempCompondint(tempProductId);
            return PartialView(AllTemp);
        }

        public ActionResult PrepareCompoundDetail()
        {
            var vm = Product.PrepareProductCompound();
            return PartialView(vm);
        }

        [HttpPost]
        public virtual JsonResult AddCompoundToProduct(ProductCompoundVM vm)
        {
            string msg = "";
            try
            {
                msg = Product.AddNewProductCompoundTemp(vm);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteTempCompoundDetail(int iD)
        {
            var temp = Product.DeleteCompoundFromTemp(iD);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteTempDetail(int iD)
        {
            var temp = Product.DeleteSliceFromTemp(iD);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

   
             // GET: Product/Create
             public ActionResult Create()
            {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء صنف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Product.CreateVMForCreation();
                return View(model);
            }
         
             // POST: Product/Create
             [HttpPost]
             public ActionResult Create(ProductVM productVM)
            {
                try
                {
                    // TODO: Add insert logic here

                    var model = Product.CreateProdect(productVM);
                if (model == 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Details", new { id= model });
                }
                }
                catch (Exception ex)
               {
                var msg = ex.Message;
                return View();
               }
            }  


            // GET: Product/Delete/5
            public ActionResult Delete(int id)
            {
                var model = Product.GetProdectById(id);
                return View(model);
            }

            // POST: Product/Delete/5
            [HttpPost]
            public ActionResult Delete(int id, FormCollection collection)
            {
                try
                {
                    // TODO: Add delete logic here

                    var model = Product.Deleteprode(id);
                    return RedirectToAction("Index",new {msg= model });
                }
                catch
                {
                    return View();
                }
            }
            //public ActionResult CreateOpeningBalanace()
            //{
            // var model = Product.CreateVMForCreationOpenBalance();
            //return View(model);
            //}

             // POST: Product/Create
            //[HttpPost]
            //public ActionResult CreateOpeningBalanace(ProductOpenBalanceVM productVM)
            // {
            //try
            //{
            //    // TODO: Add insert logic here

            //    var model = Product.CreateProductOpenBalanace(productVM);
            //    return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}
            // }


        /////////////////////////////// For open balance product

        [HttpGet]
        public ActionResult GetAllTempSlicesDetailsForOpen(int tempProductId)
        {
            var SliceProductDetailVm = Product.GetAllTempSliceProductDetailsForOpen(tempProductId);
            return PartialView("SliceProductDetailTempForOpen", SliceProductDetailVm);
        }

        public ActionResult PrepareSliceProductDetailForOpen()
        {
            var sliceProduct = Product.PrepareSliceProductDetailForOpen();
            return PartialView("SliceProductDetailCreationForOpen", sliceProduct);
        }

        [HttpPost]
        public virtual JsonResult AddSliceToProductForOpen(SlicesProductVM slicesDetail)
        {
            string msg = "";
            try
            {
                msg = Product.AddNewSliceDetailsTemp(slicesDetail);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);

        }



        public JsonResult DeleteTempDetailForOpen(int iD)
        {
            var temp = Product.DeleteSliceFromTempForOpen(iD);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }


        // GET: Product/Create
        //public ActionResult CreateForOpen()
        //{
        //    var model = Product.CreateVMForCreationForOpen();
        //    return View(model);
        //}

        // POST: Product/Create
        //[HttpPost]
        //public ActionResult CreateForOpen(ProductOpenBalanceVM productVM)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        var model = Product.addOpenBalanceProdectBatch(productVM);
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        var msg = ex.Message;
        //        return View();
        //    }
        //}

        //////////////////////////// For Edit Product 

        [HttpGet]
        //for list from details
        public ActionResult PrepareSliceProductDetails(int prodID)
        {
            var slice = Product.PreparSliceProductDetailForEdit(prodID);
            return PartialView("SliceProductDetailCreationForEdit", slice);

        }


        [HttpGet]
        public ActionResult GetAllTempSlicesDetailsEdit(int tempProductId)
        {
            var sliceProduct = Product.GetAllTempSliceProductDetailsForEdit(tempProductId);
            return PartialView("SliceProductListForEdit", sliceProduct);
        }
        [HttpGet]
        public ActionResult GetAllTempSliceProductDetailsResult(int tempProductId)
        {
            var _SliceProductTemp = Product.GetAllTempSlicesProductDetailsresult(tempProductId);
            return PartialView("SliceProductResultListTemp", _SliceProductTemp);
        }
        [HttpGet]
        public ActionResult EditForPending(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل صنف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var Slice = Product.fillForEditingProduct(id);
            return View(Slice);
        }
        [HttpPost]
        public ActionResult EditForPending(ProductVM productVM)
        {
            var Slice = Product.EditForProduct(productVM);
            return RedirectToAction("Details", new { id = Slice });
        }

        // for Category
        [HttpGet]
        public ActionResult PartialDialog()
        {
            var category = Category.CreateVMForCreation();
            return PartialView("CategoryPartailView", category);
        }
        public ActionResult AddNewCategoryPartial(CategroyVM cateVM)
        {
            HttpFileCollectionBase files = Request.Files;
            if (files.Count > 0)
            {
                cateVM.Cat_Img2 = files;
            }
            var result = Category.CreateCatgery(cateVM);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        [HttpGet]
        public ActionResult GetCategoryDropdownList()
        {
            var Cat = Product.GetCategoryList();
            return new JsonResult { Data = Cat, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }




        // For Measurunit
        [HttpGet]
        public ActionResult PartialDialog2()
        {
            var measure = MeasuringUnit.CreateVMForCreation();
            return PartialView("MeasureUnitPartial", measure);
        }
        public ActionResult AddNewMeasurePartial(MeasureVM masureVM)
        {

            var result = MeasuringUnit.CreateMeasruingUnit(masureVM);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public ActionResult DeleteTrans(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ت غير نشط صنف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = Product.DeleteProductTransfer(id);
            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public ActionResult GetMeasureDropdownList()
        {
            var Measure = Product.GetMeasurList();
            return new JsonResult { Data = Measure, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult PartialDialog3()
        {
            var measure = Slices.AddSlicesForCreation();
            return PartialView (measure);
        }
        [HttpPost]
        public ActionResult AddNewSlicePartial(SlicesVM masureVM)
        {

            var result = Slices.CreateSlice(masureVM);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpGet]
        public ActionResult GetSliceDropdownList()
        {
            var slice = Product.GetSliceList();
            return new JsonResult { Data = slice, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult getProductJsondrobDown()
        {
            List<Product> Products = Product.getAllproductFordrobDownList();
            List<dynamic> result = new List<dynamic>();
            foreach (var pro in Products)
            {
                var data = new
                {
                    // the value of each select item
                    id = pro.Prod_ID,
                    // showing both code and name in the drop-down list 
                    text = pro.Prod_Name + "\t" + pro.Prod_Code
                };
                result.Add(data);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getProductBatchJsondrobDown()
        {
            //var Products = Product.getAllproductbatchFordrobDownList();
            List<dynamic> result = new List<dynamic>();
            //foreach (var pro in Products)
            //{
            //    var data = new
            //    {
            //        // the value of each select item
            //        id = pro.Product_ID,
            //        // showing both code and name in the drop-down list 
            //        text = pro.Product.Prod_Name 
            //    };
            //    result.Add(data);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult getProductBatchJsondrobDown()
        //{
        //    List<ProductBatches> Products = Product.getAllproductbatchFordrobDownList();
        //    List<dynamic> result = new List<dynamic>();
        //    foreach (var pro in Products)
        //    {
        //        var data = new
        //        {
        //            // the value of each select item
        //            id = pro.Product_ID,
        //            // showing both code and name in the drop-down list 
        //            text = pro.Product.Prod_Name + "\t" + pro.Product.Prod_Code
        //        };
        //        result.Add(data);
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetAllTempSlicesProductForEdit(int tempProductId)
        {
            var _SliceProductTemp = Product.GetAllTempSlicesProductForEdit(tempProductId);
            return PartialView( _SliceProductTemp);
        }
        public FileResult ReturnInvoicetFile(int fileId)
        {

            var document = Product.getFileById(fileId);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = document.FileName,
                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.ProImg, document.File_Type);
        }
        public ActionResult deleteFile(int fileId)
        {

            var document = Product.getFileById(fileId);
            var delete = Product.deleteFile(fileId);
            return RedirectToAction("Details", new { id = document.ProductProd_ID });
        }
        public ActionResult GetAllProductBatch(int ProductId)
        {
            var Batches = Product.getAllProductBatchesForThatProduct(ProductId);
            return View(Batches);
        }
        public ActionResult GetAllProductBatchPOst(string btachQan, int hh)
        {
            dynamic batchList = JsonConvert.DeserializeObject<dynamic>(btachQan);

            int msg = Product.PostNewSalesForStore(batchList);
            var fullUrl = this.Url.Action("Details", "Product", new { id = msg }, this.Request.Url.Scheme);
            return Json(fullUrl, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index", new { msg = msg });
        }
    }
}
