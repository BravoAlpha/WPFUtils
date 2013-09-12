using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WPFUtils.Controls
{
    public static class Win32
    {
        public static class User32
        {
            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChild, string lpClassName, string lpWindowName);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll")]
            public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetWindowRect(IntPtr hwnd, out Rect lpRect);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosFlags uFlags);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            public static extern IntPtr CreateWindowEx(int dwExStyle, string lpszClassName, string lpszWindowName,
                                                       int style, int x, int y, int width, int height,
                                                       IntPtr hwndParent, IntPtr hMenu, IntPtr hInst,
                                                       [MarshalAs(UnmanagedType.AsAny)] object pvParam);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            public static extern bool DestroyWindow(IntPtr hwnd);

            [DllImport("user32.dll")]
            public static extern bool EnumDesktopWindows(IntPtr desktop, EnumDelegate callback, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int GetWindowTextLength(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int GetWindowText(IntPtr hWnd, StringBuilder windowText, int manCount);

            public delegate bool EnumDelegate(IntPtr hWnd, int lParam);

            public enum WindowLongFlags : int
            {
                GWL_EXSTYLE = -20,
                GWLP_HINSTANCE = -6,
                GWLP_HWNDPARENT = -8,
                GWL_ID = -12,
                GWL_STYLE = -16,
                GWL_USERDATA = -21,
                GWL_WNDPROC = -4,
                DWLP_USER = 0x8,
                DWLP_MSGRESULT = 0x0,
                DWLP_DLGPROC = 0x4
            }

            /// <summary>
            /// Window Styles.
            /// The following styles can be specified wherever a window style is required. 
            /// After the control has been created, these styles cannot be modified, except as noted.
            /// </summary>
            [Flags]
            public enum WindowStyles : uint
            {
                /// <summary>
                /// The window has a thin-line border.
                /// </summary>
                Border = 0x800000,

                /// <summary>
                /// The window has a title bar (includes the Border style).
                /// </summary>
                Caption = 0xc00000,

                /// <summary>
                /// The window is a child window. 
                /// A window with this style cannot have a menu bar. 
                /// This style cannot be used with the Popup style.
                /// </summary>
                Child = 0x40000000,

                /// <summary>
                /// Excludes the area occupied by child windows when drawing occurs within the parent window. 
                /// This style is used when creating the parent window.
                /// </summary>
                ClipChildren = 0x2000000,

                /// <summary>
                /// Clips child windows relative to each other; 
                /// that is, when a particular child window receives a WM_PAINT message, 
                /// the ClipSiblings style clips all other overlapping child windows out of the region of the child window to be updated.
                /// If ClipSiblings is not specified and child windows overlap, 
                /// it is possible, when drawing within the client area of a child window, 
                /// to draw within the client area of a neighboring child window.
                /// </summary>
                ClipSiblings = 0x4000000,

                /// <summary>
                /// The window is initially disabled. 
                /// A disabled window cannot receive input from the user. 
                /// To change this after a window has been created, use the EnableWindow function.
                /// </summary>
                Disabled = 0x8000000,

                /// <summary>
                /// The window has a border of a style typically used with dialog boxes. 
                /// A window with this style cannot have a title bar.
                /// </summary>
                DlgFrame = 0x400000,

                /// <summary>
                /// The window is the first control of a group of controls. 
                /// The group consists of this first control and all controls defined after it, up to the next control with the Group style.
                /// The first control in each group usually has the TabStop style so that the user can move from group to group. 
                /// The user can subsequently change the keyboard focus from one control in the group to the next control in the group by using the direction keys.
                /// You can turn this style on and off to change dialog box navigation. 
                /// To change this style after a window has been created, use the SetWindowLong function.
                /// </summary>
                Group = 0x20000,

                /// <summary>
                /// The window has a horizontal scroll bar.
                /// </summary>
                HorizontalScrollBar = 0x100000,

                /// <summary>
                /// The window is initially maximized.
                /// </summary>
                Maximize = 0x1000000,

                /// <summary>
                /// The window has a maximize button. 
                /// Cannot be combined with the WS_EX_CONTEXTHELP style. 
                /// The SysMenu style must also be specified.
                /// </summary>
                MaximizeBox = 0x10000,

                /// <summary>
                /// The window is initially minimized.
                /// </summary>
                Minimize = 0x20000000,

                /// <summary>
                /// The window has a minimize button. 
                /// Cannot be combined with the WS_EX_CONTEXTHELP style. 
                /// The SysMenu style must also be specified.
                /// </summary>
                MinimizeBox = 0x20000,

                /// <summary>
                /// The window is an overlapped window. 
                /// An overlapped window has a title bar and a border.
                /// </summary>
                Overlapped = 0x0,

                /// <summary>
                /// The window is an overlapped window.
                /// </summary>
                OverlappedWindow = Overlapped | Caption | SysMenu | SizeFrame | MinimizeBox | MaximizeBox,

                /// <summary>
                /// The window is a pop-up window. 
                /// This style cannot be used with the Child style.
                /// </summary>
                Popup = 0x80000000u,

                /// <summary>
                /// The window is a pop-up window. 
                /// The Caption and PopupWindow styles must be combined to make the window menu visible.
                /// </summary>
                PopupWindow = Popup | Border | SysMenu,

                /// <summary>
                /// The window has a sizing border.
                /// </summary>
                SizeFrame = 0x40000,

                /// <summary>
                /// The window has a window menu on its title bar. 
                /// The Caption style must also be specified.
                /// </summary>
                SysMenu = 0x80000,

                /// <summary>
                /// The window is a control that can receive the keyboard focus when the user presses the TAB key.
                /// Pressing the TAB key changes the keyboard focus to the next control with the TabStop style.  
                /// You can turn this style on and off to change dialog box navigation. 
                /// To change this style after a window has been created, use the SetWindowLong function.
                /// For user-created windows and modeless dialogs to work with tab stops, 
                /// alter the message loop to call the IsDialogMessage function.
                /// </summary>
                TabStop = 0x10000,

                /// <summary>
                /// The window is initially visible. 
                /// This style can be turned on and off by using the ShowWindow or SetWindowPos function.
                /// </summary>
                Visible = 0x10000000,

                /// <summary>
                /// The window has a vertical scroll bar.
                /// </summary>
                VerticalScrollBar = 0x200000
            }

            /// <summary>
            /// Window Extended Styles.
            [Flags]
            public enum WindowStylesEx : uint
            {
                WS_EX_DLGMODALFRAME = 0x0001,
                WS_EX_NOPARENTNOTIFY = 0x0004,
                WS_EX_TOPMOST = 0x0008,
                WS_EX_ACCEPTFILES = 0x0010,
                WS_EX_TRANSPARENT = 0x0020,
                WS_EX_MDICHILD = 0x0040,
                WS_EX_TOOLWINDOW = 0x0080,
                WS_EX_WINDOWEDGE = 0x0100,
                WS_EX_CLIENTEDGE = 0x0200,
                WS_EX_CONTEXTHELP = 0x0400,
                WS_EX_RIGHT = 0x1000,
                WS_EX_LEFT = 0x0000,
                WS_EX_RTLREADING = 0x2000,
                WS_EX_LTRREADING = 0x0000,
                WS_EX_LEFTSCROLLBAR = 0x4000,
                WS_EX_RIGHTSCROLLBAR = 0x0000,
                WS_EX_CONTROLPARENT = 0x10000,
                WS_EX_STATICEDGE = 0x20000,
                WS_EX_APPWINDOW = 0x40000,
                WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),
                WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST),
                WS_EX_LAYERED = 0x00080000,
                WS_EX_NOINHERITLAYOUT = 0x00100000, // Disable inheritence of mirroring by children
                WS_EX_LAYOUTRTL = 0x00400000, // Right to left mirroring
                WS_EX_COMPOSITED = 0x02000000,
                WS_EX_NOACTIVATE = 0x08000000,
            }

            public enum WindowMessages : uint
            {
                /// <summary>
                /// The WM_COMMAND message is sent when the user selects a command item from a menu, 
                /// when a control sends a notification message to its parent window, or when an accelerator keystroke is translated.
                /// </summary>
                Command = 0x0111
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct Rect
            {
                public int Left, Top, Right, Bottom;

                public Rect(int left, int top, int right, int bottom)
                {
                    Left = left;
                    Top = top;
                    Right = right;
                    Bottom = bottom;
                }
            }

            /// <summary>
            /// Window handles (HWND) used for hWndInsertAfter
            /// </summary>
            public static class InsertAfterHandleTypes
            {
                public static IntPtr NoTopMost = new IntPtr(-2);
                public static IntPtr TopMost = new IntPtr(-1);
                public static IntPtr Top = new IntPtr(0);
                public static IntPtr Bottom = new IntPtr(1);
            }

            [Flags]
            public enum SetWindowPosFlags : uint
            {
                /// <summary>
                /// If the calling thread and the thread that owns the window are attached to different input queues,
                /// the system posts the request to the thread that owns the window. This prevents the calling thread from
                /// blocking its execution while other threads process the request.
                /// </summary>
                /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
                AsynchronousWindowPosition = 0x4000,
                /// <summary>
                /// Prevents generation of the WM_SYNCPAINT message.
                /// </summary>
                /// <remarks>SWP_DEFERERASE</remarks>
                DeferErase = 0x2000,
                /// <summary>
                /// Draws a frame (defined in the window's class description) around the window.
                /// </summary>
                /// <remarks>SWP_DRAWFRAME</remarks>
                DrawFrame = 0x0020,
                /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to
                /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE
                /// is sent only when the window's size is being changed.</summary>
                /// <remarks>SWP_FRAMECHANGED</remarks>
                FrameChanged = 0x0020,
                /// <summary>Hides the window.</summary>
                /// <remarks>SWP_HIDEWINDOW</remarks>
                HideWindow = 0x0080,
                /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the
                /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter
                /// parameter).</summary>
                /// <remarks>SWP_NOACTIVATE</remarks>
                DoNotActivate = 0x0010,
                /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid
                /// contents of the client area are saved and copied back into the client area after the window is sized or
                /// repositioned.</summary>
                /// <remarks>SWP_NOCOPYBITS</remarks>
                DoNotCopyBits = 0x0100,
                /// <summary>Retains the current position (ignores X and Y parameters).</summary>
                /// <remarks>SWP_NOMOVE</remarks>
                IgnoreMove = 0x0002,
                /// <summary>Does not change the owner window's position in the Z order.</summary>
                /// <remarks>SWP_NOOWNERZORDER</remarks>
                DoNotChangeOwnerZOrder = 0x0200,
                /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to
                /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent
                /// window uncovered as a result of the window being moved. When this flag is set, the application must
                /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
                /// <remarks>SWP_NOREDRAW</remarks>
                DoNotRedraw = 0x0008,
                /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
                /// <remarks>SWP_NOREPOSITION</remarks>
                DoNotReposition = 0x0200,
                /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
                /// <remarks>SWP_NOSENDCHANGING</remarks>
                DoNotSendChangingEvent = 0x0400,
                /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
                /// <remarks>SWP_NOSIZE</remarks>
                IgnoreResize = 0x0001,
                /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
                /// <remarks>SWP_NOZORDER</remarks>
                IgnoreZOrder = 0x0004,
                /// <summary>Displays the window.</summary>
                /// <remarks>SWP_SHOWWINDOW</remarks>
                ShowWindow = 0x0040,
            }
        }
    }
}