using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class OperationRun
    {
        public static void Addnewoperation(string code )
        {
            var db = new AccountingContainer();
            var operations = db.OperationRuns.Where(a => a.Code == code).ToList();
            if (operations.Any())
            {
                db.OperationRuns.RemoveRange(operations);
                db.SaveChanges();
            }
            var operate = new OperationRun
            {
                Code = code
            };
            db.OperationRuns.Add(operate);
            db.SaveChanges();
        }
        public static void DeleteOperation(string code)
        {
            var db = new AccountingContainer();
            var op = db.OperationRuns.Where(a => a.Code == code).ToList();
            if (op.Count != 0)
            {
                db.OperationRuns.RemoveRange(op);
                db.SaveChanges();
            }


        }
    }
}