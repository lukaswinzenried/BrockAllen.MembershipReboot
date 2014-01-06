﻿/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;

namespace BrockAllen.MembershipReboot
{
    public class EmailEventHandler<T>
        where T: UserAccount
    {
        IMessageFormatter<T> messageFormatter;
        IMessageDelivery messageDelivery;

        public EmailEventHandler(IMessageFormatter<T> messageFormatter)
            : this(messageFormatter, new SmtpMessageDelivery())
        {
        }

        public EmailEventHandler(IMessageFormatter<T> messageFormatter, IMessageDelivery messageDelivery)
        {
            if (messageFormatter == null) throw new ArgumentNullException("messageFormatter");
            if (messageDelivery == null) throw new ArgumentNullException("messageDelivery");

            this.messageFormatter = messageFormatter;
            this.messageDelivery = messageDelivery;
        }

        public virtual void Process(UserAccountEvent<T> evt, object extra = null)
        {
            dynamic d = new DynamicDictionary(extra);
            var msg = this.messageFormatter.Format(evt, d);
            if (msg != null)
            {
                msg.To = d.NewEmail ?? evt.Account.Email;
                if (!String.IsNullOrWhiteSpace(msg.To))
                {
                    this.messageDelivery.Send(msg);
                }
            }
        }
    }

    public class EmailAccountCreatedEventHandler : EmailAccountCreatedEventHandler<UserAccount>
    {
        public EmailAccountCreatedEventHandler(IMessageFormatter<UserAccount> messageFormatter)
            : base(messageFormatter)
        {
        }

        public EmailAccountCreatedEventHandler(IMessageFormatter<UserAccount> messageFormatter, IMessageDelivery messageDelivery)
            : base(messageFormatter, messageDelivery)
        {
        }
    }

    public class EmailAccountCreatedEventHandler<T>
        : EmailEventHandler<T>, IEventHandler<AccountCreatedEvent<T>>
        where T : UserAccount
    {
        public EmailAccountCreatedEventHandler(IMessageFormatter<T> messageFormatter)
            : base(messageFormatter)
        {
        }

        public EmailAccountCreatedEventHandler(IMessageFormatter<T> messageFormatter, IMessageDelivery messageDelivery)
            : base(messageFormatter, messageDelivery)
        {
        }

        public void Handle(AccountCreatedEvent<T> evt)
        {
            Process(evt, new { evt.VerificationKey });
        }
    }

    public class EmailAccountEventsHandler : EmailAccountEventsHandler<UserAccount>
    {
        public EmailAccountEventsHandler(IMessageFormatter<UserAccount> messageFormatter)
            : base(messageFormatter)
        {
        }
        public EmailAccountEventsHandler(IMessageFormatter<UserAccount> messageFormatter, IMessageDelivery messageDelivery)
            : base(messageFormatter, messageDelivery)
        {
        }
    }

    public class EmailAccountEventsHandler<T> :
        EmailEventHandler<T>,
        IEventHandler<AccountVerifiedEvent<T>>,
        IEventHandler<PasswordResetRequestedEvent<T>>,
        IEventHandler<PasswordChangedEvent<T>>,
        IEventHandler<UsernameReminderRequestedEvent<T>>,
        IEventHandler<AccountClosedEvent<T>>,
        IEventHandler<UsernameChangedEvent<T>>,
        IEventHandler<EmailChangeRequestedEvent<T>>,
        IEventHandler<EmailChangedEvent<T>>,
        IEventHandler<MobilePhoneChangedEvent<T>>,
        IEventHandler<MobilePhoneRemovedEvent<T>>,
        IEventHandler<CertificateAddedEvent<T>>,
        IEventHandler<CertificateRemovedEvent<T>>,
        IEventHandler<LinkedAccountAddedEvent<T>>,
        IEventHandler<LinkedAccountRemovedEvent<T>>
        where T: UserAccount
    {
        public EmailAccountEventsHandler(IMessageFormatter<T> messageFormatter)
            : base(messageFormatter)
        {
        }
        public EmailAccountEventsHandler(IMessageFormatter<T> messageFormatter, IMessageDelivery messageDelivery)
            : base(messageFormatter, messageDelivery)
        {
        }

        public void Handle(AccountVerifiedEvent<T> evt)
        {
            Process(evt);
        }

        public void Handle(PasswordResetRequestedEvent<T> evt)
        {
            Process(evt, new { evt.VerificationKey });
        }

        public void Handle(PasswordChangedEvent<T> evt)
        {
            Process(evt);
        }

        public void Handle(UsernameReminderRequestedEvent<T> evt)
        {
            Process(evt);
        }

        public void Handle(AccountClosedEvent<T> evt)
        {
            Process(evt);
        }

        public void Handle(UsernameChangedEvent<T> evt)
        {
            Process(evt);
        }

        public void Handle(EmailChangeRequestedEvent<T> evt)
        {
            Process(evt, new{evt.NewEmail, evt.VerificationKey});
        }

        public void Handle(EmailChangedEvent<T> evt)
        {
            Process(evt);
        }

        public void Handle(MobilePhoneChangedEvent<T> evt)
        {
            Process(evt);
        }

        public void Handle(MobilePhoneRemovedEvent<T> evt)
        {
            Process(evt);
        }

        public void Handle(CertificateAddedEvent<T> evt)
        {
            Process(evt);
        }

        public void Handle(CertificateRemovedEvent<T> evt)
        {
            Process(evt);
        }

        public void Handle(LinkedAccountAddedEvent<T> evt)
        {
            Process(evt);
        }

        public void Handle(LinkedAccountRemovedEvent<T> evt)
        {
            Process(evt);
        }
    }
}
