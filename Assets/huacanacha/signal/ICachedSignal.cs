using System;

namespace huacanacha.signal {
    
    public interface ICachedSignal : ISignal {
        /// <summary>True if Dispatch has been called (and therefore a cached value exists).</summary>
        bool HasValue {get;}
        /// <summary>Removes cached value.</summary>
        void ClearCache();
    
    }
    public interface ICachedSignal<T> : ICachedSignal {
        /// <summary>Returns the cached value if cached, otherwise default(T).</summary>
        T Value {get;}
    }

    public interface ICachedSignal<T,U> : ICachedSignal {
        /// <summary>Returns the cached value if cached, otherwise default(T).</summary>
        ValueTuple<T,U> Value {get;}
    }
}
