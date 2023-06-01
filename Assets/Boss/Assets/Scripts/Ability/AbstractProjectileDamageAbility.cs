
using Character;
using Combat;
using System.Collections;
using UnityEngine;

namespace Ability
{
	public abstract class AbstractProjectileDamageAbility : AbstractDamageSource
	{
		[Header ("Abstract Projectile Damage Ability Parameters")]
		[SerializeField] protected Collider _collider;
		[SerializeField] protected float _projectileForwardVelocity;
		[SerializeField] protected float _projectileUpwardVelocity;
		[SerializeField] protected Rigidbody _rigidbody;
		[SerializeField] protected LayerMask _ignoreLayer;
		[SerializeField] protected float _destoryObjectTimer = 3f;


		private Coroutine _Coroutine;

		public void CastSpell(AbstractCharacterManager owner, Vector3 direction)
		{
			Owner = owner;
			OnCastSpell();
			_rigidbody.AddForce(direction * _projectileForwardVelocity);
			_rigidbody.AddForce(transform.up * _projectileUpwardVelocity);
		}


		protected abstract void OnCastSpell();

		protected abstract void OnTriggerEnter(Collider collision);

		protected void  StartDestoryObjectCountDown()
		{
			StartCoroutine(TimerCoroutine());
		}

		IEnumerator TimerCoroutine()
		{
			yield return new WaitForSeconds (_destoryObjectTimer);
			Destroy(this.gameObject);
		}
	}
}
