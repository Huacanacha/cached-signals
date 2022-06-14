using System;
using huacanacha.signal;
using huacanacha.unity.signal;
using UnityEngine;

public class User : MonoBehaviour, ISignalSystem<User.SystemSignals> {
    #region Signalling section
    public class SystemSignals {
        public readonly ActionSignal userChanged = new ActionSignal();
        public readonly CachedSignal<Data> userData = new CachedSignal<Data>();
        // For convenience. This is also avaiable via the Data object (directly or via userData signal)
        public readonly CachedSignal<bool> userSetupComplete = new CachedSignal<bool>();
    }
    readonly SystemSignals _signals = new SystemSignals();
    public SystemSignals Signals {get => _signals;}
    #endregion

    #region System data section
    public class Data {
        public string username = "";
        public bool hasAcceptedPrivacyPolicy = false;

        public bool UserSetupComplete => !string.IsNullOrEmpty(username) && hasAcceptedPrivacyPolicy;
    }
    Data _data;
    public Data UserData {get => _data;}
    #endregion

    #region System - main part!
    GameSessionSignals _gameSessionSignals;
    void Start() {
        _gameSessionSignals = SignalDiscovery.GetSignalProvider<GameSessionSignals>(this);
        _gameSessionSignals.user.Send(this);
        LoadUserData();
    }

    void LoadUserData() {
        UpdateDataState((data) => {
            // Create the initial Data object. Other code would update existing object.
            _data = new Data {
                username = PlayerPrefs.GetString("user_USERNAME", ""),
                hasAcceptedPrivacyPolicy = PlayerPrefs.GetInt("user_USER_ACCEPTED_PRIVACY_POLICY", 0) > 0,
            };
        });
    }

    /// Update the UserData inside the updateAction. The system then knows when to trigger change signals.
    public void UpdateDataState(Action<Data> updateAction) {
        updateAction(_data);
        SaveData();
        _signals.userChanged.Send();
        _signals.userData.Send(_data);
        _signals.userSetupComplete.Send(_data.UserSetupComplete);
        _gameSessionSignals.username.Send(_data.username);
    }

    private void SaveData() {
        Debug.Log("Save user data!");
        PlayerPrefs.SetString("user_USERNAME", _data.username);
        PlayerPrefs.SetInt("user_USER_ACCEPTED_PRIVACY_POLICY", _data.hasAcceptedPrivacyPolicy ? 1 : 0);
    }
    #endregion
}
