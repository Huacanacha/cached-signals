using UnityEngine;
using huacanacha.signal;

namespace huacanacha.signals.examples {

public class HidingProviderBinding : TextBinding<CantFindMeSignals, string, string> {
    protected override CachedSignal<string> GetSignal(CantFindMeSignals signalProvider) => signalProvider.imHiding;
}

}