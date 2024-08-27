using System.Runtime.InteropServices;

namespace Lib.CS
{
    internal class DumpProcess
    {
        public void Run() //use example
        {
            //need windows.Forms.Application
            //System.Windows.Windows.Forms.Application.ThreadException += new ThreadExceptionEventHandler(Dump.ExceptionDump);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        [Flags]
        public enum _MINIDUMP_TYPE
        {
            MiniDumpNormal = 0x00000000,
            MiniDumpWithDataSegs = 0x00000001,
            MiniDumpWithFullMemory = 0x00000002,
            MiniDumpWithHandleData = 0x00000004,
            MiniDumpFilterMemory = 0x00000008,
            MiniDumpScanMemory = 0x00000010,
            MiniDumpWithUnloadedModules = 0x00000020,
            MiniDumpWithIndirectlyReferencedMemory = 0x00000040,
            MiniDumpFilterModulePaths = 0x00000080,
            MiniDumpWithProcessThreadData = 0x00000100,
            MiniDumpWithPrivateReadWriteMemory = 0x00000200,
            MiniDumpWithoutOptionalData = 0x00000400,
            MiniDumpWithFullMemoryInfo = 0x00000800,
            MiniDumpWithThreadInfo = 0x00001000,
            MiniDumpWithCodeSegs = 0x00002000,
            MiniDumpWithoutAuxiliaryState = 0x00004000,
            MiniDumpWithFullAuxiliaryState = 0x00008000,
            MiniDumpWithPrivateWriteCopyMemory = 0x00010000,
            MiniDumpIgnoreInaccessibleMemory = 0x00020000,
            MiniDumpWithTokenInformation = 0x00040000,
            MiniDumpValidTypeFlags = 0x0007ffff,
        };

        // Pack=4 is important! So it works also for x64!
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct MiniDumpExceptionInformation
        {
            public uint ThreadId;
            public nint ExceptioonPointers;
            [MarshalAs(UnmanagedType.Bool)]
            public bool ClientPointers;
        }

        [DllImport("dbghelp.dll",
            EntryPoint = "MiniDumpWriteDump",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            ExactSpelling = true,
            SetLastError = true)]
        static extern bool MiniDumpWriteDump(
            nint hProcess,
            uint processId,
            nint hFile,
            _MINIDUMP_TYPE dumpType,
            ref MiniDumpExceptionInformation expParam,
            nint userStreamParam,
            nint callbackParam);

        [DllImport("kernel32.dll", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
        static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll", EntryPoint = "GetCurrentProcess", ExactSpelling = true)]
        static extern nint GetCurrentProcess();

        [DllImport("kernel32.dll", EntryPoint = "GetCurrentProcessId", ExactSpelling = true)]
        static extern uint GetCurrentProcessId();

        [DllImport("kernel32.dll", EntryPoint = "TerminateProcess", ExactSpelling = true)]
        static extern int TerminateProcess(nint hprocess, int ExitCode);

        public static void Install_self_mini_dump()
        {
            MiniDumpExceptionInformation exp;

            exp.ThreadId = GetCurrentThreadId();
            exp.ClientPointers = false;
            exp.ExceptioonPointers = Marshal.GetExceptionPointers();

            //file name
            string dt = DateTime.Now.ToString("yyyy.MM.dd.hh.mm.ss");
            string fileName = dt + ".dmp";

            //make
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                bool bRet = MiniDumpWriteDump(
                    GetCurrentProcess(),
                    GetCurrentProcessId(),
                    fs.SafeFileHandle.DangerousGetHandle(),
                    //include info
                    _MINIDUMP_TYPE.MiniDumpNormal,
                    ref exp,
                    nint.Zero,
                    nint.Zero);
            }

            //exit
            TerminateProcess(GetCurrentProcess(), 0);
        }

        //ui except
        public static void ExceptionDump(object sender, ThreadExceptionEventArgs args)
        {
            Install_self_mini_dump();
        }

        //other except
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Install_self_mini_dump();
        }
    }
}
