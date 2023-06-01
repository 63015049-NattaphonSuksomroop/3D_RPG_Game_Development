
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SettingsController : SingletonBehaviour<SettingsController>
{
	[Header("Renderer")]
	[SerializeField] UniversalRenderPipelineAsset _renderer;
	[SerializeField] UniversalRendererData _rendererData;

	[Header("Graphics Settings On Launch")]
	[SerializeField] TargetFps _targetFps = TargetFps.Thirty;

	//Target FPS
	readonly float _thirtyFpsFixedDeltaTime = 0.04f;
	readonly int _thirtyFpsTargetFrameRate = 30;
	readonly float _sixtyFpsFixedDeltaTime = 0.02f;
	readonly int _sixtyFpsTargetFrameRate = 60;


	public TargetFps TargetFps 
	{ 
		get => _targetFps;
		set
		{
			_targetFps = value;
		}
	}

	public override void Awake()
	{
		base.Awake();

		UpdateAppTargetFps(_targetFps);
		
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}


	public void UpdateAppTargetFps(TargetFps targetFps)
	{
		if (targetFps == TargetFps.Thirty)
		{
			TargetFps = targetFps;
			SetAppTo30Fps();
		}
		if (targetFps == TargetFps.Sixty)
		{
			TargetFps = targetFps;
			SetAppTo60Fps();
		}

	}

	private void SetAppTo30Fps()
	{
		Time.fixedDeltaTime = _thirtyFpsFixedDeltaTime;
		Application.targetFrameRate = _thirtyFpsTargetFrameRate;
	}

	private void SetAppTo60Fps()
	{
		Time.fixedDeltaTime = _sixtyFpsFixedDeltaTime;
		Application.targetFrameRate = _sixtyFpsTargetFrameRate;
	}
}

public enum TargetFps
{
	Thirty, Sixty
}