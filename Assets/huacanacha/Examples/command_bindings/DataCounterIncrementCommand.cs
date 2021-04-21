using UnityEngine;
using UnityEngine.UI;
using huacanacha.signal;
using huacanacha.unity.signal;

public class DataCounterIncrementCommand : ButtonCommand<DataSignals, int>
{
    public int incrementAmount = 1;
    protected override void Command(CachedSignal<int> signal) {
        if (!signal.HasValue) {
            signal.Send(42);
            return;
        }
        signal.Send(signal.Value+incrementAmount);
    }
    protected override CachedSignal<int> GetSignal(DataSignals signalProvider) => signalProvider.counter;
}
