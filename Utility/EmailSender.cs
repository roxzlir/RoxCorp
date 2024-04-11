using Microsoft.AspNetCore.Identity.UI.Services;

namespace RoxCorp.Utility
{
    public class EmailSender : IEmailSender //** Vi skapar denna för att ta oss förbi den där EmailSender som ligger kopplat, vi ärver IEmailSender och hämtar hem interfacet
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //Här kan vi lägga email's logiken/koden

            return Task.CompletedTask;
        }
    }
}
