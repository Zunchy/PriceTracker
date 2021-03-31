using System;

namespace Twilio
{
    internal class TwilioRestClient
    {
        private string sID;
        private string authToken;

        public TwilioRestClient(string sID, string authToken)
        {
            this.sID = sID;
            this.authToken = authToken;
        }

        internal object SendMessage(string sendNumber, string number, string message)
        {
            throw new NotImplementedException();
        }
    }
}