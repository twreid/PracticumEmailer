﻿using Microsoft.Office.Interop.Outlook;
using PracticumEmailer.Interfaces;
using PracticumEmailer.Interfaces.Attributes;
using PracticumEmailer.Ui.Properties;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Weakly;

namespace PracticumEmailer.Ui.Managers
{
    [ExportEmailManager(Handler = EmailHandler.Test)]
    public class TestEmailManager : OutlookManagerBase
    {
        private readonly int _maximumTestEmails = Settings.Default.MaximumTestEmails;

        public override void Send(IEnumerable<MailMessage> message)
        {
            message.Take(_maximumTestEmails).ForEach(DisplayEmail);
        }

        private void DisplayEmail(MailMessage message)
        {
            var outlookMessage = Outlook.CreateItem(OlItemType.olMailItem) as MailItem;
            if (outlookMessage != null)
            {
                outlookMessage.Subject = message.Subject;
                outlookMessage.Importance = OlImportance.olImportanceHigh;
                Recipient to = outlookMessage.Recipients.Add(message.To.First().ToString());

                if (!to.Resolve())
                {
                    throw new InvalidDataException(string.Format("{0} is an invalid email.", message.To.First()));
                }

                outlookMessage.HTMLBody = message.Body;
                outlookMessage.BodyFormat = OlBodyFormat.olFormatHTML;

                outlookMessage.Display();
            }
        }
    }
}