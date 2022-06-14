using System;
using System.Collections.Generic;
using UnityEngine;
using huacanacha.signal;
using huacanacha.unity.signal;

namespace huacanacha.signals.examples {

public class DateTimeSystem : MonoBehaviour {
    TimeSignals timeSignals;
    DateSignals dateSignals;

    void OnEnable() {
        timeSignals = SignalDiscovery.GetSignalProvider<TimeSignals>(this);
        dateSignals = SignalDiscovery.GetSignalProvider<DateSignals>(this);
    }

    void Update() {
        var dt = DateTime.Now;
        timeSignals?.time.Send(dt.ToString("HH:mm:ss"));
        dateSignals?.date.Send($"{dt.ToString("yyyy-MM-dd")} {1/Time.deltaTime:0}fps");
    }
}

}