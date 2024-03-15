using System.Runtime.InteropServices;

namespace Common.Helpers
{
    public static class NativeMethods
    {
        public static int GWL_STYLE = -16;
        public static int WS_MAXIMIZEBOX = 0x10000;

        public static uint MF_BYCOMMAND = 0x00000000;
        public static uint MF_GRAYED = 0x00000001;

        public static uint SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool GetLastInputInfo(ref Lastinputinfo plii);

        [StructLayout(LayoutKind.Sequential)]
        public struct Lastinputinfo
        {
            private static readonly int SizeOf = Marshal.SizeOf(typeof(Lastinputinfo));

            [MarshalAs(UnmanagedType.U4)] public int cbSize;
            [MarshalAs(UnmanagedType.U4)] public int dwTime;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern bool EnableMenuItem(IntPtr hMenu, uint uIdEnableItem, uint uEnable);
    }
}
