using UnityEngine;
using huacanacha.signal;

public class HidingProviderBinding : TextBinding<CantFindMeSignals, string, string> {
    protected override CachedSignal<string> GetSignal(CantFindMeSignals signalProvider) => signalProvider.imHiding;
}
