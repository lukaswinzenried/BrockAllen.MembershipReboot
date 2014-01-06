﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace BrockAllen.MembershipReboot.Mvc.App_Start
{
    public class PasswordValidator : IValidator
    {
        public ValidationResult Validate(UserAccountService service, UserAccount account, string value)
        {
            if (value.Length < 4)
            {
                return new ValidationResult("Password must be at least 4 characters long");
            }
            
            return null;
        }
    }

    public class MembershipRebootConfig
    {
        public static MembershipRebootConfiguration Create()
        {
            var settings = SecuritySettings.Instance;
            settings.MultiTenant = false;
            
            var config = new MembershipRebootConfiguration(settings);
            config.RegisterPasswordValidator(new PasswordValidator());

            var delivery = new SmtpMessageDelivery();

            var appinfo = new AspNetApplicationInformation("Test", "Test Email Signature",
                "UserAccount/Login", 
                "UserAccount/Register/Confirm/",
                "UserAccount/Register/Cancel/",
                "UserAccount/PasswordReset/Confirm/",
                "UserAccount/ChangeEmail/Confirm/");
            var formatter = new CustomEmailMessageFormatter(appinfo);

            if (settings.RequireAccountVerification)
            {
                config.AddEventHandler(new EmailAccountCreatedEventHandler(formatter, delivery));
            }
            config.AddEventHandler(new EmailAccountEventsHandler(formatter, delivery));
            config.AddEventHandler(new AuthenticationAuditEventHandler());
            config.AddEventHandler(new NotifyAccountOwnerWhenTooManyFailedLoginAttempts());

            config.AddValidationHandler(new PasswordChanging());
            config.AddEventHandler(new PasswordChanged());

            return config;
        }
    }
}