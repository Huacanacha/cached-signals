using UnityEngine;
using UnityEngine.UI;
using huacanacha.signal;
using huacanacha.unity.signal;

public class DataCounterIncrementCommand : ButtonCommand<DataSignals, int>
{
    public int incrementAmount = 1;
    public int initialVaue = 42;
    protected override void Command(CachedSignal<int> signal) {
        if (!signal.HasValue) {
            signal.Send(initialVaue);
            return;
        }
        signal.Send(signal.Value+incrementAmount);
    }
    protected override CachedSignal<int> GetSignal(DataSignals signalProvider) => signalProvider.counter;
}
