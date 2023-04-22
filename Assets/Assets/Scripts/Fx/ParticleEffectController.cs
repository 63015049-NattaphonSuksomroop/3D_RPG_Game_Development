
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fx
{
	public class ParticleEffectController : MonoBehaviour
	{
		[SerializeField] AudioSource _audioSource;
		[SerializeField] ParticleSystem[] _vfx;
		[SerializeField] AbstractAnimationEvaluatiorBehaviour[] _startEvaluatiors;
		[SerializeField] AbstractAnimationEvaluatiorBehaviour[] _stopEvaluatiors;
		[SerializeField] AudioClip[] _audioClips;

		[ContextMenu("Play")]
		public void Play()
		{
			for (int i = 0; i < _vfx.Length; i++)
			{
				_vfx[i].Play();
			}
			for (int i = 0; i < _startEvaluatiors.Length; i++)
			{
				_startEvaluatiors[i].Play();
			}
			if (_audioSource != null)
			{
				for (int i = 0; i < _audioClips.Length; i++)
				{
					_audioSource.PlayOneShot(_audioClips[i]);
				}
			}
		}
		public void Stop()
		{
			for (int i = 0; i < _vfx.Length; i++)
			{
				_vfx[i].Stop();
			}
			for (int i = 0; i < _stopEvaluatiors.Length; i++)
			{
				_stopEvaluatiors[i].Play();
			}
			if (_audioSource != null && _audioClips.Length > 0)
			{
				_audioSource.Stop();
			}

		}

		public bool IsAlive()
		{
			for (int i = 0; i < _vfx.Length; i++)
			{
				if (_vfx[i].isEmitting)
				{
					return true;
				}
				if (_vfx[i].IsAlive())
				{
					return true;
				}
			}
			for (int i = 0; i < _startEvaluatiors.Length; i++)
			{
				if (_startEvaluatiors[i].IsPlaying)
				{
					return true;
				}
			}
			for (int i = 0; i < _stopEvaluatiors.Length; i++)
			{
				if (_stopEvaluatiors[i].IsPlaying)
				{
					return true;
				}
			}
			if (_audioSource!= null && _audioSource.isPlaying)
			{
				return true;
			}
			return false;
		}
	}
}
