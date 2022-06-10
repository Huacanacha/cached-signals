using System;

namespace huacanacha.signal {

    public readonly struct SubscriptionReceipt {
        private readonly Action _unsubscribe;

        internal SubscriptionReceipt(Action unsubscribe) => _unsubscribe = unsubscribe;

        public void Unsubscribe() => _unsubscribe?.Invoke();

        public bool IsValid {get => _unsubscribe != null;}

        // // Nothing to see here...
        // static internal readonly SubscriptionReceipt emptyReceipt = new SubscriptionReceipt(null);
    }

}