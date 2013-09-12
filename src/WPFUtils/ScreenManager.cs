using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System;
using System.Runtime.InteropServices;

namespace WPFUtils
{
    public class ScreenManager
    {
        private static class DpiHelper
        {
            [DllImport("User32.dll")]
            private static extern IntPtr GetDC(HandleRef hWnd);

            [DllImport("User32.dll")]
            private static extern int ReleaseDC(HandleRef hWnd, HandleRef hDC);

            [DllImport("GDI32.dll")]
            private static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

            private static int m_dpi = 0;

            public static int DPI
            {
                get
                {
                    if (m_dpi == 0)
                    {

                        HandleRef desktopHwnd = new HandleRef(null, IntPtr.Zero);
                        HandleRef desktopDC = new HandleRef(null, GetDC(desktopHwnd));

                        m_dpi = GetDeviceCaps(desktopDC, 88 /*LOGPIXELSX*/);
                        ReleaseDC(desktopHwnd, desktopDC);
                    }

                    return m_dpi;
                }
            }

            public static Rectangle ConvertPixelsToDIPixels(Rectangle rectangle)
            {
                double x = ConvertPixelsToDIPixels(rectangle.X);
                double y = ConvertPixelsToDIPixels(rectangle.Y);
                double width = ConvertPixelsToDIPixels(rectangle.Width);
                double height = ConvertPixelsToDIPixels(rectangle.Height);

                var rect = new Rectangle((int)x, (int)y, (int)width, (int)height);
                return rect;
            }

            public static double ConvertPixelsToDIPixels(int pixels)
            {
                return (double)pixels * 96 / DPI;
            }
        }

        private readonly IList<Rectangle> m_screenBounds;

        public ScreenManager()
        {
            m_screenBounds = ProbeScreenBounds();
        }

        private IList<Rectangle> ProbeScreenBounds()
        {
            var screenBoundsList = new List<Rectangle>();

            foreach (Screen screen in Screen.AllScreens)
            {
                Rectangle currentBounds = DpiHelper.ConvertPixelsToDIPixels(screen.Bounds);

                if (screenBoundsList.Count == 0)
                {
                    screenBoundsList.Add(currentBounds);
                    continue;
                }

                int index = screenBoundsList.TakeWhile(bounds => (currentBounds.Top >= bounds.Top) && (currentBounds.Left >= bounds.Left)).Count();
                screenBoundsList.Insert(index, currentBounds);
            }

            return screenBoundsList;
        }

        public void Place(Window window, int screenNumber)
        {
            if (window == null)
                return;

            Rectangle screenBounds = GetScreenBounds(screenNumber);
            if (screenBounds != Rectangle.Empty)
            {
                // Maximized WindowState will place it on the primary screen.
                if (screenNumber > 0)
                    window.WindowState = WindowState.Normal;

                window.Left = screenBounds.X;
                window.Top = screenBounds.Y;
                window.Width = screenBounds.Width;
                window.Height = screenBounds.Height;
            }
            else
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        public Rectangle GetScreenBounds(int zeroBasedScreenIndex)
        {
            return !IsScreenIndexValid(zeroBasedScreenIndex) ? Rectangle.Empty : m_screenBounds[zeroBasedScreenIndex];
        }

        private bool IsScreenIndexValid(int zeroBasedScreenIndex)
        {
            return zeroBasedScreenIndex >= 0 && zeroBasedScreenIndex < m_screenBounds.Count;
        } 
    }
}