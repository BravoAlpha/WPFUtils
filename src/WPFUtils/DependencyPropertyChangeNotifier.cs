using System;
using System.Windows;
using System.Windows.Data;

namespace WPFUtils
{
    /// <summary>
    /// Notifies of property changes in a weak manner.
    /// </summary>
    internal sealed class DependencyPropertyChangeNotifier : DependencyObject, IDisposable
    {
        public event DependencyPropertyChangedEventHandler ValueChanged = delegate {};

        private readonly WeakReference _propertySource;

        /// <summary>
        /// Constructs a property notifier.
        /// </summary>
        /// <param name="propertySource">The source object to watch.</param>
        /// <param name="property">The property to watch.</param>
        public DependencyPropertyChangeNotifier(DependencyObject propertySource, DependencyProperty property)
        {
            _propertySource = new WeakReference(propertySource);
            var binding = new Binding
                              {
                                  Path = new PropertyPath(property), 
                                  Mode = BindingMode.OneWay, 
                                  Source = propertySource
                              };
            BindingOperations.SetBinding(this, WatcherProperty, binding);
        }

        /// <summary>
        /// Gets the source of the watched property.
        /// </summary>
        public DependencyObject PropertySource
        {
            get { return _propertySource.IsAlive ? _propertySource.Target as DependencyObject : null; }
        }

        public static readonly DependencyProperty WatcherProperty = 
            DependencyProperty.Register("Watcher",
                                        typeof(object),
                                        typeof(DependencyPropertyChangeNotifier),
                                        new FrameworkPropertyMetadata(null, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var notifier = (DependencyPropertyChangeNotifier)d;
            notifier.ValueChanged(notifier.PropertySource, e);
        }

        public object Watcher
        {
            get { return GetValue(WatcherProperty); }
            set { SetValue(WatcherProperty, value);  }
        }

        public void Dispose()
        {
            BindingOperations.ClearBinding(this, WatcherProperty);
        }
    }
}
