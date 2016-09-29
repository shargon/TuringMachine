using System.Collections.Generic;
using System.IO;
using TuringMachine.Core.Mutational.Changes;

namespace TuringMachine.Core.Mutational
{
    public class MutationalStream : Stream
    {
        Stream _Stream;
        ulong _Offset;

        /// <summary>
        /// Fuzzing conditions
        /// </summary>
        public List<MutationalOffset> Conditions { get; set; }

        /// <summary>
        /// Offset
        /// </summary>
        public ulong Offset { get { return _Offset; } }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stream">Stream</param>
        public MutationalStream(Stream stream)
        {
            _Stream = stream;
            _Offset = 0;
        }

        #region Read
        public override bool CanRead { get { return _Stream.CanRead; } }
        public override bool CanSeek { get { return _Stream.CanSeek; } }
        public override long Length { get { return _Stream.Length; } }
        public override long Position { get { return _Stream.Position; } set { _Stream.Position = value; _Offset = (ulong)value; } }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int saveOffset = offset, saveCount = count;

            // Leer todo
            while (saveCount > 0)
            {
                int ret = _Stream.Read(buffer, saveOffset, saveCount);
                if (ret > 0)
                {
                    _Offset += (ulong)ret;

                    saveCount -= ret;
                    saveOffset += ret;
                }
            }

            foreach (MutationalOffset cond in Conditions)
            {
                // Si no está permitido dentro de este offset no se tiene en cuenta
                if (!cond.Value.AreBetween(_Offset)) continue;

                // TODO fuz!
            }

            return count;
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            _Offset = (ulong)_Stream.Seek(offset, origin);
            return (long)_Offset;
        }
        #endregion

        #region Write
        public override bool CanWrite { get { return _Stream.CanWrite; } }
        public override void Flush() { _Stream.Flush(); }

        public override void SetLength(long value)
        {
            _Offset = (ulong)value;
            _Stream.SetLength(value);
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            _Offset += (ulong)count;
            _Stream.Write(buffer, offset, count);
        }
        #endregion
    }
}