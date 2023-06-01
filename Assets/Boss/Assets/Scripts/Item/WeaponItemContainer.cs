
using Combat;
using UnityEngine;

namespace Item {
	public class WeaponItemContainer : MonoBehaviour {
		[SerializeField]
		DamageCollider _damageCollider;

		[SerializeField]
		ParticleSystem[] _attackVfx;

		public DamageCollider DamageCollider { get => _damageCollider; }
		public ParticleSystem[] AttackVfx { get => _attackVfx;}



		public void PlayAttackVFX()
		{
			for (int i = 0; i < _attackVfx.Length; i++)
			{
				_attackVfx[i].Play();
			}
		}
		public void StopAttackVFX()
		{
			for (int i = 0; i < _attackVfx.Length; i++)
			{
				_attackVfx[i].Stop();
			}	
		}
	}
}
