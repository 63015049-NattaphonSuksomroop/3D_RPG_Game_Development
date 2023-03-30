
using Character;
using Cinemachine;
using Player;
using PlayerInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerCamera
{
    public class ThirdPersonCameraMovement : SingletonBehaviour<ThirdPersonCameraMovement>
    {
        [SerializeField] Camera _camera;
        [SerializeField] CinemachineFreeLook _cinemachineFreeLook;
        [SerializeField] float TouchSensitivity_x = 4f;
        [SerializeField] float TouchSensitivity_y = 4f;
        [SerializeField] bool _invertRightStick = true;

        [Header("Lock On")]
        [SerializeField] CinemachineTargetGroup _targetGroup;
        [SerializeField] CinemachineVirtualCamera _virtualCamera;
        [SerializeField] LayerMask _lockOnBlockerMask;
        [SerializeField] Transform _overrideTarget;
        [SerializeField] Transform _lockOnCamRaycastTransform;

        public bool InvertRightStick { get => _invertRightStick; set => _invertRightStick = value; }
        public LayerMask LockOnBlockerMask { get => _lockOnBlockerMask; set => _lockOnBlockerMask = value; }
        public Camera Camera { get => _camera; }
        public CinemachineVirtualCamera VirtualCamera { get => _virtualCamera; }
        public Transform LockOnCamRaycastTransform { get => _lockOnCamRaycastTransform; }
        private Vector3 _activePlayerPostion { get => _activePlayer.transform.position; }

        private PlayerInputController _input
        {
            get
            {
                return PlayerInputController.Instance;
            }
        }

        private float _xValue;
        private float _yValue;
        private AbstractCharacterManager _lockOnTarget;
        private AbstractCharacterManager _activePlayer;

        public EventHandler<TargetLockedOnEventArgs> NewLockOnTarget;
        public EventHandler<EventArgs> RemovedLockOnTarget;
        private bool _recentlyLockedOnLeft;
        private bool _recentlyLockedOnRight;
        private bool _recentlyLockedOnAbove;
        private bool _recentlylockedOnBelow;
        private WaitForEndOfFrame wait = new WaitForEndOfFrame();


		private void enableLockOnCamera(bool enable)
        {
            _cinemachineFreeLook.Priority = enable ? 0 : 10;
            _virtualCamera.Priority = enable ? 10 : 0;
        }

        public override void Awake()
        {
            base.Awake();

            _input.RightJoystickUpdate += HandleOrbitUpdated;
            _input.LockOnInputPressed += HandleLockOnPressedEvent;
        }

        public void OverrideEvents(PlayerInputController inputController, AbstractCharacterManager player)
		{
			if (_input != null)
			{
                _input.RightJoystickUpdate -= HandleOrbitUpdated;
                _input.LockOnInputPressed -= HandleLockOnPressedEvent;
            }

            inputController.RightJoystickUpdate += HandleOrbitUpdated;
            inputController.LockOnInputPressed += HandleLockOnPressedEvent;

            HandleNewPlayerEvent(player);
        }


        private void HandleLockOnPressedEvent(object sender, EventArgs e)
        {
            HandleLockOn();
        }

        void Start()
        {
            CinemachineCore.GetInputAxis = HandleAxisInputDelegate;
        }

        private void HandleNewPlayerEvent( AbstractCharacterManager player)
        {
            _cinemachineFreeLook.enabled = true;
            _virtualCamera.enabled = true;
            enableLockOnCamera(false);

            _activePlayer = player;
            _cinemachineFreeLook.Follow = _activePlayer.LockOnTransform;
            _cinemachineFreeLook.LookAt = _activePlayer.LockOnTransform;

            Vector3 relativePos = player.transform.position - _camera.transform.position;
        }


        private void HandleOrbitUpdated(object sender, InputJoystickEventArgs e)
        {
            _xValue = e.InputVector.x;
            _yValue = e.InputVector.y;
        }

        private float HandleAxisInputDelegate(string axisName)
        {
            switch (axisName)
            {
                case "Mouse X":
                    return   -_xValue / TouchSensitivity_x;
                case "Mouse Y":
                    return _invertRightStick ? -_yValue / TouchSensitivity_y : _yValue / TouchSensitivity_y;
                default:
                    Debug.LogError("Input <" + axisName + "> not recognized.", this);
                    break;
            }

            return 0f;
        }

        public void HandleLockOn()
        {
            if (_lockOnTarget == null)
            {
                LockOnToNearest();
            }
            else
            {
                RemoveLockOn();
            }
        }

        public void RemoveLockOn()
        {
            _lockOnTarget.SetTargetLock(false);
            _lockOnTarget = null;
            RemovedLockOnTarget?.Invoke(this, EventArgs.Empty);

            enableLockOnCamera(false);
        }
        
        private void LockOnToNearest()
        {
            LookAtTarget();
        }

        public void SetNewLockOnTarget(AbstractCharacterManager target)
        {
            if (_lockOnTarget != null)
            {
                _lockOnTarget.SetTargetLock(false);
            }

            _lockOnTarget = target;
            _lockOnTarget.SetTargetLock(true);
            NewLockOnTarget?.Invoke(this, new TargetLockedOnEventArgs() { TargetCharacter = _lockOnTarget });
            LookAtTarget();
        }

        public void UpdateOverrideTargetPosition(Vector3 postion)
        {
            _overrideTarget.position = postion;
        }

        public void LookAtOverrideTarget()
        {
            if (_lockOnTarget != null)
            {
                _targetGroup.m_Targets[0].target = _activePlayer.LockOnTransform;
                _targetGroup.m_Targets[1].target = _overrideTarget;

                _virtualCamera.Follow = _activePlayer.LockOnTransform;
                _virtualCamera.LookAt = _overrideTarget;
            }
        }

        public void ResumeLookingAtTarget()
        {
            if (_lockOnTarget != null)
            {
                _targetGroup.m_Targets[0].target = _activePlayer.LockOnTransform;
                _targetGroup.m_Targets[1].target = _lockOnTarget.LockOnTransform;

                _virtualCamera.Follow = _activePlayer.LockOnTransform;
                _virtualCamera.LookAt = _lockOnTarget.LockOnTransform;
            }
        }

        private void LookAtTarget()
        {
            if (_lockOnTarget != null)
            {
                _targetGroup.m_Targets[0].target = _activePlayer.LockOnTransform;
                _targetGroup.m_Targets[1].target = _lockOnTarget.LockOnTransform;

                _virtualCamera.LookAt = _lockOnTarget.LockOnTransform;
                _virtualCamera.Follow = _activePlayer.LockOnTransform;
                enableLockOnCamera(true);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (_input != null)
            {
                _input.RightJoystickUpdate -= HandleOrbitUpdated;
            }
        }
    }

    public class TargetLockedOnEventArgs : EventArgs
    {
        public AbstractCharacterManager TargetCharacter;
    }
}
