using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AppShared.Conversations.InitiatorConversations;
using CommSub;
using Messages;
using SharedObjects;

namespace AppSharedTesting.Conversations.InitiatorConversations
{
    [TestClass]
    public class RegistrationTester
    {
        private readonly object _testLock = new object();

        [TestMethod]
        public void Registration_Initiator_NormalScenario()
        {
            lock (_testLock)
            {
                LocalProcessInfo.Instance.ProcessId = 0;

                // Setup a mock registry (just a receiver)
                UdpClient mockRegistry = new UdpClient(0) {Client = {ReceiveTimeout = 1000}};

                // Setup a mock app process
                RuntimeOptions options = new RuntimeOptions(((IPEndPoint) mockRegistry.Client.LocalEndPoint).Port);
                MockAppProcess mockAppProcess = new MockAppProcess()
                {
                    Options = new RuntimeOptions(12000),
                    Label = "Mock"
                };
                mockAppProcess.Start();
                Assert.AreEqual(CommProcessState.PossibleState.Initialized, mockAppProcess.State);

                // Create a registration
                var conv = mockAppProcess.MyCommSubsystem.CreateFromConversationType<Registration>();
                Assert.IsNotNull(conv);
                conv.RemotEndPoint = new PublicEndPoint(options.RegistryHostAndPort);
                conv.Launch();

                // Recieve the request
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                byte[] bytes = mockRegistry.Receive(ref ep);
                Assert.IsTrue(bytes.Length > 0);
                Register msg = ControlMessage.Decode(bytes) as Register;
                Assert.IsNotNull(msg);

                // Send back a valid reply
                ProcessInfo reply = new ProcessInfo()
                {
                    ProcessId = 10
                };
                reply.SetMessageAndConversationIds(new MessageId() {Pid = 1, Seq = 100}, msg.ConvId);
                bytes = reply.Encode();
                mockRegistry.Send(bytes, bytes.Length, ep);

                // The conversation should have succeeed
                Thread.Sleep(1000);
                Assert.AreEqual(Conversation.PossibleState.Successed, conv.State);
                Assert.AreEqual(CommProcessState.PossibleState.Running, mockAppProcess.State);
                Assert.AreEqual(10, LocalProcessInfo.Instance.ProcessId);
            }
        }

        [TestMethod]
        public void Registration_Initiator_ThreeCommunicationFailures()
        {
            lock (_testLock)
            {
                // This test case simulates what the initiator would see if there were any of the following
                //      - a failure in the registry process
                //      - requests were being lost
                //      - replies were beging lost

                LocalProcessInfo.Instance.ProcessId = 0;

                // Setup a mock registry (just a receiver)
                UdpClient mockRegistry = new UdpClient(0) {Client = {ReceiveTimeout = 2000}};

                // Setup a mock app process
                RuntimeOptions options = new RuntimeOptions(((IPEndPoint) mockRegistry.Client.LocalEndPoint).Port);
                MockAppProcess mockAppProcess = new MockAppProcess()
                {
                    Options = new RuntimeOptions(12000),
                    Label = "Mock"
                };
                mockAppProcess.Start();
                Assert.AreEqual(CommProcessState.PossibleState.Initialized, mockAppProcess.State);

                // Create a registration conversation
                var conv = mockAppProcess.MyCommSubsystem.CreateFromConversationType<Registration>();
                Assert.IsNotNull(conv);
                conv.RemotEndPoint = new PublicEndPoint(options.RegistryHostAndPort);
                conv.Launch();

                // Don't reply back.  But, we can still check for three requests coming to make sure the conversations
                // reply loop is working
                for (int i = 0; i < 3; i++)
                {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                    byte[] bytes = mockRegistry.Receive(ref ep);
                    Assert.IsTrue(bytes.Length > 0);
                    Register msg = ControlMessage.Decode(bytes) as Register;
                    Assert.IsNotNull(msg);
                }

                // Try to receive one more incoming request, but there shouldn't be one
                try
                {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                    byte[] bytes = mockRegistry.Receive(ref ep);
                    Assert.Fail("Unexcepted incoming message");
                }
                catch
                {
                    // Ignore
                }

                // The conversation should have failed
                Assert.AreEqual(Conversation.PossibleState.Failed, conv.State);
                Assert.AreEqual(CommProcessState.PossibleState.Initialized, mockAppProcess.State);
                Assert.AreEqual(0, LocalProcessInfo.Instance.ProcessId);
            }
        }

