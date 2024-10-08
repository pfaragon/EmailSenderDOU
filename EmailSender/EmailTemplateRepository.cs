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
            string lastCaseNumber = "";
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
                                lastCaseNumber = renewal.CaseNumber;
                                renewal.YourReference = reader["YourReference"].ToString();
                                renewal.AgentNameId = reader["AgentNameId"].ToString();
                                renewal.AgentName = reader["AgentName"].ToString();
                                renewal.ApplicantNameId = reader["ApplicantNameId"].ToString();
                                renewal.ApplicantName = reader["ApplicantName"].ToString();
                                renewal.CountryId = reader["CountryId"].ToString();
                                renewal.CountryName = reader["Country"].ToString();
                                renewal.CorrespondenceAddressId = reader["CorrespondenceAddressId"].ToString();
                                renewal.CorrespondenceAddressName = reader["CorrespondenceAddressName"].ToString();
                                renewal.CorrespondenceAddressEmails = reader["CorrespondenceAddressEmails"] == DBNull.Value ? "" : reader["CorrespondenceAddressEmails"].ToString();
                                renewal.CorrespondenceAddressLanguageId = int.Parse(reader["CorrespondenceAddressLanguageId"].ToString());
                                renewal.NextRenewal = (DateTime)reader["NextRenewal"];
                                renewal.ApplicationNumber = reader["ApplicationNumber"].ToString();
                                renewal.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                renewal.Catchword = reader["Catchword"].ToString();
                                renewal.Classes = reader["Classes"].ToString();
                                renewal.ClassesDescription = reader["ClassesDescription"].ToString();
                                renewal.TrademarkCategory = reader["TrademarkCategory"].ToString();
                                renewal.DeviceFilePath = reader["DeviceFilePath"].ToString();
                                renewal.DocumentsPath = reader["DocumentsPath"].ToString();
                                renewalList.Add(renewal);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing case number {lastCaseNumber}: {ex.Message}");
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

        public void LogEmailSent(List<RenewalDetail> listRenewals, string emailStatus, string emailStatusMessage, string emailSubject, string emailFileName = "")
        {
            int emailNumber = SetProxEmailNumber();

            foreach (var renewal in listRenewals)
            {
                RenewalEmailLog(renewal, emailNumber, emailStatus, emailStatusMessage, emailSubject, emailFileName);
            }
        }
        private void RenewalEmailLog(RenewalDetail renewalEmail, int emailNumber, string emailStatus, string emailStatusMessage, string emailSubject, string emailFileName)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_moe_0020_InsertRenewalEmailLog", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmailNumber", emailNumber);
                        command.Parameters.AddWithValue("@EmailSubject", emailSubject);
                        command.Parameters.AddWithValue("@EmailStatus", emailStatus);
                        command.Parameters.AddWithValue("@EmailStatusMessage", emailStatusMessage);
                        command.Parameters.AddWithValue("EmailFileName", emailFileName);
                        command.Parameters.AddWithValue("@CaseId", renewalEmail.CaseId);
                        command.Parameters.AddWithValue("@CaseNumber", renewalEmail.CaseNumber);
                        command.Parameters.AddWithValue("@AgentNameId", renewalEmail.AgentNameId);
                        command.Parameters.AddWithValue("@AgentName", renewalEmail.AgentName);
                        command.Parameters.AddWithValue("@ApplicantNameId", renewalEmail.ApplicantNameId);
                        command.Parameters.AddWithValue("@ApplicantName", renewalEmail.ApplicantName);
                        command.Parameters.AddWithValue("@CountryId", renewalEmail.CountryId);
                        command.Parameters.AddWithValue("@Country", renewalEmail.CountryName);
                        command.Parameters.AddWithValue("@CorrespondenceAddressId", renewalEmail.CorrespondenceAddressId);
                        command.Parameters.AddWithValue("@CorrespondenceAddressName", renewalEmail.CorrespondenceAddressName);
                        command.Parameters.AddWithValue("@CorrespondenceAddressEmails", renewalEmail.CorrespondenceAddressEmails);
                        command.Parameters.AddWithValue("@CorrespondenceAddressLanguageId", renewalEmail.CorrespondenceAddressLanguageId);
                        command.Parameters.AddWithValue("@NextRenewal", renewalEmail.NextRenewal);
                        command.Parameters.AddWithValue("@ApplicationNumber", renewalEmail.ApplicationNumber);
                        command.Parameters.AddWithValue("@RegistrationNumber", renewalEmail.RegistrationNumber);
                        command.Parameters.AddWithValue("@Catchword", renewalEmail.Catchword);
                        command.Parameters.AddWithValue("@Classes", renewalEmail.Classes);
                        command.Parameters.AddWithValue("@ClassesDescription", renewalEmail.ClassesDescription);
                        command.Parameters.AddWithValue("@TrademarkCategory", renewalEmail.TrademarkCategory);
                        command.Parameters.AddWithValue("@DeviceFilePath", renewalEmail.DeviceFilePath);
                        command.Parameters.AddWithValue("@DocumentsPath", renewalEmail.DocumentsPath);

                        command.ExecuteNonQuery();
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

        public void InsertPatriciaDocument(int CaseId, string DocName, string DocFileName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_moe_0017_InsertDocLog", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CASE_ID", CaseId);
                        command.Parameters.AddWithValue("@DOC_NAME", DocName);
                        command.Parameters.AddWithValue("@DOC_FILE_NAME", DocFileName);
                        command.ExecuteNonQuery();
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

        private int SetProxEmailNumber()
        {
            int emailNumber;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_moe_0019_SetProxEmailNumber", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        emailNumber = Int32.Parse(command.ExecuteScalar().ToString());
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
            return emailNumber;
        }
    }
}
