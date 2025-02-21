using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Text.RegularExpressions;

namespace DesktopTips
{
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        private const int HWND_BOTTOM = 1;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOSIZE = 0x0001;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int GWL_EXSTYLE = -20;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        private const uint WS_EX_TOOLWINDOW = 0x00000080;
        private const uint WS_EX_APPWINDOW = 0x00040000;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowPos(hwnd, new IntPtr(HWND_BOTTOM), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            // 读取命令行参数并设置窗口位置、大小和提示框内容
            string[] args = Environment.GetCommandLineArgs();
            ParseArguments(args);
        }

        private void ParseArguments(string[] args)
        {
            string xsa = "left"; // 默认值
            string xwa = "left"; // 默认值
            int xOffset = 0;
            string ysa = "top";  // 默认值
            string ywa = "top";  // 默认值
            int yOffset = 0;
            string tipText = "Hello, Desktop!"; // 默认提示文本
            double width = -1; // 默认宽度
            double height = -1; // 默认高度
            int detectionInterval = 100;

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "--x-screen-align":
                    case "--xsa":
                        if (i + 1 < args.Length)
                        {
                            xsa = args[i + 1].ToLower();
                        }
                        break;
                    case "--x-window-align":
                    case "--xwa":
                        if (i + 1 < args.Length)
                        {
                            xwa = args[i + 1].ToLower();
                        }
                        break;
                    case "--x":
                    case "-x":
                        if (i + 1 < args.Length)
                        {
                            xOffset = int.Parse(args[i + 1]);
                        }
                        break;
                    case "--y-screen-align":
                    case "--ysa":
                        if (i + 1 < args.Length)
                        {
                            ysa = args[i + 1].ToLower();
                        }
                        break;
                    case "--y-window-align":
                    case "--ywa":
                        if (i + 1 < args.Length)
                        {
                            ywa = args[i + 1].ToLower();
                        }
                        break;
                    case "--y":
                    case "-y":
                        if (i + 1 < args.Length)
                        {
                            yOffset = int.Parse(args[i + 1]);
                        }
                        break;
                    case "-t":
                    case "--text":
                        if (i + 1 < args.Length)
                        {
                            tipText = Regex.Unescape(args[i + 1]);
                        }
                        break;
                    case "--width":
                    case "-w":
                        if (i + 1 < args.Length && double.TryParse(args[i + 1], out double w))
                        {
                            width = w;
                        }
                        break;
                    case "--height":
                    case "-h":
                        if (i + 1 < args.Length && double.TryParse(args[i + 1], out double h))
                        {
                            height = h;
                        }
                        break;
                    case "--detection-interval":
                    case "--di":
                        if (i + 1 < args.Length && int.TryParse(args[i + 1], out int interval))
                        {
                            detectionInterval = interval;
                        }
                        break;
                }
            }

            // 设置窗口大小
            SetWindowSize(width, height);
            // 设置窗口位置
            SetWindowPosition(xsa, xwa, xOffset, ysa, ywa, yOffset);
            // 设置提示框内容
            TipText.Text = tipText;
            this.Opacity = 0.7;
        }

        private void SetWindowPosition(string xsa, string xwa, int xOffset, string ysa, string ywa, int yOffset)
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            double x = 0;
            double y = 0;

            // 设置X坐标
            switch (xsa)
            {
                case "left":
                    x = 0;
                    break;
                case "center":
                    x = screenWidth / 2;
                    break;
                case "right":
                    x = screenWidth;
                    break;
            }

            switch (xwa)
            {
                case "left":
                    x = 0;
                    break;
                case "center":
                    x -= windowWidth / 2;
                    break;
                case "right":
                    x -= windowWidth;
                    break;
            }

            // 调整X偏移
            if (xsa == "right")
            {
                x -= xOffset;
            }
            else
            {
                x += xOffset;
            }

            // 设置Y坐标
            switch (ysa)
            {
                case "top":
                    y = 0;
                    break;
                case "center":
                    y = screenHeight / 2;
                    break;
                case "bottom":
                    y = screenHeight - windowHeight;
                    break;
            }

            switch (ywa)
            {
                case "left":
                    y = 0;
                    break;
                case "center":
                    y -= windowHeight / 2;
                    break;
                case "right":
                    y -= windowHeight;
                    break;
            }

            // 调整Y偏移
            if (ysa == "bottom")
            {
                y -= yOffset;
            }
            else
            {
                y += yOffset;
            }

            // 设置窗口位置
            this.Left = x;
            this.Top = y;
        }

        private void SetWindowSize(double width, double height)
        {
            if (width > 0)
            {
                this.Width = width;
            }
            if (height > 0)
            {
                this.Height = height;
            }
            // 如果宽度或高度为-1，自动调整大小
            if (width == -1 || height == -1)
            {
                this.SizeToContent = SizeToContent.WidthAndHeight;
            }
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            uint extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            // 添加 WS_EX_TOOLWINDOW 样式
            extendedStyle |= WS_EX_TOOLWINDOW;
            // 添加 WS_EX_TRANSPARENT 样式
            extendedStyle |= WS_EX_TRANSPARENT;
            // 移除 WS_EX_APPWINDOW 样式
            extendedStyle &= ~WS_EX_APPWINDOW;
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle);
        }

        #region Window styles
        [Flags]
        public enum ExtendedWindowStyles
        {
            WS_EX_TOOLWINDOW = 0x00000080,
        }

        public enum GetWindowLongFields
        {
            GWL_EXSTYLE = (-20),
        }

       /*  [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex) */;

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);
        #endregion

        private void image_Loaded(object sender, RoutedEventArgs e)
        {
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);

            int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);

            exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;

            SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
        }
    }
}