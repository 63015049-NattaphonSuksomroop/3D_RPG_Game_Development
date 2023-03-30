
using Joystick;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerInput {
	public class PlayerInputController : SingletonBehaviour<PlayerInputController> {

		[SerializeField] UnityEngine.InputSystem.PlayerInput _playerInput;

		public EventHandler<GenericInputEvent> LeftJoystickInputStarted;
		public EventHandler<InputJoystickEventArgs> LeftJoystickUpdated;
		public EventHandler<GenericInputEvent> LeftJoystickInputEnded;

		public EventHandler<InputJoystickEventArgs> RightJoystickUpdate;
		public EventHandler<GenericInputEvent> RollInputPressed;
		public EventHandler<GenericInputEvent> AttackInputPressed;
		public EventHandler<GenericInputEvent> LockOnInputPressed;
		public EventHandler<GenericInputEvent> StartInputPressed;
		public EventHandler<RunEventArgs> RunningInputUpdated;
		public EventHandler<AbilityButtonPressedEventArgs> AbilityInputPressed;

		public Vector2 LastLeftInputValue { get; protected set; }
		public Vector2 LastRightInputValue { get; protected set; }

		private AbstractJoystick _directional;
		private AbstractJoystick _orbit;
		public bool IsJoystickConnected()
		{
			foreach (var name in Input.GetJoystickNames())
			{
				if (name != "")
				{
					return true;
				}
			}

			return false;
		}

		public void SetUpOnScreenJoysticks(AbstractJoystick right, AbstractJoystick left)
		{
			_directional = right;
			_orbit = left;
			_directional.InputStarted += HandleInputStartedEvent;
			_directional.InputUpdated += HandleLeftJoystickInputUpdated;
			_directional.InputEnded += HandleLeftJoystickInputEnded;

			_orbit.InputUpdated += HandleRightJoystickInputUpdated;
			_orbit.InputEnded += HandleRightJoystickInputEnded;

		}

		public void StartPause(InputAction.CallbackContext context)
		{
			var buttonContext = context.ReadValue<Single>();
			if (buttonContext == 0)
			{
				StartInputPressed?.Invoke(this, new GenericInputEvent() { FromController = true });
			}
		}

		public void Move(InputAction.CallbackContext context)
		{

			Vector2 moveVector = context.ReadValue<Vector2>();

			if (moveVector == Vector2.zero)
			{
				LeftJoystickUpdated?.Invoke(this, new InputJoystickEventArgs() { InputVector = moveVector, InputEngaged = false, FromController = true });
				LeftJoystickInputEnded?.Invoke(this, new GenericInputEvent() { FromController = true }); ;
			}
			else
			{
				if (LastLeftInputValue == Vector2.zero)
				{
					LeftJoystickInputStarted?.Invoke(this, new GenericInputEvent() { FromController = true }); ;
				}
				LeftJoystickUpdated?.Invoke(this, new InputJoystickEventArgs() { InputVector = moveVector, InputEngaged = true, FromController = true });
			}
			LastLeftInputValue = moveVector;
		}

		public void Move(Vector2 moveVector)
		{
			if (moveVector == Vector2.zero)
			{
				LeftJoystickUpdated?.Invoke(this, new InputJoystickEventArgs() { InputVector = moveVector, InputEngaged = false });
				LeftJoystickInputEnded?.Invoke(this, new GenericInputEvent() { FromController = false }); ;
			}
			else
			{
				if (LastLeftInputValue == Vector2.zero)
				{
					LeftJoystickInputStarted?.Invoke(this, new GenericInputEvent() { FromController = false }); ;
				}
				LeftJoystickUpdated?.Invoke(this, new InputJoystickEventArgs() { InputVector = moveVector, InputEngaged = true });
			}
			LastLeftInputValue = moveVector;
		}

		public void Roll(InputAction.CallbackContext context)
		{
			Single buttonContext = context.ReadValue<Single>();
			if (buttonContext == 0)
			{
				HandleRollInput(true);
			}
		}
		public void Roll()
		{
			HandleRollInput(false);
		}

		public void Attack(InputAction.CallbackContext context)
		{
			Single buttonContext = context.ReadValue<Single>();
			if (buttonContext == 0)
			{
				HandleAttackInput(true);
			}
		}
		public void Attack()
		{
			HandleAttackInput(false);
		}

		public void LockOn(InputAction.CallbackContext context)
		{
			Single lockOnButtonContext = context.ReadValue<Single>();
			if (lockOnButtonContext == 0)
			{
				HandleLockOnInput(true);
			}
		}

		public void Fire(InputAction.CallbackContext context)
		{
			Single buttonContext = context.ReadValue<Single>();
			if (buttonContext == 0)
			{
				AbilityInputPressed?.Invoke(this, new AbilityButtonPressedEventArgs() { AbilityNumber = 0, FromController = true });
			}
		}
		public void Fire()
		{ 
			AbilityInputPressed?.Invoke(this, new AbilityButtonPressedEventArgs() { AbilityNumber = 0, FromController = false });
		}

		public void Run(InputAction.CallbackContext context)
		{
			Single _runValue = context.ReadValue<Single>();
			if (_runValue == 1)
			{
				RunningInputUpdated?.Invoke(this, new RunEventArgs() { IsRunning = true, FromController = true });
			}
			else
			{
				RunningInputUpdated?.Invoke(this, new RunEventArgs() { IsRunning = false, FromController = true });
			}
		}

		public void Look(InputAction.CallbackContext context)
		{
			Vector2 _lookVector = context.ReadValue<Vector2>();

			if (_lookVector == Vector2.zero)
			{
				RightJoystickUpdate?.Invoke(this, new InputJoystickEventArgs() { InputVector = _lookVector, InputEngaged = false, FromController = true });
			}
			else
			{
				RightJoystickUpdate?.Invoke(this, new InputJoystickEventArgs() { InputVector = _lookVector, InputEngaged = true, FromController = true });
			}
			LastRightInputValue = _lookVector;
		}
		
		public void Look(Vector2 lookVector)
		{
		
			if (lookVector == Vector2.zero)
			{
				RightJoystickUpdate?.Invoke(this, new InputJoystickEventArgs() { InputVector = lookVector, InputEngaged = false, FromController = false });
			}
			else
			{
				RightJoystickUpdate?.Invoke(this, new InputJoystickEventArgs() { InputVector = lookVector, InputEngaged = true, FromController = false });
			}
			LastRightInputValue = lookVector;
		}

		private void HandleInputStartedEvent(object sender, EventArgs e)
		{
			LeftJoystickInputStarted?.Invoke(this, new GenericInputEvent() { FromController = false }); 
		}


        private void HandleLeftJoystickInputUpdated(object sender, JoystickEventArgs e)
		{
			LeftJoystickUpdated?.Invoke(this, new InputJoystickEventArgs() { InputVector = e.Input, InputEngaged = true, FromController = false });
			LastLeftInputValue = e.Input;

		}

		private void HandleLeftJoystickInputEnded(object sender, EventArgs e)
		{
			LeftJoystickUpdated?.Invoke(this, new InputJoystickEventArgs() {InputVector = Vector2.zero, InputEngaged = false, FromController = false });
			LeftJoystickInputEnded?.Invoke(this, new GenericInputEvent() { FromController = false }); ;
			LastLeftInputValue = Vector2.zero;
		}

		private void HandleRightJoystickInputUpdated(object sender, JoystickEventArgs e)
		{
			RightJoystickUpdate?.Invoke(this, new InputJoystickEventArgs() { InputVector = e.Input, InputEngaged = true, FromController = false });
		}

		private void HandleRightJoystickInputEnded(object sender, EventArgs e)
		{
			RightJoystickUpdate?.Invoke(this, new InputJoystickEventArgs() { InputVector = Vector2.zero, InputEngaged = false, FromController = false});
		}

		public void HandleRollInput(bool v)
		{
			RollInputPressed?.Invoke(this, new GenericInputEvent() { FromController = v}); ;
		}

		public void HandleAttackInput(bool v)
		{
			AttackInputPressed?.Invoke(this, new GenericInputEvent() { FromController = v }); ;
		}

		public void HandleLockOnInput(bool v)
		{
			LockOnInputPressed?.Invoke(this, new GenericInputEvent() { FromController = v }); ;
		}

		public void HandleRunInputUpdated(bool isRunning)
		{
			RunningInputUpdated?.Invoke(this, new RunEventArgs() { IsRunning = isRunning, FromController = false });
		}

		public void HandleAbilityInputUpdated(int abilityNumber)
		{
			AbilityInputPressed?.Invoke(this, new AbilityButtonPressedEventArgs() { AbilityNumber = abilityNumber, FromController = false });
		}

		public override void OnDestroy()
		{
			base.OnDestroy();

			if (_directional != null)
			{
				_directional.InputUpdated -= HandleLeftJoystickInputUpdated;
			}
			if (_orbit != null)
			{
				_orbit.InputUpdated -= HandleRightJoystickInputUpdated;
			}
		}
	}

	public class InputJoystickEventArgs : EventArgs {
		public Vector2 InputVector;
		public bool InputEngaged;
		public bool FromController;
	}

	public class RunEventArgs : EventArgs
	{
		public bool IsRunning;
		public bool FromController;
	}

	public class AbilityButtonPressedEventArgs : EventArgs
	{
		public int AbilityNumber;
		public bool FromController;
	}

	public class GenericInputEvent: EventArgs
	{
		public bool FromController;
	}

}
