using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Text;
using GenericDefs.Classes.Quirky;

namespace GenericDefs.DotNet
{
    public static class QueuedConsole
    {
        static SpecialConsole _console;
        static QueuedConsole() {
            _console = new SpecialConsole(10, 1000);
        }
        
        public static void WriteLine(string message)
        {
            _console.WriteLine(message);
        }

        public static void Flush()
        {
            _console.Flush();
        }

        public static void Write(string message)
        {
            _console.WriteTextInline(message);
        }

        public static void LazySuppressWrite(string message)
        {
            _console.LazySuppressWrite(message);
        }

        public static SpecialConsole GetSpecialConsole(int lazyQueueMax, int normalQueueMax) {
            return new SpecialConsole(lazyQueueMax, normalQueueMax);
        }

        /// <summary>
        /// Prints this message to console, and waits for a key return.
        /// </summary>
        /// <param name="s"></param>
        public static void WriteFinalAnswer(string s) {
            _console.SetTextColor(new ConsoleColours.Combination() { Foreground = ConsoleColor.White, Background = ConsoleColor.Blue });
            _console.WriteImmediate(s);
            _console.ResetTextColor();
            _console.WriteImmediate("Press any key to continue.");
            ReadKey();
        }

        /// <summary>
        /// Prints this message to console, and waits for a key return.
        /// </summary>
        /// <param name="s"></param>
        public static void WriteAndWaitOnce(string s)
        {
            _console.SetTextColor(new ConsoleColours.Combination() { Foreground = ConsoleColor.White, Background = ConsoleColor.Blue });
            _console.WriteImmediate(s);
            _console.ResetTextColor();
            _console.WriteImmediate("Press any key to continue.");
            ReadKey();
        }

        /// <summary>
        /// Press any key to continue utility.
        /// </summary>
        /// <param name="s"></param>
        public static void PressAnyKeyToContinue()
        {
            _console.WriteImmediate("Press any key to continue......");
            ReadKey();
        }

        /// <summary>
        /// Prints this message to console, and waits for a key return.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteFinalAnswer(string format, params object[] args)
        {
            _console.SetTextColor(new ConsoleColours.Combination() { Foreground = ConsoleColor.White, Background = ConsoleColor.Blue });
            WriteFormatter.WriteFormat(format, _console, args);
            _console.ResetTextColor();
            _console.WriteImmediate("Press any key to continue.");
            ReadKey();
        }

        /// <summary>
        /// Prints this message to console, and waits for a key return.
        /// </summary>
        /// <param name="s"></param>
        public static void WriteAndWaitOnce(string format, params object[] args)
        {
            _console.SetTextColor(new ConsoleColours.Combination() { Foreground = ConsoleColor.White, Background = ConsoleColor.Blue });
            WriteFormatter.WriteFormat(format, _console, args);
            _console.ResetTextColor();
            _console.WriteImmediate("Press any key to continue.");
            ReadKey();
        }

        /// <summary>
        /// Works like string.format
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteFormat(string format, params object[] args)
        {
            WriteFormatter.WriteFormat(format, _console, args);
        }

        public static void ReadKey()
        {
            Console.ReadKey();
        }

        public static void WriteImmediate(string s)
        {
            _console.WriteImmediate(s);
        }

        public static void WriteImmediate(string format, params object[] args)
        {
            WriteFormatter.WriteFormat(format, _console, args);
        }

        public static void Write(string format, params object[] args)
        {
            _console.WriteTextInline(WriteFormatter.FormatText(format, args));
        }

        internal static void WriteStatusMessage(string message)
        {
            _console.WriteImmediate(message);
        }
    }

    internal static class WriteFormatter
    {
        /// <summary>
        /// Works like string.format
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        internal static void WriteFormat(string format, SpecialConsole _console,params object[] args)
        {
            _console.WriteImmediate(string.Format(format, args));
        }

        internal static string FormatText(string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// Works like string.format
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        internal static void WriteFormatHighlightArguments(string format, object[] args, SpecialConsole _console)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }
            StringBuilder sb = new StringBuilder();
            AppendFormatHelper(format, new ParamsArray(args), _console, sb);
        }

