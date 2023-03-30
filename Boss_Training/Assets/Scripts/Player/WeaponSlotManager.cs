
using Animation;
using Character;
using Combat;
using Item;
using System;
using UnityEngine;

namespace Player {
	public class WeaponSlotManager : MonoBehaviour {

		[SerializeField] AbstractCharacterManager _manager;
		[SerializeField] AbstractAnimatorController _animatorController;
		[SerializeField] WeaponHolderSlot _rightWeaponSlot;
		[SerializeField] WeaponHolderSlot _leftWeaponSlot;

		public WeaponItem CurrentWeapon { get; private set; }
		public WeaponItemContainer RightWeaponContainer { get; private set; }
		public WeaponItemContainer LeftWeaponContainer { get; private set; }
		public WeaponHolderSlot RightWeaponSlot { get => _rightWeaponSlot; }
		public WeaponHolderSlot LeftWeaponSlot { get => _leftWeaponSlot; }
		public int CharaterID { get => _manager.CharacterID; }

		public EventHandler<NewWeaponEquipedEventArgs> NewWeaponEquipedEvent;

		private void Start()
		{
			_manager.CharacterDied += HandleCharacterDeath;
		}

		private void HandleCharacterDeath(object sender, EventArgs e)
		{
			CloseLeftDamageCollider();
			CloseRightDamageCollider();
		}

		public void LoadWeapon(WeaponItem weaponItem)
		{
			CurrentWeapon = weaponItem;
			if (weaponItem.IsWeaponSet)
			{
				RightWeaponContainer = _rightWeaponSlot.LoadWeaponModel(CurrentWeapon);
				LeftWeaponContainer = _leftWeaponSlot.LoadWeaponModel(CurrentWeapon);

				LeftWeaponContainer.DamageCollider.SetUpDamageCollider(weaponItem, _manager);
				RightWeaponContainer.DamageCollider.SetUpDamageCollider(weaponItem, _manager);
				NewWeaponEquipedEvent?.Invoke(this, new NewWeaponEquipedEventArgs()
				{
					RightWeaponItemContainer = RightWeaponContainer, 
					LeftWeaponItemContainer = LeftWeaponContainer
				});
			}
			else
			{
				_leftWeaponSlot.UnloadWeapon();
				RightWeaponContainer = _rightWeaponSlot.LoadWeaponModel(CurrentWeapon);
				RightWeaponContainer.DamageCollider.SetUpDamageCollider(weaponItem, _manager);
				NewWeaponEquipedEvent?.Invoke(this, new NewWeaponEquipedEventArgs()
				{
					RightWeaponItemContainer = RightWeaponContainer,
					LeftWeaponItemContainer = null
				});
			}
		}

		public void OpenLeftDamageCollider()
		{
			if (LeftWeaponContainer != null)
			{
				LeftWeaponContainer.DamageCollider.EnableDamageCollider();
			}
		}

		public void OpenRightDamageCollider()
		{
			if (RightWeaponContainer != null)
			{
				RightWeaponContainer.DamageCollider.EnableDamageCollider();
			}
		}

		public void CloseLeftDamageCollider()
		{
			if (LeftWeaponContainer != null)
			{
				LeftWeaponContainer.DamageCollider.DisableDamageCollider();
			}
		}

		public void CloseRightDamageCollider()
		{
			if (RightWeaponContainer != null)
			{
				RightWeaponContainer.DamageCollider.DisableDamageCollider();
			}
		}
		private void OnDestroy()
		{
			if (_manager != null)
			{
				_manager.CharacterDied -= HandleCharacterDeath;
			}
		}

	}

	public class NewWeaponEquipedEventArgs: EventArgs
	{
		public WeaponItemContainer RightWeaponItemContainer;
		public WeaponItemContainer LeftWeaponItemContainer;
	}
}
