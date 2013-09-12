using System;
using System.Windows;

namespace WPFUtils
{
    public class WeakEventListener<T> : IWeakEventListener where T : EventArgs
    {
        private readonly EventHandler<T> _handler;

        public WeakEventListener(EventHandler<T> handler)
        {
            _handler = handler;
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            T eventArgs = e as T;
            if (eventArgs == null)
                return false;

            _handler(sender, eventArgs);
            return true;
        }
    }
}