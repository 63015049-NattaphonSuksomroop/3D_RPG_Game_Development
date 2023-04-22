
using System;
using UnityEngine;

namespace Joystick { 
	public abstract class AbstractJoystick : MonoBehaviour {

		public EventHandler<EventArgs> InputStarted;
		public EventHandler<JoystickEventArgs> InputUpdated;
		public EventHandler<EventArgs> InputEnded;

		public void SendInputStartedEvent()
		{
			InputStarted?.Invoke(this, EventArgs.Empty);
		}

		public void SendInputUpdatedEvet(Vector2 input)
		{
			InputUpdated?.Invoke(this, new JoystickEventArgs() { Input = input });
		}

		public void SendInputEndedEvent()
		{
			InputEnded?.Invoke(this, EventArgs.Empty);
		}
	}

	public class JoystickEventArgs: EventArgs {
		public Vector2 Input;
	}
}
