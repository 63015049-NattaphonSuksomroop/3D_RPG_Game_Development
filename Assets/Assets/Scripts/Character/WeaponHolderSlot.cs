
using Item;
using UnityEngine;

namespace Character {
	public class WeaponHolderSlot : MonoBehaviour {

		[SerializeField] bool _isLeftHandSlot;

		private GameObject _currentModel;
		public GameObject CurrentModel { get => _currentModel; set => _currentModel = value; }

		public void UnloadWeapon()
		{
			if (_currentModel != null)
			{
				_currentModel.SetActive(false);
			}
		}

		public void UnloadWeaponAndDestory()
		{
			if (_currentModel != null)
			{
				Destroy(_currentModel);
			}
		}

		public WeaponItemContainer LoadWeaponModel(WeaponItem item)
		{
			GameObject weaponGO = null;
			UnloadWeaponAndDestory();
			if (item == null)
			{
				UnloadWeapon();
				return null;
			}

			if (_isLeftHandSlot)
			{
				weaponGO = Instantiate(item.LeftContainer.gameObject, transform, false);
			}
			else
			{
				weaponGO = Instantiate(item.RightContainer.gameObject, transform, false);
			}

			_currentModel = weaponGO;
			return weaponGO.GetComponent<WeaponItemContainer>();
		}

		public void SetOnStartOverrideWeapon(WeaponItem item, GameObject gameObject)
		{
			_currentModel = gameObject;
		}
	}
}
