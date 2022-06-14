using System;
using System.Collections;
using huacanacha.signal;
using huacanacha.unity.signal;
using UnityEngine;

namespace huacanacha.signals.examples {

public class Settings : MonoBehaviour, ISignalSystem<Settings.SystemSignals> {
    #region Signalling section
    public class SystemSignals {
        public readonly ActionSignal settingsChanged = new ActionSignal();
        public readonly CachedSignal<Data> settingsData = new CachedSignal<Data>();
    }
    readonly SystemSignals _signals = new SystemSignals();
    public SystemSignals Signals {get => _signals;}
    #endregion

    #region System data section
    public enum GraphicsQuality {Low, Medium, High, UltraMegaSuperGalactic}
    public class Data {
        public bool audioOn = true;
        /// Range [0,1]
        public float volume = 1;
        public GraphicsQuality graphicsQuality = GraphicsQuality.High;
        public bool antimatterReactorEnabled = false;

        override public string ToString() {
            return $"audioOn= {audioOn}\nvolume= {volume*100:0}%\ngraphicsQuality= {graphicsQuality}\nantimatterReactorEnabled= {antimatterReactorEnabled}";
        }
    }
    Data _data;
    public Data SettingsData {get => _data;}
    #endregion

    #region System logic & API section (i.e. main part!)
    void Start() {
        SignalDiscovery.GetSignalProvider<AppSystemSignals>(this).settings.Send(this);
        StartCoroutine(PretendToLoadSettingsData());
    }

    IEnumerator PretendToLoadSettingsData() {
        yield return new WaitForSecondsRealtime(1); // Must be a spinning rust drive!
        UpdateDataState((data) => {
            _data = new Data();
        });
    }

    /// Update the SettingsData inside the updateAction. The system then knows when to trigger change signals.
    public void UpdateDataState(Action<Data> updateAction) {
        updateAction(_data);
        _signals.settingsChanged.Send();
        _signals.settingsData.Send(_data);
    }
    #endregion
}

}