
using Character;
using System.Collections.Generic;
using UnityEngine;

namespace Ability
{
    public class FireBallProjectile : AbstractProjectileDamageAbility
    {
        [Header("Fire Ball Parameters")]
        [SerializeField] float _impactRadius;
        [SerializeField] LayerMask _impactMask;
        [SerializeField] int _damage;
        [SerializeField] int _poiseDamage;
        [SerializeField] int _splashDamage;
        [SerializeField] int _splashPoise;
        [SerializeField] AbstractAnimationEvaluatiorBehaviour _lightAnimation;
		[Header("Fire Ball VFX")]
		[SerializeField] protected ParticleSystem _inAirParticle;
		[SerializeField] protected ParticleSystem _impactPartible;
		[Header("FireballSound")]
		[SerializeField] protected AudioSource _audioSource;
		[SerializeField] AudioClip _ImpactSound;
        [SerializeField] float _audioSourceMaxDistanceOnImpact;

		protected override void OnCastSpell()
		{
			_inAirParticle.Play();
			_audioSource.Play();
		}

		protected override void OnTriggerEnter(Collider collision)
		{
			AbstractDamageable damageable = collision.GetComponent<AbstractDamageable>();
			if (damageable == null)
			{
				//Dont impact anything on the ignore layer
				if ((_ignoreLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
				{
					return;
				}
			}
			else
			{
				if (CharacterID == damageable.CharacterID || CharacterTeamID == damageable.TeamID)
				{
					return;
				}
				else
				{
					damageable.TakeDamage(_damage, this).TakePoiseDamage(_poiseDamage, this);
				}
			}

			_audioSource.Stop();
			_audioSource.maxDistance = _audioSourceMaxDistanceOnImpact;
			if (_ImpactSound != null)
				_audioSource.PlayOneShot(_ImpactSound);

			_inAirParticle.Stop();
			_impactPartible.Play();
			_lightAnimation.Play();
			_collider.enabled = false;

			Collider[] colliders = Physics.OverlapSphere(this.transform.position, _impactRadius, _impactMask);
			for (int i = 0; i < colliders.Length; i++)
			{
				damageable = colliders[i].GetComponent<AbstractDamageable>();
				//Dont lock onto the casting character, or dead NPC.
				if (damageable != null && CharacterID != damageable.CharacterID && CharacterTeamID != damageable.TeamID)
				{
					damageable.TakeDamage(_splashDamage, this).TakePoiseDamage(_splashPoise, this);
				}
			}

			StartDestoryObjectCountDown();
		}
	}
}
