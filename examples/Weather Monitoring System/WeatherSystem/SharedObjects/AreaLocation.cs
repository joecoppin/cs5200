using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SharedObjects
{
    [DataContract]
    public class AreaLocation : Location
    {
        [DataMember]
        public List<PointLocation> Points { get; set; }
    }
}