        internal static void AppendFormatHelper(string format, ParamsArray args, SpecialConsole _console, StringBuilder sb)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }
            int num = 0;
            int length = format.Length;
            char c = '\0';
            while (true)
            {
                if (num < length)
                {
                    c = format[num];
                    num++;
                    if (c == '}')
                    {
                        if (num < length && format[num] == '}')
                        {
                            num++;
                        }
                        else
                        {
                            throw new ArgumentNullException("FormatError");
                        }
                    }
                    if (c == '{')
                    {
                        if (num >= length || format[num] != '{')
                        {
                            num--;
                            goto IL_8D;
                        }
                        num++;
                    }
                    sb.Append(c);
                    continue;
                }
            IL_8D:
                if (num == length)
                {
                    return;
                }
                num++;
                if (num == length || (c = format[num]) < '0' || c > '9')
                {
                    throw new ArgumentNullException("FormatError");
                }
                int num2 = 0;
                do
                {
                    num2 = num2 * 10 + (int)c - 48;
                    num++;
                    if (num == length)
                    {
                        throw new ArgumentNullException("FormatError");
                    }
                    c = format[num];
                }
                while (c >= '0' && c <= '9' && num2 < 1000000);
                if (num2 >= args.Length)
                {
                    break;
                }
                while (num < length && (c = format[num]) == ' ')
                {
                    num++;
                }
                bool flag = false;
                int num3 = 0;
                if (c == ',')
                {
                    num++;
                    while (num < length && format[num] == ' ')
                    {
                        num++;
                    }
                    if (num == length)
                    {
                        throw new ArgumentNullException("FormatError");
                    }
                    c = format[num];
                    if (c == '-')
                    {
                        flag = true;
                        num++;
                        if (num == length)
                        {
                            throw new ArgumentNullException("FormatError");
                        }
                        c = format[num];
                    }
                    if (c < '0' || c > '9')
                    {
                        throw new ArgumentNullException("FormatError");
                    }
                    do
                    {
                        num3 = num3 * 10 + c - 48;
                        num++;
                        if (num == length)
                        {
                            throw new ArgumentNullException("FormatError");
                        }
                        c = format[num];
                        if (c < '0' || c > '9')
                        {
                            break;
                        }
                    }
                    while (num3 < 1000000);
                }
                while (num < length && (c = format[num]) == ' ')
                {
                    num++;
                }
                object obj = args[num2];
                StringBuilder stringBuilder = null;
                if (c == ':')
                {
                    num++;
                    while (true)
                    {
                        if (num == length)
                        {
                            throw new ArgumentNullException("FormatError");
                        }
                        c = format[num];
                        num++;
                        if (c == '{')
                        {
                            if (num < length && format[num] == '{')
                            {
                                num++;
                            }
                            else
                            {
                                throw new ArgumentNullException("FormatError");
                            }
                        }
                        else if (c == '}')
                        {
                            if (num >= length || format[num] != '}')
                            {
                                break;
                            }
                            num++;
                        }
                        if (stringBuilder == null)
                        {
                            stringBuilder = new StringBuilder();
                        }
                        stringBuilder.Append(c);
                    }
                    num--;
                }
                if (c != '}')
                {
                    throw new ArgumentNullException("FormatError");
                }
                num++;
                string text = null;
                string text2 = null;
                StringBuilder t = new StringBuilder();
                IFormattable formattable = obj as IFormattable;
                bool isArg = false;
                if (formattable != null)
                {
                    isArg = true;
                    if (text == null && stringBuilder != null)
                    {
                        text = stringBuilder.ToString();
                    }
                    text2 = formattable.ToString(text, null);
                }
                else if (obj != null)
                {
                    isArg = true;
                    text2 = obj.ToString();
                }
                if (text2 == null)
                {
                    text2 = string.Empty;
                }
                int num4 = num3 - text2.Length;
                if (!flag && num4 > 0)
                {
                    sb.Append(' ', num4);
                    t.Append(' ', num4);
                }

                _console.WriteTextInline(sb.ToString());

                t.Append(text2);
                sb.Append(text2);
                sb.Clear();
                if (isArg)
                {
                    _console.SetTextColor(_console.GetContext().GetColourContext().GetNext());
                }
                _console.WriteTextInline(t.ToString());
                if (isArg)
                {
                    _console.ResetTextColor();
                }

                if (flag && num4 > 0)
                {
                    sb.Append(' ', num4);
                }
            }
            throw new FormatException("Format_IndexOutOfRange");
        }
    }

    internal class ConsoleColours
    {
        internal class Combination
        {
            internal ConsoleColor Foreground { set; get; }
            internal ConsoleColor Background { get; set; }
        }
        Classes.Collections.CircularLinkedList<Combination> _cache;
        public ConsoleColours()
        {
            if(_cache == null) { 
                _cache = new Classes.Collections.CircularLinkedList<Combination>();
                _cache.AddLast(new Combination() { Background = ConsoleColor.Blue, Foreground = ConsoleColor.White });
                _cache.AddLast(new Combination() { Background = ConsoleColor.DarkCyan, Foreground = ConsoleColor.White });
                _cache.AddLast(new Combination() { Background = ConsoleColor.White, Foreground = ConsoleColor.Black });
                _cache.AddLast(new Combination() { Background = ConsoleColor.Red, Foreground = ConsoleColor.White });
                _cache.AddLast(new Combination() { Background = ConsoleColor.DarkMagenta, Foreground = ConsoleColor.White });
                _cache.AddLast(new Combination() { Background = ConsoleColor.DarkYellow, Foreground = ConsoleColor.DarkBlue });
                _cache.AddLast(new Combination() { Background = ConsoleColor.Yellow, Foreground = ConsoleColor.Black });
            }
        }

        public Combination GetNext() {
            Classes.Collections.CircularLinkedList<Combination>.Enumerator e = _cache.GetEnumerator();
            e.MoveNext();
            return e.Current;
        }
    }

    internal class ConsoleContext
    {
        public ConsoleColourContext CCC { get; set; }

        public ConsoleColours GetColourContext()
        {
            if (CCC == null) { throw new ArgumentException("Colour Context was not set."); }
            return CCC.ConsoleColours;
        }
    }

    internal class ConsoleColourContext
    {
        public ConsoleColours ConsoleColours { get; set; }
        public ConsoleColourContext() : base()
        {
            
        }
    }

    internal class DefaultColourContext : ConsoleColourContext
    {
        public DefaultColourContext()
        {
            ConsoleColours = new ConsoleColours();
        }
    }

    /// <summary>
    /// Unless specified writes text to a new line.
    /// </summary>
    public class SpecialConsole
    {
        private static List<string> _sb = new List<string>();
        private static int _currentQueue;
        private int _queueMax;
        private ConsoleContext _cc;

        public int LazyQueueMax {
            get { return _lazyqueueMax; }
            set { _lazyqueueMax = value; }
        }

        public int NormalQueueMax
        {
            get { return this._queueMax; }
            set { this._queueMax = value; }
        }

        public bool LazyWriteLastMessage { get; set; }

        public SpecialConsole(int lazyQueueMax, int normalQueueMax) {
            LazyQueueMax = lazyQueueMax;
            NormalQueueMax = normalQueueMax;
        }

        private static object _lockWrite = new object();
        public void WriteLine(string message)
        {
            _sb.Add(message);
            _currentQueue++;
            if (_currentQueue >= _queueMax)
            {
                lock (_lockWrite)
                {
                    Flush();
                }
            }
        }

        public void WriteImmediate(string s) {
            Console.WriteLine(s);
        }

        public void Flush()
        {
            foreach (string s in _sb)
            {
                Console.WriteLine(s);
            }
            _currentQueue = 0;
            _sb.Clear();
        }

        private static List<string> _lazy = new List<string>();
        private static int _lazyQueue;
        private int _lazyqueueMax;
        private static object _lock = new object();
        public void LazySuppressWrite(string message)
        {
            _lazy.Add(message);
            _lazyQueue++;
            if (_lazyQueue >= _lazyqueueMax)
            {
                lock (_lock)
                {
                    LazyWrite();
                }
            }
        }

        private void LazyWrite()
        {
            if(this.LazyWriteLastMessage) _lazy.Reverse();
            foreach (string s in _lazy)
            {
                Console.WriteLine(s);
                break;
            }
            Console.WriteLine("Suppressed messages ..... {0}", _lazyQueue - 1);
            _lazyQueue = 0;
            _lazy.Clear();
        }

        internal void WriteTextInline(string s)
        {
            Console.Write(s);
        }

        internal void SetTextColor(ConsoleColours.Combination c)
        {
            Console.ForegroundColor = c.Foreground;
            Console.BackgroundColor = c.Background;
        }

        internal void ResetTextColor()
        {
            Console.ResetColor();
        }

        internal void AttachContext(ConsoleContext cc)
        {
            _cc = cc;
        }

        internal ConsoleContext GetContext()
        {
            if(_cc == null) { throw new ArgumentException("Console Context not attached."); }
            return _cc;
        }

        public void WriteHighlightedImmediate(string s, object[] args)
        {
            if (_cc == null) AttachContext(new ConsoleContext() { CCC = new DefaultColourContext() });
            WriteFormatter.WriteFormatHighlightArguments(s, args, this);
            Console.Write(Environment.NewLine);
        }

        public void ReadKey()
        {
            Console.ReadKey();
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ConsoleFont
    {
        public uint Index;
        public short SizeX, SizeY;
    }

    public static class ConsoleHelper
    {
        [DllImport("kernel32")]
        public static extern bool SetConsoleIcon(IntPtr hIcon);

        public static bool SetConsoleIcon(Icon icon)
        {
            return SetConsoleIcon(icon.Handle);
        }

        [DllImport("kernel32")]
        private extern static bool SetConsoleFont(IntPtr hOutput, uint index);

        private enum StdHandle
        {
            OutputHandle = -11
        }

        [DllImport("kernel32")]
        private static extern IntPtr GetStdHandle(StdHandle index);

        public static bool SetConsoleFont(uint index)
        {
            return SetConsoleFont(GetStdHandle(StdHandle.OutputHandle), index);
        }

        [DllImport("kernel32")]
        private static extern bool GetConsoleFontInfo(IntPtr hOutput, [MarshalAs(UnmanagedType.Bool)]bool bMaximize,
            uint count, [MarshalAs(UnmanagedType.LPArray), Out] ConsoleFont[] fonts);

        [DllImport("kernel32")]
        private static extern uint GetNumberOfConsoleFonts();

        public static uint ConsoleFontsCount
        {
            get
            {
                return GetNumberOfConsoleFonts();
            }
        }

        public static ConsoleFont[] ConsoleFonts
        {
            get
            {
                ConsoleFont[] fonts = new ConsoleFont[GetNumberOfConsoleFonts()];
                if (fonts.Length > 0)
                    GetConsoleFontInfo(GetStdHandle(StdHandle.OutputHandle), false, (uint)fonts.Length, fonts);
                return fonts;
            }
        }

    }
}