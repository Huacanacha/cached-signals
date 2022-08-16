using System;
using System.Collections.Generic;
using huacanacha.signal;
using UnityEngine;

namespace huacanacha.unity.signal {
public abstract class SignalController<SIGNAL_TYPE> : MonoBehaviour
    where SIGNAL_TYPE : class
{
    protected SIGNAL_TYPE _signals;
    protected List<SubscriptionReceipt> _subscriptions = new List<SubscriptionReceipt>();

    protected void OnEnable() {
        AttachSignals();
    }

    protected void OnDisable() {
        Unsubscribe();
    }

    protected void AttachSignals() {
        _signals ??= huacanacha.unity.signal.SignalDiscovery.GetLocalSignalProvider<SIGNAL_TYPE>(this);
        if (_signals == null) {
            Debug.LogError($"Could not obtain SignalProvider '{typeof(SIGNAL_TYPE).Name}' for '{GetType().Name}'");
            enabled = false;
            return;
        }
        Subscribe();
    }

    protected abstract void Subscribe();
    protected void Unsubscribe() {
        foreach (var receipt in _subscriptions) {
            receipt.Unsubscribe();
        }
        _subscriptions.Clear();
    }
}

}