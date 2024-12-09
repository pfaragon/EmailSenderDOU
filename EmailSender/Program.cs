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
using SelectPdf;

namespace EmailSender
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Lista de valores de monthsBefore
            int[] monthsBeforeValues = { 1, 3, 6 };

            foreach (var monthsBefore in monthsBeforeValues)
            {
                Console.WriteLine($"Inicio de procesamiento para monthsBefore = {monthsBefore}");

                // Creo la conexión a la base de datos.
                EmailTemplateRepository templateRepository = new EmailTemplateRepository(GetConnection());
                Console.WriteLine("La conexión a la base de datos ha sido exitosa.");

                // Obtengo el listado de renovaciones según el parámetro indicado
                List<RenewalDetail> renewalsList = templateRepository.GetRenewals(monthsBefore);
                Console.WriteLine($"Se encontraron {renewalsList.Count} renovaciones para monthsBefore = {monthsBefore}.");

                // Lista para agrupar las renovaciones que van en el mismo email
                List<RenewalDetail> sameEmailRenewalsList = new List<RenewalDetail>();
                RenewalDetail lastReviewed = null;
                int count = 1;

                // Agrupo las renovaciones que van en el mismo email de acuerdo al CaseCountry, AgentNameId y ApplicantNameId
                foreach (var renewal in renewalsList)
                {
                    if (lastReviewed == null)
                    {
                        lastReviewed = renewal;
                        sameEmailRenewalsList.Add(renewal);
                    }

                    if (count != renewalsList.Count)
                    {
                        if ((lastReviewed.AgentNameId == renewal.AgentNameId) &&
                            (lastReviewed.ApplicantNameId == renewal.ApplicantNameId) &&
                            (lastReviewed.CountryName == renewal.CountryName))
                        {
                            if (count != 1)
                            {
                                sameEmailRenewalsList.Add(renewal);
                            }
                        }
                        else
                        {
                            ValidationSendEmail(sameEmailRenewalsList, monthsBefore, templateRepository);
                            sameEmailRenewalsList.Clear();
                            sameEmailRenewalsList.Add(renewal);
                            lastReviewed = renewal;
                        }
                    }
                    else
                    {
                        if ((lastReviewed.AgentNameId == renewal.AgentNameId) &&
                            (lastReviewed.ApplicantNameId == renewal.ApplicantNameId) &&
                            (lastReviewed.CountryName == renewal.CountryName))
                        {
                            if (count != 1)
                            {
                                sameEmailRenewalsList.Add(renewal);
                            }
                            ValidationSendEmail(sameEmailRenewalsList, monthsBefore, templateRepository);
                        }
                        else
                        {
                            ValidationSendEmail(sameEmailRenewalsList, monthsBefore, templateRepository);

                            List<RenewalDetail> listLastRenewal = new List<RenewalDetail>();
                            listLastRenewal.Add(renewal);
                            ValidationSendEmail(listLastRenewal, monthsBefore, templateRepository);
                        }
                    }
                    count++;
                }

                Console.WriteLine($"Procesamiento para monthsBefore = {monthsBefore} completado.");
            }

            // Indica que el programa ha finalizado y cierra
            Console.WriteLine("Todos los procesos se han completado. Presiona cualquier tecla para salir.");
            Console.ReadKey();
        }

        private static string GetConnection()
        {
            string connectionCoded = ConfigurationManager.ConnectionStrings["ConexionMoreProd"].ConnectionString;
            return Encoding.Default.GetString(Convert.FromBase64String(Encoding.Default.GetString(Convert.FromBase64String(connectionCoded))));
        }

        //Valido los datos de la lista a enviar el email
        private static void ValidationSendEmail(List<RenewalDetail> renewalList, int monthsBefore, EmailTemplateRepository repository)
        {
            // Variables inicializadas con valores predeterminados para manejar posibles nulls
            string correspondenceAddressEmails = renewalList[0]?.CorrespondenceAddressEmails ?? "";
            string agentName = renewalList[0]?.AgentName ?? "";
            string applicantName = renewalList[0]?.ApplicantName ?? "";
            string correspondenceAddressName = renewalList[0]?.CorrespondenceAddressName ?? "";
            string countryId = renewalList[0]?.CountryId ?? "";
            string countryName = renewalList[0]?.CountryName ?? "";
            int languageId;

            switch (renewalList[0].CorrespondenceAddressLanguageId)
            {
                case 6: // español 
                    languageId = 6;
                    break;
                default: // el resto en ingles
                    languageId = 3;
                    break;
            }

            // Obtener las plantillas de email
            EmailTemplate emailTemplate = repository.GetTemplate("EMAIL", languageId, countryId, monthsBefore);
            EmailTemplate attachTemplate = repository.GetTemplate("ATTACH", languageId);

            Console.WriteLine("Email Template: {0}", emailTemplate?.TemplateName ?? "No disponible");

            // Validación de email
            if (!string.IsNullOrWhiteSpace(correspondenceAddressEmails))
            {
                // Llamada al método de envío de email
                SendEmail(repository, emailTemplate, attachTemplate, renewalList, correspondenceAddressEmails, agentName, applicantName, correspondenceAddressName, countryName);
                Console.WriteLine("Email enviado correctamente.");
            }
            else
            {
                // Log y mensaje para email vacío
                repository.LogEmailSent(
                    renewalList,
                    "EMPTY_EMAIL",
                    "El email no pudo ser enviado. No hay dirección de email.",
                    emailTemplate?.SubjectText?.Replace("{xx_Country}", countryName.ToUpper()).Replace("{xx_Applicant}", applicantName.ToUpper()) ?? "Sin asunto"
                );
                Console.WriteLine("El email no pudo ser enviado. No hay dirección de email.");
            }
        }



        private static void SendEmail(EmailTemplateRepository repository,
            EmailTemplate emailTemplate,
            EmailTemplate attachTemplate,
            List<RenewalDetail> renewalList,
            string correspondenceAddressEmails,
            string agentName,
            string applicantName,
            string correspondenceAddressName,
            string countryName)
        {
            try
            {
                string emailBody = String.Empty;
                string emailBodylRows = String.Empty;

                string attachBody = String.Empty;
                string attachBodyRows = String.Empty;

                foreach (var renewal in renewalList)
                {
                    emailBodylRows +=
                        $"<tr>" +
                            $"<td class='invoice'>{renewal.Catchword}</td>" +
                            $"<td>{renewal.ApplicationNumber}</td>" +
                            $"<td>{renewal.RegistrationNumber}</td>" +
                            $"<td>{renewal.NextRenewal.ToString("dd/MM/yyyy")}</td>" +
                            $"<td>{renewal.Classes}</td>" +
                            $"<td>{renewal.YourReference}</td>" +
                            $"<td>{renewal.CaseNumber}</td>" +
                        $"</tr>";

                    attachBodyRows +=
                        $"<tr>" +
                            $"<td class='invoice'>{renewal.Catchword}</td>" +
                            $"<td style='text-align: center;'>{GetDataURL(renewal.DeviceFilePath)}</td>" +
                            $"<td>{renewal.ApplicationNumber}</td>" +
                            $"<td>{renewal.RegistrationNumber}</td>" +
                            $"<td>{renewal.NextRenewal.ToString("dd/MM/yyyy")}</td>" +
                            $"<td>{renewal.Classes}</td>" +
                            $"<td>{renewal.YourReference}</td>" +
                            $"<td>{renewal.CaseNumber}</td>" +
                        $"</tr>" +
                        $"<tr><td colspan='8'>{renewal.ClassesDescription}</td></tr>";
                }

                emailBody = emailTemplate.TemplateText
                    .Replace("{xx_Emails}", correspondenceAddressEmails.Replace(',', ';'))
                    .Replace("{xx_Cases}", emailBodylRows)
                    .Replace("{xx_Agent}", agentName)
                    .Replace("{xx_Applicant}", applicantName)
                    .Replace("{xx_CorrespondenceAddress}", correspondenceAddressName);

                attachBody = attachTemplate.TemplateText
                    .Replace("{xx_Applicant}", applicantName.ToUpper())
                    .Replace("{xx_Cases}", attachBodyRows);

                //reemplazo el formato de comas que viene de sql para que el mail distinga varios destinatarios
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                message.From = new MailAddress(emailTemplate.MailFrom);

                message.To.Clear();

                foreach (var address in correspondenceAddressEmails.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.To.Add(address);
                }

                //message.To.Add("pablo.aragon@moellerip.com");
                //message.Bcc.Add("pablo.aragon@moellerip.com");
                //message.To.Add("liliana.bajos@moellerip.com");

                message.Subject = emailTemplate.SubjectText.Replace("{xx_Country}", countryName.ToUpper()).Replace("{xx_Applicant}", applicantName.ToUpper());
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = emailBody;

                //Creo el documento y lo adjunto al email
                MemoryStream attachFile = HtmlToPdf(attachBody);
                message.Attachments.Add(new Attachment(attachFile, attachTemplate.SubjectText.Replace("{xx_Applicant}", applicantName.ToUpper()) + ".pdf"));

                smtp.Port = 587;
                smtp.Host = "smtp.office365.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("no-reply@moellerip.com", "Pent$xeR44");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                //Creo el email con el adjunto en un archivo

                string emailFileName = attachTemplate.SubjectText.Replace("{xx_Applicant}", applicantName.ToUpper()) + DateTime.Now.ToString(String.Format("_yyyyMMdd_HHmmss_ffff")) + ".eml";

                foreach (var renewal in renewalList)
                {
                    string filePath = Path.Combine(renewal.DocumentsPath, emailFileName);
                    FileInfo file = new FileInfo(filePath);

                    string emailFile = message.ToEml();
                    byte[] emailFileBytes = System.Text.Encoding.UTF8.GetBytes(emailFile);

                    WriteFile(file, emailFileBytes);
                    repository.InsertPatriciaDocument(renewal.CaseId, message.Subject, file.Name);

                }

                smtp.Send(message);

                repository.LogEmailSent(renewalList, "SUCCESS", "El email fue enviado con exito.", emailTemplate.SubjectText.Replace("{xx_Country}", countryName.ToUpper()).Replace("{xx_Applicant}", applicantName.ToUpper()), emailFileName);
            }
            catch (Exception ex)
            {
                repository.LogEmailSent(renewalList, "ERROR", ex.Message, emailTemplate.SubjectText.Replace("{xx_Country}", countryName.ToUpper()).Replace("{xx_Applicant}", applicantName.ToUpper()));
                Console.WriteLine(ex.Message);
            }
        }

        private static MemoryStream HtmlToPdf(string html)
        {
            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(html);
            MemoryStream pdfStream = new MemoryStream();

            doc.Save(pdfStream);
            pdfStream.Position = 0;
            doc.Close();
            return pdfStream;
        }

        private static void WriteFile(FileInfo file, byte[] fileBytes)
        {
            file.Directory.Create(); // If the directory already exists, this method does nothing.
            File.WriteAllBytes(file.FullName, fileBytes);
        }

        public static string GetDataURL(string deviceFilePath)
        {
            FileInfo file = new FileInfo(deviceFilePath);

            if (file.Name == string.Empty)
            {
                return " - ";
            }
            else
            {
                return "<img style='display: block; max-width: 100px; max-height: 100px; width: auto; height: auto; ' src=\"data:image/"
                            + Path.GetExtension(deviceFilePath).Replace(".", "")
                            + ";base64,"
                            + Convert.ToBase64String(File.ReadAllBytes(deviceFilePath)) + "\" />";
            }
        }
    }
}

