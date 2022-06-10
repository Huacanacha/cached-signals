namespace huacanacha.signal
{
    using System;
    using huacanacha.core;
 
    /// <summary>Cached signal restricted to object references.</summary>
    public class ReferenceSignal<T> : CachedSignal<T>, IReferenceSignal<T> where T : class {}

}
