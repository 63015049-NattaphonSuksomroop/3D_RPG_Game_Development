
using Animation;
using Character;
using Player;
using UnityEngine;

namespace Ability {
	public class AbilityHandler : MonoBehaviour {

		[SerializeField] AbstractAbility[] _abilities;
		[SerializeField] AbstractCharacterManager _playerManager;

		private AbstractAbility _loadedAbility;
		private int _characterID;

		public AbstractAbility AbilityTwo { get => _abilities[0]; }
		public AbstractAbility[] Abilities { get => _abilities; }

		public void Init(int characterID)
		{
			_characterID = characterID;
			for (int i = 0; i < _abilities.Length; i++)
			{
				_abilities[i].InitilizeAbility(_playerManager);
			}
		}

		public void LoadAbility(int abilityNumber)
		{
			_loadedAbility = _abilities[abilityNumber];
			_loadedAbility.LoadAbility();
			_playerManager.SendLoadedAbilityEvent(abilityNumber);
		}

		public void FireAbility()
		{
			var abilityGameObject = _loadedAbility.FireAbility();
			_playerManager.SendFiredAbilityEvent(abilityGameObject);
			
		}

	}
}
