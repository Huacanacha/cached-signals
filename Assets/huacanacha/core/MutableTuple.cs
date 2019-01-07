namespace huacanacha.core {

    /// <summary>
    /// Provides a similar API to .NET 4.6+ Tuple classes. Should be replaced by .NET Tuples or ValueTuples where available.
    /// </summary>
   class MutableTuple<T> {
        public T Item1 { get; set; }
        public MutableTuple(T t) { Item1 = t; }
    }
    /// <summary> Provides a similar API to .NET 4.6+ Tuple classes. Should be replaced by .NET Tuples where available.</summary>
    class MutableTuple<T,U> {
        public T Item1 { get; set; }
        public U Item2 { get; set; }
        public MutableTuple(T t, U u) { Item1 = t; Item2 = u; }
    }
    /// <summary> Provides a similar API to .NET 4.6+ Tuple classes. Should be replaced by .NET Tuples where available.</summary>
    class MutableTuple<T, U, V> {
        public T Item1 { get; set; }
        public U Item2 { get; set; }
        public V Item3 { get; set; }
        public MutableTuple(T t, U u, V v) { Item1 = t; Item2 = u; Item3 = v; }
    }
    /// <summary> Provides a similar API to .NET 4.6+ Tuple classes. Should be replaced by .NET Tuples where available.</summary>
    class MutableTuple<T, U, V, W> {
        public T Item1 { get; set; }
        public U Item2 { get; set; }
        public V Item3 { get; set; }
        public W Item4 { get; set; }
        public MutableTuple(T t, U u, V v, W w) { Item1 = t; Item2 = u; Item3 = v; Item4 = w; }
    }

}