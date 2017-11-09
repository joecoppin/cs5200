using System;
using System.Collections.Generic;
using System.Linq;
using CommSub;
using SharedObjects;

namespace Registry
{
    public class RegistryState : CommProcessState
    {
        private short _nextProcessId = 2;
        private readonly Dictionary<short, RegistryEntry> _processes = new Dictionary<short, RegistryEntry>();
        private readonly object _myLock = new object();

        public short RegisterProcess(ProcessType type, PublicEndPoint ep)
        {
            short newProcessId = 0;
            if (type != ProcessType.Unknown && type != ProcessType.Register && ep != null)
            {
                RegistryEntry entry = FindEntry(ep);
                if (entry != null)
                    newProcessId = entry.Id;
                else
                {
                    newProcessId = _nextProcessId++;
                    entry = new RegistryEntry()
                    {
                        Id = newProcessId,
                        ProcessType = type,
                        EndPoint = ep
                    };
                    lock (_myLock)
                    {
                        _processes.Add(newProcessId, entry);
                    }
                }
            }
            return newProcessId;
        }

        public List<RegistryEntry> RegisteredProcesses
        {
            get
            {
                lock (_myLock)
                {
                    return _processes.Values.ToList();
                }
            }
        }

        public int RegisteredProcessCount
        {
            get
            {
                lock (_myLock)
                {
                    return _processes.Count;
                }
            }
        }

        public RegistryEntry FindEntry(PublicEndPoint ep)
        {
            if (ep == null) return null;

            RegistryEntry result = null;
            lock (_myLock)
            {
                try
                {
                    result = _processes.First(kv => kv.Value.EndPoint == ep).Value;
                }
                catch (InvalidOperationException)
                {
                    // Ignore 
                }
            }

            return result;
        }

        public void Unregister(PublicEndPoint ep)
        {
            RegistryEntry entry = FindEntry(ep);
            if (entry == null) return;


            lock (_myLock)
            {
                _processes.Remove(entry.Id);
            }
        }

        public class RegistryEntry
        {
            public short Id { get; set; }
            public ProcessType ProcessType { get; set; }
            public PublicEndPoint EndPoint { get; set; }
        }
    }
}
