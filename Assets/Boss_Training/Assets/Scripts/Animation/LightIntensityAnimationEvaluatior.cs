
using UnityEngine;

namespace Animation
{
	public class LightIntensityAnimationEvaluatior : AbstractAnimationEvaluatiorBehaviour
	{
		[SerializeField] Light _light;
		[SerializeField] AnimationCurve _animationCurve;


		public override void Evaluate(float percentComplete)
		{
			_light.enabled = true;
			float value = _animationCurve.Evaluate(percentComplete);
			_light.intensity = value;
		}

		public override void OnAnimationComplete()
		{
			_light.enabled = false;
		}

		public override void OnPlay()
		{
			_light.enabled = true;
		}
	}
}
