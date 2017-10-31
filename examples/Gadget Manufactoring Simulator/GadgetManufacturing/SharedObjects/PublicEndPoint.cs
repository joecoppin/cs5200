using System;
using System.Runtime.Serialization;
using System.Net;

namespace SharedObjects
{
    /// <summary>
    /// PublicEndPoint
    /// 
    /// This class is an Adapter (or Wrapper) for .Net's IPEndPoint that provides some additional
    /// functionality, including
    ///     - Keeping track of the IP hostname, not just the IPv4 Address
    ///     - Deferred, but automatic, DNS lookup of IP hostname and selection of an appropriate IPv4 Address
    /// </summary>
    [DataContract]
    public class PublicEndPoint
    {
        #region Private Data Members
        private string _myHost = "0.0.0.0";               // String reprsentation of Hostname or IP Address
        private bool _needToResolveHostname;              // If true, then the hostname needs to be resolved into an IP Address
        private IPEndPoint _myEp;                         // Adaptee (object being adapted or "wrapped"
        #endregion

        #region Constructors
        public PublicEndPoint()
        {
            SetDefaults();
        }

        public PublicEndPoint(string hostnameAndPort) : this()
        {
            SetHostAndPort(hostnameAndPort);
        }
        #endregion

        #region Public properties and methods
        /// <summary>
        /// Host property
        /// 
        /// The property to access the host, which can be hostname or string representation of the IP Address
        /// </summary>
        [DataMember]
        public string Host
        {
            get
            {
                return _myHost;
            }
            set
            {
                // If the IPEndPoint hasn't been set up, do so now.  Note that this might be the case when creating
                // the object via deserialization.  The properties will be called before the data member initializers
                // or the default constructor's body.
                if (_myEp == null)
                    SetDefaults();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    _myHost = value.Trim();
                    _needToResolveHostname = true;
                }
                else
                {
                    _myHost = "0.0.0.0";
                    _myEp.Address = IPAddress.Any;
                    _needToResolveHostname = false;
                }
            }
        }

        [DataMember]
        public Int32 Port
        {
            get { return _myEp.Port; }
            set
            {
                // If the IPEndPoint hasn't been set up, do so now.  Note that this might be the case when creating
                // the object via deserialization.  The properties will be called before the data member initializers
                // or the default constructor's body.
                if (_myEp == null)
                    SetDefaults();

                _myEp.Port = value;
            }
        }

        public string HostAndPort
        {
            get { return ToString(); }
            set { SetHostAndPort(value); }
        }

        /// <summary>
        /// IPEndPoint Property
        /// 
        /// This property is for convenience in work with .Net IPEndPoint objects.
        /// </summary>
        public IPEndPoint IpEndPoint
        {
            get
            {
                if (_needToResolveHostname)
                {
                    _needToResolveHostname = false;
                    _myEp.Address = LookupAddress(_myHost);
                }
                return _myEp;
            }
            set
            {
                _myEp = (value != null) ?  _myEp = value : _myEp = new IPEndPoint(IPAddress.Any, 0);
                _myHost = _myEp.Address.ToString();
                _needToResolveHostname = false;
            }
        }

        public static IPAddress LookupAddress(string host)
        {
            IPAddress result = null;
            if (!string.IsNullOrWhiteSpace(host))
            {
                IPAddress[] addressList = Dns.GetHostAddresses(host);
                for (int i = 0; i < addressList.Length && result == null; i++)
                    if (addressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        result = addressList[i];
            }
            return result;
        }

        public override string ToString()
        {
            return $"{_myHost}:{Port}";
        }

        public override int GetHashCode()
        {
            return HostAndPort.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            bool result = false;
            PublicEndPoint other = obj as PublicEndPoint;
            if (other != null)
            {
                result = (HostAndPort == other.HostAndPort);
            }
            return result;
        }

        public PublicEndPoint Clone()
        {
            return MemberwiseClone() as PublicEndPoint;
        }

        public static int Compare(PublicEndPoint a, PublicEndPoint b)
        {
            int result = 0;

            if (!ReferenceEquals(a, b))
            {
                if (((object)a == null) && ((object)b != null))
                    result = -1;
                else if (((object)a != null) && ((object)b == null))
                    result = 1;
                else
                {
                    result = String.Compare(a?.Host, b?.Host, StringComparison.Ordinal);
                    if (result == 0)
                    {
                        if (a?.Port < b?.Port)
                            result = -1;
                        else if (a?.Port > b?.Port)
                            result = 1;
                    }
                }
            }
            return result;
        }

        public static bool operator ==(PublicEndPoint a, PublicEndPoint b)
        {
            return (Compare(a, b) == 0);
        }

        public static bool operator !=(PublicEndPoint a, PublicEndPoint b)
        {
            return (Compare(a, b) != 0);
        }

        public static bool operator <(PublicEndPoint a, PublicEndPoint b)
        {
            return (Compare(a, b) < 0);
        }

        public static bool operator >(PublicEndPoint a, PublicEndPoint b)
        {
            return (Compare(a, b) > 0);
        }

        public static bool operator <=(PublicEndPoint a, PublicEndPoint b)
        {
            return (Compare(a, b) <= 0);
        }

        public static bool operator >=(PublicEndPoint a, PublicEndPoint b)
        {
            return (Compare(a, b) >= 0);
        }

        #endregion

        #region Private Methods
        private void SetDefaults()
        {
            _myHost = "0.0.0.0";
            _needToResolveHostname = false;
            _myEp = new IPEndPoint(IPAddress.Any, 0);
        }

        private void SetHostAndPort(string hostAndPort)
        {
            SetDefaults();

            if (!string.IsNullOrWhiteSpace(hostAndPort))
            {
                string[] tmp = hostAndPort.Split(':');
                if (tmp.Length == 1)
                {
                    Host = hostAndPort;
                    _needToResolveHostname = true;
                }
                else if (tmp.Length >= 2)
                {
                    Host = tmp[0].Trim();

                    int port;
                    Int32.TryParse(tmp[1].Trim(), out port);
                    Port = port;

                    _needToResolveHostname = true;
                }
            }
        }
        #endregion

    }
}
