
using UnityEngine;
using System.Collections;
using System;

public abstract class AbstractAnimationEvaluatiorBehaviour : MonoBehaviour {

	[SerializeField] protected float _animationDuration = 1f;
	[SerializeField] protected float _endValue = 1f;
	[SerializeField] protected bool _playOnStart;
	[SerializeField] protected bool _loop;

	public float AnimationDuration { get => _animationDuration; }
	public bool Loop { get => _loop; }
	public float EndValue { get => _endValue; }
	public bool IsPlaying { get; protected set; }

	protected float _timeElapsed;
	public Coroutine _animationCoroutine;

	public event EventHandler<EventArgs> Started;
	public event EventHandler<EventArgs> Ended;

	void Start ()
	{
		if (_playOnStart) {
			Play ();
		}
	}

	[ContextMenu("Play")]
	public void Play ()
	{
		OnPlay();
		if (_animationCoroutine != null) {
			StopCoroutine (_animationCoroutine);
		}
		IsPlaying = true;
		_animationCoroutine = StartCoroutine (AnimateCoroutine ());
		Started?.Invoke(this, EventArgs.Empty);
	}

	public abstract void OnPlay();

	public void Stop()
	{
		if (_animationCoroutine != null)
		{
			StopCoroutine(_animationCoroutine);
		}
		Evaluate(_endValue);
		OnAnimationComplete();
		Ended?.Invoke(this, EventArgs.Empty);
		IsPlaying = false;
	}

	IEnumerator AnimateCoroutine ()
	{
		_timeElapsed = 0f;
		while (_timeElapsed < _animationDuration) {
			yield return null;
			var percentComplete = _timeElapsed / _animationDuration;

			Evaluate (percentComplete);

			_timeElapsed += Time.deltaTime;
		}
		Evaluate (_endValue);
		Ended?.Invoke(this, EventArgs.Empty);
		LoopAnimation();
	}

	void LoopAnimation ()
	{
		if (_loop) {
			Play ();
		}
		else {
			IsPlaying = false;
			OnAnimationComplete();
		}
	}

	public abstract void Evaluate (float percentComplete);
	public abstract void OnAnimationComplete();
}
