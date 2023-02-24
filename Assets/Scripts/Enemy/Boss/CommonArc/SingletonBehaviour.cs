
using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
	private static T singletonInstance = null;
	public static T Instance {
		get {
			if (singletonInstance == null) {
				singletonInstance = FindObjectOfType<T>();
			}
			return singletonInstance;
		}
	}

	public virtual void Awake() {
		if (singletonInstance != null) {
			Debug.LogWarning("Singleton<" + typeof(T) + "> already exists! Something may have called Instance accessor before Awake.");
		}
		else {
			singletonInstance = this as T;
		}
	}

	public virtual void OnDestroy() {
		if (singletonInstance == this) {
			singletonInstance = null;
		}
	}
}
