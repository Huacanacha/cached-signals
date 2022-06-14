using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using huacanacha.signal;
using huacanacha.unity.signal;

namespace huacanacha.signals.examples {

[RequireComponent(typeof(SceneLoader))]
public class BootstrapInitializer : SignalController<AppPhaseSignals> {

    SceneLoader sceneLoader;

    List<System.Type> waitForSystems = new List<System.Type> {
        typeof(Settings),
    };
    protected override void Subscribe() {
        // Nothing yet
    }

    void Start() {
        sceneLoader = GetComponent<SceneLoader>();

        _signals.appPhase.Send(AppPhase.Initializing);


        // SubscribeOnce will trigger once as soon as Settings system comes online
        SignalDiscovery.GetSignalProvider<AppSystemSignals>(this).settings.SubscribeOnce((settings) => {
            // Then we wait for settings to load and initialize before removing from wait list
            settings.Signals.settingsData.SubscribeOnce((settingsData) => waitForSystems.Remove(typeof(Settings)));
        });

        // Anything else to initialize before loading main scene??

        // Now wait for all dependencies to resolve before declaring victory
        StartCoroutine(WaitForDependencies());
    }

    IEnumerator WaitForDependencies() {
        while (waitForSystems.Count > 0) {
            yield return null;
        }
        sceneLoader.LoadScene();
    }
}

}