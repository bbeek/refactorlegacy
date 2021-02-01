using System;
using System.Net;
using System.Net.Mail;

namespace SubclassAndOverride
{
    public class MessageForwarder
    {
        private const string MAILSERVER = "Something";

        public bool SendMessage(MailMessage message)
        {
            var forwardedMessage = CreateForwardMessage(message);

            #region More obscure logic









            #endregion

            return forwardedMessage != null;
        }

        private MailMessage CreateForwardMessage(MailMessage message)
        {
            var forward = new MailMessage();
            forward.From = message.From;
            BuildReplyTo(forward);
            BuildReceivers(message, forward);

            BuildForwardContent(message, forward);

            //Send the message.
            SmtpClient client = new SmtpClient(MAILSERVER);

            client.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                return null;
            }

            return forward;
        }


        #region More logic
        private void BuildReplyTo(MailMessage forward)
        {
            foreach (var replyTo in GetReplyToList())
            {
                forward.ReplyToList.Add(replyTo);
            }
        }

        private void BuildReceivers(MailMessage message, MailMessage forward)
        {
            foreach (var to in message.To)
            {
                forward.To.Add(to);
            }
            foreach (var bcc in message.Bcc)
            {
                forward.Bcc.Add(bcc);
            }
        }
        private void BuildForwardContent(MailMessage message, MailMessage forward)
        {
            throw new NotImplementedException();
        }

        private MailAddressCollection GetReplyToList()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
