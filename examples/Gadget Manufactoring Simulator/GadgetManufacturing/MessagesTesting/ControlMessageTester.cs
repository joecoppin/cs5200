using System;
using Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedObjects;

namespace MessagesTesting
{
    [TestClass]
    public class ControlMessageTester
    {
        [TestMethod]
        public void ControlMessage_EncodeAndDecode_Ack()
        {
            Ack msg1 = new Ack();
            msg1.InitMessageAndConversationIds();
            byte[] bytes = msg1.Encode();

            Ack msg2 = ControlMessage.Decode(bytes) as Ack;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg1.MsgId, msg2.MsgId);
            Assert.AreEqual(msg1.ConvId, msg2.ConvId);
        }

        [TestMethod]
        public void ControlMessage_EncodeAndDecode_Availability()
        {
            Availability msg1 = new Availability() {Available = 5};
            msg1.InitMessageAndConversationIds();
            byte[] bytes = msg1.Encode();

            Availability msg2 = ControlMessage.Decode(bytes) as Availability;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg1.MsgId, msg2.MsgId);
            Assert.AreEqual(msg1.ConvId, msg2.ConvId);
            Assert.AreEqual(msg1.Available, msg2.Available);
        }

        [TestMethod]
        public void ControlMessage_EncodeAndDecode_GetResource()
        {
            GetResource msg1 = new GetResource() { NumberNeeded = 5, ResourceType = ResourceType.Gadget, TargetMilliseconds = 100};
            msg1.InitMessageAndConversationIds();
            byte[] bytes = msg1.Encode();

            GetResource msg2 = ControlMessage.Decode(bytes) as GetResource;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg1.MsgId, msg2.MsgId);
            Assert.AreEqual(msg1.ConvId, msg2.ConvId);
            Assert.AreEqual(msg1.NumberNeeded, msg2.NumberNeeded);
            Assert.AreEqual(msg1.ResourceType, msg2.ResourceType);
            Assert.AreEqual(msg1.TargetMilliseconds, msg2.TargetMilliseconds);
        }

        [TestMethod]
        public void ControlMessage_EncodeAndDecode_Nak()
        {
            Nak msg1 = new Nak() { Error = new Error() { Text = @"Test Error Text"}};
            msg1.InitMessageAndConversationIds();
            byte[] bytes = msg1.Encode();

            Nak msg2 = ControlMessage.Decode(bytes) as Nak;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg1.MsgId, msg2.MsgId);
            Assert.AreEqual(msg1.ConvId, msg2.ConvId);
            Assert.IsNotNull(msg2.Error);
            Assert.AreEqual(msg1.Error.Text, msg2.Error.Text);
        }

        [TestMethod]
        public void ControlMessage_EncodeAndDecode_ProcessInfo()
        {
            ProcessInfo msg1 = new ProcessInfo() { ProcessId = 100, GroupMulticast = "224.5.6.0" };
            msg1.InitMessageAndConversationIds();
            byte[] bytes = msg1.Encode();

            ProcessInfo msg2 = ControlMessage.Decode(bytes) as ProcessInfo;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg1.MsgId, msg2.MsgId);
            Assert.AreEqual(msg1.ConvId, msg2.ConvId);
            Assert.AreEqual(msg1.ProcessId, msg2.ProcessId);
            Assert.AreEqual(msg1.GroupMulticast, msg2.GroupMulticast);
        }

        [TestMethod]
        public void ControlMessage_EncodeAndDecode_Register()
        {
            Register msg1 = new Register() { ProcessType = ProcessType.Monitor };
            msg1.InitMessageAndConversationIds();
            byte[] bytes = msg1.Encode();

            Register msg2 = ControlMessage.Decode(bytes) as Register;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg1.MsgId, msg2.MsgId);
            Assert.AreEqual(msg1.ConvId, msg2.ConvId);
            Assert.AreEqual(msg1.ProcessType, msg2.ProcessType);
        }

        [TestMethod]
        public void ControlMessage_EncodeAndDecode_ReserveResource()
        {
            ReserveResource msg1 = new ReserveResource() { NumberToReserve = 5};
            msg1.InitMessageAndConversationIds();
            byte[] bytes = msg1.Encode();

            ReserveResource msg2 = ControlMessage.Decode(bytes) as ReserveResource;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg1.MsgId, msg2.MsgId);
            Assert.AreEqual(msg1.ConvId, msg2.ConvId);
            Assert.AreEqual(msg1.NumberToReserve, msg2.NumberToReserve);
        }

        [TestMethod]
        public void ControlMessage_EncodeAndDecode_SetupExchange()
        {
            SetupExchange msg1 = new SetupExchange() { TcpEndPoint = new PublicEndPoint() { HostAndPort = "127.0.0.1:4000"} };
            msg1.InitMessageAndConversationIds();
            byte[] bytes = msg1.Encode();

            SetupExchange msg2 = ControlMessage.Decode(bytes) as SetupExchange;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg1.MsgId, msg2.MsgId);
            Assert.AreEqual(msg1.ConvId, msg2.ConvId);
            Assert.AreEqual(msg1.TcpEndPoint, msg2.TcpEndPoint);
        }

        [TestMethod]
        public void ControlMessage_EncodeAndDecode_Shutdown()
        {
            Shutdown msg1 = new Shutdown();
            msg1.InitMessageAndConversationIds();
            byte[] bytes = msg1.Encode();

            Shutdown msg2 = ControlMessage.Decode(bytes) as Shutdown;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg1.MsgId, msg2.MsgId);
            Assert.AreEqual(msg1.ConvId, msg2.ConvId);
        }

        [TestMethod]
        public void ControlMessage_EncodeAndDecode_Status()
        {
            Status msg1 = new Status() { /* TODO */ };
            msg1.InitMessageAndConversationIds();
            byte[] bytes = msg1.Encode();

            Status msg2 = ControlMessage.Decode(bytes) as Status;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg1.MsgId, msg2.MsgId);
            Assert.AreEqual(msg1.ConvId, msg2.ConvId);
        }

        [TestMethod]
        public void ControlMessage_EncodeAndDecode_SubscribeMonitor()
        {
            SubscribeMonitor msg1 = new SubscribeMonitor();
            msg1.InitMessageAndConversationIds();
            byte[] bytes = msg1.Encode();

            SubscribeMonitor msg2 = ControlMessage.Decode(bytes) as SubscribeMonitor;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg1.MsgId, msg2.MsgId);
            Assert.AreEqual(msg1.ConvId, msg2.ConvId);
        }

        [TestMethod]
        public void ControlMessage_EncodeAndDecode_Unsubscribe()
        {
            UnsubscribeMonitor msg1 = new UnsubscribeMonitor();
            msg1.InitMessageAndConversationIds();
            byte[] bytes = msg1.Encode();

            UnsubscribeMonitor msg2 = ControlMessage.Decode(bytes) as UnsubscribeMonitor;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg1.MsgId, msg2.MsgId);
            Assert.AreEqual(msg1.ConvId, msg2.ConvId);
        }
    }
}
