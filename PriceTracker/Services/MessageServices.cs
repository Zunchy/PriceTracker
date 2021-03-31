/*using Microsoft.AspNetCore.Identity.UI.Services;
using PriceTracker.Areas.Identity.Pages.Account.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Base;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PriceTracker.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public AuthMessageSender(Microsoft.Extensions.Options.IOptions<Areas.Identity.Pages.Account.Services.AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public Areas.Identity.Pages.Account.Services.AuthMessageSenderOptions Options { get; }  // set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            var twilio = new Twilio.TwilioRestClient(
                Options.SID,           // Account Sid from dashboard
                Options.AuthToken);    // Auth Token

            var result = twilio.SendMessage(Options.SendNumber, number, message);
            // Use the debug output for testing without receiving a SMS message.
            // Remove the Debug.WriteLine(message) line after debugging.
            // System.Diagnostics.Debug.WriteLine(message);
            return Task.FromResult(0);
        }
    }
}
*/