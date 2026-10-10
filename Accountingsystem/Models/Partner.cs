using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using System.Web.Mvc;

using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace Accountingsystem.Models
{
    public partial class Partner
    {

        public static List<Partner> GetPartners()
        // i want here to get all Partner in a list .
        {
            var db = new AccountingContainer();
            var part = db.Partners.ToList();
           
            return part;
        }
    


        public static List<Partner> GetPartnerById(int comp_id) 
        //Person will get Partner by iD to find it 
        // detail function
        {
            var db = new AccountingContainer();
            var part = db.Partners.Where(b => b.CompanyComp_Id == comp_id).ToList();
            return part;
        }


        public static Partner GetPartById(int partid)
        //Person will get Partner by iD to find it 
        // detail function
        {
            var db = new AccountingContainer();
            var part = db.Partners.Find(partid);
            return part;
        }






        //get partner
        public static PartnerVM CreateVMForCreation(int companyid)
        // creat vm to user to create partner 
        //load page of ceration to create company department
        {

            var db = new AccountingContainer();
            var partnervm = new PartnerVM();

            Company compmodel = new Company();
            var compdata = db.Companies.Find(companyid);
            partnervm.CompanyComp_Id = compdata.Comp_Id;
            partnervm.Company_Name = compdata.Comp_Name;
            var lastCode = (from s in db.Banks
                            select s.Bank_Code).ToList().LastOrDefault();
            partnervm.Partner_Code = (lastCode + 1).ToString();

            partnervm.MyChartOfAccountList = new SelectList(db.ChartOfAccountings.Where(accCode => accCode.Account_Code == ("0302")&&accCode.isActive!=false), "Account_Id", "Account_Name");

            return partnervm;
        }



        // post create
        public static Partner CreatePartner(PartnerVM partVM)
        // fill for data in the partner to add partner 
        {
            var db = new AccountingContainer();
            Partner p = new Partner();

           // p.Part_Id = partVM.Part_Id;
            p.Part_Name = partVM.Part_Name;
            p.Part_Phone = partVM.Part_Phone;
            p.Part_Email = partVM.Part_Email;
            p.Part_Birthday = partVM.Part_Birthday;
            p.Part_AddedDate = partVM.Part_AddedDate;
            p.Part_RemovedDate = partVM.Part_RemovedDate;
            p.Part_Nationality = partVM.Part_Nationality;
            p.CompanyComp_Id = db.Companies.FirstOrDefault().Comp_Id;


            if (partVM.Files.FirstOrDefault() != null)
            {
                foreach (var file in partVM.Files)
                {
                    var filedetails = new PartnerFileDetail();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.ProImg = data;
                    filedetails.PartnerPart_Id = p.Part_Id;
                    if (data.Length < 1000000)
                    {
                        p.PartnerFileDetails.Add(filedetails);
                    }
                   
                }

            }


            ChartOfAccountingVM accountVM = new ChartOfAccountingVM();
            accountVM.ChartOfAccountingAccount_Id = partVM.SelectedChartOfAccountId;
            accountVM.Account_Name = partVM.Part_Name;
            accountVM.Account_Curenncy = 1;
            p.Partner_Code = ChartOfAccounting.CreateAccount(accountVM);

            db.Partners.Add(p);
            db.SaveChanges();
            db.Dispose();
            return p;



        }

        public static PartnerVM FillEditPartner(int partnerid)
        // get for edit return the view of the screen fill of partner's data for user
        {
            var db = new AccountingContainer();

            Partner part = db.Partners.Find(partnerid);

            PartnerVM partvm = new PartnerVM();

            partvm.Part_Id = part.Part_Id;
            partvm.Part_Name = part.Part_Name;
            partvm.Part_Phone = part.Part_Phone;
            partvm.Part_Email = part.Part_Email;
            partvm.Part_Birthday = part.Part_Birthday;
            partvm.Part_AddedDate = part.Part_AddedDate;
            partvm.Part_RemovedDate = part.Part_RemovedDate;
            partvm.Part_Nationality = part.Part_Nationality;
            partvm.CompanyComp_Id = part.CompanyComp_Id;
            var compName = db.Companies.FirstOrDefault().Comp_Name;
            partvm.Company_Name = compName;

            return partvm;

        }




        public static Partner EditPartner(PartnerVM partvm)
        //we made that function to edit in the DB of the Partner after editing
        //we will go to the DB . we will get the Partner by id .if Partner does not exist it will return by null .
        //else it will get to Partner view model 
        {
            var db = new AccountingContainer();
            Partner part = db.Partners.Find(partvm.Part_Id);
            var partChart = db.ChartOfAccountings.Where(a => a.Account_Name == part.Part_Name).FirstOrDefault();
            if (part != null)
            {

                part.Part_Name = partvm.Part_Name;
                part.Part_Email = partvm.Part_Email;
                part.Part_Phone = partvm.Part_Phone;

                part.Part_Birthday = partvm.Part_Birthday;
                part.Part_AddedDate = partvm.Part_AddedDate;
                part.Part_RemovedDate = partvm.Part_RemovedDate;
                part.Part_Nationality = partvm.Part_Nationality;
               
                partChart.Account_Name= partvm.Part_Name;
                db.Entry(partChart).State = EntityState.Modified;
                db.Entry(part).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return part;
            }


            return null;
        }

        /// <summary>
        /// get the file by its id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>PartnerFileDetail</returns>


        public static PartnerFileDetail getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.PartnerFileDetails.Find(fileId);
            return file;
        }
        /// <summary>
        /// get All File for that by partner Id
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        public static List<PartnerFileDetail> GetAllFiles(int partId)
        {
            var db = new AccountingContainer();
            var files = db.PartnerFileDetails.Where(emp => emp.PartnerPart_Id == partId).ToList();
            return files;
        }
        public static int DeleteFile(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.PartnerFileDetails.Find(fileId);
            var comp = file.PartnerPart_Id;
            if (file != null)
            {
                db.PartnerFileDetails.Remove(file);
                db.SaveChanges();
            }

            return comp;
        }

        /// <summary>
        /// import  file in Excel file
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        public static String Import(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return "Please select a excel file<br> ";

            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + excelfile.FileName);

                    if (System.IO.File.Exists(path))
                    {
                        excelfile.SaveAs(path + "1");

                    }
                    else
                    {
                        excelfile.SaveAs(path);
                    }
                    // read data from excel file
                    Application application = new Application();
                    Workbook workbook = application.Workbooks.Open(path);
                    Worksheet worksheet = workbook.ActiveSheet;
                    Range range = worksheet.UsedRange;

                    List<Partner> listPartners = new List<Partner>();

                    var db = new AccountingContainer();

                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        Partner partner = new Partner();

                        partner.Part_Name = ((Range)range.Cells[row, 1]).Text;
                        if (partner.Part_Name == "")
                        {
                            break;
                        }

                        if (((Range)range.Cells[row, 2]).Text == "")
                        {
                            partner.Part_Phone = "";
                        }
                        else
                        {
                            partner.Part_Phone = ((Range)range.Cells[row, 2]).Text;
                        }

                        var balance = ((Range)range.Cells[row, 3]).Text;
                        if (balance == "")
                        {
                            partner.Balance_Amount = 0;
                        }
                        else
                        {
                            partner.Balance_Amount = Convert.ToDecimal(((Range)range.Cells[row, 3]).Text);
                        }

                        var email = ((Range)range.Cells[row, 4]).Text;
                        if (email == "")
                        {
                            partner.Part_Email = "";
                        }
                        else
                        {
                            partner.Part_Email = ((Range)range.Cells[row, 4]).Text;
                        }


                        var company = ((Range)range.Cells[row, 5]).Text;
                        if (company == "")
                        {
                            partner.CompanyComp_Id = 1;
                        }
                        else
                        {
                            partner.CompanyComp_Id = ((Range)range.Cells[row, 5]).Text;
                        }


                        var nationallity = ((Range)range.Cells[row, 6]).Text;
                        if (nationallity == "")
                        {
                            partner.Part_Nationality = "";
                        }
                        else
                        {
                            partner.Part_Nationality = ((Range)range.Cells[row, 6]).Text;
                        }


                        ChartOfAccountingVM accountVM = new ChartOfAccountingVM();
                        accountVM.ChartOfAccountingAccount_Id = 96;
                        accountVM.Account_Name = partner.Part_Name;
                        accountVM.Account_Curenncy = 1;
                        accountVM.Account_Balance = 0;

                        partner.Partner_Code = ChartOfAccounting.CreateAccount(accountVM);

                        listPartners.Add(partner);

                    }

                    db.Partners.AddRange(listPartners);
                    db.SaveChanges();
                    Utilites.OpenBalanceFile.MakeOpenBallanceForGroupOFpartner(listPartners);
                    db.Dispose();
                    workbook.Save();
                    workbook.Close(path);
                    File.Delete(path);
                    return "تم اضافة الملف بنجاح";
                }
                else
                {

                    return "يوجد خلل بالملف يرجى تعديل الملف واعادة المحاولة";
                }
            }

        }



    }
}