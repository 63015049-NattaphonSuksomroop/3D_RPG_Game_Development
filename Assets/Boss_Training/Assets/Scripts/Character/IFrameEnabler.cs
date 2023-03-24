
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public class IFrameEnabler : MonoBehaviour
	{
		[SerializeField] AbstractDamageable _damageable;

		public void EnableIFrames()
		{
			_damageable.EnableIFrames();
		}

		public void DisableIFrames()
		{
			_damageable.DisableIFrames();
		}
	}
}
