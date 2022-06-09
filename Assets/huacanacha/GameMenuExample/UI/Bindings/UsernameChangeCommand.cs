using UnityEngine;
using UnityEngine.UI;
using huacanacha.signal;
using huacanacha.unity.signal;

public class UsernameChangeCommand : InputFieldCommand<GameSessionSignals, User> {
    protected override void Command(CachedSignal<User> signal, string value) {
        signal.Value?.UpdateDataState((data) => data.username = value);
    }

    protected override CachedSignal<User> GetSignal(GameSessionSignals signalProvider) => signalProvider.user;
}
