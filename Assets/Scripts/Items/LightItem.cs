using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class LightItem : Tool 
{
    [SerializeField]
    [Tooltip("Location to parent the item on the player model")]
    private GameObject attachJoint;

    private Light lightSource;
    private LightCategory lightBase;

    private GameObject lightObject;
    private bool on;

    private const string addFuelActName = "Add Fuel";
    private const string turnOnOffActName = "Turn On/Off";

	// Fuel has an an attribute named burnTime
	private const string burnTimeAttrName = "burnTime"; 

	private const string brightnessAttrName = "brightness";

	private const string maxDurationAttrName = "maxDuration";

    private ItemCondition fuelCondition;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		On = false;
		ToolName = toolName;
		transform.SetParent(attachJoint.transform, false);

		lightObject = transform.GetChild(0).gameObject;
		lightSource = GetComponentInChildren<Light>();
		lightObject.SetActive(false);
	}

	/// <summary>
	/// Sets up the tool so that it is linked to the proper item in the inventory.
	/// </summary>
	/// <param name="itemForTool">Item for tool.</param>
	public override void SetUpTool(BaseItem itemForTool)
	{
		LightCategory lightCategory = (LightCategory) itemForTool.GetItemCategoryByClass(typeof(LightCategory));
		lightSource.intensity = lightCategory.Brightness;
		lightSource.enabled = On;

		lightBase = lightCategory;
		List<ItemAction> actions = new List<ItemAction>();

		ItemAction addFuelAction = new ItemAction(addFuelActName, new UnityAction(AddFuel));
		addFuelAction.TypeUsed.Add(ItemTypes.Fuel);
		fuelCondition = new ItemCondition(lightBase.CurrentFuelLevel, lightBase.MaxFuel, BooleanOperator.Less);
		addFuelAction.Conditions.Add(fuelCondition);

		ItemAction turnOnOffAction = new ItemAction(turnOnOffActName, new UnityAction(ToggleOn));

		ItemAction unequipAction = new ItemAction(unequipActName, new UnityAction(lightCategory.UnEquip));

		actions.Add(addFuelAction);
		actions.Add(turnOnOffAction);
		actions.Add(unequipAction);

		GuiInstanceManager.EquippedItemGuiInstance.SetEquipped(itemForTool.InventorySprite, actions);
	}

	/// <summary>
	/// Gets or sets a value indicating whether the light it on.
	/// </summary>
	/// <value><c>true</c> if on; otherwise, <c>false</c>.</value>
	public bool On
	{
		get
		{
			return on;
		}
		set
		{
			on = value;
			if(lightSource != null)
			{
				lightSource.enabled = on;
			}
		}
	}

	/// <summary>
	/// Equip this instance.
	/// </summary>
	public override void Equip()
	{
		On = false;
		lightObject.SetActive(true);
	}

	/// <summary>
	/// Unequip this instance.
	/// </summary>
	public override void Unequip()
	{
		On = false;

		lightObject.SetActive(false);
		StopAllCoroutines();
	}

	/// <summary>
	/// Use this instance.
	/// </summary>
	public override void Use()
	{
		ToggleOn();
	}

	/// <summary>
	/// Adds fuel to the light item.
	/// </summary>
	public void AddFuel()
	{
		BaseItem item = Game.Instance.PlayerInstance.Inventory.GetInventoryBaseItem(GuiInstanceManager.EquippedItemGuiInstance.GetSelected());

		lightBase.AddFuel(item.GetItemAttribute(burnTimeAttrName).Value);
		Game.Instance.PlayerInstance.Inventory.UseItem(item.ItemName, 1);
		fuelCondition.ConditionValue = lightBase.CurrentFuelLevel;
	}

	/// <summary>
	/// Switches the lantern from on to off and vice versa.
	/// </summary>
	public void ToggleOn()
	{
		On = !On;

		if(On)
		{
			StartCoroutine(consumeFuel());
		}
	}

	/// <summary>
	/// Consumes the fuel.
	/// </summary>
	/// <returns>The fuel.</returns>
	private IEnumerator consumeFuel()
	{
		while(On)
		{
			fuelCondition.ConditionValue = lightBase.CalculateRemainingFuel();
			if(lightBase.CurrentFuelLevel <= 0)
			{
				On = false;
			}

			yield return null;
		}
	}
}
