using System;
using System.Windows;
using System.Windows.Controls;

namespace WPFUtils.Controls
{
    [TemplatePart(Name = PartHost, Type = typeof(ContentControl))]
    [TemplatePart(Name = PartPlaceHolder, Type = typeof(ContentControl))]
    public class ExternalApplicationWindowControl : Control
    {
        private const string PartHost = "PART_Host";
        private const string PartPlaceHolder = "PART_PlaceHolder";

        private ContentControl m_hostControl;
        private ContentControl m_placeHolderControl;

        static ExternalApplicationWindowControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExternalApplicationWindowControl), new FrameworkPropertyMetadata(typeof(ExternalApplicationWindowControl)));
        }

        public static DependencyProperty WindowNameProperty
            = DependencyProperty.Register("WindowName",
                                          typeof(string),
                                          typeof(ExternalApplicationWindowControl),
                                          new PropertyMetadata(OnWindowNameChanged));

        public static DependencyProperty PlaceHolderContentProperty
            = DependencyProperty.Register("PlaceHolderContent",
                                          typeof(object),
                                          typeof(ExternalApplicationWindowControl),
                                          new PropertyMetadata(OnPlaceHolderContentChanged));

        public string WindowName
        {
            get { return (string)GetValue(WindowNameProperty); }
            set { SetValue(WindowNameProperty, value); }
        }

        public object PlaceHolderContent
        {
            get { return GetValue(PlaceHolderContentProperty); }
            set { SetValue(PlaceHolderContentProperty, value); }
        }

        private static void OnWindowNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = (ExternalApplicationWindowControl)obj;
            control.Update();
        }

        private static void OnPlaceHolderContentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = (ExternalApplicationWindowControl)obj;
            if (control.m_placeHolderControl == null)
                return;

            control.m_placeHolderControl.Content = args.NewValue;
        }

        public ExternalApplicationWindowControl()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            if (!IsLoaded || String.IsNullOrEmpty(WindowName) || m_hostControl == null)
                return;

            var previousHost = m_hostControl.Content as ExternalWindowHost;

            IntPtr externalWindowHandle = Win32.User32.FindWindow(null, WindowName);
            if (externalWindowHandle != IntPtr.Zero)
            {
                var externalWindowHost = new ExternalWindowHost(externalWindowHandle);
                m_hostControl.Content = externalWindowHost;

                if (m_placeHolderControl != null)
                    m_placeHolderControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                WindowWatcher.Instance.WindowExists += OnWindowWatcherWindowExists;
                if (m_placeHolderControl != null)
                    m_placeHolderControl.Visibility = Visibility.Visible;
            }

            if (previousHost != null)
                previousHost.Dispose();
        }

        private void OnWindowWatcherWindowExists(WindowData windowData)
        {
            Action action = () =>
            {
                if (windowData.Name != WindowName)
                    return;
                WindowWatcher.Instance.WindowExists -= OnWindowWatcherWindowExists;
                Update();
            };

            Dispatcher.BeginInvoke(action);
        }

        public override void OnApplyTemplate()
        {
            m_hostControl = GetTemplateChild(PartHost) as ContentControl;
            if (m_hostControl == null)
            {
                string message = String.Format("ExternalApplicationWindowControl must contain a ContentControl named {0}", PartHost);
                throw new ApplicationException(message);
            }

            m_placeHolderControl = GetTemplateChild(PartPlaceHolder) as ContentControl;
        }
    }
}
