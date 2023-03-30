
using Character;
using System;
using UnityEngine;

namespace Animation
{
	public abstract class AbstractAnimatorController : MonoBehaviour
	{
		[SerializeField] protected Animator _animator;
		[SerializeField] protected AbstractCharacter _stats;

		[SerializeField] public string RollingAnimatorParameterName = "Rolling";
		[SerializeField] public string BackStepAnimatorParameterName = "BackStep";
		[SerializeField] public string FallingAnimatorParameterName = "Falling";
		[SerializeField] public string LandAnimatorParameterName = "Land";
		[SerializeField] public string EmptyAnimatorParameterName = "Empty";

		public Animator Animator { get => _animator; }
		public bool IsInteracting { get => _animator.GetBool(_interactingAnimatorParameterName); }
		public bool CanDoCombo { get => _animator.GetBool(_comboAnimatorParameterName); }

		private readonly string _interactingAnimatorParameterName = "IsInteracting";
		private readonly string _comboAnimatorParameterName = "CanDoCombo";
		private readonly string _verticalAnimatorParameterName = "Vertical";
		private readonly string _horizontalAnimatorParameterName = "Horizontal";
		private readonly string _deathAnimatorParameterName = "Death";
		private readonly string _takeDamnageParametername = "TakeDamage";

		public void Start()
		{
			_stats.CharacterDied += HandleCharacterDeathEvent;
			_animator.SetBool("IsAlive", true);
		}

		private void HandleCharacterDeathEvent(object sender, EventArgs e)
		{
			PlayTargetAnimation(_deathAnimatorParameterName, true);
			_animator.SetBool("IsAlive", false);
		}

		public void AssignNewWeaponAnimations(AnimatorOverrideController animations)
		{
			_animator.runtimeAnimatorController = animations;
		}

		public void PlayStaggeredAnimation()
		{
			PlayTargetAnimation(_takeDamnageParametername, true);
		}

		public void SetVerticalMovementParameter(Vector3 moveDirection)
		{
			float absX = Mathf.Abs(moveDirection.x);
			float absZ = Mathf.Abs(moveDirection.z);
			if (absX >= absZ)
			{
				_animator.SetFloat(_verticalAnimatorParameterName, absX);
			}
			else
			{
				_animator.SetFloat(_verticalAnimatorParameterName, absZ);
			}
		}
		public void SetVerticalMovementParameter(float verticalValue)
		{
			_animator.SetFloat(_verticalAnimatorParameterName, verticalValue);
		}

		public void SetHorizontalMovementParameter(float horizontalValue)
		{
			_animator.SetFloat(_horizontalAnimatorParameterName, horizontalValue);
		}

		public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
		{
			if (!_stats.IsDead)
			{
				_animator.applyRootMotion = isInteracting;
				_animator.SetBool(_interactingAnimatorParameterName, isInteracting);
				_animator.CrossFade(targetAnimation, 0.2f);
			}
		}

		public void EnableCombo()
		{
			_animator.SetBool(_comboAnimatorParameterName, true);
		}

		protected abstract void OnAnimatorMove();

		internal void Reset()
		{
			_animator.SetBool("IsAlive", true);
		}

		private void OnDestroy()
		{
			_stats.CharacterDied -= HandleCharacterDeathEvent;
		}
	}
}
