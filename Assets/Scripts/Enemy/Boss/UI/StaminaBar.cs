
using System;
using Character;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class StaminaBar : AbstractPlayerManagerUiInterface
	{
		[SerializeField] Image _image;


		protected override void OnNewCharacterAssigned()
		{
			_currentPlayer.Stats.StaminaUpdatedEvent += HandleStaminaUpdatedEvent;
			_image.fillAmount = _currentPlayer.Stats.CurrenStaminaPercent;
		}

		private void HandleStaminaUpdatedEvent(object sender, StaminaUpdatedEventArgs e)
		{
			SetCurrentStamina(e.CurrentStamina / e.MaxStamina);
		}

		private void SetCurrentStamina(float healthPercent)
		{
			_image.fillAmount = healthPercent;
		}

		private void OnDestroy()
		{
			if (_currentPlayer.Stats.HealthUpdatedEvent != null)
			{
				_currentPlayer.Stats.StaminaUpdatedEvent -= HandleStaminaUpdatedEvent;
			}
		}

	}
}