        [TestMethod]
        public void Registration_Initiator_RecoveryFromOneCommunicationFailure()
        {
            lock (_testLock)
            {
                LocalProcessInfo.Instance.ProcessId = 0;

                UdpClient mockRegistry = new UdpClient(0) {Client = {ReceiveTimeout = 2000}};

                // Setup a mock app process
                RuntimeOptions options = new RuntimeOptions(((IPEndPoint) mockRegistry.Client.LocalEndPoint).Port);
                MockAppProcess mockAppProcess = new MockAppProcess()
                {
                    Options = new RuntimeOptions(12000),
                    Label = "Mock"
                };
                mockAppProcess.Start();
                Assert.AreEqual(CommProcessState.PossibleState.Initialized, mockAppProcess.State);

                // Create a registration conversation
                var conv = mockAppProcess.MyCommSubsystem.CreateFromConversationType<Registration>();
                Assert.IsNotNull(conv);
                conv.RemotEndPoint = new PublicEndPoint(options.RegistryHostAndPort);
                conv.Launch();

                // Get the first request
                var ep = new IPEndPoint(IPAddress.Any, 0);
                var bytes = mockRegistry.Receive(ref ep);
                Assert.IsTrue(bytes.Length > 0);
                var msg = ControlMessage.Decode(bytes) as Register;
                Assert.IsNotNull(msg);

                // Ignore it
                Thread.Sleep((100));

                // Get the second request
                ep = new IPEndPoint(IPAddress.Any, 0);
                bytes = mockRegistry.Receive(ref ep);
                Assert.IsTrue(bytes.Length > 0);
                msg = ControlMessage.Decode(bytes) as Register;
                Assert.IsNotNull(msg);

                // Send back a valid reply
                ProcessInfo reply = new ProcessInfo()
                {
                    ProcessId = 10
                };
                reply.SetMessageAndConversationIds(new MessageId() {Pid = 1, Seq = 100}, msg.ConvId);
                bytes = reply.Encode();
                mockRegistry.Send(bytes, bytes.Length, ep);

                // The conversation should have succeeded
                Thread.Sleep(1000);
                Assert.AreEqual(Conversation.PossibleState.Successed, conv.State);
                Assert.AreEqual(CommProcessState.PossibleState.Running, mockAppProcess.State);
                Assert.AreEqual(10, LocalProcessInfo.Instance.ProcessId);
            }
        }

        [TestMethod]
        public void Registration_Initiator_NakResponse()
        {
            lock (_testLock)
            {
                LocalProcessInfo.Instance.ProcessId = 0;

                // Setup a mock registry (just a receiver)
                UdpClient mockRegistry = new UdpClient(0) {Client = {ReceiveTimeout = 1000}};

                // Setup a mock app process
                RuntimeOptions options = new RuntimeOptions(((IPEndPoint) mockRegistry.Client.LocalEndPoint).Port);
                MockAppProcess mockAppProcess = new MockAppProcess()
                {
                    Options = new RuntimeOptions(12000),
                    Label = "Mock"
                };
                mockAppProcess.Start();
                Assert.AreEqual(CommProcessState.PossibleState.Initialized, mockAppProcess.State);

                // Create a registration

                var conv = mockAppProcess.MyCommSubsystem.CreateFromConversationType<Registration>();
                Assert.IsNotNull(conv);
                conv.RemotEndPoint = new PublicEndPoint(options.RegistryHostAndPort);
                conv.Launch();

                // Get the request message
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                byte[] bytes = mockRegistry.Receive(ref ep);
                Assert.IsTrue(bytes.Length > 0);
                Register msg = ControlMessage.Decode(bytes) as Register;
                Assert.IsNotNull(msg);

                // Send back a Nak reply
                Nak reply = new Nak()
                {
                    Error = new Error() {Text = "Process already registered"}
                };
                reply.SetMessageAndConversationIds(new MessageId() {Pid = 1, Seq = 100}, msg.ConvId);
                bytes = reply.Encode();
                mockRegistry.Send(bytes, bytes.Length, ep);

                Thread.Sleep(1000);

                // The conversation should have failed
                Assert.AreEqual(Conversation.PossibleState.Failed, conv.State);
                Assert.AreEqual("Process already registered", conv.Error.Text);
                Assert.AreEqual(CommProcessState.PossibleState.Initialized, mockAppProcess.State);
                Assert.AreNotEqual(10, LocalProcessInfo.Instance.ProcessId);
            }
        }

