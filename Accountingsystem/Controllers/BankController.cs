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

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class BankController : Controller
    {
        public ActionResult Getexcel()
        {
            return File("~/FiLeDownLoad/Bank.xlsx", "application/xlsx", Server.UrlEncode("~/FiLeDownLoad/Bank.xlsx"));
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
                Bank product = new Bank();



                worksheet.Cells[1, 1] = "الزامى";
                worksheet.Cells[1, 2] = "غير الزامى";
                worksheet.Cells[1, 3] = "الزامى";
                worksheet.Cells[1, 4] = "الزامى";
                worksheet.Cells[1, 5] = "الزامى";

                worksheet.Cells[2, 1] = "اسم البنك";
                worksheet.Cells[2, 2] = "عنوان البنك";
                worksheet.Cells[2, 3] = "رقم حساب البنك";
                worksheet.Cells[2, 4] = "رقم ايبان البنك";
                worksheet.Cells[2, 5] = "رصيد البنك";


                worksheet.Cells[1, 9] = "الارصدة الدائنة";
                worksheet.Cells[2, 9] = "الارصدة المدينة";

                worksheet.Cells[1, 8] = "بالسالب";
                worksheet.Cells[2, 8] = "بالموجب";

                int row = 3;
                foreach (Bank item in Bank.GetAllBank())
                {
                    worksheet.Cells[row, 1] = item.Bank_Name;
                    worksheet.Cells[row, 2] = item.Bank_Address;
                    worksheet.Cells[row, 3].Value = item.Bank_Number;
                    worksheet.Cells[row, 4] = item.Bank_Eban_Number;
                    worksheet.Cells[row, 5] = item.Balance_Amount;
                   
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
                var msg = Bank.Import(excelfile);

                return RedirectToAction("Index", new { msg = msg });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { msg = ex.Message });

            }
        }

        public JsonResult CheckCode(string Balance_Code)
        {
            if (Bank.containCodeOpen(Balance_Code))
            {
                var msg = "OPB- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: Bank
        public ActionResult Index(string Msg)
        {
            ViewBag.Msg = Msg;
            var Banks = Bank.GetAllBank();
            return View(Banks);
        }
        public ActionResult IndexNotActive(String msg)
        {
            var models = Bank.GetAllNotActive();
            ViewBag.message = msg;
            return View(models);
        }

        // GET: Bank/Details/5
        public ActionResult Details(int id)
        {
            
            var bank = Bank.GetBankById(id);
            return View(bank);
        }

        // GET: Bank/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء بنك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var bank = Bank.AddBankVM();
            return View(bank);
        }

        // POST: Bank/Create
        [HttpPost]
        public ActionResult Create(BankVM bvm)
        {
            try
            {
                // TODO: Add insert logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var NewBank = Bank.AddNewBank(bvm , user.Id);
                if (NewBank != null)
                {
                    return RedirectToAction("Details", new { id = NewBank.Bank_ID });
                }
                else
                {
                    string msg = Resources.validate.Error;
                    return RedirectToAction("Index", new { Msg = msg });
                }
              

            }
            catch
            {
                return View(bvm);
            }
        }

        // GET: Bank/Edit/5
        public ActionResult Edit(int id)
        {
            if (id < 1)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل بنك");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                string Msg = Resources.validate.Error;
                return RedirectToAction("Index", new { Msg = Msg });
            }
            var Bank = Models.Bank.FillBank(id);
            return View(Bank);
        }

        // POST: Bank/Edit/5
        [HttpPost]
        public ActionResult Edit(BankVM bvm)
        {
            try
            {
                // TODO: Add update logic here
                var bank = Bank.editbank(bvm);
                if (bank != null)
                {
                    return RedirectToAction("Details", new { id = bank.Bank_ID });
                }
                else
                {
                    string msg = Resources.validate.Error;
                    return RedirectToAction("Index", new { Msg = msg });
                }
                
            }
            catch
            {
                return View();
            }
        }

        // GET: Bank/Delete/5
        public ActionResult Delete(int id)
        {
            var bank = Bank.GetBankById(id);
            return View(bank);
        }

        // POST: Bank/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var bank = Bank.DeleteBank(id);
                if (bank == 1)
                {
                    return RedirectToAction("Index");
                }
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

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ت غير نشط بنك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = Bank.DeleteTransfer(id);
            return RedirectToAction("Details", new { id = id });
        }
    }
}