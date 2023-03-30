
using Ability;
using Combat;
using UnityEngine;

namespace Ability
{
    public class AbilityFireBall : AbstractAbility
    {
        [SerializeField] FireBallProjectile _fireBallPrefab;


        private Transform _castingTransform;
        private Transform _characterTransform;

        public override GameObject FireAbility()
        {
            if (HasResourcesToCastAbility())
            {
                FireBallProjectile fireball = Instantiate(_fireBallPrefab, _castingTransform.transform.position, 
                    _characterTransform.transform.rotation);
                Vector3 direction = Vector3.zero;
                direction = _characterManager.RangedAttackTarget == null ? _characterTransform.transform.forward : 
                        (_characterManager.RangedAttackTarget.position - _characterManager.transform.position).normalized;
                fireball.CastSpell(_characterManager, direction);
                SpendResource();
                return fireball.gameObject;
            }
            return null;
        }

        public override void LoadAbility()
        {
            if (!_characterManager.AnimatorController.IsInteracting)
            {
                _characterManager.CharacterVfxController.LoadFxAffix(AffixType.Fire);
                _characterManager.AnimatorController.PlayTargetAnimation(AnimationName, true);
            }
        }

        public override void SetUpAblity()
        {
            _castingTransform = _characterManager.WeaponSlotManager.RightWeaponSlot.transform;
            _characterTransform = _characterManager.transform;
        }
    }
}
