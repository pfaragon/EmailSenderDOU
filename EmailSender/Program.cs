using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;


namespace EmailSender
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connection = getConnection();
            EmailTemplateRepository templateRepository = new EmailTemplateRepository(connection);
            Console.WriteLine("Conexion creada");
            EmailTemplate emailTemplateReminder = templateRepository.GetTemplate("sp_moe_0011_Reminder_EmailTemplate");
            EmailTemplate emailTemplateOverdue = templateRepository.GetTemplate("sp_moe_0012_Overdue_Days_EmailTemplate");
            Console.WriteLine("template obtenidos");

            //para usar el template que corresponda en base a si es reminder o si ya se vencio.
            List<InvoiceMail> emailList = templateRepository.GetInvoices();

            List<InvoiceMail> listSameAccount = new List<InvoiceMail>();
            InvoiceMail lastReviewed = null;

            int count = 1;
            //en el invoice tengo que ver si es una misma cuenta para mandar un solo email con los datos de esa cuenta.
            foreach (var invoice in emailList)
            {
                //si es la primer vuelta seteo al actual.
                if (lastReviewed == null)
                {
                    lastReviewed = invoice;
                    listSameAccount.Add(invoice);
                }
                //si no es la primer vuelta me fijo si es la misma cuenta que la anterior.
                else
                {
                    //si no es el ultimo de la lista realizo esta accion
                    if (count != emailList.Count)
                    {
                        //si es la misma cuenta que la anterior la meto en la lista para mandar toda la info junta en un mail.
                        if (lastReviewed.AccountName == invoice.AccountName)
                        {
                            listSameAccount.Add(invoice);
                        }
                        //si no es la misma cuenta, mando el mail para la cuenta anterior que hasta ahora nunca se mando, limpio la lista para agrupar por esta nueva cuenta
                        else
                        {
                            ValidationSendEmail(listSameAccount, emailTemplateReminder, emailTemplateOverdue, templateRepository);
                            listSameAccount.Clear();
                            listSameAccount.Add(invoice);
                            lastReviewed = invoice;
                        }
                    }
                    //si es el ultimo de la lista
                    else
                    {
                        //si es la misma cuenta que la anterior la meto en la lista para mandar toda la info junta en un mail, pero desde aca porque termina la lista.
                        if (lastReviewed.AccountName == invoice.AccountName)
                        {
                            listSameAccount.Add(invoice);
                            ValidationSendEmail(listSameAccount,emailTemplateReminder, emailTemplateOverdue, templateRepository);
                        }
                        //si no es la misma cuenta que el anteultimo tengo que realizar la accion desde aca con un elemento pero tambien enviar el anterior porque estaba en espera
                        else
                        {
                            ValidationSendEmail(listSameAccount, emailTemplateReminder, emailTemplateOverdue, templateRepository);
                            List<InvoiceMail> listLastInvoice = new List<InvoiceMail>();
                            listLastInvoice.Add(invoice);
                            ValidationSendEmail(listLastInvoice, emailTemplateReminder, emailTemplateOverdue, templateRepository);
                        }
                    }
                }
                count++;
            }
        }

        private static void ValidationSendEmail(List<InvoiceMail> listSameAccount, EmailTemplate emailTemplateReminder, EmailTemplate emailTemplateOverdue, EmailTemplateRepository repository)
        {
            List<InvoiceMail> listWithOutReminder = new List<InvoiceMail>();
            string EmailAddress = "";
            string clientName = "";
            foreach (var invoiceMail in listSameAccount)
            {
                if (invoiceMail.Email != "")
                {
                    EmailAddress = invoiceMail.Email;
                    clientName = invoiceMail.AccountName;
                    listWithOutReminder.Add(invoiceMail);
                    if (invoiceMail.Overdue_Days == -7)
                    {
                        SendEmailReminder(emailTemplateReminder, invoiceMail);
                        repository.LogReminder(invoiceMail, "Success Reminder Email");
                        //lo quito de la lista para que en overdue no se mande de nuevo
                        listWithOutReminder.Remove(invoiceMail);
                    }
                }
            }
            if (DateTime.Now.Day == 1 || DateTime.Now.Day == 15)
            {
                SendEmailOverdue(emailTemplateOverdue, listWithOutReminder, EmailAddress, clientName);
                repository.LogOverdue(listWithOutReminder);
            }
        }

        private static string getConnection()
        {
            string connectionCoded = ConfigurationManager.ConnectionStrings["ConexionMoreProd"].ConnectionString;
            return Encoding.Default.GetString(Convert.FromBase64String(Encoding.Default.GetString(Convert.FromBase64String(connectionCoded))));
        }

        //reminder al service level no se le manda nada
        //si service level es != 2 no se manda
        //quizas 20005
        private static void SendEmailReminder(EmailTemplate template, InvoiceMail invoice)
        {
            try
            {
                if (invoice.ServiceLevel == 2 || invoice.ServiceLevel == 20005)
                {
                    string EmailAddress = "";
                    string bodyMessage = template.TemplateText;
                    //reemplazo el formato de comas que viene de sql para que el mail distinga varios destinatarios
                    EmailAddress = invoice.Email.Replace(',', ';');
                    bodyMessage = bodyMessage.Replace("{xx_CLIENT_NAME}", invoice.AccountName)
                        .Replace("{xx_INVOICE#}", invoice.InvoiceTango.ToString())
                        .Replace("{xx_INVOICE_AMOUNT}", invoice.Currency + " " + invoice.InvoiceAmount);
                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    message.From = new MailAddress(template.MailFrom);
                    foreach (var address in EmailAddress.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        message.To.Add(address);
                    }
                    message.Subject = template.SubjectText;
                    message.IsBodyHtml = true;
                    message.Body = bodyMessage;
                    smtp.Port = 587;
                    smtp.Host = "smtp.office365.com";
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("invoices@moellerip.com", "B$tam$x#36");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                }
            }
            catch (Exception ex) 
            {
                string connection = getConnection();
                EmailTemplateRepository templateRepository = new EmailTemplateRepository(connection);
                templateRepository.LogReminder(invoice, ex.Message);
            }
        }

        //aca si es service level solo le mando a paula
        //los que tengan otra cosa que lvl 2 son la excepcion que va a paula.
        private static void SendEmailOverdue(EmailTemplate template, List<InvoiceMail> invoiceList, string EmailAdress, string ClientName)
        {
            try
            {
                decimal total = 0;
                string InvoicesRows = "";
                string bodyMessage = "";
                string Currency = "";
                int ServiceLevel = 0;
                foreach (var invoice in invoiceList)
                {
                    InvoicesRows += $"<tr><td class='invoice'>{invoice.InvoiceTango}</td>" +
                        $"<td class='invoice'>{invoice.OurReference}</td>" +
                        $"<td class='invoice'>{invoice.InvoiceDate.ToString("MM/dd/yyyy")}</td>" +
                        $"<td class='invoice'>{invoice.Overdue_Days}</td>" +
                        $"<td class='invoice'>{invoice.Currency} {invoice.InvoiceAmount}</td></tr>";
                    total  += invoice.InvoiceAmount;
                    Currency = invoice.Currency;
                    ServiceLevel = invoice.ServiceLevel;
                }
                
                bodyMessage = template.TemplateText.Replace("{xx_CLIENT_NAME}", ClientName).Replace("{xx_Inovoices_rows}",InvoicesRows).Replace("{xx_TOTAL}",Currency + " " +total.ToString());
                //reemplazo el formato de comas que viene de sql para que el mail distinga varios destinatarios
                EmailAdress = EmailAdress.Replace(',', ';');
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(template.MailFrom);
                foreach (var address in EmailAdress.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.To.Add(address);
                }
                if (ServiceLevel != 2 || ServiceLevel != 20005)
                {
                    message.To.Clear();
                    message.To.Add("paula.gonzalez@moellerip.com");
                }
                message.Subject = template.SubjectText;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = bodyMessage;
                smtp.Port = 587;
                smtp.Host = "smtp.office365.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("invoices@moellerip.com", "B$tam$x#36");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
