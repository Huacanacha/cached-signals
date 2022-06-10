using UnityEngine;

namespace huacanacha.unity.util {

	public class SceneSingleton<T> : MonoBehaviour where T : Component {

		public static T Instance { get; private set; }

		virtual protected void Awake() {
			// Debug.Log($"SceneSingleton<${GetType().Name}> = ${Instance} => ${this}");
			if (Instance != null) {
				Destroy(gameObject);
				return;
			}

			Instance = this as T;
		}

		virtual protected void OnDestroy() {
			if (Instance != this) return;
			Instance = null;
		}
	}

}