using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class FishingRodCategory : EquipableCategory
{
	public override ItemCategory GetDuplicate()
	{
		FishingRodCategory category = new FishingRodCategory();
		return category;
	}

	public override void ReadyCategory()
	{
		toolType = ToolTypes.FishingRod;
		ItemAction equip = new ItemAction (equipActionName, new UnityAction(Equip));
		Actions.Add(equip);
	}
}
