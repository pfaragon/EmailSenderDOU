using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    internal class EmailTemplate
    {
        public int TemplateID { get; set; }
        public string TemplateName { get; set; }
        public string TemplateText { get; set; }
        public string SubjectText { get; set; }
        public string MailFrom { get; set; }
        public string MailTo { get; set; }
        public string MailCC { get; set; }
        public string MailBCC { get; set; }
    }
}
