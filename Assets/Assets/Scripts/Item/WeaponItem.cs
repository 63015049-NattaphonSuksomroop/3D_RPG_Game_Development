
using UnityEngine;

namespace Item {
	[CreateAssetMenu(menuName = "Items/Weapon Item")]
	public class WeaponItem : Item {

		[SerializeField] bool _isWeaponSet;
		[SerializeField] WeaponItemContainer _rightContainer;
		[SerializeField] WeaponItemContainer _leftContainer;
		[SerializeField] WeaponWeight _weaponWeight;
		[SerializeField] bool _isUnarmed;
		[SerializeField] int _damage;
		[SerializeField] int _poiseDamage;
		[SerializeField] float _knockbackForce;
		[SerializeField] float _range = 1.5f;

		[Header("Animations")]
		[SerializeField] AnimatorOverrideController _animations;

		[Header("Sounds")]
		[SerializeField] AudioClip _comboOneWoosh;
		[SerializeField] AudioClip _comboTwoWoosh;
		[SerializeField] AudioClip _comboThreeWoosh;

		public WeaponItemContainer RightContainer { get => _rightContainer; }
		public WeaponItemContainer LeftContainer { get => _leftContainer;}
		public bool IsUnarmed { get => _isUnarmed;}
		public int Damage { get => _damage; }
		public int PoiseDamage { get => _poiseDamage; }
		public float KnockbackForce { get => _knockbackForce; }
		public float Range { get => _range; }
		public bool IsWeaponSet { get => _isWeaponSet;}
		public WeaponWeight WeaponWeight { get => _weaponWeight;}
		public AnimatorOverrideController Animations { get => _animations; }
		public AudioClip ComboOneWoosh { get => _comboOneWoosh; }
		public AudioClip ComboTwoWoosh { get => _comboTwoWoosh; }
		public AudioClip ComboThreeWoosh { get => _comboThreeWoosh; }
	}

	public enum WeaponWeight
	{
		Light, Medium, Heavy
	}
}
