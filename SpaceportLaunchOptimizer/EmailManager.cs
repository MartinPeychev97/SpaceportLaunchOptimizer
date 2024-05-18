using System.Net.Mail;

namespace SpaceportLaunchOptimizer
{
    public class EmailManager
    {
        public static void SendEmailWithAttachment(string sender, string password, string receiverEmail, Attachment attachment, KeyValuePair<string, WeatherData> keyValues)
        {
            var client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(sender, password);
            client.Port = 587;
            client.Host = "smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;

            var mail = new MailMessage();
            mail.From = new MailAddress(sender, "From: ");
            mail.To.Add(new MailAddress(receiverEmail));
            mail.Subject = "Launch Analysis Report";
            mail.Body = $"Please find the attached launch analysis report.\n The Best Space port is: {keyValues.Key}.\n The best launch date is the {keyValues.Value.Date} of July";

            mail.Attachments.Add(attachment);
            client.Send(mail);
        }
    }
}
