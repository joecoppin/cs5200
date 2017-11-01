using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Common
{
    public class ByteList
    {
        #region Data Members

        private const int SectionSize = 1024;

        private readonly List<byte[]> _sections = new List<byte[]>();
        private short _addCurrentSection;
        private short _addCurrentOffset;
        private short _readCurrentPosition;
        private readonly Stack<short> _readLimitStack = new Stack<short>();
        #endregion

        #region Constructors
        public ByteList()
        {
            _sections.Add(new byte[SectionSize]);
        }

        public ByteList(params object[] items)
        {
            _sections.Add(new byte[SectionSize]);
            AddObjects(items);
        }
        #endregion

        #region Other Public Methods

        public void Clear()
        {
            _sections.Clear();
            _readLimitStack.Clear();
            _addCurrentSection = 0;
            _addCurrentOffset = 0;
            _readCurrentPosition = 0;
        }


        public byte this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ApplicationException();

                byte[] section = GetSection(ref index);
                return section[index];
            }
            set
            {
                Grow(index);
                byte[] section = GetSection(ref index);
                section[index] = value;
            }
        }

        public int Length => _addCurrentSection * SectionSize + _addCurrentOffset;

        public bool IsMore
        {
            get {
                int tmpMax = (_readLimitStack.Count == 0) ? Length : _readLimitStack.Peek();
                return (_readCurrentPosition < tmpMax);
            }
        }

        #endregion

        #region Write and Add Methods

        public short CurrentWritePosition => Convert.ToInt16(_addCurrentSection * SectionSize + _addCurrentOffset);

        public void WriteInt16To(int writePosition, short length)
        {
            if (writePosition >= 0 && writePosition < Length - 2)
            {
                var sectionIdx = writePosition / SectionSize;
                var sectionOffset = writePosition - sectionIdx * SectionSize;

                byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(length));
                Buffer.BlockCopy(bytes, 0,                                  // Source
                                    _sections[sectionIdx], sectionOffset,   // Destination
                                    sizeof(short));                         // Length

            }
        }

        public void FromBytes(byte[] bytes)
        {
            Clear();
            Add(bytes);
        }

        public void AddObjects(params object[] items)
        {
            foreach (object item in items)
                AddObject(item);
        }

        public void AddObject(object item)
        {
            if (item != null)
            {
                Type type = item.GetType();

                if (type == typeof(ByteList))
                    Add((ByteList)item);
                if (type == typeof(bool))
                    Add((bool)item);
                else if (type == typeof(byte))
                    Add((byte)item);
                else if (type == typeof(char))
                    Add((char)item);
                else if (type == typeof(short) || type == typeof(short))
                    Add((short)item);
                else if (type == typeof(int) || type == typeof(int))
                    Add((int)item);
                else if (type == typeof(long) || type == typeof(long))
                    Add((long)item);
                else if (type == typeof(double))
                    Add((double)item);
                else if (type == typeof(float))
                    Add((float)item);
                else if (type == typeof(string))
                    Add((string)item);
                else if (item is byte[])
                    Add((byte[])item);
                else
                    throw new ApplicationException("Invalid type of object");
            }
        }

        public void Add(ByteList value)
        {
            if (value != null)
            {
                for (int i = 0; i <= value._addCurrentSection; i++)
                {
                    Add(value._sections[i], 0, (i < value._addCurrentSection) ? SectionSize : value._addCurrentOffset);
                }
            }
        }

        public void Add(byte value)
        {
            Add(new [] { value });
        }

        public void Add(bool value) => Add(value ? new byte[] {1} : new byte[] {0});

        public void Add(char value)
        {
            Add(BitConverter.GetBytes(value));
        }

        public void Add(short value)
        {
            Add(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)));
        }

        public void Add(int value)
        {
            Add(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)));
        }

        public void Add(long value)
        {
            Add(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)));
        }

        public void Add(double value)
        {
            Add(BitConverter.GetBytes(value));
        }

        public void Add(float value)
        {
            Add(BitConverter.GetBytes(value));
        }

        public void Add(string value)
        {
            if (value != null)
            {
                byte[] bytes = Encoding.BigEndianUnicode.GetBytes(value);
                Add((short)bytes.Length);
                Add(bytes);
            }
            else
                Add((short)0);
        }

        public void Add(byte[] value)
        {
            if (value != null)
                Add(value, 0, value.Length);
        }

        public void Add(byte[] value, int offset, int length)
        {
            if (value != null)
            {
                int additionalBytesNeeded = _addCurrentOffset + Length - SectionSize;
                Grow(additionalBytesNeeded);

                int cnt = 0;
                while (cnt < length)
                {
                    short blockSize = (short)Math.Min(SectionSize - _addCurrentOffset, length - cnt);
                    Buffer.BlockCopy(value, offset + cnt,
                                            _sections[_addCurrentSection], _addCurrentOffset,
                                            blockSize);

                    cnt += blockSize;
                    _addCurrentOffset += blockSize;
                    if (_addCurrentOffset == SectionSize)
                    {
                        _addCurrentOffset = 0;
                        _addCurrentSection++;
                    }
                }
            }
        }

        #endregion

        #region Read and Get Method

        public void ResetRead()
        {
            _readCurrentPosition = 0;
        }

        public ByteList GetByteList(int length)
        {
            ByteList result = new ByteList();
            result.FromBytes(GetBytes(length));
            return result;
        }

        public byte GetByte()
        {
            return GetBytes(1)[0];
        }

        public bool GetBool()
        {
            return (GetBytes(1)[0] != 0);
        }

        public char GetChar()
        {
            return BitConverter.ToChar(GetBytes(2), 0);
        }

        public short GetShort()
        {
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt16(GetBytes(2), 0));
        }

        public short PeekShort()
        {
            short result = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(GetBytes(2), 0));
            _readCurrentPosition -= 2;                 // Move the current read position back two bytes
            return result;
        }

        public int GetInt()
        {
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(GetBytes(4), 0));
        }

        public long GetLong()
        {
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt64(GetBytes(8), 0));
        }

        public double GetDouble()
        {
            return BitConverter.ToDouble(GetBytes(8), 0);
        }

        public float GetFloat(ref int offset)
        {
            return BitConverter.ToSingle(GetBytes(4), 0);
        }

        public string GetString()
        {
            string result = string.Empty;
            short length = GetShort();
            if (length > 0)
                result = Encoding.BigEndianUnicode.GetString(GetBytes(length));
            return result;
        }

        public byte[] GetBytes(int length)
        {
            if (_readLimitStack.Count > 0 && _readCurrentPosition + length > _readLimitStack.Peek())
                throw new ApplicationException("Attempt to read beyond read limit");

            byte[] result = new byte[length];
            int bytesRead = 0;

            while (bytesRead < length)
            {
                int sectionIndex = _readCurrentPosition / SectionSize;
                int sectionOffset = _readCurrentPosition - sectionIndex * SectionSize;

                int cnt = Math.Min(SectionSize - sectionOffset, length - bytesRead);

                Buffer.BlockCopy(_sections[sectionIndex], sectionOffset, result, bytesRead, cnt);

                _readCurrentPosition += Convert.ToInt16(cnt);
                bytesRead += cnt;
            }

            return result;
        }

        public virtual byte[] ToBytes()
        {
            int bytesRead = 0;
            byte[] bytes = new byte[Length];

            for (int i = 0; i <= _addCurrentSection; i++)
            {
                int sectionBytes = SectionSize;
                if (i == _addCurrentSection)
                    sectionBytes = _addCurrentOffset;
                Buffer.BlockCopy(_sections[i], 0, bytes, bytesRead, sectionBytes);
                bytesRead += sectionBytes;
            }

            return bytes;
        }

        #endregion

        #region Read Limit Set, Restore, and Clear Methods

        public void SetNewReadLimit(short length)
        {
            _readLimitStack.Push(Convert.ToInt16(_readCurrentPosition + length));
        }

        public void RestorePreviosReadLimit()
        {
            if (_readLimitStack.Count > 0)
                _readLimitStack.Pop();
        }

        public void ClearMaxReadPosition()
        {
            _readLimitStack.Clear();
        }

        #endregion

        #region Private Methods
        private void Grow(int additionBytesNeeded)
        {
            int sectionIndex = (additionBytesNeeded / SectionSize) + 1;
            for (int i = _sections.Count - 1; i < sectionIndex; i++)
                _sections.Add(new byte[SectionSize]);
        }

        private byte[] GetSection(ref int index)
        {
            int sectionIndex = index / SectionSize;

            if (sectionIndex >= _sections.Count)
                throw new ApplicationException();

            index -= sectionIndex * SectionSize;
            return _sections[sectionIndex];
        }
        #endregion

        # region Static Byte Parsing Functions
        public static string GetString(byte[] bytes, ref int offset, bool isNullTerminated)
        {
            short length = BitConverter.ToInt16(bytes, offset);
            offset += 2;
            string result = Encoding.BigEndianUnicode.GetString(bytes, offset, length);
            offset += length;
            if (isNullTerminated)
                offset++;
            return result;
        }
        # endregion

    }
}



