using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office.Interop.Excel;


using System.IO;


namespace Accountingsystem.Models
{
    public partial class Customer
    {
        /// <summary>
        /// Check the code and make it unique
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns></returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.Customers.ToList();

            if (!Serial.StartsWith("CUST-"))
            {
                return true;
            }
            foreach (var item in allinData)
            {
                if (item.Customer_Code == Serial || !Serial.StartsWith("CUST-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }
        // use this method to  Get List Customer from Data base 
        public static List<Customer> GetAllCustomer()
        {
            var db = new AccountingContainer();
            var customer = db.Customers.Where(C =>  C.Customer_State == Models.Customer_State.نشط).ToList();

            //var account = db.Accounts.ToList();
            //foreach (var cut in customer)
            //{
            //    cut.Customer_Balance = 0;
            //    if (cut.Customer_Phone == null || cut.Customer_Phone == "")
            //    {
            //        cut.Customer_Phone = "1";
            //        db.Entry(cut).State = EntityState.Modified;
            //        db.SaveChanges();
            //    }
            //    var custaccount = db.Accounts.Where(a => a.Account_Code == cut.Customer_Code).FirstOrDefault();
            //    if (custaccount != null)
            //    {
            //        cut.Customer_Balance += custaccount.Account_Balance;
            //    }

                
            //}


            return customer.OrderBy(a=>a.Customer_Id).ToList();

            
        }


        public static List<Customer> GetAllCustomerNew(string userId)
        {
            var db = new AccountingContainer();
            var customer = new List<Customer>();


            var emp = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
            if (emp == null || emp.ViewAllCustomer == true||emp.SystemRoleId==6)
            {
                customer = db.Customers.Where(C => C.Customer_State == Models.Customer_State.نشط).ToList();
            }
            else
            {
                customer = db.Customers.Where(a => a.Customer_State == Models.Customer_State.نشط && (a.Employee_Id == emp.Emp_Id || a.Employee_Id == null)).ToList();


            }


            return customer.OrderBy(a => a.Customer_Id).ToList();


        }
        /// <summary>
        /// get all Customer that not Active
        /// </summary>
        /// <returns></returns>
        public static List<Customer> GetAllCustomernotactive()
        {
            var db = new AccountingContainer();
            var customer = db.Customers.Where(C =>C.Customer_State==Models.Customer_State.غير_نشط).ToList();

            //var account = db.Accounts.ToList();
            //foreach (var cut in customer)
            //{
            //    cut.Customer_Balance = 0;
            //    if (cut.Customer_Phone == null || cut.Customer_Phone == "")
            //    {
            //        cut.Customer_Phone = "1";
            //        db.Entry(cut).State = EntityState.Modified;
            //        db.SaveChanges();
            //    }
            //    var custaccount = db.Accounts.Where(a => a.Account_Code == cut.Customer_Code).FirstOrDefault();
            //    if (custaccount != null)
            //    {
            //        cut.Customer_Balance += custaccount.Account_Balance;
            //    }

               
            //}


            return customer;


        }

        public static List<Customer> GetAllCustomernotactiveNew(string userId)
        {
            var db = new AccountingContainer();
            var customer = new List<Customer>();


            var emp = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
            if (emp == null || emp.ViewAllCustomer == true||emp.SystemRoleId==6)
            {
                customer = db.Customers.Where(C => C.Customer_State == Models.Customer_State.غير_نشط).ToList();
            }
            else
            {
                customer = db.Customers.Where(a => a.Customer_State == Models.Customer_State.غير_نشط && (a.Employee_Id == emp.Emp_Id || a.Employee_Id == null)).ToList();


            }


            return customer.OrderBy(a => a.Customer_Id).ToList();


        }


        // use this method to   Get one element CustomerById from Database.  
        public static Customer GetCustomerById(int id)
        {
            var db = new AccountingContainer();
            var customer = db.Customers.Find(id);
            
            return customer;
        }
        // use this method to   Get one CustomerByCode from Database.
        public static Customer GetCustomerByCode(string code)
        {
            var db = new AccountingContainer();
            var customer = db.Customers.Where(s => s.Customer_Code == code);
            var _customer = new Customer();
            if (customer != null)
            {
                _customer = customer.FirstOrDefault();
                return _customer;
            }
            return null;
        }
        // use this method For Creation new Customer and get Data from user.
        public static CustomerVM AddCustomerVM()
        {
            var db = new AccountingContainer();
            var customervm = new CustomerVM();

            var Cus = new Customer();

           

            var lastSerial = db.Customers.OrderByDescending(c => c.Customer_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                customervm.Customer_Code = "CUST-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                customervm.Customer_Code = "CUST-" + (Convert.ToInt32(lastSerial.Customer_Code.Substring(5, lastSerial.Customer_Code.Length - 5)) + 1).ToString("D3");
            }


            customervm.EmployeeList = new SelectList(db.Employees.Where(a=>a.Reg_ID!=null), "Emp_Id", "Emp_Name");
            customervm.SlicesList = new SelectList(db.Slices, "Slice_Id", "Slice_Name");
            customervm.MyCountryList = new SelectList(db.Countries, "Country_Id", "Country_Name");
            customervm.MyProvinceList = new SelectList(db.Provinces, "Province_Id", "Province_Name");
            customervm.MyCitiesList = new SelectList(db.Cities, "City_Id", "City_Name");
            customervm.BranchList = new SelectList(db.Branches, "Branch_Id", "Branch_Name");
            customervm.MyPaymentPolicy = new SelectList(db.PaymentMethods, "PaymentMethod_Id", "Payment_Type");
            customervm.MyChartOfAccountList = new SelectList(db.ChartOfAccountings.Where(accCode => accCode.Account_Code.StartsWith("010109")), "Account_Id", "Account_Name");


            var sasa = new CustomerTemp();

            var id = (from s in db.CustomerTemps
                      select s).ToList().LastOrDefault();
            if (id == null)
            {
                sasa.Customer_Id = 1;
            }
            else
            {
                sasa.Customer_Id = id.Customer_Id + 1;
            }
            db.CustomerTemps.Add(sasa);
            db.SaveChanges();
            customervm._Customer_Id = sasa.Customer_Id;
            return customervm;
        }
        // use this method to  Create New Customer and save it in DataBase.
        public static int AddNewCustomer(CustomerVM customerVm)
        {
            var db = new AccountingContainer();

            var customer = new Customer();
            var Customertem = new CustomerTemp();

            if (customerVm._Customer_Id != 0)
            {
                Customertem = db.CustomerTemps.Find(customerVm._Customer_Id);
            }
            try
            {
                if (customerVm != null)
                {
                    // create selectList from Database to entity Contry.
                    var resultCountry = db.Countries.Find(customerVm.SelectedCountryId);
                    if (resultCountry != null)
                    {
                        customer.Customer_Country = resultCountry.Country_Name;
                    }
                    customer.Employee_Id = customerVm.CustEmployeeId;
                    customer.Customer_Balance = customerVm.Customer_Balance;
                    // create selectList from Database to entity Province.
                    var resultProvinces = db.Provinces.Find(customerVm.SelectedProvinceId);
                    if (resultProvinces != null)
                    {
                        customer.Customer_Province = resultProvinces.Province_Name;
                    }
                    // create selectList from Database to entity City.
                    var resultCities = db.Cities.Find(customerVm.SelectedCityId);
                    if (resultCities != null)
                    {
                        customer.Customer_City = resultCities.City_Name;
                    }
                    var resultbranch = db.Branches.Find(customerVm.CustomerBranch_Id);
                    if (resultbranch != null)
                    {
                        customer.Branch_Id = resultbranch.Branch_Id;
                    }

                    var resultPaymentPolicy = db.PaymentMethods.Find(customerVm.Payment_Policy_Id);
                    if (resultPaymentPolicy != null)
                    {
                        customer.Payment_Policy_Id = resultPaymentPolicy.PaymentMethod_Id;
                    }

                 
                    if (customerVm.CustomerSelectSlice_Id != null && customerVm.CustomerSelectSlice_Id!=0)
                    {
                        var resultSliceId = db.Slices.Find(customerVm.CustomerSelectSlice_Id);
                        customer.SlicesSlice_Id = resultSliceId.Slice_Id;
                    }
                    else
                    {
                        customer.SlicesSlice_Id = db.Slices.FirstOrDefault().Slice_Id;
                    }
                    //TODO: Acquire code auromation. from VM

                    customer.Customer_Balance = 0;
                    customer.Customer_Name = customerVm.Customer_Name;
                    customer.CommercialRegistrationNo = customerVm.CommercialRegistrationNo;
                    customer.IdentificationNumber = customerVm.IdentificationNumber;
                    customer.Customer_Address = customerVm.Customer_Address;
                    customer.Customer_Phone = customerVm.Customer_Phone;
                    customer.Customer_Mail = customerVm.Customer_Mail;
                    customer.Customer_Fax = customerVm.Customer_Fax;
                    customer.Customer_Mobile = customerVm.Customer_Mobile;
                    customer.Comment = customerVm.Comment;
                    customer.Country = customerVm.Country;
                    customer.Province = customerVm.Province;
                    customer.City = customerVm.City;

                    customer.Customer_State = Models.Customer_State.نشط;
                    customer.Customer_Bal = true;
                    customer.Customer_Website = customerVm.Customer_Website;
                    customer.Customer_TaxNumber = customerVm.customer_TaxNumber;
                    customerVm.SelectedChartOfAccountId = 37;

                    var ChartOfAccount = db.ChartOfAccountings.Find(customerVm.SelectedChartOfAccountId);
                    var ChartOfAccountParent = db.ChartOfAccountings.Find(ChartOfAccount.ChartOfAccountingAccount_Id);

                    var lastSerial = db.Customers.OrderByDescending(c => c.Customer_Id).FirstOrDefault();
                    
                      
                    if (lastSerial == null)
                    {
                        customer.Customer_Code = "CUST-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        customer.Customer_Code = "CUST-" + (Convert.ToInt32(lastSerial.Customer_Code.Substring(5, lastSerial.Customer_Code.Length - 5)) + 1).ToString("D3");
                    }
                    if (customerVm.customerFiles2 != null&& customerVm.customerFiles2.FirstOrDefault()!=null)
                    {
                        foreach(var file in customerVm.customerFiles2) { 
                            var filedetails = new CustomerFileDetail();
                            var length =file.InputStream.Length; //Length: 103050706
                            MemoryStream target = new MemoryStream();
                            file.InputStream.CopyTo(target); // generates problem in this line
                            byte[] data = target.ToArray();
                            filedetails.FileName = file.FileName;
                            filedetails.File_Type = file.ContentType;
                            filedetails.ContentLength = file.ContentLength;
                            filedetails.ProImg = data;
                            filedetails.CustomerCustomer_Id = customer.Customer_Id;
                            if (data.Length < 1000000)
                            {
                                customer.CustomerFileDetails.Add(filedetails);
                            }
                        }
                    }

                    if (customerVm.customerFiles != null)
                    {
                        for (int i=0; i < customerVm.customerFiles.Count; i++)
                        {
                            

                            var filedetails = new CustomerFileDetail();
                            var length = customerVm.customerFiles[i].InputStream.Length; //Length: 103050706
                            MemoryStream target = new MemoryStream();
                            customerVm.customerFiles[i].InputStream.CopyTo(target); // generates problem in this line
                            byte[] data = target.ToArray();
                            filedetails.FileName = customerVm.customerFiles[i].FileName;
                            filedetails.File_Type = customerVm.customerFiles[i].ContentType;
                            filedetails.ContentLength = customerVm.customerFiles[i].ContentLength;                            
                            filedetails.ProImg = data;
                            filedetails.CustomerCustomer_Id = customer.Customer_Id;
                            if (data.Length < 1000000)
                            {
                                customer.CustomerFileDetails.Add(filedetails);
                            }
                            
                        }
                    }
                    if (Customertem.ContentPersonTemps.FirstOrDefault() != null)
                    {
                        foreach (var contact in Customertem.ContentPersonTemps.ToList())
                        {
                            var contactdetail = new ContentPersonDetails();
                            contactdetail.ContentPerson_ID = contact.ContentPerson_ID;
                            contactdetail.Content_Job = contact.Content_Job;
                            contactdetail.Content_Mail = contact.Content_Mail;
                            contactdetail.Content_Name = contact.Content_Name;
                            contactdetail.Content_Number = contact.Content_Number;
                            customer.ContentPersonDetails.Add(contactdetail);
                            db.ContentPersonDetailsTemps.Remove(contact);

                        }
                        db.CustomerTemps.Remove(Customertem);
                    }

                    customer.Customer_Balance = customerVm.Customer_Balance;
                    db.Customers.Add(customer);


                   



                    var accounts = new Accounts();
                    accounts.Account_Id = ChartOfAccount.Account_Id.ToString();
                    accounts.Account_Code = customer.Customer_Code;
                    var date = DateTime.Now.ToString();
                    accounts.Account_Date = DateTime.Now.Date;
                    accounts.Account_Name = customer.Customer_Name;
                    accounts.Account_Type = ChartOfAccount.Account_Name;
                    accounts.Account_Desc = ChartOfAccountParent.Account_Name;
                    accounts.Account_Balance = 0;

                    db.Accounts.Add(accounts);

                    db.SaveChanges();
                    db.Dispose();
                    return customer.Customer_Id;

                }
            }
            catch (Exception e)
            {
                var masg = e.Message;
                var result = e.Data;

                throw;
            }

            return 0;
        }
        public static CustomerFileDetail getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.CustomerFileDetails.Find(fileId);
            return file;
        }
        public static string deleteFile(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.CustomerFileDetails.Find(fileId);
            db.CustomerFileDetails.Remove(file);
            db.SaveChanges();
            return Resources.validate.DeleteSuccessful;
        }
        public static List<CustomerFileDetail> GetAllFiles(int custId)
        {
            var db = new AccountingContainer();
            var files = db.CustomerFileDetails.Where(emp => emp.CustomerCustomer_Id == custId).ToList();
            return files;
        }
        // use this method to load Details of Data for customer.
        public static CustomerVM FillCustomerForEditing(int custvm)
        {
            var db = new AccountingContainer();
            var Customer = db.Customers.Find(custvm);
            var cont = db.ContentPersonDetails.Find(custvm);
            var customervm = new CustomerVM();
            var countryName = Customer.Customer_Country;
            customervm.CommercialRegistrationNo = Customer.CommercialRegistrationNo;
            customervm.IdentificationNumber = Customer.IdentificationNumber;
            customervm.CustEmployeeId = Customer.Employee_Id;
            var provinceName = Customer.Customer_Province;
            var cityName = Customer.Customer_City;
            var branchid = Customer.Branch_Id;
            var PaymentPolicy = Customer.Payment_Policy_Id;
            var sliceId = Customer.SlicesSlice_Id;

            //  select Contry from entity Contry.
            var Countries = db.Countries.Where(c => c.Country_Name == countryName);
            if (Countries.Any())
            {
                customervm.SelectedCountryId = Countries.FirstOrDefault().Country_Id;
            }
            // select Province from  entity Province.
            var province = db.Provinces.Where(c => c.Province_Name == provinceName);
            if (province.Any())
            {
                customervm.SelectedProvinceId = province.FirstOrDefault().Province_Id;
            }
            //  select city from entity city.
            var city = db.Cities.Where(c => c.City_Name == cityName);
            if (city.Any())
            {
                customervm.SelectedCityId = city.FirstOrDefault().City_Id;
            }
            var branch = db.Branches.Where(c => c.Branch_Id == branchid);
            if (branch.Any())
            {
                customervm.CustomerBranch_Id = branch.FirstOrDefault().Branch_Id;
            }

            var Payment = db.PaymentMethods.Where(c => c.PaymentMethod_Id == PaymentPolicy);
            if (Payment.Any())
            {
                customervm.Payment_Policy_Id = Payment.FirstOrDefault().PaymentMethod_Id;
            }

            var Slice = db.Slices.Where(c => c.Slice_Id == sliceId);
            if (Slice.Any())
            {
                customervm.CustomerSelectSlice_Id = Slice.FirstOrDefault().Slice_Id;
            }

            customervm.SlicesList = new SelectList(db.Slices, "Slice_Id", "Slice_Name");
            customervm.EmployeeList = new SelectList(db.Employees.Where(a => a.Reg_ID != null), "Emp_Id", "Emp_Name");
            customervm.MyCountryList = new SelectList(db.Countries, "Country_Id", "Country_Name");
            customervm.MyProvinceList = new SelectList(db.Provinces, "Province_Id", "Province_Name");
            customervm.MyCitiesList = new SelectList(db.Cities, "City_Id", "City_Name");
            customervm.BranchList = new SelectList(db.Branches, "Branch_Id", "Branch_Name");
            customervm.MyPaymentPolicy = new SelectList(db.PaymentMethods, "PaymentMethod_Id", "Payment_Type");
            customervm.customer_TaxNumber = Customer.Customer_TaxNumber;
            customervm.Comment = Customer.Comment;

            customervm._Customer_Id = Customer.Customer_Id;
            customervm.Customer_Fax = Customer.Customer_Fax;
            customervm.Customer_Code = Customer.Customer_Code;
            customervm.Customer_Name = Customer.Customer_Name;
            customervm.Customer_Mobile = Customer.Customer_Mobile;
            customervm.Customer_Phone = Customer.Customer_Phone;
            customervm.Customer_Mail = Customer.Customer_Mail;
            customervm.Customer_Balance = Customer.Customer_Balance;
            customervm.Customer_Website = Customer.Customer_Website;
            customervm.Customer_Address = Customer.Customer_Address;
            customervm.Country = Customer.Country;
            customervm.Province = Customer.Province;
            customervm.City = Customer.City;
            var CustomerTemppp = new CustomerTemp();
            CustomerTemppp.Customer_Fax = Customer.Customer_Fax;
            CustomerTemppp.Customer_Code = Customer.Customer_Code;
            CustomerTemppp.Customer_Name = Customer.Customer_Name;
            CustomerTemppp.Customer_Mobile = Customer.Customer_Mobile;
            CustomerTemppp.Customer_Phone = Customer.Customer_Phone;
            CustomerTemppp.Customer_Mail = Customer.Customer_Mail;
            CustomerTemppp.Customer_Website = Customer.Customer_Website;
            CustomerTemppp.Customer_Address = Customer.Customer_Address;

            var sasaTe = db.CustomerTemps.Find(Customer.Customer_Id);
            if (sasaTe != null)
            {

            }
            else
            {
                CustomerTemppp.Customer_Id = Customer.Customer_Id;
                db.CustomerTemps.Add(CustomerTemppp);
            }
            db.SaveChanges();
            return customervm;
        }
        /// <summary>
        /// prepare Detail of Customer Detail To Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ContentPersonDetailsVM PreperCustomerTemp(int id)
        {
            var db = new AccountingContainer();
            var custtvm = new ContentPersonDetailsVM();
            var Custtemp = GetallCustomerDetailsTemp(id).ToList().FirstOrDefault();
            custtvm.CustomerCustomer_Id = id;
            custtvm.Content_Job = Custtemp.Content_Job;
            custtvm.Content_Mail = Custtemp.Content_Mail;
            custtvm.Content_Name = Custtemp.Content_Name;
            custtvm.Content_Number = Custtemp.Content_Number;
            return custtvm;

        }
        public static List<ContentPersonDetails> GetallCustomerDetailsTemp(int Custid)
        {
            var db = new AccountingContainer();
            var DetailsTemp = db.ContentPersonDetails.Where(C => C.ContentPerson_ID == Custid).ToList();
            return DetailsTemp;
        }
        public static List<ContentPersonDetails> GetAllContentCustumerDetails(int Content)
        {
            var db = new AccountingContainer();
            var Details = db.ContentPersonDetails.Where(invoice => invoice.CustomerCustomer_Id == Content).ToList();
            return Details;
        }
        // use this method For Edite Customer and save it DataBase.
        public static Customer Editcustomer(CustomerVM custvm)
        {
            var db = new AccountingContainer();
            var customer = db.Customers.Find(custvm._Customer_Id);
            var Contacttemp = db.CustomerTemps.Find(custvm._Customer_Id);
            var cust = db.Accounts.Where(a => a.Account_Name == customer.Customer_Name && a.Account_Code.StartsWith("CUST")).FirstOrDefault();

            if (customer != null)
            {
                customer.Employee_Id = custvm.CustEmployeeId;
                //  select Country from   entity Contry.
                var resultCountry = db.Countries.Find(custvm.SelectedCountryId);
                if (resultCountry != null)
                {
                    customer.Customer_Country = resultCountry.Country_Name;
                }
                // select  Province from entity Province.
                var resultProvinces = db.Provinces.Find(custvm.SelectedProvinceId);
                if (resultProvinces != null)
                {
                    customer.Customer_Name = resultProvinces.Province_Name;
                }
                //  select a City from  entity City.
                var resultCities = db.Cities.Find(custvm.SelectedCityId);
                if (resultCities != null)
                {
                    customer.Customer_City = resultCities.City_Name;
                }
                var resultbranch = db.Branches.Find(custvm.CustomerBranch_Id);
                if (resultbranch != null)
                {
                    customer.Branch_Id = resultbranch.Branch_Id;
                }
                var resultPaymentPolicy = db.PaymentMethods.Find(custvm.Payment_Policy_Id);
                if (resultPaymentPolicy != null)
                {
                    customer.Payment_Policy_Id = resultPaymentPolicy.PaymentMethod_Id;
                }

                var resultSliceId = db.Slices.Find(custvm.CustomerSelectSlice_Id);
                if (resultSliceId != null)
                {
                    customer.SlicesSlice_Id = resultSliceId.Slice_Id;
                }


                var oldDetail = db.ContentPersonDetails.Where(a => a.CustomerCustomer_Id == customer.Customer_Id).ToList();


                var detailListTemp = db.ContentPersonDetailsTemps.Where(a => a.CustomerTemp_Id == Contacttemp.Customer_Id);

                List<ContentPersonDetailsTemp> DeletedList = new List<ContentPersonDetailsTemp>();
                foreach (var item in detailListTemp)
                {

                    var details = new ContentPersonDetails();

                    details.Content_Name = item.Content_Name;
                    details.Content_Number = item.Content_Number;
                    details.Content_Mail = item.Content_Mail;
                    details.Content_Job = item.Content_Job;
                    customer.ContentPersonDetails.Add(details);
                  
                    DeletedList.Add(item);

                }
                customer.CommercialRegistrationNo = custvm.CommercialRegistrationNo;
                customer.IdentificationNumber = custvm.IdentificationNumber;
                db.ContentPersonDetailsTemps.RemoveRange(DeletedList);
                customer.Customer_Id = custvm._Customer_Id;
                customer.Comment = custvm.Comment;
                customer.Customer_Fax = custvm.Customer_Fax;
                customer.Customer_Balance = custvm.Customer_Balance;
                customer.Customer_Code = custvm.Customer_Code;
                customer.Customer_Name = custvm.Customer_Name;
                customer.Customer_Mobile = custvm.Customer_Mobile;
                customer.Customer_Phone = custvm.Customer_Phone;
                customer.Customer_Mail = custvm.Customer_Mail;
                customer.Customer_Website = custvm.Customer_Website;
                customer.Customer_Address = custvm.Customer_Address;
                customer.Customer_TaxNumber = custvm.customer_TaxNumber;
                customer.Country = custvm.Country;
                customer.Province = custvm.Province;
                customer.City = custvm.City;
                if (custvm.customerFiles2 != null && custvm.customerFiles2.FirstOrDefault() != null)
                {
                    foreach (var file in custvm.customerFiles2)
                    {
                        var filedetails = new CustomerFileDetail();
                        var length = file.InputStream.Length; //Length: 103050706
                        MemoryStream target = new MemoryStream();
                        file.InputStream.CopyTo(target); // generates problem in this line
                        byte[] data = target.ToArray();
                        filedetails.FileName = file.FileName;
                        filedetails.File_Type = file.ContentType;
                        filedetails.ContentLength = file.ContentLength;
                        filedetails.ProImg = data;
                        filedetails.CustomerCustomer_Id = customer.Customer_Id;
                        if (data.Length < 1000000)
                        {
                            customer.CustomerFileDetails.Add(filedetails);
                        }
                    }
                }
                if (custvm.customerFiles != null)
                {
                    for (int i = 0; i < custvm.customerFiles.Count; i++)
                    {


                        var filedetails = new CustomerFileDetail();
                        var length = custvm.customerFiles[i].InputStream.Length; //Length: 103050706
                        MemoryStream target = new MemoryStream();
                        custvm.customerFiles[i].InputStream.CopyTo(target); // generates problem in this line
                        byte[] data = target.ToArray();
                        filedetails.FileName = custvm.customerFiles[i].FileName;
                        filedetails.File_Type = custvm.customerFiles[i].ContentType;
                        filedetails.ContentLength = custvm.customerFiles[i].ContentLength;
                        filedetails.ProImg = data;
                        filedetails.CustomerCustomer_Id = customer.Customer_Id;
                        if (data.Length < 1000000)
                        {
                            customer.CustomerFileDetails.Add(filedetails);
                        }

                    }
                }


                db.Entry(customer).State = EntityState.Modified;
                db.CustomerTemps.Remove(Contacttemp);
                db.ContentPersonDetails.RemoveRange(oldDetail);
                cust.Account_Name = custvm.Customer_Name;
                db.Entry(cust).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return customer;
            }
            return customer;
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
        public static IEnumerable<InvoiceMaster> getCustomerInvoices(int customerId)
        {
            var db = new AccountingContainer();
            var invoices = db.InvoiceMasters.Where(invoice => invoice.Customer_Id == customerId).ToList();
            db.Dispose();
            return invoices;
        }

        public static IEnumerable<Receipt> getCustomerReceipts(int customerId)
        {
            var db = new AccountingContainer();
            var receipts = db.Receipts.Where(receipt => receipt.Customer_Id == customerId).ToList();
            db.Dispose();
            return receipts;
        }
        public static void Editplance(Customer PAlense, Accounts Blance)
        {

            if (PAlense.Customer_Balance == null)
            {

            }
            else
            {
                PAlense.Customer_Balance = Blance.Account_Balance.Value;
                //db.Entry(PAlense).State = EntityState.Modified;
                //db.SaveChanges();
                //db.Dispose();
            }

        }
        public static string HiddenCustomer(int id)
        {
            var db = new AccountingContainer();
            var Balan = db.Customers.Find(id);
            Balan.Customer_Bal = false;
            if (Balan.Customer_Bal == false)
            {
                Balan.Customer_Type = Models.Customer_Type.اخفاء_مؤقت;

            }
            db.Entry(Balan).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return "تم اخفاء العميل";
        }
        public static List<Customer> GetAllCustomerBal()
        {
            var db = new AccountingContainer();
            var Cust = db.Customers.Where(s => s.Customer_Bal == false && s.Customer_Balance >= 0).ToList();
            return Cust;
        }
        public static bool UpdateCustomerState(int id)
        {
            var db = new AccountingContainer();
            var CustState = db.Customers.Find(id);
            var account = db.Accounts.Where(a => a.Account_Code == CustState.Customer_Code).FirstOrDefault();
            if (CustState.Customer_State == Models.Customer_State.غير_نشط)
            {
                CustState.Customer_State = Models.Customer_State.نشط;
                account.OpenBalance_Code = null;
            }
            else { CustState.Customer_State = Models.Customer_State.غير_نشط;
                account.OpenBalance_Code = "1";

            }
           

            db.Entry(CustState).State = EntityState.Modified;
            db.Entry(account).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return true;
        }
        public static ContentPersonDetailsVM preperCustomer()
        {
            var db = new AccountingContainer();
            var Custt = new ContentPersonDetailsVM();


            return Custt;
        }
        /// <summary>
        /// get All Customer Contact by customer Id
        /// </summary>
        /// <param name="CustomerVMId"></param>
        /// <returns></returns>
        public static List<ContentPersonDetailsTemp> GetAllCustomerContent(int CustomerVMId)
        {
            var db = new AccountingContainer();
            var Content = db.ContentPersonDetailsTemps.Where(d => d.CustomerTemp_Id == CustomerVMId).ToList();
            return Content;
        }
        public static string DddContextToTemp(ContentPersonDetailsVM ContextVm)
        {
            var db = new AccountingContainer();
            var ContecxtTemp = db.CustomerTemps.Find(ContextVm.CustomerCustomer_Id);

            if (ContextVm.CustomerCustomer_Id == 0 || ContextVm.CustomerCustomer_Id == null)
            {
                var C = db.ContentPersonDetails.Find(ContextVm.ContentPerson_ID).CustomerCustomer_Id;
                ContecxtTemp = db.CustomerTemps.Find(C);
            }
            else
            {
                ContecxtTemp = db.CustomerTemps.Find(ContextVm.CustomerCustomer_Id);
            }

            var ConteDetailsTemp = new ContentPersonDetailsTemp();


            ConteDetailsTemp.Content_Job = ContextVm.Content_Job;
            ConteDetailsTemp.Content_Mail = ContextVm.Content_Mail;
            ConteDetailsTemp.Content_Name = ContextVm.Content_Name;
            ConteDetailsTemp.Content_Number = ContextVm.Content_Number;

            ConteDetailsTemp.CustomerTemp_Id = ContextVm.CustomerCustomer_Id;
            ContecxtTemp.ContentPersonTemps.Add(ConteDetailsTemp);
            db.Entry(ContecxtTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "Details has been added";
        }
        /// <summary>
        /// get All Customer Temp by customer 
        /// </summary>
        /// <param name="CustomerVMId"></param>
        /// <returns></returns>
        public static List<ContentPersonDetailsTemp> GetNewCustomer(int CustomerVMId)
        {
            var db = new AccountingContainer();
            var Content = db.ContentPersonDetailsTemps.Where(d => d.CustomerTemp_Id == CustomerVMId).ToList();
            return Content;
        }

        /// <summary>
        /// Add detail to Customer
        /// </summary>
        /// <param name="ContextVm"></param>
        /// <returns></returns>
        public static string DdNewCustomerToTemp(ContentPersonDetailsVM ContextVm)
        {
            var db = new AccountingContainer();
            var ContecxtTemp = db.CustomerTemps.Find(ContextVm.CustomerCustomer_Id);
            if (ContextVm.CustomerCustomer_Id == 0)
            {
                var C = db.ContentPersonDetails.Find(ContextVm.ContentPerson_ID).CustomerCustomer_Id;
                ContecxtTemp = db.CustomerTemps.Find(C);
            }
            else
            {
                ContecxtTemp = db.CustomerTemps.Find(ContextVm.CustomerCustomer_Id);
            }

            var ConteDetailsTemp = new ContentPersonDetailsTemp();


            ConteDetailsTemp.Content_Job = ContextVm.Content_Job;
            ConteDetailsTemp.Content_Mail = ContextVm.Content_Mail;
            ConteDetailsTemp.Content_Name = ContextVm.Content_Name;
            ConteDetailsTemp.Content_Number = ContextVm.Content_Number;

            ConteDetailsTemp.CustomerTemp_Id = ContextVm.CustomerCustomer_Id;
            ContecxtTemp.ContentPersonTemps.Add(ConteDetailsTemp);
            db.Entry(ContecxtTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "Details has been added";
        }
        /// <summary>
        /// Fill For Creation Customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CustomerVM CreatforCreation(int id)
        {
            var db = new AccountingContainer();
            var customervm = new CustomerVM();
            customervm._Customer_Id = id;
            var data = db.Customers.Find(id);
            customervm.Customer_Name = data.Customer_Name;

            var custtemp = new CustomerTemp();
            custtemp.Customer_Id = id;
            var checktemp = db.CustomerTemps.Find(id);
            if (checktemp != null)
            {
                db.CustomerTemps.Remove(checktemp);
                db.SaveChanges();
            }

            db.CustomerTemps.Add(custtemp);
            db.SaveChanges();


            customervm._Customer_Id = custtemp.Customer_Id;

            return customervm;
        }
        /// <summary>
        /// Create customer and save that in Data base
        /// </summary>
        /// <param name="customerVm"></param>
        /// <returns></returns>
        public static string CreateCustomer(CustomerVM customerVm)
        {
            var db = new AccountingContainer();

            var Customertem = db.CustomerTemps.Find(customerVm._Customer_Id);
            var cust = db.Customers.Find(customerVm._Customer_Id);
            cust.Customer_Name = customerVm.Customer_Name;
            try
            {

                if (Customertem.ContentPersonTemps.FirstOrDefault() != null)
                {
                    foreach (var contact in Customertem.ContentPersonTemps.ToList())
                    {
                        var contactdetail = new ContentPersonDetails();
                        contactdetail.ContentPerson_ID = contact.ContentPerson_ID;
                        contactdetail.Content_Job = contact.Content_Job;
                        contactdetail.Content_Mail = contact.Content_Mail;
                        contactdetail.Content_Name = contact.Content_Name;
                        contactdetail.Content_Number = contact.Content_Number;
                        contactdetail.ContentPerson_ID = contact.ContentPerson_ID;
                        cust.ContentPersonDetails.Add(contactdetail);
                        db.ContentPersonDetailsTemps.Remove(contact);

                    }

                    db.CustomerTemps.Remove(Customertem);

                    db.SaveChanges();
                    db.Dispose();
                    return "Added Successufully";

                }
            }
            catch (Exception e)
            {
                var masg = e.Message;
                var result = e.Data;

                throw;
            }

            return "Is not added";
        }
        /// <summary>
        /// get List of Contect Person Temp
        /// </summary>
        /// <param name="conid"></param>
        /// <returns></returns>
        public static List<ContentPersonDetailsTemp> GetAllTempCustomerDetailsresult2(int conid)
        {
            var db = new AccountingContainer();
            var cust = db.Customers.Find(conid);
            
            var Contwnt = db.CustomerTemps.Find(conid);
          
           
            db.ContentPersonDetailsTemps.RemoveRange(Contwnt.ContentPersonTemps);



            db.SaveChanges();
           

            List<ContentPersonDetailsTemp> Lista = new List<ContentPersonDetailsTemp> { };
          
                foreach (var item in cust.ContentPersonDetails)
            {
                var temp = new ContentPersonDetailsTemp();

                temp.Content_Job = item.Content_Job;
                temp.Content_Mail = item.Content_Mail;
                temp.Content_Name = item.Content_Name;
                temp.Content_Number = item.Content_Number;
                temp.CustomerTemp_Id = item.CustomerCustomer_Id;
            
                Lista.Add(temp);

            }
            db.ContentPersonDetailsTemps.AddRange(Lista);

            db.SaveChanges();

            return Contwnt.ContentPersonTemps.ToList();
        }
        /// <summary>
        /// Delete Contact temp from DB
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public static ContentPersonDetailsTemp DeleteContentFromTemp(int contentId)
        {
            var db = new AccountingContainer();
            ContentPersonDetailsTemp vm = db.ContentPersonDetailsTemps.Find(contentId);
            db.ContentPersonDetailsTemps.Remove(vm);
            db.SaveChanges();
            db.Dispose();
            return vm;
        }

        /// <summary>
        /// Get Customer Drobdown List
        /// </summary>
        /// <returns></returns>
        public static List<Customer> getAllCustomerFordrobDownList()
        {
            var db = new AccountingContainer();

            var lista = db.Customers.ToList();

            return lista;


        }
        /// <summary>
        /// import Customer By Excel File
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>

        public static String Import(HttpPostedFileBase excelfile)
        {

            var data= getdataFillter(excelfile);
            List<Customer> listCustomers = new List<Customer>();
            if (data.Item2.Count() == 0)
            {
                foreach (var item in data.Item1)
                {
                  var cust=  AddNewCustomerFromFile(item);
                    listCustomers.Add(cust);

                }
                var listcutomerEmpty = listCustomers.Where(a => a.Customer_Balance != 0).ToList();

                if (listcutomerEmpty.Count == 0)
                {

                }
                else
                {
                    Utilites.OpenBalanceFile.MakeOpenBallanceForGroupOFCutomer(listCustomers);
                }

                return "تم حفظ الملف بنجاح";

            }
            string combindedString = string.Join(" ,", data.Item2.ToArray());
            return "يوجد اخطاء فى الاسطر التالية" + combindedString;

        }
        /// <summary>
        /// Check File Validation in Excel File
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        public static Tuple<List<CustomerVM>, List<String>> getdataFillter(HttpPostedFileBase excelfile)
        {
            var customerList = new List<CustomerVM> { };
            var messageList = new List<String> { };
            var db = new AccountingContainer();
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return Tuple.Create(customerList, messageList);

            }

            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                   // string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + excelfile.FileName);
                    var nameOfFile = "Customer" + DateTime.Now.ToBinary() + ".xlsx";
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + nameOfFile);

                    //if (System.IO.File.Exists(path))
                    //{


                    //    File.Delete(path);




                    //}

                    excelfile.SaveAs(path);

                    Application application = new Application();
                    Workbook workbook = application.Workbooks.Open(path);
                    Worksheet worksheet = workbook.ActiveSheet;
                    Range range = worksheet.UsedRange;
                    for (int row = 3; row <= range.Rows.Count; row++)
                    {
                        CustomerVM cust = new CustomerVM();
                       // cust.Customer_Id = int.Parse(((Range)range.Cells[row, 1]).Text);
                        cust.Customer_Name = ((Range)range.Cells[row, 1]).Text;
                        if (cust.Customer_Name == "")
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }
                        
                        cust.Customer_Phone = ((Range)range.Cells[row, 2]).Text;
                        if (cust.Customer_Phone == "")
                        {
                            messageList.Add(row.ToString());
                            continue;

                        }
                        try
                        {
                            var cusbal = ((Range)range.Cells[row, 3]).Text;
                            if (cusbal == "")
                            {
                                cust.Customer_Balance = 0;
                            }
                            else
                            {
                                cust.Customer_Balance = Convert.ToDecimal(((Range)range.Cells[row, 3]).Text);
                            }


                            //cust.Customer_Balance = Convert.ToDecimal(((Range)range.Cells[row, 3]).Text);
                            cust.CustomerSelectSlice_Id = Convert.ToInt32(((Range)range.Cells[row, 5]).Text);
                           //var empid= Convert.ToInt32(((Range)range.Cells[row, 11]).Text);
                           // var sliece = db.Slices.Find(cust.CustomerSelectSlice_Id).Slice_Code;
                           // if (empid > 0)
                           // {
                           //     Employee emp = db.Employees.Find(empid);
                           //     cust.CustEmployeeId = emp.Emp_Id;
                           // }
                        }
                        catch
                        {

                            messageList.Add(row.ToString());
                            continue;


                        }
 

                        cust.Customer_Mail = ((Range)range.Cells[row, 4]).Text;
                        cust.Customer_Address = ((Range)range.Cells[row, 6]).Text;
                        cust.Customer_Mobile = ((Range)range.Cells[row, 7]).Text;
                        cust.Customer_Website = ((Range)range.Cells[row, 8]).Text;
                        cust.Customer_Fax = ((Range)range.Cells[row, 9]).Text;
                        cust.customer_TaxNumber = ((Range)range.Cells[row, 10]).Text;

                        customerList.Add(cust);
                        


                       

                    }

                    workbook.Save();
                    workbook.Close(path);
                   // File.Delete(path);

                }
            }
            return Tuple.Create(customerList, messageList);
        }
      /// <summary>
      /// Create Customer By using File Excel
      /// </summary>
      /// <param name="customerVm"></param>
      /// <returns></returns>
        public static Customer AddNewCustomerFromFile(CustomerVM customerVm)
        {
            var db = new AccountingContainer();
            var customer = new Customer();
            var Customertem = new CustomerTemp();

            if (customerVm._Customer_Id != 0)
            {
                Customertem = db.CustomerTemps.Find(customerVm._Customer_Id);
            }
            try
            {
                if (customerVm != null)
                {
                    // create selectList from Database to entity Contry.
                  
                    var resultSliceId = db.Slices.Find(customerVm.CustomerSelectSlice_Id);
                    if (resultSliceId != null)
                    {
                        customer.SlicesSlice_Id = resultSliceId.Slice_Id;
                    }
                    //TODO: Acquire code auromation. from VM
                    customer.Employee_Id = customerVm.CustEmployeeId;
                    customer.Customer_Balance = customerVm.Customer_Balance;
                    customer.Customer_Name = customerVm.Customer_Name;
                    customer.Customer_Address = customerVm.Customer_Address;
                    customer.Customer_Phone = customerVm.Customer_Phone;
                    customer.Customer_Mail = customerVm.Customer_Mail;
                    customer.Customer_Fax = customerVm.Customer_Fax;
                    customer.Customer_TaxNumber = customerVm.customer_TaxNumber;
                    customer.Customer_Mobile = customerVm.Customer_Mobile;
                    //customer.Customer_Type = Customer_Type.اخفاء;
                    customer.Customer_State = Models.Customer_State.نشط;
                    customer.Customer_Bal = true;
                    customer.Customer_Website = customerVm.Customer_Website;
                    customerVm.SelectedChartOfAccountId = 37;

                    var ChartOfAccount = db.ChartOfAccountings.Find(customerVm.SelectedChartOfAccountId);
                    var ChartOfAccountParent = db.ChartOfAccountings.Find(ChartOfAccount.ChartOfAccountingAccount_Id);

                    var lastSerial = db.Customers.OrderByDescending(c => c.Customer_Id).FirstOrDefault();
                    if (lastSerial == null)
                    {
                        customer.Customer_Code = "CUST-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        customer.Customer_Code = "CUST-" + (Convert.ToInt32(lastSerial.Customer_Code.Substring(5, lastSerial.Customer_Code.Length - 5)) + 1).ToString("D3");
                    }
                    var accounts = new Accounts();
                    accounts.Account_Id = ChartOfAccount.Account_Id.ToString();
                    accounts.Account_Code = customer.Customer_Code;
                    var date = DateTime.Now.ToString();
                    accounts.Account_Date = DateTime.Now.Date;
                    accounts.Account_Name = customer.Customer_Name;
                    accounts.Account_Type = ChartOfAccount.Account_Name;
                    accounts.Account_Desc = ChartOfAccountParent.Account_Name;
                    accounts.Account_Balance = 0;

                    db.Accounts.Add(accounts);

                  


                    db.Customers.Add(customer);


                    db.SaveChanges();
                    db.Dispose();
                    return customer;

                }
            }
            catch (Exception e)
            {
                var masg = e.Message;
                var result = e.Data;

                throw;
            }

            return customer;
        }
        /// <summary>
        /// drobdown List For Slice 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSlicesList()
        {
            var db = new AccountingContainer();
          

            var SlicesList = new SelectList(db.Slices, "Slice_Id", "Slice_Name");

            return SlicesList;
        }

        //public static decimal GetBalanceForCustomer(int custId)
        //{
        //    var db = new AccountingContainer();
        //    var customer = db.Customers.Where(C => C.Customer_Id == custId).ToList();
        //    decimal total = 0;
        //    var account = db.Accounts.ToList();
        //    foreach (var cut in customer)
        //    {
               
              
        //        var custaccount = db.Accounts.Where(a => a.Account_Code == cut.Customer_Code).FirstOrDefault();
        //        if (custaccount != null)
        //        {
        //            total += custaccount.Account_Balance.Value;
        //        }


        //    }


        //    return total;


        //}
    }
}
