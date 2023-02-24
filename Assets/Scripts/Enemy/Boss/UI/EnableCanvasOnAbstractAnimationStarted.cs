
using UnityEngine;

public class EnableCanvasOnAbstractAnimationStarted : MonoBehaviour
{
	[SerializeField] AbstractAnimationEvaluatiorBehaviour _animator;
	[SerializeField] Canvas _canvas;

	private void Awake()
	{
		_animator.Started += HandleAnimationStatedEvent;
	}

	private void HandleAnimationStatedEvent(object sender, System.EventArgs e)
	{
		_canvas.enabled = true;
	}

	private void OnDestroy()
	{
		if (_animator != null)
		{
			_animator.Started -= HandleAnimationStatedEvent;
		}
	}
}
