using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.InteropServices;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
//using Rotativa;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        public ActionResult Getexcel()
        {
            return File("~/FiLeDownLoad/Supplier.xlsx", "application/xlsx", Server.UrlEncode("~/FiLeDownLoad/Supplier.xlsx"));
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
                worksheet.Cells[1, 4] = "غير الزامى";
                //worksheet.Cells[1, 5] = "غير الزامى";
                //worksheet.Cells[1, 6] = "غير الزامى";
                //worksheet.Cells[1, 7] = "غير الزامى";
                //worksheet.Cells[1, 8] = "غير الزامى";
                worksheet.Cells[1, 5] = "غير الزامى ";
                worksheet.Cells[1, 6] = "غير الزامى";
                //worksheet.Cells[1, 11] = "غير الزامى";
                worksheet.Cells[1, 7] = "غير الزامى";
                worksheet.Cells[1, 8] = "غير الزامى";


                worksheet.Cells[2, 1] = "اسم المورد";
                worksheet.Cells[2, 2] = "التليفون";
                worksheet.Cells[2, 3] = "الرصيد";
                worksheet.Cells[2, 4] = "البريد الالكترونى";
                //worksheet.Cells[2, 5] = "المدينة";
                //worksheet.Cells[2, 6] = "المحافظة";
                //worksheet.Cells[2, 7] = "البلد";
                //worksheet.Cells[2, 8] = "طريقة الدفع";
                worksheet.Cells[2, 5] = "العنوان";
                worksheet.Cells[2, 6] = "الهاتف ";
                //worksheet.Cells[2, 11] = "رقم الفرع ";
                worksheet.Cells[2, 7] = "الموقع";
                worksheet.Cells[2, 8] = "الفاكس";


                worksheet.Cells[1, 18] = "الارصدة الدائنة";
                worksheet.Cells[2, 18] = "الارصدة المدينة";

                worksheet.Cells[1, 17] = "بالسالب";
                worksheet.Cells[2, 17] = "بالموجب";

                int row = 3;
                foreach (Supplier item in Supplier.GetAllSuppliers())
                {
                    worksheet.Cells[row, 1] = item.Supp_Name;
                    worksheet.Cells[row, 2] = item.Supp_Phone;
                    worksheet.Cells[row, 3] = item.Supp_Balance;
                    worksheet.Cells[row, 4] = item.Supp_Mail;
                    //worksheet.Cells[row, 5] = item.Supp_City;
                    //worksheet.Cells[row, 6] = item.Supp_Province;
                    //worksheet.Cells[row, 7] = item.Supp_Country;
                    //worksheet.Cells[row, 8] = item.Payment_Policy_Id;
                    worksheet.Cells[row, 5] = item.Supp_Address;
                    worksheet.Cells[row, 6] = item.Supp_Mobile;
                    //worksheet.Cells[row, 11] = item.Branch_Id;
                    worksheet.Cells[row, 7] = item.Supp_Website;
                    worksheet.Cells[row, 8] = item.Supp_Fax;
                    row++;
                }

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Path.Combine(directory, fileName));
                }
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
                var msg = Supplier.Import(excelfile);
                return RedirectToAction("Index", new { msg = msg });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { msg = ex.Message });

            }

        }

        public JsonResult CheckCode(string Supp_Code)
        {
            if (Supplier.containCode(Supp_Code))
            {
                var msg = "SUP- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: Supplier
        public ActionResult Index(string msg)
        {
            ViewBag.Message = msg;
            var suppliers = Supplier.GetAllSuppliers();
            return View(suppliers);
        }
        public ActionResult IndexnonActiveSuppliers(string msg)
        {
            ViewBag.Message = msg;
            var suppliers = Supplier.GetAllSuppliersnotActive();
            return View(suppliers);
        }
        // GET: Supplier/Details/5
        public ActionResult Details(int id)
        {
            var supplier = Supplier.GetSupplierById(id);
            return View(supplier);
        }

        // GET: Supplier/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء مورد");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var supplier = Supplier.AddSupplierVM();
            return View(supplier);
        }
        public FileResult ReturnsupFile(int fileId)
        {

            var document = Supplier.getFileById(fileId);
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
            var model = Supplier.GetAllFiles(empId);
            return View(model);
        }

        // POST: Supplier/Create
        [HttpPost]
        public ActionResult Create(SupplierVM suppVM)
        {
            try
            {
                var result = Supplier.AddNewSupplier(suppVM);
                
                if (result.Item2 == 0)
                {
                    return RedirectToAction("Index", new { msg = result.Item1 });
                }
                else
                {
                    return RedirectToAction("Details", new { id = result.Item2 });
                }

               
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var erorr = msg;
                return View(suppVM);
            }
        }

        // GET: Supplier/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل مورد");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            if (id == 0)
            {
                string msg = "Erorr, No Supplier exist";
                return RedirectToAction("Index", new { msg = msg });
            }

            var supplier = Supplier.FillViewModelForSupplier(id);

            return View(supplier);
        }


        // POST: Supplier/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, SupplierVM suppVM)
        {
            try
            {
                var supplier = Supplier.EditSupplier(suppVM);

                return RedirectToAction("Details", new {id=supplier.Supp_Id});
            }
            catch
            {
                return View();
            }
        }



        [HttpGet]
        public virtual JsonResult GetProvinces(int countryId)
        {
            var my = Supplier.GetCountryProvinces(countryId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetCities(int provinceId)
        {
            var my = Supplier.GetProvincesCities(provinceId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }




        // GET: Supplier/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Supplier/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Gets all customers in the form of Json array in the form specified by 
        /// Select2 (https://select2.org/data-sources/arrays) to populate 
        /// the drop-down list
        /// </summary>
        /// <returns> A Json array in the form specified by Select2  </returns>
        [HttpGet]
        public ActionResult getSuppliersJson()
        {
            List<Supplier> suppliers = Supplier.GetAllSuppliers();
            List<dynamic> result = new List<dynamic>();
            foreach (var supplier in suppliers)
            {
                var data = new
                {
                    // the value of each select item
                    id = supplier.Supp_Id,
                    // showing both code and name in the drop-down list 
                    text = supplier.Supp_Code + "\t\t\t" + supplier.Supp_Name
                };
                result.Add(data);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowAccountPayable()
        {
            return View();
        }

        public PartialViewResult GetPurInvoicesPartial(int id)
        {
            var purInvoices = Supplier.GetSupplierPurInvoices(id);
            return PartialView(purInvoices);
        }

        public PartialViewResult GetPaymentsPartial(int id)
        {
            var payments = Supplier.GetSupplierPayments(id);
            return PartialView(payments);
        }




     


        public ActionResult EditStatus(int id)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تغير حالة مورد");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }

                Supplier.UpdateSupplier(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var erorr = msg;
                var data = ex.Data;
                return View();
            }
        }
        
        public ActionResult IndexStopSuplier(int id)
        {
            var sasa = Supplier.GetAllSupplierStop();
            HideSupplier(id);
            return View(sasa);
        }
      
        public ActionResult HideSupplier(int id)
        {
            try
            {
                var msg = Supplier.StopSupplier(id);
                return RedirectToAction("Index", new  { msg = msg });
            }
            catch (Exception ex)
            {
                var ms = ex.Message;
                var dd = ex.Data;

                return View();
            }
        }

        public FileResult ReturnInvoicetFile(int fileId)
        {

            var document = Supplier.getFileById(fileId);
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

            var document = Supplier.getFileById(fileId);
            var delete = Supplier.deleteFile(fileId);
            return RedirectToAction("Details", new { id = document.SupplierSupp_Id });
        }

        public ActionResult ReturnSupplierToList(int id)
        {
            try
            {
                var msg = Supplier.ChangeSupplierType(id);
                return RedirectToAction("Index", new { msg = msg });
            }
            catch (Exception ex)
            {
                var ms = ex.Message;
                var dd = ex.Data;

                return View();
            }
        }
        public ActionResult getSupplierJsondrobDown()
        {
            List<Supplier> Suppliers = Supplier.getAllSuPPlierFordrobDownList();
            List<dynamic> result = new List<dynamic>();
            foreach (var Sub in Suppliers)
            {
                var data = new
                {
                    // the value of each select item
                    id = Sub.Supp_Id,
                    // showing both code and name in the drop-down list 
                    text = Sub.Supp_Name + "\t" + Sub.Supp_Code
                };
                result.Add(data);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}