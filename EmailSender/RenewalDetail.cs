using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    internal class RenewalDetail
    {
        public int CaseId { get; set; }
        public string CaseNumber { get; set; }
        public string YourReference { get; set; }
        public string AgentNameId { get; set; }
        public string AgentName { get; set; }
        public string ApplicantNameId { get; set; }
        public string ApplicantName { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string CorrespondenceAddressName { get; set; }
        public string CorrespondenceAddressEmails { get; set; }
        public int CorrespondenceAddressLanguageId { get; set; }
        public DateTime NextRenewal { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ApplicationNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string Catchword { get; set; }
        public string Classes { get; set; }
        public string ClassesDescription { get; set; }
        public string TrademarkCategory { get; set; }
        public string DeviceFileName { get; set; }
        public string DocumentsPath { get; set; }

    }
}
