using System;
using System.Collections.Generic;
using System.IO;

namespace TuringMachine.Core.Mutational
{
    public class MutationalStream : Stream
    {
        Stream _Source;
        long _RealOffset;
        List<byte> _Buffer;
        List<MutationLog> _Log;
        MutationLog[] _Replicate;

        /// <summary>
        /// Fuzzing conditions
        /// </summary>
        public MutationConfig Config { get; private set; }
        /// <summary>
        /// Log
        /// </summary>
        public MutationLog[] Log { get { return _Log.ToArray(); } }
        /// <summary>
        /// Sample Id
        /// </summary>
        public string SampleId { get; private set; }

        /// <summary>
        /// Fuzzer constructor
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="config">Mutations</param>
        /// <param name="sampleId">Mutation sample id</param>
        public MutationalStream(Stream stream, MutationConfig config, string sampleId)
        {
            _Source = stream;
            Config = config;
            _RealOffset = 0;
            _Buffer = new List<byte>();
            SampleId = sampleId;
            _Log = new List<MutationLog>();
        }
        /// <summary>
        /// Replicate constructor
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="replicate">Replicate log</param>
        public MutationalStream(Stream stream, MutationLog[] replicate)
        {
            _Source = stream;
            Config = null;
            _RealOffset = 0;
            _Buffer = new List<byte>();
            SampleId = null;
            _Log = new List<MutationLog>();
            _Replicate = replicate;
        }

        #region Read
        public override bool CanRead { get { return _Source.CanRead; } }
        public override bool CanSeek { get { return _Source.CanSeek; } }
        public override long Length { get { return _Source.Length; } }
        public override int WriteTimeout { get { return _Source.WriteTimeout; } set { _Source.WriteTimeout = value; } }
        public override int ReadTimeout { get { return _Source.ReadTimeout; } set { _Source.ReadTimeout = value; } }
        public override bool CanTimeout { get { return _Source.CanTimeout; } }
        public override long Position
        {
            get { return _RealOffset; }
            set
            {
                _Source.Position = value;
                _RealOffset = value;
            }
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (count <= 0) return 0;

            // Read buffer first
            int lee = ReadBuffer(ref buffer, ref offset, ref count);

            // Perform mutations (byte peer byte)
            while (count > 0)
            {
                MutationLog log = null;

                // If no buffer are available
                if (_Buffer.Count == 0)
                    log = GetNext(_RealOffset);

                // If change!
                if (log != null)
                {
                    // Add to log
                    _Log.Add(log);

                    // Add to buffer
                    if (log.Append != null)
                        _Buffer.AddRange(log.Append);

                    // Remove from source
                    if (log.Remove > 0)
                    {
                        byte[] r = new byte[log.Remove];
                        int ret= StreamHelper.ReadFull(_Source, r, 0, log.Remove);
                        if (ret <= 0)
                        {
                            count = 0;
                            break;
                        }

                        _RealOffset += ret;
                    }
                }

                int c = 1;
                // Read buffer first (if available)
                int d = ReadBuffer(ref buffer, ref offset, ref c);
                if (d <= 0)
                {
                    // Peek one byte if not from buffer
                    d = StreamHelper.ReadFull(_Source, buffer, ref offset, 1);
                    if (d <= 0)
                    {
                        count = 0;
                        break;
                    }
                    _RealOffset += d;
                }

                lee += d;
                count -= d;
            }

            return lee;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            _RealOffset = _Source.Seek(offset, origin);
            return _RealOffset;
        }
        #endregion

        #region Write
        public override bool CanWrite { get { return _Source.CanWrite; } }
        public override void Flush() { _Source.Flush(); }

        public override void SetLength(long value)
        {
            _RealOffset = value;
            _Source.SetLength(value);
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            _RealOffset += count;
            _Source.Write(buffer, offset, count);
        }
        #endregion

        #region Privates
        public override void Close()
        {
            _Source.Close();
            base.Close();
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _Source.Dispose();
        }
        /// <summary>
        /// Get next MutationChange
        /// </summary>
        /// <param name="offset">Offset</param>
        MutationLog GetNext(long offset)
        {
            if (Config != null)
            {
                MutationLog next = Config.Get(offset);
                if (next != null)
                    return next;
            }
            if (_Replicate != null)
            {
                // Replicate
                foreach (MutationLog cond in _Replicate)
                {
                    if (cond.Offset == offset)
                        return cond;
                }
            }
            return null;
        }
        /// <summary>
        /// Read from buffer
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="offset">Offset</param>
        /// <param name="count">Count</param>
        int ReadBuffer(ref byte[] buffer, ref int offset, ref int count)
        {
            int read = Math.Min(count, _Buffer.Count);
            // buffer empty or count <= 0
            if (read <= 0) return 0;

            for (int x = 0; x < read; x++)
                buffer[offset + x] = _Buffer[x];

            // increment readed
            count -= read;
            offset += read;

            _Buffer.RemoveRange(0, read);
            return read;
        }
        #endregion
    }
}