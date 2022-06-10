namespace huacanacha.signal
{
    using System;
    using huacanacha.core;
 
    /// <summary>
    /// CachedSignal with no parameters.
    /// Useful for statefull one-off events such app start, indicating availability of persistent systems etc.
    /// </summary>
    public class CachedSignal : Signal, ICachedSignal {
        bool _hasFired;
        public bool HasValue {get => _hasFired;}

   	    /// <summary>
		/// Adds the listener and calls immediately if the signal has fired.
		/// </summary>
		/// <param name="callback">Callback function.</param>
        override public SubscriptionReceipt Subscribe(Action callback) {
            var receipt = base.SubscribeOnce(callback);
            if (_hasFired) {
                callback();
            }
            return receipt;
        }

        /// <summary>
		/// Adds the listener but DOES NOT call it yet, even if the signal has fired.
		/// </summary>
		/// <param name="callback">Callback function.</param>
        public SubscriptionReceipt DelayedSubscribe(Action callback) {
            var receipt = base.Subscribe(callback);
            return receipt;
        }

   	    override public SubscriptionReceipt SubscribeOnce(Action callback) {
            if (_hasFired) {
                callback();
                return new SubscriptionReceipt(() => {});
            }
            return base.SubscribeOnce(callback);
	    }

        public void ClearCache() {
            _hasFired = false;
        }

        override public void Send() {
            _hasFired = true;
            base.Send();
        }
    }

    /// <summary>CachedSignal with one parameter.</summary>
    public class CachedSignal<T> : Signal<T>, ICachedSignal {
        T _cachedValue;
        public bool HasValue { get; private set;}
        public T Value => HasValue ? _cachedValue : default(T);

		/// <summary>
		/// Adds the listener and calls immediately with the cached value, if it exists.
		/// </summary>
		/// <param name="callback">Callback.</param>
		override public SubscriptionReceipt Subscribe(Action<T> callback) {
            var receipt = base.Subscribe(callback);
            if (HasValue) {
                callback(_cachedValue);
            }
            return receipt;
        }

        /// <summary>
		/// Adds the listener but DOES NOT call it with the cached value.
		/// </summary>
		/// <param name="callback">Callback function.</param>
        public SubscriptionReceipt DelayedSubscribe(Action<T> callback) {
            var receipt = base.Subscribe(callback);
            return receipt;
        }

        /// <summary>
        /// If a cached signal exists call the callback now. Otherwise call one time on next Dispatch.
        /// </summary>
        /// <param name="callback">Callback function.</param>
        override public SubscriptionReceipt SubscribeOnce(Action<T> callback) {
            if (HasValue) {
                callback(_cachedValue);
                return new SubscriptionReceipt();
            }
            return base.SubscribeOnce(callback);
	    }
		
        public void ClearCache() {
            _cachedValue = default(T);
            HasValue = false;
        }

        new public void Send(T value) {
            // Set cache
            _cachedValue = value;
            HasValue = true;

            base.Send(value);
        }
    }

    /// <summary>CachedSignal with two parameters.</summary>
    public class CachedSignal<T, U> : Signal<T,U>, ICachedSignal<T,U> {
        (T, U) _cachedValue;
        public bool HasValue {get; private set;}
        public ValueTuple<T,U> Value => HasValue ? _cachedValue : default((T,U));

	    /// <summary>
        /// Adds the listener, and calls immediately if cached signal exists.
        /// </summary>
		/// <param name="callback">Callback function.</param>
   	    override public SubscriptionReceipt Subscribe(Action<T,U> callback) {
            var receipt = base.Subscribe(callback);
            if (HasValue) {
                callback(_cachedValue.Item1, _cachedValue.Item2);
            }
            return receipt;
        }

        /// <summary>
		/// Adds the listener but DOES NOT call it with the cached value.
		/// </summary>
		/// <param name="callback">Callback function.</param>
        public SubscriptionReceipt DelayedSubscribe(Action<T,U> callback) {
            var receipt = base.Subscribe(callback);
            return receipt;
        }

        /// <summary>
        /// If a cached signal exists call the callback now. Otherwise call one time on next Dispatch.
        /// </summary>
        override public SubscriptionReceipt SubscribeOnce(Action<T,U> callback) {
            if (HasValue) {
                callback(_cachedValue.Item1, _cachedValue.Item2);
                return new SubscriptionReceipt();//.emptyReceipt;
            }
            return base.SubscribeOnce(callback);
	    }

        public void ClearCache() {
            HasValue = false;
        }

        override public void Send(T t, U u) {
            // Set cache
            _cachedValue.Item1 = t;
            _cachedValue.Item2 = u;
            HasValue = true;

            base.Send(t, u);
        }
    }

}
