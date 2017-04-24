using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;


public class ShelterCategory : ItemCategory
{
    /// <summary>
    /// Gets and sets the warmth rate.
    /// </summary>
    public int WarmthRate
    {
        get;
        set;
    }

    private const string warmthRateAttrName = "warmthRate";

    private const string setDownActName = "Set Down";

    /// <summary>
    /// Creates a copy of this shelter category.
    /// </summary>
    /// <returns>The duplicate.</returns>
    public override ItemCategory GetDuplicate()
    {
        ShelterCategory category = new ShelterCategory();

        category.WarmthRate = WarmthRate;
        category.Attributes = new List<ItemAttribute>();
        category.Actions = new List<ItemAction>();

        ItemAction setDown = new ItemAction(setDownActName, new UnityAction(category.SetDown));

        category.Actions.Add(setDown);

        finishDuplication(category);

        return category;
    }

    /// <summary>
    /// Readies the item category by adding the attributes and actions it can complete.
    /// </summary>
    public override void ReadyCategory()
    {
        Attributes = new List<ItemAttribute>();
        Attributes.Add(new ItemAttribute(warmthRateAttrName, WarmthRate));

        Actions = new List<ItemAction>();
        Actions.Add(new ItemAction(setDownActName, new UnityAction(SetDown)));
    }

    /// <summary>
    /// Sets down the shelter in the world. Drops it where the player stands.
    /// </summary>
    public void SetDown()
    {
        // create the object with the model
        GameObject item = Game.Instance.WorldItemFactoryInstance.CreateGenericInteractableItem(baseItem);
        ShelterInteractable shelter = item.AddComponent<ShelterInteractable>();

        shelter.SetUp();
        shelter.Shelter = this;

        item.name = baseItem.ItemName;
        item.transform.position = Game.Instance.PlayerInstance.WorldTransform.position;
        Game.Instance.ItemPoolInstance.AddItemFromWorld(item);

        SetActionComplete(setDownActName);

        baseItem.RemovalFlag = true;
    }
}
