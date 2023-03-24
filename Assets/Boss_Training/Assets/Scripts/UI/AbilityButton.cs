
using Ability;
using Character;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
	public class AbilityButton : AbstractInputControllerButton
	{
		[Header("Ability Button:")]
		[SerializeField] int _abilityNumer;
		private ResourceCostType _currentResource;
		private AbstractAbility _currentAbility;

		public override void OnPointerDown(PointerEventData eventData)
		{
			if (_enabled)
			{
				_inputController.HandleAbilityInputUpdated(_abilityNumer);
			}
		}


		protected override void OnInputControllerAssigned()
		{
			
		}

		protected override void OnNewCharacterAssigned()
		{
			_currentAbility = _currentPlayer.AbilityHandler.AbilityTwo;
			switch (_currentAbility.ResourceCostType)
			{
				case ResourceCostType.None:
					break;
				case ResourceCostType.Health:
					break;
				case ResourceCostType.Stamina:
					break;
				case ResourceCostType.Mana:
					_currentPlayer.Stats.ManaUpdatedEvent += HandleManaUpdatedEvent;
					_currentResource = _currentAbility.ResourceCostType;
					break;
				default:
					break;
			}
		}

		private void HandleManaUpdatedEvent(object sender, ManaUpdatedEventArgs e)
		{
			if (!_currentAbility.HasResourcesToCastAbility())
			{
				Disable();
			}
			else
			{
				Enable();
			}
		}

		private void OnDestroy()
		{
			switch (_currentResource)
			{
				case ResourceCostType.None:
					break;
				case ResourceCostType.Health:
					break;
				case ResourceCostType.Stamina:
					break;
				case ResourceCostType.Mana:
					if (_currentPlayer.Stats != null)
					{
						_currentPlayer.Stats.ManaUpdatedEvent -= HandleManaUpdatedEvent;
					}
					break;
				default:
					break;
			}
		}
	}
}
