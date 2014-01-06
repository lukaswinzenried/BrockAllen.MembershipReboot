﻿using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using BrockAllen.MembershipReboot.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

[assembly: Microsoft.Owin.OwinStartup(typeof(OwinHostSample.Startup))]

namespace OwinHostSample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMembershipReboot(app);
            app.UseNancy();
        }

        private static void ConfigureMembershipReboot(IAppBuilder app)
        {
            var cookieOptions = new CookieAuthenticationOptions
            {
                AuthenticationType = MembershipRebootOwinConstants.AuthenticationType
            };
            Func<IOwinContext, UserAccountService> uaFunc = ctx =>
            {
                var appInfo = new OwinApplicationInformation(
                    ctx,
                    "Test",
                    "Test Email Signature",
                    "/Login",
                    "/Register/Confirm/",
                    "/Register/Cancel/",
                    "/PasswordReset/Confirm/");

                var config = new MembershipRebootConfiguration();
                var emailFormatter = new EmailMessageFormatter(appInfo);
                // uncomment if you want email notifications -- also update smtp settings in web.config
                config.AddEventHandler(new EmailAccountEventsHandler(emailFormatter));

                var svc = new UserAccountService(config, new DefaultUserAccountRepository());
                svc.TwoFactorAuthenticationPolicy = new OwinCookieBasedTwoFactorAuthPolicy(ctx);
                return svc;
            };
            Func<IOwinContext, AuthenticationService> authFunc = ctx =>
            {
                return new OwinAuthenticationService(cookieOptions.AuthenticationType, uaFunc(ctx), ctx);
            };

            app.UseMembershipReboot(cookieOptions, uaFunc, authFunc);
        }
    }
}