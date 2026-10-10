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
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Accountingsystem.Utilites;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace Accountingsystem.Controllers
{
    [Authorize]
    // GET: Store
    public class StoreController : Controller
    {
        public ActionResult Getexcel()
        {
            return File("~/FiLeDownLoad/Store.xlsx", "application/xlsx", Server.UrlEncode("~/FiLeDownLoad/Store.xlsx"));
        }
        public void maketestExcel(int id)
        {
            var excelProduct = new ExcelSheetTemplate();
            Response.ClearContent();
            Response.BinaryWrite(excelProduct.GenerateExcel(id));
            Response.AddHeader("content-disposition", "attachment; filename=product.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();


        }
        public ActionResult ImportStartBalance(HttpPostedFileBase excelfile)
        {
            try
            {
                var msg = Store.ImportStartbalance(excelfile);
                return RedirectToAction("Index", new { msg = msg });
            }
            catch ( Exception ex)
            {
                return RedirectToAction("Index", new { msg = ex.Message });
            }
        }
        public async System.Threading.Tasks.Task<ActionResult> ExportTheProduct(int StoreId)
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
                worksheet.Cells[1, 9] = "الزامى";

                worksheet.Cells[2, 1] = "كود الصنف";
                worksheet.Cells[2, 2] = "اسم الصنف";

                worksheet.Cells[2, 3] = " الكمية ";
                worksheet.Cells[2, 9] = " المخزن ";


                int row = 3;
                foreach (Product item in Product.GetAllPordect())
                {
                    worksheet.Cells[row, 1] = item.Prod_Code;
                    worksheet.Cells[row, 2] = item.Prod_Name;

                    worksheet.Cells[row, 3] = null;
                    row++;
                }
                worksheet.Cells[3, 9] = Store.GetStoreById(StoreId).Store_Name;


                //string path = System.Web.HttpContext.Current.Server.MapPath("~/FiLeDownLoad/" + "ProductForStore.xlsx");
                string path = Server.MapPath("../FiLeDownLoad/ProductForStore.xlsx");

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);

                }
                pathxcel = path;
                workbook.SaveAs(path);
                workbook.Close();
                

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
        public ActionResult Export()
        {
            HttpPostedFileBase file = Request.Files["goods_in_data.xls"];
            string directory = @"Uploads\";
            try
            {
                Application application = new Application();
                Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Worksheet worksheet = workbook.ActiveSheet;

                worksheet.Cells[1, 1] = "الزامى";
              
                worksheet.Cells[1, 2] = "غير الزامى";

                worksheet.Cells[2, 1] = "اسم المخزن";
                
                worksheet.Cells[2,2] = "عنوان المخزن";
               
                int row = 3;
                foreach (Store item in Store.GetAllStore())
                {
                    //worksheet.Cells[row, 1] = item.Customer_Id;
                    worksheet.Cells[row, 1] = item.Store_Name;
                   
                    worksheet.Cells[row, 2] = item.Store_Address;
                    
                    row++;
                }

                //workbook.get_Range("A1", "O1").EntireColumn.AutoFite();
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Path.Combine(directory, fileName));
                }
                //workbook.SaveAs("~\\Files excel\\myCustomersList.xls");
                workbook.Close();
                Marshal.ReleaseComObject(workbook);
                application.Quit();
                Marshal.FinalReleaseComObject(workbook);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            return RedirectToAction("Index");
        }

        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            try
            {
                var msg = Store.Import(excelfile);
                return RedirectToAction("Index", new { msg = msg });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { msg = ex.Message });

            }
        }

        // GET: Store
        public ActionResult Index(String msg)
        {
                var models = Store.GetAllStore();
                ViewBag.message = msg;
                return View(models);
            
        }
        public ActionResult IndexNotActive(String msg)
        {
            var models = Store.GetAllStoreNotActive();
            ViewBag.message = msg;
            return View(models);
        }

        // GET: Store/Details/5
        public ActionResult Details(int id)
        {
            var model = Store.GetStoreByIdForDetail(id);
            return View(model);
        }

        // GET: Store/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء مخزن");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
    var pack = PackageRecognize.checkValiditeOfStore(user.Id);
    if (pack.Item2 == true)
    {
        var store = Store.CreateVMForCreation();
            return View(store);
            }
            else
            {
                var msgo = Resources.validate.YourpackagedoesnotsupportthatNumberOfStore;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }

        // POST: Store/Create
        [HttpPost]
        public ActionResult Create(StoreVM storeVM)
        {
            try
            {

                var store = Store.CreateStore(storeVM);
                return RedirectToAction("Details", new {id=store });
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Store/Edit/5
        public ActionResult Edit(int id)
        {
            var store = Store.FillStoreVM(id);
            return View(store);
        }

        // POST: Store/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, StoreVM storeVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل مخزن");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var store = Store.EditeStore(storeVM);

                return RedirectToAction("Details", new { id = store });
            }
            catch
            {
                return View();
            }
        }
        public ActionResult DeleteTrans(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ت غير نشط مخزن");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = Store.DeleteTransfer(id);
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Store/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    var store = Store.GetStoreById(id);
        //    return View(store);
        //}

        // POST: Store/Delete/5

        public ActionResult Delete(int id)
        {
            try
            {
                var store = Store.DeleteStore(id);
                return RedirectToAction("Index",new {msg= store });
            }
            catch
            {
                return View();
            }
        }

        // Views the products within a store in a table
        [HttpGet]
        public PartialViewResult ViewProducts(int id)
        {
            //JArray json = Store.getStoreProducts(id);
            var model= Store.getStoreProducts(id); 
            return PartialView("_productpatch",model);
 
        }

        [HttpGet]
        public virtual JsonResult GetProducts(int id)
        {
            var products = Store.getStoreBatches(id);
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ChooseProductFromList(int id)
        {
            try
            {
                var store = Store.ListOFProduct(id);
                return View(store);
            }
            catch (Exception )
            {

                return RedirectToAction("Index");
            }
      
        }
        
        public ActionResult PreparerecallDetails(int prodID, int storID)
        {
            var btachQan = Store.PrepareBatchQuantity(prodID, storID);
            return PartialView("PreparerecallDetails", btachQan);

        }
        public ActionResult PreparerecallDetails2(int prodID, int storID)
        {
            var btachQan = Store.PrepareProducty(prodID, storID);
            return PartialView("PreparerecallDetails", btachQan);

        }
        [HttpPost]
        public virtual JsonResult GetProducts(ProductBatchSelection PPs)
        {
            var products = Store.AddInTemp(PPs);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult DeleteFromTemps(int BatchID, int ProductID)
        {
            var products = Store.DeleteFromTemp(BatchID, ProductID);
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ShowTempList()
        {
            var btachQan = Store.batchList();
            return PartialView("ShowTempList", btachQan);

        }
        [HttpPost]
        public virtual JsonResult AddtempToBatch()
        {
            var products = Store.addTemptoBatchList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult MakeStoreEqualStoreCard()
        {
            var products = Store.makeStoreQuantity();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult RenewPriceProductStore(int StoreId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تحديث منتجات مخزن");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var btachQan = Store.getAllProductInThatStore(StoreId);
            return View( btachQan);

        }
   
        public  ActionResult RenewPriceProductStoreqtg(string btachQan, int hh)
        {
            dynamic batchList = JsonConvert.DeserializeObject<dynamic>(btachQan);

            int msg = Store.PostNewSalesForStore(batchList);
            var fullUrl = this.Url.Action("Details","Store", new { id = msg }, this.Request.Url.Scheme);
            return Json(fullUrl, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index", new { msg = msg });
        }
    }
}
