
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationService : SingletonBehaviour<AnnotationService>
{
	public void SetMarker(string markerMessage)
	{
		Arm.Annotations.marker(markerMessage);
	}
}
