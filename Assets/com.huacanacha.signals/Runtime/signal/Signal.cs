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
        bool isSending = false;

        virtual public SubscriptionReceipt Subscribe(Action callback) {
            if (!Listeners.Contains(callback)) {
                Listeners.Add(callback);
            }
            return new SubscriptionReceipt(() => UnsubscribeListener(callback));
        }

        virtual public SubscriptionReceipt SubscribeOnce(Action callback) {
            _ToRemove.Add(callback);
            return Subscribe(callback);
        }

        virtual public void Send() {
            if (isSending) return;
            isSending = true;
            for (int i = 0; i < Listeners.Count; i++) {
                Listeners[i]();
            }
            // foreach (var action in Listeners) {
            //     action();
            // }
            isSending = false;

            if (_toRemove?.Count > 0) {
                foreach (var action in _toRemove) {
                    Listeners.Remove(action);
                }
                _toRemove.Clear();
            }
        }

        virtual public bool UnsubscribeByCallback(Action callback) {
            return Listeners.Remove(callback);
        }

        protected void UnsubscribeListener(Action callback) {
            if (isSending) {
                _ToRemove.Add(callback);
            } else {
                Listeners.Remove(callback);
            }
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
        bool isSending = false;

        virtual public SubscriptionReceipt Subscribe(Action<T> callback) {
            if (!Listeners.Contains(callback)) {
                Listeners.Add(callback);
            }
            return new SubscriptionReceipt(() => UnsubscribeListener(callback));
        }

        virtual public SubscriptionReceipt SubscribeOnce(Action<T> callback) {
            _ToRemove.Add(callback);
            return Subscribe(callback);
        }


        virtual public void Send(T arg1) {
            if (isSending) return;
            isSending = true;
            for (int i = 0; i < Listeners.Count; i++) {
                Listeners[i](arg1);
            }
            isSending = false;

            if (_toRemove?.Count > 0) {
                foreach (var action in _toRemove) {
                    Listeners.Remove(action);
                }
                _toRemove.Clear();
            }
        }

        virtual public bool UnsubscribeByCallback(Action<T> callback) {
            return Listeners.Remove(callback);
        }

        protected void UnsubscribeListener(Action<T> callback) {
            if (isSending) {
                _ToRemove.Add(callback);
            } else {
                Listeners.Remove(callback);
            }
        }
    }

    /// <summary>Basic signal with two parameters</summary>
    public class Signal<T, U> : ISignal {
        List<Action<T,U>> Listeners = new List<Action<T,U>>(2);
        List<Action<T,U>> _toRemove;
        List<Action<T,U>> _ToRemove => _toRemove ??= new List<Action<T,U>>(1);
        bool isSending = false;

        virtual public SubscriptionReceipt Subscribe(Action<T,U> callback) {
            if (!Listeners.Contains(callback)) {
                Listeners.Add(callback);
            }
            return new SubscriptionReceipt(() => UnsubscribeListener(callback));
        }

        virtual public SubscriptionReceipt SubscribeOnce(Action<T,U> callback) {
            _ToRemove.Add(callback);
            return Subscribe(callback);
        }

        virtual public void Send(T arg1, U arg2) {
            if (isSending) return;
            isSending = true;
            for (int i = 0; i < Listeners.Count; i++) {
                Listeners[i](arg1, arg2);
            }
            isSending = false;

            if (_toRemove?.Count > 0) {
                foreach (var action in _toRemove) {
                    Listeners.Remove(action);
                }
                _toRemove.Clear();
            }
        }

        virtual public bool UnsubscribeByCallback(Action<T,U> callback) {
            return Listeners.Remove(callback);
        }

        protected void UnsubscribeListener(Action<T,U> callback) {
            if (isSending) {
                _ToRemove.Add(callback);
            } else {
                Listeners.Remove(callback);
            }
        }
    }

    /// <summary>Basic signal with three parameters.</summary>
    public class Signal<T, U, V> : ISignal {
        List<Action<T,U,V>> Listeners = new List<Action<T,U,V>>(2);
        List<Action<T,U,V>> _toRemove;
        List<Action<T,U,V>> _ToRemove => _toRemove ??= new List<Action<T,U,V>>(1);
        bool isSending = false;

        virtual public SubscriptionReceipt Subscribe(Action<T,U,V> callback) {
            if (!Listeners.Contains(callback)) {
                Listeners.Add(callback);
            }
            return new SubscriptionReceipt(() => UnsubscribeListener(callback));
        }

        virtual public SubscriptionReceipt SubscribeOnce(Action<T,U,V> callback) {
            _ToRemove.Add(callback);
            return Subscribe(callback);
        }

        virtual public void Send(T arg1, U arg2, V arg3) {
            if (isSending) return;
            isSending = true;
            for (int i = 0; i < Listeners.Count; i++) {
                Listeners[i](arg1, arg2, arg3);
            }
            isSending = false;

            if (_toRemove?.Count > 0) {
                foreach (var action in _toRemove) {
                    Listeners.Remove(action);
                }
                _toRemove.Clear();
            }
        }

        virtual public bool UnsubscribeByCallback(Action<T,U,V> callback) {
            return Listeners.Remove(callback);
        }

        protected void UnsubscribeListener(Action<T,U,V> callback) {
            if (isSending) {
                _ToRemove.Add(callback);
            } else {
                Listeners.Remove(callback);
            }
        }
    }

    /// <summary>Basic signal with four parameters.</summary>
    public class Signal<T, U, V, W> : ISignal {
        List<Action<T,U,V,W>> Listeners = new List<Action<T,U,V,W>>(2);
        List<Action<T,U,V,W>> _toRemove;
        List<Action<T,U,V,W>> _ToRemove => _toRemove ??= new List<Action<T,U,V,W>>(1);
        bool isSending = false;

        virtual public SubscriptionReceipt Subscribe(Action<T,U,V,W> callback) {
            if (!Listeners.Contains(callback)) {
                Listeners.Add(callback);
            }
            return new SubscriptionReceipt(() => UnsubscribeListener(callback));
        }

        virtual public SubscriptionReceipt SubscribeOnce(Action<T,U,V,W> callback) {
            _ToRemove.Add(callback);
            return Subscribe(callback);
        }

        virtual public void Send(T arg1, U arg2, V arg3, W arg4) {
            if (isSending) return;
            isSending = true;
            for (int i = 0; i < Listeners.Count; i++) {
                Listeners[i](arg1, arg2, arg3, arg4);
            }
            isSending = false;

            if (_toRemove?.Count > 0) {
                foreach (var action in _toRemove) {
                    Listeners.Remove(action);
                }
                _toRemove.Clear();
            }
        }

        virtual public bool UnsubscribeByCallback(Action<T,U,V,W> callback) {
            return Listeners.Remove(callback);
        }

        protected void UnsubscribeListener(Action<T,U,V,W> callback) {
            if (isSending) {
                _ToRemove.Add(callback);
            } else {
                Listeners.Remove(callback);
            }
        }
    }

}
