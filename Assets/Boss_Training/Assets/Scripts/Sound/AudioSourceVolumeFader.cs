
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sounds
{
	public class AudioSourceVolumeFader : AbstractAnimationEvaluatiorBehaviour
	{
		[SerializeField] AudioSource _audioSource;
		[SerializeField] AnimationCurve _volumeAnimation;
		[SerializeField] bool _startAudioPlayingOnAnimationStart;
		[SerializeField] bool _StopAudioOnAnimationComplete;

		public override void Evaluate(float percentComplete)
		{
			_audioSource.volume = _volumeAnimation.Evaluate(percentComplete);
		}

		public override void OnAnimationComplete()
		{
			if (_StopAudioOnAnimationComplete)
			{
				_audioSource.Stop();
			}
		}

		public override void OnPlay()
		{
			if (_startAudioPlayingOnAnimationStart)
			{
				_audioSource.Play();
			}
		}

	}
}
