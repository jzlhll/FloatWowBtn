using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace FloatWowBtn
{
    public class WindowApi
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

		[DllImport("user32")]
		public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
		public enum MouseType
		{
			//移动鼠标 
			MOUSEEVENTF_MOVE = 0x0001,
			//模拟鼠标左键按下 
			MOUSEEVENTF_LEFTDOWN = 0x0002,
			//模拟鼠标左键抬起 
			MOUSEEVENTF_LEFTUP = 0x0004,
			//模拟鼠标右键按下 
			MOUSEEVENTF_RIGHTDOWN = 0x0008,
			//模拟鼠标右键抬起 
			MOUSEEVENTF_RIGHTUP = 0x0010,
			//模拟鼠标中键按下 
			MOUSEEVENTF_MIDDLEDOWN = 0x0020,
			//模拟鼠标中键抬起 
			MOUSEEVENTF_MIDDLEUP = 0x0040,
			//标示是否采用绝对坐标 
			MOUSEEVENTF_ABSOLUTE = 0x8000,

			//模拟鼠标侧键
			MOUSEEVENTF_WM_XBUTTONUP  = 0x020C,
			MOUSEEVENTF_WM_XBUTTONDOWN = 0x020D,

			MOUSEEVENTF_WM_NCXBUTTONUP = 0x00AC,
			MOUSEEVENTF_WM_NCXBUTTONDOWN = 0x00AB,
		}

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

		/// <summary>
		/// 传入左右下和上2个
		/// </summary>
		/// <param name="types"></param>
		public static void ClickMouse(MouseType[] types)
		{
			mouse_event((int)types[0] | (int)(int)types[1], 0, 0, 0, 0);
			Thread.Sleep(10);
		}
	}
}
