using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CommSub;
using Messages;
using SharedObjects;

namespace CommSubTesting
{
    [TestClass]
    public class UdpCommunicatorTester
    {
        private Envelope _lastIncomingEnvelope1;
        private Envelope _lastIncomingEnvelope2;

        [TestMethod]
        public void UdpCommunicator_TestSendAndReceive()
        {
            LocalProcessInfo.Instance.ProcessId = 10;

            UdpCommunicator comm1 = new UdpCommunicator()
            {
                MinPort = 10000,
                MaxPort = 10999,
                Timeout = 1000,
                EnvelopeHandler = ProcessEnvelope1
            };

            comm1.Start();

            UdpCommunicator comm2 = new UdpCommunicator()
            {
                MinPort = 10000,
                MaxPort = 10999,
                Timeout = 1000,
                EnvelopeHandler = ProcessEnvelope2
            };
            comm2.Start();

            PublicEndPoint targetEndPoint = new PublicEndPoint() {Host = "127.0.0.1", Port = comm2.Port};

            ProcessInfo msg = new ProcessInfo() { ProcessId = 100 };
            msg.InitMessageAndConversationIds();
            Envelope env = new Envelope() {Message = msg, EndPoint = targetEndPoint};

            comm1.Send(env);

            Thread.Sleep(100);

            Assert.IsNotNull(_lastIncomingEnvelope2);
            Assert.IsNotNull(_lastIncomingEnvelope2.Message);
            Assert.AreEqual(msg.MsgId, _lastIncomingEnvelope2.Message.MsgId);
            Assert.AreEqual(msg.ConvId, _lastIncomingEnvelope2.Message.ConvId);
            ProcessInfo msg2 = _lastIncomingEnvelope2.Message as ProcessInfo;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg.ProcessId, msg2.ProcessId);
        }

        private void ProcessEnvelope1(Envelope env)
        {
            _lastIncomingEnvelope1 = env;
        }

        private void ProcessEnvelope2(Envelope env)
        {
            _lastIncomingEnvelope2 = env;
        }

    }
}