        [TestMethod]
        public void Registration_Initiator_ThreeInvalidReplies()
        {
            lock (_testLock)
            {
                LocalProcessInfo.Instance.ProcessId = 0;

                UdpClient mockRegistry = new UdpClient(0) {Client = {ReceiveTimeout = 2000}};

                // Setup a mock app process
                RuntimeOptions options = new RuntimeOptions(((IPEndPoint) mockRegistry.Client.LocalEndPoint).Port);
                MockAppProcess mockAppProcess = new MockAppProcess()
                {
                    Options = new RuntimeOptions(12000),
                    Label = "Mock"
                };
                mockAppProcess.Start();
                Assert.AreEqual(CommProcessState.PossibleState.Initialized, mockAppProcess.State);

                // Create a registration conversation
                var conv = mockAppProcess.MyCommSubsystem.CreateFromConversationType<Registration>();
                Assert.IsNotNull(conv);
                conv.RemotEndPoint = new PublicEndPoint(options.RegistryHostAndPort);
                conv.Launch();

                // Don't reply back.  But, we can still check for three requests coming to make sure the conversations
                // reply loop is working
                for (int i = 0; i < 3; i++)
                {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                    byte[] bytes = mockRegistry.Receive(ref ep);
                    Assert.IsTrue(bytes.Length > 0);
                    Register msg = ControlMessage.Decode(bytes) as Register;
                    Assert.IsNotNull(msg);

                    bytes = Encoding.ASCII.GetBytes("Garbage");
                    mockRegistry.Send(bytes, bytes.Length, ep);
                }

                // Try to receive one more incoming request, but there shouldn't be one
                try
                {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                    byte[] bytes = mockRegistry.Receive(ref ep);
                    Assert.Fail("Unexcepted incoming message");
                }
                catch
                {
                    // Ignore
                }

                // The conversation should have failed
                Assert.AreEqual(Conversation.PossibleState.Failed, conv.State);
                Assert.AreEqual(CommProcessState.PossibleState.Initialized, mockAppProcess.State);
                Assert.AreEqual(0, LocalProcessInfo.Instance.ProcessId);
            }
        }


        [TestMethod]
        public void Registration_Initiator_InvalidReplyFollowedByGoodReply()
        {
            lock (_testLock)
            {
                LocalProcessInfo.Instance.ProcessId = 0;

                UdpClient mockRegistry = new UdpClient(0) {Client = {ReceiveTimeout = 2000}};

                // Setup a mock app process
                RuntimeOptions options = new RuntimeOptions(((IPEndPoint) mockRegistry.Client.LocalEndPoint).Port);
                MockAppProcess mockAppProcess = new MockAppProcess()
                {
                    Options = new RuntimeOptions(12000),
                    Label = "Mock"
                };
                mockAppProcess.Start();
                Assert.AreEqual(CommProcessState.PossibleState.Initialized, mockAppProcess.State);

                // Create a registration conversation
                var conv = mockAppProcess.MyCommSubsystem.CreateFromConversationType<Registration>();
                Assert.IsNotNull(conv);
                conv.RemotEndPoint = new PublicEndPoint(options.RegistryHostAndPort);
                conv.Launch();

                // Receive the request
                var ep = new IPEndPoint(IPAddress.Any, 0);
                var bytes = mockRegistry.Receive(ref ep);
                Assert.IsTrue(bytes.Length > 0);
                var msg = ControlMessage.Decode(bytes) as Register;
                Assert.IsNotNull(msg);

                // Send back an invalid type of message
                ControlMessage reply = new Status();
                reply.SetMessageAndConversationIds(new MessageId() {Pid = 1, Seq = 100}, msg.ConvId);
                bytes = reply.Encode();
                mockRegistry.Send(bytes, bytes.Length, ep);

                // Get the resend of the request
                ep = new IPEndPoint(IPAddress.Any, 0);
                bytes = mockRegistry.Receive(ref ep);
                Assert.IsTrue(bytes.Length > 0);
                msg = ControlMessage.Decode(bytes) as Register;
                Assert.IsNotNull(msg);

                // Send back a valid reply
                reply = new ProcessInfo()
                {
                    ProcessId = 10
                };
                reply.SetMessageAndConversationIds(new MessageId() {Pid = 1, Seq = 100}, msg.ConvId);
                bytes = reply.Encode();
                mockRegistry.Send(bytes, bytes.Length, ep);

                // The conversation should have succeeded
                Thread.Sleep(1000);
                Assert.AreEqual(Conversation.PossibleState.Successed, conv.State);
                Assert.AreEqual(CommProcessState.PossibleState.Running, mockAppProcess.State);
                Assert.AreEqual(10, LocalProcessInfo.Instance.ProcessId);
            }
        }
    }
}
