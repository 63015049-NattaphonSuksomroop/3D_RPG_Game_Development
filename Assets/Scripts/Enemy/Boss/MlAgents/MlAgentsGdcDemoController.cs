
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static BattleEnvController;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
	public class MlAgentsGdcDemoController : BattleEnvController
	{
		[SerializeField] BattleEnvController _battleController;
		[SerializeField] AbstractAnimationEvaluatiorBehaviour _hideDemoUi;
		[SerializeField] AbstractAnimationEvaluatiorBehaviour _showDemoUi;
		[SerializeField] AbstractAnimationEvaluatiorBehaviour _autoDemoTimer;
		[SerializeField] MlBattleEndedUI _drArmWinsUi;
		[SerializeField] MlBattleEndedUI _mlWinsUi;
		[SerializeField] bool _autoDemo = true;
		[SerializeField] MLBattleStartTimer _battleTimer;
		[SerializeField] GdcDemoMenuInputController _input;
		[SerializeField] MlPlayerManager _mlPLayer;

		private BattleMode _mode;

		private void Start()
		{
			StartAutoDemoTimer();
			ListenForControllerEvents();
			_mlPLayer.RemoveInputListeners();
			_input.SendInputEvents(true);
		}

		private void ListenForControllerEvents()
		{
			_input.Easy += HandleEasyModeControllerInput;
			_input.Medium += HandleMediumModeControllerInput;
			_input.Hard += HandleHardModeControllerInput;
			_input.Demo += HandleDemoModeControllerInput;
		}

		private void HandleDemoModeControllerInput(object sender, EventArgs e)
		{
			PlayDemo();
		}

		private void HandleHardModeControllerInput(object sender, EventArgs e)
		{
			HardModeSelected();
		}

		private void HandleMediumModeControllerInput(object sender, EventArgs e)
		{
			MediumModeSelected();
		}

		private void HandleEasyModeControllerInput(object sender, EventArgs e)
		{
			EasyModeSelected();
		}

		private void StartAutoDemoTimer()
		{
			if (_autoDemo)
			{
				_autoDemoTimer.Play();
				_autoDemoTimer.Ended += HandleAutoDemoTimerCompletedEvent;
			}
		}

		private void StopAutoDemoTimer()
		{
			if (_autoDemo)
			{
				_autoDemoTimer.Ended -= HandleAutoDemoTimerCompletedEvent;
				_autoDemoTimer.Stop();
			}
		}

		private void HandleAutoDemoTimerCompletedEvent(object sender, EventArgs e)
		{

			_autoDemoTimer.Ended -= HandleAutoDemoTimerCompletedEvent;
			PlayDemo();
		}

		internal void ShowMenu()
		{
			_showDemoUi.Play();
			_showDemoUi.Ended += DemoUiShowing;
		}

		private void DemoUiHidden(object sender, System.EventArgs e)
		{
			_hideDemoUi.Ended -= DemoUiHidden;
			_battleTimer.Play();
			_mlPLayer.RemoveInputListeners();
			_mlPLayer.ZeroOutLastInput();
			_battleTimer.Ended += HandleBattleTimerEndedEvent;
		}


		private void HandleBattleTimerEndedEvent(object sender, EventArgs e)
		{
			_battleTimer.Ended -= HandleBattleTimerEndedEvent;
			StartBattle();
		}

		private void StartBattle()
		{
			_mlPLayer.ZeroOutLastInput();
			_mlPLayer.ListenForInput();
			_battleController.BattleEndEvent += HandleBattleEndedEvent;
			switch (_mode)
			{
				case BattleMode.Easy:
					_mlPLayer.ListToControllerEvents(true);
					_battleController.StartBattleEasy();
					break;
				case BattleMode.Normal:
					_mlPLayer.ListToControllerEvents(true);
					_battleController.StartBattleNormal();
					break;
				case BattleMode.Hard:
					_mlPLayer.ListToControllerEvents(true);
					_battleController.StartBattleHard();
					break;
				case BattleMode.Demo:
					_mlPLayer.ListToControllerEvents(false);
					_battleController.StartBattleDemo();
					break;
				case BattleMode.Pause:
					_mlPLayer.ListToControllerEvents(false);
					_battleController.PauseBattle();
					break;
				case BattleMode.Default:
					break;
				default:
					break;
			}
		}

		private void HandleBattleEndedEvent(object sender, BattleEndEventArgs e)
		{
			_mlPLayer.RemoveInputListeners();
			_mlPLayer.ZeroOutLastInput();
			_battleController.BattleEndEvent -= HandleBattleEndedEvent;
			if (e.BattleResult == 1)
			{
				_drArmWinsUi.ShowWinner(this);
			}
			else
			{
				_mlWinsUi.ShowWinner(this);
			}

		}

		private void DemoUiShowing(object sender, EventArgs e)
		{
			_showDemoUi.Ended -= DemoUiShowing;
			_input.SendInputEvents(true);
			_battleController.ResetScene();
			_drArmWinsUi.Hide();
			_mlWinsUi.Hide();
			StartAutoDemoTimer();
		}

		public void EasyModeSelected()
		{
			_input.SendInputEvents(false);
			StopAutoDemoTimer();
			_hideDemoUi.Play();
			_hideDemoUi.Ended += DemoUiHidden;
			_mode = BattleMode.Easy;
		}

		public void MediumModeSelected()
		{
			_input.SendInputEvents(false);
			StopAutoDemoTimer();
			_hideDemoUi.Play();
			_hideDemoUi.Ended += DemoUiHidden;
			_mode = BattleMode.Normal;
		}
		public void HardModeSelected()
		{
			_input.SendInputEvents(false);
			StopAutoDemoTimer();
			_hideDemoUi.Play();
			_hideDemoUi.Ended += DemoUiHidden;
			_mode = BattleMode.Hard;
		}

		public void PlayDemo()
		{
			_input.SendInputEvents(false);
			StopAutoDemoTimer();
			_hideDemoUi.Play();
			_hideDemoUi.Ended += DemoUiHidden;
			_mode = BattleMode.Demo;
		}
	}

}

