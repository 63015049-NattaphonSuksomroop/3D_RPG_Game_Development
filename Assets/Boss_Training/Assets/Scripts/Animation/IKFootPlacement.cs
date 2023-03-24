
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootPlacement : MonoBehaviour
{
    [SerializeField] Animator _animator;
	[SerializeField, Range (0,1f)] float _distanceToGroud;
	[SerializeField] LayerMask _hitMask;

	private RaycastHit _hit;

	private void Reset()
	{
        _animator = gameObject.GetComponent<Animator>();
	}

	private void OnAnimatorIK(int layerIndex)
	{
		DoFootIK(AvatarIKGoal.LeftFoot, 1f);

		DoFootIK(AvatarIKGoal.RightFoot, 1f);
	}

	private void DoFootIK(AvatarIKGoal iKGoal, float ikWeight)
	{

		_animator.SetIKPositionWeight(iKGoal, ikWeight);
		_animator.SetIKRotationWeight(iKGoal, ikWeight);

		Ray ray = new Ray(_animator.GetIKPosition(iKGoal) + Vector3.up, Vector3.down);
		if (Physics.Raycast(ray, out _hit, _distanceToGroud + 1f, _hitMask))
		{
			Vector3 footPosition = _hit.point;
			footPosition.y += _distanceToGroud;

			_animator.SetIKPosition(iKGoal, footPosition);
		}
	}
}
