using UnityEngine;
using huacanacha.signal;

public class DataCounterBinding : TextBinding<DataValueSignals, int, string> {
    protected override CachedSignal<int> GetSignal(DataValueSignals signalProvider) => signalProvider.counter;
}
