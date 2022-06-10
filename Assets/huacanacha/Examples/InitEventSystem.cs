using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitEventSystem : MonoBehaviour {
    public GameObject eventSystem;

    void Start() {
        if (UnityEngine.EventSystems.EventSystem.current == null) {
            var ev = GameObject.Instantiate(eventSystem, gameObject.transform.parent);
        }
        Destroy(this.gameObject);
    }
}
