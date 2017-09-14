using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Homework_1
{
    [TestClass]
    public class Testing
    {
        [TestMethod]
        public void Message_Test_Encode_NewGame()
        {
            Server_Client.Message msg = new Server_Client.Message()
            {
                MessageType = 1,
                a_num = "A01499868",
                l_name = "Coppin",
                f_name = "Joe",
                alias = "jc"
            };
            byte[] bytes = msg.Encode_NewGame();

            Assert.AreEqual(2 + 20 + 14 + 8 + 6, bytes.Length);

            //Message Type
            Assert.AreEqual(0, bytes[0]);
            Assert.AreEqual(1, bytes[1]);

            //A # 
            Assert.AreEqual(65, bytes[5]);
            Assert.AreEqual(48, bytes[7]);
            Assert.AreEqual(49, bytes[9]);
            Assert.AreEqual(52, bytes[11]);
            Assert.AreEqual(57, bytes[13]);
            Assert.AreEqual(57, bytes[15]);
            Assert.AreEqual(56, bytes[17]);
            Assert.AreEqual(54, bytes[19]);
            Assert.AreEqual(56, bytes[21]);

            //Last Name
            Assert.AreEqual(67, bytes[25]);
            Assert.AreEqual(111, bytes[27]);
            Assert.AreEqual(112, bytes[29]);
            Assert.AreEqual(112, bytes[31]);
            Assert.AreEqual(105, bytes[33]);
            Assert.AreEqual(110, bytes[35]);

            //First Name
            Assert.AreEqual(74, bytes[39]);
            Assert.AreEqual(111, bytes[41]);
            Assert.AreEqual(101, bytes[43]);

            //Alias
            Assert.AreEqual(106, bytes[47]);
            Assert.AreEqual(99, bytes[49]);
        }

        [TestMethod]
        public void Message_Test_Encode_Guess()
        {
            Server_Client.Message msg = new Server_Client.Message()
            {
                MessageType = 3,
                game_id = 10,
                guess = "abc"
            };

            byte[] bytes = msg.Encode_Guess();

            Assert.AreEqual(2 + 2 + 8 , bytes.Length);
        }

        [TestMethod]
        public void Message_Test_Encode_Hint_Exit_Ack()
        {
            Server_Client.Message msg = new Server_Client.Message()
            {
                MessageType = 4,
                game_id = 3,
            };

            byte[] bytes = msg.Encode_Hint_Exit_Ack();

            Assert.AreEqual(2 + 2, bytes.Length);
        }

        [TestMethod]
        public void Message_Test_Decode()
        {
            Server_Client.Message newGame_m = new Server_Client.Message()
            {
                MessageType = 1,
                a_num = "A01499868",
                l_name = "Coppin",
                f_name = "Joe",
                alias = "jc"
            };
            byte[] bytes_newGame = newGame_m.Encode_NewGame();
            Server_Client.Message newGame2 = Server_Client.Message.Decode(bytes_newGame);
            Assert.AreEqual(newGame_m.alias, newGame2.alias);

            Server_Client.Message hint_m = new Server_Client.Message()
            {
                MessageType = 3,
                game_id = 10,
            };
            byte[] bytes_Guess = hint_m.Encode_Hint_Exit_Ack();
            Server_Client.Message hint2 = Server_Client.Message.Decode(bytes_Guess);
            Assert.AreEqual(hint_m.guess, hint2.guess);
        }
    }
}
