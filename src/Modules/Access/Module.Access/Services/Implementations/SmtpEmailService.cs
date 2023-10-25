using System;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity;
using Module.Access.Identity;
using Module.Access.Services.Interfaces;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Module.Access.Services.Implementations
{
    public class SmtpEmailService : IEmailService
    {
        private readonly UserManager<Persona> _userManager;
        private readonly ILogger<SmtpEmailService> _logger;
        private readonly IConfiguration _configuration;

        public SmtpEmailService(UserManager<Persona> userManager,
            ILogger<SmtpEmailService> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string htmlMessage)
        {
            var apiKey = _configuration["Smtp:Username"];
            var secretKey = _configuration["Smtp:Password"];
            var user = await _userManager.FindByEmailAsync(email);

            MailjetClient client = new MailjetClient($"{apiKey}", $"{secretKey}")
            {
                //Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = SendV31.Resource,
            }
            .Property(Send.Messages, new JArray {
            new JObject {
            {
            "From",
            new JObject {
            {"Email", "abdulquddusnuhu@gmail.com"},
            {"Name", "noreply@smsabuja.com"}
            }
            }, {
            "To",
            new JArray {
            new JObject {
                {
                "Email",
                email
                }, {
                "Name",
                user.FirstName
                }
            }
            }
            }, {
            "Subject",
            subject
            }, {
            "HTMLPart",
            htmlMessage
            },
            }
                });

            _logger.LogInformation("Sending email to {0}", user.FirstName + " " + user.LastName);
            await client.PostAsync(request);
        }
    }

}

