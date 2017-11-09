using System.Runtime.Serialization;

namespace SharedObjects
{
    [DataContract]
    public class ThingABob : Resource
    {
        [DataMember]
        public int Height { get; set; }

        [DataMember]
        public int Width { get; set; }
    }
}
