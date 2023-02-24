
using Character;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class ManaBar : AbstractPlayerManagerUiInterface
	{
		[SerializeField] Image _image;


		protected override void OnNewCharacterAssigned()
		{
			_currentPlayer.Stats.ManaUpdatedEvent += HandleManaUpdatedEvent;
			_image.fillAmount = _currentPlayer.Stats.CurrentManaPercent;
		}

		private void HandleManaUpdatedEvent(object sender, ManaUpdatedEventArgs e)
		{
			SetCurrentMana(e.CurrentMana / e.MaxMana);
		}

		private void SetCurrentMana(float healthPercent)
		{
			_image.fillAmount = healthPercent;
		}

		private void OnDestroy()
		{
			if (_currentPlayer.Stats.ManaUpdatedEvent != null)
			{
				_currentPlayer.Stats.ManaUpdatedEvent -= HandleManaUpdatedEvent;
			}
		}

	}
}