
using Combat;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Character
{
	public abstract class AbstractDamageable : MonoBehaviour
	{
		public ImpactType ImpactType;
		public int CharacterID { get;  set; }

		public TeamID TeamID { get; set; }

		public bool InvincibilityFrames { get; protected set; }

		public abstract AbstractDamageable TakeDamage(int damage, AbstractDamageSource source);

		public abstract AbstractDamageable TakePoiseDamage(int poise, AbstractDamageSource source);

		public abstract AbstractDamageable TakeKnockback(float knockBackForce, Vector3 direction, AbstractDamageSource source);

		public abstract void TakePercentDamage(float percentDamage);


		public void EnableIFrames()
		{
			InvincibilityFrames = true;
		}

		public void DisableIFrames()
		{
			InvincibilityFrames = false;
		}

		public bool CanDamage()
        {
			return !InvincibilityFrames;
        }			
	}

	public enum ImpactType { Wood, stone, dirt, metal }
}
