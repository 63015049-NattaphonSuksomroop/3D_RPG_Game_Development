
using UnityEngine.EventSystems;

namespace UI
{
	public class AttackButton : AbstractInputControllerButton
	{
		public override void OnPointerDown(PointerEventData eventData)
		{
			_inputController.HandleAttackInput(false);
		}

		protected override void OnInputControllerAssigned()
		{
		
		}

		protected override void OnNewCharacterAssigned()
		{
			
		}
	}
}
