using System;
using Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessagesTest
{
    [TestClass]
    public class ServerAliveMessageTest
    {
        [TestMethod]
        public void ServerAliveMessage_EncodeAndDecode()
        {
            var msg1 = new ServerAliveMessage();
            msg1.InitMessageAndConversationIds();
            var bytes = msg1.Encode();

            var msg2 = Message.Decode(bytes) as ServerAliveMessage;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg1.MsgId, msg2.MsgId);
            Assert.AreEqual(msg1.ConvId, msg2.ConvId);
        }
    }
}
