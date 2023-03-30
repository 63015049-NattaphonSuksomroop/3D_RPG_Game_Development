
using System;
using UI;
using UnityEngine;

public class MlBattleEndedUI : MonoBehaviour
{
	[SerializeField] MainMenuCanvasController _canvasController;
	[SerializeField] AbstractAnimationEvaluatiorBehaviour _loadMainMenuTimer;
	private MlAgentsGdcDemoController _demoController;


	private void LoadMainMenuTimer_Ended(object sender, EventArgs e)
	{
		_loadMainMenuTimer.Ended -= LoadMainMenuTimer_Ended;
		_demoController.ShowMenu();
}

	internal void ShowWinner(MlAgentsGdcDemoController mlAgentsGdcDemoController)
	{
		_demoController = mlAgentsGdcDemoController;
		_canvasController.Show();
		_loadMainMenuTimer.Play();
		_loadMainMenuTimer.Ended += LoadMainMenuTimer_Ended;
	}

	internal void Hide()
	{
		_canvasController.Hide();
	}
}
