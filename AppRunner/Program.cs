using Brilliant;
using GenericDefs.DotNet;
using ProjectEuler;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static AppRunner.Program.NativeConsoleMethods;

namespace AppRunner
{
    class Program
    {
        static string _title = "DO NOT CLOSE - Important Program";
        static void Main(string[] args)
        {
            //Initiate dll, xml generation post-build.
            if (args != null && args.Length > 0) {
                if (args[0].Contains("PostBuild")) {
                    PreRunTasks();
                    return;
                }
            }

            ExecutionState es = new ExecutionState();
            es.fPreviousExecutionState = NativeMethods.SetThreadExecutionState(NativeMethods.ES_CONTINUOUS | NativeMethods.ES_SYSTEM_REQUIRED);
            if (es.fPreviousExecutionState == 0) {
                Console.WriteLine("SetThreadExecutionState failed. Do something here...");
            }
            AssignCurrentTask(es);
            SetConsoleProperties(es);
            try {
                MainAsync(args, es).Wait();
            } catch(Exception ex) {
                if (ex.InnerException != null) {
                    QueuedConsole.WriteImmediate("Inner Exception : " + ex.InnerException.Message);
                    QueuedConsole.WriteImmediate("Stack Trace : " + ex.InnerException.StackTrace);
                }
                QueuedConsole.WriteImmediate("Exception : " + ex.Message);
                QueuedConsole.WriteImmediate("Stack Trace : " + ex.StackTrace);
                QueuedConsole.ReadKey();
            }
            if (args == null || args.Length == 0) Exit();
        }

        static void Exit()
        {
            int maxTries = 10;
            int count = 0;
            while (true) {
                count++;
                Console.WriteLine("Press Y to exit.");
                ConsoleKeyInfo cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.End || cki.Key == ConsoleKey.Escape || cki.Key == ConsoleKey.Enter) {
                    continue;
                }
                else if (cki.KeyChar == 'y' || cki.KeyChar == 'Y') return;
                else if ((cki.Key == ConsoleKey.C || cki.Key == ConsoleKey.Clear) && cki.Modifiers == ConsoleModifiers.Control) {
                    return;
                }
                if (count == maxTries) {
                    Console.WriteLine("Max exit trials reached. Exiting.");
                    return;
                }
            }
        }

        static void PreRunTasks()
        {
            ProjectEnumBuilder.GenerateDlls();
        }

        static void AssignCurrentTask(ExecutionState es)
        {
            Tuple<TaskType, string> curTask = ConfigManager.GetCurrentTask();
            es.CurrentTask = curTask.Item1;
            string className = curTask.Item2;
            object _task = null;
            if (es.CurrentTask == TaskType.Brilliant) {
                Assembly assembly = typeof(ISolve).Assembly;
                Type type = assembly.GetType(className);
                _task = Activator.CreateInstance(type);
            } else if (es.CurrentTask == TaskType.ProjectEuler) {
                Assembly assembly = typeof(IProblem).Assembly;
                Type type = assembly.GetType(className);
                _task = Activator.CreateInstance(type);
            }

            if (_task == null)
            {
                throw new ArgumentNullException("Task to execute was not specified in config.");
            }
            es.CurrentRunningTask = _task;
        }

        static unsafe void SetConsoleProperties(ExecutionState es)
        {
            Console.SetWindowSize(60, 60);
            Console.SetBufferSize(100, 100);

            IntPtr hMenu = Process.GetCurrentProcess().MainWindowHandle;
            IntPtr hSystemMenu = NativeMethods.GetSystemMenu(hMenu, false);

            NativeMethods.EnableMenuItem(hSystemMenu, NativeMethods.SC_CLOSE, NativeMethods.MF_GRAYED);
            NativeMethods.RemoveMenu(hSystemMenu, NativeMethods.SC_CLOSE, NativeMethods.MF_BYCOMMAND);

            SetConsoleTitle(es);

            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Clean-up code invoked in CancelKeyPress handler. Current task : " + es.CurrentTask.ToString());
                if (es.CurrentRunningTask is ISaveProgress && ((ISaveProgress)es.CurrentRunningTask).CanSave) {
                    Task<bool> task = ((ISaveProgress)es.CurrentRunningTask).Save();
                    task.Wait();
                    bool saved = task.Result;
                    if (saved) {
                        Console.WriteLine("Progress saved successfully.");
                    } else {
                        Console.WriteLine("Failed.");
                    }
                } else {
                    Exit();
                }
            };

            ConsoleHelper.SetConsoleFont(2);
            ConsoleHelper.SetConsoleIcon(SystemIcons.Information);

