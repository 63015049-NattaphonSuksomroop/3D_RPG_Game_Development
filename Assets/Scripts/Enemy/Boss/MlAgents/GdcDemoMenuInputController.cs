
using PlayerInput;
using System;
using UnityEngine;

public class GdcDemoMenuInputController : MonoBehaviour
{
	[SerializeField] PlayerInputController _input;
	private bool _sendInput;

	public event EventHandler<EventArgs> Easy;
	public event EventHandler<EventArgs> Medium;
	public event EventHandler<EventArgs> Hard;
	public event EventHandler<EventArgs> Demo;


	private void Awake()
	{

		_input.RollInputPressed += HandleEasyButtonPressed;
		_input.AttackInputPressed += HandleMediumSelected;
		_input.AbilityInputPressed += HandleHardSelected;
		_input.StartInputPressed += DemoSelected;
	}

	public void SendInputEvents(bool value)
	{
		_sendInput = value;
	}

	private void HandleEasyButtonPressed(object sender, EventArgs e)
	{
		if (_sendInput)
		{
			Easy?.Invoke(this, EventArgs.Empty);
		}
	}

	private void HandleMediumSelected(object sender, EventArgs e)
	{
		if (_sendInput)
		{
			Medium?.Invoke(this, EventArgs.Empty);
		}
	}

	private void HandleHardSelected(object sender, EventArgs e)
	{
		if (_sendInput)
		{
			Hard?.Invoke(this, EventArgs.Empty);
		}
	}
	private void DemoSelected(object sender, EventArgs e)
	{
		if (_sendInput)
		{
			Demo?.Invoke(this, EventArgs.Empty);
		}
	}


	private void OnDestroy()
	{
		_input.AbilityInputPressed -= HandleEasyButtonPressed;
		_input.RunningInputUpdated -= HandleMediumSelected;
		_input.RollInputPressed -= HandleHardSelected;
		_input.StartInputPressed -= DemoSelected;
	}
}
