using UnityEngine;

public class Player
{
    private const int maxHealth = 100;
    private const int maxWarmth = 100;
    private const int maxHunger = 100;

    /// <summary>
    /// Player constructor.
    /// Initializes all fields to full.
    /// </summary>
    public Player ()
    {
        Health = MaxHealth = maxHealth;
        Warmth = MaxWarmth = maxWarmth;
        Hunger = MaxHunger = maxHunger;
    }

    /// <summary>
    /// The player's current health. 
    /// Used for physical dammage.
    /// </summary>
    public int Health
    {
        get;
        set;
    }

    /// <summary>
    /// The player's maximum health. 
    /// Used for physical dammmage.
    /// </summary>
    public int MaxHealth
    {
        get;
        private set;
    }

    /// <summary>
    /// The player's current warmth. 
    /// Warmth is reduced when the player is exposed to cold.
    /// </summary>
    public int Warmth
    {
        get;
        set;
    }

    /// <summary>
    /// The player's maximum warmth. 
    /// Warmth is reduced when the player is exposed to cold.
    /// </summary>
    public int MaxWarmth
    {
        get;
        private set;
    }

    /// <summary>
    /// The player's current hunger.
    /// Hunger is reduced when the player does not eat.
    /// </summary>
    public int Hunger
    {
        get;
        set;
    }

    /// <summary>
    /// The player's maximum hunger.
    /// Hunger is reduced when the player does not eat.
    /// </summary>
    public int MaxHunger
    {
        get;
        private set;
    }
}
