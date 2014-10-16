using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PracticumEmailer.Interfaces
{
    public interface IEmailManager
    {
        MailMessage GenerateEmail(Domain.Student student);

        void Send(MailMessage message);
    }
}
