using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Messages;
using SharedObjects;

namespace MessagesTest
{
    [TestClass]
    public class WeatherDataRequestTest
    {
        [TestMethod]
        public void WeatherDataRequest_Everything()
        {
            var start = new DateTime(2017, 3, 10);
            var end = new DateTime(2017, 3, 14);
            var msg1 = new WeatherDataRequest()
            {
                SelectedDateRange = new DateRange() { Start = start, End = end },
                GeoLocation = new PointLocation() { Longitude = 1.24, Latitude = 3.32 }
            };

            msg1.InitMessageAndConversationIds();
            var bytes = msg1.Encode();

            var msg2 = Message.Decode(bytes) as WeatherDataRequest;
            Assert.IsNotNull(msg2);
            Assert.AreEqual(msg1.MsgId, msg2.MsgId);
            Assert.AreEqual(msg1.ConvId, msg2.ConvId);

            Assert.AreNotSame(start, msg2.SelectedDateRange.Start);
            Assert.AreEqual(start, msg2.SelectedDateRange.Start);
            Assert.AreNotSame(end, msg2.SelectedDateRange.End);
            Assert.AreEqual(end, msg2.SelectedDateRange.End);
            var location = msg2.GeoLocation as PointLocation;
            Assert.IsNotNull(location);
            Assert.AreEqual(1.24,  location.Longitude);
            Assert.AreEqual(3.32, location.Latitude);
        }
    }
}
