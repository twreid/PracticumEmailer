using Microsoft.Office.Interop.Outlook;
using PracticumEmailer.Domain;
using PracticumEmailer.Interfaces;
using PracticumEmailer.Ui.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace PracticumEmailer.Ui.Managers
{
    public abstract class OutlookManagerBase : IEmailManager
    {
        protected static readonly string EmailTemplatesDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Settings.Default.TemplateDirectory);

        protected readonly string EmailSubject = Settings.Default.EmailSubject;

        protected readonly FileInfo FbiTemplate =
            new FileInfo(Path.Combine(EmailTemplatesDirectory, Settings.Default.FbiTemplate));

        protected readonly FileInfo FcsrTemplate =
            new FileInfo(Path.Combine(EmailTemplatesDirectory, Settings.Default.FcsrTemplate));

        protected readonly FileInfo FooterTemplate =
            new FileInfo(Path.Combine(EmailTemplatesDirectory, Settings.Default.FooterTemplate));

        protected readonly FileInfo HeaderTemplate =
            new FileInfo(Path.Combine(EmailTemplatesDirectory, Settings.Default.HeaderTemplate));

        protected readonly FileInfo LiabTemplate =
            new FileInfo(Path.Combine(EmailTemplatesDirectory, Settings.Default.LiabTemplate));

        protected readonly Application Outlook = new Application();

        protected readonly FileInfo TbTemplate =
            new FileInfo(Path.Combine(EmailTemplatesDirectory, Settings.Default.TbTemplate));

        protected readonly IDictionary<string, string> Templates = new Dictionary<string, string>();

        public MailMessage GenerateEmail(Student student, Requirements emailRequirements)
        {
            var mailMessage = new MailMessage(Environment.UserName + "@missouristate.edu", student.Email)
            {
                Subject = EmailSubject,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Body = GetEmailBody(student, emailRequirements)
            };

            return mailMessage;
        }

        public abstract void Send(IEnumerable<MailMessage> message);

        private string GetEmailBody(Student student, Requirements emailRequirements)
        {
            var sb = new StringBuilder();

            sb.Append(GetHeader(student, emailRequirements));

            if (emailRequirements.HasFlag(Requirements.Fbi))
            {
                sb.Append(GetTemplate(FbiTemplate));
            }

            if (emailRequirements.HasFlag(Requirements.Fcsr))
            {
                sb.Append(GetTemplate(FcsrTemplate));
            }

            if (emailRequirements.HasFlag(Requirements.Liab))
            {
                sb.Append(GetTemplate(LiabTemplate));
            }

            if (emailRequirements.HasFlag(Requirements.Tb))
            {
                sb.Append(GetTemplate(TbTemplate));
            }

            sb.Append(GetTemplate(FooterTemplate));

            return sb.ToString();
        }

        private string GetHeader(Student student, Requirements requirements)
        {
            GetTemplate(HeaderTemplate);

            var template = new StringBuilder(Templates[HeaderTemplate.Name]);
            template
                .Replace("%student_name%", student.Name)
                .Replace("%courses%", string.Join(",", student.Courses))
                .Replace("%plural_courses%", student.Courses.Count > 1 ? "courses" : "course")
                .Replace("%class_type%",
                    requirements.HasFlag(Requirements.Practicum) ? "a practicum" : "student teaching")
                .Replace("%plural_documents%", Convert.ToUInt64(requirements) > 1 ? "documents" : "document");

            return template.ToString();
        }

        private string GetTemplate(FileInfo template)
        {
            if (!Templates.ContainsKey(template.Name))
            {
                using (var stream = new StreamReader(template.OpenRead()))
                {
                    Templates.Add(template.Name, stream.ReadToEnd());
                }
            }

            return Templates[template.Name];

        }
    }
}