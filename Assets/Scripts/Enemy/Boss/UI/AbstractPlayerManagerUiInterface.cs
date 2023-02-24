
using Character;
using Player;
using UnityEngine;

namespace UI
{
	public abstract class AbstractPlayerManagerUiInterface : MonoBehaviour
	{
		protected AbstractCharacterManager _currentPlayer;

		public void AssignNewPlayerManager(AbstractCharacterManager player)
		{
			_currentPlayer = player;
			OnNewCharacterAssigned();
		}

		protected abstract void OnNewCharacterAssigned();

	}
}
