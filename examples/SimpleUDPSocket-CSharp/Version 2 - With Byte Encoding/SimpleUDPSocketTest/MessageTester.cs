using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SimpleUDPSocket;

namespace SimpleUDPSocketTest
{
    [TestClass]
    public class MessageTester
    {
        [TestMethod]
        public void Message_TestEncoding()
        {
            var msg = new Message()
            {
                Id = 1,
                Text = "ABC",
                Timestamp = DateTime.Parse("2017-09-01 10:04:03")
            };

            byte[] bytes = msg.Encode();

            Assert.AreEqual(0, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
            Assert.AreEqual(1, bytes[3]);

            Message msg2 = Message.Decode(bytes);
            Assert.AreEqual(msg.Id, msg2.Id);
            Assert.AreEqual(msg.Timestamp, msg2.Timestamp);
            Assert.AreEqual(msg.Text, msg2.Text);

        }
    }
}
