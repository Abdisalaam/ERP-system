using System;
using System.Data.Entity;
using System.Linq;
using Accountingsystem.ViewModel;

namespace Accountingsystem.Models
{
    public partial class UserConnection
    {
        public static int AddUserLogin(UserConnectionVM userConnectionVM)
        {
        
            var db = new AccountingContainer();
            var UserLogin = db.UserConnections.Where(a => a.Email == userConnectionVM.Email && a.Connect == true).Count();
            if (UserLogin == 0)
            {
                var userConnect = new UserConnection();
                userConnect.Connect = true;
                userConnect.DateLogin = DateTime.Now;
                userConnect.Email = userConnectionVM.Email;
                userConnect.SessionId = userConnectionVM.SessionId;
                userConnect.Expires = userConnectionVM.Expires;
                userConnect.Token = userConnectionVM.Token;
                db.UserConnections.Add(userConnect);
                db.SaveChanges();
                db.Dispose();               
                return 1;
            }
            return 0;

        }
        public static int RemoveUserLogout(string email)
        {
            var db = new AccountingContainer();
            var connectList = db.UserConnections.ToList();
            var latstRecord = connectList.Max(p => p.Id);
            var UserLogin = connectList.Find(x => x.Id == latstRecord);
            if (UserLogin.Connect == true)
            {                
                UserLogin.Connect = false;
                UserLogin.DateLogout = DateTime.Now;
                db.Entry(UserLogin).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return 1;
            }
            return 0;

        }

    }
}