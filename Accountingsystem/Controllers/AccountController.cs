using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Accountingsystem.Models;
using System.Web.Security;
using Accountingsystem.Utilites;
using System.Threading;
using System.Security.Cryptography;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace Accountingsystem.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }
        [AllowAnonymous]
        public ActionResult ChangeLanguage(string url, string languageChoice)
        {
            if (!string.IsNullOrEmpty(languageChoice))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(languageChoice);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageChoice);
            }

            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = languageChoice;
            Response.Cookies.Add(cookie);
            return Redirect(url);
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
        private static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return buffer3.SequenceEqual(buffer4); ;
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
            var a = 0;

            if (!ModelState.IsValid)
            {

                if (cookie != null && cookie.Value != null)
                {
                    if (cookie.Value == "en")
                    {
                        ViewBag.Message = "Please make sure providing the correct crdintials";
                    }
                    else
                    {
                        ViewBag.Message = "يرجى التأكد من تقديم البيانات الأساسية الصحيحة";
                    }
                }
                else
                {
                    ViewBag.Message = "يرجى التأكد من تقديم البيانات الأساسية الصحيحة";
                }
                ModelState.AddModelError("", ViewBag.Message);
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //HttpCookie Authcookie = HttpContext.Request.Cookies.Get(".AspNet.ApplicationCookie");

            //if (Authcookie != null && Authcookie.Value != null)
            //{
            //    var user = UserManager.FindByName(model.Email);  
            //    var comparePasswords = SignInManager.UserManager.CheckPassword(user, model.Password);
            //    if (comparePasswords)
            //    {
            //        ViewModel.UserConnectionVM userConnectionVM = new ViewModel.UserConnectionVM();
            //        userConnectionVM.Email = model.Email;
            //        userConnectionVM.SessionId = Authcookie.Value;
            //        userConnectionVM.Expires = Authcookie.Expires;
            //        userConnectionVM.Token = new Guid().ToString();
            //        UserConnection.RemoveUserLogout(model.Email);
            //        UserConnection.AddUserLogin(userConnectionVM);                 
            //        return RedirectToLocal(returnUrl);
            //    }               
            //}
            Client.ChecedTheEmailForAllClient();
            var msg = Client.CheckClientLogin(model);
            SignInStatus result = new SignInStatus();
            if (msg != "")
            {
                result = SignInStatus.Failure;
                a = 7;
            }
            else
            {

                result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            }
            switch (result)
            {
                case SignInStatus.Success:

                    //if (true)
                    //{
                    //    if (cookie != null && cookie.Value != null)
                    //    {
                    //        if (cookie.Value == "en")
                    //        {
                    //            ViewBag.Message = "that Account is open in another Device";
                    //        }
                    //        else
                    //        {
                    //            ViewBag.Message = "هذا الحساب مفتوح على جهاز اخر";
                    //        }
                    //    }
                    //    else
                    //    {
                    //        ViewBag.Message = "هذا الحساب مفتوح على جهاز اخر";
                    //    }
                    //    ModelState.AddModelError("", ViewBag.Message);
                    //    return View(model);
                    //}
                    if (Company.GetCompanies().Any())
                    {
                        return RedirectToLocal(returnUrl);
                    
                    }
                    else
                    {
                        return RedirectToAction("Create", "Company");
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    if (a == 7)
                    {
                        if (cookie != null && cookie.Value != null)
                        {
                            if (cookie.Value == "en")
                            {
                                ViewBag.Message = "your subscription is Expire ";
                            }
                            else
                            {
                                ViewBag.Message = "تم انتهاء مدة الاشتراك برجاء التجديد";
                            }
                        }
                        else
                        {
                            ViewBag.Message = "تم انتهاء مدة الاشتراك برجاء التجديد";
                        }

                    }
                    else
                    {
                        if (cookie != null && cookie.Value != null)
                        {
                            if (cookie.Value == "en")
                            {
                                ViewBag.Message = "Wrong username or password";
                            }
                            else
                            {
                                ViewBag.Message = "اسم المستخدم أو كلمة المرور خاطئة";
                            }
                        }
                        else
                        {
                            ViewBag.Message = "اسم المستخدم أو كلمة المرور خاطئة";
                        }







                    }
                    ModelState.AddModelError("", ViewBag.Message);
                    return View(model);
            }
        }

        //
        // POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        //{
        //    HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");

        //    var status = 0;
        //    SignInStatus result = new SignInStatus();
        //    var msg = Client.CheckClientLogin(model);
        //    if (msg != "")
        //    {
        //        result = SignInStatus.Failure;
        //        status = 7;
        //    }
        //    if (msg == "" && UserConnection.addUserLogin(model.Email) != 1)
        //    {
        //        result = SignInStatus.Failure;
        //        status = 8;
        //    }
        // if (!ModelState.IsValid)
        //    {
        //        status = 9;
        //        if (cookie != null && cookie.Value != null)
        //        {
        //            if (cookie.Value == "en")
        //            {
        //                ViewBag.Message = "Please make sure providing the correct crdintials";
        //            }
        //            else
        //            {
        //                ViewBag.Message = "يرجى التأكد من تقديم البيانات الأساسية الصحيحة";
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.Message = "يرجى التأكد من تقديم البيانات الأساسية الصحيحة";
        //        }
        //        ModelState.AddModelError("", ViewBag.Message);
        //        return View(model);
        //    }

        //    // This doesn't count login failures towards account lockout
        //    // To enable password failures to trigger account lockout, change to shouldLockout: true
        //    if (status == 0)
        //    {
        //        result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
        //    }

        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        //        case SignInStatus.Failure:
        //        default:
        //            if (status == 7)
        //            {
        //                if (cookie != null && cookie.Value != null)
        //                {
        //                    if (cookie.Value == "en")
        //                    {
        //                        ViewBag.Message = "your subscription is Expire ";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Message = "تم انتهاء مدة الاشتراك برجاء التجديد";
        //                    }
        //                }
        //                else
        //                {
        //                    ViewBag.Message = "تم انتهاء مدة الاشتراك برجاء التجديد";
        //                }

        //            }
        //            if (status == 8)
        //            {
        //                if (cookie != null && cookie.Value != null)
        //                {
        //                    if (cookie.Value == "en")
        //                    {
        //                        ViewBag.Message = "that Account is open in another Device";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Message = "هذا الحساب مفتوح على جهاز اخر";
        //                    }
        //                }
        //                else
        //                {
        //                    ViewBag.Message = "هذا الحساب مفتوح على جهاز اخر";
        //                }

        //            }



        //            else
        //            {
        //                if (cookie != null && cookie.Value != null)
        //                {
        //                    if (cookie.Value == "en")
        //                    {
        //                        ViewBag.Message = "Wrong username or password";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Message = "اسم المستخدم أو كلمة المرور خاطئة";
        //                    }
        //                }
        //                else
        //                {
        //                    ViewBag.Message = "اسم المستخدم أو كلمة المرور خاطئة";
        //                }
        //            }
        //            ModelState.AddModelError("", ViewBag.Message);
        //            return View(model);
        //    }
        //}

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(int id, string msg)

        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تسجيل موظف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var pack = PackageRecognize.checkValiditeOfUser(user.Id);
            if (pack.Item2 == true)
            {
                var allRoless = (new ApplicationDbContext()).Roles.OrderBy(r => r.Name).ToList();
                List<IdentityRole> RoleList = new List<IdentityRole>();

                    foreach(var role in allRoless)
                {
                    if(role.Name=="عميل")
                    {
                        role.Name = "Admin";
                      
                    }
                    RoleList.Add(role);
                }



            //    var allRoles = RoleList.Select(rr =>

            //new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList().Where(a => a.Text != "مالك النظام");

                //var allRoles = (new ApplicationDbContext()).Roles.ToList();
                var emp = Employee.getEmpById(id);

                RegisterViewModel reg = new RegisterViewModel();
                reg.selectrole = new SelectList(SystemRole.getAllRole(), "Id", "Name");
                reg.Email = emp.Emp_Mail;
                reg.employeeid = id;

                ViewBag.Message = msg;
                TempData["empid"] = reg.employeeid;
                return View(reg);
            }
            else
            {
                var msgo = Resources.validate.user;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                if (model.Value == "Admin")
                {
                    model.Value = "عميل";
                }
                var microRole = "عميل";
                UserManager.AddToRole(user.Id, microRole);
                model.employeeid = Convert.ToInt32(TempData["empid"]);
                Employee.TakeIDString(user.Id, model.employeeid, model.Email,Convert.ToInt32( model.Value));

                var msg = Resources.validate.CreateSucc;
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                ////return RedirectToAction("Index", "Home");
                return RedirectToAction("Index", "Employee", new { msg = msg });

            }

            AddErrors(result);
            ViewBag.Message = "يوجد اخطاء ما فى الباسورد او هذا الايميل موجود من قبل";
            return RedirectToAction("Register", "Account", new { msg = ViewBag.Message });

        }





        [Authorize(Roles = "مالك النظام")]
        public ActionResult RegisterClient()
        {
            var allRoles = (new ApplicationDbContext()).Roles.OrderBy(r => r.Name).ToList().Select(rr =>

            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            //var allRoles = (new ApplicationDbContext()).Roles.ToList();



            RegisterViewModel reg = new RegisterViewModel();
            reg.selectrole = new SelectList(allRoles, "Text", "Text");
            reg.PackageList = new SelectList(Package.getAllPackage(), "Id", "Package_Name");

            return View(reg);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterClient(RegisterViewModel model)
        {


            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);
            model.Value = "عميل";
            if (result.Succeeded)
            {
                //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);


                UserManager.AddToRole(user.Id, model.Value);
                //model.employeeid = Convert.ToInt32(TempData["empid"]);
                //Employee.TakeIDString(user.Id, model.employeeid);
                Client.addClientByRegisterMethod(user.Id, model);

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                ////return RedirectToAction("Index", "Home");
                return RedirectToAction("Index", "Client");

            }




            // If we got this far, something failed, redisplay form
            return View(model);
        }



        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Delete")]




        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "الرجاء.. لتغير الكلمة السرية اظغط <a href=\"" + callbackUrl + "\">هنا</a>");
                //  MailManager.SendEmail(model.Email,"reset the passwoed", "الرجاء.. لتغير الكلمة السرية اظغط <a href=\"" + callbackUrl + "\">هنا</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("MainLogin");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //UserConnection.RemoveUserLogout(user.Email);
            //return RedirectToAction("Index", "Home");
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request

            return RedirectToAction("Login", "Account");

        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }



        [AllowAnonymous]
        public ActionResult MainLogin(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MainLogin(LoginViewModel model, string ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            if (result == SignInStatus.Success)
            {
                

                Client.ChecedTheEmailForAllClient();
                var msg = Client.CheckClientLogin(model);
                if (msg != "")
                {
                    result = SignInStatus.Failure;

                }
                
            }

            switch (result)
            {
                case SignInStatus.Success:

                    return RedirectToLocal(ReturnUrl);

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = ReturnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt or your Subscription Ended.");
                    return View(model);
            }
        }


        public async Task<ActionResult> ResetPasswordAdmin(string email)
        {
            ResetPasswordViewModel resetPasswordViewModel = new ResetPasswordViewModel();
            var user = await UserManager.FindByNameAsync(email);
            resetPasswordViewModel.Code = email;
            resetPasswordViewModel.Email = email;
            return View(resetPasswordViewModel);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPasswordAdmin(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Employee.updateEmailEmpAndClient(model);
            var user = await UserManager.FindByNameAsync(model.Code);
            user.Email = model.Email;
            user.UserName = model.Email;
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var result = await UserManager.ResetPasswordAsync(user.Id, code, model.Password);
            await UserManager.UpdateAsync(user);
      
            if (result.Succeeded)
            {
                return RedirectToAction( "Index", "Client");
            }
            AddErrors(result);
            return View();
        }







        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {

            if (User.IsInRole("بائع") || User.IsInRole("مدخل بيانات") || User.IsInRole("فني"))
            {
                return RedirectToAction("WelcomePage", "Company");

            }
            else
            {
                //if (string.IsNullOrEmpty(returnUrl))
                //{
                //    return RedirectToAction("Index", "Home");
                //}
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {

                    // return RedirectToAction("WelcomePage", "Company");
                    return RedirectToAction("Index", "Home");
                }
            }

            //  return RedirectToAction("WelcomePage", "Company");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }

        }
        #endregion
    }
}