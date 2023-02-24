
using Animation;
using UnityEngine;

namespace Player
{
	public class PlayerAnimatorController : AbstractAnimatorController
	{
		[SerializeField] CharacterController _characterController;

		protected override void OnAnimatorMove()
		{
			if (IsInteracting == false)
			{
				return;
			}

			Vector3 deltaPosition = _animator.deltaPosition;
			deltaPosition.y = 0;
			Vector3 velocity = deltaPosition / Time.deltaTime;
			_characterController.SimpleMove(velocity);
		}
	}
}
