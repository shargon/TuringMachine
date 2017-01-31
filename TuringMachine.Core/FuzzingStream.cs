using System;
using System.Collections.Generic;
using System.IO;
using TuringMachine.Core.Collections;
using TuringMachine.Core.FuzzingMethods.Patchs;
using TuringMachine.Core.Interfaces;
using TuringMachine.Helpers;

namespace TuringMachine.Core
{
    public class FuzzingStream : Stream
    {
        Stream _Source;
        MemoryStream _Original;
        bool _ReadedAll;
        long _RealOffset;
        List<byte> _Buffer;
        List<PatchChange> _Log;

        /// <summary>
        /// Fuzzing conditions
        /// </summary>
        public IGetPatch Config { get; private set; }
        /// <summary>
        /// Variables
        /// </summary>
        public VariableCollection<string, object> Variables { get; private set; }
        /// <summary>
        /// Readed
        /// </summary>
        public byte[] OriginalData { get { return _Original.ToArray(); } }
        /// <summary>
        /// Log
        /// </summary>
        public PatchChange[] Log { get { return _Log.ToArray(); } }
        /// <summary>
        /// Sample Id
        /// </summary>
        public Guid SampleId { get; private set; }
        /// <summary>
        /// Input name
        /// </summary>
        public string InputName { get; set; }
        /// <summary>
        /// Config name
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// Fuzzer constructor
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="config">Mutations</param>
        public FuzzingStream(byte[] stream, IGetPatch config) : this(new MemoryStream(stream), config) { }
        /// <summary>
        /// Fuzzer constructor
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="config">Mutations</param>
        public FuzzingStream(Stream stream, IGetPatch config)
        {
            _RealOffset = 0;
            _Source = stream;
            Config = config;
            _Original = new MemoryStream();
            _Log = new List<PatchChange>();
            _Buffer = new List<byte>();
            SampleId = Guid.NewGuid();
            Variables = new VariableCollection<string, object>();
            if (Config != null) Config.InitFor(this);
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
            if (count <= 0 || _ReadedAll)
                return 0;

            int lee;
            if (Config == null)
            {
                // No fuzzing
                lee = ReadFromOriginal(_Source, buffer, ref offset, count);
            }
            else
            {
                // Read buffer first
                lee = ReadBuffer(ref buffer, ref offset, ref count);

                // Perform mutations (byte peer byte)
                while (count > 0)
                {
                    PatchChange log = null;

                    // If no buffer are available (FUZZ!)
                    if (_Buffer.Count == 0)
                        log = Config.Get(this);

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
                            int ix = 0;
                            byte[] r = new byte[log.Remove];
                            int ret = ReadFromOriginal(_Source, r, ref ix, log.Remove);
                            if (ret <= 0) break;
                        }
                    }

                    int c = 1;
                    // Read buffer first (if available)
                    int d = ReadBuffer(ref buffer, ref offset, ref c);
                    if (d <= 0)
                    {
                        // Peek one byte if not from buffer
                        d = ReadFromOriginal(_Source, buffer, ref offset, 1);
                        if (d <= 0) break;
                    }

                    lee += d;
                    count -= d;
                }
            }

            return lee;
        }
        int ReadFromOriginal(Stream source, byte[] buffer, ref int offset, int v)
        {
            // Try read from original
            int saveOffset = offset;
            int lee = StreamHelper.ReadFull(source, buffer, ref offset, v);

            if (lee <= 0) _ReadedAll = true;
            else
            {
                _RealOffset += lee;
                _Original.Write(buffer, offset - lee, lee);
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
        public void AppendToSource(byte[] buffer, int offset, int count, bool reSeek)
        {
            long ps = 0;
            if (reSeek) ps = _Source.Position;
            _Source.Write(buffer, offset, count);
            if (reSeek) _Source.Position = ps;
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (count <= 0) return;

            _RealOffset += count;
            //_Original.Write(buffer, offset , count);
            //if (!_FuzzWrite)
            //{
            //    _Source.Write(buffer, offset, count);
            //    return;
            //}
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
            _Original.Dispose();
            Variables.Dispose();
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