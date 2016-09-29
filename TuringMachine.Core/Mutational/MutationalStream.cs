using System;
using System.Collections.Generic;
using System.IO;
using TuringMachine.Core.Mutational.Changes;

namespace TuringMachine.Core.Mutational
{
    public class MutationalStream : Stream
    {
        Stream _Source;
        ulong _Offset;
        List<byte> _Buffer;

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
            _Source = stream;
            _Offset = 0;
            _Buffer = new List<byte>();
        }

        #region Read
        public override bool CanRead { get { return _Source.CanRead; } }
        public override bool CanSeek { get { return _Source.CanSeek; } }
        public override long Length { get { return _Source.Length; } }
        public override long Position { get { return _Source.Position; } set { _Source.Position = value; _Offset = (ulong)value; } }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (count <= 0) return 0;

            // Read buffer
            ReadBuffer(ref buffer, ref offset, ref count);

            if (count <= 0) return 0;

            foreach (MutationalOffset cond in Conditions)
            {
                // Check offset
                if (!cond.Value.AreIn(_Offset)) continue;

                // check byte by byte
                for (int x = 0; x < count; x++)
                {
                    IMutationalChange change = cond.Get();
                    if (change == null) continue;

                    int remove;
                    byte[] add = change.Process(out remove);

                    if (remove > 0)
                    {
                        // Remove from source
                        byte[] r = new byte[remove];
                        StreamHelper.ReadFull(_Source, r, 0, remove);
                    }

                    // Add to buffer
                    if (add != null) _Buffer.AddRange(add);
                }
            }

            // Read buffer
            ReadBuffer(ref buffer, ref offset, ref count);

            // Read direct
            while (count > 0)
            {
                int ret = _Source.Read(buffer, offset, count);
                if (ret > 0)
                {
                    _Offset += (ulong)ret;

                    count -= ret;
                    offset += ret;
                }
            }

            return count;
        }
        /// <summary>
        /// Read from buffer
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="offset">Offset</param>
        /// <param name="count">Count</param>
        void ReadBuffer(ref byte[] buffer, ref int offset, ref int count)
        {
            int lee = Math.Min(count, _Buffer.Count);

            if (lee <= 0) return;

            for (int x = 0; x < lee; x++)
                buffer[offset + x] = _Buffer[x];

            count -= lee;
            offset += lee;

            _Buffer.RemoveRange(0, lee);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            _Offset = (ulong)_Source.Seek(offset, origin);
            return (long)_Offset;
        }
        #endregion

        #region Write
        public override bool CanWrite { get { return _Source.CanWrite; } }
        public override void Flush() { _Source.Flush(); }

        public override void SetLength(long value)
        {
            _Offset = (ulong)value;
            _Source.SetLength(value);
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            _Offset += (ulong)count;
            _Source.Write(buffer, offset, count);
        }
        #endregion
    }
}