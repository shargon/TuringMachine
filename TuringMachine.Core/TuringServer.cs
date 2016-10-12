using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using TuringMachine.Core.Enums;
using TuringMachine.Core.FuzzingMethods.Mutational;
using TuringMachine.Core.FuzzingMethods.Patchs;
using TuringMachine.Core.Interfaces;
using TuringMachine.Core.Sockets;
using TuringMachine.Core.Sockets.Enums;
using TuringMachine.Core.Sockets.Messages;
using TuringMachine.Core.Sockets.Messages.Requests;
using TuringMachine.Core.Sockets.Messages.Responses;
using TuringMachine.Helpers;

namespace TuringMachine.Core
{
    /// <summary>
    /// [Client] <<----  Read Fuzz Information      <<----  [Server]  <<---- [Inputs]
    ///          ----->> Write Information for Fuz  ---------------------->>
    /// </summary>
    public class TuringServer
    {
        public delegate void delOnTestEnd(object sender, EFuzzingReturn result, FuzzerStat<IFuzzingInput>[] sinput, FuzzerStat<IFuzzingConfig>[] sconfig);
        public delegate void delOnCrashLog(object sender, FuzzerLog log);

        public event EventHandler OnListenChange;

        public event delOnTestEnd OnTestEnd;
        public event delOnCrashLog OnCrashLog;

