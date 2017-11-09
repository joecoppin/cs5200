using System.Runtime.Serialization;

namespace SharedObjects
{
    [DataContract]
    public class Widget : Resource
    {
        [DataMember]
        public int Weight { get; set; }
    }
}
