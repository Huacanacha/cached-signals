namespace huacanacha.signal
{
    using System;
    using System.Collections.Generic;

    /// <summary>Basic signal with no parameters.</summary>
    public class Signal : ISignal
    {
        List<Action> Listeners = new List<Action>(2);
        List<Action> _toRemove;
        List<Action> _ToRemove => _toRemove ??= new List<Action>(1);

        virtual public SubscriptionReceipt Subscribe(Action callback) {
            if (!Listeners.Contains(callback)) {
                Listeners.Add(callback);
            }
            return new SubscriptionReceipt(() => Listeners.Remove(callback));
        }

        virtual public SubscriptionReceipt SubscribeOnce(Action callback) {
            _ToRemove.Add(callback);
            Listeners.Add(callback);
            return new SubscriptionReceipt(() => Listeners.Remove(callback));
        }

        virtual public void Send() {
            foreach (var action in Listeners) {
                action();
            }
            if (_toRemove != null && _toRemove.Count > 0) {
                Listeners.RemoveAll(_ToRemove.Contains);
                _toRemove.Clear();
            }
        }

        virtual public bool UnsubscribeByCallback(Action callback) {
            return Listeners.Remove(callback);
        }


        public string GenerateReport() {
            return string.Format("{0}: listeners={1}, onceListeners={2}",
                GetType().Name,
                Listeners.Count,
                _toRemove?.Count);
        }
    }

    /// <summary>Basic signal with one parameters.</summary>
    public class Signal<T> : ISignal {
        List<Action<T>> Listeners = new List<Action<T>>(2);
        List<Action<T>> _toRemove;
        List<Action<T>> _ToRemove => _toRemove ??= new List<Action<T>>(1);

        virtual public SubscriptionReceipt Subscribe(Action<T> callback) {
            if (!Listeners.Contains(callback)) {
                Listeners.Add(callback);
            }
            return new SubscriptionReceipt(() => Listeners.Remove(callback));
        }

        virtual public SubscriptionReceipt SubscribeOnce(Action<T> callback) {
            _ToRemove.Add(callback);
            Listeners.Add(callback);
            return new SubscriptionReceipt(() => Listeners.Remove(callback));
        }

        virtual public void Send(T arg1) {
            foreach (var action in Listeners) {
                action(arg1);
            }
            if (_toRemove != null && _toRemove.Count > 0) {
                Listeners.RemoveAll(_ToRemove.Contains);
                _toRemove.Clear();
            }
        }

        virtual public bool UnsubscribeByCallback(Action<T> callback) {
            return Listeners.Remove(callback);
        }
    }

    /// <summary>Basic signal with two parameters</summary>
    public class Signal<T, U> : ISignal {
        List<Action<T,U>> Listeners = new List<Action<T,U>>(2);
        List<Action<T,U>> _toRemove;
        List<Action<T,U>> _ToRemove => _toRemove ??= new List<Action<T,U>>(1);

        virtual public SubscriptionReceipt Subscribe(Action<T,U> callback) {
            if (!Listeners.Contains(callback)) {
                Listeners.Add(callback);
            }
            return new SubscriptionReceipt(() => Listeners.Remove(callback));
        }

        virtual public SubscriptionReceipt SubscribeOnce(Action<T,U> callback) {
            Action<T,U> callbackThenRemove = (a, b) => {
                callback(a, b);
                Listeners.Remove(callback);
            };
            Listeners.Add(callbackThenRemove);
            return new SubscriptionReceipt(() => Listeners.Remove(callbackThenRemove));
        }

        virtual public void Send(T arg1, U arg2) {
            foreach (var action in Listeners) {
                action(arg1, arg2);
            }
            if (_toRemove != null && _toRemove.Count > 0) {
                Listeners.RemoveAll(_ToRemove.Contains);
                _toRemove.Clear();
            }
        }

        virtual public bool UnsubscribeByCallback(Action<T,U> callback) {
            return Listeners.Remove(callback);
        }
    }

    /// <summary>Basic signal with three parameters.</summary>
    public class Signal<T, U, V> : ISignal {
        List<Action<T,U,V>> Listeners = new List<Action<T,U,V>>(2);
        List<Action<T,U,V>> _toRemove;
        List<Action<T,U,V>> _ToRemove => _toRemove ??= new List<Action<T,U,V>>(1);
        

        virtual public SubscriptionReceipt Subscribe(Action<T,U,V> callback) {
            if (!Listeners.Contains(callback)) {
                Listeners.Add(callback);
            }
            return new SubscriptionReceipt(() => Listeners.Remove(callback));
        }

        virtual public SubscriptionReceipt SubscribeOnce(Action<T,U,V> callback) {
            Action<T,U,V> callbackThenRemove = (a,b,c) => {
                callback(a,b,c);
                Listeners.Remove(callback);
            };
            Listeners.Add(callbackThenRemove);
            return new SubscriptionReceipt(() => Listeners.Remove(callbackThenRemove));
        }

        virtual public void Send(T arg1, U arg2, V arg3) {
            foreach (var action in Listeners) {
                action(arg1, arg2, arg3);
            }
            if (_toRemove != null && _toRemove.Count > 0) {
                Listeners.RemoveAll(_ToRemove.Contains);
                _toRemove.Clear();
            }
        }

        virtual public bool UnsubscribeByCallback(Action<T,U,V> callback) {
            return Listeners.Remove(callback);
        }
        
    }

    /// <summary>Basic signal with four parameters.</summary>
    public class Signal<T, U, V, W> : ISignal {
        List<Action<T,U,V,W>> Listeners = new List<Action<T,U,V,W>>(2);
        List<Action<T,U,V,W>> _toRemove;
        List<Action<T,U,V,W>> _ToRemove => _toRemove ??= new List<Action<T,U,V,W>>(1);

        virtual public SubscriptionReceipt Subscribe(Action<T,U,V,W> callback) {
            if (!Listeners.Contains(callback)) {
                Listeners.Add(callback);
            }
            return new SubscriptionReceipt(() => Listeners.Remove(callback));
        }

        virtual public SubscriptionReceipt SubscribeOnce(Action<T,U,V,W> callback) {
            Action<T,U,V,W> callbackThenRemove = (a,b,c,d) => {
                callback(a,b,c,d);
                Listeners.Remove(callback);
            };
            Listeners.Add(callbackThenRemove);
            return new SubscriptionReceipt(() => Listeners.Remove(callbackThenRemove));
        }

        virtual public void Send(T arg1, U arg2, V arg3, W arg4) {
            foreach (var action in Listeners) {
                action(arg1, arg2, arg3, arg4);
            }
            if (_toRemove != null && _toRemove.Count > 0) {
                Listeners.RemoveAll(_ToRemove.Contains);
                _toRemove.Clear();
            }
        }

        virtual public bool UnsubscribeByCallback(Action<T,U,V,W> callback) {
            return Listeners.Remove(callback);
        }
    }

}
