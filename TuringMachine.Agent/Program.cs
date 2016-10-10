using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TuringMachine.Helpers;

namespace TuringMachine.Agent
{
    class Program
    {
        static AgentConfig Config;
        static bool Cancel = false;

        static int Main(string[] args)
        {
#if DEBUG
            // Forze copy for test
            object dummy = new BasicAgents.StartProcessAndSendTcpData();
            args = new string[]
            {
                "NumTasks=1",
                "RetrySeconds=5",
                "TuringServer=127.0.0.1,7777",

                "AgentLibrary=" + Path.Combine(Application.StartupPath, "TuringMachine.BasicAgents.dll"),
                "AgentClassName=StartProcessAndSendTcpData",
                "AgentArguments={\"FileName\":\"D:/Fuzzing/vulnserver/vulnserver.exe\",\"Arguments\":\"9{Task000}\",\"ConnectTo\":\"127.0.0.1,9{Task000}\"}",
            };
#endif
            Config = new AgentConfig();
            try
            {
                Config.LoadFromArguments(args);
                if (Config.TuringServer == null) throw (new Exception("TuringServer cant be null"));
            }
            catch (Exception e)
            {
                WriteError(-1, e);
                return 0;
            }

            Type agent = Config.GetAgent();

            if (agent == null)
            {
                Console.WriteLine("Agent not found '" +
                    Config.AgentLibrary + "':'" +
                    (string.IsNullOrEmpty(Config.AgentClassName) ? "*" : Config.AgentClassName) + "'");
                return 0;
            }

            Console.CancelKeyPress += Console_CancelKeyPress;

            // Write log
            WriteSeparator(true);
            Console.WriteLine("Configuration");
            WriteSeparator(true);

            Console.WriteLine("          Server : " + Config.TuringServer.ToString());
            Console.WriteLine("           Tasks : " + Config.NumTasks.ToString());
            WriteSeparator();
            Console.WriteLine("Agent");
            WriteSeparator();

            Console.WriteLine("         Library : " + Config.AgentLibrary);
            Console.WriteLine("           Class : " + agent.ToString());

            if (!string.IsNullOrEmpty(Config.AgentArguments))
            {
                WriteSeparator();
                Console.WriteLine("Agent Properties");
                WriteSeparator();

                foreach (KeyValuePair<string, object> pi in SerializationHelper.EnumerateFromJson(Config.AgentArguments))
                {
                    object val = (pi.Value == null ? "<NULL>" : pi.Value.ToString());
                    Console.WriteLine("   " + pi.Key.PadLeft(13, ' ') + " : " + val.ToString());
                }
            }
            Console.WriteLine("");

            TuringTask[] task = new TuringTask[Config.NumTasks];

            while (!Cancel)
            {
                for (int x = 0; x < Config.NumTasks; x++)
                {
                    TuringTask t = task[x];

                    // No task or compleated
                    if (t == null || t.IsCompleted)
                    {
                        if (t != null)
                        {
                            if (t.Task != null && t.Task.Exception != null)
                                WriteError(x, t.Task.Exception);

                            // Send result
                            t.Dispose();
                        }

                        try
                        {
                            task[x] = t = new TuringTask(agent, Config.AgentArguments, x);
                            t.ConnectTo(Config.TuringServer);
                        }
                        catch (Exception e)
                        {
                            if (t != null)
                                t.SetException(e);

                            WriteError(x, e);
                            continue;
                        }

                        // Check
                        if (t == null || t.Task == null)
                        {
                            WriteError(x, new Exception("Agent return empty task"));
                            return 1;
                        }

                        t.Task.Start();
                        continue;
                    }
                }

                Thread.Sleep(1);
            }

            return 1;
        }
        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Cancel = true;
        }
        #region Log
        static void WriteSeparator(bool bold = false)
        {
            Console.WriteLine("".PadLeft(Console.WindowWidth - 1, bold ? '═' : '─'));
        }
        static void WriteError(int taskNum, Exception e)
        {
            // Red
            ConsoleColor f = ConsoleColor.White;
            try
            {
                f = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
            }
            catch { }

            WriteSeparator(true);

            if (taskNum >= 0) Console.WriteLine("Error at task " + taskNum.ToString() + " retry in " + Config.RetrySeconds + " seconds");
            else Console.WriteLine("Error");

            WriteSeparator();

            Console.WriteLine(Config.Verbose ? e.ToString() : e.Message.ToString());
            WriteSeparator(true);

            if (taskNum >= 0) Thread.Sleep(Config.RetrySeconds * 1000);

            // White
            try
            {
                Console.ForegroundColor = f;
            }
            catch { }
        }
        #endregion
    }
}