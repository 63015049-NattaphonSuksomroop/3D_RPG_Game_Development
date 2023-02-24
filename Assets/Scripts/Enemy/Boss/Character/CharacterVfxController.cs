
using Combat;
using Fx;
using Player;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public class CharacterVfxController : MonoBehaviour
	{
		[SerializeField] WeaponSlotManager _weaponSlotManager;
		[SerializeField] Transform _lowerVfxTransform;
		[SerializeField] Transform _leftFootVFXTransform;
		[SerializeField] Transform _rightFootVFXTransform;

		private Transform _rightHandTransfrom;
		private Transform _leftHandTransform;
		private ParticleSystem _rightCastingParticles;
		private ParticleSystem _leftCastingParticles;

		private ParticleEffectController[] _fireCastingHandVfx;

		private List<ParticleEffectController> _woodImpactVfxs;
		private List<ParticleEffectController> _stoneImpactVfxs;
		private List<ParticleEffectController> _dirtImpactVfxs;
		private List<ParticleEffectController> _metalImpactVfxs;
		private ParticleEffectController _rollVfx;
		private ParticleEffectController _leftWalkingVFX;
		private ParticleEffectController _rightWalkingVFX;
		private AffixType _LoadedFxAffix;
		private VfxService _vfxService;

		private void Awake()
		{
			_vfxService = VfxService.Instance;
			_rightHandTransfrom = _weaponSlotManager.RightWeaponSlot.transform;
			_leftHandTransform = _weaponSlotManager.LeftWeaponSlot.transform;
			_woodImpactVfxs = new List<ParticleEffectController>();
			_stoneImpactVfxs = new List<ParticleEffectController>();
			_dirtImpactVfxs = new List<ParticleEffectController>();
			_metalImpactVfxs = new List<ParticleEffectController>();
		}

		public void PlayRollVfx()
		{
			_rollVfx?.Play();
		}

		public void LoadFxAffix(AffixType affixType)
		{
			_LoadedFxAffix = affixType;
		}

		public void PlayLeftStep()
		{
			_leftWalkingVFX?.Play();
		}

		public void PlayRightStep()
		{
			_rightWalkingVFX?.Play();
		}

		public void PlayImpactEffectAtPoint(ImpactType impactType, Vector3 worldPosition)
		{
			switch (impactType)
			{
				case ImpactType.Wood:
					PlayEffectInList(worldPosition, _woodImpactVfxs, _vfxService.CharacterFxSO.WoodVfxPrefab);
					break;
				case ImpactType.stone:
					PlayEffectInList(worldPosition, _stoneImpactVfxs, _vfxService.CharacterFxSO.StoneVfxPrefab);
					break;
				case ImpactType.dirt:
					PlayEffectInList(worldPosition, _dirtImpactVfxs, _vfxService.CharacterFxSO.DirtVfxPrefab);
					break;
				case ImpactType.metal:
					PlayEffectInList(worldPosition, _metalImpactVfxs, _vfxService.CharacterFxSO.MetalVfxPrefab);
					break;
				default:
					break;
			}
		}

		private void PlayEffectInList(Vector3 worldPosition, List<ParticleEffectController> effectList, ParticleEffectController effectPrefab)
		{

			ParticleEffectController particleEffect;
			Quaternion rotionation = Quaternion.Euler(Vector3.zero);
			if (effectList.Count <= 0)
			{
				particleEffect = InstantiateParticleForList(worldPosition, effectList, effectPrefab, rotionation);

			}
			else
			{
				for (int i = 0; i < effectList.Count; i++)
				{
					ParticleEffectController effect = effectList[i];
					if (!effect.IsAlive())
					{
						effect.transform.position = worldPosition;
						effect.Play();
						return;
					}
				}
				InstantiateParticleForList(worldPosition, effectList, effectPrefab, rotionation);
			}
		}

		private static ParticleEffectController InstantiateParticleForList(Vector3 worldPosition, List<ParticleEffectController> effectList, ParticleEffectController effectPrefab, Quaternion rotionation)
		{
			ParticleEffectController particleEffect = Instantiate(effectPrefab, worldPosition, rotionation);
			effectList.Add(particleEffect);
			particleEffect.Play();
			return particleEffect;
		}

		public void StartHandCastingVfx()
		{
			switch (_LoadedFxAffix)
			{
				case AffixType.Physical:
					break;
				case AffixType.Fire:
					StartFireCastingHandsVFX();
					break;
				default:
					break;
			}
		}

		public void StopHandCastingVfx()
		{
			switch (_LoadedFxAffix)
			{
				case AffixType.Physical:
					break;
				case AffixType.Fire:
					StopFireCastingHandsVFX();
					break;
				default:
					break;
			}
		}

		private void StartFireCastingHandsVFX()
		{
			if (_fireCastingHandVfx == null)
			{
				_fireCastingHandVfx = SetUpCastHandVFX(_vfxService.CharacterFxSO.FireHandVfxPrefab);
			}
			for (int i = 0; i < _fireCastingHandVfx.Length; i++)
			{
				_fireCastingHandVfx[i].Play();
			}
		}

		private ParticleEffectController[] SetUpCastHandVFX(ParticleEffectController handCastingVfx)
		{
			ParticleEffectController left = Instantiate(handCastingVfx, _leftHandTransform);
			ParticleEffectController right = Instantiate(handCastingVfx, _rightHandTransfrom);
			ParticleEffectController[] handFxArray = new ParticleEffectController[] { left, right };
			return handFxArray;
		}

		private void StopFireCastingHandsVFX()
		{
			for (int i = 0; i < _fireCastingHandVfx.Length; i++)
			{
				_fireCastingHandVfx[i].Stop();
			}
		}

		public void EnableRightAttackVfx()
		{
			if (_weaponSlotManager.RightWeaponContainer != null)
			{
				_weaponSlotManager.RightWeaponContainer.PlayAttackVFX();
			}
		}

		public void DisableRightAttackVfx()
		{
			if (_weaponSlotManager.RightWeaponContainer != null)
			{
				_weaponSlotManager.RightWeaponContainer.StopAttackVFX();
			}
		}

		private void OnDestroy()
		{
			DestoryObjectInVfxList(_woodImpactVfxs);
			DestoryObjectInVfxList(_stoneImpactVfxs);
			DestoryObjectInVfxList(_dirtImpactVfxs);
			DestoryObjectInVfxList(_metalImpactVfxs);
		}

		private void DestoryObjectInVfxList(List<ParticleEffectController> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != null)
				{
					Destroy(list[i].gameObject);

				}
			}
		}
	}

}
