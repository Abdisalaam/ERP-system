using Accountingsystem.Utilites;
using Accountingsystem.ViewModel;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Accountingsystem.Models
{
    public partial class Supplier
    {
        /// <summary>
        /// check that code unique 
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns>bool</returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.Suppliers.ToList();
            foreach (var item in allinData)
            {
                if (item.Supp_Code == Serial || !Serial.StartsWith("SUP-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }

        // use this method to  Get List of Supplier from Data base. 
        public static List<Supplier> GetAllSuppliers()
        {
            var db = new AccountingContainer();
            var suppliers = db.Suppliers.Where(S => S.Supplier_Bal == true && (S.Supp_Balance >= 0 || S.Supp_Balance <= 0) && S.Supplier_state == Models.Customer_State.نشط).ToList();
            var account = db.Accounts.ToList();
            foreach (var sup in suppliers)
            {
                sup.Supp_Balance = 0;
             
                if (sup.Supp_Phone == null || sup.Supp_Phone == "")
                {
                    sup.Supp_Phone = "1";
                    db.Entry(sup).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var suppaccount = db.Accounts.Where(a => a.Account_Code == sup.Supp_Code).FirstOrDefault();
                if (suppaccount != null)
                {
                    sup.Supp_Balance += suppaccount.Account_Balance;
                }
                //foreach (var acc in account)
                //{
                //    if (acc.Account_Code == sup.Supp_Code)
                //    {
                //        sup.Supp_Balance += acc.Account_Balance;
                //    }
                //    db.Entry(sup).State = EntityState.Modified;
                //    db.SaveChanges();
                //}
            }
            return suppliers;
        }
        /// <summary>
        /// get all supplier the status is not active
        /// </summary>
        /// <returns>List<Supplier></returns>
        public static List<Supplier> GetAllSuppliersnotActive()
        {
            var db = new AccountingContainer();
            var suppliers = db.Suppliers.Where(S => S.Supplier_Bal == true && (S.Supp_Balance >= 0 || S.Supp_Balance <= 0)&&S.Supplier_state==Models.Customer_State.غير_نشط).ToList();
            var account = db.Accounts.ToList();
            foreach (var sup in suppliers)
            {
                sup.Supp_Balance = 0;

                if (sup.Supp_Phone == null || sup.Supp_Phone == "")
                {
                    sup.Supp_Phone = "1";
                    db.Entry(sup).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var suppaccount = db.Accounts.Where(a => a.Account_Code == sup.Supp_Code).FirstOrDefault();
                if (suppaccount != null)
                {
                    sup.Supp_Balance += suppaccount.Account_Balance;
                }
                //foreach (var acc in account)
                //{
                //    if (acc.Account_Code == sup.Supp_Code)
                //    {
                //        sup.Supp_Balance += acc.Account_Balance;
                //    }
                //    db.Entry(sup).State = EntityState.Modified;
                //    db.SaveChanges();
                //}
            }
            return suppliers;
        }
        /// <summary>
        /// get the file by its id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>SupplierFileDetail</returns>
        public static SupplierFileDetail getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.SupplierFileDetails.Find(fileId);
            return file;
        }
        public static string deleteFile(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.SupplierFileDetails.Find(fileId);
            db.SupplierFileDetails.Remove(file);
            db.SaveChanges();
            return "تم حذف الملف ";
        }
        /// <summary>
        /// get all files that belong to that supplier by its supplier id
        /// </summary>
        /// <param name="supId"></param>
        /// <returns></returns>
        public static List<SupplierFileDetail> GetAllFiles(int supId)
        {
            var db = new AccountingContainer();
            var files = db.SupplierFileDetails.Where(emp => emp.SupplierSupp_Id == supId).ToList();
            return files;
        }

        // use this method to  Get one Element SuppliersById from Data base. 
        public static Supplier GetSupplierById(int id)
        {
            var db = new AccountingContainer();
            var supplier = db.Suppliers.Find(id);

            return supplier;
        }
        // use this method to  Get one Element SuppliersByCode from Data base.
        public static Supplier GetSupplierByCode(string code)
        {
            var db = new AccountingContainer();
            var supplier = db.Suppliers.Where(s => s.Supp_Code == code);
            var _supplier = new Supplier();
            if (supplier != null)
            {
                _supplier = supplier.FirstOrDefault();
                return _supplier;
            }

            return null;
        }
        // use this method For Creation new Supplier and get Data from user.
        public static SupplierVM AddSupplierVM()
        {
            var db = new AccountingContainer();
            var supplierVM = new SupplierVM();
           
            var lastSerial = db.Suppliers.OrderByDescending(c => c.Supp_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                supplierVM.Supp_Code = "SUP-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                supplierVM.Supp_Code = "SUP-" + (Convert.ToInt32(lastSerial.Supp_Code.Substring(4, lastSerial.Supp_Code.Length - 4)) + 1).ToString("D3");
            }

           // supplierVM.SelectedCountryId = db.Countries.FirstOrDefault().Country_Id;
            //supplierVM.SelectedProvinceId = db.Provinces.Where(myProvince => myProvince.CountryId == supplierVM.SelectedCountryId).FirstOrDefault().Province_Id;
            supplierVM.MyCountryList = new SelectList(db.Countries, "Country_Id", "Country_Name");
            supplierVM.MyProvinceList = new SelectList(db.Provinces, "Province_Id", "Province_Name");
            //supplierVM.SelectedProvinceId = supplierVM.SelectedProvinceId;
            supplierVM.MyCitiesList = new SelectList(db.Cities, "City_Id", "City_Name");
            supplierVM.BranchList = new SelectList(db.Branches, "Branch_Id", "Branch_Name");
            supplierVM.MyPaymentPolicy = new SelectList(db.PaymentMethods, "PaymentMethod_Id", "Payment_Type");
            supplierVM.MyChartOfAccountList = new SelectList(db.ChartOfAccountings.Where(accCode => accCode.Account_Code.StartsWith("020101")), "Account_Id", "Account_Name");
            return supplierVM;
        }

        // use this method to  Create New Supplier and save it in DataBase.
        // use this method to  Create New Supplier and save it in DataBase.
        public static Tuple<string,int> AddNewSupplier(SupplierVM supplierVm)
        {
            try
            {
                var db = new AccountingContainer();
                var Supplier = new Supplier();

                if (supplierVm != null)
                {
                    // create selectList from Database to entity Contry.
                    var resultCountry = db.Countries.Find(supplierVm.SelectedCountryId);
                    if (resultCountry != null)
                    {
                        Supplier.Supp_Country = resultCountry.Country_Name;
                    }
                    // create selectList from Database to entity Province.

                    var resultProvinces = db.Provinces.Find(supplierVm.SelectedProvinceId);
                    if (resultProvinces != null)
                    {
                        Supplier.Supp_Province = resultProvinces.Province_Name;
                    }
                    // create selectList from Database to entity city.
                    var resultCities = db.Cities.Find(supplierVm.SelectedCityId);
                    if (resultCities != null)
                    {
                        Supplier.Supp_City = resultCities.City_Name;
                    }
                    var resultbranch = db.Branches.Find(supplierVm.Branch_Id);
                    if (resultbranch != null)
                    {
                        Supplier.Branch_Id = resultbranch.Branch_Id;
                    }
                    var resultPaymentPolicy = db.PaymentMethods.Find(supplierVm.Payment_Policy_Id);
                    if (resultPaymentPolicy != null)
                    {
                        Supplier.Payment_Policy_Id = resultPaymentPolicy.PaymentMethod_Id;
                    }

                    Supplier.Supp_Name = supplierVm.Supp_Name;
                    Supplier.Supp_Phone = supplierVm.Supp_Phone;
                    Supplier.Supp_Mail = supplierVm.Supp_Mail;
                    supplierVm.SelectedChartOfAccountId = 45;
                    Supplier.Country = supplierVm.Country;
                    Supplier.Province = supplierVm.Province;
                    Supplier.City = supplierVm.City;
                    var ChartOfAccount = db.ChartOfAccountings.Find(supplierVm.SelectedChartOfAccountId);
                    var ChartOfAccountParent = db.ChartOfAccountings.Find(ChartOfAccount.ChartOfAccountingAccount_Id);

                    //var lastCode = (from s in db.Suppliers
                    //                select s.Supp_Id).ToList().LastOrDefault();
                    //int addingToLastDigit = Convert.ToInt32(lastCode) + 1;

                    //var lastSerial = db.Suppliers.OrderByDescending(c => c.Supp_Id).FirstOrDefault();
                    //if (lastSerial == null)
                    //{
                    //    Supplier.Supp_Code = "SUP-001";
                    //}
                    //else
                    //{
                    //    //using string substring method to get the number of the last inserted
                    //    Supplier.Supp_Code = "SUP-" + (Convert.ToInt32(lastSerial.Supp_Code.Substring(4, lastSerial.Supp_Code.Length - 4)) + 1).ToString("D3");
                    //}

                    Supplier.Supp_Fax = supplierVm.Supp_Fax;
                    Supplier.Supp_Address = supplierVm.Supp_Address;
                    Supplier.Supp_Mobile = supplierVm.Supp_Mobile;
                    Supplier.Purchase_Price = supplierVm.Purchase_Price;
                    Supplier.Supp_Website = supplierVm.Supp_Website;
                    Supplier.Tax_Number = supplierVm.Tax_Number;
                    Supplier.Supp_Balance = 0;
                    Supplier.Supplier_state = Customer_State.نشط;
                    Supplier.Supplier_Bal = true;


                    // var subledger = new SubLedger();
                    // subledger.Account_Id = ChartOfAccount.Account_Id.ToString();
                    // subledger.SubLed_Code = Supplier.Supp_Code;
                    //// var date = DateTime.Now.ToString();
                    // subledger.SubLed_Date = DateTime.Now.Date;
                    // subledger.SubLed_Name = Supplier.Supp_Name;
                    // subledger.SubLed_Type = ChartOfAccount.Account_Name;
                    // subledger.SubLed_Desc = ChartOfAccountParent.Account_Name;
                    // subledger.SubLedger_Balance = 0;
                    // db.SubLedgers.Add(subledger);

                    var lastSerial = db.Suppliers.OrderByDescending(c => c.Supp_Id).FirstOrDefault();

                    if (lastSerial == null)
                    {
                        Supplier.Supp_Code = "SUP-001";
                    }
                    else
                    {
                        Supplier.Supp_Code = "SUP-" + (Convert.ToInt32(lastSerial.Supp_Code.Substring(4, lastSerial.Supp_Code.Length - 4)) + 1).ToString("D3");

                    }
                    if (supplierVm.SuppFiles2 != null) { 
                    if (supplierVm.SuppFiles2.FirstOrDefault() != null)
                    {
                        foreach (var file in supplierVm.SuppFiles2)
                        {
                            var filedetails = new SupplierFileDetail();
                            var data = new byte[file.ContentLength];
                            filedetails.FileName = file.FileName;
                            filedetails.ContentLength = file.ContentLength;
                            filedetails.File_Type = file.ContentType;
                            file.InputStream.Read(data, 0, file.ContentLength);
                            filedetails.ProImg = data;
                            filedetails.SupplierSupp_Id = Supplier.Supp_Id;
                            if (data.Length < 1000000)
                            {
                                Supplier.SupplierFileDetails.Add(filedetails);
                            }

                        }
                    }
                }
                    if (supplierVm.SuppFiles != null)
                    {
                        for (int i = 0; i < supplierVm.SuppFiles.Count; i++)
                        {


                            var filedetails = new SupplierFileDetail();
                            var length = supplierVm.SuppFiles[i].InputStream.Length; //Length: 103050706
                            MemoryStream target = new MemoryStream();
                            supplierVm.SuppFiles[i].InputStream.CopyTo(target); // generates problem in this line
                            byte[] data = target.ToArray();
                            filedetails.FileName = supplierVm.SuppFiles[i].FileName;
                            filedetails.File_Type = supplierVm.SuppFiles[i].ContentType;
                            filedetails.ContentLength = supplierVm.SuppFiles[i].ContentLength;
                            filedetails.ProImg = data;
                            filedetails.SupplierSupp_Id = Supplier.Supp_Id;
                            if (data.Length < 1000000)
                            {
                                Supplier.SupplierFileDetails.Add(filedetails);
                            }

                        }
                    }
                    db.Suppliers.Add(Supplier);

                    var accounts = new Accounts();
                    accounts.Account_Id = ChartOfAccount.Account_Id.ToString();
                    accounts.Account_Code = Supplier.Supp_Code;
                    var date = DateTime.Now.ToString();
                    accounts.Account_Date = DateTime.Now.Date;
                    accounts.Account_Name = Supplier.Supp_Name;
                    accounts.Account_Type = ChartOfAccount.Account_Name;
                    accounts.Account_Desc = ChartOfAccountParent.Account_Name;
                    accounts.Account_Balance = 0;
                    db.Accounts.Add(accounts);

                    db.SaveChanges();
                    db.Dispose();
                    return Tuple.Create( "تمت اضافة المورد بنجاح",Supplier.Supp_Id);

                }
            }
            catch (Exception e)
            {
                var masg = e.Message;
                var result = e.Data;

                throw;
            }

            return Tuple.Create( "لم يتم اضافة المورد",0);

        }
        

        // use this method For Edite Supplier and save it DataBase.
        public static  Supplier EditSupplier(SupplierVM suppVM)
        {
            var db = new AccountingContainer();
            var MySupplier = db.Suppliers.Find(suppVM.Supp_Id);
            var sub = db.Accounts.Where(a => a.Account_Name == MySupplier.Supp_Name && a.Account_Code.StartsWith("SUP")).FirstOrDefault();
            if (MySupplier != null)
            {
                // create selectList from Database to entity Country .
                var resultCountry = db.Countries.Find(suppVM.SelectedCountryId);
                if (resultCountry != null)
                {
                    MySupplier.Supp_Country = resultCountry.Country_Name;
                }

                // create selectList from Database to entity Provinces.
                var resultProvinces = db.Provinces.Find(suppVM.SelectedProvinceId);
                if (resultProvinces != null)
                {
                    MySupplier.Supp_Province = resultProvinces.Province_Name;
                }
                // create selectList from Database to entity city.
                var resultCities = db.Cities.Find(suppVM.SelectedCityId);
                if (resultCities != null)
                {
                    MySupplier.Supp_City = resultCities.City_Name;
                }
                var resultbranch = db.Branches.Find(suppVM.Branch_Id);
                if (resultbranch != null)
                {
                    MySupplier.Branch_Id = resultbranch.Branch_Id;
                }

                var resultPaymentPolicy = db.PaymentMethods.Find(suppVM.Payment_Policy_Id);
                if (resultPaymentPolicy != null)
                {
                    MySupplier.Payment_Policy_Id = resultPaymentPolicy.PaymentMethod_Id;
                }
                
                MySupplier.Supp_Name = suppVM.Supp_Name;
                MySupplier.Supp_Phone = suppVM.Supp_Phone;
                MySupplier.Supp_Mail = suppVM.Supp_Mail;
                MySupplier.Supp_Fax = suppVM.Supp_Fax;
                MySupplier.Supp_Address = suppVM.Supp_Address;
                MySupplier.Supp_Mobile= suppVM.Supp_Mobile;
                MySupplier.Purchase_Price = suppVM.Purchase_Price;
                MySupplier.Supp_Website = suppVM.Supp_Website;
                MySupplier.Tax_Number = suppVM.Tax_Number;
                MySupplier.Country = suppVM.Country;
                MySupplier.Province = suppVM.Province;
                MySupplier.City = suppVM.City;
                if (suppVM.SuppFiles2.FirstOrDefault() != null)
                {
                    foreach (var file in suppVM.SuppFiles2)
                    {
                        var filedetails = new SupplierFileDetail();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        filedetails.File_Type = file.ContentType;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.ProImg = data;
                        filedetails.SupplierSupp_Id = MySupplier.Supp_Id;
                        if (data.Length < 1000000)
                        {
                            MySupplier.SupplierFileDetails.Add(filedetails);
                        }
                       
                    }
                }
                db.Entry(MySupplier).State = EntityState.Modified;
                sub.Account_Name = suppVM.Supp_Name;
                db.Entry(sub).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();

                return MySupplier;
            }
            return null;
        }
        // use this method to load Details of Data for Supplier.
        public static SupplierVM FillViewModelForSupplier(int suppId)
        {
            var db = new AccountingContainer();
            var SupplierVM = new SupplierVM();
            var Supplier = db.Suppliers.Find(suppId);

            var countryName = Supplier.Supp_Country;
            var provinceName = Supplier.Supp_Province;
            var cityName = Supplier.Supp_City;
            var branchid = Supplier.Branch_Id;
            var PaymentPolicy = Supplier.Payment_Policy_Id;
            //  select Countrie from entity Countrie .
            var Countries = db.Countries.Where(c => c.Country_Name == countryName);
            if (Countries.Any())
            {
                SupplierVM.SelectedCountryId = Countries.FirstOrDefault().Country_Id;
            }
            //  select province from entity province .
            var province = db.Provinces.Where(c => c.Province_Name == provinceName);
            if (province.Any())
            {
                SupplierVM.SelectedProvinceId = province.FirstOrDefault().Province_Id;
            }
            //  select city from entity city .
            var city = db.Cities.Where(c => c.City_Name == cityName);
            if (city.Any())
            {
                SupplierVM.SelectedCityId = city.FirstOrDefault().City_Id;
            }
            var branch = db.Branches.Where(c => c.Branch_Id == branchid);
            if (branch.Any())
            {
                SupplierVM.Branch_Id = branch.FirstOrDefault().Branch_Id;
            }
            var Payment = db.PaymentMethods.Where(c => c.PaymentMethod_Id == PaymentPolicy);
            if (Payment.Any())
            {
                SupplierVM.Payment_Policy_Id = Payment.FirstOrDefault().PaymentMethod_Id;
            }
            SupplierVM.Supp_Mobile = Supplier.Supp_Mobile;
            SupplierVM.Purchase_Price = Supplier.Purchase_Price.Value;           
            SupplierVM.Supp_Website = Supplier.Supp_Website;
            SupplierVM.Supp_Id = Supplier.Supp_Id;
            SupplierVM.Supp_Name = Supplier.Supp_Name;           
            SupplierVM.Supp_Phone = Supplier.Supp_Phone;
            SupplierVM.Supp_Mail = Supplier.Supp_Mail;
            SupplierVM.MyPaymentPolicy = new SelectList(db.PaymentMethods, "PaymentMethod_Id", "Payment_Type");
            SupplierVM.BranchList = new SelectList(db.Branches, "Branch_Id", "Branch_Name");
            SupplierVM.MyCountryList = new SelectList(db.Countries, "Country_Id", "Country_Name");
            SupplierVM.MyProvinceList = new SelectList(db.Provinces, "Province_Id", "Province_Name");
            SupplierVM.MyCitiesList = new SelectList(db.Cities, "City_Id", "City_Name");
            SupplierVM.Supp_Code = Supplier.Supp_Code;
            SupplierVM.Supp_Fax = Supplier.Supp_Fax;
            SupplierVM.Supp_Address = Supplier.Supp_Address;
            SupplierVM.Tax_Number = Supplier.Tax_Number;
            SupplierVM.Country = Supplier.Country;
            SupplierVM.Province = Supplier.Province;
            SupplierVM.City = Supplier.City;

            return SupplierVM;
        }

        // use this method to make ListItem from Country Provinces and get Data from DataBase.
        public static IEnumerable<SelectListItem> GetCountryProvinces(int countryId)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> provinces = new SelectList(db.Provinces.Where(p => p.CountryId == countryId), "Province_Id", "Province_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return provinces;
        }

        // use this method to make ListItem from Provinces Cities and get Data from DataBase.
        public static IEnumerable<SelectListItem> GetProvincesCities(int provinceId)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> Cities = new SelectList(db.Cities.Where(p => p.ProvinceId == provinceId), "City_Id", "City_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return Cities;
        }
        /// <summary>
        /// get all purinvoice that has the same supplier id 
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns> IEnumerable<PurInvoiceMaster></returns>
        public static IEnumerable<PurInvoiceMaster> GetSupplierPurInvoices(int supplierId)
        {
            var db = new AccountingContainer();
            var invoices = db.PurInvoiceMasters.Where(invoice => invoice.Supplier_Id == supplierId).ToList();
            db.Dispose();
            return invoices;
        }
        /// <summary>
        /// get all payment that has the same supplier id 
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>

        public static IEnumerable<Payment> GetSupplierPayments(int supplierId)
        {
            var db = new AccountingContainer();
            var payments = db.Payments.Where(payment => payment.Supplier_Id == supplierId).ToList();
            db.Dispose();
            return payments;
        }
        /// <summary>
        /// transfor all supplier that not active to active 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public static bool UpdateSupplier(int id)
        {
            var db = new AccountingContainer();
            var SupplierSta = db.Suppliers.Find(id);
            var account = db.Accounts.Where(a => a.Account_Code == SupplierSta.Supp_Code).FirstOrDefault();
            if (SupplierSta.Supplier_state ==Customer_State.غير_نشط)
            {
                SupplierSta.Supplier_state = Customer_State.نشط;
                account.OpenBalance_Code = null;
            }
            else
            {
                SupplierSta.Supplier_state = Customer_State.غير_نشط;
                account.OpenBalance_Code = "1";
            }
            db.Entry(SupplierSta).State = EntityState.Modified;
            db.Entry(account).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return true;
        }

        /// <summary>
        /// make supplier type is hidden and disaple to show in any list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string StopSupplier(int id)
        {
            var db = new AccountingContainer();
            var supllierty = db.Suppliers.Find(id);
            supllierty.Supplier_Bal = false;
            supllierty.Supplier_type = Customer_Type.اخفاء_مؤقت;
            db.Entry(supllierty).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return "تم اخفاء العميل";
        }
        /// <summary>
        /// get all supplier were stoped 
        /// </summary>
        /// <returns>List<Supplier> </returns>
        public static List<Supplier> GetAllSupplierStop()
        {
            var db = new AccountingContainer();
            var Supresult = db.Suppliers.Where(S => S.Supplier_Bal == false).ToList();
            return Supresult;
        }
        /// <summary>
        /// change type supplier by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Supplier ChangeSupplierType(int id)
        {
            var db = new AccountingContainer();
            var Supresult = db.Suppliers.Find(id);
            Supresult.Supplier_Bal = true;
            db.Entry(Supresult).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return Supresult;
        }
        /// <summary>
        /// get all supplier for dropdown list
        /// </summary>
        /// <returns>List<Supplier></returns>
        public static List<Supplier> getAllSuPPlierFordrobDownList()
        {
            var db = new AccountingContainer();

            var lista = db.Suppliers.ToList();

            return lista;




        }


        /// <summary>
        /// import supplier by excel file after check mista
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        public static String Import(HttpPostedFileBase excelfile)
        {
            var data = getdataFillter(excelfile);
            List<Supplier> listsupplier = new List<Supplier>();
            if (data.Item2.Count() == 0)
            {
                foreach (var item in data.Item1)
                {
                    var sub = AddNewSupplierfromFile(item);
                    listsupplier.Add(sub);

                }
               
                var supplierempty = listsupplier.Where(a => a.Supp_Balance != 0).ToList();

                if (supplierempty.Count == 0)
                {

                }
                else
                {
                    Utilites.OpenBalanceFile.MakeOpenBallanceForGroupOFSupplier(listsupplier);
                }
                return "تم حفظ الملف بنجاح";

            }
            string combindedString = string.Join(" ,", data.Item2.ToArray());
            return "يوجد اخطاء فى الاسطر التالية" + combindedString;


        }
        /// <summary>
        /// check excel file and collect the number of line Error
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns>Tuple<List<SupplierVM>, List<String>> </returns>
        public static Tuple<List<SupplierVM>, List<String>> getdataFillter(HttpPostedFileBase excelfile)
        {
            var SupplierList = new List<SupplierVM> { };
            var messageList = new List<String> { };
            var db = new AccountingContainer();
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return Tuple.Create(SupplierList, messageList);

            }

            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                   // string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + excelfile.FileName);
                    var nameOfFile = "Supplier" + DateTime.Now.ToBinary() + ".xlsx";
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + nameOfFile);


                    excelfile.SaveAs(path);
                   
                    // read data from excel file

                    Application application = new Application();
                    Workbook workbook = application.Workbooks.Open(path);
                    Worksheet worksheet = workbook.ActiveSheet;
                    Range range = worksheet.UsedRange;
                    for (int row = 3; row <= range.Rows.Count; row++)
                    {
                        SupplierVM suppl = new SupplierVM();
                        // cust.Customer_Id = int.Parse(((Range)range.Cells[row, 1]).Text);
                        suppl.Supp_Name = ((Range)range.Cells[row, 1]).Text;
                        if (suppl.Supp_Name == "") 
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }
                        suppl.Supp_Phone = ((Range)range.Cells[row, 2]).Text;
                        if (suppl.Supp_Phone == "")
                        {
                            messageList.Add(row.ToString());
                            continue;

                        }
                        try
                        {
                            var supbal= ((Range)range.Cells[row, 3]).Text;
                            if (supbal == "")
                            {
                                suppl.Supplier_Balance = 0;
                            }
                            else
                            {
                                suppl.Supplier_Balance = Convert.ToDecimal(((Range)range.Cells[row, 3]).Text);
                            }
                        }
                        catch
                        {

                            messageList.Add(row.ToString());
                            continue;


                        }


                        suppl.Supp_Mail = ((Range)range.Cells[row, 4]).Text;
                        suppl.Supp_Address = ((Range)range.Cells[row, 5]).Text;
                        suppl.Supp_Mobile = ((Range)range.Cells[row, 6]).Text;
                        suppl.Supp_Website = ((Range)range.Cells[row, 7]).Text;
                        suppl.Supp_Fax = ((Range)range.Cells[row, 8]).Text;
                        SupplierList.Add(suppl);





                    }
                    workbook.Save();
                    workbook.Close(path);



                   // File.Delete(path);

                }
            }
            return Tuple.Create(SupplierList, messageList);
        }
        /// <summary>
        /// add supplier from file excel after check validation
        /// </summary>
        /// <param name="supplierVm"></param>
        /// <returns>Supplier</returns>
        public static Supplier AddNewSupplierfromFile(SupplierVM supplierVm)
        {
            try
            {
                var db = new AccountingContainer();
                var Supplier = new Supplier();

                if (supplierVm != null)
                {
                    // create selectList from Database to entity Contry.
                  

                    Supplier.Supp_Name = supplierVm.Supp_Name;
                    Supplier.Supp_Phone = supplierVm.Supp_Phone;
                    Supplier.Supp_Mail = supplierVm.Supp_Mail;
                    supplierVm.SelectedChartOfAccountId = 45;

                    var ChartOfAccount = db.ChartOfAccountings.Find(supplierVm.SelectedChartOfAccountId);
                    var ChartOfAccountParent = db.ChartOfAccountings.Find(ChartOfAccount.ChartOfAccountingAccount_Id);

                  

                    var lastSerial = db.Suppliers.OrderByDescending(c => c.Supp_Id).FirstOrDefault();
                    if (lastSerial == null)
                    {
                        Supplier.Supp_Code = "SUP-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        Supplier.Supp_Code = "SUP-" + (Convert.ToInt32(lastSerial.Supp_Code.Substring(4, lastSerial.Supp_Code.Length - 4)) + 1).ToString("D3");
                    }

                    Supplier.Supp_Fax = supplierVm.Supp_Fax;
                    Supplier.Supp_Address = supplierVm.Supp_Address;
                    Supplier.Supp_Mobile = supplierVm.Supp_Mobile;
                    Supplier.Purchase_Price = supplierVm.Purchase_Price;
                    Supplier.Supp_Website = supplierVm.Supp_Website;
                    Supplier.Tax_Number = supplierVm.Tax_Number;
                    Supplier.Supp_Balance = supplierVm.Supplier_Balance;
                    Supplier.Supplier_state = Customer_State.نشط;
                    Supplier.Supplier_Bal = true;


                  
                    var accounts = new Accounts();
                    accounts.Account_Id = ChartOfAccount.Account_Id.ToString();
                    accounts.Account_Code = Supplier.Supp_Code;
                    var date = DateTime.Now.ToString();
                    accounts.Account_Date = DateTime.Now.Date;
                    accounts.Account_Name = Supplier.Supp_Name;
                    accounts.Account_Type = ChartOfAccount.Account_Name;
                    accounts.Account_Desc = ChartOfAccountParent.Account_Name;
                    accounts.Account_Balance = 0;
                    db.Accounts.Add(accounts);

                   
                    db.Suppliers.Add(Supplier);
                    db.SaveChanges();
                    db.Dispose();
                    return Supplier;

                }
            }
            catch (Exception e)
            {
                var masg = e.Message;
                var result = e.Data;

                throw;
            }

            return null;

        }
    }
}