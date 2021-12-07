using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    internal class InvoiceMail
    {
        public string CaseNumber { get; set; }
        public string Email { get; set; }
        public string AccountName { get; set; }
        public int Overdue_Days { get; set; }
        public int InvoiceTango { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceAmount { get; set; }
        public string Currency { get; set; }
    }
}
