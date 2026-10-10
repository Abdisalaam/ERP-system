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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class TreasuryController : Controller
    {
        public ActionResult Getexcel()
        {
            return File("~/FiLeDownLoad/Treasury.xlsx", "application/xlsx", Server.UrlEncode("~/FiLeDownLoad/Treasury.xlsx"));
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
                Treasury product = new Treasury();

                worksheet.Cells[1, 1] = "الزامى";
                worksheet.Cells[1, 2] = "غير الزامى";
               
               
                worksheet.Cells[1, 3] = "غير الزامى";

                worksheet.Cells[2, 1] = "اسم الصندوق";
                worksheet.Cells[2, 2] = "وصف الصندوق";
                
                worksheet.Cells[2, 3] = "رصيد الصندوق";

                worksheet.Cells[1, 9] = "الارصدة الدائنة";
                worksheet.Cells[2, 9] = "الارصدة المدينة";

                worksheet.Cells[1, 8] = "بالسالب";
                worksheet.Cells[2, 8] = "بالموجب";

                int row = 3;
                foreach (Treasury item in Treasury.getTreasuries())
                {
                    worksheet.Cells[row, 1] = item.Trea_Name;
                    worksheet.Cells[row, 2] = item.Trea_Desc;
                    
                    worksheet.Cells[row, 3] = item.Balance_Amount;

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
                var msg = Treasury.Import(excelfile);
                return RedirectToAction("Index", new { msg = msg });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { msg = ex.Message });

            }
        }





        // GET: Treasury
        public ActionResult Index(string msg)
        {
            var allTreasury = Treasury.getTreasuries();
            ViewBag.Message = msg;
            return View(allTreasury);
        }
        public ActionResult IndexNotActive(String msg)
        {
            var models = Treasury.GetAllNotActive();
            ViewBag.message = msg;
            return View(models);
        }

        // GET: Treasury/Details/5
        public ActionResult Details(int id)
        {
            var thistreasury = Treasury.getTreaById(id);
            return View(thistreasury);
        }

        // GET: Treasury/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء صندوق");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var VMtoCreate = Treasury.Create();
            return View(VMtoCreate);
        }

        // POST: Treasury/Create
        [HttpPost]
        public ActionResult Create(TreaVM TVM)
        {
            //try
            //{
              var tre=  Treasury.CreateTreasury(TVM);
            if (tre != null)
            {
                return RedirectToAction("Details", new { id = tre.Trea_Id });
            }
            else
            {
                string msg = Resources.validate.Error;
                return RedirectToAction("Index", new { msg = msg });
            }

           
            //}
            //catch (Exception ex)
            //{
            //    return View(ex.Message);
            //}
        }

        // GET: Treasury/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل صندوق");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var thisTre = Treasury.FillTheViewModel(id);
            return View(thisTre);
        }

        // POST: Treasury/Edit/5
        [HttpPost]
        public ActionResult Edit(TreaVM TVM)
        {
            try
            {

               var tre= Treasury.EditTrea(TVM);

                if (tre != null)
                {
                    return RedirectToAction("Details", new { id = tre.Trea_Id });
                }
                else
                {
                    string msg = Resources.validate.Error;
                    return RedirectToAction("Index", new { msg = msg });
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Treasury/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Treasury/Delete/5
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
        public ActionResult DeleteTrans(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ت غير نشط خزينة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = Treasury.DeleteTransfer(id);
            return RedirectToAction("Details", new { id = id });
        }
    }
}
