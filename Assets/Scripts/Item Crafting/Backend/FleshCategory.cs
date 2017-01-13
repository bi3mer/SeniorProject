using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class FleshCategory : ItemCategory
{
    /// <summary>
    /// Gets or sets the toughness.
    /// </summary>
    /// <value>The stomach effect.</value>
    public float Toughness
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the stomach effect.
    /// </summary>
    /// <value>The stomach effect.</value>
    public float StomachEffect
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the hunger gain.
    /// </summary>
    /// <value>The stomach effect.</value>
    public int HungerGain
    {
        get;
        set;
    }

    /// <summary>
    /// string added to item name when cooked
    /// </summary>
    private const string defaultCookedNameAddition = "Cooked";

    private const string toughAttrName = "toughness";
    private const string hungerGainAttrName = "hungerGain";

    private const string cookActName = "Cook";
    private const string eatActName = "Eat";

    private const float stomachEffectDecreaseRate = 0.25f;

    /// <summary>
    /// Creates a copy of this flesh category.
    /// </summary>
    /// <returns>The duplicate.</returns>
    public override ItemCategory GetDuplicate()
    {
        FleshCategory category = new FleshCategory();
        category.StomachEffect = StomachEffect;
        category.HungerGain = HungerGain;

        category.Actions = new List<ItemAction>();
        category.Attributes = new List<Attribute>();

        ItemAction cook = new ItemAction(cookActName, new UnityAction(category.Cook));
        ItemAction eat = new ItemAction(eatActName, new UnityAction(category.Eat));

        category.Actions.Add(cook);
        category.Actions.Add(eat);

        finishDuplication(category);

        return category;
    }

    /// <summary>
    /// Readies the item category by adding the attributes and actions it can complete.
    /// </summary>
    public override void ReadyCategory()
    {
        Attributes = new List<Attribute>();

        Attributes.Add(new Attribute(hungerGainAttrName, HungerGain));
        Attributes.Add(new Attribute(toughAttrName, Toughness));

        Actions = new List<ItemAction>();

        ItemAction cook = new ItemAction(cookActName, new UnityAction(Cook));
        ItemAction eat = new ItemAction(eatActName, new UnityAction(Eat));

        Actions.Add(cook);
        Actions.Add(eat);
    }

    /// <summary>
    /// Cooks the item. Lowers toughness and increases water content. If the item 
    ///has high enough water content, it becomes soup. Otherwise, it becomes Cooked X.
    /// </summary>
    public void Cook()
    {
        //Toughness = Toughness * cookingWaterIncreaseRate;

        GetAttribute(toughAttrName).Value = Toughness;

        SetActionComplete(cookActName);
    }

    public void Eat()
    {
        if (StomachEffect < 0)
        {
            // TODO: stomach illness code
        }

        Game.Instance.PlayerInstance.Hunger += HungerGain;
        baseItem.RemovalFlag = true;
    }
}
