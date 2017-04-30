using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Barometer : PlayerTool
{
	[Tooltip("Text component that displays barometer value.")]
	[SerializeField]
	private Text barometerValueText;

	private GameObject barometerObject;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		//ToolName = toolName; // temporary, need to add it to yaml so that toolname can be directly used,
		// it will still show up as the dropdown in the editor though
		ToolName = toolName;
		CheckBarometer = false; // will be set to false once tool can be equipped and unequipped from within the inventory
		transform.SetParent(attachJoint.transform, false);
		barometerObject = transform.GetChild(0).gameObject;

		barometerObject.SetActive(false);
	}

	/// <summary>
	/// Sets up the tool so that it is linked to the proper item in the inventory.
	/// </summary>
	/// <param name="itemForTool">Item for tool.</param>
	public override void SetUpTool(BaseItem itemForTool)
	{
		List<ItemAction> actions = new List<ItemAction>();
		EquipableCategory barometerCategory = (EquipableCategory) itemForTool.GetItemCategoryByClass(typeof(EquipableCategory));

		ItemAction unequipAction = new ItemAction(unequipActName, new UnityAction(barometerCategory.UnEquip));
		actions.Add(unequipAction);

		GuiInstanceManager.EquippedItemGuiInstance.SetEquipped(itemForTool.InventorySprite, actions);
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
		CheckBarometer = !CheckBarometer;
	}

	/// <summary>
	/// Equip the barometer.
	/// </summary>
	public override void Equip ()
	{
		// TODO: Run equip animation
		CheckBarometer = false;
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


