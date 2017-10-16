using System;
using System.Net.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleUDPSocket;

namespace SimpleUDPSocketTest
{
    [TestClass]
    public class MessageTest
    {
        [TestMethod]
        public void Message_TestBigEndianEncoding()
        {
            Message msg = new Message()
            {
                    Id = 1,
                    Text = "ABC",
                    Timestamp = DateTime.MinValue.AddMilliseconds(1)
            };

            byte[] msgBytes = msg.Encode();

            Assert.AreEqual(4+8+8, msgBytes.Length);

            // Id
            Assert.AreEqual(0, msgBytes[0]);
            Assert.AreEqual(0, msgBytes[1]);
            Assert.AreEqual(0, msgBytes[2]);
            Assert.AreEqual(1, msgBytes[3]);

            // Timestamp (1 ms = 39*256 + 16 ticks
            Assert.AreEqual(0, msgBytes[4]);
            Assert.AreEqual(0, msgBytes[5]);
            Assert.AreEqual(0, msgBytes[6]);
            Assert.AreEqual(0, msgBytes[7]);
            Assert.AreEqual(0, msgBytes[8]);
            Assert.AreEqual(0, msgBytes[9]);
            Assert.AreEqual(39, msgBytes[10]);
            Assert.AreEqual(16, msgBytes[11]);

            // Text
            Assert.AreEqual(0, msgBytes[12]);
            Assert.AreEqual(6, msgBytes[13]);
            Assert.AreEqual(0, msgBytes[14]);
            Assert.AreEqual(65, msgBytes[15]);
            Assert.AreEqual(0, msgBytes[16]);
            Assert.AreEqual(66, msgBytes[17]);
            Assert.AreEqual(0, msgBytes[18]);
            Assert.AreEqual(67, msgBytes[19]);            
        }

        [TestMethod]
        public void Message_TestEncodeAndDecodeWithSimpleMessage()
        {
            Message msg1 = new Message()
            {
                Id = 1,
                Text = "ABCDEFG",
                Timestamp = DateTime.Now
            };

            byte[] msgBytes = msg1.Encode();

            Message msg2 = Message.Decode(msgBytes);

            Assert.AreEqual(msg1.Id, msg2.Id);
            Assert.AreEqual(msg1.Timestamp, msg2.Timestamp);
            Assert.AreEqual(msg1.Text, msg2.Text);
        }

        [TestMethod]
        public void Message_TestEncodeAndDecodeWithEmpyText()
        {
            Message msg1 = new Message()
            {
                Id = 1,
                Text = "",
                Timestamp = DateTime.Now
            };

            byte[] msgBytes = msg1.Encode();

            Message msg2 = Message.Decode(msgBytes);

            Assert.AreEqual(msg1.Id, msg2.Id);
            Assert.AreEqual(msg1.Timestamp, msg2.Timestamp);
            Assert.AreEqual(msg1.Text, msg2.Text);
        }

        [TestMethod]
        public void Message_TestEncodeAndDecodeWithNullText()
        {
            Message msg1 = new Message()
            {
                Id = 1,
                Text = "",
                Timestamp = DateTime.Now
            };

            byte[] msgBytes = msg1.Encode();

            Message msg2 = Message.Decode(msgBytes);

            Assert.AreEqual(msg1.Id, msg2.Id);
            Assert.AreEqual(msg1.Timestamp, msg2.Timestamp);
            Assert.AreEqual(msg1.Text, msg2.Text);
        }

        [TestMethod]
        public void Message_TestEncodeAndDecodeWithLotsOfTimes()
        {
            Message msg1 = new Message()
            {
                Id = 1,
                Text = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ",
                Timestamp = DateTime.Now
            };

            byte[] msgBytes = msg1.Encode();

            Message msg2 = Message.Decode(msgBytes);

            Assert.AreEqual(msg1.Id, msg2.Id);
            Assert.AreEqual(msg1.Timestamp, msg2.Timestamp);
            Assert.AreEqual(msg1.Text, msg2.Text);
        }


    }
}
