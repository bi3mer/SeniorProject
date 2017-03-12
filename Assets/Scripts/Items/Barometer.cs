using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Barometer : Tool
{
	[Tooltip("Barometer tool name.")]
	[SerializeField]
	private string toolName;

	[Tooltip("Text component that displays barometer value.")]
	[SerializeField]
	private Text barometerValueText;

	/// <summary>
	/// Initializes a new instance of the <see cref="Barometer"/> class.
	/// </summary>
	public Barometer ()
	{
		ToolName = toolName;
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		Equip(); // will be Unequip once tool can be equipped and unequipped from within the inventory
	}

	/// <summary>
	/// Update the barometer values.
	/// </summary>
	void Update()
	{
		if (CheckBarometer) 
		{
			updateBarometer ();
		}
	}

	/// <summary>
	/// Uses the barometer.
	/// </summary>
	public override void Use ()
	{
		// UI model on-screen transformations
		CheckBarometer = true;
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
			setBarometerActive (value);
		}
	}

	/// <summary>
	/// Sets the barometer active.
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
	private void setBarometerActive(bool value)
	{
		// for now this is a UI component, but once the barometer item is added into the yaml, it should have a world model.
		barometerValueText.gameObject.SetActive (value);
	}

	/// <summary>
	/// Updates the barometer.
	/// </summary>
	private void updateBarometer()
	{
		// temporarily updating the UI text value - this is where the model should update it's text
		barometerValueText.text = "Pressure: " + Game.Instance.WeatherInstance.WeatherInformation[(int)Weather.Pressure];
	}
}


