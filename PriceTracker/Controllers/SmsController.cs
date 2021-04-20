/*using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using System.Configuration;

using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.TwiML;
using Twilio.AspNet.Mvc;

namespace PriceTracker.Controllers
{
    public class SmsController : TwilioController
    {
        public IActionResult SendSms()
        {
            var accountSid = "AC99cf35e2ac296171dbb0d575bf511036";
            var authToken = "c36c40838a78978b76f49014f63b333d";
            TwilioClient.Init(accountSid, authToken);

            var to = new PhoneNumber("+13308313952");
            var from = new PhoneNumber("+19853268462"); // twilio appointed number for our project

            var message = MessageResource.Create(
               to: to,
               from: from,
               body: "This is a test for the eTrack PriceTracking app. Thanks for adding your phone number!");

            return (IActionResult)Content(message.Sid);
        }
    }
}
*/