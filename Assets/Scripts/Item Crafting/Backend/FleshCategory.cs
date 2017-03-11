using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class FleshCategory : ItemCategory
{
    /// <summary>
    /// Gets or sets the health effect.
    /// </summary>
    /// <value>The health effect.</value>
    public float HealthEffect
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the hunger gain.
    /// </summary>
    /// <value>The hunger gain.</value>
    public float HungerGain
    {
        get;
        set;
    }

    /// <summary>
    /// string added to item name when cooked
    /// </summary>
    private const string defaultCookedNameAddition = "Cooked ";
    private const string defaultEmptyNameAddition = "Empty ";

    private const string healthEffectAttrName = "healthEffect";
    private const string hungerGainAttrName = "hungerGain";

    private const string cookActName = "Cook";
    private const string eatActName = "Eat";

    private const float healthEffectDecreaseRate = 0.25f;

    /// <summary>
    /// Creates a copy of this flesh category.
    /// </summary>
    /// <returns>The duplicate.</returns>
    public override ItemCategory GetDuplicate()
    {
        FleshCategory category = new FleshCategory();
        category.HealthEffect = HealthEffect;
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
        Attributes.Add(new Attribute(healthEffectAttrName, HealthEffect));

        Actions = new List<ItemAction>();

        ItemAction cook = new ItemAction(cookActName, new UnityAction(Cook));
        ItemAction eat = new ItemAction(eatActName, new UnityAction(Eat));

        Actions.Add(cook);
        Actions.Add(eat);
    }

    /// <summary>
    /// Cooks the item. Decreases health effect.
    /// </summary>
    public void Cook()
    {
        baseItem.ChangeName(defaultCookedNameAddition + baseItem.ItemName);
        HealthEffect -= healthEffectDecreaseRate;

        baseItem.DirtyFlag = true;
        SetActionComplete(cookActName);
    }

    /// <summary>
    /// Player eats item. If there is a health effect, the player will get food poisoning.
    /// </summary>
    public void Eat()
    {
        // If this is a bad food
        if (HealthEffect < 0)
        {
            // Health effects don't stack
            if (Game.Instance.PlayerInstance.HealthStatus == PlayerHealthStatus.None)
            {
                // Random chance of getting food poisoning
                if (RandomUtility.RandomPercent <= Player.FoodPoisoningChance)
                {
                    Game.Instance.PlayerInstance.HealthStatus = PlayerHealthStatus.FoodPoisoning;
                }
            }
        }

        Game.Instance.PlayerInstance.Hunger += (int)HungerGain;

        if (baseItem.ModifyingActionNames.IndexOf(eatActName) > -1)
        {
            baseItem.ChangeName(defaultEmptyNameAddition + baseItem.ItemName);
        }
        else
        {
            baseItem.RemovalFlag = true;
        }
    }
}
