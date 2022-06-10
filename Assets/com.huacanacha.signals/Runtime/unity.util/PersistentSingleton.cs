using UnityEngine;

namespace huacanacha.unity.util {

	public class PersistentSingleton<T> : MonoBehaviour where T : Component {

		public static T Instance { get; private set; }

		virtual protected void Awake() {
			// Debug.Log($"PersistentSingleton<${GetType().Name}> = ${Instance} => ${this}");
			if (Instance != null) {
				Destroy(gameObject);
				return;
			}

			Instance = this as T;
			this.transform.SetParent(null);
			DontDestroyOnLoad(this.gameObject);
		}

		virtual protected void OnDestroy() {
			if (Instance != this) return;
			Instance = null;
		}
	}

}