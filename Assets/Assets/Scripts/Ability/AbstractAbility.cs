
using Character;
using Player;
using System;
using UnityEngine;

namespace Ability {
	public abstract class AbstractAbility : MonoBehaviour {

		[SerializeField] protected string AnimationName;
		[SerializeField] int _resourceCost = 1;
		[SerializeField] ResourceCostType _resourceCostType = ResourceCostType.Mana;
		protected AbstractCharacterManager _characterManager;
		
		public ResourceCostType ResourceCostType { get => _resourceCostType; }
		public int ResourceCost { get => _resourceCost; }

		public void InitilizeAbility(AbstractCharacterManager characterManager)
		{
			_characterManager = characterManager;
			SetUpAblity();
		}


		public abstract void SetUpAblity();

		public abstract void LoadAbility();

		public abstract GameObject FireAbility();

		public bool HasResourcesToCastAbility()
		{
			switch (_resourceCostType)
			{
				case ResourceCostType.None:
					return true;
				case ResourceCostType.Health:
					if (_characterManager.Stats.CurrentHealth > _resourceCost + 1) 
					{
						return true;
					}
					else
					{
						return false;
					}
				case ResourceCostType.Stamina:
					if (_characterManager.Stats.CurrentStamina > _resourceCost )
					{
						return true;
					}
					else
					{
						return false;
					}
				case ResourceCostType.Mana:
					if (_characterManager.Stats.CurrentMana > _resourceCost )
					{
						return true;
					}
					else
					{
						return false;
					}
			}
			return false;
		}

		protected void SpendResource()
		{
			switch (_resourceCostType)
			{
				case ResourceCostType.None:
					break;
				case ResourceCostType.Health:
					_characterManager.Stats.TakeDamage(_resourceCost);
					break;
				case ResourceCostType.Stamina:
					_characterManager.Stats.UseStamina(_resourceCost);
					break;
				case ResourceCostType.Mana:
					_characterManager.Stats.UseMana(_resourceCost);
					break;
				default:
					break;
			}
		}
	}

	public enum ResourceCostType { None, Health, Stamina, Mana}
}
