
using Character;
using Joystick;
using Player;
using PlayerCamera;
using PlayerInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
	public class GameTimeUiManager : MonoBehaviour
	{
		[Header("Ui Elements")]
		[SerializeField] AttackButton _attackButton;
		[SerializeField] RollButton _rollButton;
		[SerializeField] AbilityButton _abilityButtonTwo;
		[SerializeField] AbstractJoystick _right;
		[SerializeField] AbstractJoystick _left;
		[SerializeField] Canvas _mobileUiCanvas;

        [SerializeField] private PlayerInputController _inputController;
        [SerializeField] private MlPlayerManager _player;

		private bool _isJoystickConnected = false;

		private void Start()
		{
			_inputController.SetUpOnScreenJoysticks(_left, _right);

			_attackButton.AssignDependencies(_player, _inputController);
			_rollButton.AssignDependencies(_player, _inputController);
			_abilityButtonTwo.AssignDependencies(_player, _inputController);
		}

        private void Update()
        {
			bool isJoystickConnected = _inputController.IsJoystickConnected();
			if (isJoystickConnected && !_isJoystickConnected)
            {
				_mobileUiCanvas.enabled = false;
            } 
			else if (!isJoystickConnected && _isJoystickConnected)
            {
				_mobileUiCanvas.enabled = true;
            }

			_isJoystickConnected = isJoystickConnected;
		}
	}
}