        bool _Paused;
        EFuzzerState _State;
        TuringSocket _Socket;
        IPEndPoint _EndPoint = new IPEndPoint(IPAddress.Any, 7777);
        /// <summary>
        /// Inputs
        /// </summary>
        public ObservableCollection<FuzzerStat<IFuzzingInput>> Inputs { get; private set; }
        /// <summary>
        /// Configurations
        /// </summary>
        public ObservableCollection<FuzzerStat<IFuzzingConfig>> Configurations { get; private set; }
        /// <summary>
        /// Logs
        /// </summary>
        public ObservableCollection<FuzzerLog> Logs { get; private set; }
        /// <summary>
        /// Listen address
        /// </summary>
        public IPEndPoint Listen
        {
            get { return _EndPoint; }
            set
            {
                _EndPoint = value;
                OnListenChange?.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// State
        /// </summary>
        public EFuzzerState State { get { return _State; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public TuringServer()
        {
            Inputs = new ObservableCollection<FuzzerStat<IFuzzingInput>>();
            Configurations = new ObservableCollection<FuzzerStat<IFuzzingConfig>>();
            Logs = new ObservableCollection<FuzzerLog>();
        }
        /// <summary>
        /// Add input
        /// </summary>
        /// <param name="input">Input</param>
        public void AddInput(IFuzzingInput input)
        {
            if (Inputs == null) return;

            Inputs.Add(new FuzzerStat<IFuzzingInput>(input));
        }
        /// <summary>
        /// Add config
        /// </summary>
        /// <param name="file">File</param>
        public void AddConfig(string file)
        {
            if (string.IsNullOrEmpty(file)) return;
            if (!File.Exists(file)) return;

            switch (Path.GetExtension(file).ToLowerInvariant())
            {
                case ".fmut":
                    {
                        MutationConfig c = null;
                        try { c = MutationConfig.FromJson(File.ReadAllText(file, Encoding.UTF8)); } catch { }
                        if (c != null)
                        {
                            Configurations.Add(new FuzzerStat<IFuzzingConfig>(c));
                        }
                        break;
                    }
                case ".fpatch":
                    {
                        PatchConfig c = null;
                        try { c = PatchConfig.FromJson(File.ReadAllText(file, Encoding.UTF8)); } catch { }
                        if (c != null)
                        {
                            Configurations.Add(new FuzzerStat<IFuzzingConfig>(c));
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Raise on crash log
        /// </summary>
        /// <param name="log">Log</param>
        public void RaiseOnCrashLog(FuzzerLog log)
        {
            if (log == null) return;

            Logs.Add(log);
            OnCrashLog?.Invoke(this, log);
        }
        /// <summary>
        /// Raise end of test
        /// </summary>
        /// <param name="result">Result</param>
        void RaiseOnTestEnd(EFuzzingReturn result, FuzzerStat<IFuzzingInput>[] sinput, FuzzerStat<IFuzzingConfig>[] sconfig)
        {
            if (sinput != null) foreach (FuzzerStat<IFuzzingInput> i in sinput) i.Increment(result);
            if (sconfig != null) foreach (FuzzerStat<IFuzzingConfig> i in sconfig) i.Increment(result);

            OnTestEnd?.Invoke(this, result, sinput, sconfig);
        }
        /// <summary>
        /// Stop logic
        /// </summary>
        public bool Stop()
        {
            _State = EFuzzerState.Stopped;
            _Paused = false;
            if (_Socket != null)
            {
                _Socket.Dispose();
                _Socket = null;
            }
            return true;
        }
        /// <summary>
        /// Start logic
        /// </summary>
        public bool Start()
        {
            Stop();
            _State = EFuzzerState.Started;
            _Socket = TuringSocket.Bind(Listen);
            _Socket.OnMessage += _Socket_OnMessage;
            return true;
        }
        /// <summary>
        /// Pause logic
        /// </summary>
        public bool Pause()
        {
            _State = EFuzzerState.Paused;
            _Paused = !_Paused;
            return true;
        }
        /// <summary>
        /// Message logic
        /// </summary>
        /// <param name="sender">Socket</param>
        /// <param name="message">Message</param>
        void _Socket_OnMessage(TuringSocket sender, TuringMessage message)
        {
            if (_Paused)
            {
                // Send Paused signal
                //sender.SendMessage(new WaitMessage(TimeSpan.FromSeconds(1)));

                // Wait disable pause
                while (_Paused) { Thread.Sleep(500); }
            }

            TuringMessage response = new ExceptionMessage("Bad request");
            try
            {
                switch (message.Type)
                {
                    case ETuringMessageType.EndTask:
                        {
                            EndTaskMessage msg = (EndTaskMessage)message;

                            List<FuzzerStat<IFuzzingInput>> sinput = null;
                            List<FuzzerStat<IFuzzingConfig>> sconfig = null;

                            // Log if are data
                            FuzzerLog log = msg.SaveResult(sender, out sinput, out sconfig);
                            if (log != null) RaiseOnCrashLog(log);

                            // Send message of the end
                            RaiseOnTestEnd(msg.Result, sinput == null ? null : sinput.ToArray(), sconfig == null ? null : sconfig.ToArray());
                            response = new BoolMessageResponse(true);
                            break;
                        }
                    case ETuringMessageType.OpenStreamRequest:
                        {
                            Guid id = Guid.NewGuid();
                            OpenStreamMessageRequest msg = (OpenStreamMessageRequest)message;
                            FuzzingStream stream = GetRandomStream(sender, true, id);

                            if (stream == null) throw new Exception("Not found stream");

                            sender[id.ToString()] = stream;

                            response = new OpenStreamMessageResponse(id)
                            {
                                CanRead = stream.CanRead,
                                CanSeek = stream.CanSeek,
                                CanTimeout = stream.CanTimeout,
                                CanWrite = stream.CanWrite
                            };
                            break;
                        }
                    case ETuringMessageType.GetStreamLengthRequest:
                        {
                            GetStreamLengthMessageRequest msg = (GetStreamLengthMessageRequest)message;
                            Stream stream = (Stream)sender[msg.Id.ToString()];
                            if (stream == null) response = new ExceptionMessage("No stream openned with id: " + msg.Id.ToString());

                            response = new LongMessageResponse(stream.Length);
                            break;
                        }
                    case ETuringMessageType.GetStreamPositionRequest:
                        {
                            GetStreamPositionMessageRequest msg = (GetStreamPositionMessageRequest)message;
                            Stream stream = (Stream)sender[msg.Id.ToString()];
                            if (stream == null) response = new ExceptionMessage("No stream openned with id: " + msg.Id.ToString());

                            response = new LongMessageResponse(stream.Position);
                            break;
                        }
                    case ETuringMessageType.SetStreamRequest:
                        {
                            SetStreamMessageRequest msg = (SetStreamMessageRequest)message;
                            Stream stream = (Stream)sender[msg.Id.ToString()];
                            if (stream == null) response = new ExceptionMessage("No stream openned with id: " + msg.Id.ToString());

                            switch (msg.ValueType)
                            {
                                case SetStreamMessageRequest.EMode.Position: stream.Position = msg.Value; break;
                                case SetStreamMessageRequest.EMode.Length: stream.SetLength(msg.Value); break;
                            }

                            response = new BoolMessageResponse(true);
                            break;
                        }
                    case ETuringMessageType.FlushStreamRequest:
                        {
                            FlushStreamMessageRequest msg = (FlushStreamMessageRequest)message;
                            Stream stream = (Stream)sender[msg.Id.ToString()];
                            if (stream == null) response = new ExceptionMessage("No stream openned with id: " + msg.Id.ToString());

                            stream.Flush();

                            response = new BoolMessageResponse(true);
                            break;
                        }
                    case ETuringMessageType.CloseStreamRequest:
                        {
                            CloseStreamMessageRequest msg = (CloseStreamMessageRequest)message;
                            Stream stream = (Stream)sender[msg.Id.ToString()];
                            if (stream == null) response = new ExceptionMessage("No stream openned with id: " + msg.Id.ToString());

                            try { stream.Close(); } catch { }
                            try { stream.Dispose(); } catch { }

                            sender[msg.Id.ToString()] = null;

                            if (stream is FuzzingStream)
                            {
                                // Save patch for dump
                                FuzzingStream fs = (FuzzingStream)stream;

                                sender["Info=" + msg.Id.ToString()] = new FuzzingLogInfo(fs);
                            }

                            response = new BoolMessageResponse(true);
                            break;
                        }
                    case ETuringMessageType.ReadStreamRequest:
                        {
                            StreamReadMessageRequest msg = (StreamReadMessageRequest)message;
                            Stream stream = (Stream)sender[msg.Id.ToString()];
                            if (stream == null) response = new ExceptionMessage("No stream openned with id: " + msg.Id.ToString());

                            byte[] data = new byte[msg.Length];
                            int r = stream.Read(data, 0, data.Length);

                            if (r != data.Length) Array.Resize(ref data, r);

                            response = new ByteArrayMessageResponse(data);
                            break;
                        }
                    case ETuringMessageType.WriteStreamRequest:
                        {
                            StreamWriteMessageRequest msg = (StreamWriteMessageRequest)message;
                            Stream stream = (Stream)sender[msg.Id.ToString()];
                            if (stream == null) response = new ExceptionMessage("No stream openned with id: " + msg.Id.ToString());

                            stream.Write(msg.Data, 0, msg.Data.Length);
                            response = new BoolMessageResponse(true);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                response = new ExceptionMessage(e.ToString());
            }
            sender.SendMessage(response);
        }
        FuzzingStream GetRandomStream(TuringSocket sender, bool fuzzer, Guid id)
        {
            FuzzerStat<IFuzzingInput> sinput = RandomHelper.GetRandom(Inputs);
            if (sinput != null)
            {
                IFuzzingInput input = sinput.Source;
                if (input == null) throw (new Exception("Require input"));

                if (sender["INPUT"] == null)
                {
                    List<FuzzerStat<IFuzzingInput>> ls = new List<FuzzerStat<IFuzzingInput>>();
                    ls.Add(sinput);
                    sender["INPUT"] = ls;
                }
                else
                {
                    List<FuzzerStat<IFuzzingInput>> ls = (List<FuzzerStat<IFuzzingInput>>)sender["INPUT"];
                    ls.Add(sinput);
                }

                if (fuzzer)
                {
                    FuzzerStat<IFuzzingConfig> sconfig = RandomHelper.GetRandom(Configurations);

                    if (sconfig != null && sconfig != null)
                    {
                        IFuzzingConfig config = sconfig.Source;
                        if (sconfig == null) throw (new Exception("Require fuzzer configuration"));

                        if (sender["CONFIG"] == null)
                        {
                            List<FuzzerStat<IFuzzingConfig>> ls = new List<FuzzerStat<IFuzzingConfig>>();
                            ls.Add(sconfig);
                            sender["CONFIG"] = ls;
                        }
                        else
                        {
                            List<FuzzerStat<IFuzzingConfig>> ls = (List<FuzzerStat<IFuzzingConfig>>)sender["CONFIG"];
                            ls.Add(sconfig);
                        }

                        FuzzingStream ret = config.CreateStream(input.GetStream());
                        if (ret != null)
                        {
                            ret.InputName = sinput.ToString();
                            ret.ConfigName = sconfig.ToString();
                        }
                        return ret;
                    }
                }

                // Disable Fuzzing
                return new FuzzingStream(input.GetStream(), null) { InputName = sinput.ToString() };
            }
            return null;
        }
    }
}