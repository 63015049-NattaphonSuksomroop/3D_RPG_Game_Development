
using UnityEngine.EventSystems;

namespace UI
{
	public class RollButton : AbstractInputControllerButton
	{
		public override void OnPointerDown(PointerEventData eventData)
		{
			_inputController.HandleRollInput(false);
		}

		protected override void OnInputControllerAssigned()
		{
			
		}

		protected override void OnNewCharacterAssigned()
		{
			
		}
	}
}
