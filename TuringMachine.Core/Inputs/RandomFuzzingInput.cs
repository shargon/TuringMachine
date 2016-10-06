using System;
using System.IO;
using TuringMachine.Core.Interfaces;
using TuringMachine.Helpers;

namespace TuringMachine.Core.Inputs
{
    public class RandomFuzzingInput : IFuzzingInput
    {
        class RandomStream : Stream
        {
            long _Index;
            long _Length;

            public RandomStream(long length)
            {
                _Length = length;
                _Index = 0;
            }

            public override bool CanRead { get { return true; } }
            public override bool CanSeek { get { return true; } }
            public override bool CanWrite { get { return true; } }
            public override long Length { get { return _Length; } }
            public override long Position
            {
                get { return _Index; }
                set { _Index = Math.Max(0, Math.Min(_Length, value)); }
            }
            public override void Flush() { }
            public override int Read(byte[] buffer, int offset, int count)
            {
                int lee = 0;

                while (_Index < _Length && count > 0)
                {
                    buffer[offset] = RandomHelper.GetRandom(byte.MinValue, byte.MaxValue);

                    _Index++;
                    offset++;
                    count--;
                    lee++;
                }

                return lee;
            }
            public override long Seek(long offset, SeekOrigin origin)
            {
                switch (origin)
                {
                    case SeekOrigin.Begin: Position = offset; break;
                    case SeekOrigin.Current: Position += offset; break;
                    case SeekOrigin.End: Position = Length - offset; break;
                }
                return _Index;
            }
            public override void SetLength(long value) { _Length = value; }
            public override void Write(byte[] buffer, int offset, int count) { }
        }

        /// <summary>
        /// Length
        /// </summary>
        public FromToValue<long> Length { get; private set; }
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get { return "Random"; } }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filename">File</param>
        public RandomFuzzingInput(FromToValue<long> length)
        {
            Length = length;
        }
        /// <summary>
        /// Get file stream
        /// </summary>
        public Stream GetStream()
        {
            return new RandomStream(Length.Get());
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return "Random " + Length.ToString() + " bytes";
        }
    }
}