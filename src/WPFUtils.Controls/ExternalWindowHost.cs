using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace WPFUtils.Controls
{
    public class ExternalWindowHost : HwndHost
    {
        private readonly IntPtr m_externalWindowHandle;
        private Win32.User32.WindowStyles m_originalWindowStyle;
        private Win32.User32.Rect m_originalBounds;
        private Win32.User32.WindowStylesEx m_originalWindowStyleEx;

        public ExternalWindowHost(IntPtr externalWindowHandle)
        {
            m_externalWindowHandle = externalWindowHandle;
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            Win32.User32.GetWindowRect(m_externalWindowHandle, out m_originalBounds);
            Win32.User32.SetParent(m_externalWindowHandle, hwndParent.Handle);

            var style = (Win32.User32.WindowStyles)Win32.User32.GetWindowLong(m_externalWindowHandle, (int)Win32.User32.WindowLongFlags.GWL_STYLE);
            m_originalWindowStyle = style;
            var extendedStyle = (Win32.User32.WindowStylesEx)Win32.User32.GetWindowLong(m_externalWindowHandle, (int)Win32.User32.WindowLongFlags.GWL_EXSTYLE);
            m_originalWindowStyleEx = extendedStyle;

            style = style & ~(Win32.User32.WindowStyles.Popup | Win32.User32.WindowStyles.SysMenu |
                              Win32.User32.WindowStyles.MinimizeBox | Win32.User32.WindowStyles.Caption);
            style = style | Win32.User32.WindowStyles.Child;

            extendedStyle = extendedStyle & ~(Win32.User32.WindowStylesEx.WS_EX_CLIENTEDGE);

            Win32.User32.SetWindowLong(m_externalWindowHandle, (int)Win32.User32.WindowLongFlags.GWL_STYLE, (int)style);
            Win32.User32.SetWindowLong(m_externalWindowHandle, (int)Win32.User32.WindowLongFlags.GWL_EXSTYLE, (int)extendedStyle);

            Win32.User32.SetWindowPos(m_externalWindowHandle, Win32.User32.InsertAfterHandleTypes.Top, 0, 0, (int)Width,
                                      (int)Height, Win32.User32.SetWindowPosFlags.FrameChanged | Win32.User32.SetWindowPosFlags.ShowWindow);

            return new HandleRef(this, m_externalWindowHandle);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            Win32.User32.SetWindowLong(hwnd.Handle, (int)Win32.User32.WindowLongFlags.GWL_STYLE, (int)m_originalWindowStyle);
            Win32.User32.SetWindowLong(hwnd.Handle, (int)Win32.User32.WindowLongFlags.GWL_EXSTYLE, (int)m_originalWindowStyleEx);
            Win32.User32.SetParent(hwnd.Handle, IntPtr.Zero);
            Win32.User32.SetWindowPos(hwnd.Handle, Win32.User32.InsertAfterHandleTypes.Top,
                                      m_originalBounds.Left, m_originalBounds.Top,
                                      m_originalBounds.Right - m_originalBounds.Left,
                                      m_originalBounds.Bottom - m_originalBounds.Top,
                                      Win32.User32.SetWindowPosFlags.FrameChanged);
        }
    }
}