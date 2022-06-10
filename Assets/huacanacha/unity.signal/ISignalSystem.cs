namespace huacanacha.unity.signal
{
    public interface ISignalSystem<TSignalProvider> {
        TSignalProvider Signals {get;}
    }
}
