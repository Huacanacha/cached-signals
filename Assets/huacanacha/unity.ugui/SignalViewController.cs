// using System;
// using System.Collections.Generic;
// using huacanacha.signal;
// using UnityEngine;


// public abstract class SignalViewController<SIGNAL_TYPE> : MonoBehaviour
//     where SIGNAL_TYPE : class
// {
//     public enum AttachmentType {ON_ENABLED, ON_START}

//     public AttachmentType attachmentType = AttachmentType.ON_ENABLED;

//     protected SIGNAL_TYPE _signals;
//     protected List<SubscriptionReceipt> _subscriptions = new List<SubscriptionReceipt>();

//     protected void Start() {
//         if (attachmentType == AttachmentType.ON_START) {
//             AttachSignals();
//         }
//     }
//     protected void OnEnable() {
//         // if (typeof(SIGNAL_TYPE).IsAssignableFrom(typeof(NullSignalProvider))) return;
//         if (attachmentType == AttachmentType.ON_ENABLED) {
//             AttachSignals();
//         }
//     }

//     protected void OnDisable() {
//         if (attachmentType == AttachmentType.ON_ENABLED) {
//             Unsubscribe();
//         }
//     }
//     protected void OnDestroy() {
//         if (attachmentType == AttachmentType.ON_START) {
//             Unsubscribe();
//         }
//     }

//     protected void AttachSignals() {
//         _signals ??= huacanacha.unity.signal.SignalDiscovery.GetSignalProvider<SIGNAL_TYPE>(this);
//         if (_signals == null) {
//             Debug.LogError($"Could not obtain SignalProvider '{typeof(SIGNAL_TYPE).Name}' for '{GetType().Name}'");
//             enabled = false;
//             return;
//         }
//         Subscribe();
//     }

//     protected abstract void Subscribe();
//     protected void Unsubscribe() {
//         foreach (var receipt in _subscriptions) {
//             receipt.Unsubscribe();
//         }
//         _subscriptions.Clear();
//     }
// }