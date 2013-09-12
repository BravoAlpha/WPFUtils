using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WPFUtils.Controls
{
    internal class WindowData
    {
        public IntPtr Handle { get; private set; }
        public string Name { get; private set; }

        public WindowData(IntPtr handle, string name)
        {
            Handle = handle;
            Name = name;
        }
    }

    /// <summary>
    /// Allows you to get information about the creation and /or the destruction of windows
    /// </summary>
    internal class WindowWatcher
    {
        private static WindowWatcher m_instance;

        private readonly Dictionary<IntPtr, WindowData> m_oldWindows = new Dictionary<IntPtr, WindowData>();
        private readonly Dictionary<IntPtr, WindowData> m_currentWindows = new Dictionary<IntPtr, WindowData>();

        private readonly Thread m_watcherThread;
        private bool m_isWatching;

        private event Action<WindowData> InnerWindowCreated;
        private event Action<WindowData> InnerWindowDestroyed;
        private event Action<WindowData> InnerWindowExists;

        public static WindowWatcher Instance
        {
            get { return m_instance ?? (m_instance = new WindowWatcher()); }
        }

        /// <summary>
        /// register to this event if you want to be informed about the creation of a window
        /// </summary>
        public event Action<WindowData> WindowCreated
        {
            add
            {
                InnerWindowCreated += value;
                StartWatchingIfNeeded();
            }
            remove
            {
                InnerWindowCreated -= value;
                StopWatchingIfNoMoreSubscribers();
            }
        }

        /// <summary>
        /// register to this event, if you want to be informed about the destruction of a window
        /// </summary>
        public event Action<WindowData> WindowDestroyed
        {
            add
            {
                InnerWindowDestroyed += value;
                StartWatchingIfNeeded();
            }
            remove
            {
                InnerWindowDestroyed -= value;
                StopWatchingIfNoMoreSubscribers();
            }
        }

        /// <summary>
        /// register to this event, if you want to be informed about the existance of a window
        /// </summary>
        public event Action<WindowData> WindowExists
        {
            add
            {
                InnerWindowExists += value;
                StartWatchingIfNeeded();
            }
            remove
            {
                InnerWindowExists -= value;
                StopWatchingIfNoMoreSubscribers();
            }
        }

        private void StartWatchingIfNeeded()
        {
            if (m_isWatching)
                return;

            m_isWatching = true;
            m_watcherThread.Start();
        }

        private void StopWatchingIfNoMoreSubscribers()
        {
            if (InnerWindowCreated == null && InnerWindowDestroyed == null && InnerWindowExists == null)
                m_isWatching = false;
        }

        private WindowWatcher()
        {
            m_watcherThread = new Thread(Run);
        }

        private void Run()
        {
            while (m_isWatching)
            {
                m_currentWindows.Clear();

                IntPtr currentDesktop = IntPtr.Zero;
                Win32.User32.EnumDesktopWindows(currentDesktop, EnumWindowsProc, IntPtr.Zero);

                if (m_oldWindows.Count == 0)
                    FireExistingWindows();
                else
                {
                    FireClosedWindows();
                    FireCreatedWindows();
                    FireExistingWindows();
                }

                m_oldWindows.Clear();
                foreach (var windowEntry in m_currentWindows)
                {
                    m_oldWindows.Add(windowEntry.Key, windowEntry.Value);
                }

                Thread.Sleep(1000);
            }

            // if the hook has been uninstalled, delete the list of old windows,
            // because when it is restarted you do not want to get a whole 
            // lot of events for windows that where already there.
            m_oldWindows.Clear();
            m_currentWindows.Clear();
        }

        private bool EnumWindowsProc(IntPtr hWnd, int lParam)
        {
            int textLength = Win32.User32.GetWindowTextLength(hWnd);

            var windowTextBuffer = new StringBuilder(textLength + 1);
            Win32.User32.GetWindowText(hWnd, windowTextBuffer, windowTextBuffer.Capacity);

            var windowText = windowTextBuffer.ToString();
            if (!string.IsNullOrEmpty(windowText))
            {
                var windowData = new WindowData(hWnd, windowText);
                m_currentWindows.Add(windowData.Handle, windowData);
            }

            return true;
        }

        private void FireExistingWindows()
        {
            if (InnerWindowExists == null)
                return;

            foreach (var window in m_currentWindows)
            {
                if(InnerWindowExists != null) // Can become null if all subscribers unsubscribe during the loop
                    InnerWindowExists(window.Value);
            }
        }

        private void FireClosedWindows()
        {
            if (InnerWindowDestroyed == null)
                return;

            // if the old list contains a key that is not in the new list, that window has been destroyed
            foreach (IntPtr oldWindow in m_oldWindows.Keys)
            {
                if (!m_currentWindows.ContainsKey(oldWindow) || InnerWindowDestroyed != null)
                    InnerWindowDestroyed(m_oldWindows[oldWindow]);
            }
        }

        private void FireCreatedWindows()
        {
            if (InnerWindowCreated == null)
                return;

            // if the new list contains a key that is not in the old list, that window has been created
            foreach (IntPtr window in m_currentWindows.Keys)
            {
                if (!m_oldWindows.ContainsKey(window) && InnerWindowCreated != null)
                    InnerWindowCreated(m_currentWindows[window]);
            }
        }

        public void Shutdown()
        {
            if (m_isWatching)
            {
                m_isWatching = false;
            }
        }
    }
}
