using System;

namespace huacanacha.signal {

    public struct SubscriptionReceipt {
        private readonly Action _unsubscribe;

        internal SubscriptionReceipt(Action unsubscribe) => _unsubscribe = unsubscribe;

        public void Unsubscribe() => _unsubscribe();

        // Nothing to see here...
        static internal readonly SubscriptionReceipt emptyReceipt = new SubscriptionReceipt(() => {});
    }

}