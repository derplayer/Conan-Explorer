using System;
using System.Collections.Generic;

namespace ConanExplorer.Conan
{
    public class CircularBuffer
    {
        private readonly Queue<byte> _queue;
        private readonly UInt32 _length;
        private UInt32 _cursor;

        public UInt32 Cursor { get { return _cursor; } }
        public byte[] Data { get { return _queue.ToArray(); } }


        public CircularBuffer(UInt32 length)
        {
            _queue = new Queue<byte>((int)length);
            _length = length;
            _cursor = 0;
        }

        private void AddByte(byte value)
        {
            if (_cursor >= _length)
            {
                _queue.Dequeue();
                _queue.Enqueue(value);
                return;
            }
            _queue.Enqueue(value);
            _cursor++;
        }

        public void AddData(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                AddByte(data[i]);
            }
        }

        /// <summary>
        /// Gets data from the circular buffer and adds the data that is being read to the buffer
        /// </summary>
        /// <param name="position">Position in the buffer</param>
        /// <param name="length">Length of the data</param>
        /// <returns>Byte array</returns>
        public byte[] GetData(UInt32 position, UInt32 length)
        {
            if (position >= _length) return null;
            byte[] result = new byte[length];
            List<byte> array = new List<byte>(_queue.ToArray());
            for (int i = 0; i < length; i++)
            {
                if (position + i >= array.Count)
                {
                    result[i] = array[(int)position + i - 1];
                    AddByte(array[(int)position + i - 1]);
                    array.Add(array[(int)position + i - 1]);
                }
                result[i] = array[(int)position + i];
                AddByte(array[(int)position + i]);
                array.Add(array[(int)position + i]);
            }
            return result;
        }

    }
}
