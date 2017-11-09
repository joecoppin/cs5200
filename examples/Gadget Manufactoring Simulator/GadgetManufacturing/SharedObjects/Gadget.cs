using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SharedObjects
{
    [DataContract]
    public class Gadget : Resource
    {
        [DataMember]
        public List<Widget> Widgets { get; set; } = new List<Widget>();

        [DataMember]
        public List<ThingABob> ThingABobs { get; set; } = new List<ThingABob>();
    }
}
