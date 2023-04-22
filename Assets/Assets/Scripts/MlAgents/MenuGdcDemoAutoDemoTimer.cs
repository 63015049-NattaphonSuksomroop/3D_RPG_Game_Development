
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGdcDemoAutoDemoTimer : AbstractAnimationEvaluatiorBehaviour
{
	[SerializeField] GameObject _visulizationGO;
	[SerializeField] AnimationCurve _gameObjectXScaleAniamation;


	public override void Evaluate(float percentComplete)
	{
		_visulizationGO.transform.localScale = new Vector3(_gameObjectXScaleAniamation.Evaluate(percentComplete), _visulizationGO.transform.localScale.y, _visulizationGO.transform.localScale.z);

	}

	public override void OnAnimationComplete()
	{

	}

	public override void OnPlay()
	{

	}


}
