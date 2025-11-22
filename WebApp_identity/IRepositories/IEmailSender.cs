namespace WebApp_identity.IRepositories;
using Microsoft.AspNetCore.Identity.UI.Services;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}