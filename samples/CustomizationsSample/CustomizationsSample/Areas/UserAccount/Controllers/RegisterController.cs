﻿using BrockAllen.MembershipReboot.Mvc.Areas.UserAccount.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BrockAllen.MembershipReboot.Mvc.Areas.UserAccount.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        UserAccountService<CustomUserAccount> userAccountService;

        public RegisterController(UserAccountService<CustomUserAccount> userAccountService)
        {
            this.userAccountService = userAccountService;
        }

        public ActionResult Index()
        {
            return View(new RegisterInputModel());
        }
        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = this.userAccountService.CreateAccount(model.Username, model.Password, model.Email);
                    
                    // add our custom stuff
                    account.FirstName = model.FirstName;
                    account.LastName = model.LastName;
                    this.userAccountService.Update(account);

                    if (userAccountService.Configuration.RequireAccountVerification)
                    {
                        return View("Success", model);
                    }
                    else
                    {
                        return View("Confirm", true);
                    }
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

        public ActionResult Cancel(string id)
        {
            this.userAccountService.CancelVerification(id);
            return View("Cancel");
        }

        public ActionResult Confirm(string id)
        {
            this.userAccountService.VerifyEmailFromKey(id);
            return View("Success");
        }
    }
}
