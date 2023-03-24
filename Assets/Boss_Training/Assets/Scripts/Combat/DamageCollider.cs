
using Character;
using Item;
using System;
using UnityEngine;

namespace Combat {
	public class DamageCollider : AbstractDamageSource
	{
		[SerializeField] Collider _collider;
		public int Damage { get; set; }
		public int PoiseDamage { get; set; }
		public float KnockbackForce { get; set; }

		public EventHandler<DamageSourceCollisionEventArgs> DamageSourceCollisionEvent;
		private TeamID TeamID { get => Owner.TeamID; }

		private void Awake()
		{
			_collider.gameObject.SetActive(false);
			_collider.isTrigger = true;
			_collider.enabled = false;
		}

		public void SetUpDamageCollider(WeaponItem weaponItem, AbstractCharacterManager owner)
		{
			Damage = weaponItem.Damage;
			PoiseDamage = weaponItem.PoiseDamage;
			KnockbackForce = weaponItem.KnockbackForce;
			Owner = owner;
		}


		public void EnableDamageCollider()
		{
			_collider.enabled = true;
            _collider.gameObject.SetActive(true);
        }

		public void DisableDamageCollider()
		{
            _collider.enabled = false;
            _collider.gameObject.SetActive(false);
        }

		private void OnTriggerEnter(Collider collision)
		{
			var damageable = collision.GetComponent<AbstractDamageable>();
			if (damageable == null || TeamID == damageable.TeamID)
			{
				return;
			}

			damageable.TakeDamage(Damage, this)
				.TakeKnockback(KnockbackForce, -collision.transform.forward, this)
				.TakePoiseDamage(PoiseDamage, this);

            _collider.enabled = false;
            _collider.gameObject.SetActive(false);

            DamageSourceCollisionEvent?.Invoke(this, new DamageSourceCollisionEventArgs()
			{
				HittingCollider = _collider,
				Damageable = damageable,
				ColliderHit = collision
			});
		}
	}
}
