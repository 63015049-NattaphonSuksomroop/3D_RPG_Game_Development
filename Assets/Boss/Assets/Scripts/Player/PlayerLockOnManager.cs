
using Character;
using PlayerCamera;
using System;
using UnityEngine;

namespace Player
{
    class LockOnTargetChangedEventArgs : EventArgs
    {
        public Transform newTarget;
    }

    class PlayerLockOnManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] AbstractMovement _thirdPersonMovement;

        [Header("Unlock if Target is Occluded By Environment Controls")]
        [SerializeField] bool _targetOcculdedByEnviromentUnlockTimer;
        [SerializeField] float _targetBehindBlockerThreshold = 3f;


        public bool IsLockedOn { get => _isLockedOn; }

        private bool _isLockedOn;
        private AbstractCharacterManager _currentTarget;
        private ThirdPersonCameraMovement _cameraMovement;
        private bool _targetBehindBlocker;
        private bool _targetBlocked;
        private float _targetBehindBlockerTimer;

        public event EventHandler<LockOnTargetChangedEventArgs> LockOnTargetChanged;

        public void Init(ThirdPersonCameraMovement thirdPersonCameraMovement)
        {
            _cameraMovement = thirdPersonCameraMovement;

            _cameraMovement.NewLockOnTarget += HandleNewLockOnTarget;
            _cameraMovement.RemovedLockOnTarget += HandleRemovedLockOnTarget;
        }

        public void SetLockOnTarget(AbstractCharacterManager target)
        {
            _currentTarget = target;
            _thirdPersonMovement.NewLockOnTarget(_currentTarget.transform);
            _isLockedOn = true;
        }

        private void FixedUpdate()
        {
            if (_isLockedOn)
            {
                if (_currentTarget.IsDead)
                {
                    if (_cameraMovement != null)
                    {
                        _cameraMovement.RemoveLockOn();
                    }
                    else
                    {
                        HandleRemovedLockOnTarget();
                    }

                }
                else
                {
                    if (_targetOcculdedByEnviromentUnlockTimer)
                    {
                        Vector3 lockTargetDirection = _currentTarget.transform.position - _cameraMovement.LockOnCamRaycastTransform.position;
                        float distance = Vector3.Distance(_cameraMovement.LockOnCamRaycastTransform.position, _currentTarget.transform.position);

                        LockOnTargetOccludedByEnvironment(lockTargetDirection, distance);
                    }
                }
            }
        }

        private void LockOnTargetOccludedByEnvironment(Vector3 lockTargetDirection, float distance)
        {
            RaycastHit hit;
            //Check if target is blocked by the environment.
            if (Physics.Raycast(_cameraMovement.LockOnCamRaycastTransform.position, lockTargetDirection, out hit, distance, _cameraMovement.LockOnBlockerMask))
            {
                _cameraMovement.UpdateOverrideTargetPosition(hit.point);
                if (!_targetBlocked)
                {
                    _cameraMovement.LookAtOverrideTarget();
                    _targetBlocked = true;
                }
                _targetBehindBlockerTimer = _targetBehindBlockerTimer += Time.deltaTime;
                if (_targetOcculdedByEnviromentUnlockTimer && _targetBehindBlockerTimer > _targetBehindBlockerThreshold)
                {
                    _cameraMovement.RemoveLockOn();
                }
            }
            else
            {
                if (_targetBlocked)
                {
                    _cameraMovement.UpdateOverrideTargetPosition(_currentTarget.transform.position);
                    _targetBlocked = false;
                    _cameraMovement.ResumeLookingAtTarget();
                    _targetBehindBlockerTimer = 0;
                }
            }
        }

        private void HandleRemovedLockOnTarget(object sender, EventArgs e)
        {
            HandleRemovedLockOnTarget();
        }

        private void HandleRemovedLockOnTarget()
        {
            _thirdPersonMovement.RemoveLockOnTarget();
            _isLockedOn = false;
            _targetBehindBlockerTimer = 0;
            _targetBlocked = false;

            LockOnTargetChanged?.Invoke(this, new LockOnTargetChangedEventArgs { newTarget = null });
        }

        private void HandleNewLockOnTarget(object sender, TargetLockedOnEventArgs e)
        {
            _currentTarget = e.TargetCharacter;
            _thirdPersonMovement.NewLockOnTarget(_currentTarget.transform);
            _isLockedOn = true;

            LockOnTargetChanged?.Invoke(this, new LockOnTargetChangedEventArgs { newTarget = _currentTarget.transform });
        }

        private void OnDestroy()
        {
            if (_cameraMovement != null)
            {
                _cameraMovement.NewLockOnTarget -= HandleNewLockOnTarget;
                _cameraMovement.RemovedLockOnTarget -= HandleRemovedLockOnTarget;
            }
        }
    }
}
