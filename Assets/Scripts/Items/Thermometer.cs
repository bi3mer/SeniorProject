using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Thermometer : Tool
{
	[SerializeField]
	private string toolName;

	[SerializeField]
	private Text temporaryScreenThermometer;

	/// <summary>
	/// Initializes a new instance of the <see cref="Thermometer"/> class.
	/// </summary>
	public Thermometer ()
	{
		ToolName = toolName;
		CheckThermometer = true; // will be false once tool can be equipped and unequipped from within the inventory
	}

	/// <summary>
	/// Update the thermometer values.
	/// </summary>
	void FixedUpdate()
	{
		SetThermometerActive(CheckThermometer);
		UpdateThermometer ();
	}

	/// <summary>
	/// Uses the thermometer.
	/// </summary>
	public override void Use ()
	{
		if (CheckThermometer)
		{
			// call any additional methods that need to run when thermometer is in use
		}
	}

	/// <summary>
	/// Equip the thermometer.
	/// </summary>
	public override void Equip ()
	{
		// TODO: Run equip animation
		CheckThermometer = true;
	}

	/// <summary>
	/// Unequip the thermometer.
	/// </summary>
	public override void Unequip ()
	{
		// TODO: Run unequip animation
		CheckThermometer = false;
	}

	/// <summary>
	/// Returns true if the thermometer is in use.
	/// </summary>
	public bool CheckThermometer
	{
		get
		{
			return InUse;
		}
		private set
		{
			InUse = value;
		}
	}

	/// <summary>
	/// Sets the thermometer active.
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
	private void SetThermometerActive(bool value)
	{
		// for now this is a UI component, but once the thermometer item is added into the yaml, it should have a world model.
		temporaryScreenThermometer.gameObject.SetActive (value);
	}

	/// <summary>
	/// Updates the thermometer.
	/// </summary>
	private void UpdateThermometer()
	{
		// temporarily updating the UI text value - this is where the model should update it's text
		temporaryScreenThermometer.text = "Temperature: " + Game.Instance.WeatherInstance.WeatherInformation[(int)Weather.Temperature];
	}
}


