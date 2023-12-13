using InputSimulator;
using InputSimulator.Native;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Interop;

namespace Example.WPF
{
    static class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            // var rightInfo = NativeMethods.GetMonitorInfo(NativeMethods.GetMonitorFromPoint(2000, 1000));

            //  MouseInputMethods.MoveTo(rightInfo, 1920 / 2, 1080 / 2);

            var monitors = NativeMethods.GetAllMonitorHandles().Select(hMonitor => NativeMethods.GetMonitorInfo(hMonitor));

            //foreach (var monitor in monitors)
            //{
            //    for (int y = monitor.rcMonitor.top, y_max = monitor.rcMonitor.bottom, y_incr = y_max / 10; y < y_max; y += y_incr)
            //    {
            //        for (int x = monitor.rcMonitor.left, x_max = monitor.rcMonitor.right; x < x_max; ++x)
            //        {
            //            MouseInputMethods.MoveTo(x, y);
            //        }
            //    }
            //}



            //MouseInputMethods.MoveTo(monitors.First(monitor => monitor.dwFlags.HasFlag(MONITORINFO.Flags.MONITORINFOF_PRIMARY)), 1920 / 2, 1080 / 2);

            //  Process.Start(new ProcessStartInfo("gitex") { UseShellExecute = true })?.Dispose();

            // Thread.Sleep(2500);

            MouseInputMethods.HorizontalScroll(100, 120);

            foreach (var gitex in Process.GetProcessesByName("GitExtensions"))
            {
                var hWnd = gitex.MainWindowHandle;

                var rect = NativeMethods.GetWindowRect(hWnd);

                if (rect.IsZeroed) continue;

                var pos = NativeMethods.GetCursorPos();

                MouseInputMethods.ButtonClickAt(EMouseButton.Left, rect.right - 10, rect.top + 10);

                gitex.Dispose();
            }

            //var absPos = NativeMethods.GetCursorPosAbs();
            //NativeMethods.SendInput(new INPUT(new MOUSEINPUT()
            //{
            //    dwFlags = MOUSEINPUT.Flags.MOUSEEVENTF_LEFTDOWN | MOUSEINPUT.Flags.MOUSEEVENTF_MOVE | MOUSEINPUT.Flags.MOUSEEVENTF_ABSOLUTE | MOUSEINPUT.Flags.MOUSEEVENTF_VIRTUALDESK,
            //    dx = InputHelper.ToAbsCoordinateX(mainWindowRect.right - 3),
            //    dy = InputHelper.ToAbsCoordinateY(mainWindowRect.top + 3)
            //}));
            //Thread.Sleep(500);
            //NativeMethods.SendInput(new INPUT(new MOUSEINPUT()
            //{
            //    dwFlags = MOUSEINPUT.Flags.MOUSEEVENTF_LEFTUP | MOUSEINPUT.Flags.MOUSEEVENTF_LEFTDOWN | MOUSEINPUT.Flags.MOUSEEVENTF_MOVE | MOUSEINPUT.Flags.MOUSEEVENTF_ABSOLUTE | MOUSEINPUT.Flags.MOUSEEVENTF_VIRTUALDESK,
            //    dx = absPos.x,
            //    dy = absPos.y
            //}));

            return 0;
        }
    }
}
