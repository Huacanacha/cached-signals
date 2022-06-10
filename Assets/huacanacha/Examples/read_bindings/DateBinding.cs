using UnityEngine;
using huacanacha.signal;

public class DateBinding : TextBinding<DateSignals, string, string> {
    protected override CachedSignal<string> GetSignal(DateSignals signalProvider) => signalProvider.date;
}
