using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TuringMachine.Client;
using TuringMachine.Client.Detectors;
using TuringMachine.Client.Sockets;
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
        /// ResultData
        /// </summary>
        byte[] ResultData { get; set; }
        /// <summary>
        /// ResultExtension
        /// </summary>
        string ResultExtension { get; set; }
        /// <summary>
        /// Return
        /// </summary>
        EFuzzingReturn Result { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="agent">Agent</param>
        /// <param name="arguments">Arguments</param>
        /// <param name="taskNumber">TaskNumber</param>
        public TuringTask(Type agent, string arguments, int taskNumber)
        {
            Result = EFuzzingReturn.Test;

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
                // Create detector
                ICrashDetector crash = Agent.CreateDetector(Socket);
                if (crash == null) return EFuzzingReturn.Fail;

                // Run action
                try { Agent.OnRun(Socket); } catch { }

                // Detect are alive
                try { crash.SetIsAlive(Agent.GetItsAlive(Socket)); } catch { }

                byte[] crashData;
                string crashExtension;

                if (crash.IsCrashed(out crashData, out crashExtension))
                {
                    ResultData = crashData;
                    ResultExtension = crashExtension;

                    // Free and return
                    if (crash is IDisposable) ((IDisposable)crash).Dispose();
                    return EFuzzingReturn.Crash;
                }

                // Free and return
                if (crash is IDisposable) ((IDisposable)crash).Dispose();
                return EFuzzingReturn.Test;
            });
        }
        /// <summary>
        /// Set exception
        /// </summary>
        /// <param name="e">Exception</param>
        public void SetException(Exception e)
        {
            if (Result != EFuzzingReturn.Crash && e != null)
            {
                // Error result
                if (ResultData == null || ResultData.Length == 0)
                {
                    ResultData = Encoding.UTF8.GetBytes(e.ToString());
                    ResultExtension = "txt";
                }

                Result = EFuzzingReturn.Fail;
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
                else
                {
                    // Check if crash
                    if (Task.Result == EFuzzingReturn.Crash)
                        Result = EFuzzingReturn.Crash;
                }

                Task.Dispose();
                Task = null;
            }

            if (Socket != null)
            {
                // Send end
                //ResultData
                //ResultExtension
                //Result

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