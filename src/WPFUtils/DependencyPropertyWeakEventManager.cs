using System.ComponentModel;
using System.Windows;

namespace WPFUtils
{
    public class DependencyPropertyWeakEventManager : WeakEventManager
    {
        private static readonly object SyncObj = new object();

        private static DependencyPropertyWeakEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(DependencyPropertyWeakEventManager);
                var currentManager = WeakEventManager.GetCurrentManager(managerType) as DependencyPropertyWeakEventManager;

                if (currentManager == null)
                {
                    lock(SyncObj)
                    {
                        currentManager = WeakEventManager.GetCurrentManager(managerType) as DependencyPropertyWeakEventManager;
                        if (currentManager == null)
                        {
                            currentManager = new DependencyPropertyWeakEventManager();
                            WeakEventManager.SetCurrentManager(managerType, currentManager);
                        }
                    }
                }

                return currentManager;
            }
        }

        private static DependencyProperty GetDependencyProperty(DependencyObject obj)
        {
            return (DependencyProperty) obj.GetValue(DependencyPropertyProperty);
        }

        private static void SetDependencyProperty(DependencyObject obj, DependencyProperty value)
        {
            obj.SetValue(DependencyPropertyProperty, value);
        }

        private static readonly DependencyProperty DependencyPropertyProperty =
            DependencyProperty.RegisterAttached("DependencyProperty",
                                                typeof (DependencyProperty),
                                                typeof (DependencyPropertyWeakEventManager),
                                                new UIPropertyMetadata(null));

        public static void AddListener(DependencyObject source, IWeakEventListener listener, DependencyProperty property)
        {
            SetDependencyProperty(source, property);
            CurrentManager.ProtectedAddListener(source, listener);
        }

        public static void RemoveListener(DependencyObject source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        protected override void StartListening(object source)
        {
            var property = GetDependencyProperty((DependencyObject) source);
            DependencyPropertyDescriptor.FromProperty(property, property.OwnerType).AddValueChanged(source, DeliverEvent);
        }

        protected override void StopListening(object source)
        {
            var dependencyObject = (DependencyObject) source;
            var property = GetDependencyProperty(dependencyObject);
            DependencyPropertyDescriptor.FromProperty(property, property.OwnerType).RemoveValueChanged(dependencyObject, DeliverEvent);
            SetDependencyProperty(dependencyObject, null);
        }
    }
}