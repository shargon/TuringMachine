using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TuringMachine.Core.Arguments;
using TuringMachine.Core.Detectors;
using TuringMachine.Core.Enums;
using TuringMachine.Core.Interfaces;
using TuringMachine.Core.Sockets;
using TuringMachine.Core.Sockets.Messages;
using TuringMachine.Helpers;

namespace TuringMachine.Agent
{
    public class TuringTask : IDisposable
    {
        /// <summary>
        /// Return if Task its Completed
        /// </summary>
        public bool IsCompleted { get { return Task == null || Task.IsCompleted || Task.IsCanceled || Task.IsFaulted; } }

        /// <summary>
        /// Task
        /// </summary>
        public Task<EFuzzingReturn> Task { get; private set; }
        /// <summary>
        /// Socket
        /// </summary>
        private TuringSocket Socket { get; set; }
        /// <summary>
        /// Agent
        /// </summary>
        private ITuringMachineAgent Agent { get; set; }

        /// <summary>
        /// Result
        /// </summary>
        EndTaskMessage Result { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="agent">Agent</param>
        /// <param name="arguments">Arguments</param>
        /// <param name="taskNumber">TaskNumber</param>
        public TuringTask(Type agent, string arguments, int taskNumber)
        {
            if (string.IsNullOrEmpty(arguments))
                arguments = "{}";

            arguments = arguments.Replace("{Task}", taskNumber.ToString());
            arguments = arguments.Replace("{Task00}", taskNumber.ToString().PadLeft(2, '0'));
            arguments = arguments.Replace("{Task000}", taskNumber.ToString().PadLeft(3, '0'));

            Agent = (ITuringMachineAgent)SerializationHelper.DeserializeFromJson(arguments, agent);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="remoteEndPoint">EndPoint</param>
        public void ConnectTo(IPEndPoint remoteEndPoint)
        {
            Socket = TuringSocket.ConnectTo(remoteEndPoint);

            Task = new Task<EFuzzingReturn>(() =>
            {
                TuringAgentArgs e = new TuringAgentArgs();

                ICrashDetector crash = null;
                try
                {
                    //Agent.OnLoad(Socket, e);

                    // Create detector
                    crash = Agent.GetCrashDetector(Socket, e);
                    if (crash == null) return EFuzzingReturn.Fail;

                    // Run action

                    Agent.OnRun(Socket, e);

                    byte[] zipData;

                    if (crash.IsCrashed(Socket, out zipData, new ITuringMachineAgent.delItsAlive(Agent.GetItsAlive), e))
                    {
                        Result = new EndTaskMessage(EFuzzingReturn.Crash) { ZipData = zipData, };
                        return EFuzzingReturn.Crash;
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    // Free and return
                    if (crash != null && crash is IDisposable) ((IDisposable)crash).Dispose();
                    //Agent.OnFree(Socket, e);
                }
                return EFuzzingReturn.Test;
            });
        }
        /// <summary>
        /// Set exception
        /// </summary>
        /// <param name="e">Exception</param>
        public void SetException(Exception e)
        {
            if (Result == null && e != null)
            {
                // Error result
                byte[] zip = null;
                if (ZipHelper.AppendOrCreateZip(ref zip, new ZipHelper.FileEntry[] { new ZipHelper.FileEntry("exception.txt", Encoding.UTF8.GetBytes(e.ToString())) }) > 0)
                    Result = new EndTaskMessage(EFuzzingReturn.Fail) { ZipData = zip };
            }
        }
        /// <summary>
        /// Free resources
        /// </summary>
        public void Dispose()
        {
            // Send end
            if (Task != null)
            {
                if (Task.Exception != null)
                {
                    // Error result
                    SetException(Task.Exception);
                }

                Task.Dispose();
                Task = null;
            }

            if (Socket != null)
            {
                // Test
                if (Result == null) Socket.SendMessage(new EndTaskMessage(EFuzzingReturn.Test));
                // Other result
                else Socket.SendMessage(Result);

                TuringMessage msg = Socket.ReadMessage<TuringMessage>();

                Socket.Dispose();
                Socket = null;
            }
            if (Agent != null && Agent is IDisposable)
            {
                ((IDisposable)Agent).Dispose();
                Agent = null;
            }
        }
    }
}