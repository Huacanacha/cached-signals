namespace huacanacha.signal
{
    using System;
    using huacanacha.core;
 
    /// <summary>Signal intended to be used for data binding.</summary>
    public class BoundSignal<T> : CachedSignal<T> where T : class {
        private SubscriptionReceipt _receipt;
        void Bind(Action<T> callback) => _receipt = Subscribe(callback);
        void Unbind(Action<T> callback) => _receipt.Unsubscribe();
    }

}
