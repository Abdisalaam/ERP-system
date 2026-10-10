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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Accountingsystem.Utilites;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class FixedAssetController : Controller
    {
        public ActionResult Getexcel()
        {
            return File("~/FiLeDownLoad/Asset.xlsx", "application/xlsx", Server.UrlEncode("~/FiLeDownLoad/Asset.xlsx"));
        }

        public JsonResult CheckCode(string FAsset_Codes)
        {
            if (FixedAsset.containCodeInfo(FAsset_Codes))
            {
                var msg = "AS- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckCodeOpenFixed(string FAsset_Codes)
        {
            if (FixedAsset.containCodeInfoOpen(FAsset_Codes))
            {
                var msg = "AS- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckCodeOpen(string OpenBalance_Codes)
        {
            if (FixedAsset.containCodeOpen(OpenBalance_Codes))
            {
                var msg = "OPA- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index(string search, string msg, string sortFixed)

        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfAsset(user.Id);
            if (pack.Item2 == true)
            {



                HttpCookie Newcookie = HttpContext.Request.Cookies["FixedAssetSearchResult"];
                if (!String.IsNullOrEmpty(search))
                {
                    if (Newcookie != null)
                    {
                        Newcookie.Value = search;
                    }
                    else
                    {
                        var cookieName = "FixedAssetSearchResult";
                        Newcookie = new HttpCookie(cookieName);
                    }

                    Response.Cookies.Add(Newcookie);

                }


                if (String.IsNullOrEmpty(search) && Newcookie != null && !String.IsNullOrEmpty(Newcookie.Value))
                {

                    search = Newcookie.Value;
                }
                var AllAsset = FixedAsset.getAllAssets(search, sortFixed);
                ViewBag.Message = msg;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortFixed) ? "Name" : "";
                ViewBag.FixedAssetCodeSortParm = sortFixed == "Code" ? "Price" : "Code";
                ViewBag.FixedAssetPriceSortParm = sortFixed == "Price" ? "Code" : "Price";
                return View(AllAsset);
            }
            else
            {
                var msgo = "باقتك لا تدعم قسم الاصول";
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }
        public ActionResult Indexdispose(string search, string msg, string sortFixed)
        {
            var AllAsset = FixedAsset.getAllDisposeAssets(search, sortFixed);
            ViewBag.Message = msg;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortFixed) ? "Name" : "";
            ViewBag.FixedAssetCodeSortParm = sortFixed == "Code" ? "Price" : "Code";
            ViewBag.FixedAssetPriceSortParm = sortFixed == "Price" ? "Code" : "Price";
            return View(AllAsset);
        }
        //[HttpPost]
        //public ActionResult Index(string search,FixedAsset fx)
        //{
        //    fx.FAsset_Code = search;
        //    var ExactAsset = FixedAsset.getAllAssets(fx.FAsset_Code);
        //    return View(ExactAsset);
        //}
        [HttpGet]
        public virtual JsonResult GetTypeCat(string code)
        {
            var my = FixedAsset.GetTypeCategory(code);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetDepartment(int code)
        {
            var my = FixedAsset.GetDepartments(code);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public virtual JsonResult GetBussines(int code)
        {
            var my = FixedAsset.GetBussinessUnits(code);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        // GET: FixedAsset/Details/5
        public ActionResult Details(int id)
        {
            var findAsset = FixedAsset.GetAssetByID(id);
            return View(findAsset);
        }

        // GET: FixedAsset/Create
        [HttpGet]
        public ActionResult Create(string msg)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء اصل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            //Using  the  Function creatVMForCreation  to Change  Our Views of  IDs except  My entity id  To SelectedList
            var CVM = FixedAsset.createVMForCreation();
            ViewBag.Message = msg;
            return View(CVM);
        }

        // POST: FixedAsset/Create
        [HttpPost]
        public ActionResult Create(FixedAssetVM FVM)
        {
            try
            {
                var msg = "";
                //  if (ModelState.IsValid)
                // {
                var createAsset = FixedAsset.AddNewAsset(FVM);
                msg =Resources.validate.savedSuc+ " " + FVM.FAsset_Descriptions ;
                //  }
                return RedirectToAction("Create", new { msg = msg });
            }
            catch
            {
                return View(FVM);
            }
        }
        [HttpGet]
        public ActionResult CreateOpenBalance()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "رصيد افتتاحي اصول");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            //Using  the  Function creatVMForCreation  to Change  Our Views of  IDs except  My entity id  To SelectedList
            var fill = FixedAsset.FillOpeingBalance();
            return View(fill);
        }
        [HttpPost]
        public ActionResult CreateOpenBalance(FixedAssetOpeningBalanceVM FOVM)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            //Using  the  Function creatVMForCreation  to Change  Our Views of  IDs except  My entity id  To SelectedList
            var Blanace = FixedAsset.AddOpeningBalanace(FOVM , user.Id); 

            return RedirectToAction("Index");
        }

        public FileResult ReturnFixedFile(int fileId)
        {

            var document = FixedAsset.getFileById(fileId);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = document.FileName,
                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.ProImg, "txt");
        }
        public ActionResult GetAllFiles(int empId)
        {
            var model = FixedAsset.GetAllFiles(empId);
            return View(model);
        }

        // GET: FixedAsset/Edit/5

        public ActionResult Edit(int ID)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل اصل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var editFAsst = FixedAsset.FillTheViewModel(ID);
            if (editFAsst.Fasset_IDs != ID)
            {
                string msg = "No Asset  With this Code";
                return RedirectToAction("Index", new { msg = msg });
            }

            return View(editFAsst);
        }

        // POST: FixedAsset/Edit/5
        [HttpPost]
        public ActionResult Edit(FixedAssetVM fvm)
        {
            try
            {
                var editfixed = FixedAsset.EditFixedAsset(fvm);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(fvm);
            }
        }
        public ActionResult Dispose(int id)
        {
            var msg = "";
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                FixedAsset.DisposeFixedAsset(id , user.Id);
                msg = Resources.validate.DisposeAsset;
                return RedirectToAction("Index", new { msg = msg });
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return View(msg);
            }
        }
        //public ActionResult StartDep(int id, FixedAssetVM vm, string msg)
        //{
        //    try
        //    {
        //        vm.Fasset_IDs = id;
        //        FixedAsset.startDep(vm);
        //        msg = "تم بدء الاهلاك";
        //        return RedirectToAction("Index", new { msg = msg });
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ex.Message;
        //        return View(msg);
        //    }
        //}
        public ActionResult StartAllDep(string msg)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "بدء الاهلاك");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var result = FixedAsset.startAllDep(user.Id);
                if (result > 0)
                {
                    msg = Resources.validate.Thedepreciationwassuccessful;
                    return RedirectToAction("Index", new { msg = msg });
                }
                else
                {
                    msg = Resources.validate.Depreciationisstartedonceeveryday;
                    return RedirectToAction("Index", new { msg = msg });
                }              
               
            }
            catch(Exception)
            {
                msg = Resources.validate.Error;
                return RedirectToAction("Index", new { msg = msg });
            }
        }
        /// <summary>
        /// Gets all assets in the form of Json array in the form specified by 
        /// Select2 (https://select2.org/data-sources/arrays) to populate 
        /// the drop-down list
        /// </summary>
        /// <returns> A Json array in the form specified by Select2  </returns>
        /// 
        public ActionResult StartAllDepNew()
        {
         
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "بدء الاهلاك");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var model = new AssetDepDaysVM();
           
                    return View(model);
                

           
        }
        [HttpPost]
        public ActionResult StartAllDepNew(AssetDepDaysVM vm)
        {
           
              var  msg = Resources.validate.Thedepreciationwassuccessful;
            var a = FixedAsset.StartDep(vm);
                return RedirectToAction("Index", new { msg = a });
            
        }
        [HttpGet]
        public ActionResult getAssetsJson()
        {
            var result = FixedAsset.getAllNotDisposedAssets();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: FixedAssetSecCategory/Create
        //public ActionResult CreateCategory(string msg)
        //{
        //    var CVM = FixedAsset.createVMForCreationCategory();
        //    ViewBag.Message = msg;
        //    return View(CVM);
        //}

        //// POST: FixedAssetSecCategory/Create
        //[HttpPost]

        //public ActionResult CreateCategory(FixedAssetSecCategoryVM cvm)
        //{
        //    try
        //    {
        //        var msg = "";



        //        var creatFixedcattype = FixedAsset.addFixedsecType(cvm);
        //        msg = "تم حفظ التصنيف" + " --" + creatFixedcattype.FAssetCat_Name;

        //        // return RedirectToAction("Create");


        //        //var msg = "Succes";
        //        return RedirectToAction("CreateCategory", new { msg = msg });
        //    }
        //    catch
        //    {

        //        return View(cvm);
        //    }
        //}
        //// GET: FixedAssetType/Create
        //public ActionResult CreateFixedType(string msg)
        //{
        //    var fVM = new FixedAssetTypeVM();
        //    ViewBag.Message = msg;
        //    return View(fVM);
        //}

        //// POST: FixedAssetType/Create
        //[HttpPost]
        //public ActionResult CreateFixedType(FixedAssetTypeVM fixedvm)
        //{
        //    try
        //    {
        //        var msg = "";
        //        if (ModelState.IsValid)
        //        {


        //            var creatFixedtype = FixedAsset.addFixedType(fixedvm);
        //            msg = "تم حفظ النوع" + " --" + creatFixedtype.FAsset_TypeName;
        //        }
        //        return RedirectToAction("CreateFixedType", new { msg = msg });

        //    }
        //    catch
        //    {
        //        return View(fixedvm);
        //    }
        //}
        [HttpGet]
        public ActionResult disposeAsset(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "بدء الاهلاك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            //Using  the  Function creatVMForCreation  to Change  Our Views of  IDs except  My entity id  To SelectedList
            var CVM = FixedAsset.FillTheViewModelofDispose(id);
           
            return View(CVM);
        }
        [HttpPost]
        public ActionResult disposeAsset(FixedDisposeVM FDIP)
        {
            //Using  the  Function creatVMForCreation  to Change  Our Views of  IDs except  My entity id  To SelectedList
          

          
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                
                //  if (ModelState.IsValid)
                // {
                var CVM = FixedAsset.DisposingAsset(FDIP , user.Id);
               
                //  }
                return RedirectToAction("Index", new { msg = CVM });
            }
            catch
            {
                return View(FDIP);
            }
        }
        public FileResult ReturnInvoicetFile(int fileId)
        {

            var document = FixedAsset.getFileById(fileId);
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

        public ActionResult getBankorTreasreAccount(int ID)
        {

            var BankAccountList = FixedAsset.FilltheViewofBankAccount(ID);
            return PartialView(BankAccountList);

        }
        public ActionResult EditFixedInfo(int ID)
        {
            var editFAsst = FixedAsset.createVMForEditInfo(ID);
            if (editFAsst.Fasset_IDs != ID)
            {
                string msg = "No Asset  With this Code";
                return RedirectToAction("Index", new { msg = msg });
            }

            return View(editFAsst);
        }

        // POST: FixedAsset/Edit/5
        [HttpPost]
        public ActionResult EditFixedInfo(FixedAssetVM fvm)
        {
            try
            {
                var editfixed = FixedAsset.EditFixedAssetInformation(fvm);
                return RedirectToAction("FixedInfoList");
            }
            catch
            {
                return View(fvm);
            }
        }
        [HttpGet]
        public ActionResult DeleteFixedInfo(int id)
        {
            var myAsset = FixedAsset.getinfoByID(id);
            return View(myAsset);
        }
        [HttpPost]
        public ActionResult DeleteFixedInfo(int id,FixedAssetVM FVM)
        {

            try
            {
                // TODO: Add delete logic here
                string deprt = FixedAsset.deleteFixedassetInfo(id);
                return RedirectToAction("FixedInfoList");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult FixedInfoList()
        {
            var allList = FixedAsset.ListofAssetInfo();
            return View(allList);
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
                worksheet.Cells[1, 2] = "الزامى";
                worksheet.Cells[1, 3] = "الزامى";
                worksheet.Cells[1, 4] = "الزامى ";
                worksheet.Cells[1, 5] = "الزامى";
                worksheet.Cells[1, 6] = "الزامى";
                worksheet.Cells[1, 7] = "الزامى";
                worksheet.Cells[1, 8] = "الزامى";
                worksheet.Cells[1, 9] = "الزامى";

                worksheet.Cells[2, 1] = "اسم الاصل";
                worksheet.Cells[2, 2] = "رقم نوع الاصل";
                worksheet.Cells[2, 3] = "سعر شراء الاصل";
                worksheet.Cells[2, 4] = "مجمع الاهلاك ";
                worksheet.Cells[2, 5] = "القيمة الدفترية";
                worksheet.Cells[2, 6] = "تاريخ تشغيل الاصل";
                worksheet.Cells[2, 7] = "تاريخ اخر اهلاك";
                worksheet.Cells[2, 8] = "عمر الاصل بالاشهر";
                worksheet.Cells[2, 9] = "قيمة الخردة";


                worksheet.Cells[1, 13] = "نوع الاصل";
                worksheet.Cells[1, 14] = "أراضي";
                worksheet.Cells[1, 15] = "مباني";
                worksheet.Cells[1, 16] = "سيارات";
                worksheet.Cells[1, 17] = "الأثاث والديكور";
                worksheet.Cells[1, 18] = "الكمبيوتر";
                worksheet.Cells[1, 19] = "معدات";
                worksheet.Cells[1, 20] = "اجهزه كهربائيه";
                worksheet.Cells[1, 21] = "عدد وادوات";
                worksheet.Cells[1, 22] = "خزائن حديدية";
                worksheet.Cells[1, 23] = "برامج حاسب الي";

                worksheet.Cells[2, 13] = "رقم الاصل";
                worksheet.Cells[2, 14] = 11;
                worksheet.Cells[2, 15] = 12;
                worksheet.Cells[2, 16] = 13;
                worksheet.Cells[2, 17] = 14;
                worksheet.Cells[2, 18] = 15;
                worksheet.Cells[2, 19] = 16;
                worksheet.Cells[2, 20] = 17;
                worksheet.Cells[2, 21] = 18;
                worksheet.Cells[2, 22] = 19;
                worksheet.Cells[2, 23] = 20;

                int row = 3;
                foreach (FixedAsset item in FixedAsset.GetAllFixedAssetForExport())
                {
                    worksheet.Cells[row, 1] = item.FAsset_Description;
                    worksheet.Cells[row, 2] = item.FixedAssetTypeFAsset_TypeID;
                    worksheet.Cells[row, 3] = item.FAsset_PurchasePrice;
                    worksheet.Cells[row, 4] = item.Complex;
                    worksheet.Cells[row, 5] = item.BookValue;
                    worksheet.Cells[row, 6] = item.FAsset_WorkingDate;
                    worksheet.Cells[row, 7] = item.OpenBalanceWorkDate;
                    worksheet.Cells[row, 8] = item.FAsset_AgeMonth;
                    worksheet.Cells[row, 9] = item.JunckValue;

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

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            try
            {
                var msg = FixedAsset.Import(excelfile);
                return RedirectToAction("Index", new { msg = msg });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { msg = ex.Message });

            }
        }


    }
}