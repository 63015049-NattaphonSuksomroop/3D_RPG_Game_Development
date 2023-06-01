
using Item;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
	public class PlayerInventory : MonoBehaviour {

		[SerializeField] WeaponSlotManager _weaponSlotManager;
        [SerializeField] List<WeaponItem> _weapons;


		public List<WeaponItem> Weapons { get => _weapons;}

		private void Start()
        {
			_weaponSlotManager.LoadWeapon(_weapons[0]);
        }

		internal void EquipWeapon(WeaponItem weaponItem)
		{
			_weaponSlotManager.LoadWeapon(weaponItem);
		}
	}
}
