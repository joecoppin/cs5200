using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SimpleUDPSocket;

namespace SimpleUDPSocketTest
{
    /// <summary>
    /// Summary description for SimpleSenderTest
    /// 
    /// Note that this test class uses a PrivateType object to wrap up a SimpleSender object
    /// so the test cases can call the private methods
    /// </summary>
    [TestClass]
    public class SimpleSenderTest
    {
        [TestClass]
        public class SimpleSenderTester
        {
            private static readonly object MyLock = new object();
            private readonly PrivateType _simpleSenderWrapper = new PrivateType((new SimpleSender()).GetType());

            [TestMethod]
            public void SimpleSenderTester_TestSingleGetNextId()
            {
                lock (MyLock)
                {
                    int id1 = (int) _simpleSenderWrapper.InvokeStatic("GetNextId");
                    int id2 = (int) _simpleSenderWrapper.InvokeStatic("GetNextId");
                    if (id1 != int.MaxValue)
                        Assert.IsTrue(id2 == id1 + 1);
                    else
                        Assert.AreEqual(1, id2);
                }
            }

            [TestMethod]
            public void SimpleSenderTester_TestLotsOfGetNextId()
            {
                Parallel.For(0, 100000, (i) =>
                {
                    lock (MyLock) // Without this lock here and around other test cases that use
                        // GetNextId, the id2 == id1 + 1 assertation may fail
                    {
                        int id1 = (int) _simpleSenderWrapper.InvokeStatic("GetNextId");
                        int id2 = (int) _simpleSenderWrapper.InvokeStatic("GetNextId");
                        if (id1 != int.MaxValue)
                            Assert.IsTrue(id2 == id1 + 1);
                        else
                            Assert.AreEqual(1, id2);
                    }
                });
            }

            [TestMethod]
            public void SimpleSenderTester_TestRolloverOfGetNextId()
            {
                lock (MyLock)
                {
                    _simpleSenderWrapper.SetStaticField("_nextId", int.MaxValue);
                    int id = (int) _simpleSenderWrapper.InvokeStatic("GetNextId");
                    Assert.AreEqual(1, id);
                }
            }

            [TestMethod]
            public void SimpleSenderTester_TestSendToPeers()
            {
                var sender = new SimpleSender();
                var senderWrapper = new PrivateObject(sender);

                List<UdpClient> clients = new List<UdpClient>();

                for (int i = 0; i < 100; i++)
                {
                    var client = CreateReceiver();
                    clients.Add(client);
                    sender.Peers.Add(GetLocalIpEndPoint(client));
                }

                DateTime beginTime = DateTime.Now;

                lock (MyLock)
                {
                    senderWrapper.Invoke("SendToPeers", "Test message");
                }                    

                Parallel.ForEach(clients, (client) =>
                {
                    var incomingMessage = ReceiveMessage(client);
                    Assert.IsNotNull(incomingMessage);
                    Assert.IsTrue(beginTime < incomingMessage.Timestamp);
                    Assert.AreEqual("Test message", incomingMessage.Text);
                });
            }

            private UdpClient CreateReceiver()
            {
                IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 0);
                UdpClient client = new UdpClient(localEp) {Client = {ReceiveTimeout = 5000}};

                return client;
            }

            private IPEndPoint GetLocalIpEndPoint(UdpClient client)
            {
                return new IPEndPoint(IPAddress.Loopback, ((IPEndPoint) client.Client.LocalEndPoint).Port);
            }

            private Message ReceiveMessage(UdpClient client)
            {
                IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);
                byte[] bytes = null;

                try
                {
                    bytes = client.Receive(ref remoteEp);
                }
                catch (SocketException err)
                {
                    if (err.SocketErrorCode != SocketError.TimedOut)
                        throw;
                }

                Message result = Message.Decode(bytes);
                return result;
            }
        }
    }
}
