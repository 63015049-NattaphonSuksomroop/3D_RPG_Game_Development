
using System;
using Character;
using Player;
using PlayerInput;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public abstract class AbstractInputControllerButton: MonoBehaviour, IPointerDownHandler
	{
		[Header("Input Controller Button:")]
		[SerializeField] protected Image[] _buttonImages;
		[SerializeField] Color _enabledIamgeColor = Color.white;
		[SerializeField] Color _disabledImageColor = new Color(1,1,1,0.5f);

		protected PlayerInputController _inputController;
		protected AbstractCharacterManager _currentPlayer;
		protected bool _enabled = true;

		public void AssignDependencies(AbstractCharacterManager player, PlayerInputController inputController)
		{
			_currentPlayer = player;
			OnNewCharacterAssigned();

			_inputController = inputController;
			OnInputControllerAssigned();
		}

		protected void Disable()
		{
			_enabled = false;
			foreach (var bImage in _buttonImages)
			{
				bImage.color = _disabledImageColor;
			}
		}

		protected void Enable()
		{
			_enabled = true;
			foreach (var bImage in _buttonImages)
			{
				bImage.color = _enabledIamgeColor;
			}
		}

		protected abstract void OnNewCharacterAssigned();
		protected abstract void OnInputControllerAssigned();
		public abstract void OnPointerDown(PointerEventData eventData);

	}
}
