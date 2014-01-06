﻿using BrockAllen.MembershipReboot.Ef;
using BrockAllen.MembershipReboot.Mvc.App_Start;
using System.Data.Entity;
using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace BrockAllen.MembershipReboot.Mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<DefaultMembershipRebootDatabase>(new CreateDatabaseIfNotExists<DefaultMembershipRebootDatabase>());
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Application_BeginRequest()
        {
            InitDatabase();
        }

        private void InitDatabase()
        {
            var svc = DependencyResolver.Current.GetService<UserAccountService>();
            if (svc.GetByUsername("admin") == null)
            {
                var account = svc.CreateAccount("admin", "admin123", "brockallen@gmail.com");
                svc.VerifyAccount(account.VerificationKey, "admin123");

                account = svc.GetByID(account.ID);
                account.AddClaim(ClaimTypes.Role, "Administrator");
                svc.Update(account);
            }
        }
    }
}