using System.Threading;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CommSubsystem;
using Messages;
using SharedObjects;

namespace CommSubsystemTest
{
    [TestClass]
    public class UdpCommunicatorTest
    {
        private Envelope _lastIncomingEnvelope1;
        private Envelope _lastIncomingEnvelope2;

        [TestMethod]
        public void UdpCommunicator_SimpleSendAndReceive()
        {
            LocalProcessInfo.Instance.ProcessId = 10;

            var comm1 = new UdpCommunicator()
            {
                MinPort = 10000,
                MaxPort = 10999,
                Timeout = 1000,
                EnvelopeHandler = ProcessEnvelope1
            };

            comm1.Start();

            var comm2 = new UdpCommunicator()
            {
                MinPort = 10000,
                MaxPort = 10999,
                Timeout = 1000,
                EnvelopeHandler = ProcessEnvelope2
            };
            comm2.Start();

            var targetEndPoint = new IPEndPoint(IPAddress.Loopback, comm2.Port);

            var msg = new ProgressStatus() { PercentComplete = 0.75F };
            msg.InitMessageAndConversationIds();
            var env = new Envelope() { Message = msg, EndPoint = targetEndPoint };

            comm1.Send(env);

            Thread.Sleep(100);

            Assert.IsNotNull(_lastIncomingEnvelope2);
            Assert.IsNotNull(_lastIncomingEnvelope2.Message);
            Assert.AreEqual(msg.MsgId, _lastIncomingEnvelope2.Message.MsgId);
            Assert.AreEqual(msg.ConvId, _lastIncomingEnvelope2.Message.ConvId);
            var msg2 = _lastIncomingEnvelope2.Message as ProgressStatus;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg.PercentComplete, msg2.PercentComplete);
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
