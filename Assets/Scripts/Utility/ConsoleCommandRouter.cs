﻿using UnityEngine;
using System.IO;
using System.Globalization;
using System.CodeDom.Compiler;
using System.Text;

public class ConsoleCommandRouter : MonoBehaviour 
{
	[SerializeField]
	private string testCommand = "test";

	[SerializeField]
	private string debugMovement = "fly";

	[SerializeField]
	private string debugWeather = "weather";

	[SerializeField]
	private string timeScale = "time";

	/// <summary>
	/// Testing console with very important command
	/// </summary>
	/// <param name="args">Arguments.</param>
	public string Test(params string[] args) 
	{
		return "Colan Rulez";
	}

	/// <summary>
	/// Calls debug movement from the console
	/// </summary>
	/// <returns>The movement.</returns>
	/// <param name="args">Arguments.</param>
	public string DebugMovement(params string[] args)
	{
		return "Gabby has not implemented this yet";
	}

	/// <summary>
	/// Opens debug weather gui
	/// </summary>
	/// <returns>The weather GU.</returns>
	/// <param name="args">Arguments.</param>
	public string DebugWeatherGUI(params string[] args)
	{
		return "Colan has not implemented this yet";
	}

	/// <summary>
	/// Set time scale for game
	/// </summary>
	/// <returns>Validation time scale was set</returns>
	/// <param name="args">Arguments.</param>
	public string SetTimeScale(params string[] args)
	{
		// set time
		Time.timeScale = float.Parse(args[0]);

		return "Time scale set to " + args[0];
	}

	/// <summary>
	/// initialize console commands
	/// </summary>
	void Start () 
	{
		ConsoleCommandsRepository.Instance.RegisterCommand(this.testCommand, this.Test);
		ConsoleCommandsRepository.Instance.RegisterCommand(this.debugMovement, this.DebugMovement);
		ConsoleCommandsRepository.Instance.RegisterCommand(this.debugWeather, this.DebugWeatherGUI);
		ConsoleCommandsRepository.Instance.RegisterCommand(this.timeScale, this.SetTimeScale);
	}
}
