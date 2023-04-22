
using System;
using UnityEngine;

namespace UI
{
	public class MainMenuCanvasController : MonoBehaviour
	{
		[SerializeField] Canvas _canvas;
		[SerializeField] AbstractAnimationEvaluatiorBehaviour[] _hideAnimations;
		[SerializeField] AbstractAnimationEvaluatiorBehaviour[] _showAnimations;
		[SerializeField] CanvasGroup _canvasGroup;
	


		public void Hide()
		{
			_canvasGroup.interactable = false;
			foreach (var animation in _hideAnimations)
			{
				animation.Play();
			}
		}

		public void Show()
		{
			_canvas.enabled = true;
			foreach (var animation in _showAnimations)
			{
				animation.Play();
			}
			_canvasGroup.interactable = true;
		}
	}
}
