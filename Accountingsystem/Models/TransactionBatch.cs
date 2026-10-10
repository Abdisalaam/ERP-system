using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accountingsystem.ViewModel;

namespace Accountingsystem.Models
{
    public partial class TransactionBatch
    {
        /// <summary>
        /// get All trans in db
        /// </summary>
        /// <returns></returns>
        public static List<TransactionBatch> GetAllTransactions()
        {
            var db = new AccountingContainer();
            var Transactions = db.TransactionBatches.ToList();
            return Transactions;
        }

        /// <summary>
        ///  get trans  by its id 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public static TransactionBatch GetTransactionById(int transactionId)
        {
            var db = new AccountingContainer();
            var Transaction = db.TransactionBatches.Find(transactionId);
            return Transaction;
        }

        /// <summary>
        /// save anew trans in db
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool AddNewTransaction(TransactionBatch transaction)
        {
            try
            {
                var db = new AccountingContainer();
                db.TransactionBatches.Add(transaction);
                db.SaveChanges();
                db.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                var result = ex.Message;
                throw;
            }
           
           
        }
    }
}