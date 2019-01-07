namespace huacanacha.signal
{
    using System;
    using System.Collections.Generic;

    /// <summary>Basic signal with no parameters.</summary>
    public class Signal : ISignal
    {
        List<Action> Listeners = new List<Action>(2);
        List<Action> OnceListeners;

        virtual public void AddListener(Action callback) {
            if (Listeners.Contains(callback)) return;
            Listeners.Add(callback);
        }

        virtual public void AddOnce(Action callback) {
            if (OnceListeners != null && OnceListeners.Contains(callback)) return;
            OnceListeners = OnceListeners ?? new List<Action>(1);
            OnceListeners.Add(callback);
        }

        public void RemoveListener(Action callback) {
            Listeners.Remove(callback);
        }

        public void Dispatch() {
            foreach (var action in Listeners) {
                action();
            }
            if (OnceListeners == null) return;
            foreach (var action in OnceListeners) {
                action();
            }
            OnceListeners.Clear();
        }

        public string GenerateReport() {
            return string.Format("{0}: listeners={1}, onceListeners={2}",
                GetType().Name,
                Listeners.Count,
                OnceListeners?.Count);
        }
    }

    /// <summary>Basic signal with one parameters.</summary>
    public class Signal<T> : ISignal {
        List<Action<T>> Listeners = new List<Action<T>>(2);
        List<Action<T>> OnceListeners;

        virtual public void AddListener(Action<T> callback) {
            if (Listeners.Contains(callback)) return;
            Listeners.Add(callback);
        }

        virtual public void AddOnce(Action<T> callback) {
            if (OnceListeners != null && OnceListeners.Contains(callback)) return;
            OnceListeners = OnceListeners ?? new List<Action<T>>(1);
            OnceListeners.Add(callback);
        }

        public void RemoveListener(Action<T> callback) {
            Listeners.Remove(callback);
        }

        public virtual void Dispatch(T arg1) {
            foreach (var action in Listeners) {
                action(arg1);
            }
            if (OnceListeners == null) return;
            foreach (var action in OnceListeners) {
                action(arg1);
            }
            OnceListeners.Clear();
        }
    }

    /// <summary>Basic signal with two parameters</summary>
    public class Signal<T, U> : ISignal {
        List<Action<T,U>> Listeners = new List<Action<T,U>>(2);
        List<Action<T,U>> OnceListeners;

        virtual public void AddListener(Action<T,U> callback) {
            if (Listeners.Contains(callback)) return;
            Listeners.Add(callback);
        }

        virtual public void AddOnce(Action<T,U> callback) {
            if (OnceListeners != null && OnceListeners.Contains(callback)) return;
            OnceListeners = OnceListeners ?? new List<Action<T,U>>(1);
            OnceListeners.Add(callback);
        }

        public void RemoveListener(Action<T,U> callback) {
            Listeners.Remove(callback);
        }

        public virtual void Dispatch(T arg1, U arg2) {
            foreach (var action in Listeners) {
                action(arg1, arg2);
            }
            if (OnceListeners == null) return;
            foreach (var action in OnceListeners) {
                action(arg1, arg2);
            }
            OnceListeners.Clear();
        }
    }

    /// <summary>Basic signal with three parameters.</summary>
    public class Signal<T, U, V> : ISignal {
        List<Action<T,U,V>> Listeners = new List<Action<T,U,V>>(2);
        List<Action<T,U,V>> OnceListeners;

        virtual public void AddListener(Action<T,U,V> callback) {
            if (Listeners.Contains(callback)) return;
            Listeners.Add(callback);
        }

        virtual public void AddOnce(Action<T,U,V> callback) {
            if (OnceListeners != null && OnceListeners.Contains(callback)) return;
            OnceListeners = OnceListeners ?? new List<Action<T,U,V>>(1);
            OnceListeners.Add(callback);
        }

        public void RemoveListener(Action<T,U,V> callback) {
            Listeners.Remove(callback);
        }

        public virtual void Dispatch(T arg1, U arg2, V arg3) {
            foreach (var action in Listeners) {
                action(arg1, arg2, arg3);
            }
            if (OnceListeners == null) return;
            foreach (var action in OnceListeners) {
                action(arg1, arg2, arg3);
            }
            OnceListeners.Clear();
        }
    }

    /// <summary>Basic signal with four parameters.</summary>
    public class Signal<T, U, V, W> : ISignal {
        List<Action<T,U,V,W>> Listeners = new List<Action<T,U,V,W>>(2);
        List<Action<T,U,V,W>> OnceListeners;

        virtual public void AddListener(Action<T,U,V,W> callback) {
            if (Listeners.Contains(callback)) return;
            Listeners.Add(callback);
        }

        virtual public void AddOnce(Action<T,U,V,W> callback) {
            if (OnceListeners != null && OnceListeners.Contains(callback)) return;
            OnceListeners = OnceListeners ?? new List<Action<T,U,V,W>>(1);
            OnceListeners.Add(callback);
        }

        public void RemoveListener(Action<T,U,V,W> callback) {
            Listeners.Remove(callback);
        }

        public virtual void Dispatch(T arg1, U arg2, V arg3, W arg4) {
            foreach (var action in Listeners) {
                action(arg1, arg2, arg3, arg4);
            }
            if (OnceListeners == null) return;
            foreach (var action in OnceListeners) {
                action(arg1, arg2, arg3, arg4);
            }
            OnceListeners.Clear();
        }
    }

}
