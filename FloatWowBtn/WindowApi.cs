using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace FloatWowBtn
{
    class WindowApi
    {
		public const int SW_HIDE = 0;

		public const int SW_NORMAL = 1;

		public const int SW_MAXIMIZE = 3;

		public const int SW_SHOWNOACTIVATE = 4;

		public const int SW_SHOW = 5;

		public const int SW_MINIMIZE = 6;

		public const int SW_RESTORE = 9;

		public const int SW_SHOWDEFAULT = 10;

		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		[DllImport("user32.dll")]
		public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr FindWindow(string classname, string captionName);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr FindWindowEx(IntPtr parent, IntPtr child, string classname, string captionName);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

		[DllImport("user32.dll")]
		public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		/// <summary>
		/// 键盘事件
		/// </summary>
		/// <param name="bVk"> virtual-key code</param>
		/// <param name="bScan">hardware scan code</param>
		/// <param name="dwFlags"> flags specifying various function options</param>
		/// <param name="dwExtraInfo"> additional data associated with keystroke</param>
		/// bScan设置为0，dwFlags设置0表示按下，2表示抬起；dwExtraInfo也设置为0即可。
		[DllImport("user32.dll")]
		public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

		public static void CtrlC() {
			keybd_event((byte)Keys.ControlKey, 0, 0, 0);
			keybd_event((byte)Keys.C, 0, 0, 0);
			keybd_event((byte)Keys.ControlKey, 0, 2, 0);
			keybd_event((byte)Keys.C, 0, 2, 0);
		}

		public static void ClickKeyboard(Keys key) {
			keybd_event((byte)key, 0, 0, 0);
			Thread.Sleep(10);
			keybd_event((byte)key, 0, 2, 0);
		}
	}
}
