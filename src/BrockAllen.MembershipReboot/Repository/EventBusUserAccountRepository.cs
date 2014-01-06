﻿/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace BrockAllen.MembershipReboot
{
    public class EventBusUserAccountRepository : IUserAccountRepository
    {
        IEventSource source;
        IUserAccountRepository inner;
        IEventBus validationBus;
        IEventBus eventBus;

        public EventBusUserAccountRepository(IEventSource source, IUserAccountRepository inner, IEventBus validationBus, IEventBus eventBus)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (inner == null) throw new ArgumentNullException("inner");
            if (validationBus == null) throw new ArgumentNullException("validationBus");
            if (eventBus == null) throw new ArgumentNullException("eventBus");

            this.source = source;
            this.inner = inner;
            this.validationBus = validationBus;
            this.eventBus = eventBus;
        }

        private void RaiseValidation()
        {
            foreach (var evt in source.GetEvents())
            {
                this.validationBus.RaiseEvent(evt);
            }
        }

        private void RaiseEvents()
        {
            foreach (var evt in source.GetEvents())
            {
                this.eventBus.RaiseEvent(evt);
            }

            source.Clear();
        }

        public IQueryable<IUserAccount> GetAll()
        {
            return inner.GetAll();
        }

        public IUserAccount Get(Guid key)
        {
            return inner.Get(key);
        }

        public IUserAccount Create()
        {
            return inner.Create();
        }

        public void Add(IUserAccount item)
        {
            RaiseValidation();
            inner.Add(item);
            RaiseEvents();
        }

        public void Remove(IUserAccount item)
        {
            RaiseValidation();
            inner.Remove(item);
            RaiseEvents();
        }

        public void Update(IUserAccount item)
        {
            RaiseValidation();
            inner.Update(item);
            RaiseEvents();
        }

        public void Dispose()
        {
            if (inner.TryDispose()) inner = null;
        }
    }
}
