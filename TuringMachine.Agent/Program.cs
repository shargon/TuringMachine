using Newtonsoft.Json.Linq;
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
        static int X = 0, Y = 0;

        static int Main(string[] args)
        {
#if DEBUG
            // Forze copy for test
            object dummy = new BasicAgents.StartProcessAndSendTcpData();
            args = new string[]
            {
                "NumTasks=8",
                "RetrySeconds=5",
                "TuringServer=127.0.0.1,7777",

                "AgentLibrary=" + Path.Combine(Application.StartupPath, "TuringMachine.BasicAgents.dll"),
                "AgentClassName=StartProcessAndSendTcpData",
                "AgentArguments=cfg_vulnserver.json",
            };

            args = new string[]
            {
                "NumTasks=9",
                "RetrySeconds=5",
                "TuringServer=127.0.0.1,7777",

                "AgentLibrary=" + Path.Combine(Application.StartupPath, "TuringMachine.BasicAgents.dll"),
                "AgentClassName=StartProcessAndInvisibleProxy",
                //"AgentArguments=cfg_mysql.json",
                "AgentArguments=cfg_mysqld.json"
            };
#endif

            // Initialize winDbg !exploitable
            WinDbgHelper.WinDbgPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Windows Kits\10\Debuggers\x64\windbg.exe");
            if (!File.Exists(WinDbgHelper.WinDbgPath)) WinDbgHelper.WinDbgPath = null;

            // Parse arguments
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

            const int PaddingLeft = 25;

            Console.WriteLine("Server".PadLeft(PaddingLeft, ' ') + " : " + Config.TuringServer.ToString());
            Console.WriteLine("Tasks".PadLeft(PaddingLeft, ' ') + " : " + Config.NumTasks.ToString());
            WriteSeparator();
            Console.WriteLine("Agent");
            WriteSeparator();

            Console.WriteLine("Library".PadLeft(PaddingLeft, ' ') + " : " + Config.AgentLibrary);
            Console.WriteLine("Class".PadLeft(PaddingLeft, ' ') + " : " + agent.ToString());

            if (!string.IsNullOrEmpty(Config.AgentArguments))
            {
                WriteSeparator();
                Console.WriteLine("Agent Properties");
                WriteSeparator();

                foreach (KeyValuePair<string, object> pi in SerializationHelper.EnumerateFromJson(Config.AgentArguments))
                {
                    if (pi.Value is JObject)
                    {
                        foreach (JToken pi2 in ((JObject)pi.Value).Values())
                        {
                            if (pi2 is JValue)
                            {
                                JValue jval = (JValue)pi2;

                                string val = (jval.Value == null ? "<NULL>" : jval.Value.ToString());
                                string name = jval.Path;
                                Console.WriteLine(name.PadLeft(PaddingLeft, ' ') + " : " + val.Replace("\n", "\n                    "));
                            }
                        }
                    }
                    else
                    {
                        if (pi.Value is JArray)
                        {
                            JArray arr = (JArray)pi.Value;
                            foreach (JObject ojb in arr)
                            {
                                foreach (JToken pi2 in (ojb).Values())
                                {
                                    if (pi2 is JValue)
                                    {
                                        JValue jval = (JValue)pi2;

                                        string val = (jval.Value == null ? "<NULL>" : jval.Value.ToString());
                                        string name = jval.Path;
                                        if (arr.Count == 1) name = name.Replace("[0]", "");

                                        Console.WriteLine(name.PadLeft(PaddingLeft, ' ') + " : " + val.Replace("\n", "\n                    "));
                                    }
                                }
                            }
                        }
                        else
                        {
                            string val = (pi.Value == null ? "<NULL>" : pi.Value.ToString());
                            string name = pi.Key;
                            Console.WriteLine(name.PadLeft(PaddingLeft, ' ') + " : " + val.Replace("\n", "\n                    "));
                        }
                    }
                }
            }
            Console.WriteLine("");

            //Console.WriteLine("");
            //WriteSeparator(true);

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

                        //try { t.InternalJob(); }
                        //catch (Exception exx) { }
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

            if (X == 0 && Y == 0)
            {
                X = Console.CursorLeft;
                Y = Console.CursorTop;
            }
            else Console.SetCursorPosition(X, Y);

            WriteSeparator(true);
            if (taskNum >= 0) Console.WriteLine("Error at task " + taskNum.ToString() + " retry in " + Config.RetrySeconds + " seconds");
            else Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " - Error");

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