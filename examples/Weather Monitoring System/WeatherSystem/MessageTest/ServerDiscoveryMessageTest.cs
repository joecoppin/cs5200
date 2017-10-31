using System;
using Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedObjects;

namespace MessagesTest
{
    [TestClass]
    public class ServerDiscoveryMessageTest
    {
        [TestMethod]
        public void ServerDiscoveryMessage_EncodeAndDecode()
        {
            var msg1 = new ServerDiscoveryMessage();
            msg1.InitMessageAndConversationIds();
            var bytes = msg1.Encode();

            var msg2 = Message.Decode(bytes) as ServerDiscoveryMessage;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg1.MsgId, msg2.MsgId);
            Assert.AreEqual(msg1.ConvId, msg2.ConvId);
        }
    }
}
