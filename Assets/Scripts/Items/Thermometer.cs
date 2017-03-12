using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Thermometer : Tool
{
	[Tooltip("Thermometer tool name.")]
	[SerializeField]
	private string toolName;

	[Tooltip("Text component that displays thermometer value.")]
	[SerializeField]
	private Text thermometerValueText;

	/// <summary>
	/// Initializes a new instance of the <see cref="Thermometer"/> class.
	/// </summary>
	public Thermometer ()
	{
		ToolName = toolName;
	}

	void Start ()
	{
		Equip();// will be Unequip() once tool can be equipped and unequipped from within the inventory
	}

	/// <summary>
	/// Update the thermometer values.
	/// </summary>
	void Update()
	{
		if (CheckThermometer) 
		{
			updateThermometer ();
		}
	}

	/// <summary>
	/// Uses the thermometer.
	/// </summary>
	public override void Use ()
	{
		// UI model on-screen transformations
		CheckThermometer = true;
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
			setThermometerActive (value);
		}
	}

	/// <summary>
	/// Sets the thermometer active.
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
	private void setThermometerActive(bool value)
	{
		// for now this is a UI component, but once the thermometer item is added into the yaml, it should have a world model.
		thermometerValueText.gameObject.SetActive (value);
	}

	/// <summary>
	/// Updates the thermometer.
	/// </summary>
	private void updateThermometer()
	{
		// temporarily updating the UI text value - this is where the model should update it's text
		thermometerValueText.text = "Temperature: " + Game.Instance.WeatherInstance.WeatherInformation[(int)Weather.Temperature];
	}
}


