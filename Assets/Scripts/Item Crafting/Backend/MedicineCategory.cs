using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class MedicineCategory : ItemCategory
{
    /// <summary>
    /// Gets or sets the HealthGain.
    /// </summary>
    /// <value>The health gain.</value>
    public float HealthGain
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the Sickness specialization (pnuemonia, food poisoning, none, all).
    /// </summary>
    public string Sickness
    {
        get;
        set;
    }

    private const string healthGainAttrName = "healthGain";
    private const string sicknessAttrName = "sickness";

    private const string healActName = "Heal";
    private const string curePneumoniaActName = "Cure Pneumonia";
    private const string cureFoodPoisoningActName = "Cure Food Poisoning";
    private const string cureAllActName = "Cure All";

    /// <summary>
    /// Creates a copy of this medicine category.
    /// </summary>
    /// <returns>The duplicate.</returns>
    public override ItemCategory GetDuplicate()
    {
        MedicineCategory category = new MedicineCategory();
        category.HealthGain = HealthGain;

        category.Actions = new List<ItemAction>();
        category.Attributes = new List<ItemAttribute>();

        if (Sickness == "food poisoning")
        {
            ItemAction cureFoodPoisoning = new ItemAction(cureFoodPoisoningActName, new UnityAction(category.CureFoodPoisoning));
            category.Actions.Add(cureFoodPoisoning);
        }

        else if (Sickness == "pneumonia")
        {
            ItemAction curePneumonia = new ItemAction(curePneumoniaActName, new UnityAction(category.CurePneumonia));
            category.Actions.Add(curePneumonia);
        }

        else if (Sickness == "all")
        {
            ItemAction cureAll = new ItemAction(cureAllActName, new UnityAction(category.CureAll));
            category.Actions.Add(cureAll);
        }

        else
        {
            ItemAction heal = new ItemAction(healActName, new UnityAction(category.Heal));
            category.Actions.Add(heal);
        }

        finishDuplication(category);

        return category;
    }

    /// <summary>
    /// Readies the item category by adding the attributes and actions it can complete.
    /// </summary>
    public override void ReadyCategory()
    {
        Attributes = new List<ItemAttribute>();

        Attributes.Add(new ItemAttribute(healthGainAttrName, HealthGain));

        Actions = new List<ItemAction>();

        if (Sickness == "food poisoning")
        {
            ItemAction cureFoodPoisoning = new ItemAction(cureFoodPoisoningActName, new UnityAction(CureFoodPoisoning));
            Actions.Add(cureFoodPoisoning);
        }

        else if (Sickness == "pneumonia")
        {
            ItemAction curePneumonia = new ItemAction(curePneumoniaActName, new UnityAction(CurePneumonia));
            Actions.Add(curePneumonia);
        }

        else if (Sickness == "all")
        {
            ItemAction cureAll = new ItemAction(cureAllActName, new UnityAction(CureAll));
            Actions.Add(cureAll);
        }

        else
        {
            ItemAction heal = new ItemAction(healActName, new UnityAction(Heal));
            Actions.Add(heal);
        }
    }

    /// <summary>
    /// Heals the player based on the HealthRegain.
    /// </summary>
    public void Heal()
    {
		Game.Player.Controller.PlayerStatManager.HealthRate.UseHealthEnergy((int) HealthGain);
        baseItem.RemovalFlag = true;
    }

    /// <summary>
    /// Cures the player of pneumonia.
    /// </summary>
    public void CurePneumonia()
    {
        if (Game.Player.HealthStatus == PlayerHealthStatus.Pneumonia)
        {
            Game.Player.HealthStatus = PlayerHealthStatus.None;
        }

        baseItem.RemovalFlag = true;
    }

    /// <summary>
    /// Cures the player of food poisoning.
    /// </summary>
    public void CureFoodPoisoning()
    {
        if (Game.Player.HealthStatus == PlayerHealthStatus.FoodPoisoning)
        {
            Game.Player.HealthStatus = PlayerHealthStatus.None;
        }

        baseItem.RemovalFlag = true;
    }

    /// <summary>
    /// Cures all illness and maxes health.
    /// </summary>
    public void CureAll()
    {
        Game.Player.HealthStatus = PlayerHealthStatus.None;

        Game.Player.Health = Game.Player.MaxHealth;
        baseItem.RemovalFlag = true;
    }
}
