using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Barometer : Tool
{
	[SerializeField]
	private string toolName;

	[SerializeField]
	private Text temporaryScreenBarometer;

	/// <summary>
	/// Initializes a new instance of the <see cref="Barometer"/> class.
	/// </summary>
	public Barometer ()
	{
		ToolName = toolName;
		CheckBarometer = true; // will be false once tool can be equipped and unequipped from within the inventory
	}

	/// <summary>
	/// Update the barometer values.
	/// </summary>
	void FixedUpdate()
	{
		SetBarometerActive(CheckBarometer);
		UpdateBarometer ();
	}

	/// <summary>
	/// Uses the barometer.
	/// </summary>
	public override void Use ()
	{
		if (CheckBarometer)
		{
			// call any additional methods that need to run when barometer is in use
		}
	}

	/// <summary>
	/// Equip the barometer.
	/// </summary>
	public override void Equip ()
	{
		// TODO: Run equip animation
		CheckBarometer = true;
	}

	/// <summary>
	/// Unequip the barometer.
	/// </summary>
	public override void Unequip ()
	{
		// TODO: Run unequip animation
		CheckBarometer = false;
	}

	/// <summary>
	/// Returns true if the barometer is in use.
	/// </summary>
	public bool CheckBarometer
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
	/// Sets the barometer active.
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
	private void SetBarometerActive(bool value)
	{
		// for now this is a UI component, but once the barometer item is added into the yaml, it should have a world model.
		temporaryScreenBarometer.gameObject.SetActive (value);
	}

	/// <summary>
	/// Updates the barometer.
	/// </summary>
	private void UpdateBarometer()
	{
		// temporarily updating the UI text value - this is where the model should update it's text
		temporaryScreenBarometer.text = "Pressure: " + Game.Instance.WeatherInstance.WeatherInformation[(int)Weather.Pressure];
	}
}


