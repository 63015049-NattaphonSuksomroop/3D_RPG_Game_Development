
using Character;
using Player;
using PlayerCamera;
using PlayerInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MlPlayerManager : PlayerManager
{
	[SerializeField] PlayerInputController _mlInput;
	[SerializeField] ThirdPersonCameraMovement _cam;
	[SerializeField] bool _initOnStart;
	private bool _listenToControllerEvents;

	public ThirdPersonCameraMovement ML_Can
	{
		get => _cam;
	}


	override protected void Awake()
	{
		base.Awake();
		if (_initOnStart)
		{
			Init();
		}
	}


	public void Init()
	{
		_cam.OverrideEvents(_mlInput, this);
		CameraMovement = _cam;
		_playerLockOnManager.Init(_cam);

		_thirdPersonMovement.Init(_cam.transform);

		_abilityHandler.Init(CharacterID);

		_stats.CharacterDied += HandleCharacterDeathEvent;

		ListenForInput();
	}

	public void ListenForInput()
	{
		_mlInput.AttackInputPressed += HandleAttackInputPressed;
		_mlInput.LeftJoystickUpdated += HandleLeftJoystickUpdated;
		_mlInput.RollInputPressed += HandleRollInputPressed;
		_mlInput.RunningInputUpdated += HandleRunningInputUpdated;
		_mlInput.AbilityInputPressed += HandleAbilityInputPressed;
	}


	public void RemoveInputListeners()
	{
		_mlInput.AttackInputPressed -= HandleAttackInputPressed;
		_mlInput.LeftJoystickUpdated -= HandleLeftJoystickUpdated;
		_mlInput.RollInputPressed -= HandleRollInputPressed;
		_mlInput.RunningInputUpdated -= HandleRunningInputUpdated;
		_mlInput.AbilityInputPressed -= HandleAbilityInputPressed;
	}

	public void ListToControllerEvents(bool value)
	{
		_listenToControllerEvents = value;
	}

	public void ZeroOutLastInput()
	{
		var input = new InputJoystickEventArgs() { InputEngaged = false, InputVector = Vector2.zero };
		_thirdPersonMovement.DirectionUpdated(input);
	}

	public void SetTarget(AbstractCharacterManager abstractCharacterManager)
	{
		_playerLockOnManager.SetLockOnTarget(abstractCharacterManager);
	}

	private void HandleAbilityInputPressed(object sender, AbilityButtonPressedEventArgs e)
	{
		if (_listenToControllerEvents)
		{
			if (!_animatorController.IsInteracting)
			{
				_abilityHandler.LoadAbility(e.AbilityNumber);
			}
		}
		else
		{
			if (!e.FromController)
			{
				if (!_animatorController.IsInteracting)
				{
					_abilityHandler.LoadAbility(e.AbilityNumber);
				}
			}
		}
	}

	private void HandleRunningInputUpdated(object sender, RunEventArgs e)
	{
		if (_listenToControllerEvents)
		{
			if (!e.IsRunning)
			{
				_thirdPersonMovement.RunningTimer = 0;
			}

			IsRunning = e.IsRunning;
		}
		else
		{
			if (!e.FromController)
			{
				if (!e.IsRunning)
				{
					_thirdPersonMovement.RunningTimer = 0;
				}

				IsRunning = e.IsRunning;
			}
		}

	}

	private void HandleRollInputPressed(object sender, GenericInputEvent e)
	{
		if (_listenToControllerEvents)
		{
			if (_stats.CurrentStamina > 0)
			{
				_thirdPersonMovement.HandleRoll();
			}
		}
		else
		{
			if (!e.FromController)
			{
				if (_stats.CurrentStamina > 0)
				{
					_thirdPersonMovement.HandleRoll();
				}
			}
		}

	}

	private void HandleLeftJoystickUpdated(object sender, InputJoystickEventArgs e)
	{
		if (_listenToControllerEvents)
		{
			_thirdPersonMovement.DirectionUpdated(e);
		}
		else
		{
			if (!e.FromController)
			{
				_thirdPersonMovement.DirectionUpdated(e);
			}
		}
	}

	private void HandleCharacterDeathEvent(object sender, EventArgs e)
	{
		CharacterDied?.Invoke(this, new GenericAbstractCharacterManagerEvent() { CharacterManager = this });
	}

	private void HandleAttackInputPressed(object sender, GenericInputEvent e)
	{
		if (_listenToControllerEvents)
		{
			if (_stats.CurrentStamina > 0)
			{
				CharacterAttacker.Attack();
			}
		}
		else
		{
			if (!e.FromController)
			{
				if (_stats.CurrentStamina > 0)
				{
					CharacterAttacker.Attack();
				}
			}
		}
	}

	private void FixedUpdate()
	{
		if (RollFlag)
		{
			//Turn off roll flag when roll is complete
			RollFlag = _animatorController.IsInteracting;
		}
		_thirdPersonMovement.HandleMovement();
		_thirdPersonMovement.HandleFalling();

	}

	private void LateUpdate()
	{
		if (IsInAir)
		{
			_thirdPersonMovement.InAirTimer = _thirdPersonMovement.InAirTimer += Time.deltaTime;
		}
		if (IsRunning)
		{
			if (_stats.CurrentStamina > 0)
			{
				HandleRunningTimer();
			}
			else
			{
				_thirdPersonMovement.InAirTimer = 0;
			}
		}
	}

	private void HandleRunningTimer()
	{
		if (IsLockedOn)
		{
			if (_thirdPersonMovement.CanRunWhileLockedOn())
			{
				_thirdPersonMovement.RunningTimer = _thirdPersonMovement.RunningTimer += Time.deltaTime;
			}
			else
			{
				_thirdPersonMovement.InAirTimer = 0;
			}
		}
		else
		{
			_thirdPersonMovement.RunningTimer = _thirdPersonMovement.RunningTimer += Time.deltaTime;
		}
	}

	private void OnDestroy()
	{
		if (_mlInput != null)
		{
			_mlInput.LeftJoystickUpdated -= HandleLeftJoystickUpdated;
			_mlInput.AttackInputPressed -= HandleAttackInputPressed;
			_mlInput.RollInputPressed -= HandleRollInputPressed;
			_mlInput.RunningInputUpdated -= HandleRunningInputUpdated;
			_mlInput.AbilityInputPressed -= HandleAbilityInputPressed;
			_stats.CharacterDied -= HandleCharacterDeathEvent;
		}

	}


}
