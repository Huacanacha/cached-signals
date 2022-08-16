using UnityEngine;
using huacanacha.signal;

namespace huacanacha.signals.examples {

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class OnFrameCountUpdated : MonoBehaviour {
    TMPro.TextMeshProUGUI text;
    SystemsSignals _signals;
    SystemThatDoesSomething _system;

    SubscriptionReceipt systemSubReceipt;

    void Start() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        text = GetComponent<TMPro.TextMeshProUGUI>();
        _signals = huacanacha.unity.signal.SignalDiscovery.GetSignalProviderAnywhere<SystemsSignals>(this);
        systemSubReceipt = _signals.systemThatDoesSomething.Subscribe(OnSystemChanged);
    }
    void OnDestroy() {
        systemSubReceipt.Unsubscribe();
    }

    void OnSystemChanged(SystemThatDoesSomething system) {
        _system = system;
        ConfigureListeners(system, isActiveAndEnabled);
    }

    void ConfigureListeners(SystemThatDoesSomething system, bool add) {
        if (system == null) return;
        if (add) {
            system.signals.frameCount.Subscribe(OnValueChanged);
        } else {
            system.signals.frameCount.UnsubscribeByCallback(OnValueChanged);
        }
    }

    void OnEnable() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        ConfigureListeners(_system, true);
    }
    void OnDisable() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        ConfigureListeners(_system, false);
    }

    void OnValueChanged(int value) {
        text.text = value.ToString();
    } 
}

}