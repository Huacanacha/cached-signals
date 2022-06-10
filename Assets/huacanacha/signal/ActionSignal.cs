namespace huacanacha.signal
{
    using System;
    using huacanacha.core;
 
    /// <summary>Cached trigger/action signal. Can be queried if it has ever fired.</summary>
    public class ActionSignal : CachedSignal {
        public bool HasFired => HasValue;
    }

}
