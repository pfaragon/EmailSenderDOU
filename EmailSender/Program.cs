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
            EmailTemplateRepository templateRepository = new EmailTemplateRepository("Data Source=WS_603;Initial Catalog=Patricia_MoellerIp;Integrated Security=True");
            EmailTemplate emailTemplateReminder = templateRepository.GetTemplate("sp_moe_0011_Reminder_EmailTemplate");
            EmailTemplate emailTemplateOverdue = templateRepository.GetTemplate("sp_moe_0012_Overdue_Days_EmailTemplate");

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
                            ValidationSendEmail(listSameAccount, emailTemplateReminder, emailTemplateOverdue);
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
                            ValidationSendEmail(listSameAccount,emailTemplateReminder, emailTemplateOverdue);
                        }
                        //si no es la misma cuenta que el anteultimo tengo que realizar la accion desde aca con un elemento pero tambien enviar el anterior porque estaba en espera
                        else
                        {
                            ValidationSendEmail(listSameAccount, emailTemplateReminder, emailTemplateOverdue);
                            List<InvoiceMail> listLastInvoice = new List<InvoiceMail>();
                            listLastInvoice.Add(invoice);
                            ValidationSendEmail(listLastInvoice, emailTemplateReminder, emailTemplateOverdue);
                        }
                    }
                }
                count++;
            }
        }

        private static void ValidationSendEmail(List<InvoiceMail> listSameAccount, EmailTemplate emailTemplateReminder, EmailTemplate emailTemplateOverdue)
        {
            string EmailAddress = "";
            string clientName = "";
            foreach (var invoiceMail in listSameAccount)
            {
                if (invoiceMail.Email != "")
                {
                    EmailAddress = invoiceMail.Email;
                    clientName = invoiceMail.AccountName;
                    if (invoiceMail.Overdue_Days == -7)
                    {
                       SendEmailReminder(emailTemplateReminder, invoiceMail);
                       //lo quito de la lista para que en overdue no se mande de nuevo
                       listSameAccount.Remove(invoiceMail);
                    }
                }
            }
            if (DateTime.Now.Day == 9 || DateTime.Now.Day == 15)
            {
                SendEmailOverdue(emailTemplateOverdue, listSameAccount, EmailAddress, clientName);
            }
        }

        private static string getConnection()
        {
            string connectionCoded = ConfigurationManager.ConnectionStrings["ConexionMoreProd"].ConnectionString;
            return Encoding.Default.GetString(Convert.FromBase64String(Encoding.Default.GetString(Convert.FromBase64String(connectionCoded))));
        }

        private static void SendEmailReminder(EmailTemplate template, InvoiceMail invoice)
        {
            try
            {
                string bodyMessage = template.TemplateText;
                //reemplazo el formato de comas que viene de sql para que el mail distinga varios destinatarios
                invoice.Email = invoice.Email.Replace(',', ';');
                bodyMessage = bodyMessage.Replace("{xx_CLIENT_NAME}", invoice.AccountName)
                    .Replace("{xx_INVOICE#}", invoice.InvoiceTango.ToString())
                    .Replace("{xx_INVOICE_AMOUNT}", invoice.Currency+" "+invoice.InvoiceAmount);
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(template.MailFrom);
                foreach (var address in invoice.Email.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.To.Add(address);
                }
                message.Subject = template.SubjectText;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = bodyMessage;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new NetworkCredential("FromMailAddress", "password");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
        private static void SendEmailOverdue(EmailTemplate template, List<InvoiceMail> invoiceList, string EmailAdress, string ClientName)
        {
            try
            {
                decimal total = 0;
                string InvoicesRows = "";
                string bodyMessage = "";
                string Currency = "";
                string logo = Directory.GetCurrentDirectory() + "\\MIP_Logo_RGB_6000px_9.png";
                foreach (var invoice in invoiceList)
                {
                    InvoicesRows += $"<tr><td class='invoice'>{invoice.InvoiceTango}</td>" +
                        $"<td class='invoice'>{invoice.OurReference}</td>" +
                        $"<td class='invoice'>{invoice.InvoiceDate.ToString("MM/dd/yyyy")}</td>" +
                        $"<td class='invoice'>{invoice.Overdue_Days}</td>" +
                        $"<td class='invoice'>{invoice.Currency} {invoice.InvoiceAmount}</td></tr>";
                    total  += invoice.InvoiceAmount;
                    Currency = invoice.Currency;
                }
                
                bodyMessage = template.TemplateText.Replace("{xx_CLIENT_NAME}", ClientName).Replace("{xx_Inovoices_rows}",InvoicesRows).Replace("{xx_TOTAL}",Currency + " " +total.ToString());
                //reemplazo el formato de comas que viene de sql para que el mail distinga varios destinatarios
                EmailAdress = "julian.perez@moellerip.com";
                //EmailAdress = "gabriel.biasella@moellerip.com; mariana.volpi@moellerip.com";
                EmailAdress = EmailAdress.Replace(',', ';');
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(template.MailFrom);
                foreach (var address in EmailAdress.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.To.Add(address);
                }
                message.Subject = template.SubjectText;
                message.IsBodyHtml = true; //to make message body as html  
                var htmlView = AlternateView.CreateAlternateViewFromString(bodyMessage, Encoding.UTF8, MediaTypeNames.Text.Html);
                var imgRes = new LinkedResource(logo, MediaTypeNames.Image.Jpeg);
                imgRes.ContentId = "{xx_LOGO}";
                imgRes.TransferEncoding = TransferEncoding.Base64;
                htmlView.LinkedResources.Add(imgRes);
                message.AlternateViews.Add(htmlView);
                smtp.Port = 587;
                smtp.Host = "smtp.office365.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("no-reply@moellerip.com", "Pent$xeR44)T9");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
