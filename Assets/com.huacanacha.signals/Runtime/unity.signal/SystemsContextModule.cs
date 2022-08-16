namespace huacanacha.unity.signal
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;

    public class SomeSystem {
        public string name;
    }

    public interface ISystemRequester<T> {
        void OnSystemFound(T system);
    }

    public class SystemHolster<T> where T : class {

        public SystemHolster(Action<T> onFound) {
            SignalDiscovery.FindSceneContext(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), false);
        }
        public T Value {get;}
        public bool HasValue {get;}
    }

    public class IWantSomeSystem : MonoBehaviour, ISystemRequester<SomeSystem>
    {
        SystemHolster<SomeSystem> someSystem = new SystemHolster<SomeSystem>((s) => {
            Debug.Log($"System unholstered! It's name is: {s.name}");
        });


        public void OnSystemFound(SomeSystem system) {
            Debug.Log($"System found! It's name is: {system.name}");
        }
    }

    public class SystemsContextModule
    {
        internal void RequestSystem<T>(Action<T> onSystemFound) where T : class {
            T system = GetByType<T>(_systems);
            if (system != null) {
                onSystemFound(system);
                return;
            }
        }

        void RegisterWhenAvailable<T>(Action<T> onSystemFound)  where T : class {
            if (!_systemRequests.TryGetValue(typeof(T), out ArrayList callbacks)) {
                callbacks = new ArrayList();
                _systemRequests.Add(typeof(T), callbacks);
            }
            callbacks.Add(onSystemFound);
        }

        T GetByType<T>(Dictionary<System.Type, object> dictionary) where T : class {
            object item;
            if (!dictionary.TryGetValue(typeof(T), out item)) {
                return null;
            }
            return item as T;
        }

        Dictionary<System.Type, object> _systems;
        Dictionary<System.Type, ArrayList> _systemRequests;
    }

}