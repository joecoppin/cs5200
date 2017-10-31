using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedObjects;
namespace SharedObjectsTest
{
    [TestClass]
    public class MessageIdTest
    {
        [TestInitialize]
        public void Setup()
        {
            LocalProcessInfo.Instance.ProcessId = 1;
        }   

        [TestMethod]
        public void MessageId_CheckCreateMethod()
        {
            var id1 = MessageId.Create();

            Assert.AreEqual(1, id1.Pid);
            Assert.IsTrue(id1.Seq >= 1);
        }


        // TODO: Still need test cases for all the other methods and non-trival properties.
    }
}
