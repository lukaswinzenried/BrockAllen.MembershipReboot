﻿/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;

namespace BrockAllen.MembershipReboot
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureCookieBasedTwoFactorAuthPolicy(this MembershipRebootConfiguration config, CookieBasedTwoFactorAuthPolicy policy)
        {
            if (config == null) throw new ArgumentNullException("config");

            config.TwoFactorAuthenticationPolicy = policy;
            config.AddEventHandler(policy);
        }

        public static void ConfigurePasswordComplexity(this MembershipRebootConfiguration config)
        {
            if (config == null) throw new ArgumentNullException("config");
            config.RegisterPasswordValidator(new PasswordComplexityValidator());
        }
        
        public static void ConfigurePasswordComplexity(this MembershipRebootConfiguration config, int minimumLength, int minimumNumberOfComplexityRules)
        {
            if (config == null) throw new ArgumentNullException("config");
            config.RegisterPasswordValidator(new PasswordComplexityValidator(minimumLength, minimumNumberOfComplexityRules));
        }
    }
}
