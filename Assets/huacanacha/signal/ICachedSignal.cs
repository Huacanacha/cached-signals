namespace huacanacha.signal {
    
    public interface ICachedSignal : ISignal {
        /// <summary>True if Dispatch has been called (and therefore a cached value exists).</summary>
        bool HasDispatched {get;}
        /// <summary>Removes cached value.</summary>
        void ClearCache();
    }

}
