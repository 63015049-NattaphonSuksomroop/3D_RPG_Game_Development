
using Animation;
using Combat;
using Player;
using UnityEngine;

namespace Character
{
	public class CharacterDamageHandler : AbstractDamageable
	{
        [SerializeField] CharacterVfxController _characterVfxController;
        [SerializeField] WeaponSlotManager _weaponSlotManager;
		[SerializeField] AbstractCharacter _stats;
		[SerializeField] AbstractAnimatorController _animatorController;
        [SerializeField] AbstractMovement _movement;

        public override AbstractDamageable TakeDamage(int damage, AbstractDamageSource source)
        {
            if (!InvincibilityFrames)
            {
                _stats.TakeDamage(damage, source);
            }

            return this;
        }

        public override AbstractDamageable TakeKnockback(float knockBackForce, Vector3 direction, AbstractDamageSource source)
        {
            if (!InvincibilityFrames && knockBackForce > float.Epsilon)
            {
                _movement.HandleKnockback(knockBackForce, direction);
            }

            return this;
        }

        public override void TakePercentDamage(float percentDamage)
        {
            if (!InvincibilityFrames)
            {
                _stats.TakePercentDamage(percentDamage);
            }
        }

        public override AbstractDamageable TakePoiseDamage(int poise, AbstractDamageSource source)
        {
            if (!InvincibilityFrames)
            {
                _stats.LosePoise(poise);
                CheckPoise();
            }

            return this;
        }

        private void CheckPoise()
		{
			if (_stats.CurrentPoise < 0)
			{
				_animatorController.PlayStaggeredAnimation();
                _characterVfxController.StopHandCastingVfx();
                _characterVfxController.DisableRightAttackVfx();

            }
		}
	}
}
