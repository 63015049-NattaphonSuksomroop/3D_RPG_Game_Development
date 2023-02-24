
using System;
using Character;
using PlayerCamera;
using PlayerInput;
using UnityEngine;

namespace Player
{
    public class PlayerManager : AbstractCharacterManager
    {

        [Header("Player")]

        [SerializeField] internal ThirdPersonMovement _thirdPersonMovement;
        [SerializeField] internal PlayerLockOnManager _playerLockOnManager;

        public bool IsLockedOn { get => _playerLockOnManager.IsLockedOn; }

        public ThirdPersonCameraMovement CameraMovement { get; protected set; }

        public void Init(Transform cameraTransform, ThirdPersonCameraMovement thirdPersonCameraMovement)
        {
            CameraMovement = thirdPersonCameraMovement;
            _playerLockOnManager.Init(thirdPersonCameraMovement);


            _thirdPersonMovement.Init(cameraTransform);

            _abilityHandler.Init(CharacterID);

            _stats.CharacterDied += HandleCharacterDeathEvent;

            _playerLockOnManager.LockOnTargetChanged += _playerLockOnManager_LockOnTargetChanged;
        }

        private void _playerLockOnManager_LockOnTargetChanged(object sender, LockOnTargetChangedEventArgs e)
        {
            RangedAttackTarget = e.newTarget;
        }

        [ContextMenu("Rest Player")]
        public void ResetPlayer()
        {
            Stats.SetStartingStats();
            AnimatorController.Reset();
        }

        private void HandleAbilityInputPressed(object sender, AbilityButtonPressedEventArgs e)
        {
            if (!_animatorController.IsInteracting)
            {
                _abilityHandler.LoadAbility(e.AbilityNumber);
            }
        }

        private void HandleRunningInputUpdated(object sender, RunEventArgs e)
        {
            if (!e.IsRunning)
            {
                _thirdPersonMovement.RunningTimer = 0;
            }

            IsRunning = e.IsRunning;
        }

        private void HandleRollInputPressed(object sender, EventArgs e)
        {
            if (_stats.CurrentStamina > 0)
            {
                _thirdPersonMovement.HandleRoll();
            }
        }

        private void HandleLeftJoystickUpdated(object sender, InputJoystickEventArgs e)
        {
            _thirdPersonMovement.DirectionUpdated(e);
        }

        private void HandleCharacterDeathEvent(object sender, EventArgs e)
        {
            CharacterDied?.Invoke(this, new GenericAbstractCharacterManagerEvent() { CharacterManager = this });
        }

        private void HandleAttackInputPressed(object sender, EventArgs e)
        {
            if (_stats.CurrentStamina > 0)
            {
                CharacterAttacker.Attack();
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
            _stats.CharacterDied -= HandleCharacterDeathEvent;

            if (_playerLockOnManager != null)
            {
                _playerLockOnManager.LockOnTargetChanged -= _playerLockOnManager_LockOnTargetChanged;
            }
        }
    }
}
