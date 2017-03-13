using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Thermometer : Tool
{
	[Tooltip("Text component that displays thermometer value.")]
	[SerializeField]
	private Text thermometerValueText;

	private GameObject thermometerObject;

	void Start ()
	{
		//ToolName = toolName; // temporary, need to add it to yaml so that toolname can be directly used,
		// it will still show up as the dropdown in the editor though
		ToolName = toolName;
		CheckThermometer = false; // will be set to false once tool can be equipped and unequipped from within the inventory

		transform.SetParent(attachJoint.transform, false);
		thermometerObject = transform.GetChild(0).gameObject;

		thermometerObject.SetActive(false);
	}

	/// <summary>
	/// Sets up the tool so that it is linked to the proper item in the inventory.
	/// </summary>
	/// <param name="itemForTool">Item for tool.</param>
	public override void SetUpTool(BaseItem itemForTool)
	{
		List<ItemAction> actions = new List<ItemAction>();
		EquipableCategory thermometerCategory = (EquipableCategory) itemForTool.GetItemCategoryByClass(typeof(EquipableCategory));

		ItemAction unequipAction = new ItemAction(unequipActName, new UnityAction(thermometerCategory.UnEquip));
		actions.Add(unequipAction);

		GuiInstanceManager.EquippedItemGuiInstance.SetEquipped(itemForTool.InventorySprite, actions);
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
		CheckThermometer = !CheckThermometer;
	}

	/// <summary>
	/// Equip the thermometer.
	/// </summary>
	public override void Equip ()
	{
		// TODO: Run equip animation
		CheckThermometer = false;
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


