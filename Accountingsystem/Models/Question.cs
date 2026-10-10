using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class Question
    {
        public static List<Question> GetAllQuestions()
        {
            var db = new AccountingContainer();
            var quest = db.Questions.ToList();

            return quest;
        }

        // use this method to   Get one element CategoryById from Database.  
        public static Question GetQuestionById(int id)
        {
            var db = new AccountingContainer();
            var quest = db.Questions.Find(id);
            return quest;
        }
        public static QuestionVM CreateVMForCreation()
        {
          
            var vm = new QuestionVM();


          
            return vm;
        }

        // use this method to  Create New Catagery and save it in DataBase.
        public static int Create(QuestionVM vm)
        {
            var db = new AccountingContainer();

            var quest = new Question();

            quest.IsRequired = vm.isRequired;
            quest.Name = vm.Name;
            quest.IsActive = true;
            if (vm.Sort > 0)
            {
                quest.Sort = vm.Sort;
            }
            else
            {
                var last = db.Questions.OrderByDescending(a => a.Sort).FirstOrDefault();
                if (last != null)
                {
                    quest.Sort = db.Questions.OrderByDescending(a => a.Sort).FirstOrDefault().Sort + 1;
                }
                else
                {
                    quest.Sort = 1;
                }
            }
            db.Questions.Add(quest);
            db.SaveChanges();
            db.Dispose();
            return quest.Id;
        }

        // use this method to load Details of Data for Category.
        public static QuestionVM FillTheViewModel(int id)
        {
            var db = new AccountingContainer();
            var quest = db.Questions.Find(id);
            var vm = new QuestionVM();
            vm.isRequired = quest.IsRequired;
            vm.Name = quest.Name;
            vm.Sort = quest.Sort;
            return vm;
          

        }

        // use this method to Edite Catgery and save it in DataBase.
        public static Question EditeQuestion(QuestionVM vm)
        {
            var db = new AccountingContainer();
            var quest = db.Questions.Find(vm.Id);
            if (quest != null)
            {

                quest.IsRequired = vm.isRequired;
                quest.Name = vm.Name;
                if (vm.Sort > 0)
                {
                    quest.Sort = vm.Sort;
                }
                else
                {
                    var last = db.Questions.OrderByDescending(a => a.Sort).FirstOrDefault();
                    if (last != null)
                    {
                        quest.Sort = db.Questions.OrderByDescending(a => a.Sort).FirstOrDefault().Sort + 1;
                    }
                    else
                    {
                        quest.Sort = 1;
                    }
                }
                db.Entry(quest).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return quest;
            }
            return null;
        }

        // use this method to Delete Category and save DataBase.
     
        public static AnswerVM AddAnswer(int id)
        {
            var db = new AccountingContainer();
        
            var vm = new AnswerVM();

            vm.QuestionId = id;

            return vm;


        }
        public static Answer AddAnswer(AnswerVM vm)
        {
            var db = new AccountingContainer();
            var quest = db.Questions.Find(vm.QuestionId);
            var answer = new Answer();

            answer.ProductId = vm.ProductId;

            answer.Desc = vm.Desc;
            answer.PathImage = "";
          
                
                quest.Answers.Add(answer);
                db.SaveChanges();
               
          
            if (vm.Image != null)
            {
                var ext = Path.GetExtension(vm.Image.FileName); //getting the extension(ex-.jpg)  
                string subPath = ("/Image"+"/"+quest.Id); // Your code goes here

                bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(subPath));

                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subPath));
                }
                string name = Path.GetFileNameWithoutExtension(vm.Image.FileName); //getting file name without extension  
                string myfile = answer.Id + ext; //appending the name with id  
                                                 // store the file inside ~/project folder(Img)  
                var path = Path.Combine(HttpContext.Current.Server.MapPath(subPath), myfile);
                answer.PathImage = subPath+"/"+ myfile;
                vm.Image.SaveAs(path);
            }
            db.Entry(answer).State = EntityState.Modified;
            db.SaveChanges();
            return answer;


        }
        public static AnswerVM EditAnswer(int id)
        {
            var db = new AccountingContainer();

            var vm = new AnswerVM();
            var Answer = db.Answers.Find(id);
            vm.QuestionId = Answer.QuestionId;
            vm.Desc = Answer.Desc;
            return vm;


        }
        public static Answer EditAnswer(AnswerVM vm)
        {
            var db = new AccountingContainer();
            var quest = db.Questions.Find(vm.QuestionId);
            var answer = db.Answers.Find(vm.Id);

            answer.ProductId = vm.ProductId;

            answer.Desc = vm.Desc;
       


            if (vm.Image != null)
            {
                var ext = Path.GetExtension(vm.Image.FileName); //getting the extension(ex-.jpg)  
                string subPath = ("/Image" + "/" + quest.Id); // Your code goes here

                bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(subPath));

                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subPath));
                }
                string name = Path.GetFileNameWithoutExtension(vm.Image.FileName); //getting file name without extension  
                string myfile = answer.Id + ext; //appending the name with id  
                                                 // store the file inside ~/project folder(Img)  
                var path = Path.Combine(HttpContext.Current.Server.MapPath(subPath), myfile);
                answer.PathImage = subPath + "/" + myfile;
                vm.Image.SaveAs(path);
            }
            db.Entry(answer).State = EntityState.Modified;
            db.SaveChanges();
            return answer;


        }

        public static bool DeleteQuestions(int id)
        {
            var db = new AccountingContainer();
            var quest = db.Questions.Find(id);
            if (quest.IsActive == true)
            {
                quest.IsActive = false;
            }
            else
            {
                quest.IsActive = true;
            }
            db.Entry(quest).State = EntityState.Modified;
            db.SaveChanges();
            return true;

        }
        public static int DeleteAnswers(int id)
        {
            var db = new AccountingContainer();
            var answer = db.Answers.Find(id);
            if (answer.IsActive == true)
            {
                answer.IsActive = false;
            }
            else
            {
                answer.IsActive = true;
            }
            db.Entry(answer).State = EntityState.Modified;
            db.SaveChanges();
            return answer.QuestionId;

        }
    }
}