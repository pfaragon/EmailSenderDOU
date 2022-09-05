using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace EmailSender
{
    internal class EmailTemplateRepository
    {
        
        private readonly string _connectionString;
        public EmailTemplateRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public EmailTemplate GetTemplate(string templateType, int language, string country = null, int? monthsBefore = null)
        {
            EmailTemplate EmailTemplate = new EmailTemplate();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_moe_0016_GetEmailTemplate", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 30000;
                        command.Parameters.AddWithValue("@templateType", templateType);
                        command.Parameters.AddWithValue("@language", language);
                        if (country != null) command.Parameters.AddWithValue("@country", country);
                        if (monthsBefore != null) command.Parameters.AddWithValue("@monthsBefore", monthsBefore);
                        using (SqlDataReader sdr = command.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                EmailTemplate.TemplateID = (int)sdr["TemplateID"];
                                EmailTemplate.TemplateText = sdr["TemplateText"].ToString();
                                EmailTemplate.SubjectText = sdr["SubjectText"].ToString();
                                EmailTemplate.MailFrom = sdr["MailFrom"].ToString();
                                EmailTemplate.MailTo = sdr["MailTo"] == DBNull.Value ? "" : sdr["MailCC"].ToString();
                                EmailTemplate.MailCC = sdr["MailCC"] == DBNull.Value ? "" : sdr["MailCC"].ToString();
                                EmailTemplate.MailBCC = sdr["MailBCC"] == DBNull.Value ? "" : sdr["MailBCC"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw ex;
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return EmailTemplate;
        }

        public List<RenewalDetail> GetRenewals(int pn_months)
        {
            List<RenewalDetail> renewalList = new List<RenewalDetail>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_moe_0015_GetRenewals", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 30000;
                        command.Parameters.AddWithValue("@pn_months", pn_months);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RenewalDetail renewal = new RenewalDetail();
                                renewal.CaseId = (int)reader["CaseId"];
                                renewal.CaseNumber = reader["CaseNumber"].ToString();
                                renewal.YourReference = reader["YourReference"].ToString();
                                renewal.AgentNameId = reader["AgentNameId"].ToString();
                                renewal.AgentName = reader["AgentName"].ToString();
                                renewal.ApplicantNameId = reader["ApplicantNameId"].ToString();
                                renewal.ApplicantName = reader["ApplicantName"].ToString();
                                renewal.CountryId = reader["CountryId"].ToString();
                                renewal.CountryName = reader["Country"].ToString();
                                renewal.CorrespondenceAddressName = reader["CorrespondenceAddressName"].ToString();
                                renewal.CorrespondenceAddressEmails = reader["CorrespondenceAddressEmails"] == DBNull.Value ? "" : reader["CorrespondenceAddressEmails"].ToString();
                                renewal.CorrespondenceAddressLanguageId = int.Parse(reader["CorrespondenceAddressLanguageId"].ToString());
                                renewal.NextRenewal = (DateTime)reader["NextRenewal"];
                                renewal.RegistrationDate = (DateTime)reader["RegistrationDate"];
                                renewal.ApplicationNumber = reader["ApplicationNumber"].ToString();
                                renewal.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                renewal.Catchword = reader["Catchword"].ToString();
                                renewal.Classes = reader["Classes"].ToString();
                                renewal.ClassesDescription = reader["ClassesDescription"].ToString();
                                renewal.TrademarkCategory = reader["TrademarkCategory"].ToString();
                                renewal.DeviceFileName = reader["DeviceFileName"].ToString();
                                renewal.DocumentsPath = reader["DocumentsPath"].ToString();
                                renewalList.Add(renewal);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return renewalList;
        }
        
        public void LogReminder(RenewalDetail invoiceMail, string state)
        {
            this.InvoiceEmailLog(invoiceMail, state);
        }

        public void LogOverdue(List<RenewalDetail> listOverdue)
        {
            foreach (var item in listOverdue)
            {
                InvoiceEmailLog(item, "Success Overdue Email");
            }
        }
        private void InvoiceEmailLog(RenewalDetail invoiceMail, string stateText)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_moe_0014_RegisterInvoiceOverdueLog", connection))
                {
                    try
                    {
                        //command.CommandType = CommandType.StoredProcedure;
                        //command.Parameters.AddWithValue("@Client_Name", invoiceMail.AccountName);
                        //command.Parameters.AddWithValue("@State", stateText);
                        //command.Parameters.AddWithValue("@Invoice_Tango", invoiceMail.InvoiceTango);
                        //command.Parameters.AddWithValue("@case_numnber", invoiceMail.OurReference);
                        //command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw ex;
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }
    }
}
