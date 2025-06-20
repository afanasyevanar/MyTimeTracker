using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace MyTimeTracker.Services
{
    public class ActiveWindowTracker
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public static (string, string) GetActiveWindowInfo()
        {
            try
            {
                var handle = GetForegroundWindow();

                GetWindowThreadProcessId(handle, out var processId);

                var process = Process.GetProcessById((int)processId);
                var processName = process.ProcessName;

                const int nChars = 256;
                var buff = new StringBuilder(nChars);
                var windowTitle = string.Empty;

                if (GetWindowText(handle, buff, nChars) > 0)
                {
                    windowTitle = buff.ToString();
                }

                return (processName, windowTitle);
            }
            catch (Exception)
            {
                // Probably access denied on getting process info
                return ("Unknown", "Unknown");
            }
        }
    }
} 