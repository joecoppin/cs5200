using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Messages;

namespace CommonTesting
{
    [TestClass]
    public class NewGameMessageTester
    {
        [TestMethod]
        public void NewGameMessage_CheckEverything()
        {
            NewGameMessage msg = new NewGameMessage()
            {
                ANumber = "a",
                FirstName = "a",
                LastName = "a"
            };
            byte[] bytes = msg.Encode();

        }
    }
}
