
using Character;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class HealthBar : AbstractPlayerManagerUiInterface
	{
		[SerializeField] Image _image;

		protected override void OnNewCharacterAssigned()
		{
			_currentPlayer.Stats.HealthUpdatedEvent += HandelHealthUpdatedEvent;
			_image.fillAmount = _currentPlayer.Stats.CurrentHealthPercent;
		}


		private void HandelHealthUpdatedEvent(object sender, HealthUpdatedEventArgs e)
		{
			SetCurrentHealth(((float)e.CurrentHealth / (float)e.MaxHealth));
		}

		private void SetCurrentHealth(float healthPercent)
		{
			_image.fillAmount = healthPercent;
		}

		private void OnDestroy()
		{
			if (_currentPlayer != null)
			{
				_currentPlayer.Stats.HealthUpdatedEvent -= HandelHealthUpdatedEvent;
			}
		}

	}
}
