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
	/// Gets or sets the player controller if we are in a scene with the player controller.
	/// </summary>
	/// <value>The controller</value>
	public PlayerController Controller 
	{
		get;
		set;
	}

    /// <summary>
    /// Returns true if the player is instantiated in the game scene.
    /// </summary>
    public bool IsInWorld
    {
        get
        {
            return (WorldTransform != null);
        }
    }

    /// <summary>
    /// Player's transform in the game world.
    /// Returns null if the player is not in the scene, check IsInWorld before using the value directly.
    /// </summary>
    public Transform WorldTransform
    {
        get;
        set;
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
