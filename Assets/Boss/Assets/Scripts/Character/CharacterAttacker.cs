
using Animation;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public class CharacterAttacker : MonoBehaviour
	{
		[Header("Weapon Weight Stamina Costs")]
		[SerializeField] float _lightCost = 14f;
		[SerializeField] float _mediumCost = 20f;
		[SerializeField] float _heavyCost = 30f;

		[Header("Dependencies")]
		[SerializeField] AbstractAnimatorController _animatorController;
		[SerializeField] AbstractCharacter _stats;
		[SerializeField] WeaponSlotManager _weaponManager;

		public float AttackStaminaCost
		{
			get
			{
				return _weaponManager.CurrentWeapon.WeaponWeight switch
				{
					Item.WeaponWeight.Light => _lightCost,
					Item.WeaponWeight.Medium => _mediumCost,
					Item.WeaponWeight.Heavy => _heavyCost,
					_ => 0,
				};
			}
		}

		private bool _waitingForCombo;

		private void FixedUpdate()
		{
			if (_waitingForCombo)
			{
				if (!_animatorController.CanDoCombo)
				{
					UseStamina();
					_waitingForCombo = false;
				}
			}
		}

		public void Attack()
		{
			if (!_animatorController.IsInteracting)
			{
				_animatorController.PlayTargetAnimation("Attack", true);
                UseStamina();  
            }
            else
			{
				_animatorController.EnableCombo();
				_waitingForCombo = true;
			}
		}

		private void UseStamina()
		{
            switch (_weaponManager.CurrentWeapon.WeaponWeight)
			{
				case Item.WeaponWeight.Light:
					_stats.UseStamina(_lightCost);
					break;
				case Item.WeaponWeight.Medium:
					_stats.UseStamina(_mediumCost);
					break;
				case Item.WeaponWeight.Heavy:
					_stats.UseStamina(_heavyCost);
					break;
				default:
					break;
			}
		}
	}
}
