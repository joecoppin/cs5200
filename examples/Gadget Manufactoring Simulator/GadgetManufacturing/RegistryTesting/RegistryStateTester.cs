using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Registry;
using SharedObjects;
using Registry = Registry.Registry;

namespace RegistryTesting
{
    [TestClass]
    public class RegistryStateTester
    {
        [TestMethod]
        public void Registry_Construction()
        {
            RegistryState state = new RegistryState();
            Assert.IsNotNull(state.RegisteredProcesses);
            Assert.AreEqual(0, state.RegisteredProcessCount);
        }

        [TestMethod]
        public void Registry_RegisterProcessAndFindEntry()
        {
            RegistryState state = new RegistryState();

            PublicEndPoint ep1 = new PublicEndPoint() {HostAndPort = "127.0.0.1:3001"};
            int pid1 = state.RegisterProcess(ProcessType.GadgetAssembler, ep1);
            Assert.IsTrue(pid1>0);
            Assert.AreEqual(1, state.RegisteredProcessCount);
            RegistryState.RegistryEntry e = state.FindEntry(ep1);
            Assert.AreEqual(pid1, e.Id);
            Assert.AreEqual(ProcessType.GadgetAssembler, e.ProcessType);
            Assert.AreEqual(ep1, e.EndPoint);

            PublicEndPoint ep2 = new PublicEndPoint() { HostAndPort = "127.0.0.1:3002" };
            int pid2 = state.RegisterProcess(ProcessType.WidgetBuilder, ep2);
            Assert.IsTrue(pid2 > 0);
            Assert.AreEqual(2, state.RegisteredProcessCount);
            e = state.FindEntry(ep1);
            Assert.AreEqual(pid1, e.Id);
            Assert.AreEqual(ProcessType.GadgetAssembler, e.ProcessType);
            Assert.AreEqual(ep1, e.EndPoint);
            e = state.FindEntry(ep2);
            Assert.AreEqual(pid2, e.Id);
            Assert.AreEqual(ProcessType.WidgetBuilder, e.ProcessType);
            Assert.AreEqual(ep2, e.EndPoint);

            PublicEndPoint ep3 = new PublicEndPoint() { HostAndPort = "127.0.0.1:3003" };
            int pid3 = state.RegisterProcess(ProcessType.ThingABobBuilder, ep3);
            Assert.IsTrue(pid3 > 0);
            Assert.AreEqual(3, state.RegisteredProcessCount);
            e = state.FindEntry(ep1);
            Assert.AreEqual(pid1, e.Id);
            Assert.AreEqual(ProcessType.GadgetAssembler, e.ProcessType);
            Assert.AreEqual(ep1, e.EndPoint);
            e = state.FindEntry(ep2);
            Assert.AreEqual(pid2, e.Id);
            Assert.AreEqual(ProcessType.WidgetBuilder, e.ProcessType);
            Assert.AreEqual(ep2, e.EndPoint);
            e = state.FindEntry(ep3);
            Assert.AreEqual(pid3, e.Id);
            Assert.AreEqual(ProcessType.ThingABobBuilder, e.ProcessType);
            Assert.AreEqual(ep3, e.EndPoint);

            int pid4 = state.RegisterProcess(ProcessType.ThingABobBuilder, ep2);
            Assert.AreEqual(pid2, pid4);
            Assert.AreEqual(3, state.RegisteredProcessCount);
            e = state.FindEntry(ep1);
            Assert.AreEqual(pid1, e.Id);
            Assert.AreEqual(ProcessType.GadgetAssembler, e.ProcessType);
            Assert.AreEqual(ep1, e.EndPoint);
            e = state.FindEntry(ep2);
            Assert.AreEqual(pid2, e.Id);
            Assert.AreEqual(ProcessType.WidgetBuilder, e.ProcessType);
            Assert.AreEqual(ep2, e.EndPoint);
            e = state.FindEntry(ep3);
            Assert.AreEqual(pid3, e.Id);
            Assert.AreEqual(ProcessType.ThingABobBuilder, e.ProcessType);
            Assert.AreEqual(ep3, e.EndPoint);

            PublicEndPoint ep4 = new PublicEndPoint() { HostAndPort = "127.0.0.1:3004" };
            e = state.FindEntry(ep4);
            Assert.IsNull(e);
        }

        [TestMethod]
        public void RegisterState_ThreadSafe()
        {
            RegistryState state = new RegistryState();

            PublicEndPoint[] eps = new PublicEndPoint[1000];

            Parallel.For(0, 1000, (i) =>
            {
                eps[i] = new PublicEndPoint() { HostAndPort = $"127.0.0.1:{2000+i}" };
                int pid = state.RegisterProcess(ProcessType.GadgetAssembler, eps[i]);
                Assert.IsTrue(pid > 0);
                RegistryState.RegistryEntry e = state.FindEntry(eps[i]);
                Assert.AreEqual(pid, e.Id);
                Assert.AreEqual(ProcessType.GadgetAssembler, e.ProcessType);
                Assert.AreEqual(eps[i], e.EndPoint);
            });

            Assert.AreEqual(1000, state.RegisteredProcessCount);
        }

    }
}
