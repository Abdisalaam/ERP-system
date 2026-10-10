using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class Company
    {
        /// <summary>
        /// Get Company code and make Sure its unique
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns></returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.Companies.ToList();
            foreach (var item in allinData)
            {
                if (item.Comp_Code == Serial || !Serial.StartsWith("COM-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }
        public static string checkEndDate(string date)
        {
            var datetime = Convert.ToDateTime(date);
            var db = new AccountingContainer();
            var ComEnddate = db.Companies.FirstOrDefault().EndFinancialDate;
            if (ComEnddate != null)
            {
                if (datetime > ComEnddate.Value)
                {
                    return Resources.Expenses.CompanyFysicalFinancialYearExeeded;
                }

                
            }

            return "";
        }

        // i want here to get all people in a list 
        public static List<Company> GetCompanies()       
        {
            var db = new AccountingContainer();
            var companies = db.Companies.ToList();
            //db.Dispose();
            return companies;
        }
        //company will get company by iD to find it 
        // detail function
        public static Company GetCompanyById(int id)     
        {
            var db = new AccountingContainer();
            var company = db.Companies.Find(id);
            return company;
        }
        /// <summary>
        /// get company File by using id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public static CompanyFileDetail getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.CompanyFileDetails.Find(fileId);
            return file;
        }
        /// <summary>
        /// get All File for that Company
        /// </summary>
        /// <param name="compId"></param>
        /// <returns></returns>
        public static List<CompanyFileDetail> GetAllFiles(int compId)
        {
            var db = new AccountingContainer();
            var files = db.CompanyFileDetails.Where(emp => emp.CompanyComp_Id == compId).ToList();
            return files;
        }
        /// <summary>
        /// get company Image by using id
        /// </summary>
        /// <param name="compId"></param>
        /// <returns></returns>
        public static CompanyPhotoDetail getPhotoById(int fileId)
        {
            var db = new AccountingContainer();
            var photo = db.CompanyPhotoDetails.Find(fileId);
            return photo;
        }
        /// <summary>
        ///Get All Photo for that Company 
        /// </summary>
        /// <param name="compId"></param>
        /// <returns></returns>
        public static List<CompanyPhotoDetail> GetAllPhoto(int compId)
        {
            var db = new AccountingContainer();
            var photo = db.CompanyPhotoDetails.Where(emp => emp.CompanyComp_Id == compId).ToList();
            return photo;
        }

        //company will get company by code to find it
        public static Company GetCompanyByCode(string companyCode)    
        {
            var db = new AccountingContainer();
            var company = db.Companies.Where(c => c.Comp_Code == companyCode);
            //we will go to the DB . we will get the company code .if company does not exist it will return by null .
            var localCompany = new Company();
            if (company != null)
            {
                localCompany = company.FirstOrDefault();
                return localCompany;
            }
            return null;

        }

        // creat vm to user to create branch department 
        //load page of ceration to create company
        public static CompanyVM CreateVMForCreation()
        {
            var db = new AccountingContainer();
            var CompanyVM = new CompanyVM();
            var currencyVM = new CurrencyVM();
            CompanyVM.currencyPartial = currencyVM;
            CompanyVM.MyDepTypeList = new SelectList(db.CompanyConfigrations, "Id", "TypeOfConfigration");
            CompanyVM.MyCountryList = new SelectList(db.Countries, "Country_Id", "Country_Name");
            CompanyVM.MyProvinceList = new SelectList(db.Provinces, "Province_Id", "Province_Name");
            CompanyVM.MyCitiesList = new SelectList(db.Cities, "City_Id", "City_Name");
            CompanyVM.currencyPartial.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id ", "Currency_Type");

            var lastCompany = db.Companies.OrderByDescending(c => c.Comp_Id).FirstOrDefault();
            if (lastCompany == null)
            {
                CompanyVM.Comp_Code = "COM-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                CompanyVM.Comp_Code = "COM-" + (Convert.ToInt32(lastCompany.Comp_Code.Substring(4, lastCompany.Comp_Code.Length - 4)) + 1).ToString("D3");
            }
            return CompanyVM;
        }

        //CompanyVM  we made it to add country ,Provinces , Cities & id of the Companies in the DB.
        public static bool CreateCompany(CompanyVM cmpVM)        
        {
            var db = new AccountingContainer();

            var Company = new Company();
            try
            {
                if (cmpVM != null)
                {
                    var MyResult = db.Companies.ToList();

                    if (MyResult.Any())
                    {
                        var MyCompany = MyResult.FirstOrDefault();
                        if (MyCompany.Comp_Name == cmpVM.Comp_Name)
                        {
                            return true;
                        }

                    }
                    else
                    {
                        // we made selecte list to select country from it
                        var resultCountry = db.Countries.Find(cmpVM.SelectedCountryId);


                        if (resultCountry != null)
                        {
                            Company.Company_Country = resultCountry.Country_Name;
                        }
                        // we made selecte list to select Provinces from it
                        var resultProvinces = db.Provinces.Find(cmpVM.SelectedProvinceId);
                        if (resultProvinces != null)
                        {
                            Company.Comp_Province = resultProvinces.Province_Name;
                        }
                        // we made selecte list to select Cities from it
                        var resultCities = db.Cities.Find(cmpVM.SelectedCityId);
                        if (resultCities != null)
                        {
                            Company.Comp_City = resultCities.City_Name;
                        }
                        var resultDepType = db.CompanyConfigrations.Find(cmpVM.SelectedDepTypeId);

                        if (resultDepType != null)
                        {
                            resultDepType.ChosenBoolean = true;
                            db.Entry(resultDepType).State = EntityState.Modified;
                        }

                        var resultCurrency = db.Currencies.Find(cmpVM.Comp_Currency_Id);
                        if (resultCurrency != null)
                        {
                            Company.Comp_Currency_Id = resultCurrency.Currency_Id;
                        }

                        if (cmpVM.Files.FirstOrDefault() != null)
                        {
                            foreach (var file in cmpVM.Files)
                            {
                                var filedetails = new CompanyFileDetail();
                                var data = new byte[file.ContentLength];
                                filedetails.FileName = file.FileName;
                                filedetails.ContentLength = file.ContentLength;
                                file.InputStream.Read(data, 0, file.ContentLength);
                                filedetails.ProImg = data;
                                filedetails.CompanyComp_Id = Company.Comp_Id;
                                filedetails.File_Extention = file.ContentType;
                                if (data.Length < 1000000)
                                {
                                    Company.CompanyFileDetails.Add(filedetails);
                                }
                            }
                        }

                        if (cmpVM.Photo.FirstOrDefault() != null)
                        {
                            foreach (var file in cmpVM.Photo)
                            {
                                var filedetails = new CompanyPhotoDetail();
                                var data = new byte[file.ContentLength];
                                filedetails.FileName = file.FileName;
                                filedetails.ContentLength = file.ContentLength;
                                file.InputStream.Read(data, 0, file.ContentLength);
                                filedetails.ProImg = data;
                                filedetails.CompanyComp_Id = Company.Comp_Id;
                                filedetails.File_Extention = file.ContentType;
                                if (data.Length <1000000)
                                {
                                    Company.CompanyPhotoDetails.Add(filedetails);
                                }
                                
                            }
                        }
                        if (cmpVM.ImageHeader != null && cmpVM.ImageHeader.FirstOrDefault() != null)
                        {
                            foreach (var file in cmpVM.ImageHeader)
                            {
                                var datam = new byte[file.ContentLength];
                                var length = file.ContentLength;
                                file.InputStream.Read(datam, 0, file.ContentLength);
                                Company.ImageHeader = datam;

                            }
                        }
                        if (cmpVM.ImageFooter != null && cmpVM.ImageFooter.FirstOrDefault() != null)
                        {
                            foreach (var file in cmpVM.ImageFooter)
                            {
                                var datam = new byte[file.ContentLength];
                                var length = file.ContentLength;
                                file.InputStream.Read(datam, 0, file.ContentLength);
                                Company.ImageFooter = datam;

                            }
                        }
                        if (cmpVM.ImageWaterMark != null && cmpVM.ImageWaterMark.FirstOrDefault() != null)
                        {
                            foreach (var file in cmpVM.ImageWaterMark)
                            {
                                var datam = new byte[file.ContentLength];
                                var length = file.ContentLength;
                                file.InputStream.Read(datam, 0, file.ContentLength);
                                Company.ImageWaterMark = datam;

                            }
                        }
                        var lastCompany = db.Companies.OrderByDescending(c => c.Comp_Id).FirstOrDefault();
                        if (lastCompany == null)
                        {
                            Company.Comp_Code = "COM-001";
                        }
                        else
                        {
                            //using string substring method to get the number of the last inserted
                            Company.Comp_Code = "COM-" + (Convert.ToInt32(lastCompany.Comp_Code.Substring(4, lastCompany.Comp_Code.Length - 4)) + 1).ToString("D3");
                        }

                        if (cmpVM.StartFinancialDate == null)
                        {
                            Company.StartFinancialDate = DateTime.Now.Date;
                        }
                        else
                        {
                            Company.StartFinancialDate = Convert.ToDateTime(cmpVM.StartFinancialDate).Date;
                        }
                        if (cmpVM.EndFinancialDate == null)
                        {
                            Company.EndFinancialDate = DateTime.Now.Date;
                        }
                        else
                        {
                            Company.EndFinancialDate = Convert.ToDateTime(cmpVM.EndFinancialDate).Date;
                        }
                        if (Convert.ToDateTime (cmpVM.StartFinancialDate)>=Convert.ToDateTime( cmpVM.EndFinancialDate)) {

                            return false;

                        }
                        Company.CRN = cmpVM.CRN;
                        Company.Country = cmpVM.Country;
                        Company.Province = cmpVM.Province;
                        Company.City = cmpVM.City;

                        Company.Number = cmpVM.Number;
                        Company.Comp_Name = cmpVM.Comp_Name;
                        Company.Comp_Address = cmpVM.Comp_Address;
                        Company.Comp_Email = cmpVM.Comp_Email;
                        Company.Comp_Fax = cmpVM.Comp_Fax;
                        Company.Comp_Field = cmpVM.Comp_Field;
                        Company.Comp_phone = cmpVM.Comp_phone;
                        Company.Tax_Number = cmpVM.Tax_Number;
                        var SubCompany = new SubLedger();
                        SubCompany.SubLed_Code = "0";
                        SubCompany.SubLed_Date = DateTime.Now;
                        SubCompany.SubLed_Desc = cmpVM.Comp_Name;
                        SubCompany.Subled_Level = 0;
                        SubCompany.SubLed_Name = cmpVM.Comp_Name;
                        SubCompany.SubLedger_Balance = 0;
                        SubCompany.SubLed_Type = "شركة";
                        SubCompany.Subledger_Expense = 0;
                        SubCompany.Subledger_Revenu = 0;
                        db.SubLedgers.Add(SubCompany);
                        db.Companies.Add(Company);
                        db.SaveChanges();
                        db.Dispose();
                        return true;
                    }
                }
            }
             
            catch (Exception ex)
            {
                var msg = ex.Message;
                var result = ex.Data;
                var tt = result;

                return false;
            }
            return false;

        }

        public static IEnumerable<SelectListItem> GetCountryProvinces(int countryId)
        // method for select list
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> provinces = new SelectList(db.Provinces.Where(p => p.CountryId == countryId), "Province_Id", "Province_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return provinces;
        }


        public static IEnumerable<SelectListItem> GetProvincesCities(int provinceId)
        // method for select list
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> Cities = new SelectList(db.Cities.Where(p => p.ProvinceId == provinceId), "City_Id", "City_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return Cities;
        }




        public static CompanyVM FillTheViewModel(int id)
        // get for edit return the view of the screen fill of person's data for user 
        {
            var db = new AccountingContainer();

            var CompanyModel = GetCompanyById(id);
            CompanyVM companyvm = new CompanyVM();
            companyvm.CRN = CompanyModel.CRN;
            companyvm.Comp_Id = CompanyModel.Comp_Id;
            companyvm.Comp_Code = CompanyModel.Comp_Code;
            companyvm.Comp_Name = CompanyModel.Comp_Name;
            companyvm.Comp_Address = CompanyModel.Comp_Address;
            companyvm.Comp_Email = CompanyModel.Comp_Email;
            companyvm.Comp_Fax = CompanyModel.Comp_Fax;
            companyvm.Comp_Field = CompanyModel.Comp_Field;
            //companyvm.Comp_PortalCode = CompanyModel.Comp_PortalCode;
            companyvm.Comp_phone = CompanyModel.Comp_phone;
            companyvm.Tax_Number = CompanyModel.Tax_Number;
            //companyvm.StartFinancialDate = CompanyModel.StartFinancialDate.ToString();
            //companyvm.EndFinancialDate = CompanyModel.EndFinancialDate.ToString();
            companyvm.Number = CompanyModel.Number;

            companyvm.Country = CompanyModel.Country;
            companyvm.Province = CompanyModel.Province;
            companyvm.City = CompanyModel.City;

            companyvm.EndFinancialDate = CompanyModel.EndFinancialDate.Value.ToString("dd-MM-yyyy   HH:mm:ss"); 
            companyvm.StartFinancialDate = CompanyModel.StartFinancialDate.Value.ToString("dd-MM-yyyy   HH:mm:ss");
            var countryName = CompanyModel.Company_Country;
            var provinceName = CompanyModel.Comp_Province;
            var cityName = CompanyModel.Comp_City;
            var currencyTypeId = CompanyModel.Comp_Currency_Id;

            var Country = db.Countries.Where(c => c.Country_Name == countryName);
            if (Country.Any())
            {
                companyvm.SelectedCountryId = Country.First().Country_Id;
            }
            var province = db.Provinces.Where(c => c.Province_Name == provinceName);
            if (province.Any())
            {
                companyvm.SelectedProvinceId = province.First().Province_Id;
            }
            var city = db.Cities.Where(c => c.City_Name == cityName);
            if (city.Any())
            {
                companyvm.SelectedCityId = city.First().City_Id;
            }

            var currency = db.Currencies.Where(c => c.Currency_Id == currencyTypeId);
            if (currency.Any())
            {
                companyvm.Comp_Currency_Id = currency.First().Currency_Id;
            }

            companyvm.MyCountryList = new SelectList(db.Countries, "Country_Id", "Country_Name");
            companyvm.MyProvinceList = new SelectList(db.Provinces, "Province_Id", "Province_Name");
            companyvm.MyCitiesList = new SelectList(db.Cities, "City_Id", "City_Name");
            companyvm.MyDepTypeList = new SelectList(db.CompanyConfigrations, "Id", "TypeOfConfigration");
            companyvm.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id ", "Currency_Type");



            /*  companyvm.MyCountryList = CompanyModel.Company_Country;
              companyvm.MyProvinceList = CompanyModel.Comp_Province;
              companyvm.MyCitiesList = CompanyModel.Comp_City;*/


            return companyvm;

        }


        public static Company EditCompany(CompanyVM compvm)
        //we made that function to edit in the DB of the company after editing
        //we will go to the DB . we will get the company by id .if company does not exist it will return by null .
        //else it will get to company view model & then it will be the oppsite of Adding company . 
        {
            var db = new AccountingContainer();
            Company comp = db.Companies.Find(compvm.Comp_Id);
            var sub = db.SubLedgers.Where(a => a.SubLed_Name == comp.Comp_Name).FirstOrDefault();

            if (comp != null)
            {
                comp.CRN = compvm.CRN;
                comp.Comp_Name = compvm.Comp_Name;
              
                comp.Comp_Address = compvm.Comp_Address;
                comp.Comp_Email = compvm.Comp_Email;
                comp.Comp_Fax = compvm.Comp_Fax;
                comp.Comp_Field = compvm.Comp_Field;
                comp.Comp_phone = compvm.Comp_phone;
                comp.Tax_Number = compvm.Tax_Number;
                comp.Country = compvm.Country;
                comp.Province = compvm.Province;
                comp.City = compvm.City;
                //comp.Comp_PortalCode = compvm.Comp_PortalCode;
                if (Convert.ToDateTime(compvm.StartFinancialDate) >= Convert.ToDateTime(compvm.EndFinancialDate))
                {

                    return null;

                }
                var resultCountry = db.Countries.Find(compvm.SelectedCountryId);
                if (resultCountry != null)
                {
                    comp.Company_Country = resultCountry.Country_Name;
                }

                var resultProvinces = db.Provinces.Find(compvm.SelectedProvinceId);
                if (resultProvinces != null)
                {
                    comp.Comp_Province = resultProvinces.Province_Name;
                }

                var resultCities = db.Cities.Find(compvm.SelectedCityId);
                if (resultCities != null)
                {
                    comp.Comp_City = resultCities.City_Name;
                }
                var resultCurrency = db.Currencies.Find(compvm.Comp_Currency_Id);
                if (resultCurrency != null)
                {
                    comp.Comp_Currency_Id = resultCurrency.Currency_Id;
                }
                
                if (compvm.Files.FirstOrDefault() != null)
                {
                    foreach (var file in compvm.Files)
                    {
                        var filedetails = new CompanyFileDetail();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.ProImg = data;
                        filedetails.CompanyComp_Id = comp.Comp_Id;
                        filedetails.File_Extention = file.ContentType;
                        if (filedetails.ProImg.Length > 1000000)
                        {

                        }
                        else
                        {
                            comp.CompanyFileDetails.Add(filedetails);
                        }
                    }
                }

                if (compvm.Photo.FirstOrDefault() != null)
                {
                    var img = db.CompanyPhotoDetails.Where(a => a.CompanyComp_Id == compvm.Comp_Id).ToList();
                    db.CompanyPhotoDetails.RemoveRange(img);
                    db.SaveChanges();
                    foreach (var file in compvm.Photo)
                    {
                        var filedetails = new CompanyPhotoDetail();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.ProImg = data;
                        filedetails.CompanyComp_Id = comp.Comp_Id;
                        filedetails.File_Extention = file.ContentType;
                        if (filedetails.ProImg.Length > 1000000)
                        {

                        }
                        else
                        {
                            comp.CompanyPhotoDetails.Add(filedetails);
                        }
                       
                    }
                }

                if (compvm.ImageHeader!=null && compvm.ImageHeader.FirstOrDefault() != null)
                {
                    foreach (var file in compvm.ImageHeader)
                    {
                        var datam = new byte[file.ContentLength];
                        var length = file.ContentLength;
                        file.InputStream.Read(datam, 0, file.ContentLength);
                        comp.ImageHeader = datam;

                    }
                }
                if (compvm.ImageFooter != null && compvm.ImageFooter.FirstOrDefault() != null)
                {
                    foreach (var file in compvm.ImageFooter)
                    {
                        var datam = new byte[file.ContentLength];
                        var length = file.ContentLength;
                        file.InputStream.Read(datam, 0, file.ContentLength);
                        comp.ImageFooter = datam;

                    }
                }
                if (compvm.ImageWaterMark != null && compvm.ImageWaterMark.FirstOrDefault() != null)
                {
                    foreach (var file in compvm.ImageWaterMark)
                    {
                        var datam = new byte[file.ContentLength];
                        var length = file.ContentLength;
                        file.InputStream.Read(datam, 0, file.ContentLength);
                        comp.ImageWaterMark = datam;

                    }
                }

                comp.Number = compvm.Number;
                if (compvm.StartFinancialDate == null)
                {
                    comp.StartFinancialDate = DateTime.Now.Date;
                }
                else
                {
                    comp.StartFinancialDate = Convert.ToDateTime(compvm.StartFinancialDate).Date;
                }
                if (compvm.EndFinancialDate == null)
                {
                    comp.EndFinancialDate = DateTime.Now.Date;
                }
                else
                {
                    comp.EndFinancialDate = Convert.ToDateTime(compvm.EndFinancialDate).Date;
                }
               
              

                sub.SubLed_Name = compvm.Comp_Name;
                db.Entry(sub).State = EntityState.Modified;
                db.Entry(comp).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return comp;
            }


            return null;
        }




        public static CompDeptVM CreateVMForCompDept(int compid)
        // creat vm to user to create company department 
        //load page of ceration to create company department
        {
            var db = new AccountingContainer();
            var compdeptvm = new CompDeptVM();
            Company compmodel = new Company();
            var compdata = db.Companies.Find(compid);
            compdeptvm.Company_Name = compdata.Comp_Name;

            var lastCode = db.CompanyDepartments.OrderByDescending(c => c.Id).FirstOrDefault();
            if (lastCode == null)
            {
                compdeptvm.CompDept_Code = "DEPCOM-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                compdeptvm.CompDept_Code = "DEPCOM-" + (Convert.ToInt32(lastCode.CompDept_Code.Substring(7, lastCode.CompDept_Code.Length - 7)) + 1).ToString("D3");
            }
            return compdeptvm;
        }

        public static int CreateCompDept(CompDeptVM compdeptvm)
        //CompDeptVM we made it to add name ,Description , Phone & Code  of the Branch in the DB.
        {

            var db = new AccountingContainer();
            var SubVM = new SubLedgerVM();
            var CompanyDepartmentsmodel = new CompanyDepartments();
            if (compdeptvm.CompDept_Name == null)
            {
                return 0;
            }
            var compdata = db.Companies.FirstOrDefault();

            CompanyDepartmentsmodel.CompanyComp_Id = compdata.Comp_Id;
            //CompanyDepartmentsmodel.CompDept_Code = compdeptvm.CompDept_Code;
            CompanyDepartmentsmodel.CompDept_Name = compdeptvm.CompDept_Name;
            CompanyDepartmentsmodel.CompDept_Phone = compdeptvm.CompDept_Phone;
            CompanyDepartmentsmodel.CompDept_Description = compdeptvm.CompDept_Description;
            var lastCode = db.CompanyDepartments.OrderByDescending(c => c.Id).FirstOrDefault();
            if (lastCode == null)
            {
                CompanyDepartmentsmodel.CompDept_Code = "DEPCOM-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                CompanyDepartmentsmodel.CompDept_Code = "DEPCOM-" + (Convert.ToInt32(lastCode.CompDept_Code.Substring(7, lastCode.CompDept_Code.Length - 7)) + 1).ToString("D3");
            }
            db.CompanyDepartments.Add(CompanyDepartmentsmodel);
            db.SaveChanges();
            SubVM.ParentSubledger_Id = db.SubLedgers.Where(a => a.SubLed_Type == "شركة").FirstOrDefault().SubLed_Id;
            SubVM.SubLed_Name = compdeptvm.CompDept_Name;
            SubVM.SubLed_Date = Convert.ToString(DateTime.Now);
            SubVM.SubLed_Type = "قسم رئيسى لشركة";
            SubLedger.createSubLedger(SubVM);
            db.Dispose();
            return CompanyDepartmentsmodel.Id;

        }


        /// <summary>
        /// get Cuurience DrobDownList
        /// </summary>
        /// <returns></returns>
        public static CompanyVM GetCurrencyList()
        {
            var db = new AccountingContainer();
            CompanyVM vm = new CompanyVM();

            vm.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");

            return vm;
        }

        public static int DeletePhoto(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.CompanyPhotoDetails.Find(fileId);
            var comp = file.CompanyComp_Id;
            if (file != null)
            {
                db.CompanyPhotoDetails.Remove(file);
                db.SaveChanges();
            }

            return comp;
        }

        public static int DeleteFile(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.CompanyFileDetails.Find(fileId);
            var comp = file.CompanyComp_Id;
            if (file != null)
            {
                db.CompanyFileDetails.Remove(file);
                db.SaveChanges();
            }

            return comp;
        }
        public static Company getTheFirstPage()
        {
            var db = new AccountingContainer();
            var Com = db.Companies.FirstOrDefault();
            if (Com == null)
            {
                var compnull = new Company();
                compnull.Comp_Name = "اسم الشركة";
                return compnull;
            }
            var chart = db.ChartOfAccountings.Where(a => a.Account_Code == "040102004").FirstOrDefault();
            if (chart == null)
            {
                var chartnew = new ChartOfAccounting();
                chartnew.Account_Name = "خصم مشاريع مسموح به";
                chartnew.Account_Code = "040102004";
                chartnew.ChartOfAccountingAccount_Id = 79;
                chartnew.Account_Level = 4;
                chartnew.Currency_Name = "ريال سعودي";
                chartnew.BalanceSheetValue = 0;
                chartnew.Account_Balance = 0;

                db.ChartOfAccountings.Add(chartnew);

            }
            var chart2 = db.ChartOfAccountings.Where(a => a.Account_Code == "040103004").FirstOrDefault();
            if (chart2 == null)
            {
                var chartnew1 = new ChartOfAccounting();
                chartnew1.Account_Name = "خصم اشتراكات مسموح به";
                chartnew1.Account_Code = "040103004";
                chartnew1.ChartOfAccountingAccount_Id = 83;
                chartnew1.Account_Level = 4;
                chartnew1.Currency_Name = "ريال سعودي";
                chartnew1.BalanceSheetValue = 0;
                chartnew1.Account_Balance = 0;
          
                db.ChartOfAccountings.Add(chartnew1);

            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return Com;



        }


    }
}