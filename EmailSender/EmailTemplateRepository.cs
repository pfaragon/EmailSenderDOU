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
        public EmailTemplate GetTemplate(string spTemplate)
        {
            EmailTemplate EmailTemplate = new EmailTemplate();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(spTemplate, connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 30000;
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

        public List<InvoiceMail> GetInvoices()
        {
            List<InvoiceMail> invoiceList = new List<InvoiceMail>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_moe_GetMails_Overdue_Days", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 30000;
                        using (SqlDataReader sdr = command.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                InvoiceMail invoice = new InvoiceMail();
                                invoice.CaseNumber = sdr["CASE_NUMBER"].ToString();
                                invoice.AccountName = sdr["ACCOUNT_NAME"].ToString();
                                invoice.Overdue_Days = (int)sdr["OVERDUE_DAYS"];
                                invoice.Email = sdr["EmailSender"] == DBNull.Value ? "" : sdr["EmailSender"].ToString();
                                invoice.InvoiceTango = sdr["INVOICE_TANGO"] == DBNull.Value ? (int)sdr["INVOICE_PATRICIA"] : (int)sdr["INVOICE_TANGO"];
                                invoice.InvoiceAmount = (decimal)sdr["INVOICE_AMOUNT"];
                                invoice.InvoiceDate = (DateTime)sdr["INVOICE_DATE"];
                                invoice.Currency = sdr["CURRENCY"].ToString();
                                invoice.OurReference = sdr["CASE_NUMBER"].ToString();
                                invoiceList.Add(invoice);
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
            return invoiceList;
        }
        
        public void InvoiceEmailLog(string ClientName)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("RegisterInvoiceOverdueLog", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Client_Name", ClientName);
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
    }
}
