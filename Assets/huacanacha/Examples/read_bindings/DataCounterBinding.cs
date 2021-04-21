using UnityEngine;
using huacanacha.signal;

public class DataCounterBinding : TextBinding<DataSignals, int, string> {
    protected override CachedSignal<int> GetSignal(DataSignals signalProvider) => signalProvider.counter;
}
