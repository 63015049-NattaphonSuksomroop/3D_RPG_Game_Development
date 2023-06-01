
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fx
{
	[CreateAssetMenu(menuName = "VFX SO/Character Vfx SO")]
	public class CharacterFxSO : ScriptableObject
	{
		[SerializeField] ParticleEffectController _fireHandVfxPrefab;
		[SerializeField] ParticleEffectController _woodVfxPrefab;
		[SerializeField] ParticleEffectController _stoneVfxPrefab;
		[SerializeField] ParticleEffectController _dirtVfxPrefab;
		[SerializeField] ParticleEffectController _metalVfxPrefab;

		public ParticleEffectController FireHandVfxPrefab { get => _fireHandVfxPrefab; }
		public ParticleEffectController WoodVfxPrefab { get => _woodVfxPrefab; }
		public ParticleEffectController StoneVfxPrefab { get => _stoneVfxPrefab; }
		public ParticleEffectController DirtVfxPrefab { get => _dirtVfxPrefab; }
		public ParticleEffectController MetalVfxPrefab { get => _metalVfxPrefab; }
	}
}
