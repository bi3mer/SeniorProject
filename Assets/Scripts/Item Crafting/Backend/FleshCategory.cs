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
    private const string burntNameAddition = "Burnt ";

    private const string healthEffectAttrName = "healthEffect";
    private const string hungerGainAttrName = "hungerGain";

    private const string cookActName = "Cook";
    private const string eatActName = "Eat";

    private const float healthEffectIncreaseRate = 0.25f;
    private const float hungerGainDecreaseRate = 0.5f;
    private const float hungerGainIncreaseRate = 1.25f;

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
        category.Attributes = new List<ItemAttribute>();

        ItemAction eat = new ItemAction(eatActName, new UnityAction(category.Eat));

        category.Actions.Add(eat);

        finishDuplication(category);

        return category;
    }

    /// <summary>
    /// Readies the item category by adding the attributes and actions it can complete.
    /// </summary>
    public override void ReadyCategory()
    {
        Attributes = new List<ItemAttribute>();

        Attributes.Add(new ItemAttribute(hungerGainAttrName, HungerGain));
        Attributes.Add(new ItemAttribute(healthEffectAttrName, HealthEffect));

        Actions = new List<ItemAction>();

        ItemAction eat = new ItemAction(eatActName, new UnityAction(Eat));
        Actions.Add(eat);
    }

    /// <summary>
    /// Cooks the item. Decreases health effect.
    /// </summary>
    public void Cook()
    {
    	if(baseItem.ItemName.Contains(defaultCookedNameAddition))
    	{
			string baseName = baseItem.ItemName.Replace(defaultCookedNameAddition, "");
			baseItem.ChangeName(burntNameAddition + baseName);
    		baseItem.Types.Remove(ItemTypes.Edible);
    		baseItem.Types.Add(ItemTypes.Fuel);

    		FuelCategory fuelCategory = new FuelCategory();
    		fuelCategory.BurnTime = HungerGain;

    		baseItem.AddItemCategory(fuelCategory);

    		HealthEffect -= healthEffectIncreaseRate;
    		HungerGain *= hungerGainDecreaseRate;

			GetAttribute (healthEffectAttrName).Value = HealthEffect;
			GetAttribute (hungerGainAttrName).Value = HungerGain;
    	}
    	else
    	{
	        baseItem.ChangeName(defaultCookedNameAddition + baseItem.ItemName);
	        HealthEffect += healthEffectIncreaseRate;
	        HungerGain *= hungerGainIncreaseRate;
			GetAttribute (healthEffectAttrName).Value = HealthEffect;
			GetAttribute (hungerGainAttrName).Value = HungerGain;
	    }

        baseItem.DirtyFlag = true;
    }

    /// <summary>
    /// Player eats item. If there is a health effect, the player will get food poisoning.
    /// </summary>
    public void Eat()
    {
    	Player player = baseItem.GetMockPlayer();
    	int currentAmount = 1;

    	if(player == null)
    	{
    		player = Game.Player;
			currentAmount = GuiInstanceManager.ItemAmountPanelInstance.CurrentAmount;
    	}

        for (int i = 0; i < currentAmount; ++i)
        {
            // If this is a bad food
            if (HealthEffect < 0)
            {
                // Health effects don't stack
                if (player.HealthStatus == PlayerHealthStatus.None)
                {
                    // Random chance of getting food poisoning
                    if (baseItem.GetMockPlayer() != null || RandomUtility.RandomPercent <= Player.FoodPoisoningChance)
                    {
                        player.HealthStatus = PlayerHealthStatus.FoodPoisoning;
                    }
                }
            }

            if(baseItem.GetMockPlayer() == null)
            {
            	Game.Player.Controller.PlayerStatManager.HungerRate.UseFoodEnergy((int)HungerGain);
            }
            else
            {
            	player.Hunger += ((int)(HungerGain));
            }
        }

       	baseItem.RemovalFlag = true;
    }
}
