using huacanacha.signal;

public class DataTextBinding : TextBinding<DataSignals, string, string> {
    protected override CachedSignal<string> GetSignal(DataSignals signalProvider) => signalProvider.text;
}
