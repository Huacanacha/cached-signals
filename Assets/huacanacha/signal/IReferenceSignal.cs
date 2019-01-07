namespace huacanacha.signal {
    
    public interface IReferenceSignal<T> : ICachedSignal where T : class {
        T Value {get;}
    }

}
