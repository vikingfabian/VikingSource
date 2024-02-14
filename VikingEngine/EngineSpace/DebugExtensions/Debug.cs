using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using VikingEngine.DebugExtensions;

namespace VikingEngine
{
    static class Debug
    {
        public static VikingEngine.DataStream.FilePath logFilePath;
        static OutputWindow OutputWindow = null;
        
        public static void ToggleOutput()
        {
            viewOutput = !viewOutput;
            if (viewOutput)
            { createOutputWin(); }
            else
            {
                OutputWindow.DeleteMe();
                OutputWindow = null;
            }
        }
        static bool viewOutput = false;

        public static void Init()
        {
#if PCGAME
            // Link debug output to files
            logFilePath = new DataStream.FilePath(
                "Logs",
                "log",
                ".txt", true, false);

            var fullPath = logFilePath.CompletePath(true);
            System.IO.Directory.CreateDirectory(logFilePath.CompleteDirectory);
            //System.IO.File.WriteAllText(fullPath, "Captain's log, stardate " + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Day.ToString("00") + " " + DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + "\n\n");
            //Trace.Listeners.Add(new TextWriterTraceListener(fullPath));
            //Trace.AutoFlush = true;
#endif

            if (viewOutput)
            {
                createOutputWin();
            }
        }

        public static void OnNewGameState()
        {
            //if (Input.SharpDXInput.error != null)
            //{
            //    Input.SharpDXInput.error.view();
            //    Input.SharpDXInput.error = null;
            //}

            OutputWindow = null;
            if (viewOutput)
            { createOutputWin(); }
        }

        public static void createOutputWin()
        {
            viewOutput = true;
            OutputWindow = new OutputWindow();
        }
        static CirkleCounterUp errorIndex = new CirkleCounterUp(0, 9);


        //public static void LogThreadStart(System.Threading.Thread thread)
        //{
        //    Log(DebugLogType.MSG, thread.Name);
        //}

        public static void Log(string text)
        {
            Log(DebugLogType.MSG, text);
        }
        public static void LogWarning(string text)
        {
            Log(DebugLogType.WARN, text);
        }
        public static void LogError(string text)
        {
            Log(DebugLogType.ERROR, text);
        }

        public static void Log(DebugLogType type, string text)
        {
            if (PlatformSettings.DebugLevel <= BuildDebugLevel.Release)
            {
                if (text == null)
                { throw new NullReferenceException(); }

                System.Diagnostics.Debug.WriteLine(text);
            }

            if (OutputWindow != null)
            {
                OutputWindow.AddLine(text);
            }
        }

        public static void CrashIfThreaded()
        {
            //if (PlatformSettings.RunningWindows)
            //{
#if PCGAME
            if (!MainGame.IsMainThread)
                {
                    throw new Exception("not allowed thread action");
                }
#endif
           //}
        }
        public static void CrashIfMainThread()
        {
            //if (PlatformSettings.RunningWindows)
            //{
#if PCGAME
                if (MainGame.IsMainThread)
                {
                    throw new Exception("action must be threaded");
                }
#endif
           // }
        }

        public static byte Byte_OrCrash(int value)
        {
            if (value >= byte.MinValue && value <= byte.MaxValue)
            { 
                return (byte)value;
            }
            throw new FormatException("Byte");
        }

        public static short Short_OrCrash(int value)
        {
            if (value >= short.MinValue && value <= short.MaxValue)
            {
                return (short)value;
            }
            throw new FormatException("Short");
        }

        public static ushort Ushort_OrCrash(int value)
        {
            if (value >= ushort.MinValue && value <= ushort.MaxValue)
            {
                return (ushort)value;
            }
            throw new FormatException("UShort");
        }

        const byte StreamCheckValue = 213; 
        public static void WriteCheck(System.IO.BinaryWriter w)
        {
            w.Write(StreamCheckValue);
        }
        public static void ReadCheck(System.IO.BinaryReader r)
        {
            byte val = r.ReadByte();
            if (PlatformSettings.ViewErrorWarnings && val != StreamCheckValue)
            {
                throw new NetworkWriteReadSynchException("ReadCheck");
            }
        }

        public static int GarbageCollectionCount()
        {
            int gccount = 0;
#if PCGAME
            for (int i = 0; i < GC.MaxGeneration; ++i)
            {
                gccount += GC.CollectionCount(i);
            }
#endif
            return gccount;
        }

        public static void CtrlBreak(bool active = true)
        {
#if DEBUG
            if (Input.Keyboard.Ctrl && active)
                System.Diagnostics.Debugger.Break();
#endif
        }
    }

    enum DebugLogType
    {
        ERROR,
        WARN,
        MSG,
        NUM
    }
}
