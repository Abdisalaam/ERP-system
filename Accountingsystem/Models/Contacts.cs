using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using Accountingsystem.ViewModel;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class Contacts
    {/// <summary>
     /// get All Contacts
     /// </summary>
     /// <returns>List<Contacts></returns>
        public static List<Contacts> GetAllConatcts()
        {
            var db = new AccountingContainer();
            var contacts = db.Contacts.ToList();
            return contacts;
        }
        /// <summary>
        /// get that Contact by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Contacts GetContactById(int id)
        {
            var db = new AccountingContainer();
            var contact = db.Contacts.Find(id);
            db.Dispose();
            return contact;
        }
        /// <summary>
        /// Get VM for New Creatin For Contact
        /// </summary>
        /// <returns></returns>
        public static ContactVM CreationVM()
        {
            var db = new AccountingContainer();
            var contantVm = new ContactVM();
            contantVm.MyCustomerList = new SelectList(db.Customers, "Customer_Id", "Customer_Name");
            
            return contantVm;
        }
        /// <summary>
        /// Create Contact and add it in db
        /// </summary>
        /// <param name="conatctVM"></param>
        /// <returns></returns>
        public static bool CreateNewContact(ContactVM conatctVM)
        {
            var db = new AccountingContainer();
            var newConatc = new Contacts();

            newConatc.Contact_Company = conatctVM.Contact_Company;
            newConatc.Contact_Email = conatctVM.Contact_Email;
            newConatc.Contact_Id = conatctVM.Contact_Id;

            var resulCustomer = db.Customers.Find(conatctVM.Customer_Id);
            if (resulCustomer != null)
            {

                newConatc.Customer_Id = resulCustomer.Customer_Id;

            }
            newConatc.Contact_Name = conatctVM.Contact_Name;

           
            db.Contacts.Add(newConatc);
            db.SaveChanges();
            db.Dispose();

            return true;
        }
        /// <summary>
        /// Fill For make Edit in Contact
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ContactVM FillConatct(int id)
        {
            var db = new AccountingContainer();
            var findContact = GetContactById(id);
            var customer = findContact.Customer_Id;
            if (findContact == null)
            {
                return null;
            }

            var fillContactInConatcVM = new ContactVM();
            fillContactInConatcVM.MyCustomerList = new SelectList(db.Customers, "Customer_Id", "Customer_Name");
            fillContactInConatcVM.Contact_Company = findContact.Contact_Company;
            fillContactInConatcVM.Contact_Email = findContact.Contact_Email;
            fillContactInConatcVM.Contact_Id = findContact.Contact_Id;
            fillContactInConatcVM.Contact_Name = findContact.Contact_Name;
            fillContactInConatcVM.Customer_Id = Convert.ToInt32(findContact.Customer_Id);
            var resultCustomer = db.Customers.Where(c => c.Customer_Id == customer);
            if (resultCustomer.Any())
            {
                fillContactInConatcVM.Customer_Id = resultCustomer.First().Customer_Id;
            }
            return fillContactInConatcVM;
        }
        /// <summary>
        /// save Edit data in database
        /// </summary>
        /// <param name="conatctVM"></param>
        /// <returns></returns>
        public static bool editContact(ContactVM conatctVM)
        {
            var db = new AccountingContainer();
            var contact = GetContactById(conatctVM.Contact_Id);
            if (contact == null)
            {
                return false;
            }

            contact.Contact_Company = conatctVM.Contact_Company;
            contact.Contact_Email = conatctVM.Contact_Email;
            contact.Contact_Id = conatctVM.Contact_Id;
            contact.Contact_Name = conatctVM.Contact_Name;
            var resultCustomer = db.Customers.Find(conatctVM.Customer_Id);
            if (resultCustomer != null)
            {
                contact.Customer_Id = resultCustomer.Customer_Id;
            }

            db.Entry(contact).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();

            return true;
        }
        /// <summary>
        /// Delete Contact From DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Contacts deleteContact(int id)
        {
            var db = new AccountingContainer();
            var contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return contact;
            }

            db.Contacts.Remove(contact);
            db.SaveChanges();
            db.Dispose();

            return contact;
        }

        /// <summary>
        /// make VM from Customer to help in partial View
        /// </summary>
        /// <returns></returns>
        public static ContactVM GetCustomerList()
        {
            var db = new AccountingContainer();
            ContactVM vm = new ContactVM();

            vm.MyCustomerList = new SelectList(db.Customers, "Customer_Id", "Customer_Name");

            return vm;
        }
    }
}