
using Fx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxService : SingletonBehaviour<VfxService>
{
	[SerializeField] CharacterFxSO _characterFxSO;

	public CharacterFxSO CharacterFxSO { get => _characterFxSO; set => _characterFxSO = value; }

}
