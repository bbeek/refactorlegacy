using FluentAssertions;
using SubclassAndOverride;
using System;
using System.Net.Mail;
using Xunit;

namespace SubclassAndOverrideTests
{
    public class MessageForwarderTests
    {
        [Fact]
        public void SendMessage_not_forwarded_message_should_return_false()
        {
            var message = new MailMessage();

            var sut = new MessageForwarder();

            var outcome = sut.SendMessage(message);

            outcome.Should().BeFalse();
        }
    }
}
