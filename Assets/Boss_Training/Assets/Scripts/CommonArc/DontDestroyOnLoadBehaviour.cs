
using UnityEngine;

namespace CommonArchitectureUtilities {
	public class DontDestroyOnLoadBehaviour : MonoBehaviour {
		void Awake() {
			DontDestroyOnLoad(gameObject);
		}
	}
}