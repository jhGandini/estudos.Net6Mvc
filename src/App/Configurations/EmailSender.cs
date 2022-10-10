using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Options;
using System.Net;

namespace App.Configurations;

public class EmailSender : IEmailSender
{
    private string UserName;
    private string Password;
    private string Remetente;
    private string Nome;
    private string Smtp;
    private string SmtpPorta;
    private string HeaderSender;

    public EmailSender(string remetente, string nome, string smtp, string smtpPorta, string headerSender)
    {
        Remetente = remetente;
        Nome = nome;
        Smtp = smtp;
        SmtpPorta = smtpPorta;
        HeaderSender = headerSender;
        UserName = null;
        Password = null;
    }

    public EmailSender(string remetente, string nome, string smtp, string smtpPorta, string headerSender, string userName, string password)
    {
        Remetente = remetente;
        Nome = nome;
        Smtp = smtp;
        SmtpPorta = smtpPorta;
        HeaderSender = headerSender;
        UserName = userName;
        Password = password;
    }
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        //var client = new SmtpClient(host, port)
        //{
        //    Credentials = new NetworkCredential(userName, password),
        //    EnableSsl = enableSSL
        //};
        //return client.SendMailAsync(
        //    new MailMessage(userName, email, subject, htmlMessage) { IsBodyHtml = true }
        //);


        return EnviarEmail(email, subject, htmlMessage, null, null);


    }

    public async Task EnviarEmail(string emails, string titulo, string corpo, byte[] atachment, string atachmentName)
    {        
        MailMessage mailMessage = new MailMessage
        {
            From = new MailAddress(Remetente,Nome),
            BodyEncoding = UTF8Encoding.UTF8,
            IsBodyHtml = true
        };

        mailMessage.Headers.Add("X-Sender", HeaderSender);
        mailMessage.Subject = titulo;
        mailMessage.Body = corpo;

        SmtpClient client = new SmtpClient(Smtp, int.Parse(SmtpPorta));

        if (UserName != null)
        {            
            client.Credentials = new NetworkCredential(UserName, Password);
            client.EnableSsl = true;
        }                

        if (atachment != null)
        {
            var stream = new MemoryStream(atachment);
            mailMessage.Attachments.Add(new Attachment(stream, atachmentName));
        }


        try
        {
            mailMessage.To.Clear();
            mailMessage.To.Add(emails);
            await client.SendMailAsync(mailMessage);
        }
        catch(Exception ex)
        {
            return;
        }
    }
}
