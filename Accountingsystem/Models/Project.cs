using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class Project
    {/// <summary>
     /// تقوم بارجاع جميع المشاريع المخزنة
     /// </summary>
     /// <returns>List<Project></returns>
        public static List<Project> getAllDataWorking()
        {
            var db = new AccountingContainer();
            var project = db.Projects.Where(e=>e.Project_State==ProjectState.Working).ToList();
            return project;
        }
        /// <summary>
        /// get all project that status is finished
        /// </summary>
        /// <returns>List<Project></returns>
        public static List<Project> getAllDataFinshed()
        {
            var db = new AccountingContainer();
            var project = db.Projects.Where(e => e.Project_State ==ProjectState.Finished).ToList();
            return project;
        }
        /// <summary>
        /// get all Project is status not work 
        /// </summary>
        /// <returns>List<Project></returns>
        public static List<Project> getAllDataNotWokring()
        {
            var db = new AccountingContainer();
            var project = db.Projects.Where(e => e.Project_State == ProjectState.NotWorking).ToList();
            return project;
        }
        /// <summary>
        /// get All Project in data base
        /// </summary>
        /// <returns></returns>
        public static List<Project> getAllData()
        {
            var db = new AccountingContainer();
            var project = db.Projects.ToList();
            return project;


        }
        /// <summary>
        /// make aproject with unique code 
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns>bool</returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.Projects.ToList();
            foreach (var item in allinData)
            {

                if (item.Project_Code == Serial || !Serial.StartsWith("PRO-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }
        /// <summary>
        /// getthe file of project by its id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>ProjectFileDetail </returns>
        public static ProjectFileDetail getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.ProjectFileDetails.Find(fileId);
            return file;
        }
        /// <summary>
        /// get All Files belong to Project used by Project id
        /// </summary>
        /// <param name="projId"></param>
        /// <returns></returns>
        public static List<ProjectFileDetail> GetAllFiles(int projId)
        {
            var db = new AccountingContainer();
            var files = db.ProjectFileDetails.Where(emp => emp.ProjectProject_Id == projId).ToList();
            return files;
        }
        /// <summary>
        /// get customer List for DroPdownList
        /// </summary>
        /// <returns>ProjectVM</returns>
        public static IEnumerable<SelectListItem> GetCustomerList()
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> customers = new SelectList(db.Customers.Where(a => a.Customer_State == Customer_State.نشط), "Customer_Id", "Customer_Name");
            return customers;

        }
        /// <summary>
        /// ارسال واجهه لأضافة مشروع جديد
        /// </summary>
        /// <returns>ProjectVM</returns>
        public static ProjectVM fillDataForCreate()
        {
            var db = new AccountingContainer();
            var provm = new ProjectVM();
            int lastCode = (from s in db.Projects
                            select s.Project_Id).Count();

            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() ==0&&a.isActive!=false);
            
            var lastSerial = db.Projects.OrderByDescending(c => c.Project_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                provm.Project_Code = "PRO-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                provm.Project_Code = "PRO-" + (Convert.ToInt32(lastSerial.Project_Code.Substring(4, lastSerial.Project_Code.Length - 4)) + 1).ToString("D3");
            }

            provm.MyEmpTeam = new SelectList(db.EmployeeTeams, "Team_ID", "Team_Name");
            provm.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            provm.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            provm.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            provm.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            provm.CompList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            var customerList = from cust in db.Customers.Where(e => e.Customer_State == Customer_State.نشط)
                               select new
                               {
                                   Id = cust.Customer_Id,
                                   Name = cust.Customer_Name + " " + cust.Customer_Phone

                               };
            provm.CustomerList = new SelectList(customerList, "Id", "Name");
            provm.MyEmpList = new SelectList(db.Employees.Where(e => e.Reg_ID != null), "Emp_Id", "Emp_Name");
            provm.MyPaymentPolicy = new SelectList(db.PaymentMethods.Where(a=>a.IsActive!=false), "PaymentMethod_Id", "Payment_Type");
            provm.MyRevenuelList = new SelectList(lista.Where(a=>a.Account_Code.StartsWith("04")), "Account_Id", "Account_Name");
            provm.MyChartOfAccountList = new SelectList(db.ChartOfAccountings.Where(accCode => accCode.Account_Code.StartsWith("010109") && accCode.isActive != false), "Account_Id", "Account_Name");
            provm.StartTime = DateTime.Now.ToString("dd-MM-yyyy");
            provm.Project_State = ProjectState.Working;
            provm.SelectedReveId = 80;
            var ProTemp = new ProjectTemp();
            var id = (from s in db.ProjectTemps
                      select s).ToList().LastOrDefault();
            if (id == null)
            { ProTemp.Project_Id = 1; }
            else
            {
                ProTemp.Project_Id = id.Project_Id + 1;
            }


            db.ProjectTemps.Add(ProTemp);
            db.SaveChanges();
            provm.Project_Id = ProTemp.Project_Id;
            return provm;

        }
        /// <summary>
        /// prapare the detail of Project to Create new detail
        /// </summary>
        /// <returns>ProjectDetailVM</returns>
        public static ProjectDetailVM CreateDetailOfProjectVM()
        {
            var db = new AccountingContainer();
            var ProDetail = new ProjectDetailVM();
           
            return ProDetail;

        }
        /// <summary>
        /// get all Project Detailtamp belong to that project 
        /// </summary>
        /// <param name="Master_Id"></param>
        /// <returns>List<ProjectDetailTemp></returns>
        public static List<ProjectDetailTemp> GetDataTempForCreation(int Master_Id)
        {
            var db = new AccountingContainer();
            var ProjectDetailTemp = db.ProjectDetailTemps.Where(a => a.ProjectTempProject_Id == Master_Id).ToList();
            return ProjectDetailTemp;
        }
        /// <summary>
        /// add to Project detail temp and check the validation
        /// </summary>
        /// <param name="ProVM"></param>
        /// <returns>string</returns>
        public static string AddToList(ProjectDetailVM ProVM)

        {
            var db = new AccountingContainer();

            if (ProVM.Amount == null || ProVM.Amount == 0)
            {
                return Resources.validate.Addpreviouspayments;
            }
           
            else
            {
                var ProTemp = new ProjectDetailTemp();
                ProTemp.Amount = ProVM.Amount;
                if (ProVM.Date_Amount!=null&& ProVM.Date_Amount!="")
                {
                    ProTemp.Date_Amount = Convert.ToDateTime(ProVM.Date_Amount);
                    ProTemp.Payed =true;
                }
               
                ProTemp.Description_Detail = ProVM.Description_Detail;
              
                ProTemp.ProjectTempProject_Id = ProVM.Project_Id;
                db.ProjectDetailTemps.Add(ProTemp);
                db.SaveChanges();
                return Resources.validate.SuccessfullAdd;
            }
        }

        /// <summary>
        /// حفظ بيانات المشروع الجديد فى قاعدة البيانات
        /// </summary>
        /// <param name="pvm"></param>
        /// <returns>Project</returns>
        public static Tuple< string,int> createProject(ProjectVM pvm,string user_Id)
        {
            var db = new AccountingContainer();
            var proj = new Project();
            var SubVM = new SubLedgerVM();
            if (pvm.deadline!=null)
            {
                proj.DeadLine = Convert.ToDateTime(pvm.deadline).Date;
            }
            var resultCusomer = db.Customers.Find(pvm.CustomerCustomer_Id);
            if (resultCusomer != null)
            {

                proj.CustomerCustomer_Id = resultCusomer.Customer_Id;
            }
            proj.Company_ID = pvm.Company_ID.ToString();
            var resultEmp = db.Employees.Find(pvm.SelectedEmpId);
            if (resultEmp != null)
            {
                proj.Emp_Id = resultEmp.Emp_Id;
            }

            var resultRevenue = db.ChartOfAccountings.Find(pvm.SelectedReveId);
            if (resultRevenue != null)
            {
                proj.Revenu_Id = resultRevenue.Account_Id;
            }

            var resultPaymentPolicy = db.PaymentMethods.Find(pvm.Payment_Method);
            if (resultPaymentPolicy != null)
            {
                proj.Payment_Method = resultPaymentPolicy.PaymentMethod_Id;
            }
            var resulEmp_team = db.EmployeeTeams.Find(pvm.SelectEmp_Team_Id);
            if (resulEmp_team != null)
            {
                proj.Emp_Team_Id = resulEmp_team.Team_ID;
            }

            if (pvm.Subledger_Id != null)
            {

                proj.SubLedger_Id = pvm.Subledger_Id;

            }
            else if (pvm.BussinesSubledger_Id != null && pvm.BussinesSubledger_Id>0)
            {

                proj.SubLedger_Id = pvm.BussinesSubledger_Id;

            }
            else if (pvm.DepartSubledger_Id != null && pvm.DepartSubledger_Id > 0)
            {

                proj.SubLedger_Id = pvm.DepartSubledger_Id;

            }
         
            else if (pvm.BranchSubledger_Id != null && pvm.BranchSubledger_Id > 0)
            {

                proj.SubLedger_Id = pvm.BranchSubledger_Id;

            }
            else { proj.SubLedger_Id = 1; }

            var ChartOfAccount = db.ChartOfAccountings.Find(pvm.SelectedReveId);
            var ChartOfAccountParent = db.ChartOfAccountings.Find(ChartOfAccount.ChartOfAccountingAccount_Id);
            int lastCode = (from s in db.Projects
                            select s.Project_Id).Count();
            proj.Project_Code = ("PRO-00" + (lastCode + 1)).ToString();

            var lastSerial = db.Projects.OrderByDescending(c => c.Project_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                proj.Project_Code = "PRO-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                proj.Project_Code = "PRO-" + (Convert.ToInt32(lastSerial.Project_Code.Substring(4, lastSerial.Project_Code.Length - 4)) + 1).ToString("D3");
            }
            proj.Project_Name = pvm.Project_Name;
            
            proj.Project_Location = pvm.Project_Location;
            
            proj.StartTime = Convert.ToDateTime(pvm.StartTime);
            proj.Estimate_Amount = pvm.Estimate_Amount;
            proj.Project_State = ProjectState.Working;
            proj.Payment_Method = pvm.Payment_Method;

            proj.BranchSubledger_Id = pvm.BranchSubledger_Id;
            proj.DepartSubledger_Id = pvm.DepartSubledger_Id;
            proj.BussinesSubledger_Id = pvm.BussinesSubledger_Id;
            proj.DetailOFPro = pvm.detailOfProject;
            proj.Notes = pvm.notes;
            proj.Project_Description = pvm.Project_Description;
            if (pvm.Files.FirstOrDefault() != null)
            {
                foreach (var file in pvm.Files)
                {
                    var filedetails = new ProjectFileDetail();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.ProImg = data;
                    filedetails.ProjectProject_Id = proj.Project_Id;
                    filedetails.File_Extention = file.ContentType;
                    if (data.Length < 1000000)
                    {
                        proj.ProjectFileDetails.Add(filedetails);
                    }
                  
                }

            }

            var ProjectDetailTem = db.ProjectDetailTemps.Where(a => a.ProjectTempProject_Id == pvm.Project_Id).ToList();
            List<ProjectDetailTemp> List = new List<ProjectDetailTemp> { };
            foreach (var item in ProjectDetailTem)
            {
                var Detail = new ProjectDetail(); ;
                Detail.Amount = item.Amount;
                Detail.Date_Amount = item.Date_Amount;
                Detail.Description_Detail = item.Description_Detail;
                Detail.Payed = item.Payed;



                proj.ProjectDetails.Add(Detail);
                List.Add(item);
                


            }
            db.ProjectDetailTemps.RemoveRange(List);
            var Temp = db.ProjectTemps.Find(pvm.Project_Id);
            db.ProjectTemps.Remove(Temp);


            SubVM.SubLed_Name = proj.Project_Name;
            SubVM.SubLed_Date = Convert.ToString( DateTime.Now);
            SubVM.ParentSubledger_Id = proj.SubLedger_Id;
            SubVM.SubLed_Type = "مشروع";
            var Proname = db.Projects.Where(a => a.Project_Name == proj.Project_Name).FirstOrDefault();
            var subName = db.SubLedgers.Where(a => a.SubLed_Name == proj.Project_Name).FirstOrDefault();
            if (Proname==null&& subName==null)
            {
                db.Projects.Add(proj);
                SubLedger.createSubLedger(SubVM);

                db.SaveChanges();
                db.Dispose();
                CreateTransactionBatch(proj, user_Id);
            }
            else
            {
                return Tuple.Create( "اسم هذا المشروع مستخدم من قبل",0);
            }

          
            return Tuple.Create( "تم انشاء مشروع بنجاح",proj.Project_Id);


        }
        /// <summary>
        /// make transaction batch  for any Creation of Project
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="userid"></param>
        public static void CreateTransactionBatch(Project proj,string userid)
        {
            var trans = new TransactionBatch();
            trans.Description = proj.Project_Description;
            trans.Date = DateTime.Now;
            trans.OperationName = "انشاء مشروع";
            // TODO: Edit UserId
             trans.UserId = userid;
            trans.OperationId = proj.Project_Id;
            trans.Operation_Code = proj.Project_Code;
            TransactionBatch.AddNewTransaction(trans);
        }
        /// <summary>
        /// يقوم بارجاع البيانات للمشروع المراد بواسطة(id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Project</returns>
        public static Project getDetail(int id)
        {
            var db = new AccountingContainer();
            var proj = db.Projects.Find(id);
            
            return proj;

        }

        public static Project getDetailbySubledgerId(int subId)
        {

            var db = new AccountingContainer();
            var projsub = db.SubLedgers.Find(subId);
            var proj = db.Projects.Where(a => a.Project_Name == projsub.SubLed_Name).FirstOrDefault();
            return proj;

            return proj;
        }
        /// <summary>
        /// عرض البيانات للمشروع المراد تعديلة
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ProjectVM</returns>
        public static ProjectVM fillDataForEditing(int id)
        {
            var proj = getDetail(id);
            var db = new AccountingContainer();
            var prvm = new ProjectVM();

            var customer = proj.CustomerCustomer_Id;
            var employee = proj.Emp_Id;
            var Reevenu = proj.Revenu_Id;
            var paymentMethod = proj.Payment_Method;
            var team_Id = proj.Emp_Team_Id;


            var accou = db.Customers.Where(c => c.Customer_Id == customer);
            if (accou.Any())
            {
                prvm.CustomerCustomer_Id = accou.FirstOrDefault().Customer_Id;
            }

            var Employee = db.Employees.Where(c => c.Emp_Id == employee);
            if (Employee.Any())
            {
                prvm.SelectedEmpId = Employee.FirstOrDefault().Emp_Id;
            }

            var Revenue = db.ChartOfAccountings.Where(c => c.Account_Id == Reevenu);
            if (Revenue.Any())
            {
                prvm.SelectedReveId = Revenue.FirstOrDefault().Account_Id;
            }

            var Payment = db.PaymentMethods.Where(c => c.PaymentMethod_Id == paymentMethod);
            if (Payment.Any())
            {
                prvm.Payment_Method = Payment.FirstOrDefault().PaymentMethod_Id;
            }
            var Emp_Team = db.EmployeeTeams.Where(c => c.Team_ID == team_Id);
            if (Emp_Team.Any())
            {
                prvm.SelectEmp_Team_Id = Emp_Team.FirstOrDefault().Team_ID;
            }


            prvm.Project_Id = proj.Project_Id;
            prvm.Project_Code = proj.Project_Code;
            prvm.Project_Name = proj.Project_Name;
            prvm.Project_Description = proj.Project_Description;
            prvm.Project_Location = proj.Project_Location;         
            prvm.StartTime = proj.StartTime.Day+"-"+ proj.StartTime.Month + "-"+ proj.StartTime.Year;
            prvm.Estimate_Amount = proj.Estimate_Amount;            
            prvm.Project_State = proj.Project_State;
            prvm.BranchSubledger_Id = proj.BranchSubledger_Id;
            prvm.DepartSubledger_Id = proj.DepartSubledger_Id;
            prvm.BussinesSubledger_Id = proj.BussinesSubledger_Id;
            prvm.Company_ID = Convert.ToInt32(proj.Company_ID);
            prvm.notes = proj.Notes;
            prvm.detailOfProject = proj.DetailOFPro;
            prvm.Project_Description = proj.Project_Description;

            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0);
            
            prvm.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1&&a.isActive!=false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            prvm.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            prvm.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            prvm.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            prvm.MyEmpTeam = new SelectList(db.EmployeeTeams, "Team_ID", "Team_Name");
            prvm.MyEmpList = new SelectList(db.Employees.Where(e => e.Reg_ID != null), "Emp_Id", "Emp_Name");
           if(proj.Emp_Id != null)
            {
                prvm.SelectedEmpId = proj.Emp_Id.Value;
            }
           
            prvm.MyPaymentPolicy = new SelectList(db.PaymentMethods.Where(a=>a.IsActive!=false), "PaymentMethod_Id", "Payment_Type");
            prvm.MyRevenuelList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("04")), "Account_Id", "Account_Name");
            prvm.MyChartOfAccountList = new SelectList(db.ChartOfAccountings.Where(accCode => accCode.Account_Code.StartsWith("010109") && accCode.isActive != false), "Account_Id", "Account_Name");
            prvm.CompList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            var custList = from cust in db.Customers.Where(e => e.Customer_State == Customer_State.نشط)
                           select new
                           {

                               Id = cust.Customer_Id,
                               Name = cust.Customer_Name + " " + cust.Customer_Phone

                           };
            prvm.CustomerList = new SelectList(custList, "Id", "Name");
            var ProTemp = new ProjectTemp();
            var ProTempExsist = db.ProjectTemps.Where(a => a.Project_Id == id).FirstOrDefault();
            if (ProTempExsist == null)
            {
                ProTemp.Project_Id = id;
                db.ProjectTemps.Add(ProTemp);
                db.SaveChanges();
            }

            prvm.Project_Id = id;
            return prvm;

        }
        /// <summary>
        /// حفظ التعديلات على المشروع وتسجيلة فى قاعدة البيانات
        /// </summary>
        /// <param name="prvm"></param>
        /// <returns>Project</returns>
        public static Project editTheData(ProjectVM prvm)
        {
            var db = new AccountingContainer();
            var SubVM = new SubLedgerVM();
            var proj = db.Projects.Find(prvm.Project_Id);
           db.ProjectDetails.RemoveRange(proj.ProjectDetails);
            

            if (proj != null)
            {
                if (prvm.deadline != null)
                {
                    proj.DeadLine = Convert.ToDateTime(prvm.deadline).Date;
                }
                var resultCusomer = db.Customers.Find(prvm.CustomerCustomer_Id);
                if (resultCusomer != null)
                {
                    proj.CustomerCustomer_Id = resultCusomer.Customer_Id;
                }

                var resultEmp = db.Employees.Find(prvm.SelectedEmpId);
                if (resultEmp != null)
                {
                    proj.Emp_Id = resultEmp.Emp_Id;
                }


                var resultRevenue = db.ChartOfAccountings.Find(prvm.SelectedReveId);
                if (resultRevenue != null)
                {
                    proj.Revenu_Id = resultRevenue.Account_Id;
                }
                var resultPaymentPolicy = db.PaymentMethods.Find(prvm.Payment_Method);
                if (resultPaymentPolicy != null)
                {
                    proj.Payment_Method = resultPaymentPolicy.PaymentMethod_Id;
                }
                var resulEmp_team = db.EmployeeTeams.Find(prvm.SelectEmp_Team_Id);
                if (resulEmp_team != null)
                {
                    proj.Emp_Team_Id = resulEmp_team.Team_ID;
                }
                if (prvm.Subledger_Id != null)
                {

                    proj.SubLedger_Id = prvm.Subledger_Id;

                }
                else if (prvm.BussinesSubledger_Id != null && prvm.BussinesSubledger_Id>0)
                {

                    proj.SubLedger_Id = prvm.BussinesSubledger_Id;

                }
                else if (prvm.DepartSubledger_Id != null && prvm.DepartSubledger_Id > 0)
                {

                    proj.SubLedger_Id = prvm.DepartSubledger_Id;

                }
                else if (prvm.BranchSubledger_Id != null && prvm.BranchSubledger_Id > 0)
                {

                    proj.SubLedger_Id = prvm.BranchSubledger_Id;

                }
                else { proj.SubLedger_Id = 1; }
                prvm.Project_Id = proj.Project_Id;
                proj.Project_Code = prvm.Project_Code;
                var Sub = db.SubLedgers.Where(a => a.SubLed_Name == proj.Project_Name).FirstOrDefault();
                proj.Project_Name = prvm.Project_Name;
                Sub.SubLed_Name = prvm.Project_Name;


                proj.Project_Location = prvm.Project_Location;
                proj.BranchSubledger_Id = prvm.BranchSubledger_Id;
                proj.DepartSubledger_Id = prvm.DepartSubledger_Id;
                proj.BussinesSubledger_Id = prvm.BussinesSubledger_Id;
                proj.StartTime = Convert.ToDateTime( prvm.StartTime);
                proj.Estimate_Amount = prvm.Estimate_Amount;
                proj.Payment_Method = prvm.Payment_Method;
                proj.Project_State = prvm.Project_State;
                proj.DetailOFPro = prvm.detailOfProject;
                proj.Notes = prvm.notes;
                proj.Project_Description = prvm.Project_Description;

                if (prvm.Files.FirstOrDefault() != null)
                {
                    foreach (var file in prvm.Files)
                    {
                        var filedetails = new ProjectFileDetail();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.ProImg = data;
                        filedetails.ProjectProject_Id = proj.Project_Id;
                        filedetails.File_Extention = file.ContentType;
                        if (data.Length < 1000000)
                        {
                            proj.ProjectFileDetails.Add(filedetails);
                        }
                     
                    }

                }
                var ProjectDetailTem = db.ProjectDetailTemps.Where(a => a.ProjectTempProject_Id == prvm.Project_Id).ToList();
               
                List<ProjectDetailTemp> List = new List<ProjectDetailTemp> { };
                foreach (var item in ProjectDetailTem)
                {
                    var Detail = new ProjectDetail(); ;
                    Detail.Amount = item.Amount;
                    Detail.Date_Amount = item.Date_Amount;
                    Detail.Description_Detail = item.Description_Detail;
                    Detail.Payed = item.Payed;



                    proj.ProjectDetails.Add(Detail);
                    List.Add(item);



                }
                db.ProjectDetailTemps.RemoveRange(List);
                var Temp = db.ProjectTemps.Find(prvm.Project_Id);
                db.ProjectTemps.Remove(Temp);
                
                db.Entry(proj).State = EntityState.Modified;
                db.Entry(Sub).State = EntityState.Modified;
                db.SaveChanges();
              
                db.Dispose();
                return proj;
            }
            return null;
        }
        /// <summary>
        /// prepare detail temp and put  all data of detail in detail Temp
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List<ProjectDetailTemp></returns>
        public static List<ProjectDetailTemp> GetAllTempPRojectForEdit(int id)
        {

            var db = new AccountingContainer();
            var detailTemp = db.ProjectTemps.Find(id);
            if (detailTemp.ProjectDetailTemps.FirstOrDefault()!=null)
            {
                db.ProjectDetailTemps.RemoveRange(detailTemp.ProjectDetailTemps);

                db.SaveChanges();
            }
                
            
          


            var Project = db.Projects.Find(id);
            List<ProjectDetailTemp> Lista = new List<ProjectDetailTemp> { };
            foreach (var item in Project.ProjectDetails)
            {
                var temp = new ProjectDetailTemp();
                temp.Amount = item.Amount;
                temp.Date_Amount = item.Date_Amount;
                temp.Description_Detail = item.Description_Detail;
                temp.Payed = item.Payed;
                temp.ProjectTempProject_Id = id;
              


                Lista.Add(temp);

            }
            db.ProjectDetailTemps.AddRange(Lista);

            db.SaveChanges();

            return detailTemp.ProjectDetailTemps.ToList();
        }
        /// <summary>
        /// delete Temp by its id
        /// </summary>
        /// <param name="Detailid"></param>
        /// <returns>String</returns>
        public static String DeleteFromTemp(int Detailid)

        {
            var db = new AccountingContainer();
            var Detail = db.ProjectDetailTemps.Find(Detailid);
            db.ProjectDetailTemps.Remove(Detail);
            db.SaveChanges();
            return Resources.validate.DeleteSuccessful;


        }
     
        
        /// <summary>
        /// get payment method List for dropdownList
        /// </summary>
        /// <returns>ProjectVM</returns>
        public static ProjectVM GetPaymentList()
        {
            var db = new AccountingContainer();
            ProjectVM vm = new ProjectVM();
            vm.MyPaymentPolicy = new SelectList(db.PaymentMethods, "PaymentMethod_Id", "Payment_Type");
            return vm;
        }
        ////// From Here  start  of  ProjectInvoice make serial unique for each invoice/////////
        public static bool containCodeofInv(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.ProjectInvoiceMasters.ToList();
            foreach (var item in allinData)
            {
                if (item.Serial_Number == Serial)
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }
        /// <summary>
        /// get All invoice of Project
        /// </summary>
        /// <returns>List<ProjectInvoiceMaster></returns>
        public static List<ProjectInvoiceMaster> GetAllInvoices()
        {
            var db = new AccountingContainer();
            var orders = db.ProjectInvoiceMasters.ToList();
            return orders;
        }/// <summary>
         /// fill for Creation invoice Project
         /// </summary>
         /// <returns>ProjectInvoiceMasterVM</returns>
        public static ProjectInvoiceMasterVM fillDataForCreateinvoice(int project_id,string user)
        {
            var db = new AccountingContainer();
            var provm = new ProjectInvoiceMasterVM();
            var lastSerial = db.ProjectInvoiceMasters.OrderByDescending(c => c.PInvoiceMaster_ID).FirstOrDefault();      
            if (lastSerial == null)
            {
                provm.Serial_Number = "PINV-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                provm.Serial_Number = "PINV-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(5, lastSerial.Serial_Number.Length - 5)) + 1).ToString("D3");
            }
            provm.ProjectList = new SelectList(db.Projects.Where(a=>a.Project_State!=ProjectState.Finished), "Project_Id", "Project_Name");
            provm.EmpList = new SelectList(db.Employees.Where(a=>a.Reg_ID!=null).ToList(), "Emp_Id", "Emp_Name");
            var tempmaster = new ProjectInvoiceMasterTemp();
            //var thatlast = db.ProjectInvoiceMasters.ToList().LastOrDefault();
            provm.invoicedate = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
            var transtemp = new ProjectInvoiceMasterTemp();
            var Emp = db.Employees.Where(a => a.Reg_ID == user).FirstOrDefault();
            if (Emp != null)
            {
                provm.Emp_Id = Emp.Emp_Id;
            }
            //var id = (from m in db.ProjectInvoiceMasterTemps
            //          select m).ToList().LastOrDefault();
            //if (id == null)
            //{
            //    transtemp.master_ID = 1;

            //}
            //else
            //{
            //    if (id.master_ID == null)
            //    {
            //        transtemp.master_ID = 1;
            //    }
            //    else
            //    {
            //        transtemp.master_ID = id.master_ID + 1;
            //    }

            //}

            //db.ProjectInvoiceMasterTemps.Add(transtemp);
            //db.SaveChanges();
            //provm.PInvoiceMaster_ID = transtemp.master_ID.Value;


        
            var id = (from s in db.ProjectInvoiceMasterTemps
                      select s).ToList().LastOrDefault();
            if (id == null)
            { transtemp.master_ID = 1; }
            else
            { transtemp.Id = id.Id + 1; }
           
           
            db.ProjectInvoiceMasterTemps.Add(transtemp);
            db.SaveChanges();
            provm.PInvoiceMaster_ID = transtemp.Id;

            return provm;

        }
        /// <summary>
        /// that method to Prepare detail to fill
        /// </summary>
        /// <returns>ProjectInvoiceDetailsVM</returns>
        public static ProjectInvoiceDetailsVM getPartialforDetail()
        {
            var db = new AccountingContainer();
            ProjectInvoiceDetailsVM pvm = new ProjectInvoiceDetailsVM();
            var thischartaxParent = db.ChartOfAccountings.Find(48);
            pvm.tasList = new SelectList(db.Taxes.Where(e=>e.Tax_Code.Contains(thischartaxParent.Account_Code)).Where(a => a.IsActive != false).ToList(), "Tax_ID", "Tax_Name");
            pvm.Product_Quantity = 1;
            return pvm;
        }
        /// <summary>
        /// get invoice dtail by master id
        /// </summary>
        /// <param name="detailId"></param>
        /// <returns></returns>
        public static ProjectInvoiceMaster GetProjectInvoice(int detailId)
        {
            var db = new AccountingContainer();
            var detail = db.ProjectInvoiceMasters.Find(detailId);
            return detail;

        }
        /// <summary>
        /// add to List of detail temp and check the validation
        /// </summary>
        /// <param name="PVM"></param>
        /// <returns>string</returns>
        public static string addDetailstoTemp(ProjectInvoiceDetailsVM PVM)
        {
            var db = new AccountingContainer();
            ProjectInvoiceDetailTemp prg = new ProjectInvoiceDetailTemp();
            var lastmastertemp = db.ProjectInvoiceMasterTemps.Find(PVM.ProjectInvoiceMasterPInvoiceMaster_ID);
            //if (PVM.Product_Quantity <= 0)
            //{
            //    return Resources.InvoiceMasterAddNewInvoice.Quantity;
            //}
            //if (PVM.Product_Price <= 0)
            //{
            //    return Resources.validate.PriceMust;
            //}
            if (PVM.Product_Name == null)
            {
                return Resources.validate.ChooseProduct;
            }
            prg.Product_Quantity = PVM.Product_Quantity;
            prg.Product_Price = PVM.Product_Price;
            prg.Product_Name = PVM.Product_Name;   
         
            
           
       
            if (PVM.Discount > 0)
            {
                decimal discount = 0;

                discount = (PVM.Product_Price * PVM.Product_Quantity) * (PVM.Discount / 100);



                decimal TotalAfterDiscount = (PVM.Product_Price * PVM.Product_Quantity) - discount;
                if (PVM.Tax_ID != 0)
                {
                    prg.Tax_ID = PVM.Tax_ID;
                    var thistax = db.Taxes.Find(prg.Tax_ID);
                    var taxamount = Convert.ToDecimal(thistax.Tax_Percentage) / 100;
                    prg.Detail_Total_Tax = (TotalAfterDiscount) * taxamount;
                }
                prg.Total_Amount = TotalAfterDiscount + prg.Detail_Total_Tax;
            }
            else
            {
                if (PVM.Tax_ID != 0)
                {
                    prg.Tax_ID = PVM.Tax_ID;
                    var thistax = db.Taxes.Find(prg.Tax_ID);
                    var taxamount = Convert.ToDecimal(thistax.Tax_Percentage) / 100;
                    prg.Detail_Total_Tax = (prg.Product_Price * prg.Product_Quantity) * taxamount;
                }
                prg.Total_Amount = (prg.Product_Price * prg.Product_Quantity) + prg.Detail_Total_Tax;
            }

         
            prg.ProjectInvoiceMasterTempId = lastmastertemp.Id;
            lastmastertemp.Project_Id = PVM.ProjectInvoiceMasterPInvoiceMaster_ID;
            db.Entry(lastmastertemp).State = EntityState.Modified;

            db.ProjectInvoiceDetailTemps.Add(prg);
            try
            { db.SaveChanges(); }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        var x = validationError.PropertyName;
                        var y = validationError.ErrorMessage;
                    }
                }
            }
            db.Dispose();
            return Resources.validate.SuccessfullAdd;

        }
        /// <summary>
        /// get all project innvice dtail temp by its id and change its by discount
        /// </summary>
        /// <param name="Discount"></param>
        /// <returns>List<ProjectInvoiceDetailTemp></returns>
        public static List<ProjectInvoiceDetailTemp> getListofDetailTemp(int Discount)
        {
            var db = new AccountingContainer();
            var alldata = db.ProjectInvoiceDetailTemps.Where(a=>a.ProjectInvoiceMasterTempId==Discount).ToList();
            //if (alldata.Count > 0)
            //{
            //    foreach (var item in alldata)
            //    {
            //        var TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
            //        decimal discount = 0;
            //        if (Discount > 0)
            //        {

            //            discount = (item.Product_Price * item.Product_Quantity) * (Discount / 100);



            //            decimal TotalAfterDiscount = (item.Product_Price * item.Product_Quantity) - discount;

            //            if (item.Tax_ID >0)
            //            {

            //                var thistax = db.Taxes.Find(item.Tax_ID);
            //                var taxamount = Convert.ToDecimal(thistax.Tax_Percentage) / 100;
            //                item.Detail_Total_Tax = (TotalAfterDiscount) * taxamount;
            //            }
            //            item.Total_Amount = TotalAfterDiscount + item.Detail_Total_Tax;
            //        }
            //        else
            //        {
            //            item.Total_Amount = (item.Product_Price * item.Product_Quantity) + item.Detail_Total_Tax;
            //        }

            //        db.Entry(item).State = EntityState.Modified;
            //    }
            //    db.SaveChanges();
            //}
            return alldata;
        }

        public static string changeDetailOfInvoice(decimal Discount, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.ProjectInvoiceMasterTemps.Find(Invoice_Id);
            var detail = db.ProjectInvoiceDetailTemps.Where(a => a.ProjectInvoiceMasterTempId == Invoice_Id).ToList();
           
            if (detail.Count > 0)
            {
                foreach (var item in detail)
                {
                    var TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    decimal discount = 0;
                    if (Discount > 0)
                    {

                        discount = (item.Product_Price * item.Product_Quantity) * (Discount / 100);



                        decimal TotalAfterDiscount = (item.Product_Price * item.Product_Quantity) - discount;

                        if (item.Tax_ID > 0)
                        {

                            var thistax = db.Taxes.Find(item.Tax_ID);
                            var taxamount = Convert.ToDecimal(thistax.Tax_Percentage) / 100;
                            item.Detail_Total_Tax = (TotalAfterDiscount) * taxamount;
                        }
                        item.Total_Amount = TotalAfterDiscount + item.Detail_Total_Tax;
                    }
                    else
                    {
                        item.Total_Amount = (item.Product_Price * item.Product_Quantity) + item.Detail_Total_Tax;
                    }

                    db.Entry(item).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        
            return "تم تعديل الخصم بنجاح";

        }
        /// <summary>
        /// save the invoice of project in data base
        /// </summary>
        /// <param name="pvm"></param>
        /// <returns></returns>
        public static  int SaveAllData(ProjectInvoiceMasterVM pvm)
        {
            var db = new AccountingContainer();
            ProjectInvoiceMaster PIM = new ProjectInvoiceMaster();
            PIM.Serial_Number = pvm.Serial_Number;
            PIM.Project_Id = pvm.Project_Id;
         
            PIM.Project_State = TransactionState.Pending;
            PIM.MasterDiscount = pvm.MasterDiscount;
            var thisProject = db.Projects.Find(pvm.Project_Id);
            if (pvm.FixedDiscount > 0)
            {
                PIM.FixedDiscount = (decimal)pvm.FixedDiscount;
            }
            else
            {
                PIM.FixedDiscount = 0;
            }
           
            var temp = db.ProjectInvoiceMasterTemps.Find(pvm.PInvoiceMaster_ID);
            if (pvm != null)
            {
                var lastSerial = db.ProjectInvoiceMasters.OrderByDescending(c => c.PInvoiceMaster_ID).FirstOrDefault();
                if (lastSerial == null)
                {
                    PIM.Serial_Number = "PINV-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    PIM.Serial_Number = "PINV-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(5, lastSerial.Serial_Number.Length - 5)) + 1).ToString("D3");
                }
              
                PIM.Project_Name = thisProject.Project_Name;
                PIM.Project_Description = thisProject.Project_Description;
                if (pvm.Invoice_Description == null)
                { PIM.Invoice_Description = " "; }
                else
                { PIM.Invoice_Description = pvm.Invoice_Description; }

                PIM.Project_Location = thisProject.Project_Location;
                if (pvm.DeadLine == null)
                { PIM.DeadLine = null; }
                else
                { PIM.DeadLine = Convert.ToDateTime(pvm.DeadLine); }



             

                PIM.StartTime = thisProject.StartTime;
                PIM.ProjectInv_Date = Convert.ToDateTime(pvm.invoicedate).Date;
                PIM.Estimate_Amount = thisProject.Estimate_Amount;
                PIM.CustomerCustomer_Id = thisProject.CustomerCustomer_Id;
                PIM.Customer_Name = db.Customers.Find(thisProject.CustomerCustomer_Id).Customer_Name;
                PIM.Emp_Id = pvm.Emp_Id;
                PIM.Payment_Method = pvm.Payment_Method;
                PIM.Payment_Method_Name = pvm.Payment_Method.ToString();
                PIM.Revenu_Id = pvm.Revenu_Id;
                PIM.Project_Code = db.Projects.Find(PIM.Project_Id).Project_Code; ;
                //PIM.Payment_Method_Name = db.PaymentMethods.Find(PIM.Payment_Method).Payment_Type;
                PIM.Project_Location = "-";
               



                db.ProjectInvoiceMasters.Add(PIM);
            }
            

            try
            { db.SaveChanges(); }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        var x = validationError.PropertyName;
                        var y = validationError.ErrorMessage;
                    }
                }
            }
            var tempmaster = db.ProjectInvoiceMasterTemps.Find(pvm.PInvoiceMaster_ID);
            //var thisdetail = db.ProjectInvoiceDetailTemps.Where(s => s.ProjectInvoiceMasterTempId == tempmaster.Id);
            var thisdetail = tempmaster.ProjectInvoiceDetailTemps.ToList();

            var qrMaster = new MasterQRInvoice();
            qrMaster.QrGuid = Guid.NewGuid();
            qrMaster.DetailQRInvoices = new List<DetailQRInvoice>();
            decimal totalAmount = 0;
            decimal totalBeforDiscount = 0;
            decimal discount = 0;
            decimal totaltax = 0;


            foreach (var item in thisdetail)
            {
                ProjectInvoiceDetails DIM = new ProjectInvoiceDetails();
                DIM.Product_Name = item.Product_Name;
                DIM.Product_Price = item.Product_Price;
                if (item.Tax_ID == null)
                {

                }
                else
                {
                    DIM.Tax_ID = item.Tax_ID;
                }
               
                DIM.Detail_Total_Tax = item.Detail_Total_Tax;
                DIM.Product_Quantity = item.Product_Quantity;
                DIM.ProjectInvoiceMasterPInvoiceMaster_ID = PIM.PInvoiceMaster_ID;
                DIM.Total_Amount = item.Total_Amount;
                db.ProjectInvoiceDetails.Add(DIM);


              var invoiceQRDetail = new DetailQRInvoice();
                invoiceQRDetail.Batch_ID = 0;
                invoiceQRDetail.Product_Id = 0;
                invoiceQRDetail.Product_Quantity = item.Product_Quantity;
                if (item.Tax_ID != null)
                {
                    invoiceQRDetail.Product_Tax = item.Tax_ID.Value;
                }


                invoiceQRDetail.Detail_Total_Cost = 0;

                invoiceQRDetail.Total_Amount = item.Total_Amount;
                invoiceQRDetail.ProductProJ_Name = item.Product_Name;
                invoiceQRDetail.Product_Price = item.Product_Price;
                invoiceQRDetail.Detail_Total_Tax = item.Detail_Total_Tax;
                invoiceQRDetail.ProductPriceCost = 0;
                qrMaster.DetailQRInvoices.Add(invoiceQRDetail);

                totalBeforDiscount += item.Product_Quantity * item.Product_Price;
                totalAmount += invoiceQRDetail.Total_Amount;
                totaltax += item.Detail_Total_Tax;
                discount += totalBeforDiscount * (PIM.MasterDiscount.Value / 100);
            }
            /// qr Invoice
            // /
          
            qrMaster.AllowDiscount = PIM.FixedDiscount.Value;
            qrMaster.ApprovalCode = "";
            qrMaster.Company_Id = db.Companies.ToList().FirstOrDefault().Comp_Id;
            qrMaster.Customer_Id = PIM.CustomerCustomer_Id;
            qrMaster.Description = PIM.Invoice_Description;
            qrMaster.Employee_Id = PIM.Emp_Id.Value;
            qrMaster.Invoice_Date = PIM.ProjectInv_Date;
            qrMaster.Invoice_State = PIM.Project_State;
            qrMaster.Branch_Id = thisProject.BranchSubledger_Id;
            qrMaster.InvoiceType_Id = 6;
            qrMaster.Master_Discount = PIM.MasterDiscount.Value;
            qrMaster.TotalBeforeDiscount = PIM.Estimate_Amount;
            qrMaster.Reference = "";
            qrMaster.Serial_Number = PIM.Serial_Number;

            qrMaster.Subledger_Id = 0;
            qrMaster.Notes = "";

            qrMaster.TotalNewTaxs =0;


            qrMaster.Total_Amount = totalAmount;

            qrMaster.Total_Tax = totaltax;
            qrMaster.TotalBeforeDiscount = totalBeforDiscount;
        


            qrMaster.InvoiceType = "فاتورة مبيعات مشروع";
            qrMaster.Total_Payment_Method_Cost = 0;
         
          
           
           
            qrMaster.StartDate = PIM.StartTime;
            qrMaster.EndDate = PIM.DeadLine;

            qrMaster.Total_Cost = 0;
            qrMaster.Notes = "";
            qrMaster.Project_Id = PIM.Project_Id;
            qrMaster.Number = InvoiceSerial.generateSerialForInvoice();
            qrMaster.DateOfCreate = DateTime.Now;
            PIM.Number = qrMaster.Number;

            db.MasterQRInvoices.Add(qrMaster);
            db.SaveChanges();
            PIM.QRNumber = qrMaster.Id;
            db.Entry(PIM).State = EntityState.Modified;
        

         
            db.ProjectInvoiceDetailTemps.RemoveRange(thisdetail);
            db.ProjectInvoiceMasterTemps.Remove(tempmaster);
            db.SaveChanges();
            db.Dispose();
            return PIM.PInvoiceMaster_ID;
        }
        /// <summary>
        /// make gl for Project invoice 
        /// </summary>
        /// <param name="PVM"></param>
        /// <returns>String</returns>
        public static String UpdateProjectInvoiceMaster(ProjectInvoiceMasterVM PVM)
        {
            var msg = "";
            var db = new AccountingContainer();

            ProjectInvoiceMaster invoice = db.ProjectInvoiceMasters.Find(PVM.PInvoiceMaster_ID);
            OperationRun.Addnewoperation(invoice.Project_Code);
            var operation = db.OperationRuns.Where(a => a.Code == invoice.Project_Code).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(invoice.Project_Code);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }
            if (invoice.Project_State == PVM.Project_State)
            {
                OperationRun.DeleteOperation(invoice.Project_Code);
              
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + invoice.Project_State + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            else
            {
                invoice.Project_State = PVM.Project_State;
                decimal totalamount = 0;
                decimal tax = 0;
                decimal amount = 0;
                decimal Mdiscount = 0;
                decimal FDescount = 0;
                decimal CAmount = 0;
                decimal DAmount = 0;
                decimal totalnotnet = 0;
                foreach (var item in invoice.ProjectInvoiceDetails)
                {
                    totalamount += item.Total_Amount;
                    tax += item.Detail_Total_Tax;
                    amount += item.Total_Amount - item.Detail_Total_Tax;
                    if (invoice.MasterDiscount > 0)
                    {
                        var discount = (item.Product_Price * item.Product_Quantity) * (invoice.MasterDiscount / 100);


                        CAmount += item.Product_Quantity * item.Product_Price;

                        Mdiscount += (decimal)discount;
                        totalnotnet += item.Product_Price * item.Product_Quantity;
                        DAmount = (totalnotnet + tax) - Mdiscount;


                    }

                }
                if (invoice.FixedDiscount > 0)
                {
                    FDescount = (decimal)invoice.FixedDiscount;
                    DAmount = DAmount - FDescount;

                }
                #region Create GL
                // Add GLHeader 
                GLMaster glHeader = new GLMaster();

                glHeader.GL_Date = invoice.ProjectInv_Date.Value;
                glHeader.GL_Desc = invoice.Invoice_Description;
                glHeader.Company_Id = db.Companies.ToList().FirstOrDefault().Comp_Id;
                glHeader.GLType_Id = "مبيعات مشروع";
                glHeader.Currency_Id = "الريال السعودي";
                // Initialize GLState to Pending when first created
                glHeader.GLState = invoice.Project_State;
                glHeader.GLCode = invoice.Serial_Number;
                glHeader.Auto = true;
                // Create five GlDetail
                GLDetail AccountsPayable = new GLDetail(),
                   RevenueGL = new GLDetail(),
                   TaxGL = new GLDetail(),
                   MasterDiscountGL = new GLDetail(),
                   FixedDiscountGL = new GLDetail();



                // Initialize common values
                // TODO: Fix the following values
                //  var BusId = db.BussinesUnits.FirstOrDefault().BU_Id;
                AccountsPayable.Bu_Id = 0;
                RevenueGL.Bu_Id = 0;
                TaxGL.Bu_Id = 0;
                MasterDiscountGL.Bu_Id = 0;
                FixedDiscountGL.Bu_Id = 0;


                var ChartOfAccounPay = db.ChartOfAccountings.Find(37);
                AccountsPayable.Account_Id = ChartOfAccounPay.Account_Id;
                AccountsPayable.Account_Name = ChartOfAccounPay.Account_Name;

                var MDisChartOfAccounPay = db.ChartOfAccountings.Find(82);
                MasterDiscountGL.Account_Id = MDisChartOfAccounPay.Account_Id;
                MasterDiscountGL.Account_Name = MDisChartOfAccounPay.Account_Name;

                var FixedDisChartOfAccounPay = db.ChartOfAccountings.Where(a=>a.Account_Code== "040102004").FirstOrDefault() ;
                FixedDiscountGL.Account_Id = FixedDisChartOfAccounPay.Account_Id;
                FixedDiscountGL.Account_Name = FixedDisChartOfAccounPay.Account_Name;

                var ChartOfRevenue = db.ChartOfAccountings.Find(invoice.Revenu_Id);
                RevenueGL.Account_Id = ChartOfRevenue.Account_Id;
                RevenueGL.Account_Name = ChartOfRevenue.Account_Name;

                
                var details = invoice.ProjectInvoiceDetails;
                foreach (var taxId in details)
                {
                    if (taxId.Tax_ID != null)
                    {
                        var taxDet = db.Taxes.Find(taxId.Tax_ID);
                        var ChartOfAccounTax = db.ChartOfAccountings.Where(e => e.Account_Code == taxDet.Tax_Code).ToList().FirstOrDefault();
                        TaxGL.Account_Id = ChartOfAccounTax.Account_Id;
                        TaxGL.Account_Name = ChartOfAccounTax.Account_Name;
                    }
                }

                AccountsPayable.Asset_Id =
                     TaxGL.Asset_Id =
                RevenueGL.Asset_Id = 0;

                var cus = db.Customers.Find(invoice.CustomerCustomer_Id);
                SubLedger supplSubledger = db.SubLedgers.Where(subled => subled.SubLed_Name == invoice.Project_Name).ToList().LastOrDefault();
                Accounts accountsCHild = db.Accounts.Where(subled => subled.Account_Code == cus.Customer_Code).FirstOrDefault();
                AccountsPayable.Account_Child_ID = accountsCHild.Accounts_Id;
                AccountsPayable.Account_CHild_Name = accountsCHild.Account_Name;
                RevenueGL.SubLedger_Id = supplSubledger.SubLed_Id;
                FixedDiscountGL.SubLedger_Id = supplSubledger.SubLed_Id;

                MasterDiscountGL.SubLedger_Id = supplSubledger.SubLed_Id;

                //db.StoreProducts.Load();
                switch (invoice.Project_State)
                {
                    //Post Invoice
                    //Creates Automatic GLs
                    case TransactionState.Post:
                        glHeader.GLState = TransactionState.Post;
                        var gl = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                        if (gl != null)
                        {
                            OperationRun.DeleteOperation(invoice.Project_Code);
                            return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                        }


                        if (invoice.MasterDiscount > 0)
                        {
                            if (invoice.FixedDiscount > 0)
                            {
                                FixedDiscountGL.GLDetail_Debit = FDescount;
                                glHeader.GLDetails.Add(FixedDiscountGL);
                            }
                            MasterDiscountGL.GLDetail_Debit = Mdiscount;
                            AccountsPayable.GLDetail_Debit = DAmount;
                            RevenueGL.GLDetail_Credit = CAmount;
                            if (tax != 0)
                            {
                                TaxGL.GLDetail_Credit = tax;
                                glHeader.GLDetails.Add(TaxGL);
                            }
                            glHeader.GLDetails.Add(MasterDiscountGL);
                            glHeader.GLDetails.Add(AccountsPayable);
                            glHeader.GLDetails.Add(RevenueGL);

                        }
                        else
                        {

                            RevenueGL.GLDetail_Credit = amount;
                            if (tax != 0)
                            {
                                TaxGL.GLDetail_Credit = tax;
                                glHeader.GLDetails.Add(TaxGL);
                            }
                            if (invoice.FixedDiscount > 0)
                            {
                                AccountsPayable.GLDetail_Debit = totalamount - (decimal)invoice.FixedDiscount;
                                FixedDiscountGL.GLDetail_Debit = FDescount;
                                glHeader.GLDetails.Add(FixedDiscountGL);
                            }
                            else
                            {
                                AccountsPayable.GLDetail_Debit = totalamount;
                            }


                            glHeader.GLDetails.Add(AccountsPayable);
                            glHeader.GLDetails.Add(RevenueGL);
                        }


                        break;
                    // Void Invoice
                    //Creates Automatic GLs (same as GLs Created When Posted but Reversed)
                    case TransactionState.Void:
                        glHeader.GLType_Id = "مبيعات مشروع عكسي";
                        glHeader.GLState = TransactionState.Void;
                        var glcheck = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                        if (glcheck != null)
                        {
                            OperationRun.DeleteOperation(invoice.Project_Code);
                            return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                        }
                        if (invoice.HasOff == true)
                        {
                            msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.UnderCloseing;
                            invoice.Project_State = TransactionState.Post;
                            db.SaveChanges();
                            OperationRun.DeleteOperation(invoice.Project_Code);
                            return msg;
                        }
                        else
                        {

                            if (invoice.MasterDiscount > 0)
                            {
                                if (invoice.FixedDiscount > 0)
                                {
                                    FixedDiscountGL.GLDetail_Credit = FDescount;
                                    glHeader.GLDetails.Add(FixedDiscountGL);
                                }
                                MasterDiscountGL.GLDetail_Credit = Mdiscount;
                                AccountsPayable.GLDetail_Credit = DAmount;
                                RevenueGL.GLDetail_Debit = CAmount;
                                if (tax != 0)
                                {
                                    TaxGL.GLDetail_Debit = tax;
                                    glHeader.GLDetails.Add(TaxGL);
                                }
                                glHeader.GLDetails.Add(MasterDiscountGL);
                                glHeader.GLDetails.Add(AccountsPayable);
                                glHeader.GLDetails.Add(RevenueGL);

                            }
                            else
                            {

                                RevenueGL.GLDetail_Debit = amount;
                                if (tax != 0)
                                {
                                    TaxGL.GLDetail_Debit = tax;
                                    glHeader.GLDetails.Add(TaxGL);
                                }
                                if (invoice.FixedDiscount > 0)
                                {
                                    AccountsPayable.GLDetail_Credit = totalamount - (decimal)invoice.FixedDiscount;
                                    FixedDiscountGL.GLDetail_Credit = FDescount;
                                    glHeader.GLDetails.Add(FixedDiscountGL);
                                }
                                else
                                {
                                    AccountsPayable.GLDetail_Credit = totalamount;
                                }


                                glHeader.GLDetails.Add(AccountsPayable);
                                glHeader.GLDetails.Add(RevenueGL);
                            }
                            // Remove all batches related to this invoice
                        }
                        break;
                    default:
                        break;
                }
                if (msg == "")
                {
                    #endregion
                    glHeader.GLDetails.Add(AccountsPayable);
                    if (tax != 0)
                    {
                        glHeader.GLDetails.Add(TaxGL);
                    }
                    glHeader.GLDetails.Add(RevenueGL);
                    db.GLMasters.Add(glHeader);
                    GLMaster.GetBalanceForAccount(glHeader);
                    GLMaster.GetBalanceForSubLedger(glHeader);
                    db.SaveChanges();
                    msg = "تم عمل قيد الفاتورة بنجاح";
                }
                OperationRun.DeleteOperation(invoice.Project_Code);
              
                if (invoice.Project_State == TransactionState.Post)
                {
                    msg = Resources.validate.GLDone;
                }
                else
                {
                    msg = Resources.validate.GLReverse;
                }
                return msg;
            }
           
        }
        /// <summary>
        /// get all depart ment depend on that branch
        /// </summary>
        /// <param name="Branch_Id"></param>
        /// <returns> IEnumerable<SelectListItem> </returns>
        public static IEnumerable<SelectListItem> GetDepartment(int Branch_Id)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> Department = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == Branch_Id && p.Subled_Level == 2), "SubLed_Id", "SubLed_Name");
            
            return Department;
        }
        /// <summary>
        /// get all bussiness by department_Id 
        /// </summary>
        /// <param name="Department_Id"></param>
        /// <returns> IEnumerable<SelectListItem> </returns>
        public static IEnumerable<SelectListItem> GetBussines(int Department_Id)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> Bussines = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == Department_Id), "SubLed_Id", "SubLed_Name");
           
            return Bussines;
        }
        /// <summary>
        /// delete invoice temp project from dnb  
        /// </summary>
        /// <param name="purch"></param>
        /// <returns>ProjectInvoiceDetailTemp</returns>
        public static ProjectInvoiceDetailTemp DeleteInvoiceFromTemp(int purch)
        {
            var db = new AccountingContainer();
            ProjectInvoiceDetailTemp vm = db.ProjectInvoiceDetailTemps.Find(purch);
            db.ProjectInvoiceDetailTemps.Remove(vm);
            db.SaveChanges();
            db.Dispose();
            return vm;
        }
        /// <summary>
        /// delete Project invoice from data base
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns>String</returns>
        public static String DeleteProjectInvoiceMaster(int invoiceId)
        {
            var db = new AccountingContainer();
            ProjectInvoiceMaster invoice = db.ProjectInvoiceMasters.Find(invoiceId);

            if (invoice.Project_State != TransactionState.Pending)
                return "الفاتورة  فى حالة التأثير فى الحسابات لايمكنك الحذف";
            db.ProjectInvoiceDetails.RemoveRange(invoice.ProjectInvoiceDetails);
            db.ProjectInvoiceMasters.Remove(invoice);

            db.SaveChanges();
            db.Dispose();
            return "تم الحذف بنجاح";
        }
        /// <summary>
        /// get all data for that Project
        /// </summary>
        /// <param name="pro"></param>
        /// <returns>dynamic</returns>
        public static dynamic GetPurchInvoiceJsonModel(Project pro)
        {
            var db = new AccountingContainer();
            Customer curr = new Customer();
            if (pro.CustomerCustomer_Id > 0)
            {
                 curr = db.Customers.Find(pro.CustomerCustomer_Id);
            }
        
            var deadline = pro.DeadLine.ToString();
            var starttime = pro.StartTime.ToString();
            PaymentMethod paM = new PaymentMethod();
            if (pro.Payment_Method > 0)
            {
                paM = db.PaymentMethods.Find(pro.Payment_Method);
            }
           
            var xx = new
            {
                pro.Project_Code,
                pro.Project_Name,
                curr.Customer_Name,
                pro.Project_Description,
                pro.Project_Location,
                deadline,
                pro.StartTime,
                pro.Estimate_Amount,
                pro.CustomerCustomer_Id,
                pro.Emp_Id,
                starttime,
                pro.Payment_Method,
                pro.Revenu_Id,
                pro.Project_State,
                paM.Payment_Type,
            };
            return xx;
        }
        /// <summary>
        /// make Edit for invoice of Project in pendding status
        /// </summary>
        /// <param name="projMasID"></param>
        /// <returns></returns>

        public static ProjectInvoiceMasterVM EditProjInvBeforPostforCreation(int projMasID)
        {
            var db = new AccountingContainer();
            var thisprojinv = db.ProjectInvoiceMasters.Find(projMasID);
            ProjectInvoiceMasterVM PINVM = new ProjectInvoiceMasterVM();
            PINVM.EmpList = new SelectList(db.Employees.Where(a => a.Reg_ID != null).ToList(), "Emp_Id", "Emp_Name");
            PINVM.PInvoiceMaster_ID = thisprojinv.PInvoiceMaster_ID;
            PINVM.CustomerCustomer_Id = thisprojinv.CustomerCustomer_Id;
            PINVM.Payment_Method = thisprojinv.Payment_Method;
            PINVM.Revenu_Id = thisprojinv.Revenu_Id;
            PINVM.Project_State = thisprojinv.Project_State;
            PINVM.Project_Id = thisprojinv.Project_Id;
            PINVM.Serial_Number = thisprojinv.Serial_Number;
            PINVM.Project_Name = thisprojinv.Project_Name;
            PINVM.Invoice_Description = thisprojinv.Invoice_Description;
            PINVM.Project_Description = thisprojinv.Project_Description;
            PINVM.StartTime = thisprojinv.StartTime.Day+"-" + thisprojinv.StartTime.Month+"-"+ thisprojinv.StartTime.Year;
            PINVM.invoicedate = Convert.ToDateTime(thisprojinv.ProjectInv_Date).Day + "-" + Convert.ToDateTime(thisprojinv.ProjectInv_Date).Month + "-" + Convert.ToDateTime(thisprojinv.ProjectInv_Date).Year;
            PINVM.Estimate_Amount = thisprojinv.Estimate_Amount;
            PINVM.Emp_Id = thisprojinv.Emp_Id;
            PINVM.Customer_Name = thisprojinv.Customer_Name;
            PINVM.FixedDiscount = thisprojinv.FixedDiscount;
            PINVM.MasterDiscount = (decimal)thisprojinv.MasterDiscount;
            PINVM.Project_Code = thisprojinv.Project_Code;
            var ThereORN = db.ProjectInvoiceMasterTemps.Find(projMasID);
            var ProTemp = new ProjectInvoiceMasterTemp();
            if (ThereORN != null)
            {

            }
            else
            {

                ProTemp.Id = projMasID;

                db.ProjectInvoiceMasterTemps.Add(ProTemp);
                db.SaveChanges();
            }

          
            return PINVM;
        }
        /// <summary>
        /// get the list of inoice temp by master id
        /// </summary>
        /// <param name="masterID"></param>
        /// <returns> List<ProjectInvoiceDetailTemp> </returns>
        public static List<ProjectInvoiceDetailTemp> getListofDetailTempfirstLoad(int  masterID)
        {
            var db = new AccountingContainer();
            var alltemp = db.ProjectInvoiceDetailTemps.Where(a=>a.ProjectInvoiceMasterTempId== masterID).ToList();
            var alldata = db.ProjectInvoiceDetailTemps.RemoveRange(alltemp);
            db.SaveChanges();
            var nastertemp = db.ProjectInvoiceMasterTemps.Where(e => e.Id == masterID).ToList().FirstOrDefault();

            var projInvDetails = db.ProjectInvoiceDetails.Where(e => e.ProjectInvoiceMasterPInvoiceMaster_ID == masterID).ToList();
            List<ProjectInvoiceDetailTemp> Lisa = new List<ProjectInvoiceDetailTemp>();
            if (projInvDetails.Count > 0)
            {
              
                foreach (var item in projInvDetails)
                {
                    ProjectInvoiceDetailTemp PINT = new ProjectInvoiceDetailTemp();
                
                    PINT.Product_Name = item.Product_Name;
                    PINT.Product_Price = item.Product_Price;
                    PINT.Tax_ID = item.Tax_ID;
                    PINT.Detail_Total_Tax = item.Detail_Total_Tax;
                    PINT.Total_Amount = item.Total_Amount;
                    PINT.Product_Quantity = item.Product_Quantity;
                    PINT.ProjectInvoiceMasterTempId = item.ProjectInvoiceMasterPInvoiceMaster_ID;
                    PINT.ProjectInvoiceMasterTempId = item.ProjectInvoiceMasterPInvoiceMaster_ID;
                    PINT.fixeddiscount = projInvDetails.FirstOrDefault().ProjectInvoiceMaster.FixedDiscount;
                    PINT.MasterID = masterID;
                    Lisa.Add(PINT);
                   

                }
            }
            db.ProjectInvoiceDetailTemps.AddRange(Lisa);
            db.SaveChanges();
            var newtemp = db.ProjectInvoiceDetailTemps.Where(a=>a.ProjectInvoiceMasterTempId==masterID).ToList();
            return newtemp;
        }
        /// <summary>
        /// edit in project invoice master and save it in data base
        /// </summary>
        /// <param name="pvm"></param>
        /// <returns>ProjectInvoiceMasterVM</returns>

        public static int EditDataofInv(ProjectInvoiceMasterVM pvm)
        {
            var db = new AccountingContainer();
            ProjectInvoiceMaster PIM = db.ProjectInvoiceMasters.Find(pvm.PInvoiceMaster_ID);
          
            PIM.Project_Id = pvm.Project_Id;
            var temp = db.ProjectInvoiceMasterTemps.Find(pvm.PInvoiceMaster_ID);
            if (pvm != null)
            {
               
               
               var proj=  db.Projects.Find(PIM.Project_Id);
                PIM.Project_Name = proj.Project_Name;
                PIM.Project_Description = proj.Project_Description;
                if (pvm.Invoice_Description == null)
                { PIM.Invoice_Description = " "; }
                else
                { PIM.Invoice_Description = pvm.Invoice_Description; }

                PIM.Project_Location = proj.Project_Location;
                if (proj.DeadLine == null)
                { PIM.DeadLine = null; }
                else
                { PIM.DeadLine = Convert.ToDateTime(proj.DeadLine); }



              

                PIM.StartTime = Convert.ToDateTime(proj.StartTime).Date;
                PIM.ProjectInv_Date = Convert.ToDateTime(pvm.invoicedate).Date;
              
              
               
     
                PIM.MasterDiscount = pvm.MasterDiscount;
                PIM.FixedDiscount = pvm.FixedDiscount;
                PIM.Revenu_Id = pvm.Revenu_Id;
             

                PIM.Project_State = pvm.Project_State;

            
                db.Entry(PIM).State = EntityState.Modified;
            }


            try
            { db.SaveChanges(); }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        var x = validationError.PropertyName;
                        var y = validationError.ErrorMessage;
                    }
                }
            }
           // var tempmaster = db.ProjectInvoiceMasterTemps.Where(e => e.Project_Id == PIM.Project_Id).ToList().FirstOrDefault();
            var thisdetail = db.ProjectInvoiceDetailTemps.Where(s => s.ProjectInvoiceMasterTempId == pvm.PInvoiceMaster_ID).ToList();
            var thisdetials = db.ProjectInvoiceDetails.Where(e => e.ProjectInvoiceMasterPInvoiceMaster_ID == pvm.PInvoiceMaster_ID).ToList();
            var qrMaster = db.MasterQRInvoices.Find(PIM.QRNumber);
           
            if (qrMaster != null)
            {
                db.DetailQRInvoices.RemoveRange(qrMaster.DetailQRInvoices);
                qrMaster.DetailQRInvoices = new List<DetailQRInvoice>();
                db.SaveChanges();
            }
            decimal totalAmount = 0;
            decimal totalBeforDiscount = 0;
            decimal discount = 0;
            decimal totaltax = 0;
            db.ProjectInvoiceDetails.RemoveRange(thisdetials);
            foreach (var item in thisdetail)
            {
                ProjectInvoiceDetails DIM = new ProjectInvoiceDetails();
                DIM.Product_Name = item.Product_Name;
                DIM.Product_Price = item.Product_Price;
                if (item.Tax_ID == null)
                {

                }
                else
                {
                    DIM.Tax_ID = item.Tax_ID;
                }

                DIM.Detail_Total_Tax = item.Detail_Total_Tax;
                DIM.Product_Quantity = item.Product_Quantity;
                DIM.ProjectInvoiceMasterPInvoiceMaster_ID = PIM.PInvoiceMaster_ID;
                DIM.Total_Amount = item.Total_Amount;
                db.ProjectInvoiceDetails.Add(DIM);

                if (qrMaster != null)
                {
                    var invoiceQRDetail = new DetailQRInvoice();
                    invoiceQRDetail.Batch_ID = 0;
                    invoiceQRDetail.Product_Id = 0;
                    invoiceQRDetail.Product_Quantity = item.Product_Quantity;
                    if (item.Tax_ID != null)
                    {
                        invoiceQRDetail.Product_Tax = item.Tax_ID.Value;
                    }


                    invoiceQRDetail.Detail_Total_Cost = 0;

                    invoiceQRDetail.Total_Amount = item.Total_Amount;
                    invoiceQRDetail.ProductProJ_Name = item.Product_Name;
                    invoiceQRDetail.Product_Price = item.Product_Price;
                    invoiceQRDetail.Detail_Total_Tax = item.Detail_Total_Tax;
                    invoiceQRDetail.ProductPriceCost = 0;
                    qrMaster.DetailQRInvoices.Add(invoiceQRDetail);

                    totalBeforDiscount += item.Product_Quantity * item.Product_Price;
                    totalAmount += invoiceQRDetail.Total_Amount;
                    totaltax += item.Detail_Total_Tax;
                    discount += totalBeforDiscount * (PIM.MasterDiscount.Value / 100);
                }
               
            }
            /// qr Invoice
            // /

            qrMaster.AllowDiscount = PIM.FixedDiscount.Value;
            qrMaster.ApprovalCode = "";
            qrMaster.Company_Id = db.Companies.ToList().FirstOrDefault().Comp_Id;
            qrMaster.Customer_Id = PIM.CustomerCustomer_Id;
            qrMaster.Description = PIM.Invoice_Description;
            qrMaster.Employee_Id = PIM.Emp_Id.Value;
            qrMaster.Invoice_Date = PIM.ProjectInv_Date;
            qrMaster.Invoice_State = PIM.Project_State;
            qrMaster.InvoiceType_Id = 6;
            qrMaster.Master_Discount = PIM.MasterDiscount.Value;
            qrMaster.TotalBeforeDiscount = PIM.Estimate_Amount;
            qrMaster.Reference = "";
            qrMaster.Serial_Number = PIM.Serial_Number;

            qrMaster.Subledger_Id = 0;
            qrMaster.Notes = "";

            qrMaster.TotalNewTaxs = 0;


            qrMaster.Total_Amount = totalAmount;

            qrMaster.Total_Tax = totaltax;
            qrMaster.TotalBeforeDiscount = totalBeforDiscount;



            qrMaster.InvoiceType = "فاتورة مبيعات مشروع";
            qrMaster.Total_Payment_Method_Cost = 0;
         



            qrMaster.StartDate = PIM.StartTime;
            qrMaster.EndDate = PIM.DeadLine;

            qrMaster.Total_Cost = 0;
            qrMaster.Notes = "";
            qrMaster.Project_Id = PIM.Project_Id;


            db.Entry(qrMaster).State = EntityState.Modified;
            db.SaveChanges();
        
           


        








          
            var alltempdetail = db.ProjectInvoiceDetailTemps.ToList();
            var alltempmaster = db.ProjectInvoiceMasterTemps.ToList();
            foreach (var Mast in alltempdetail)
            {
                db.ProjectInvoiceDetailTemps.Remove(Mast);
                db.SaveChanges();
            }
            foreach (var Mast in alltempmaster)
            {
                db.ProjectInvoiceMasterTemps.Remove(Mast);
                db.SaveChanges();
            }

            db.Dispose();
            return PIM.PInvoiceMaster_ID;
        }
        /// <summary>
        /// get all detal temp for that invoice detail
        /// </summary>
        /// <param name="masterID"></param>
        /// <returns>List<ProjectInvoiceDetailTemp></returns>
        public static List<ProjectInvoiceDetailTemp> getListofDetailTempsecondLoad(int masterID)
        {
            var db = new AccountingContainer();

            var projInvDetails = db.ProjectInvoiceDetailTemps.Where(e => e.MasterID == masterID).ToList();
            return projInvDetails;
        }
        /// <summary>
        /// add detail to List for Editing 
        /// </summary>
        /// <param name="PVM"></param>
        /// <returns></returns>
        public static string addDetailstoTempEdit(ProjectInvoiceDetailsVM PVM)
        {
            var db = new AccountingContainer();
            ProjectInvoiceDetailTemp prg = new ProjectInvoiceDetailTemp();
            var lastmastertemp = db.ProjectInvoiceMasterTemps.ToList().LastOrDefault();
            if (PVM.Product_Quantity <= 0)
            {
                return "من فضلك ادخل الكمية";
            }
            if (PVM.Product_Price <= 0)
            {
                return "من فضلك ادخل السعر";
            }
            if (PVM.Product_Name == null)
            {
                return "من فضلك ادخل اسم الصنف";
            }
            prg.Product_Quantity = PVM.Product_Quantity;
            prg.Product_Price = PVM.Product_Price;
            prg.Product_Name = PVM.Product_Name;



            if (PVM.Discount != null || PVM.Discount > 0)
            {
                decimal discount = 0;
            
                    discount = (PVM.Product_Price*PVM.Product_Quantity) * (PVM.Discount / 100);
              
            

                decimal TotalAfterDiscount = (PVM.Product_Price * PVM.Product_Quantity) - discount;

                if (PVM.Tax_ID != 0)
                {
                    prg.Tax_ID = PVM.Tax_ID;
                    var thistax = db.Taxes.Find(prg.Tax_ID);
                    var taxamount = Convert.ToDecimal(thistax.Tax_Percentage) / 100;
                    prg.Detail_Total_Tax = (TotalAfterDiscount) * taxamount;
                }
                prg.Total_Amount = TotalAfterDiscount + prg.Detail_Total_Tax;
            }
            else
            {

                if (PVM.Tax_ID != 0)
                {
                    prg.Tax_ID = PVM.Tax_ID;
                    var thistax = db.Taxes.Find(prg.Tax_ID);
                    var taxamount = Convert.ToDecimal(thistax.Tax_Percentage) / 100;
                    prg.Detail_Total_Tax = (prg.Product_Price * prg.Product_Quantity) * taxamount;
                }
                prg.Total_Amount = (prg.Product_Price * prg.Product_Quantity) + prg.Detail_Total_Tax;
            }
           
            prg.ProjectInvoiceMasterTempId = lastmastertemp.Id;
            prg.MasterID = PVM.masterID;
    

            db.ProjectInvoiceDetailTemps.Add(prg);
            try
            { db.SaveChanges(); }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        var x = validationError.PropertyName;
                        var y = validationError.ErrorMessage;
                    }
                }
            }
            db.Dispose();
            return "تم الاضافة في اللائحة";

        }
        /// <summary>
        /// change the amounts of invoice depend on fixedDiscount
        /// </summary>
        /// <param name="FixedDiscount"></param>
        /// <returns>List<ProjectInvoiceDetailTemp></returns>
        public static List<ProjectInvoiceDetailTemp> calculatealltotals(decimal FixedDiscount, int masterid)
        {
            var db = new AccountingContainer();

            var thisfixed = db.ProjectInvoiceDetailTemps.Where(a=>a.ProjectInvoiceMasterTempId== masterid).ToList();
            if (thisfixed.Count > 0)
            {
                var thisfixed1 = thisfixed.FirstOrDefault();
                thisfixed1.fixeddiscount = FixedDiscount;
                db.Entry(thisfixed1).State = EntityState.Modified;
                db.SaveChanges();
            }
            return thisfixed;
        }
        /// <summary>
        /// calculate amount of Project invoice by its id 
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static decimal amountsofInvoices(int projectid)
        {
            decimal total = 0;
            var db = new AccountingContainer();
            var thisinvoices = db.ProjectInvoiceMasters.Where(e => e.Project_Id == projectid).ToList();
            if (thisinvoices.Count > 0)
            {
                foreach (var master in thisinvoices)
                {
                    var thisdetails = db.ProjectInvoiceDetails.Where(e => e.ProjectInvoiceMasterPInvoiceMaster_ID == master.PInvoiceMaster_ID).ToList();
                    foreach (var detail in thisdetails)
                    {
                        total = total + detail.Total_Amount;
                    }
                }
            }
       
            return total;
        }
        /// <summary>
        /// calculate Project After fixed discount
        /// </summary>
        /// <param name="pojectid"></param>
        /// <returns></returns>
        public static decimal amountafterpost(int pojectid)
        {
            decimal total = 0;
            var db = new AccountingContainer();
            decimal totaloftax = 0;
            decimal totalbeofrdicound = 0;
            decimal totaldiscount = 0;
            decimal discount = 0;
            decimal totalAfterDiscount = 0;
            decimal totalafterTax = 0;
            decimal fixedDiscount = 0;
            decimal totalafterfixed = 0;
            var thisinvoices = db.ProjectInvoiceMasters.Where(e => e.Project_Id == pojectid && e.Project_State==TransactionState.Post).ToList();
            if (thisinvoices.Count > 0)
            {
                foreach (var master in thisinvoices)
                {


                    totaloftax = 0;
                   totalbeofrdicound = 0;
                    totaldiscount = 0;
                    discount = 0;
                    totalAfterDiscount = 0;
                    totalafterTax = 0;
                    fixedDiscount = 0;

                    var thisdetails = db.ProjectInvoiceDetails.Where(e => e.ProjectInvoiceMasterPInvoiceMaster_ID == master.PInvoiceMaster_ID).ToList();
                    foreach (var detail in thisdetails)
                    {
                        total = total + detail.Total_Amount;
                        if (master.MasterDiscount > 0)
                        {
                            discount = (detail.Product_Quantity * detail.Product_Price) * ((decimal)master.MasterDiscount / 100);
                        }
                        else
                        {
                            discount = 0;
                        }

                        totalbeofrdicound = totalbeofrdicound + (detail.Product_Quantity * detail.Product_Price);
                        totaldiscount = totaldiscount + discount;

                        totaloftax = totaloftax + detail.Detail_Total_Tax;
                    }
                    totalAfterDiscount = totalbeofrdicound - totaldiscount;
                    totalafterTax = totalAfterDiscount + totaloftax;
                    if (master.FixedDiscount > 0)
                    {
                        fixedDiscount = (decimal)master.FixedDiscount;
                    }
                    else
                    {
                        fixedDiscount = 0;
                    }
                    totalafterfixed = totalafterfixed+( totalafterTax - fixedDiscount);

                }
               
            }
         
            
            return totalafterfixed;
        }
        /// <summary>
        /// calculate Amount of Recipt of Project
        /// </summary>
        /// <param name="pojectid"></param>
        /// <returns>decimal</returns>
        public static decimal amountofProjectRecipt(int pojectid)
        {
            decimal totalpost = 0;
            decimal totalvoid = 0;
            decimal total = 0;
            var db = new AccountingContainer();
            var thisproject = db.Projects.Find(pojectid);
            var thisinvoices = db.ProjectRecipts.Where(e => e.ProjectCode == thisproject.Project_Code&&e.transActionState==TransactionState.Post).ToList();
            if (thisinvoices.Count > 0)
            {
                foreach (var master in thisinvoices)
                {

                    totalpost = totalpost + master.Pay_Amount;
                }
            }
            //var thisinvoicesvoid = db.ProjectRecipts.Where(e => e.ProjectCode == thisproject.Project_Code && e.transActionState == TransactionState.Void).ToList();
            //if (thisinvoicesvoid.Count > 0)
            //{
            //    foreach (var master in thisinvoicesvoid)
            //    {

            //        totalvoid = totalvoid + master.Pay_Amount;
            //    }
            //}
            total = totalpost;

            return total;
        }
        

    }
}