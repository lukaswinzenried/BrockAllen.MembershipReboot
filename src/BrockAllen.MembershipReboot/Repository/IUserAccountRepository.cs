﻿/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Linq;
namespace BrockAllen.MembershipReboot
{
    public interface IUserAccountRepository<T> : IRepository<T>
        where T : UserAccount
    {
    }
    
    public interface IUserAccountRepository : IUserAccountRepository<UserAccount> { }
}
