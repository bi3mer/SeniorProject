using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ClothCategory : ItemCategory
{
	/// <summary>
	/// Gets or sets the thread density.
	/// </summary>
	/// <value>The thread density.</value>
	public float ThreadDensity
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the fabric thickness.
	/// </summary>
	/// <value>The fabric thickness.</value>
	public float FabricThickness
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the impermiability.
	/// </summary>
	/// <value>The impermiability.</value>
	public float Impermiability
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the on player.
	/// </summary>
	/// <value>The on player.</value>
	public float OnPlayer
	{
		get;
		set;
	}

	private float fireWarmthDifference;
	private float outsideWarmthDifference;
	private float shelterWarmthDifference;

	private const string threadDenAttrName = "threadDensity";
	private const string impermiabilityAttrName = "impermiability";
	private const string thicknessAttrName = "fabricThickness";

	private const string putOnActName = "Put On";
	private const string takeOffActName = "Take Off";

	private const float maxWaterPermiability = 5f;
	private const float dampnessIncreaseRate = 0.1f;

	protected const string onPlayerAttributeName = "onPlayer";

	/// <summary>
	/// Gets a copy of the ItemCategory.
	/// </summary>
	/// <returns>The duplicate.</returns>
	public override ItemCategory GetDuplicate()
	{
		ClothCategory category = new ClothCategory();
		category.ThreadDensity = ThreadDensity;
		category.Impermiability = Impermiability;
		category.FabricThickness = FabricThickness;
		category.OnPlayer = OnPlayer;

		category.Actions = new List<ItemAction>();
		category.Attributes = new List<Attribute>();

		ItemAction putOn = new ItemAction (putOnActName, new UnityAction(category.PutOn));
		ItemAction takeOff = new ItemAction(takeOffActName, new UnityAction(category.TakeOff));

		category.Actions.Add(putOn);
		category.Actions.Add(takeOff);

		finishDuplication(category);

		return category;
	}

	/// <summary>
	/// Preps the category for use by loading attributes and actions into lists.
	/// </summary>
	public override void ReadyCategory()
	{
		Attributes = new List<Attribute> ();

		Attributes.Add (new Attribute (onPlayerAttributeName, OnPlayer));
		Attributes.Add (new Attribute (threadDenAttrName, ThreadDensity));
		Attributes.Add (new Attribute (impermiabilityAttrName, Impermiability));
		Attributes.Add (new Attribute(thicknessAttrName, FabricThickness));

		Actions = new List<ItemAction> ();

		ItemAction putOn = new ItemAction (putOnActName, new UnityAction(PutOn));
		ItemAction takeOff = new ItemAction (takeOffActName, new UnityAction(TakeOff));

		// threshold is 1f, which means that the cloth item is on the player
		putOn.Conditions.Add(new ItemCondition(onPlayerAttributeName, 1f, new BooleanOperator.BooleanOperatorDelegate(BooleanOperator.Less)));
		takeOff.Conditions.Add(new ItemCondition(onPlayerAttributeName, 1f, new BooleanOperator.BooleanOperatorDelegate(BooleanOperator.GreaterOrEqual)));


		Actions.Add (putOn);
		Actions.Add (takeOff);
	}

	/// <summary>
	/// Puts on the cloth item.
	/// TODO: Make this similar to how equipping works.
	/// </summary>
	public void PutOn()
	{
		PlayerController player = Game.Instance.PlayerInstance.Controller;

		fireWarmthDifference = Mathf.Min(FabricThickness + ThreadDensity, player.FireWarmthIncreaseRate);
		outsideWarmthDifference = Mathf.Min(FabricThickness + ThreadDensity, player.OutsideWarmthIncreaseRate);
		shelterWarmthDifference = Mathf.Min(FabricThickness + ThreadDensity, player.ShelterWarmthIncreaseRate);

		player.FireWarmthIncreaseRate = player.FireWarmthIncreaseRate - fireWarmthDifference;
		player.OutsideWarmthIncreaseRate = player.OutsideWarmthIncreaseRate - outsideWarmthDifference;
		player.ShelterWarmthIncreaseRate = player.ShelterWarmthIncreaseRate - shelterWarmthDifference;

		OnPlayer = 1f;
		GetAttribute(onPlayerAttributeName).Value = OnPlayer;
		baseItem.UpdateExistingFlag = true;
	}

	/// <summary>
	/// Takes the cloth item off.
	/// TODO: Make this similar to how unequipping works.
	/// </summary>
	public void TakeOff()
	{
		PlayerController player = Game.Instance.PlayerInstance.Controller;

		player.FireWarmthIncreaseRate = player.FireWarmthIncreaseRate + fireWarmthDifference;
		player.OutsideWarmthIncreaseRate = player.OutsideWarmthIncreaseRate + outsideWarmthDifference;
		player.ShelterWarmthIncreaseRate = player.ShelterWarmthIncreaseRate + shelterWarmthDifference;

		OnPlayer = 0f;
		GetAttribute(onPlayerAttributeName).Value = OnPlayer;
		baseItem.UpdateExistingFlag = true;
	}
}