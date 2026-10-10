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
using System.Web.Hosting;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        public ActionResult Getexcel()
        {
            return File("~/FiLeDownLoad/Customer.xlsx", "application/xlsx", Server.UrlEncode("~/FiLeDownLoad/Customer.xlsx"));
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
                Customer cust = new Customer();

                worksheet.Cells[1, 1] = "الزامى";
                worksheet.Cells[1, 2] = "الزامى";
                worksheet.Cells[1, 3] = "الزامى";
                worksheet.Cells[1, 4] = "غير الزامى";
                worksheet.Cells[1, 5] = "الزامى";
                //worksheet.Cells[1, 6] = "غير الزامى";
                //worksheet.Cells[1, 7] = "غير الزامى";
                //worksheet.Cells[1, 8] = "غير الزامى";
                //worksheet.Cells[1, 6] = "غير الزامى ";
                worksheet.Cells[1, 6] = "غير الزامى";
                worksheet.Cells[1, 7] = "غير الزامى";
                //worksheet.Cells[1, 9] = "غير الزامى";
                worksheet.Cells[1, 8] = "غير الزامى";
                worksheet.Cells[1, 9] = "غير الزامى";


                worksheet.Cells[2, 1] = "اسم العميل";
                worksheet.Cells[2, 2] = "تليفون العميل";
                worksheet.Cells[2, 3] = "رصيد العميل";
                worksheet.Cells[2, 4] = "ايميل العميل";
                worksheet.Cells[2, 5] = "رقم الشريحة";
                //worksheet.Cells[2, 6] = "المدينة";
                //worksheet.Cells[2, 7] = "المحافظة";
                //worksheet.Cells[2, 8] = "الدولة";
                //worksheet.Cells[2, 6] = "طريقة الدفع";
                worksheet.Cells[2, 6] = "عنوان العميل";
                worksheet.Cells[2, 7] = "جوال العميل";
                //worksheet.Cells[2, 9] = "رقم الفرع";
                worksheet.Cells[2, 8] = "ويب سايت العميل";
                worksheet.Cells[2, 9] = "فاكس العميل";



                worksheet.Cells[1, 18] = "الارصدة الدائنة";
                worksheet.Cells[2, 18] = "الارصدة المدينة";

                worksheet.Cells[1, 17] = "بالسالب";
                worksheet.Cells[2, 17] = "بالموجب";


                int row = 3;
                foreach (Customer item in Customer.GetAllCustomer())
                {
                    //worksheet.Cells[row, 1] = item.Customer_Id;
                    worksheet.Cells[row, 1] = item.Customer_Name;
                    worksheet.Cells[row, 2] = item.Customer_Phone;
                    worksheet.Cells[row, 3] = item.Customer_Balance;
                    worksheet.Cells[row, 4] = item.Customer_Mail;
                    worksheet.Cells[row, 5] = item.SlicesSlice_Id;
                    //worksheet.Cells[row, 6] = item.Customer_City;
                    //worksheet.Cells[row, 7] = item.Customer_Province;
                    //worksheet.Cells[row, 8] = item.Customer_Country;
                    //worksheet.Cells[row, 9] = item.Payment_Policy_Id;
                    worksheet.Cells[row, 6] = item.Customer_Address;
                    worksheet.Cells[row, 7] = item.Customer_Mobile;

                    //worksheet.Cells[row, 12] = item.Branch_Id;
                    worksheet.Cells[row, 8] = item.Customer_Website;
                    worksheet.Cells[row, 9] = item.Customer_Fax;
                    row++;
                }

                //workbook.get_Range("A1", "O1").EntireColumn.AutoFite();
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Path.Combine(directory, fileName));
                }
          
                // workbook.SaveAs("E:\\Files excel\\myCustomersList.xls");
                workbook.Close();
                Marshal.ReleaseComObject(workbook);
                application.Quit();
                Marshal.FinalReleaseComObject(workbook);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            try
            {

                var Message = Customer.Import(excelfile);
                return RedirectToAction("Index", new { Message = Message });
            }
            catch( Exception ex)
            {
                return RedirectToAction("Index", new { Message = ex.Message });
            }
           
        }

        public JsonResult CheckCode(string Customer_Code)
        {
            if (Customer.containCode(Customer_Code))
            {
                var Message = "CUST- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        //AccountingContainer db = new AccountingContainer();
        // GET: Customer
        public ActionResult Index(string Message)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                ViewBag.Message = Message;
                var customer = Customer.GetAllCustomerNew(user.Id);
                return View(customer);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
                if (cookie != null && cookie.Value != null)
                {
                    if (cookie.Value == "en")
                    {
                        ViewBag.Message = ex.Message + "___" + "Please review this error and try to correct it";
                    }
                    else
                    {
                        ViewBag.Message = ex.Message + "__" + "هناك خطأ في البيانات. الرجاء التاكد من صحتها";
                    }
                }
                else
                {

                    ViewBag.Message = ex.Message + "__" + "هناك خطأ في البيانات. الرجاء التاكد من صحتها";
                }

               
                return View();
            }

            //return new ViewAsPdf("Index", customer);
        }
        public ActionResult IndexCustomerNotActive(string Message)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            ViewBag.Message = Message;
            var customer = Customer.GetAllCustomernotactiveNew(user.Id);
            return View(customer);

            //return new ViewAsPdf("Index", customer);
        }

        [HttpGet]

        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var customer = Customer.GetCustomerById(id);
                return View(customer);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
                if (cookie != null && cookie.Value != null)
                {
                    if (cookie.Value == "en")
                    {
                        ViewBag.Message = ex.Message + "___" + "Please review this error and try to correct it";
                    }
                    else
                    {
                        ViewBag.Message = ex.Message + "__" + "هناك خطأ في البيانات. الرجاء التاكد من صحتها";
                    }
                }
                else
                {

                    ViewBag.Message = ex.Message + "__" + "هناك خطأ في البيانات. الرجاء التاكد من صحتها";
                }
               
                return View();
            }
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء عميل");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var customer = Customer.AddCustomerVM();
                return View(customer);
            }
            catch (Exception ex)
            {

                var Message = ex.Message;
                var erorr = Message;
                return View();
            }

        }
        public ActionResult GetAllFiles(int invoiceMasterId)
        {
            var model = Customer.GetAllFiles(invoiceMasterId);
            return View(model);
        }
        public FileResult ReturnInvoicetFile(int fileId)
        {

            var document = Customer.getFileById(fileId);
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

            var document = Customer.getFileById(fileId);
            var delete = Customer.deleteFile(fileId);
            return RedirectToAction("Details", new { id = document.CustomerCustomer_Id });
        }
        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(CustomerVM custmv)
        {
            try
            {

                var result = Customer.AddNewCustomer(custmv);
              
               
                    //var masg = "تمت اضافة العميل بنجاح";
              
                return RedirectToAction("Details", new { id = result });



            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
                if (cookie != null && cookie.Value != null)
                {
                    if (cookie.Value == "en")
                    {
                        ViewBag.Message = ex.Message + "___" + "Please review this error and try to correct it";
                    }
                    else
                    {
                        ViewBag.Message = ex.Message + "__" + "هناك خطأ في البيانات. الرجاء التاكد من صحتها";
                    }
                }
                else
                {

                    ViewBag.Message = ex.Message + "__" + "هناك خطأ في البيانات. الرجاء التاكد من صحتها";
                }
               
                var vm = Customer.AddCustomerVM();
                return View(vm);
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل عميل");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                string Message = "Erorr, No customer exist";
                return RedirectToAction("Index", new { Message = Message });
            }

            var customer = Customer.FillCustomerForEditing(id);

            return View(customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CustomerVM custVM)
        {
            try
            {
                var customer = Customer.Editcustomer(custVM);

                return RedirectToAction("Details", new { id = custVM._Customer_Id });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
                if (cookie != null && cookie.Value != null)
                {
                    if (cookie.Value == "en")
                    {
                        ViewBag.Message = ex.Message + "___" + "Please review this error and try to correct it";
                    }
                    else
                    {
                        ViewBag.Message = ex.Message + "__" + "هناك خطأ في البيانات. الرجاء التاكد من صحتها";
                    }
                }
                else
                {

                    ViewBag.Message = ex.Message + "__" + "هناك خطأ في البيانات. الرجاء التاكد من صحتها";
                }
               
                var vm = Customer.FillCustomerForEditing(custVM._Customer_Id);
                return View(vm);
            }
        }


        [HttpGet]
        public virtual JsonResult GetProvinces(int countryId)
        {
            var my = Customer.GetCountryProvinces(countryId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetCities(int provinceId)
        {
            var my = Customer.GetProvincesCities(provinceId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }


        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Customer/Delete/5
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
        public ActionResult getCustomersJson()
        {
            List<Customer> customers = Customer.GetAllCustomer();
            List<dynamic> result = new List<dynamic>();
            foreach (var customer in customers)
            {
                var data = new
                {
                    // the value of each select item
                    id = customer.Customer_Id,
                    // showing both code and name in the drop-down list 
                    text = customer.Customer_Code + "\t" + customer.Customer_Name
                };
                result.Add(data);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ShowAccountReceivable()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult GetInvoicesPartial(int id)
        {
            IEnumerable<InvoiceMaster> invoices = Customer.getCustomerInvoices(id);
            return PartialView(invoices);
        }



        [HttpGet]
        public PartialViewResult GetReceiptsPartial(int id)
        {
            IEnumerable<Receipt> receipts = Customer.getCustomerReceipts(id);
            return PartialView(receipts);
        }

        public ActionResult EditStatus(int id)
        {
            try
            {

                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تغير حالة عميل");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                Customer.UpdateCustomerState(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                var erorr = Message;
                var data = ex.Data;
                return View();
            }
        }

        public ActionResult IndexStopedSup(int id)
        {
            //ViewBag.Message = Message;
            var models = Customer.GetAllCustomerBal();
            hideCustomer(id);
            return View(models);
        }

        public ActionResult hideCustomer(int id)
        {
            try
            {
                // TODO: Add insert logic here
                var Message = Customer.HiddenCustomer(id);

                return RedirectToAction("Index", new { Message = Message });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult PrepareCustomercontent()
        {
            var invoic = Customer.preperCustomer();
            return PartialView("ContentCustomer", invoic);

        }


        public ActionResult GetAllContentResult(int TempComtent)
        {
            var TempDetails = Customer.GetAllCustomerContent(TempComtent);
            return PartialView("ContentDetailsResult", TempDetails);
        }

        public virtual JsonResult AddCustomerToContext(ContentPersonDetailsVM ContentDetails)
        {
            string Message = "";
            try
            {
                Message = Customer.DddContextToTemp(ContentDetails);
            }
            catch (Exception m)
            {

                Message = m.Message;
                var mol = m.Data;
                throw;
            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustResult2(int TempComtent)
        {
            var TempDetails = Customer.GetNewCustomer(TempComtent);
            return PartialView("ContentDetailsResult", TempDetails);
        }



        public virtual JsonResult AddInvoiceDetailToMaster(ContentPersonDetailsVM ContentDetails)
        {
            string Message = "";
            try
            {
                Message = Customer.DddContextToTemp(ContentDetails);
            }
            catch (Exception m)
            {

                Message = m.Message;
                var mol = m.Data;
                throw;
            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CreateNewCustomer(int id)
        {
            try
            {
                var customer = Customer.CreatforCreation(id);
                return View(customer);
            }
            catch (Exception ex)
            {

                var Message = ex.Message;
                var erorr = Message;
                return View();
            }

        }
        [HttpPost]

        public ActionResult CreateNewCustomer(CustomerVM custmv)
        {
            try
            {

                var result = Customer.CreateCustomer(custmv);
                if (ModelState.IsValid)
                {
                    var masg = Resources.ReceiptCreate.CreateSuccess;
                    return RedirectToAction("Index", new { masg = masg });
                }

                return View(custmv);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                var erorr = Message;
                return View(custmv);
            }
        }

        public ActionResult PrepareCustomerDetails(int masterId)
        {
            var invoic = Customer.PreperCustomerTemp(masterId);
            return PartialView("CustomerDetailsCreationEdit", invoic);

        }


        [HttpGet]
        public ActionResult GetAllCustomerDetailsEdit(int CustomerMasterId)
        {
            var CustDetailVm = Customer.GetAllContentCustumerDetails(CustomerMasterId);
            return PartialView("CustDetailsForEdit", CustDetailVm);
        }



        public ActionResult GetAllTempContenetCustomDetailsresult(int tempCusyomerMasterId)
        {
          var invoiceDetailVm = Customer.GetAllTempCustomerDetailsresult2(tempCusyomerMasterId);
            return PartialView("CustomerContenetDetailResultList", invoiceDetailVm);
        }
        public ActionResult Deletetemp(int id)
        {
            var Detail = Customer.DeleteContentFromTemp(id);
            return Json(Detail, JsonRequestBehavior.AllowGet);
        }
        //make drobDeowList search for Customer
        public ActionResult getCustomerJsondrobDown()
        {
            List<Customer> customers = Customer.getAllCustomerFordrobDownList();
            List<dynamic> result = new List<dynamic>();
            foreach (var cust in customers)
            {
                var data = new
                {
                    // the value of each select item
                    id = cust.Customer_Id,
                    // showing both code and name in the drop-down list 
                    text = cust.Customer_Name + "\t" + cust.Customer_Code
                };
                result.Add(data);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PartialDialogSlice()
        {
            var slice = Slices.AddSlicesForCreation();
            return PartialView("SelicePartialView", slice);
        }
     
       [HttpPost]
        public virtual JsonResult PartialDialogSlicePost(SlicesVM slicesVM)
        {
            var slice = Slices.CreateSlice(slicesVM);
            return Json(slice, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetSlicDropdownList()
        {
          
            var slice = Customer.GetSlicesList();
            return new JsonResult { Data = slice, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

   


        //public ActionResult ExportToExcel()
        //{
        //    var gv = new GridView();
        //    gv.DataSource = Customer.GetAllCustomer();

        //    gv.DataBind();
        //    Response.ClearContent();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
        //    Response.AddHeader("Content-Type", "application/vnd.ms-excel");
        //    //Response.Charset = "UTF-8";
        //    Response.ContentType = "application/excel";
        //    //Response.ContentType = "application/vnd.ms-excel";
        //    //Response.ContentType = "Application/x-msexcel";

        //    Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252");

        //    Encoding encoding = Encoding.UTF8;
        //    Response.Charset = encoding.EncodingName;

        //    //Response.Charset = "";
        //    StringWriter objStringWriter = new StringWriter();
        //    HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
        //    gv.RenderControl(objHtmlTextWriter);
        //    Response.Output.Write(objStringWriter.ToString());
        //    Response.Flush();
        //    Response.End();
        //    return RedirectToAction("Index");
        //}


    }
}