            string fontName = "Lucida Console";
            IntPtr hnd = GetStdHandle(STD_OUTPUT_HANDLE);
            if (hnd != INVALID_HANDLE_VALUE) {
                CONSOLE_FONT_INFO_EX info = new CONSOLE_FONT_INFO_EX();
                info.cbSize = (uint)Marshal.SizeOf(info);
                bool tt = false;
                // First determine whether there's already a TrueType font.
                if (GetCurrentConsoleFontEx(hnd, false, ref info)) {
                    tt = (info.FontFamily & TMPF_TRUETYPE) == TMPF_TRUETYPE;
                    if (tt) {
                        //Console.WriteLine("The console already is using a TrueType font.");
                        return;
                    }
                    // Set console font to Lucida Console.
                    CONSOLE_FONT_INFO_EX newInfo = new CONSOLE_FONT_INFO_EX();
                    newInfo.cbSize = (uint)Marshal.SizeOf(newInfo);
                    newInfo.FontFamily = TMPF_TRUETYPE;
                    IntPtr ptr = new IntPtr(newInfo.FaceName);
                    Marshal.Copy(fontName.ToCharArray(), 0, ptr, fontName.Length);
                    // Get some settings from current font.
                    newInfo.dwFontSize = new COORD(info.dwFontSize.X, info.dwFontSize.Y);
                    newInfo.FontWeight = info.FontWeight;
                    SetCurrentConsoleFontEx(hnd, false, newInfo);
                }
            }
        }

        static async Task<int> MainAsync(string[] args, ExecutionState es)
        {
            if (es.CurrentTask == TaskType.Brilliant) {
                await RunBrilliant(es.CurrentRunningTask);
            } else if (es.CurrentTask == TaskType.ProjectEuler) {
                await RunProjectEuler(es.CurrentRunningTask);
            }

            es.fPreviousExecutionState = NativeMethods.SetThreadExecutionState(NativeMethods.ES_CONTINUOUS);
            return await Task.FromResult(0);
        }

        static void SetConsoleTitle(ExecutionState es)
        {
            string t = string.Empty;
            if(es.CurrentRunningTask is IProblemName)
            {
                t = ((IProblemName)es.CurrentRunningTask).GetName();
            } else {
                t = es.CurrentRunningTask.GetType().FullName;
            }
            if (Debugger.IsAttached) {
                t = "Visual Studio Debug - " + t + " - " + _title;
            } else {
                t = Assembly.GetExecutingAssembly().Location + " - " + t + " - " + _title;
            }

            Console.Title = t;
        }

        static async Task<int> RunBrilliant(object task)
        {
            ISolve solve = (ISolve)task;
            solve.Init();
            if (task is IRunSaved && ((IRunSaved)task).IsResume) {
                ((IRunSaved)task).Resume();
            } else {
                solve.Solve();
            }
            return await Task.FromResult(0);
        }

        static async Task<int> RunProjectEuler(object task)
        {
            IProblem p = (IProblem)task;
            p.Solve();
            return await Task.FromResult(0);
        }

        internal static class NativeMethods
        {
            // Import SetThreadExecutionState Win32 API and necessary flags
            [DllImport("kernel32.dll")]
            public static extern uint SetThreadExecutionState(uint esFlags);
            public const uint ES_CONTINUOUS = 0x80000000;
            public const uint ES_SYSTEM_REQUIRED = 0x00000001;

            [DllImport("user32.dll")]
            internal static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

            [DllImport("user32.dll")]
            internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

            [DllImport("user32.dll")]
            internal static extern IntPtr RemoveMenu(IntPtr hMenu, uint nPosition, uint wFlags);

            internal const uint SC_CLOSE = 0xF060;
            internal const uint MF_GRAYED = 0x00000001;
            internal const uint MF_BYCOMMAND = 0x00000000;
        }

        internal static class NativeConsoleMethods
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr GetStdHandle(int nStdHandle);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern bool GetCurrentConsoleFontEx(
                   IntPtr consoleOutput,
                   bool maximumWindow,
                   ref CONSOLE_FONT_INFO_EX lpConsoleCurrentFontEx);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetCurrentConsoleFontEx(
                   IntPtr consoleOutput,
                   bool maximumWindow,
                   CONSOLE_FONT_INFO_EX consoleCurrentFontEx);

            public const int STD_OUTPUT_HANDLE = -11;
            public const int TMPF_TRUETYPE = 4;
            public const int LF_FACESIZE = 32;
            public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

            [StructLayout(LayoutKind.Sequential)]
            internal struct COORD
            {
                internal short X;
                internal short Y;

                internal COORD(short x, short y)
                {
                    X = x;
                    Y = y;
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            public unsafe struct CONSOLE_FONT_INFO_EX
            {
                internal uint cbSize;
                internal uint nFont;
                internal COORD dwFontSize;
                internal int FontFamily;
                internal int FontWeight;
                internal fixed char FaceName[LF_FACESIZE];
            }
        }

        internal class ExecutionState
        {
            internal uint fPreviousExecutionState;
            internal object CurrentRunningTask;
            internal TaskType CurrentTask;
        }
    }
}