﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrockAllen.MembershipReboot
{
    public abstract class RelativePathApplicationInformation : ApplicationInformation
    {
        public string RelativeLoginUrl { get; set; }
        public string RelativeVerifyAccountUrl { get; set; }
        public string RelativeCancelNewAccountUrl { get; set; }
        public string RelativeConfirmPasswordResetUrl { get; set; }
        public string RelativeConfirmChangeEmailUrl { get; set; }

        protected abstract string GetApplicationBaseUrl();

        string baseUrl;
        object urlLock = new object();
        string BaseUrl
        {
            get
            {
                if (baseUrl == null)
                {
                    lock (urlLock)
                    {
                        if (baseUrl == null)
                        {
                            // build URL
                            var tmp = GetApplicationBaseUrl();
                            if (!tmp.EndsWith("/")) tmp += "/";
                            baseUrl = tmp;
                        }
                    }
                }
                return baseUrl;
            }
        }

        string CleanupPath(string path)
        {
            if (path.StartsWith("~/"))
            {
                return path.Substring(2);
            }
            if (path.StartsWith("/"))
            {
                return path.Substring(1);
            }
            return path;
        }

        public override string LoginUrl
        {
            get
            {
                if (String.IsNullOrWhiteSpace(base.LoginUrl))
                {
                    LoginUrl = BaseUrl + CleanupPath(RelativeLoginUrl);
                }
                return base.LoginUrl;
            }
            set
            {
                base.LoginUrl = value;
            }
        }

        public override string VerifyAccountUrl
        {
            get
            {
                if (String.IsNullOrWhiteSpace(base.VerifyAccountUrl))
                {
                    VerifyAccountUrl = BaseUrl + CleanupPath(RelativeVerifyAccountUrl);
                }
                return base.VerifyAccountUrl;
            }
            set
            {
                base.VerifyAccountUrl = value;
            }
        }

        public override string CancelNewAccountUrl
        {
            get
            {
                if (String.IsNullOrWhiteSpace(base.CancelNewAccountUrl))
                {
                    CancelNewAccountUrl = BaseUrl + CleanupPath(RelativeCancelNewAccountUrl);
                }
                return base.CancelNewAccountUrl;
            }
            set
            {
                base.CancelNewAccountUrl = value;
            }
        }

        public override string ConfirmPasswordResetUrl
        {
            get
            {
                if (String.IsNullOrWhiteSpace(base.ConfirmPasswordResetUrl))
                {
                    ConfirmPasswordResetUrl = BaseUrl + CleanupPath(RelativeConfirmPasswordResetUrl);
                }
                return base.ConfirmPasswordResetUrl;
            }
            set
            {
                base.ConfirmPasswordResetUrl = value;
            }
        }

        public override string ConfirmChangeEmailUrl
        {
            get
            {
                if (String.IsNullOrWhiteSpace(base.ConfirmChangeEmailUrl))
                {
                    ConfirmChangeEmailUrl = BaseUrl + CleanupPath(RelativeConfirmChangeEmailUrl);
                }
                return base.ConfirmChangeEmailUrl;
            }
            set
            {
                base.ConfirmChangeEmailUrl = value;
            }
        }
    }
}
