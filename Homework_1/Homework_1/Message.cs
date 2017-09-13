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
    class Message
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

        public byte[] Encode()
        {
            MemoryStream memoryStream = new MemoryStream();
            Write(memoryStream, MessageType);
            Write(memoryStream, a_num);
            Write(memoryStream, f_name);
            Write(memoryStream, l_name);
            Write(memoryStream, alias);
            Write(memoryStream, game_id);
            Write(memoryStream, hint);
            Write(memoryStream, def);
            Write(memoryStream, guess);
            Write(memoryStream, result);
            Write(memoryStream, score);
            Write(memoryStream, error);

            return memoryStream.ToArray();
        }

        public static Message Decode(byte[] bytes)
        {

            //use this to delegate instructions later on
            Message message = null;
            if(bytes != null)
            {
                message = new Message();

                MemoryStream memoryStream = new MemoryStream(bytes);

                message.MessageType = ReadShort(memoryStream);
                message.a_num = ReadString(memoryStream);
                message.f_name = ReadString(memoryStream);
                message.l_name = ReadString(memoryStream);
                message.alias = ReadString(memoryStream);
                message.game_id = ReadShort(memoryStream);
                message.hint = ReadString(memoryStream);
                message.guess = ReadString(memoryStream);
                //message.result = ReadByte(memoryStream);
                message.score = ReadShort(memoryStream);
                message.error = ReadString(memoryStream);
            }
            return message;
        }

        private static void Write(MemoryStream memoryStream, short value)
        {
            byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
            memoryStream.Write(bytes, 0, bytes.Length);
        }
        private static void Write(MemoryStream memoryStream, string value)
        {
            byte[] bytes = Encoding.BigEndianUnicode.GetBytes(value);
            Write(memoryStream, (short)bytes.Length);
            memoryStream.Write(bytes, 0, bytes.Length);
        }

        private static void Write(MemoryStream memoryStream, byte value)
        {
            byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
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
        /*
        private static byte ReadByte(MemoryStream memoryStream)
        {
            byte[] bytes = new byte[1];
            int bytesRead = memoryStream.Read(bytes, 0, bytes.Length);
            if (bytesRead != bytes.Length)
                throw new ApplicationException("Cannot decode a byte from message");

            return IPAddress.NetworkToHostOrder(BitConverter.ToBoolean(bytes, 0));
        }
        */
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