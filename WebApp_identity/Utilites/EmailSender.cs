using System.Net;
using System.Net.Mail;
using WebApp_identity.IRepositories;

namespace WebApp_identity.Utilites;

public class EmailSender:IEmailSender, Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("mmm810813@gmail.com", "jjhp ddxs vlud ouwy")
        };

        var mailMessage = new MailMessage(
            from: "mmm810813@gmail.com",
            to: email,
            subject: subject,
            body: message
        );
        mailMessage.IsBodyHtml = true;

        return client.SendMailAsync(mailMessage);
    }
    
}