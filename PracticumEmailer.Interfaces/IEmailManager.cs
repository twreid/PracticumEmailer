using PracticumEmailer.Domain;
using System.Collections.Generic;
using System.Net.Mail;

namespace PracticumEmailer.Interfaces
{
    public enum EmailHandler : byte
    {
        Exchange,
        Outlook,
        Smtp,
        Test,
    }

    public interface IEmailManager
    {
        MailMessage GenerateEmail(Student student, Requirements emailRequirements);

        void Send(IEnumerable<MailMessage> message);
    }
}