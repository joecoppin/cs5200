using System;
using System.IO;
using System.Net;
using System.Text;
using log4net;

namespace SimpleUDPSocket
{
    public class Message
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (Message));

        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Text { get; set; }

        public byte[] Encode()
        {
            Logger.Debug("Encode message");

            MemoryStream memoryStream = new MemoryStream();

            Write(memoryStream, Id);
            Write(memoryStream, Timestamp.Ticks);
            Write(memoryStream, Text);

            return memoryStream.ToArray();
        }

        public static Message Decode(byte[] bytes)
        {
            Message message = null;
            if (bytes != null)
            {
                message = new Message();

                MemoryStream memoryStream = new MemoryStream(bytes);

                message.Id = ReadInt(memoryStream);
                long ticks = ReadLong(memoryStream);
                message.Timestamp = new DateTime(ticks);
                message.Text = ReadString(memoryStream);
            }

            return message;
        }

        private static void Write(MemoryStream memoryStream, short value)
        {
            byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
            memoryStream.Write(bytes, 0, bytes.Length);
        }

        private static void Write(MemoryStream memoryStream, int value)
        {
            byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
            memoryStream.Write(bytes, 0, bytes.Length);
        }

        private static void Write(MemoryStream memoryStream, long value)
        {
            byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
            memoryStream.Write(bytes, 0, bytes.Length);
        }

        private static void Write(MemoryStream memoryStream, string value)
        {
            byte[] bytes = Encoding.BigEndianUnicode.GetBytes(value);
            Write(memoryStream, (short) bytes.Length);
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

        private static long ReadLong(MemoryStream memoryStream)
        {
            byte[] bytes = new byte[8];
            int bytesRead = memoryStream.Read(bytes, 0, bytes.Length);
            if (bytesRead != bytes.Length)
                throw new ApplicationException("Cannot decode a long integer from message");

            return IPAddress.NetworkToHostOrder(BitConverter.ToInt64(bytes, 0));
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
