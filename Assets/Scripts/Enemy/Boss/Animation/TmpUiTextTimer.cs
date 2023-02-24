
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Animation
{
	public class TmpUiTextTimer : AbstractAnimationEvaluatiorBehaviour
	{
		[SerializeField] TextMeshProUGUI _text;

		public override void Evaluate(float percentComplete)
		{
			int timeleft = (int)AnimationDuration - (int)_timeElapsed;
			_text.text = timeleft.ToString();
		}

		public override void OnAnimationComplete()
		{
			_text.text = EndValue.ToString();
		}

		public override void OnPlay()
		{

		}


	}
}
