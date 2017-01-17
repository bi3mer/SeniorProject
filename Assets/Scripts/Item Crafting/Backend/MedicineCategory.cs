using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class MedicineCategory : ItemCategory
{
    /// <summary>
    /// Gets or sets the spiciness.
    /// </summary>
    /// <value>The spicy.</value>
    public int HealthGain
    {
        get;
        set;
    }

    private const string healthGainAttrName = "hungerGain";

    private const string healActName = "Heal";
    private const string curePneumoniaActName = "Cure Pneumonia";
    private const string cureFoodPoisoningActName = "Cure Food Poisoning";

    /// <summary>
    /// Creates a copy of this medicine category.
    /// </summary>
    /// <returns>The duplicate.</returns>
    public override ItemCategory GetDuplicate()
    {
        MedicineCategory category = new MedicineCategory();
        category.HealthGain = HealthGain;

        category.Actions = new List<ItemAction>();
        category.Attributes = new List<Attribute>();

        ItemAction heal = new ItemAction(healActName, new UnityAction(category.Heal));
        ItemAction cureFoodPoisoning = new ItemAction(cureFoodPoisoningActName, new UnityAction(category.CureFoodPoisoning));
        ItemAction curePneumonia = new ItemAction(curePneumoniaActName, new UnityAction(category.CurePneumonia));

        category.Actions.Add(heal);
        category.Actions.Add(cureFoodPoisoning);
        category.Actions.Add(curePneumonia);

        finishDuplication(category);

        return category;
    }

    /// <summary>
    /// Readies the item category by adding the attributes and actions it can complete.
    /// </summary>
    public override void ReadyCategory()
    {
        Attributes = new List<Attribute>();

        Attributes.Add(new Attribute(healthGainAttrName, HealthGain));

        Actions = new List<ItemAction>();

        ItemAction heal = new ItemAction(healActName, new UnityAction(Heal));
        ItemAction cureFoodPoisoning = new ItemAction(cureFoodPoisoningActName, new UnityAction(CureFoodPoisoning));
        ItemAction curePneumonia = new ItemAction(curePneumoniaActName, new UnityAction(CurePneumonia));

        Actions.Add(heal);
        Actions.Add(cureFoodPoisoning);
        Actions.Add(curePneumonia);
    }

    /// <summary>
    /// Heals the player based on the HealthRegain.
    /// </summary>
    public void Heal()
    {
        Game.Instance.PlayerInstance.Health += HealthGain;
        baseItem.RemovalFlag = true;

        SetActionComplete(healActName);
    }

    /// <summary>
    /// Cures the player of pneumonia.
    /// </summary>
    public void CurePneumonia()
    {
        if (Game.Instance.PlayerInstance.HealthStatus.Contains(HealthStatuses.Pneumonia))
        {
            Game.Instance.PlayerInstance.HealthStatus.Remove(HealthStatuses.Pneumonia);
        }
        SetActionComplete(curePneumoniaActName);
    }

    /// <summary>
    /// Cures the player of food poisoning.
    /// </summary>
    public void CureFoodPoisoning()
    {
        if (Game.Instance.PlayerInstance.HealthStatus.Contains(HealthStatuses.FoodPoisoning))
        {
            Game.Instance.PlayerInstance.HealthStatus.Remove(HealthStatuses.FoodPoisoning);
        }
        SetActionComplete(cureFoodPoisoningActName);
    }
}
