using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedObjects;

namespace SharedObjectsTesting
{
    [TestClass]
    public class MessageIdTester
    {
        private readonly object _testLock = new object();

        [TestMethod]
        public void MessageId_BasicConstruction()
        {
            lock (_testLock)
            {
                PrivateType privateHelperObject = new PrivateType(typeof(MessageId));
                privateHelperObject.SetStaticField("_nextSeqNumber", (short) 0);

                MessageId mn0 = new MessageId();
                Assert.AreEqual(0, mn0.Pid);
                Assert.AreEqual(0, mn0.Seq);
                Assert.AreEqual(0, mn0.GetHashCode());

                MessageId mn1 = MessageId.Create();
                Assert.AreEqual(100, mn1.Pid);
                Assert.AreEqual(1, mn1.Seq);
                Assert.AreEqual(0x00640001, mn1.GetHashCode());

                MessageId mn2 = MessageId.Create();
                Assert.AreEqual(100, mn1.Pid);
                Assert.AreEqual(mn1.Seq + 1, mn2.Seq);

                // Test sequential allocation
                for (int i = 0; i < 100; i++)
                {
                    MessageId mn4 = MessageId.Create();
                    Assert.AreEqual(100, mn1.Pid);
                    Assert.AreEqual(mn2.Seq + i + 1, mn4.Seq);
                }
            }
        }

        [TestMethod]
        public void MessageId_RolloverOfSequenceNumber()
        {
            lock (_testLock)
            {
                PrivateType privateHelperObject = new PrivateType(typeof(MessageId));
                privateHelperObject.SetStaticField("_nextSeqNumber", Convert.ToInt16(short.MaxValue - 2));

                MessageId mn5 = MessageId.Create();
                Assert.AreEqual(short.MaxValue - 1, mn5.Seq);
                mn5 = MessageId.Create();
                Assert.AreEqual(short.MaxValue, mn5.Seq);
                mn5 = MessageId.Create();
                Assert.AreEqual(1, mn5.Seq);

                MessageId mn3 = new MessageId();
                Assert.AreEqual(0, mn3.Pid);
                Assert.AreEqual(0, mn3.Seq);
            }
        }

        [TestMethod]
        public void MessageId_TestComparison()
        {
            lock (_testLock)
            {
                PrivateType privateHelperObject = new PrivateType(typeof(MessageId));
                privateHelperObject.SetStaticField("_nextSeqNumber", (short) 0);

                MessageId mn0 = new MessageId();
                MessageId mn1 = MessageId.Create();
                MessageId mn2 = MessageId.Create();
                MessageId mn3 = new MessageId() {Pid = mn2.Pid, Seq = mn2.Seq};

                Assert.AreEqual(mn2.Pid, mn3.Pid);
                Assert.AreEqual(mn2.Seq, mn3.Seq);

                Assert.IsTrue(mn2.Equals(mn3));
                Assert.IsFalse(mn2.Equals(mn1));
                Assert.IsFalse(mn2.Equals(null));
                Assert.IsFalse(mn2.Equals(MessageId.Create()));

                Assert.IsTrue(mn2 == mn3);
                Assert.IsTrue(mn1 <= mn3);
                Assert.IsTrue(mn1 < mn3);
                Assert.IsTrue(mn3 != mn1);
                Assert.IsTrue(mn3 >= mn1);
                Assert.IsTrue(mn3 > mn1);

                MessageId.MessageIdComparer comparer = new MessageId.MessageIdComparer();
                Assert.IsTrue(comparer.Equals(mn2, mn3));
                Assert.AreEqual(0, comparer.GetHashCode(mn0));
                Assert.AreEqual(0x00640001, comparer.GetHashCode(mn1));
            }
        }

    }
}
