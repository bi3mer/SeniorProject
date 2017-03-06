using UnityEngine; 
using System.Collections.Generic;
using UnityEngine.Events;

public class RaftCategory : ItemCategory
{
    /// <summary>
    /// Gets or sets the speed of the item.
    /// </summary>
    /// <value>The content of the water.</value>
    public float Speed
    {
        get;
        set;
    }

    private float byWater;

    private const string byWaterAttrName = "by water";
    private const string speedAttrName = "speed";
    private string setDownActName = "Set Down";

    private const float isByWaterThreshold = 1f;

    /// <summary>
    /// Creates a copy of this raft category.
    /// </summary>
    /// <returns>The duplicate.</returns>
    public override ItemCategory GetDuplicate()
    {
        RaftCategory category = new RaftCategory();

        category.Speed = Speed;

        category.Actions = new List<ItemAction>();
        category.Attributes = new List<Attribute>();

        ItemAction setDown = new ItemAction(setDownActName, new UnityAction(category.SetDown));

        finishDuplication(category);

        return category;
    }

    /// <summary>
    /// Readies the item category by adding the attributes and actions it can complete.
    /// </summary>
    public override void ReadyCategory()
    {
        Attributes = new List<Attribute>();

        Attributes.Add(new Attribute(speedAttrName, Speed));

        Actions = new List<ItemAction>();

        if (Game.Instance.PlayerInstance.Controller.IsWaterInView)
        {
            byWater = 1f;
        }
        else
        {
            byWater = 0;
        }

        ItemAction setDown = new ItemAction(setDownActName, new UnityAction(SetDown));
        ItemCondition setDownCondition = new ItemCondition(byWaterAttrName, isByWaterThreshold, new BooleanOperator.BooleanOperatorDelegate(BooleanOperator.Equal));

        setDown.Conditions.Add(setDownCondition);

        Actions.Add(setDown);
    }

    /// <summary>
    /// Sets down the raft in the world. Drops it where the player stands.
    /// </summary>
    public void SetDown()
    {
        // create the object with the model
        GameObject item = Game.Instance.WorldItemFactoryInstance.CreateGenericInteractableItem(baseItem, 1);
        RaftInteractable raft = item.AddComponent<RaftInteractable>();

        raft.Raft = this;
        raft.SetUp();

        item.name = baseItem.ItemName;

        // place the raft away from character
        item.transform.position = new Vector3(Game.Instance.PlayerInstance.WorldTransform.position.x, Game.Instance.PlayerInstance.WorldTransform.position.y, Game.Instance.PlayerInstance.WorldTransform.position.z + 3);
        Game.Instance.ItemPoolInstance.AddItemFromWorld(item);

        SetActionComplete(setDownActName);

        baseItem.RemovalFlag = true;
    }

    /// <summary>
    /// Finds the closest place to place raft in water.
    /// </summary>
    /// <returns></returns>
    public Vector3 FindClosestWater()
    {
        // TO DO: add a way to detect water.
        Vector3 raftPosition = new Vector3(Game.Instance.PlayerInstance.WorldTransform.position.x, Game.Instance.PlayerInstance.WorldTransform.position.y, Game.Instance.PlayerInstance.WorldTransform.position.z); 

        return raftPosition;
    }
}
