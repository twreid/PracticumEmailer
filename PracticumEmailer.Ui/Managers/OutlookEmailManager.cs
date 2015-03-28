using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Microsoft.Office.Interop.Outlook;
using PracticumEmailer.Interfaces;
using PracticumEmailer.Interfaces.Attributes;
using Weakly;

namespace PracticumEmailer.Ui.Managers
{
    [ExportEmailManager(Handler = EmailHandler.Outlook)]
    public class OutlookEmailManager : OutlookManagerBase
    {
        public override void Send(IEnumerable<MailMessage> message)
        {
            message.ForEach(SendEmail);
        }

        private void SendEmail(MailMessage message)
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

                outlookMessage.Send();
            }
        }
    }
}