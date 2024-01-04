using MimeKit;
using MimeKit;
using MailKit.Net.Smtp;

namespace Core.Services;

public class MailService
{
    private const string FARMAKARIYER_EMAIL = "info@farmakariyer.net";
    private const string PASSWORD = "fhao oept tpkw cunz";

    public void TestMessage()
    {
        string to = "info@unalmustafa.com";

        string subject = "Konu Başlığı";
        string body = "Merhaba, bu kurumsal e-posta aracılığı ile gönderilen bir dijital test mesajıdır.";

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(FARMAKARIYER_EMAIL));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        client.Connect("smtp.gmail.com", 465, true);

        client.Authenticate(FARMAKARIYER_EMAIL, PASSWORD);
        client.Send(message);
        client.Disconnect(true);
    }

    public void SendMail(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(FARMAKARIYER_EMAIL));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        client.Connect("smtp.gmail.com", 465, true);

        client.Authenticate(FARMAKARIYER_EMAIL, PASSWORD);
        client.Send(message);
        client.Disconnect(true);
    }
}
