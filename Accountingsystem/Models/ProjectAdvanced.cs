using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public class ProjectAdvanced
    {
        public static ProjectClause getClauseById(int clauseid)
        {
            var db = new AccountingContainer();
            var clause = db.ProjectClauses.Find(clauseid);
            return clause;

        }
        public static ProjectVM CreateSubledgerForProject(int pro_Id)
        {
            var db = new AccountingContainer();
            ProjectVM vm = new ProjectVM();
            var project = db.Projects.Find(pro_Id);
            vm.Project_Id = pro_Id;
            vm.Project_Name = project.Project_Name;
            vm.Project_Code = project.Project_Code;

            var ThereORN = db.ProjectTemps.Find(pro_Id);
            if (ThereORN != null)
            {
                db.SubLedgertemps.RemoveRange(ThereORN.SubLedgertemps);
                db.ProjectClausetemps.RemoveRange(ThereORN.ProjectClausetemps);
            }
            else
            {
                var projectTemp = new ProjectTemp();
                projectTemp.Project_Id = pro_Id;
             

                db.ProjectTemps.Add(projectTemp);
               
            }
            db.SaveChanges();
            return vm;

        }
        public static List<ProjectClausetemp> GetProjectClausetempsByProId( int pro_Id)
        {
            var db = new AccountingContainer();
          
            var clasues = db.ProjectClausetemps.Where(a => a.ProjectTempProject_Id == pro_Id).ToList();
            return clasues;
        }
        public static List<SubLedgertemp> GetProjectSubLedgertempsByProId(int pro_Id)
        {
            var db = new AccountingContainer();
            var sub = db.SubLedgertemps.Where(a => a.ProjectTempProject_Id == pro_Id&&a.Subled_Level==1).ToList();
            return sub;
        }
        public static List<SubLedgertemp> GetProjectSubSubLedgertempsByProId(int pro_Id)
        {
            var db = new AccountingContainer();
            var sub = db.SubLedgertemps.Where(a => a.ProjectTempProject_Id == pro_Id && a.Subled_Level == 2).ToList();
            return sub;
        }

        public static string AddSubledgerList(ProjectSubledgerVM vm)
        {
            if (vm.SubledgerName == null || vm.SubledgerName == "")
            {
                return Resources.ProjectAdvanced.AddName;
            }
            if (vm.number <= 0 || vm.number == null)
            {
                 vm.number=1;
            }
            var db = new AccountingContainer();
            var List = new List<SubLedgertemp>();
            if (vm.number > 1)
            {
                int num = vm.number;
                int increament = 1;
           
                while (num != 0)
                {
                    var subtemp = new SubLedgertemp();
                    subtemp.SubLed_Name = vm.SubledgerName + increament;
                    subtemp.SubLedgerSubLed_Id = null;
                    subtemp.Subled_Level = 1;
                    subtemp.ProjectTempProject_Id = vm.projecttempId;
                    List.Add(subtemp);

                    increament++;
                    num--;

                }

            }
            else
            {
                var subtemp = new SubLedgertemp();
                subtemp.SubLed_Name = vm.SubledgerName ;
                subtemp.SubLedgerSubLed_Id = null;
                subtemp.Subled_Level = 1;
                subtemp.ProjectTempProject_Id = vm.projecttempId;
                List.Add(subtemp);

            }
            db.SubLedgertemps.AddRange(List);
            db.SaveChanges();
            return Resources.validate.SuccessfullAdd;
        }
        public static string AddSubSubledgerToMaster(ProjectSubSubledgerVM vm)
        {
            if (vm.subsubledgerName == null || vm.subsubledgerName == "")
            {
                return Resources.ProjectAdvanced.AddName;
            }
          

            var db = new AccountingContainer();
            if ( vm.projectSubledgerId > 0)
            {
                var subtemp = new SubLedgertemp();
                subtemp.SubLed_Name = vm.subsubledgerName;
                subtemp.SubLedgerSubLed_Id = vm.projectSubledgerId;
                subtemp.Subled_Level = 2;
                subtemp.ProjectTempProject_Id = vm.projectTempId;
                db.SubLedgertemps.Add(subtemp);
                db.SaveChanges();
            }
            else if(vm.projectSubledgerId == 0&&db.SubLedgertemps.Where(a=>a.ProjectTempProject_Id==vm.projectTempId).Count()>0) {

                var allMainSub = db.SubLedgertemps.Where(a => a.ProjectTempProject_Id == vm.projectTempId && a.Subled_Level == 1).ToList();
                var list = new List<SubLedgertemp>();
                    foreach (var mainsub in allMainSub)
                {
                    var subtemp = new SubLedgertemp();
                    subtemp.SubLed_Name = vm.subsubledgerName;
                    subtemp.SubLedgerSubLed_Id = mainsub.SubLed_Id;
                    subtemp.Subled_Level = 2;
                    subtemp.ProjectTempProject_Id = vm.projectTempId;
                    list.Add(subtemp);
                 

                }
                db.SubLedgertemps.AddRange(list);
                db.SaveChanges();
            }
            else
            {
                return Resources.ProjectAdvanced.addcostcenterfirst;
            }
            return Resources.validate.SuccessfullAdd;
        }
        public static string AddClauseList(ProjectClauseVM vm)
        {
            if (vm.clauseName == null || vm.clauseName == "")
            {
                return Resources.ProjectAdvanced.AddName;
            }
          
            var db = new AccountingContainer();
       
                    var clausetemp = new ProjectClausetemp();
            clausetemp.Name = vm.clauseName;

            clausetemp.ProjectTempProject_Id = vm.Projecttemp_Id;
                  


           
            db.ProjectClausetemps.Add(clausetemp);
            db.SaveChanges();
            return Resources.validate.SuccessfullAdd;
        }
        public static IEnumerable<SelectListItem> GetSubledgerList(int proId)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> sub = new SelectList(db.SubLedgertemps.Where(a => a.ProjectTempProject_Id == proId&&a.Subled_Level==1), "SubLed_Id", "SubLed_Name");
            return sub;

        }
        public static string deleteSubledgerTemp(int sub_Id)
        {
            var db = new AccountingContainer();
            var sub = db.SubLedgertemps.Find(sub_Id);
            db.SubLedgertemps.RemoveRange(sub.SubLedgers);
            db.SubLedgertemps.Remove(sub);
            db.SaveChanges();
            return Resources.validate.DeleteSuccessful;

        }
        public static string deleteClauseTemp(int clasueId)
        {
            var db = new AccountingContainer();
            var clause = db.ProjectClausetemps.Find(clasueId);          
            db.ProjectClausetemps.Remove(clause);
            db.SaveChanges();
            return Resources.validate.DeleteSuccessful;

        }
        public static string addSubToProject(ProjectVM vm)
        {
            var db = new AccountingContainer();
            var project = db.Projects.Find(vm.Project_Id);
            var projecttemp = db.ProjectTemps.Find(vm.Project_Id);
            var proSubledger = db.SubLedgers.Where(a => a.SubLed_Name == project.Project_Name&&a.SubLed_Type=="مشروع").FirstOrDefault();
            foreach(var sub in projecttemp.SubLedgertemps.Where(a=>a.Subled_Level==1))
            {
                var subvm = new SubLedgerVM();
                subvm.ParentSubledger_Id = proSubledger.SubLed_Id;
                subvm.SubLed_Name = sub.SubLed_Name;
                subvm.SubLed_Date = Convert.ToString(DateTime.Now);
                subvm.SubLed_Type = "مركز تكلفة رئيسي";
                var Subledger = SubLedger.createSubLedger(subvm);
              foreach(var subsub in sub.SubLedgers)
                {
                    var subvm2 = new SubLedgerVM();
                    subvm2.ParentSubledger_Id = Subledger.SubLed_Id;
                    subvm2.SubLed_Name = subsub.SubLed_Name;
                    subvm2.SubLed_Date = Convert.ToString(DateTime.Now);
                    subvm2.SubLed_Type = "مركز تكلفة فرعي";
                    var Subsubledger = SubLedger.createSubLedger(subvm2);

                }

            }
            var clauseList = new List<ProjectClause>();
            foreach(var clasue in projecttemp.ProjectClausetemps)
            {
                var clause = new ProjectClause();
                clause.Name = clasue.Name;
                clause.Project_Id = clasue.ProjectTempProject_Id;
                project.ProjectSubs.Add(clause);
            }
            db.SubLedgertemps.RemoveRange(projecttemp.SubLedgertemps.Where(a => a.Subled_Level == 1));
            db.SubLedgertemps.RemoveRange(projecttemp.SubLedgertemps.Where(a => a.Subled_Level == 2));
            db.ProjectClausetemps.RemoveRange(projecttemp.ProjectClausetemps);
            db.ProjectDetailTemps.RemoveRange(projecttemp.ProjectDetailTemps);
            db.ProjectTemps.Remove(projecttemp);
            db.Entry(project).State = EntityState.Modified;
            db.SaveChanges();
            return Resources.validate.CreateSucc; 
        }
        public static SubLedger GetSubledgersByName(string  name)
        {
            var db = new AccountingContainer();
         
            var proSubledger = db.SubLedgers.Where(a => a.SubLed_Name == name && a.SubLed_Type == "مشروع").FirstOrDefault();
            
            return proSubledger;
        }
    }
}