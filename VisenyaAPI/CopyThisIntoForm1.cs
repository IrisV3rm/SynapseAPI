[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WaitNamedPipe(string name, int timeout);

        public static bool NamedPipeExist(string pipeName)
        {
            bool result;
            try
            {
                int timeout = 0;
                if (WaitNamedPipe(Path.GetFullPath(string.Format("\\\\\\\\.\\\\pipe\\\\{0}", pipeName)), timeout))
                {
                    int lastWin32Error = Marshal.GetLastWin32Error();
                    if (lastWin32Error == 0)
                    {
                        result = false;
                        return result;
                    }
                    if (lastWin32Error == 2)
                    {
                        result = false;
                        return result;
                    }
                }
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public class Inject
        {
            [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
            internal static extern IntPtr LoadLibraryA(string lpFileName);
            [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
            internal static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);
            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool FreeLibrary(IntPtr hModule);
            [DllImport("kernel32.dll")]
            internal static extern IntPtr OpenProcess(ProcessAccess dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);
            [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
            internal static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);
            [DllImport("kernel32.dll")]
            internal static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, UIntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

            public bool InjectDLL()
            {
                if (Process.GetProcessesByName("RobloxPlayerBeta").Length == 0)
                {
                    return false;
                }
                Process process = Process.GetProcessesByName("RobloxPlayerBeta")[0];
                byte[] bytes = new ASCIIEncoding().GetBytes("C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\VisenyaBin\\VisenyaModule.dll");
                IntPtr hModule = LoadLibraryA("kernel32.dll");
                UIntPtr procAddress = GetProcAddress(hModule, "LoadLibraryA");
                FreeLibrary(hModule);
                if (procAddress == UIntPtr.Zero)
                {
                    return false;
                }
                IntPtr intPtr = OpenProcess(ProcessAccess.AllAccess, false, process.Id);
                if (intPtr == IntPtr.Zero)
                {
                    return false;
                }
                IntPtr intPtr2 = VirtualAllocEx(intPtr, (IntPtr)0, (uint)bytes.Length, 12288u, 4u);
                UIntPtr uintPtr;
                IntPtr intPtr3;
                return !(intPtr2 == IntPtr.Zero) && WriteProcessMemory(intPtr, intPtr2, bytes, (uint)bytes.Length, out uintPtr) && !(CreateRemoteThread(intPtr, (IntPtr)0, 0u, procAddress, intPtr2, 0u, out intPtr3) == IntPtr.Zero);
            }

            [Flags]
            public enum ProcessAccess
            {
                AllAccess = 1050235,
                CreateThread = 2,
                DuplicateHandle = 64,
                QueryInformation = 1024,
                SetInformation = 512,
                Terminate = 1,
                VMOperation = 8,
                VMRead = 16,
                VMWrite = 32,
                Synchronize = 1048576
            }
        }

        public void Execute(string script)
        {
            using (var namedPipeClientStream = new NamedPipeClientStream(".", "visenya", PipeDirection.Out))
            {
                namedPipeClientStream.Connect(5);
                if (namedPipeClientStream.IsConnected)
                {
                    using (var streamWriter = new StreamWriter(namedPipeClientStream))
                    {
                        streamWriter.Write(script);
                        streamWriter.Dispose();
                    }
                    namedPipeClientStream.Dispose();
                }
                else
                {
                    MessageBox.Show("Visnya did not attach properly!", "VisenyaAPI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public async System.Threading.Tasks.Task<bool> VisenyaAttachAsync()
        {
            Inject Inject = new Inject();
            if (Inject.InjectDLL())
            {
                using (var namedPipeClientStream = new NamedPipeClientStream(".", "visenya", PipeDirection.Out))
                {
                    while (Process.GetProcessesByName("RobloxPlayerBeta").Length != 0)
                    {
                        await namedPipeClientStream.ConnectAsync();
                        if (namedPipeClientStream.IsConnected)
                        {
                            MessageBox.Show("Visenya has attached!", "VisenyaAPI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                    }
                }

            }
            return false;
        }

        public async Task<bool> VisenyaAttachTime()
        {
            Inject Inject = new Inject();
            if (Inject.InjectDLL())
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                using (var namedPipeClientStream = new NamedPipeClientStream(".", "visenya", PipeDirection.Out))
                {
                    while (Process.GetProcessesByName("RobloxPlayerBeta").Length != 0)
                    {
                        await namedPipeClientStream.ConnectAsync();
                        if (namedPipeClientStream.IsConnected)
                        {
                            MessageBox.Show("Visenya has attached! Took: " + watch.Elapsed.TotalSeconds.ToString(), "VisenyaAPI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            watch.Stop();
                            return true;
                        }
                    }
                }

            }
            return false;
        }
        public async Task<string> VisenyaAttachReturn()
        {
            Inject Inject = new Inject();
            if (Inject.InjectDLL())
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                using (var namedPipeClientStream = new NamedPipeClientStream(".", "visenya", PipeDirection.Out))
                {
                    while (Process.GetProcessesByName("RobloxPlayerBeta").Length != 0)
                    {
                        await namedPipeClientStream.ConnectAsync();
                        if (namedPipeClientStream.IsConnected)
                        {
                            watch.Stop();
                            return "Visenya has attached! Took: " + watch.Elapsed.TotalSeconds.ToString();
                        }
                    }
                }

            }
            return "";
        }
    }