
using UnityEngine;
using UnityEngine.EventSystems;

namespace Joystick {
	public class Joystick : AbstractJoystick, IPointerDownHandler, IDragHandler, IPointerUpHandler {

		[SerializeField]
		float _handleRange = 1;

		[SerializeField]
		float _deadZone = 0;

		[SerializeField]
		Canvas _canvas;

		[SerializeField]
		RectTransform _background;

		[SerializeField]
		RectTransform _handle;

		private Camera _cam;

		private Vector2 _input;

		private void Start()
		{
			Vector2 center = new Vector2(0.5f, 0.5f);
			_background.pivot = center;
			_handle.anchorMin = center;
			_handle.anchorMax = center;
			_handle.pivot = center;
			_handle.anchoredPosition = Vector2.zero;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			OnDrag(eventData);
			SendInputStartedEvent();
		}

		public void OnDrag(PointerEventData eventData)
		{
			//Passing in a null camera here makes joy sticks work in ScreenSpace - Overlay mode.
			Vector2 position = RectTransformUtility.WorldToScreenPoint(_cam, _background.position);
			Vector2 radius = _background.sizeDelta / 2;

			_input = (eventData.position - position) / (radius * _canvas.scaleFactor);
			HandleInput(_input.magnitude, _input.normalized);
			_handle.anchoredPosition = _input * radius * _handleRange;
			SendInputUpdatedEvet(_input);
		}

		private void HandleInput(float magnitude, Vector2 normalised)
		{
			if (magnitude > _deadZone)
			{
				if (magnitude > 1)
					_input = normalised;
			}
			else
			{
				_input = Vector2.zero;
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			_input = Vector2.zero;
			_handle.anchoredPosition = Vector2.zero;
			SendInputEndedEvent();
		}
	}
}
