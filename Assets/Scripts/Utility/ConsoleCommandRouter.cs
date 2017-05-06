using UnityEngine;
using System.IO;
using System.Globalization;
using System.CodeDom.Compiler;
using System;
using System.Text;
using System.Reflection;

public class ConsoleCommandRouter : MonoBehaviour 
{
	[SerializeField]
	private string testCommand = "test";

	[SerializeField]
	private string debugMode = "god";

	[SerializeField]
	private string debugWeather = "weather";

	[SerializeField]
	private string printWeather = "wstr";

	[SerializeField]
	private GameObject debugWeatherGUI;
	private GameObject instantiatedWeather = null;

	[SerializeField]
	private string timeScale = "time";

	[SerializeField]
	private string pressureSystem = "press";
	private PressureSystemVisualization pressureScript;
	private const string pressureMessage = "Pressure script visualization ";


	[SerializeField]
	private string creatureCount = "creature_count";

	[SerializeField]
	private GameObject debugCreatureGUI;
	private GameObject instantiatedCreatureGUI = null;



    [SerializeField]
    private string teleportToTallestBuilding = "ending";

    [SerializeField]
    private GameObject tallestBuildingTeleportLocation;


    [SerializeField]
    private string health = "health";

    [SerializeField]
    private string activateEvent  = "event";

    /// <summary>
    /// Testing console with very important command
    /// </summary>
    /// <param name="args">Arguments.</param>
    public string Test(params string[] args) 
	{
		return "Test CONFIRMED";
	}

	/// <summary>
	/// Calls debug movement from the console
	/// </summary>
	/// <returns>The movement.</returns>
	/// <param name="args">Arguments.</param>
	public string DebugMode(params string[] args)
	{
		string response = "Debug mode activated";

		if(Game.Instance.DebugMode)
		{	
			response = "Debug mode de-activated";
		}

		Game.Instance.DebugMode = !Game.Instance.DebugMode;

		return response;
	}

	/// <summary>
	/// Opens debug weather gui
	/// </summary>
	/// <returns>The weather GU.</returns>
	/// <param name="args">Arguments.</param>
	public string DebugWeatherGUI(params string[] args)
	{
		string returnInfo = "Instantiated GUI";

		if(this.instantiatedWeather == null)
		{
			// instantiate object and save
			this.instantiatedWeather = Instantiate(this.debugWeatherGUI);
		}
		else
		{
			GameObject.Destroy(this.instantiatedWeather);
			this.instantiatedWeather = null;
			returnInfo = "Removed GUI";
		}
		return returnInfo;
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
	/// Activates the pressure system.
	/// </summary>
	/// <returns>The pressure system.</returns>
	/// <param name="args">Arguments.</param>
	public string ActivatePressureSystem(params string[] args)
	{
		if(this.pressureScript == null)
		{
			this.pressureScript = this.gameObject.AddComponent<PressureSystemVisualization>();
			return pressureMessage + "activated";
		}
		else
		{
			Destroy(this.pressureScript);
			this.pressureScript = null;
			return pressureMessage + "de-activated";
		}
	}

	/// <summary>
	/// Opens debug creature count gui
	/// </summary>
	/// <returns>The weather GU.</returns>
	/// <param name="args">Arguments.</param>
	public string DebugCreatureCountGUI(params string[] args)
	{
		string returnInfo = "Instantiated GUI";

		if(this.instantiatedCreatureGUI == null)
		{
			// instantiate object and save
			this.instantiatedCreatureGUI = Instantiate(this.debugCreatureGUI);
		}
		else
		{
			if(this.instantiatedCreatureGUI.activeSelf)
			{
				this.instantiatedCreatureGUI.SetActive(false);
				returnInfo = "Removed GUI";
			}
			else
			{
				this.instantiatedCreatureGUI.SetActive(true);
			}

		}

		return returnInfo;
	}

	/// <summary>
	/// Prints the weather.
	/// </summary>
	/// <returns>The weather.</returns>
	/// <param name="args">Arguments.</param>
	public string PrintWeather(params string[] args)
	{
		return Game.Instance.WeatherInstance.ToString();
	}


    /// <summary>
    /// Teleports the player to a gameobject.
    /// </summary>
    /// <param name="args">Arguments</param>
    public string TeleportToEnding(params string[] args)
    {
        Game.Instance.PlayerInstance.Controller.transform.position = tallestBuildingTeleportLocation.transform.position;
        Game.Instance.PlayerInstance.Controller.SetIsOnLand();
        return "You're at the ending now champ";
    }

    /// <summary>
    /// Sets the player's health to a number
    /// </summary>
    /// <param name="args">Arguments</param>
    public string SetHealth(params string[] args)
    {
        Game.Instance.PlayerInstance.Health = int.Parse(args[0]);
        Game.Instance.PlayerInstance.Warmth = int.Parse(args[0]);
        Game.Instance.PlayerInstance.Hunger = int.Parse(args[0]);
        return "All Health Stats set to:  " + args[0];
    }

    /// <summary>
    /// Activate the specific event
    /// </summary>
    /// <param name="args">Arguments.</param>
    public string Event(params string[] args)
    {
 		Type type = Game.Instance.EventManager.GetType();

 		if(type != null)
 		{
 			MethodInfo method = type.GetMethod(args[0]);

 			if(method != null)
 			{
 				method.Invoke(Game.Instance.EventManager, null);
				return "Activated event: " + args[0];
 			}

			return "Invalid subscription name: " + args[0];
		}

		return "type not found. Contact admin.";
    }

    /// <summary>
    /// initialize console commands
    /// </summary>
    void Start () 
	{
		ConsoleCommandsRepository.Instance.RegisterCommand(this.testCommand,               this.Test);
		ConsoleCommandsRepository.Instance.RegisterCommand(this.debugMode,                 this.DebugMode);
		ConsoleCommandsRepository.Instance.RegisterCommand(this.debugWeather,              this.DebugWeatherGUI);
		ConsoleCommandsRepository.Instance.RegisterCommand(this.timeScale,                 this.SetTimeScale);
		ConsoleCommandsRepository.Instance.RegisterCommand(this.pressureSystem,            this.ActivatePressureSystem);
		ConsoleCommandsRepository.Instance.RegisterCommand(this.printWeather,              this.PrintWeather);
		ConsoleCommandsRepository.Instance.RegisterCommand(this.creatureCount,             this.DebugCreatureCountGUI);
        ConsoleCommandsRepository.Instance.RegisterCommand(this.teleportToTallestBuilding, this.TeleportToEnding);
		ConsoleCommandsRepository.Instance.RegisterCommand(this.health,                    this.SetHealth);
		ConsoleCommandsRepository.Instance.RegisterCommand(this.activateEvent,             this.Event);
    }
}