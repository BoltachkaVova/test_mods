using System.Collections.Generic;

namespace UI
{
    public class SubscriptionSystem
    {
        private readonly List<IWindow> _windows;

        public SubscriptionSystem(List<IWindow> windows)
        {
            _windows = windows;
        }

        public void Initialised()
        {
            foreach (var window in _windows)
            {
                if(window is ISubscription subscription)
                    subscription.Initialized();
            }
        }

        public void Disposable()
        {
            foreach (var window in _windows)
            {
                if(window is ISubscription subscription)
                    subscription.Disposable();
            }
        }
    }
}