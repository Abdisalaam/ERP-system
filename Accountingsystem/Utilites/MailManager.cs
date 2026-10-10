using Accountingsystem.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Configuration;

namespace Accountingsystem.Utilites
{
    public class MailManager
    {
        public static bool SendEmail(string address, string subject, string message)
        {

            try
            {
                var db = new AccountingContainer();
                var email = "noreply@byanaty.com";
                var company = db.Companies.FirstOrDefault();
                var companyName = company.Comp_Name;
                var msg = new MailMessage { From = new MailAddress(email, companyName) };
                msg.Subject = subject;
                msg.Body = message + "</br></br></br>" + @"<img  src = ""https://www.byanaty.com/demo/Image/ByanatyLogo.png"" style="" width:100%;height:200px; "" />";
                var mServer = "smtp.ionos.com";
                var mServerPort = 587;
                var mServerUserId = "noreply@byanaty.com";
                var mServerPassword = "Byanaty@123";
                msg.To.Add(new MailAddress(address));
                msg.IsBodyHtml = true;
                var loginInfo = new NetworkCredential(mServerUserId, mServerPassword);
                var smtpClient = new SmtpClient
                {
                    Host = mServer,
                    Port = mServerPort,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = loginInfo
                };
                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {
                var e = ex.Message;
                return false;
            }
            return true;
        }
        public static bool SendEmailToCustomer(string address, string subject, string message)
        {

            try
            {
                var db = new AccountingContainer();
                var email = "noreplay@mydataitc.com";
                var company = db.Companies.FirstOrDefault();
                var companyName = company.Comp_Name;
                var msg = new MailMessage { From = new MailAddress(email, companyName) };
                msg.Subject = subject;
                msg.Body = message + "</br></br></br>" + @"<img  src = ""https://www.mydataitc.com/almehwar/demo/Image/ByanatyLogo.png "" style="" width:100%;height:200px; "" />";
                var mServer = "smtp.ionos.com";
                var mServerPort = 587;
                var mServerUserId = "noreplay@mydataitc.com";
                var mServerPassword = "Byanaty@123";
                msg.To.Add(new MailAddress(address));
                msg.IsBodyHtml = true;
                var loginInfo = new NetworkCredential(mServerUserId, mServerPassword);
                var smtpClient = new SmtpClient
                {
                    Host = mServer,
                    Port = mServerPort,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = loginInfo
                };
                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {
                var e = ex.Message;
                return false;
            }
            return true;
        }
        public static bool SendEmailWithAttachment(string address, string subject, string message, string PathAttach)
        {

            try
            {
                var db = new AccountingContainer();
                var email = "noreplay@mydataitc.com";
                var company = db.Companies.FirstOrDefault();
                var companyName = company.Comp_Name;
                var msg = new MailMessage { From = new MailAddress(email, companyName) };
                msg.Subject = subject;
                msg.Body = message + "</br></br></br>" + @"<img  src = ""https://www.sutumatic.com/demo/Image/ByanatyLogo.png "" style="" width:100%;height:200px; "" />";
                var mServer = "smtp.ionos.com";
                var mServerPort = 587;
                var mServerUserId = "noreplay@mydataitc.com";
                var mServerPassword = "Byanaty@123";
                msg.To.Add(new MailAddress(address));
                Attachment data = new Attachment(PathAttach, MediaTypeNames.Application.Octet);
                msg.Attachments.Add(data);
                msg.IsBodyHtml = true;
                var loginInfo = new NetworkCredential(mServerUserId, mServerPassword);
                var smtpClient = new SmtpClient
                {
                    Host = mServer,
                    Port = mServerPort,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = loginInfo
                };
                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {
                var e = ex.Message;
                return false;
            }
            return true;
        }

        //public static bool SendMessage(string address, string subject, string message)
        //{
        //    try
        //    {
        //        var email = "no_reply@sutumatic.org";

        //        var mServer = "smtp.ionos.com";

        //        var mServerPort = 587;

        //        var mServerUserId = "no_reply@sutum.org";

        //        var mServerPassword = "Thamer@123";
        //        var msg = new MailMessage { From = new MailAddress(email) };
        //        msg.To.Add(new MailAddress(address));
        //        msg.Subject = subject;
        //        msg.Body = @"<img  src = ""https://www.sutum.org/sutum//Assets/img/SVG/Header.jpg "" style="" width:100%; height:220Px;"" />" + " </br>  </br>" + @"<img src = ""https://www.sutum.org/sutum/Assets/img/SVG/SVG/Logos/SVG/Sutumatic.svg""  />" +
        //            message + @"<img  src = ""https://www.sutum.org/sutum//Assets/img/SVG/Footer.jpg "" style="" width:100%;height:220Px; "" />";
        //        msg.IsBodyHtml = true;
        //        var loginInfo = new NetworkCredential(mServerUserId, mServerPassword);
        //        var smtpClient = new SmtpClient
        //        {
        //            Host = mServer,
        //            Port = mServerPort,
        //            EnableSsl = true,
        //            UseDefaultCredentials = false,
        //            Credentials = loginInfo
        //        };
        //        smtpClient.Send(msg);
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public static bool SendMessageWhollyShop(string address, string subject, string message)
        //{
        //    try
        //    {
        //        //var email = WebConfigurationManager.AppSettings["FROM_EMAIL"];

        //        //var mServer = WebConfigurationManager.AppSettings["EmailServer"];

        //        //var mServerPort = Convert.ToInt32(WebConfigurationManager.AppSettings["EmailServerPort"]);

        //        //var mServerUserId = WebConfigurationManager.AppSettings["SmtpUserID"];

        //        //var mServerPassword = WebConfigurationManager.AppSettings["SmtpPassword"];
        //        var email = "no_reply@sutumatic.org";

        //        var mServer = "smtp.ionos.com";

        //        var mServerPort = 587;

        //        var mServerUserId = "no_reply@sutum.org";

        //        var mServerPassword = "Thamer@123";
        //        var msg = new MailMessage { From = new MailAddress(email) };
        //        msg.To.Add(new MailAddress(address));
        //        msg.Subject = subject;
        //        msg.Body = @"<img  src = ""https://www.mydataitc/Assets/img/SVG/Header.jpg "" style="" width:100%; height:220Px;"" />" + @"<img src = ""https://www.sutum.org/sutum//Assets/img/SVG/SVG/Logos/SVG/Whollyshop.svg""  />" + 
        //            message + @"<img  src = ""https://www.mydataitc/Assets/img/SVG/Footer.jpg "" style="" width:100%;height:220Px; "" />";
        //        //msg.Body = @"<img src = ""https://www.mydataitc/Assets/img/SVG/SVG/Logos/SVG/Whollyshop.svg""  />" + " </br>  </br>" + @"<img src = ""https://www.sutum.org/sutum/Assets/img/SVG/Sutum Logo.svg""  />" + message;

        //        msg.IsBodyHtml = true;
        //        var loginInfo = new NetworkCredential(mServerUserId, mServerPassword);
        //        var smtpClient = new SmtpClient
        //        {
        //            Host = mServer,
        //            Port = mServerPort,
        //            EnableSsl = true,
        //            UseDefaultCredentials = false,
        //            Credentials = loginInfo
        //        };
        //        smtpClient.Send(msg);
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public static bool SendEmail(string address, string subject, string message)
        //{
        //    try
        //    {

        //        var email = WebConfigurationManager.AppSettings["FROM_EMAIL"];

        //        var mServer = WebConfigurationManager.AppSettings["EmailServer"];

        //        var mServerPort = Convert.ToInt32(WebConfigurationManager.AppSettings["EmailServerPort"]);

        //        var mServerUserId = WebConfigurationManager.AppSettings["SmtpUserID"];

        //        var mServerPassword = WebConfigurationManager.AppSettings["SmtpPassword"];
        //        var msg = new MailMessage { From = new MailAddress(email) };
        //        msg.To.Add(new MailAddress(address));
        //        msg.Subject = subject;
        //        msg.Body = message;
        //        msg.IsBodyHtml = true;
        //        var loginInfo = new NetworkCredential(mServerUserId, mServerPassword);
        //        var smtpClient = new SmtpClient
        //        {
        //            Host = mServer,
        //            Port = mServerPort,
        //            EnableSsl = true,
        //            UseDefaultCredentials = false,
        //            Credentials = loginInfo
        //        };
        //        //smtpClient.Send(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        var mes = ex.Message;
        //        return false;
        //    }

        //    return true;
        //}


        //public static bool SendEmail(string address, string subject, string message)
        //public static bool SendEmail(string message, string address, string subject)
        //{
        //    //if (String.IsNullOrEmpty("saberfawzy1994@gmail.com"))
        //    //    return true;
        //    try
        //    {
        //        MailMessage mail = new MailMessage();
        //        mail.To.Add(message);
        //        mail.From = new MailAddress("mm4436906@gmail.com");
        //        mail.Subject = subject;

        //        mail.Body = message;

        //        mail.IsBodyHtml = true;
        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new System.Net.NetworkCredential("mm4436906@gmail.com", "Sutum@#123"); // ***use valid credentials***
        //        smtp.Port = 587;

        //        smtp.EnableSsl = true;
        //        smtp.Send(mail);
        //            return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var mes = ex.Message;
        //    }
        //    return false;
        //}
    }
}