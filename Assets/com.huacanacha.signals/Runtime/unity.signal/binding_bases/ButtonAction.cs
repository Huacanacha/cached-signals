using UnityEngine;
using UnityEngine.UI;
using huacanacha.signal;
using huacanacha.unity.signal;

[RequireComponent(typeof(Button))]
public abstract class ButtonAction<TSignalProvider> : MonoBehaviour
    where TSignalProvider : class
{
    Button button;
    ActionSignal signal;
    void Awake() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        button = GetComponent<Button>();
        button.onClick.AddListener(() => signal.Send());
    }

    protected abstract ActionSignal GetSignal(TSignalProvider signalProvider);

    void OnEnable() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        if (signal == null) {
            var signalProvider = SignalDiscovery.GetSignalProviderAnywhere<TSignalProvider>(this);
            if (signalProvider != null) {
                signal = GetSignal(signalProvider);
            }
        }
    }
}
