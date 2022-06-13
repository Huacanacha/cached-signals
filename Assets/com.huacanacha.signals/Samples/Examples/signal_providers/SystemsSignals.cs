using UnityEngine;
using huacanacha.signal;
using huacanacha.unity.signal;

public class SystemsSignals : MonoBehaviour, ISignalProvider {
    readonly public ReferenceSignal<SystemThatDoesSomething> systemThatDoesSomething = new ReferenceSignal<SystemThatDoesSomething>();
    readonly public ReferenceSignal<SystemThatDoesSomethingElse> systemThatDoesSomethingElse = new ReferenceSignal<SystemThatDoesSomethingElse>();
}
