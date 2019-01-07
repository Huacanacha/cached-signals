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
        public bool HasDispatched {get {return _hasFired;} }

   	    public override void AddListener(Action callback) {
		    AddListenerAndFireCached(callback);
	    }

   	    public override void AddOnce(Action callback) {
		    FireCachedOrAddOnce(callback);
	    }


        /// <summary>
        /// Adds the listener and calls immediately if cached signal exists.
        /// </summary>
        /// <returns>True if calling immediately.</returns>
        /// <param name="callback">Callback.</param>
        public bool AddListenerAndFireCached(Action callback) {
            base.AddListener(callback);
            if (_hasFired) {
                callback();
                return true;
            }
            return false;
        }

        /// <summary>
        /// If a cached signal exists call the callback now. Otherwise call one time on next Dispatch.
        /// </summary>
        /// <returns>True if calling immediately.</returns>
        /// <param name="callback">Callback.</param>
        public bool FireCachedOrAddOnce(Action callback) {
            if (_hasFired) {
                callback();
                return true;
            }
            base.AddOnce(callback);
            return false;
        }

        /// <summary>
        /// Call the specified callback if a cached signal exists.
        /// </summary>
        /// <returns>True if able to call</returns>
        /// <param name="callback">Callback.</param>
        public bool FireCached(Action callback) {
            if (_hasFired) {
                callback();
                return true;
            }
            return false;
        }

        public void ClearCache() {
            _hasFired = false;
        }

        new public void Dispatch() {
            _hasFired = true;
            base.Dispatch();
        }
    }

    /// <summary>CachedSignal with one parameter.</summary>
    public class CachedSignal<T> : Signal<T>, ICachedSignal {
        MutableTuple<T> _signalCache;
        public bool HasDispatched { get { return _signalCache != null; } }
        public T Value {get {return _signalCache != null ? _signalCache.Item1 : default(T);} }

	    public override void AddListener(Action<T> callback) {
		    AddListenerAndFireCached(callback);
	    }

	    public override void AddOnce(Action<T> callback) {
		    FireCachedOrAddOnce(callback);
	    }

		/// <summary>
		/// Adds the listener and calls immediately if cached signal exists.
		/// </summary>
		/// <returns>True if calling immediately.</returns>
		/// <param name="callback">Callback.</param>
		public bool AddListenerAndFireCached(Action<T> callback) {
            base.AddListener(callback);
            //UnityEngine.Debug.LogFormat("AddListenerAndFire()[{2}] - '{0}' -> '{1}'", _cachedSignal != null, _cachedSignal.Item1, GetHashCode());
            if (_signalCache != null) {
                callback(_signalCache.Item1);
                return true;
            }
            return false;
        }

        /// <summary>
        /// If a cached signal exists call the callback now. Otherwise call one time on next Dispatch.
        /// </summary>
        /// <returns>True if calling immediately.</returns>
        /// <param name="callback">Callback.</param>
        public bool FireCachedOrAddOnce(Action<T> callback) {
            if (_signalCache != null) {
                callback(_signalCache.Item1);
                return true;
            }
            base.AddOnce(callback);
            return false;
        }
		
		/// <summary>
		/// Call the specified callback if a cached signal exists.
		/// </summary>
		/// <returns>True if able to call</returns>
		/// <param name="callback">Callback.</param>
		public bool FireCached(Action<T> callback) {
            if (_signalCache != null) {
                callback(_signalCache.Item1);
                return true;
            }
            return false;
        }

        public void ClearCache() {
            _signalCache = null;
        }

        new public void Dispatch(T t) {
	        // UnityEngine.Debug.LogFormat("Dispatch()[{0}] - '{1}'", GetHashCode(), t);
            SetCache(t);
            base.Dispatch(t);
        }

        private void SetCache(T t) {
            if (_signalCache == null) {
                _signalCache = new MutableTuple<T>(t);
            } else {
                _signalCache.Item1 = t;
            }
        }
    }

    /// <summary>CachedSignal with two parameters.</summary>
    public class CachedSignal<T, U> : Signal<T,U>, ICachedSignal {
        MutableTuple<T,U> _signalCache;
        public bool HasDispatched { get { return _signalCache != null; } }

	    /// <summary>
        /// Adds the listener, and calls immediately if cached signal exists.
        /// </summary>
   	    public override void AddListener(Action<T,U> callback) {
		    AddListenerAndFireCached(callback);
	    }

        /// <summary>
        /// If a cached signal exists call the callback now. Otherwise call one time on next Dispatch.
        /// </summary>
        public override void AddOnce(Action<T,U> callback) {
		    FireCachedOrAddOnce(callback);
	    }

        /// <summary>
        /// Adds the listener, and calls immediately if cached signal exists.
        /// </summary>
        /// <returns>True if calling immediately.</returns>
        /// <param name="callback">Callback.</param>
        public bool AddListenerAndFireCached(Action<T, U> callback) {
            base.AddListener(callback);
            // UnityEngine.Debug.LogFormat("AddListenerAndCall()[{3}] - '{0}' -> '{1}', '{2}'", _signalCache != null, _signalCache.Item1, _signalCache.Item2, GetHashCode());
            if (_signalCache != null) {
                callback(_signalCache.Item1, _signalCache.Item2);
                return true;
            }
            return false;
        }

        /// <summary>
        /// If a cached signal exists call the callback now. Otherwise call one time on next Dispatch.
        /// </summary>
        /// <returns>True if calling immediately.</returns>
        /// <param name="callback">Callback.</param>
        public bool FireCachedOrAddOnce(Action<T, U> callback) {
            if (_signalCache != null) {
                callback(_signalCache.Item1, _signalCache.Item2);
                return true;
            }
            base.AddOnce(callback);
            return false;
        }

        /// <summary>
        /// Call the specified callback if a cached signal exists.
        /// </summary>
        /// <returns>True if able to call</returns>
        /// <param name="callback">Callback.</param>
        public bool FireCached(Action<T,U> callback) {
            if (_signalCache != null) {
                callback(_signalCache.Item1, _signalCache.Item2);
                return true;
            }
            return false;
        }

        public void ClearCache() {
            _signalCache = null;
        }

        new public void Dispatch(T t, U u) {
            SetCache(t, u);
            base.Dispatch(t, u);
        }

        private void SetCache(T t, U u) {
            if (_signalCache == null) {
                _signalCache = new MutableTuple<T,U>(t, u);
            } else {
                _signalCache.Item1 = t;
                _signalCache.Item2 = u;
            }
        }
    }

}
