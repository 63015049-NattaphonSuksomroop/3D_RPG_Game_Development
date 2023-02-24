
using System;
using UnityEngine;

public class MLBattleStartTimer : MonoBehaviour
{
	[SerializeField] Animator _animator;

	public EventHandler<EventArgs> Ended;

	public void Play()
	{
		_animator.SetTrigger("Play");
	}

	public void AnimationEnded()
	{
		Ended?.Invoke(this, EventArgs.Empty);
	}
}
