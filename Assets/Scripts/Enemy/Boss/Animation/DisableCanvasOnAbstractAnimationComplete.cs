
using UnityEngine;

namespace UI
{
	public class DisableCanvasOnAbstractAnimationComplete : MonoBehaviour
	{
		[SerializeField] AbstractAnimationEvaluatiorBehaviour _animator;
		[SerializeField] Canvas _canvas;

		private void Awake()
		{
			_animator.Ended += _animator_Ended;
		}

		private void _animator_Ended(object sender, System.EventArgs e)
		{
			_canvas.enabled = false;
		}

		private void OnDestroy()
		{
			if (_animator != null)
			{
				_animator.Ended -= _animator_Ended;
			}
		}
	}
}