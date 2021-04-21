using System;
using UnityEngine;

public class SignalViewController<SIGNAL_TYPE> : MonoBehaviour
    where SIGNAL_TYPE : class
{
    protected SIGNAL_TYPE _signals;

    protected void OnEnable() {
        // if (typeof(SIGNAL_TYPE).IsAssignableFrom(typeof(NullSignalProvider))) return;
         
        _signals = huacanacha.unity.signal.SignalDiscovery.GetSignalProvider<SIGNAL_TYPE>(this);
        if (_signals == null) {
            Debug.LogError($"Could not obtain SignalProvider '{typeof(SIGNAL_TYPE).Name}' for '{GetType().Name}'");
            enabled = false;
            return;
        }
        AddListeners();
    }

    protected void OnDisable() {
        if (_signals == null) return;
        RemoveListeners();
    }

    protected virtual void AddListeners() {}
    protected virtual void RemoveListeners() {}
}