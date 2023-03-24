
using System;
using UnityEngine;

namespace Character
{
	public abstract class AbstractMovement : MonoBehaviour
	{
		protected float _turnSmoothVelocity;

		[Header("Movement Stats")]
		[SerializeField] protected float _speed = 6.0f;
		[SerializeField] protected float _angularSpeed = 120;
		[SerializeField] protected float _stoppingDistance = 1;
		[SerializeField] protected float _turnSmoothTime = 0.1f;
		[SerializeField] protected float _stopStopSmoothTime = 0.05f;
		[SerializeField] protected float _rollStaminaCost = 16f;

		[Header("Ground and Air Detection")]
		[SerializeField] protected float _minDistanceNeededToBeginFall = 1f;
		[SerializeField] protected LayerMask _layerMask;
		[SerializeField] protected Transform _fallingRay;

		[Header("Running Stats")]
		[SerializeField] protected float _runningSpeed = 9.0f;
		[SerializeField] protected float _runAccelerationTime = 1.0f;
		[SerializeField] protected float _runningStaminaCost = 1.0f;
		[SerializeField] protected float _lockOnRunningInputZThreshold = 0.75f;
		[SerializeField] protected float _lockOnRunningXInputMax = 0.35f;
		[SerializeField] protected float _lockOnRunningXInputMin = -0.35f;

		protected float _inAirTimer;

		public abstract void HandleFalling();
		public abstract void HandleKnockback(float force, Vector3 direction);
		internal abstract void NewLockOnTarget(Transform transform);
		internal abstract void RemoveLockOnTarget();

		public class LandedEventArgs : EventArgs
		{
			public float AirTime;
		}
	}
}