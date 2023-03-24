
using System;
using UnityEngine;

public class AppManager : SingletonBehaviour<AppManager>
{
	[SerializeField] SceneManagerService _sceneManagerService;
	[SerializeField] AnnotationService _annotationService;

	private void Start()
	{
		_annotationService.SetMarker("Game Started");
	}


}

