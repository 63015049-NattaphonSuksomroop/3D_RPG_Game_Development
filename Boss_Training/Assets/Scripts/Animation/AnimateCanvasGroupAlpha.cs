
using UnityEngine;

namespace Animation
{
	public class AnimateCanvasGroupAlpha : AbstractAnimationEvaluatiorBehaviour
	{

		[SerializeField] CanvasGroup _canvasGroup;
		[SerializeField]AnimationCurve _animation;

		public override void Evaluate(float percentComplete)
		{
			_canvasGroup.alpha = _animation.Evaluate(percentComplete);
		}

		public override void OnAnimationComplete()
		{
			
		}

		public override void OnPlay()
		{
			
		}
	}
}