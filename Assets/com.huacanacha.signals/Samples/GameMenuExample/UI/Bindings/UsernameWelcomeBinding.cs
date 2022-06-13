using UnityEngine;
using huacanacha.signal;

public class UsernameWelcomeBinding : TextBinding<GameSessionSignals, string, string> {
    protected override CachedSignal<string> GetSignal(GameSessionSignals signalProvider) => signalProvider.username;
    override protected string DefaultValue {get => "";}
    override protected System.Func<string, string> Converter {get => (s) => string.IsNullOrEmpty(s) ? "" : $"Welcome, {s}";}
}
