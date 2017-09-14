using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Net;
using System.IO;

namespace Server_Client
{

    public class Message
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Message));

        public short MessageType { get; set; }
        public string a_num { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public string alias { get; set; }
        public short game_id { get; set; }
        public string hint { get; set; }
        public string def { get; set; }
        public string guess { get; set; }
        public byte result { get; set; }
        public short score { get; set; }
        public string error { get; set; }


        //need to overwrite this so each message is unique
        public byte[] Encode_NewGame()
        {
            logger.Info("Encoding NewGame Function");
            MemoryStream memoryStream = new MemoryStream();

            Write(memoryStream, MessageType);
            Write(memoryStream, a_num);
            Write(memoryStream, l_name);
            Write(memoryStream, f_name);
            Write(memoryStream, alias);
            
            return memoryStream.ToArray();
        }

        public byte[] Encode_Guess()
        {
            logger.Info("Encoding Guess Function");

            MemoryStream memoryStream = new MemoryStream();

            Write(memoryStream, MessageType);
            Write(memoryStream, game_id);
            Write(memoryStream, guess);

            return memoryStream.ToArray();
        }

        public byte[] Encode_Hint_Exit_Ack()
        {
            logger.Info("Encoding Hint/Exit/Ack Function");

            MemoryStream memoryStream = new MemoryStream();

            Write(memoryStream, MessageType);
            Write(memoryStream, game_id);

            return memoryStream.ToArray();
        }

        public static Message Decode(byte[] bytes)
        {
            logger.Info("In Message.Decode Function");

            Message message = null;
            
            if (bytes != null)
            {
                message = new Message();
                MemoryStream memoryStream = new MemoryStream(bytes);
                // first decode the message type.
                message.MessageType = ReadShort(memoryStream);

                //NewGame Message
                if (message.MessageType == 1)
                {
                    message.a_num = ReadString(memoryStream);
                    message.l_name = ReadString(memoryStream);
                    message.f_name= ReadString(memoryStream);
                    message.alias = ReadString(memoryStream);
                }

                //Gamedef message
                if (message.MessageType == 2)
                {
                    message.game_id = ReadShort(memoryStream);
                    message.hint = ReadString(memoryStream);
                    message.def = ReadString(memoryStream);
                }
                
                //Answer Message
                if (message.MessageType == 4)
                {
                    message.game_id = ReadShort(memoryStream);
                    message.result = ReadByte(memoryStream);
                    message.score = ReadShort(memoryStream);
                    message.hint = ReadString(memoryStream);
                }

                //Hint message
                if (message.MessageType == 6)
                {
                    message.game_id = ReadShort(memoryStream);
                    message.hint = ReadString(memoryStream);
                }

                //Error Message
                if (message.MessageType == 9)
                {
                    message.game_id = ReadShort(memoryStream);
                    message.error = ReadString(memoryStream);
                }

                //Heartbeat Message
                if (message.MessageType == 10)
                {
                    message.game_id = ReadShort(memoryStream);
                }
              
            }
            return message;
        }

        private static void Write(MemoryStream memoryStream, short value)
        {
            byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
            logger.Debug($"Write short: {bytes}");
            memoryStream.Write(bytes, 0, bytes.Length);
        }
        private static void Write(MemoryStream memoryStream, string value)
        {
            byte[] bytes = Encoding.BigEndianUnicode.GetBytes(value);
            Write(memoryStream, (short)bytes.Length);
            logger.Debug($"Write string: {bytes}");
            memoryStream.Write(bytes, 0, bytes.Length);
        }

        private static void Write(MemoryStream memoryStream, byte value)
        {
            byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
            logger.Debug($"Write byte: {bytes}");
            memoryStream.Write(bytes, 0, bytes.Length);
        }

        private static short ReadShort(MemoryStream memoryStream)
        {
            byte[] bytes = new byte[2];
            int bytesRead = memoryStream.Read(bytes, 0, bytes.Length);
            if (bytesRead != bytes.Length)
                throw new ApplicationException("Cannot decode a short integer from message");
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt16(bytes, 0));
        }

        private static int ReadInt(MemoryStream memoryStream)
        {
            byte[] bytes = new byte[4];
            int bytesRead = memoryStream.Read(bytes, 0, bytes.Length);
            if (bytesRead != bytes.Length)
                throw new ApplicationException("Cannot decode an integer from message");

            return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0));
        }
        
        private static byte ReadByte(MemoryStream memoryStream)
        {
            byte[] bytes = new byte[1];
            int bytesRead = memoryStream.Read(bytes, 0, 1);
            return bytes[0];
        }
        
        private static string ReadString(MemoryStream memoryStream)
        {
            string result = String.Empty;
            int length = ReadShort(memoryStream);
            if (length > 0)
            {
                byte[] bytes = new byte[length];
                int bytesRead = memoryStream.Read(bytes, 0, bytes.Length);
                if (bytesRead != length)
                    throw new ApplicationException("Cannot decode a string from message");

                result = Encoding.BigEndianUnicode.GetString(bytes, 0, bytes.Length);
            }
            return result;
        }
    }
}