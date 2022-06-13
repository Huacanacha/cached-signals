using UnityEngine;
using huacanacha.signal;

public class SettingsBinding : TextBinding<AppSystemSignals, Settings, string>
{
    // protected override CachedSignal<string> GetSignal(GameSessionSignals signalProvider) => signalProvider.username;
    // override protected string DefaultValue {get => "";}
    // override protected System.Func<string, string> Converter {get => (s) => string.IsNullOrEmpty(s) ? "" : $"Welcome, {s}";}
    protected override CachedSignal<Settings> GetSignal(AppSystemSignals signalProvider) => signalProvider.settings;
    override protected string DefaultValue {get => "";}
    override protected System.Func<Settings, string> Converter {get => (s) => s != null ? "nope" : s.SettingsData.ToString();}
}
