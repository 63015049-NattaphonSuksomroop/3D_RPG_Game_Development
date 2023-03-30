
using Animation;
using Character;
using PlayerInput;
using UnityEngine;

namespace Player {
	public class ThirdPersonMovement : AbstractMovement
	{

        [Header("Slope Detection")]
        [SerializeField] float _slopeForce;

		[Header ("Dependancies")]
		[SerializeField] AbstractAnimatorController _animatorController;
		[SerializeField] CharacterController _characterController;
		[SerializeField] AbstractCharacterManager _playerManager;
		[SerializeField] AbstractCharacter _stats;

        private Transform _cameraTransform;
		private Vector3 _inputDirection;
        private bool _isLockedOn;
        private Transform _lockedOnTransform;    
		private float _runningTimer;
		private float _currentRunningSpeed { get => Mathf.Lerp(_speed, _runningSpeed, _runningTimer * _runAccelerationTime);}
		private float _currentRunningAnimationParameterValue { get => Mathf.Lerp(1, 2, _runningTimer * _runAccelerationTime); }

		public float InAirTimer { get => _inAirTimer; set => _inAirTimer = value; }
		public bool IsRunning { get => _playerManager.IsRunning; }
		public float RunningTimer { get => _runningTimer; set => _runningTimer = value; }

		internal void Init(Transform cameraTransform)
		{
			_cameraTransform = cameraTransform;
            _playerManager.IsGrounded = true;
        }

		public void HandleRoll()
		{
			if (_animatorController.IsInteracting)
			{
				return;
			}

			Vector3 moveDirection = GetFreeMovementDirection(false);

			if (_inputDirection.magnitude >= 0.1f)
			{
				_animatorController.PlayTargetAnimation(_animatorController.RollingAnimatorParameterName, true);
				moveDirection.y = 0;
				Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
				_playerManager.transform.rotation = rollRotation;
				_playerManager.RollFlag = true;
			}
			else
			{
                _animatorController.PlayTargetAnimation(_animatorController.BackStepAnimatorParameterName, true);
                _playerManager.RollFlag = true;
            }
			_stats.UseStamina(_rollStaminaCost);
		}

		public override void HandleKnockback(float force, Vector3 direction)
        {
			_characterController.SimpleMove(force * direction);
        }

		public void DirectionUpdated( InputJoystickEventArgs e)
		{
			_inputDirection = new Vector3(e.InputVector.x, 0f, e.InputVector.y).normalized;
			if (e.InputEngaged == false)
			{
				_animatorController.SetVerticalMovementParameter(Vector3.zero);
                _animatorController.SetHorizontalMovementParameter(0f);
			}
		}

		public void StopVelocity()
		{
            _characterController.Move(Vector3.zero);
		}

		public void HandleMovement()
		{
			if (_animatorController.IsInteracting)
			{
				return;
			}

			if (_inputDirection.magnitude >= 0.1f)
			{
                if (_isLockedOn)
                {
                    HandleLockedOnRotation();
                }
                Move();
			}
			else
			{
				if (!_animatorController.IsInteracting)
				{
                    _characterController.SimpleMove(Vector3.zero);
                    if (_isLockedOn)
                    {
                        HandleLockedOnRotation();
                    }
                }
			}
		}

        private void Move()
		{
			Vector3 velocity;
			Vector3 moveDirection;

			if (_isLockedOn)
            {
				//Handle Locked on movement
				float targetAngle = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg + gameObject.transform.eulerAngles.y ;
                //Turn target angle into a direction. 
                moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

				if (CanRunWhileLockedOn() && _stats.CurrentStamina > 0)
				{
					velocity = moveDirection * _currentRunningSpeed;
					_animatorController.SetVerticalMovementParameter(_currentRunningAnimationParameterValue);
					_stats.UseStamina(_runningStaminaCost * Time.deltaTime);
				}
				else
				{
					velocity = moveDirection * _speed;
					_animatorController.SetVerticalMovementParameter(_inputDirection.z);
				}
                _characterController.SimpleMove(velocity);
				_animatorController.SetHorizontalMovementParameter(_inputDirection.x);
            }
            else
            {
                moveDirection = GetFreeMovementDirection(true);

				if (_playerManager.IsRunning && _stats.CurrentStamina > 0)
				{
					velocity = moveDirection * _currentRunningSpeed;
					_animatorController.SetVerticalMovementParameter(_currentRunningAnimationParameterValue);
					_stats.UseStamina(_runningStaminaCost);
				}
				else
				{
					velocity = moveDirection * _speed;
					_animatorController.SetVerticalMovementParameter(moveDirection.normalized);
				}

                _characterController.SimpleMove(velocity);      
            }
        }

		public bool CanRunWhileLockedOn()
		{
			if (_isLockedOn && _playerManager.IsRunning && _inputDirection.x < _lockOnRunningXInputMax && _inputDirection.x > _lockOnRunningXInputMin)
			{
				return _inputDirection.z > _lockOnRunningInputZThreshold;
			}
			else
			{

				return false;
			}
		}

        public void HandleLockedOnRotation()
        {

            Vector3 lookAtPosition = new Vector3(_lockedOnTransform.position.x, this.transform.position.y, _lockedOnTransform.position.z);
            transform.LookAt(lookAtPosition);
        }

        private Vector3 GetFreeMovementDirection(bool rotateCharacter)
		{
			//Camera is used to keep the player moving in the direction the camera is pointed when it rotates.
			float targetAngle = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg + gameObject.transform.eulerAngles.y;
            //Smooth character rotation
            if (rotateCharacter)
            {
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
			    transform.rotation = Quaternion.Euler(0f, angle, 0f);

            }

			//Turn target angle into a direction. 
			Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
			return moveDirection;
		}

		private float computeFallDamage(float fallTime, float damageMultiplier)
		{
			return fallTime * damageMultiplier;
		}

		public override void HandleFalling()
        {
		}

        internal override void  NewLockOnTarget(Transform transform)
        {
            _isLockedOn = true;
            _lockedOnTransform = transform;
        }

        internal override void RemoveLockOnTarget()
        {
            _isLockedOn = false;
            _animatorController.SetHorizontalMovementParameter(0f);
            _lockedOnTransform = null;
        }
    }
}
