using System;
using System.Collections.Generic;
using System.Linq;
using CommSubsystem;
using Messages;

namespace SharedAppLayer
{
    public abstract class WmsAppWorker : AppWorker
    {
        protected readonly List<ServerInfo> DataServers = new List<ServerInfo>();
        protected DateTime LastServerDiscover = DateTime.MinValue;

        protected const int MainLoopSleep = 200;
        protected object MyLock = new object();

        public void AddDataServer(ServerInfo server)
        {
            if (server == null) return;

            lock (MyLock)
            {
                var existingServerInfo = DataServers.Find(s => s.ProcessId == server.ProcessId);
                if (existingServerInfo == null)
                    DataServers.Add(server);
                else
                    existingServerInfo.EndPoint = server.EndPoint;
            }
        }

        public void RemoveDataServer(ServerInfo server)
        {
            if (server == null) return;

            lock (MyLock)
            {
                DataServers.RemoveAll(s => s.ProcessId == server.ProcessId);
            }
        }

        public List<ServerInfo> GetDataServers()
        {
            var listCopy = new List<ServerInfo>();
            lock (MyLock)
            {
                listCopy.AddRange(DataServers.Select(server => server.Clone()));
            }
            return listCopy;
        }

        protected void DataServerDiscovery()
        {
            var currentTime = DateTime.Now;
            if (currentTime.Subtract(LastServerDiscover).Minutes <= Options.DsDiscoveryInterval) return;

            var targetEndPoint = EndPointParser.ParseEndPoint(Options.DsGroupMultiCastAddress);               
            var firstMsg = new ServerDiscoveryMessage();
            firstMsg.InitMessageAndConversationIds();
            var env = new Envelope() {EndPoint = targetEndPoint, Message = firstMsg};
            CommFacility.Send(env);
            LastServerDiscover = currentTime;
        }
    }
}
