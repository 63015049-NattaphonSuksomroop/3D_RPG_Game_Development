
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
	[SerializeField]
	string _targetBool;

	[SerializeField]
	bool _status;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		animator.SetBool(_targetBool, _status);
	}
}
