using System;
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
        /// Constructor
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="agent">Agent</param>
        /// <param name="arguments">Arguments</param>
        /// <param name="taskNumber">TaskNumber</param>
        public TuringTask(TuringSocket socket, Type agent, string arguments, int taskNumber)
        {
            Socket = socket;

            if (string.IsNullOrEmpty(arguments))
                arguments = "{}";

            arguments = arguments.Replace("{Task}", taskNumber.ToString());
            arguments = arguments.Replace("{Task00}", taskNumber.ToString().PadLeft(2, '0'));
            arguments = arguments.Replace("{Task000}", taskNumber.ToString().PadLeft(3, '0'));

            Agent = (ITuringMachineAgent)SerializationHelper.DeserializeFromJson(arguments, agent);
            Task = new Task<EFuzzingReturn>(() =>
            {
                ICrashDetector crash = Agent.Run(socket);
                if (crash == null) return EFuzzingReturn.Fail;

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
        /// Free resources
        /// </summary>
        public void Dispose()
        {
            // Send end
            EFuzzingReturn ret = EFuzzingReturn.Fail;

            if (Task != null)
            {
                if (Task.Exception != null)
                {
                    // Error result
                    ret = EFuzzingReturn.Fail;

                    if (ResultData == null || ResultData.Length == 0)
                    {
                        ResultData = Encoding.UTF8.GetBytes(Task.Exception.ToString());
                        ResultExtension = "txt";
                    }
                }
                else
                {
                    // Result of task
                    ret = Task.Result;
                }

                Task.Dispose();
                Task = null;
            }

            if (Socket != null)
            {
                // Send end


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